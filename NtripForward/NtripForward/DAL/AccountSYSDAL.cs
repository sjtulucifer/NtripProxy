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
    /// 系统账号数据库操作类
    /// </summary>
    class AccountSYSDAL
    {
        /// <summary>
        /// 更新系统账号资料
        /// </summary>
        /// <param name="accountSYS">需要更新的系统账号</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateAccountSYS(ACCOUNTSYS accountSYS)
        {
            bool result = false;
            using (var ctx = new NtripForwardDB())
            {
                if (accountSYS.ID != null)
                {
                    ctx.ACCOUNTSYS.Attach(accountSYS);
                    ctx.Entry(accountSYS).State = EntityState.Modified;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }


        /// <summary>
        /// 查找所有在线系统账号基本信息
        /// </summary>
        /// <returns>所有在线系统账号列表</returns>
        public List<ACCOUNTSYS> FindAllAccountSYSOnline()
        {
            List<ACCOUNTSYS> result = new List<ACCOUNTSYS>();
            using (var ctx = new NtripForwardDB())
            {
                result = ctx.ACCOUNTSYS.Where<ACCOUNTSYS>(a => a.isDelete == false && a.AccountSYS_IsOnline == true).ToList<ACCOUNTSYS>();
            }
            return result;
        }
       
        /// <summary>
        /// 通过系统账户名查找系统账号全部信息
        /// </summary>
        /// <param name="id">系统账号名</param>
        /// <returns>系统账号信息</returns>
        public ACCOUNTSYS FindAccountSYSByName(string name)
        {
            ACCOUNTSYS accountSYS = new ACCOUNTSYS();
            using (var ctx = new NtripForwardDB())
            {
                accountSYS = ctx.ACCOUNTSYS.FirstOrDefault(a => a.AccountSYS_Name == name && a.isDelete == false);
            }
            return accountSYS;
        }

        /// <summary>
        /// 账号拨号以后查找适合使用的系统账号
        /// </summary>
        /// <returns>返回的系统账号系统</returns>
        public ACCOUNTSYS FindSuitableAccountSYS()
        {
            ACCOUNTSYS accountSYS = new ACCOUNTSYS();
            using (var ctx = new NtripForwardDB())
            {
                List<ACCOUNTSYS> temp = ctx.ACCOUNTSYS.Where<ACCOUNTSYS>(
                    a => a.isDelete == false
                    && a.AccountSYS_IsOnline == false
                    && a.AccountSYS_IsLocked == false
                    && a.AccountSYS_Expire > DateTime.Now)
                    .ToList<ACCOUNTSYS>();
                int age = int.MaxValue;
                foreach (var item in temp)
                {
                    if (item.AccountSYS_Age < age)
                    {
                        age = (int)item.AccountSYS_Age;
                        accountSYS = item;
                    }
                }
            }
            return accountSYS;
        }
    }
}
