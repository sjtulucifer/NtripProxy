using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtripProxy.DAL.DBDALs
{
    /// <summary>
    /// 账号数据库操作类
    /// </summary>
    public class AccountDAL
    {
        /// <summary>
        /// 添加单个账号
        /// </summary>
        /// <param name="account">账号信息</param>
        /// <returns>是否添加成功</returns>
        public bool AddAccount(ACCOUNT account)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                ctx.ACCOUNTs.Add(account);             
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 通过ID号逻辑删除账号
        /// </summary>
        /// <param name="id">账号ID号</param>
        /// <returns>是否删除成功</returns>
        public bool SoftDeleteAccountByID(Guid id)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                var account = ctx.ACCOUNTs.FirstOrDefault<ACCOUNT>(a => a.ID == id);
                account.isDelete = true;
                result = ctx.SaveChanges() >= 1;
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
            using (var ctx = new NtripProxyDB())
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
        /// 更新账户公司信息
        /// </summary>
        /// <param name="account">账户信息</param>
        /// <param name="company">公司信息</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateAccountCompany(ACCOUNT account, COMPANY company)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (account.ID != null && company.ID != null)
                {
                    account.Account_Company = company.ID;
                    ctx.ACCOUNTs.Attach(account);
                    ctx.Entry(account).Property("Account_Company").IsModified = true;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 查找所有账号基本信息
        /// </summary>
        /// <returns>所有账号列表</returns>
        public List<ACCOUNT> FindAllAccounts()
        {
            List<ACCOUNT> result = new List<ACCOUNT>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.ACCOUNTs.Where<ACCOUNT>(a => a.isDelete == false).OrderByDescending(a=>a.createTime).ToList<ACCOUNT>();
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
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.ACCOUNTs.Where<ACCOUNT>(a => a.isDelete == false && a.Account_IsOnline == true).ToList<ACCOUNT>();
            }
            return result;
        }

        /// <summary>
        /// 通过ID号查找账号全部信息
        /// </summary>
        /// <param name="id">账号ID号</param>
        /// <returns>账号信息</returns>
        public ACCOUNT FindAccountByID(Guid id)
        {
            ACCOUNT account = new ACCOUNT();
            using (var ctx = new NtripProxyDB())
            {
                account = ctx.ACCOUNTs.Include("COMPANY").Include("USER").FirstOrDefault(a => a.ID == id && a.isDelete == false);
            }
            return account;
        }

        /// <summary>
        /// 通过公司ID号查找公司账号信息
        /// </summary>
        /// <param name="id">公司ID号</param>
        /// <returns>账号信息</returns>
        public List<ACCOUNT> FindAccountByCompanyID(Guid id)
        {
            List<ACCOUNT> result = new List<ACCOUNT>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.ACCOUNTs.Include("COMPANY").Where<ACCOUNT>(a => a.isDelete == false && a.COMPANY.ID == id).ToList<ACCOUNT>();
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
            using (var ctx = new NtripProxyDB())
            {
                account = ctx.ACCOUNTs.FirstOrDefault(a => a.Account_Name == name && a.isDelete == false);
            }
            return account;
        }
    }
}
