using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// 系统账号实体信息
    /// </summary>
    public class AccountSYSEntity
    {
        /// <summary>
        /// 账号ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 账号名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 账号注册时间
        /// </summary>
        public DateTime Register { get; set; }
        /// <summary>
        /// 账号过期时间
        /// </summary>
        public DateTime Expire { get; set; }
        /// <summary>
        /// 账号上次登陆时间
        /// </summary>
        public DateTime? LastLogin { get; set; } = null;
        /// <summary>
        /// 账号登陆次数
        /// </summary>
        public int Age { get; set; } = 0;
        /// <summary>
        /// 账号是否在线
        /// </summary>
        public bool IsOnline { get; set; } = false;
        /// <summary>
        /// 账号是否被锁定
        /// </summary>
        public bool IsLocked { get; set; } = false;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 转换WebApi千寻账号账号实体为dal层千寻账号，只包括基础信息
        /// </summary>
        /// <returns>dal层千寻账号</returns>
        public ACCOUNTSYS ToACCOUNTSYS()
        {
            ACCOUNTSYS accountSYS = new ACCOUNTSYS()
            {
                ID = ID,
                AccountSYS_Name = Name,
                AccountSYS_Password = Password,
                AccountSYS_Register = Register,
                AccountSYS_Expire = Expire,
                AccountSYS_LastLogin = LastLogin,
                AccountSYS_Age = Age,
                AccountSYS_IsOnline = IsOnline,
                AccountSYS_IsLocked = IsLocked,
                isDelete = IsDelete,
                createTime = CreateTime
            };
            return accountSYS;
        }
    }
}