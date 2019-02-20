using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// 账号实体信息
    /// </summary>
    public class AccountEntity
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
        /// 账号密码输错时间
        /// </summary>
        public DateTime? PasswordOvertime { get; set; } = null;
        /// <summary>
        /// 账号密码输错次数
        /// </summary>
        public int PasswordOvercount { get; set; } = 0;
        /// <summary>
        /// 账号是否被锁定
        /// </summary>
        public bool IsLocked { get; set; } = false;
        /// <summary>
        /// 账号是否在线
        /// </summary>
        public bool IsOnline { get; set; } = false;
        /// <summary>
        /// 账户所属的公司
        /// </summary>
        public CompanyEntity Company { get; set; }
        /// <summary>
        /// 添加账户的用户
        /// </summary>
        public UserEntity AddUser { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 转换WebApi账号实体为dal层账号，只包括基础信息
        /// </summary>
        /// <returns>dal层账号</returns>
        public ACCOUNT ToACCOUNT()
        {
            ACCOUNT account = new ACCOUNT()
            {
                ID = ID,
                Account_Name = Name,
                Account_Password = Password,
                Account_Register = Register,
                Account_Expire = Expire,
                Account_LastLogin = LastLogin,
                Account_PasswordOvertime = PasswordOvertime,
                Account_PasswordOvercount = PasswordOvercount,
                Account_IsLocked = IsLocked,
                Account_IsOnline = IsOnline,
                isDelete = IsDelete,
                createTime = CreateTime
            };
            return account;
        }
    }
}