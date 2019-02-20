using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// 会话历史实体
    /// </summary>
    public class SessionHistoryEntity
    {
        /// <summary>
        /// 会话历史实体ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 账号名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 账号类型
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// 系统账号名
        /// </summary>
        public string AccountSYSName { get; set; }
        /// <summary>
        /// 挂载点
        /// </summary>
        public string MountPoint { get; set; }
        /// <summary>
        /// 客户端类型
        /// </summary>
        public string Client { get; set; }
        /// <summary>
        /// 客户端地址
        /// </summary>
        public string ClientAddress { get; set; }
        /// <summary>
        /// 连接开始时间
        /// </summary>
        public DateTime? ConnectionStart { get; set; }
        /// <summary>
        /// 连接结束时间
        /// </summary>
        public DateTime? ConnectionEnd { get; set; }
        /// <summary>
        /// 发送GGA数量
        /// </summary>
        public int GGACount { get; set; }
        /// <summary>
        /// 定位GGA数量
        /// </summary>
        public int FixedCount { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorInfo { get; set; }
        /// <summary>
        /// 会话相关GGA列表
        /// </summary>
        public List<GGAHistoryEntity> GGAHistories { get; set; }

        /// <summary>
        /// 转换WebApi会话实体为dal层会话基础信息
        /// </summary>
        /// <returns>dal层会话</returns>
        public SessionHistory ToSessionHistory()
        {
            SessionHistory sessionHistory = new SessionHistory()
            {
                ID = ID,
                AccountName = AccountName,
                AccountType = AccountType,
                AccountSYSName = AccountSYSName,
                MountPoint = MountPoint,
                Client = Client,
                ClientAddress = ClientAddress,
                ConnectionStart = ConnectionStart,
                ConnectionEnd = ConnectionEnd,
                GGACount = GGACount,
                FixedCount = FixedCount,
                ErrorInfo = ErrorInfo                
            };
            return sessionHistory;
        }
    }
}