using NtripForward.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtripForward.DAL
{
    /// <summary>
    /// 会话数据库操作类
    /// </summary>
    class SessionHistoryDAL
    {
        /// <summary>
        /// 添加会话
        /// </summary>
        /// <param name="session">会话信息</param>
        /// <returns>是否添加成功</returns>
        public bool AddSessionHistory(SessionHistory session)
        {
            bool result = false;
            using (var ctx = new NtripForwardDB())
            {
                ctx.SessionHistories.Add(session);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 更新会话历史基础信息
        /// </summary>
        /// <param name="session">会话信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateSessionHistory(SessionHistory session)
        {
            bool result = false;
            using (var ctx = new NtripForwardDB())
            {
                if (session.ID != null)
                {
                    ctx.SessionHistories.Attach(session);
                    ctx.Entry(session).State = EntityState.Modified;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 查找所有在线会话,包含GGA数据信息
        /// </summary>
        /// <returns>所有在线会话信息列表</returns>
        public List<SessionHistory> FindAllOnlineSessions()
        {
            List<SessionHistory> result = new List<SessionHistory>();
            using (var ctx = new NtripForwardDB())
            {
                result = ctx.SessionHistories.Include("GGAHistories").Where(s => s.ConnectionEnd == null).ToList();
            }
            return result;
        }

        /// <summary>
        /// 查找指定账号名的所有会话信息
        /// </summary>
        /// <param name="account">账号名信息</param>
        /// <returns>查找到的会话信息列表,会话倒叙排列</returns>
        public List<SessionHistory> FindSessionHistoriesByAccount(string account)
        {
            List<SessionHistory> result = new List<SessionHistory>();
            using (var ctx = new NtripForwardDB())
            {
                result = ctx.SessionHistories.Where<SessionHistory>(s => s.AccountName == account && s.ConnectionEnd != null).OrderByDescending(s => s.ConnectionStart).ToList();
            }
            return result;
        }

        /// <summary>
        /// 查找所有会话信息
        /// </summary>
        /// <returns>所有会话信息列表</returns>
        public List<SessionHistory> FindAllSessionHistories()
        {
            List<SessionHistory> result = new List<SessionHistory>();
            using (var ctx = new NtripForwardDB())
            {
                result = ctx.SessionHistories.ToList();
            }
            return result;
        }
    }
}
