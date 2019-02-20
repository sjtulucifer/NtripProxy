using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// 概略位置实体
    /// </summary>
    public class GGAHistoryEntity
    {
        /// <summary>
        /// 概略位置ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 概略位置账号名
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 概略位置类型
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// 系统账号名
        /// </summary>
        public string AccountSYS { get; set; }
        /// <summary>
        /// 概略位置定位时间
        /// </summary>
        public DateTime FixedTime { get; set; }
        /// <summary>
        /// 概略位置经度
        /// </summary>
        public double Lng { get; set; }
        /// <summary>
        /// 概略位置纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 概略位置状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 概率位置信息
        /// </summary>
        public string GGAInfo { get; set; }
        /// <summary>
        /// 概略位置相关会话
        /// </summary>
        public SessionHistoryEntity Session { get; set; }

        /// <summary>
        /// 转换WebApi概略位置实体为dal层概略位置基础信息
        /// </summary>
        /// <returns>dal层概略位置</returns>
        public GGAHistory ToGGAHistory()
        {
            GGAHistory ggaHistory = new GGAHistory()
            {
                ID = ID,
                Account = Account,
                AccountType = AccountType,
                AccountSYS = AccountSYS,
                FixedTime = FixedTime,
                Lng = decimal.Parse(Lng.ToString()),
                Lat = decimal.Parse(Lat.ToString()),
                Status = Status,
                GGAInfo = GGAInfo
            };
            return ggaHistory;
        }
    }
}