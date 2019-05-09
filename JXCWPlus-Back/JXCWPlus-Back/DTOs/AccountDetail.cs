using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JXCWPlus_Back.DTOs
{
    /// <summary>
    /// 账号细节数据交换类
    /// </summary>
    public class AccountDetail
    {
        /// <summary>
        /// 账号名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 账号密码
        /// </summary>
        public string AccountPassword { get; set; }
        /// <summary>
        /// 账号注册时间
        /// </summary>
        public DateTime? AccountRegister { get; set; }
        /// <summary>
        /// 账号过期时间
        /// </summary>
        public DateTime? AccountExpire { get; set; }
        /// <summary>
        /// 账号是否被锁定
        /// </summary>
        public bool? AccountIsLocked { get; set; }
    }
}