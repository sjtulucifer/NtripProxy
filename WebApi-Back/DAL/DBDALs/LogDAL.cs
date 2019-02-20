using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtripProxy.DAL.DBDALs
{
    /// <summary>
    /// 日志数据库操作类
    /// </summary>
    public class LogDAL
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log">日志信息</param>
        /// <returns>是否添加成功</returns>
        public bool AddLog(LOG log)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                ctx.LOGs.Add(log);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 查找所有日志信息
        /// </summary>
        /// <returns>所有日志信息列表</returns>
        public List<LOG> FindAllLogs()
        {
            List<LOG> result = new List<LOG>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.LOGs.OrderByDescending(l=>l.Log_Time).ToList<LOG>();
            }
            return result;
        }
    }
}
