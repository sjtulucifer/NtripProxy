using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// 日志实体信息
    /// </summary>
    public class LogEntity
    {
        /// <summary>
        /// 日志ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 日志用户
        /// </summary>
        public Guid User { get; set; }
        /// <summary>
        /// 日志动作
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 日志模块
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 转换WebApi日志实体为dal层日志
        /// </summary>
        /// <returns>dal层日志</returns>
        public LOG ToLOG()
        {
            LOG log = new LOG()
            {
                ID = ID,
                Log_Time = Time,
                Log_User = User,
                Log_Action = Action,
                Log_Module = Module,
                Log_Message = Message,
            };
            return log;
        }
    }
}