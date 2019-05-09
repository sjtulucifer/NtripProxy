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
    /// 账号数据库操作类
    /// </summary>
    class AccountDAL
    {
        /// <summary>
        /// 添加单个账号
        /// </summary>
        /// <param name="account">账号信息</param>
        /// <returns>是否添加成功</returns>
        public bool AddAccount(ACCOUNT account)
        {
            bool result = false;
            using (var ctx = new NtripForwardDB())
            {
                ctx.ACCOUNTs.Add(account);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 更新账号基础资料
        /// </summary>
        /// <param name="account">需要更新的账号</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateAccount(ACCOUNT account)
        {
            bool result = false;
            using (var ctx = new NtripForwardDB())
            {
                if (account.ID != null)
                {
                    ctx.ACCOUNTs.Attach(account);
                    ctx.Entry(account).State = EntityState.Modified;
                    //只更新基础信息不更新关联信息
                    ctx.Entry(account).Property("Account_Company").IsModified = false;
                    ctx.Entry(account).Property("Account_AddUser").IsModified = false;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 查找所有在线账号
        /// </summary>
        /// <returns>在线账号列表</returns>
        public List<ACCOUNT> FindAccountOnline()
        {
            List<ACCOUNT> result = new List<ACCOUNT>();
            using (var ctx = new NtripForwardDB())
            {
                result = ctx.ACCOUNTs.Where<ACCOUNT>(a => a.isDelete == false && a.Account_IsOnline == true).ToList<ACCOUNT>();
            }
            return result;
        }

        /// <summary>
        /// 通过账户名查找账号全部信息
        /// </summary>
        /// <param name="name">账号名</param>
        /// <returns>账号信息</returns>
        public ACCOUNT FindAccountByName(string name)
        {
            ACCOUNT account = new ACCOUNT();
            using (var ctx = new NtripForwardDB())
            {
                account = ctx.ACCOUNTs.FirstOrDefault(a => a.Account_Name == name && a.isDelete == false);
            }
            return account;
        }
    }
}
