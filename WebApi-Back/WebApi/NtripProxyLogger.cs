using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi
{
    public static class NtripProxyLogger
    {
        private static Logger fileLogger = LogManager.GetLogger("file");
        private static Logger dbLogger = LogManager.GetLogger("database");

        /// <summary>
        /// 异常信息记录到日志文件
        /// </summary>
        /// <param name="message">异常消息</param>
        public static void LogExceptionIntoFile(string message)
        {
            fileLogger.Error(message);
        }

        /// <summary>
        /// 将客户操作记录到数据库
        /// </summary>
        public static void LogActionIntoDatabase(LogEventInfo logEventInfo)
        {
            dbLogger.Info(logEventInfo);
        }
    }
}