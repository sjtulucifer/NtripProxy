using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtripProxy.DAL.DBDALs
{
    /// <summary>
    /// 概略位置数据库操作类
    /// </summary>
    public class GGAHistoryDAL
    {
        /// <summary>
        /// 添加概略位置
        /// </summary>
        /// <param name="session">概略位置信息</param>
        /// <returns>是否添加成功</returns>
        public bool AddGGAHistory(GGAHistory gga)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                ctx.GGAHistories.Add(gga);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 查找所有概略位置信息
        /// </summary>
        /// <returns>所有概略位置列表</returns>
        public List<GGAHistory> FindAllGGAHistories()
        {
            List<GGAHistory> result = new List<GGAHistory>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.GGAHistories.ToList();
            }
            return result;
        }

        /// <summary>
        /// 查找指定账号名的GGA信息
        /// </summary>
        /// <param name="account">账号名信息</param>
        /// <returns>查找到的GGA信息列表,定位之间倒叙排列</returns>
        public List<GGAHistory> FindGGAHistoriesByAccount(string account)
        {
            List<GGAHistory> result = new List<GGAHistory>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.GGAHistories.Include("SessionHistory").Where<GGAHistory>(g => g.Account == account).OrderByDescending(g=>g.FixedTime).ToList();
            }
            return result;
        }

        /// <summary>
        /// 查找指定会话的GGA信息
        /// </summary>
        /// <param name="session">会话实体</param>
        /// <returns>查找到的GGA信息列表,定位之间倒叙排列</returns>
        public List<GGAHistory> FindGGAHistoriesBySessionHistory(SessionHistory session)
        {
            List<GGAHistory> result = new List<GGAHistory>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.GGAHistories.Where<GGAHistory>(g => g.SessionID == session.ID).OrderByDescending(g => g.FixedTime).ToList();
            }
            return result;
        }
    }
}
