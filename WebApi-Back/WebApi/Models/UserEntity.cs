using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// 用户实体信息
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// 系统ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 登陆名
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 电子邮件地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户所属公司
        /// </summary>
        public CompanyEntity Company { get; set; }
        /// <summary>
        /// 用户添加的账号信息
        /// </summary>
        public List<AccountEntity> Accounts { get; set; }
        /// <summary>
        /// 相关角色信息
        /// </summary>
        public List<RoleEntity> Roles { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 转换WebApi用户实体为dal层用户，只包括基础信息
        /// </summary>
        /// <returns>dal层用户</returns>
        public USER ToUSER()
        {
            USER user = new USER()
            {
                ID = ID,
                User_Login = Login,
                User_Password = Password,
                User_Name = Name,
                User_Phone = Phone,
                User_Email = Email,
                isDelete = IsDelete,
                createTime = CreateTime
            };
            return user;
        }
    }
}