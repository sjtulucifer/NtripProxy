using NtripForward.ConfigNode;
using NtripForward.DAL;
using NtripForward.DataModel;
using NtripForward.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NtripForward
{
    public partial class NtripForward8003 : ServiceBase
    {
        private ProxySetting settings = ConfigurationManager.GetSection("ntripProxySetting8003") as ProxySetting;
        private int LOCAL_PORT;
        private int ROMOTE_PORT;
        private string qianxunIP;

        //tcp服务器
        TcpListener listener = null;

        //账户数据库操作类
        AccountDAL accountDAL = new AccountDAL();
        //系统账户数据库操作类
        AccountSYSDAL accountSYSDAL = new AccountSYSDAL();
        //会话状态数据库操作类
        SessionHistoryDAL sessionDAL = new SessionHistoryDAL();
        //GGA数据库操作类
        GGAHistoryDAL ggaDAL = new GGAHistoryDAL();

        public NtripForward8003()
        {
            InitializeComponent();
            
            LOCAL_PORT = settings.LocalPort;
            ROMOTE_PORT = settings.RemotePort;
            qianxunIP = settings.RemoteIP;
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            //创建监听
            listener = new TcpListener(IPAddress.Any, LOCAL_PORT);
            listener.Start();
            //异步接收客户端
            listener.BeginAcceptTcpClient(new AsyncCallback(acceptCallback), listener);
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。

            //重置所有账号为不在线
            List<ACCOUNT> accountList = this.accountDAL.FindAccountOnline();

            foreach (var account in accountList)
            {
                account.Account_IsOnline = false;
                this.accountDAL.UpdateAccount(account);
            }
            //重置所有系统账号为不在线
            List<ACCOUNTSYS> accountSYSList = this.accountSYSDAL.FindAllAccountSYSOnline();
            foreach (var accountSYS in accountSYSList)
            {
                accountSYS.AccountSYS_IsOnline = false;
                accountSYSDAL.UpdateAccountSYS(accountSYS);
            }
            //关闭所有会话
            List<SessionHistory> sessionList = this.sessionDAL.FindAllOnlineSessions();
            foreach (var session in sessionList)
            {
                session.ConnectionEnd = DateTime.Now;
                session.ErrorInfo = "系统重启";
                sessionDAL.UpdateSessionHistory(session);
            }

            listener.Stop();
        }

        private void acceptCallback(IAsyncResult ar)
        {
            TcpListener lstn = (TcpListener)ar.AsyncState;
            TcpClient client = lstn.EndAcceptTcpClient(ar);
            NetworkStream stream = client.GetStream();

            Task.Run(() =>
            {
                TcpClient qianxunClient = new TcpClient();
                qianxunClient.Connect(IPAddress.Parse(qianxunIP), ROMOTE_PORT);
                //中继缓存数据
                string bufferMsg = "";
                //主机地址
                string host = client.Client.RemoteEndPoint.ToString();
                //拨号的账户
                ACCOUNT account = new ACCOUNT();
                //使用的千寻账户
                ACCOUNTSYS accountSYS = new ACCOUNTSYS();
                //会话状态,并初始化
                SessionHistory session = new SessionHistory();
                session.ID = Guid.NewGuid();
                session.GGACount = 0;
                session.FixedCount = 0;
                //GGA记录频率
                int GGARecordRate = settings.GGARecordRate;
                //GGA初始计数
                int GGACounter = 1;
                
                //是否继续维持连接
                bool isLoop = true;

                //设置定时器，作为gga发送的中继
                System.Timers.Timer timerRepeat = new System.Timers.Timer();
                timerRepeat.Interval = settings.GGARepeatRate;
                timerRepeat.AutoReset = true;
                timerRepeat.Elapsed += delegate
                {
                    if (MsgHelper.CheckRequestType(bufferMsg) == 3)
                    {
                        byte[] sendMsg = Connect(qianxunClient, bufferMsg);
                        //将千寻返回数据转发给客户端
                        stream.Write(sendMsg, 0, sendMsg.Length);
                    }
                };
                timerRepeat.Start();

                //设置定时器，如果长时间不发送数据，则关掉tcp连接
                System.Timers.Timer timerTimeout = new System.Timers.Timer();
                timerTimeout.Interval = settings.Timeout;
                timerTimeout.AutoReset = false;
                timerTimeout.Elapsed += delegate
                {
                    //记录session
                    session.ConnectionEnd = DateTime.Now;
                    if (session.AccountType != null)
                    {
                        sessionDAL.UpdateSessionHistory(session);
                    }
                    //更新在线状态
                    if (session.AccountType == "Normal")
                    {
                        account.Account_IsOnline = false;
                        accountSYS.AccountSYS_IsOnline = false;
                        accountDAL.UpdateAccount(account);
                        accountSYSDAL.UpdateAccountSYS(accountSYS);
                    }
                    //关闭计时器
                    timerTimeout.Stop();
                    timerRepeat.Stop();
                    //关闭所有连接
                    stream.Close();
                    client.Close();
                    qianxunClient.Close();
                    //跳出循环
                    isLoop = false;
                };
                timerTimeout.Start();

                #region 同步读取

                while (isLoop)
                {
                    //解决cpu100%占用问题
                    Thread.Sleep(1);
                    try
                    {
                        if (stream.DataAvailable)
                        {
                            //计时器重置
                            timerTimeout.Stop();
                            timerTimeout.Start();

                            //用来存储网络字节流数据
                            byte[] buffer = new byte[1024];
                            stream.Read(buffer, 0, buffer.Length);
                            string recMsg = Encoding.UTF8.GetString(buffer);
                            byte[] sendMsg = new byte[1024];

                            switch (MsgHelper.CheckRequestType(recMsg))
                            {
                                //浏览器请求
                                case 1:
                                    sendMsg = Connect(qianxunClient, recMsg);
                                    break;
                                //手部首次请求
                                case 2:
                                    string[] namePassword = MsgHelper.GetAuthorization(recMsg);
                                    account = accountDAL.FindAccountByName(namePassword[0]);
                                    //第三方账号
                                    if (account == null)
                                    {
                                        session.AccountType = "Third";
                                        session.AccountName = namePassword[0];
                                        session.MountPoint = MsgHelper.GetMountPoint(recMsg);
                                        session.ConnectionStart = DateTime.Now;
                                        session.Client = MsgHelper.GetUserAgent(recMsg);
                                        session.ClientAddress = host;
                                        session.ErrorInfo = string.Join(":", MsgHelper.GetAuthorization(recMsg));
                                        sessionDAL.AddSessionHistory(session);
                                        recMsg = recMsg + Environment.NewLine;
                                        sendMsg = Connect(qianxunClient, recMsg);
                                    }
                                    else
                                    {
                                        if (account.Account_Password == namePassword[1])
                                        {
                                            //账户已过期
                                            if (account.Account_Expire < DateTime.Now)
                                            {
                                                session.AccountType = "Expire";
                                                session.AccountName = namePassword[0];
                                                session.MountPoint = MsgHelper.GetMountPoint(recMsg);
                                                session.ConnectionStart = DateTime.Now;
                                                session.Client = MsgHelper.GetUserAgent(recMsg);
                                                session.ClientAddress = host;
                                                session.ErrorInfo = "账户已过期";
                                                sessionDAL.AddSessionHistory(session);
                                                //sendMsg = Connect(qianxunClient, recMsg);
                                            }
                                            else
                                            {
                                                //账户已在线
                                                if ((bool)account.Account_IsOnline)
                                                {
                                                    session.AccountType = "Online";
                                                    session.AccountName = namePassword[0];
                                                    session.MountPoint = MsgHelper.GetMountPoint(recMsg);
                                                    session.ConnectionStart = DateTime.Now;
                                                    session.Client = MsgHelper.GetUserAgent(recMsg);
                                                    session.ClientAddress = host;
                                                    session.ErrorInfo = "账户已在线";
                                                    sessionDAL.AddSessionHistory(session);
                                                    //sendMsg = Connect(qianxunClient, recMsg);
                                                }
                                                else
                                                {
                                                    //账户被锁定
                                                    if ((bool)account.Account_IsLocked)
                                                    {
                                                        session.AccountType = "Locked";
                                                        session.AccountName = namePassword[0];
                                                        session.MountPoint = MsgHelper.GetMountPoint(recMsg);
                                                        session.ConnectionStart = DateTime.Now;
                                                        session.Client = MsgHelper.GetUserAgent(recMsg);
                                                        session.ClientAddress = host;
                                                        session.ErrorInfo = "账户已锁定";
                                                        sessionDAL.AddSessionHistory(session);
                                                        //sendMsg = Connect(qianxunClient, recMsg);
                                                    }
                                                    else
                                                    {
                                                        //账户正常
                                                        session.AccountType = "Normal";
                                                        //更新账户状态
                                                        account.Account_IsOnline = true;
                                                        account.Account_LastLogin = DateTime.Now;
                                                        accountDAL.UpdateAccount(account);
                                                        //找到可替换的系统账号并更新状态
                                                        accountSYS = accountSYSDAL.FindSuitableAccountSYS();
                                                        accountSYS.AccountSYS_IsOnline = true;
                                                        accountSYS.AccountSYS_LastLogin = DateTime.Now;
                                                        accountSYS.AccountSYS_Age++;
                                                        accountSYSDAL.UpdateAccountSYS(accountSYS);
                                                        //更新session状态
                                                        session.AccountName = account.Account_Name;
                                                        session.AccountSYSName = accountSYS.AccountSYS_Name;
                                                        session.ConnectionStart = DateTime.Now;
                                                        session.Client = MsgHelper.GetUserAgent(recMsg);
                                                        session.MountPoint = MsgHelper.GetMountPoint(recMsg);
                                                        session.ClientAddress = host;
                                                        sessionDAL.AddSessionHistory(session);
                                                        //替换信息
                                                        string replaceMessage = MsgHelper.ReplaceAuthorization(recMsg, accountSYS.AccountSYS_Name, accountSYS.AccountSYS_Password) + Environment.NewLine;
                                                        sendMsg = Connect(qianxunClient, replaceMessage);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //用户名密码错误
                                            session.AccountType = "PasswordError";
                                            session.AccountName = namePassword[0];
                                            session.MountPoint = MsgHelper.GetMountPoint(recMsg);
                                            session.ConnectionStart = DateTime.Now;
                                            session.Client = MsgHelper.GetUserAgent(recMsg);
                                            session.ClientAddress = host;
                                            session.ErrorInfo = "用户名密码错误";
                                            account.Account_PasswordOvertime = DateTime.Now;
                                            account.Account_PasswordOvercount++;
                                            sessionDAL.AddSessionHistory(session);
                                            sendMsg = Connect(qianxunClient, recMsg);
                                        }
                                    }
                                    break;
                                //GGA数据采集
                                case 3:
                                    //发送的GGA数据递增
                                    session.GGACount++;
                                    //是否固定解
                                    if (MsgHelper.FixModule(recMsg) == 4)
                                    {
                                        session.FixedCount++;
                                    }
                                    //更新session信息
                                    sessionDAL.UpdateSessionHistory(session);
                                    //中继缓冲gga信息
                                    bufferMsg = recMsg;
                                    if (
                                    //是否记录GGA数据
                                    settings.EnableLogGGA
                                    //到达指定评率记录GGA数据
                                    && GGACounter % GGARecordRate == 0
                                    //第三方账户不记录GGA数据
                                    && account != null
                                    )
                                    {
                                        GGAHistory gga = new GGAHistory();
                                        gga.ID = Guid.NewGuid();
                                        gga.Account = account.Account_Name;
                                        gga.AccountSYS = accountSYS.AccountSYS_Name;
                                        gga.AccountType = "Normal";
                                        gga.FixedTime = DateTime.Now;
                                        gga.Lng = Convert.ToDecimal(MsgHelper.GetLng(recMsg));
                                        gga.Lat = Convert.ToDecimal(MsgHelper.GetLat(recMsg));
                                        gga.Status = MsgHelper.FixModule(recMsg);
                                        gga.GGAInfo = recMsg;
                                        gga.SessionID = session.ID;
                                        ggaDAL.AddGGAHistory(gga);
                                    }
                                    GGACounter++;
                                    sendMsg = Connect(qianxunClient, recMsg);
                                    break;
                                default:
                                    break;
                            }

                            //将千寻返回数据转发给客户端
                            stream.Write(sendMsg, 0, sendMsg.Length);
                        }
                    }
                    catch (Exception e)
                    {
                        //更新在线状态
                        if (session.AccountType == "Normal")
                        {
                            account.Account_IsOnline = false;
                            accountSYS.AccountSYS_IsOnline = false;
                            accountDAL.UpdateAccount(account);
                            accountSYSDAL.UpdateAccountSYS(accountSYS);
                        }
                        //结束会话
                        session.ConnectionEnd = DateTime.Now;
                        //session.ErrorInfo = e.Message;
                        sessionDAL.UpdateSessionHistory(session);
                        //关闭所有连接
                        stream.Close();
                        client.Close();
                        qianxunClient.Close();
                        timerTimeout.Stop();
                        timerRepeat.Stop();
                        break;
                    }
                }
                #endregion
            });

            lstn.BeginAcceptTcpClient(new AsyncCallback(acceptCallback), lstn);
        }

        /// <summary>
        /// 转发千寻服务器并拿到返回数据
        /// </summary>
        /// <param name="client">千寻服务器客户端</param>
        /// <param name="message">转发的消息</param>
        /// <returns>千寻返回的数据</returns>
        private byte[] Connect(TcpClient client, string message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                // Translate the passed message into UTF8 and store it as a Byte array.
                byte[] sendData = Encoding.UTF8.GetBytes(message + Environment.NewLine);  //他妈的这个回车符调了老子三天时间
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(sendData, 0, sendData.Length);

                string sendMessage = string.Format("发送到千寻服务器的数据:" + Environment.NewLine + "{0}, 时间点为:{1}" + Environment.NewLine, message, DateTime.Now);

                // Buffer to store the response bytes.
                Byte[] responseData = new Byte[2048];

                // String to store the response ASCII representation.
                String responseMessage = String.Empty;

                // Read the TcpServer response bytes.
                int bytes = stream.Read(responseData, 0, responseData.Length);
                responseMessage = string.Format("从千寻服务器接收到的数据:" + Environment.NewLine + "{0},时间点为:{1}" + Environment.NewLine, Encoding.UTF8.GetString(responseData, 0, bytes), DateTime.Now);

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                return Encoding.UTF8.GetBytes(string.Format("ArgumentNullException: {0}", e));
            }
            catch (SocketException e)
            {
                return Encoding.UTF8.GetBytes(string.Format("SocketException: {0}", e));
            }
        }
    }
}
