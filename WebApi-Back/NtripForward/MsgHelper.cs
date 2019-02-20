using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtripForward
{
    /// <summary>
    /// 用来处理传入的消息信息类
    /// </summary>
    public static class MsgHelper
    {
        /// <summary>
        /// 判断消息的类型1为浏览器请求，2为手部首次请求，3为发送的GGA请求
        /// </summary>
        /// <param name="message">传入的请求消息</param>
        /// <returns></returns>
        public static int CheckRequestType(string message)
        {
            int result = 0;
            if (message.IndexOf("GET") >= 0 && message.IndexOf("Authorization") < 0)
            {
                result = 1;
            }
            else if (message.IndexOf("GET") >= 0 && message.IndexOf("Authorization") >= 0)
            {
                result = 2;
            }
            else if (message.IndexOf("GPGGA") >= 0)
            {
                result = 3;
            }

            return result;
        }

        /// <summary>
        /// 获得挂载点信息
        /// </summary>
        /// <param name="message">传入的请求消息</param>
        /// <returns>请求的挂载点信息</returns>
        public static string GetMountPoint(string message)
        {
            string result = string.Empty;
            string[] recArray = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var item in recArray)
            {
                if (item.IndexOf("GET") >= 0 && item.IndexOf("") >= 0)
                {
                    string[] temp = item.Split(' ');
                    result = temp[1].TrimStart('/');
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获得客户端信息
        /// </summary>
        /// <param name="message">传入的请求消息</param>
        /// <returns>请求的客户端信息</returns>
        public static string GetUserAgent(string message)
        {
            string result = string.Empty;
            string[] recArray = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var item in recArray)
            {
                if (item.IndexOf("User-Agent") >= 0 && item.IndexOf(":") >= 0)
                {
                    result = item.Split(':')[1].Trim();
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 返回授权信息明文，返回的数组0为用户名明文，1为密码明文
        /// </summary>
        /// <param name="message">传入的请求消息</param>
        /// <returns>授权明文信息，0为用户名明文，1为密码明文</returns>
        public static string[] GetAuthorization(string message)
        {
            string[] result = new string[2];
            string[] recArray = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var item in recArray)
            {
                //授权信息
                if (item.IndexOf("Authorization") >= 0)
                {
                    string base64Code = item.Substring(item.IndexOf("Basic ") + 6, item.Length - item.IndexOf("Basic ") - 6);
                    string code = Base64Helper.Base64Decode(base64Code);
                    result[0] = code.Substring(0, code.IndexOf(":"));
                    result[1] = code.Substring(code.IndexOf(":") + 1, code.Length - code.IndexOf(":") - 1);
                }
            }
            return result;
        }

        /// <summary>
        /// 替换授权信息
        /// </summary>
        /// <param name="message">传入的请求信息</param>
        /// <param name="userSYS">系统账号用户名明文</param>
        /// <param name="passwordSYS">系统账号密码明文</param>
        /// <returns>替换后的请求信息</returns>
        public static string ReplaceAuthorization(string message, string userSYS, string passwordSYS)
        {
            string result = string.Empty;
            string[] recArray = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < recArray.Length; i++)
            {
                //授权信息
                if (recArray[i].IndexOf("Authorization") >= 0)
                {
                    recArray[i] = "Authorization: Basic " + Base64Helper.Base64Encode(userSYS + ":" + passwordSYS);
                    break;
                }
            }
            result = string.Join(Environment.NewLine, recArray);
            return result;
        }

        /// <summary>
        /// gga定位模式
        /// </summary>
        /// <param name="message">gga消息</param>
        /// <returns>定位模式 0初始化， 1单点定位， 2码差分， 3无效PPS， 4固定解， 5浮点解， 6正在估算 7，人工输入固定值， 8模拟模式， 9WAAS差分</returns>
        public static int FixModule(string message)
        {
            return int.Parse(DecodeGGA(message)[6]);
        }

        /// <summary>
        /// 获取经度信息，东经为+，西经为-
        /// </summary>
        /// <param name="message">gga消息</param>
        /// <returns>经度信息</returns>
        public static double GetLng(string message)
        {
            double result = 0;
            string[] ggaArray = DecodeGGA(message);
            string lngString = ggaArray[4];
            if (ggaArray[5] == "E")
            {
                result = double.Parse(lngString.Substring(0, 3)) + double.Parse(lngString.Substring(3, lngString.Length - 3)) / 60;

            }
            else if (ggaArray[5] == "W")
            {
                result = 0 - (double.Parse(lngString.Substring(0, 3)) + double.Parse(lngString.Substring(3, lngString.Length - 3)) / 60);
            }
            return result;
        }

        /// <summary>
        /// 获取纬度信息，北纬为+，南纬为-
        /// </summary>
        /// <param name="message">gga消息</param>
        /// <returns>纬度信息</returns>
        public static double GetLat(string message)
        {
            double result = 0;
            string[] ggaArray = DecodeGGA(message);
            string latString = ggaArray[2];
            if (ggaArray[3] == "N")
            {
                result = double.Parse(latString.Substring(0, 2)) + double.Parse(latString.Substring(2, latString.Length - 2)) / 60;

            }
            else if (ggaArray[3] == "S")
            {
                result = 0 - (double.Parse(latString.Substring(0, 2)) + double.Parse(latString.Substring(2, latString.Length - 2)) / 60);
            }
            return result;
        }

        /*
        GPGGA,<1>,<2>,<3>,<4>,<5>,<6>,<7>,<8>,<9>,M,<10>,M,<11>,<12>∗xxGPGGA,<1>,<2>,<3>,<4>,<5>,<6>,<7>,<8>,<9>,M,<10>,M,<11>,<12>∗xxGPGGA：起始引导符及语句格式说明(本句为GPS定位数据)； 
        <1> UTC时间，格式为hhmmss.sss； 
        <2> 纬度，格式为ddmm.mmmm(第一位是零也将传送)； 
        <3> 纬度半球，N或S(北纬或南纬)
        <4> 经度，格式为dddmm.mmmm(第一位零也将传送)； 
        <5> 经度半球，E或W(东经或西经)
        <6> GPS状态， 0初始化， 1单点定位， 2码差分， 3无效PPS， 4固定解， 5浮点解， 6正在估算 7，人工输入固定值， 8模拟模式， 9WAAS差分
        <7> 使用卫星数量，从00到12(第一个零也将传送)
        <8> HDOP-水平精度因子，0.5到99.9，一般认为HDOP越小，质量越好。 
        <9> 椭球高，-9999.9到9999.9米
        M 指单位米
        <10> 大地水准面高度异常差值，-9999.9到9999.9米M 指单位米
        <11> 差分GPS数据期限(RTCM SC-104)，最后设立RTCM传送的秒数量，如不是差分定位则为空
        <12> 差分参考基站标号，从0000到1023(首位0也将传送)。 
        1. 语句结束标志符xx 从$开始到* 之间的所有ASCII码的异或校验
        回车符，结束标记
        换行符，结束标记
        */

        /// <summary>
        /// 分析手部发送的GGA数据
        /// </summary>
        /// <param name="ggaString">发送的GGA字符串</param>
        /// <returns>手部的拆分字符串数组</returns>
        private static string[] DecodeGGA(string ggaString)
        {
            string[] result = new string[15];
            if (ggaString.IndexOf("GPGGA") >= 0)
            {
                result = ggaString.Split(',');
            }
            return result;
        }

    }
}
