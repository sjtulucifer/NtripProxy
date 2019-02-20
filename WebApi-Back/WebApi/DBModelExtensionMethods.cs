using NtripProxy.DAL.DBModels;
using NtripProxy.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi
{
    /// <summary>
    /// 数据库Model扩展方法类
    /// </summary>
    public static class DBModelExtensionMethods
    {

        /// <summary>
        /// 模型USER类扩展方法,转换基础信息到UserEntity
        /// </summary>
        /// <param name="user">DAL层用户</param>
        /// <returns>WebApi层用户实体</returns>
        public static UserEntity ToUserEntity(this USER user)
        {
            UserEntity userEntity = new UserEntity
            {
                ID = user.ID,
                Name = user.User_Name,
                Login = user.User_Login,
                Password = user.User_Password,
                Email = user.User_Email,
                Phone = user.User_Phone,
                CreateTime = (DateTime)user.createTime,
                IsDelete = (bool)user.isDelete
            };
            return userEntity;
        }

        /// <summary>
        /// 模型COMPANY类扩展方法,转换基础信息到CompanyEntity
        /// </summary>
        /// <param name="company">DAL层公司</param>
        /// <returns>WebApi层公司实体</returns>
        public static CompanyEntity ToCompanyEntity(this COMPANY company)
        {
            CompanyEntity companyEntity = new CompanyEntity
            {
                ID = company.ID,
                Name = company.Company_Name,
                Corporation = company.Company_Corporation,
                Qualification = company.Company_Qualification,
                QNo = company.Company_QNo,
                Field = company.Company_Field,
                Contract = company.Company_Contract,
                Phone = company.Company_Phone,
                Address = company.Company_Address,
                IsDelete = (bool)company.isDelete,
                CreateTime = (DateTime)company.createTime
            };
            return companyEntity;
        }

        /// <summary>
        /// 模型ACCOUNT类扩展方法,转换基础信息到AccountEntity
        /// </summary>
        /// <param name="account">DAL层账号</param>
        /// <returns>WebApi层账号实体</returns>
        public static AccountEntity ToAccountEntity(this ACCOUNT account)
        {
            AccountEntity accountEntity = new AccountEntity
            {
                ID = account.ID,
                Name = account.Account_Name,
                Password = account.Account_Password,
                Register = (DateTime)account.Account_Register,
                Expire = (DateTime)account.Account_Expire,
                LastLogin = account.Account_LastLogin,
                PasswordOvertime = account.Account_PasswordOvertime,
                PasswordOvercount = (int)account.Account_PasswordOvercount,
                IsLocked = (bool)account.Account_IsLocked,
                IsOnline = (bool)account.Account_IsOnline,
                IsDelete = (bool)account.isDelete,
                CreateTime = (DateTime)account.createTime
            };
            return accountEntity;
        }

        /// <summary>
        /// 模型ROLE类扩展方法，转换基础信息到RoleEntity
        /// </summary>
        /// <param name="role">DAL层角色</param>
        /// <returns>WebApi层角色实体</returns>
        public static RoleEntity ToRoleEntity(this ROLE role)
        {
            RoleEntity roleEntity = new RoleEntity
            {
                ID = role.ID,
                Name = role.Role_Name,
                Description = role.Role_Description,
                IsDelete = (bool)role.isDelete,
                CreateTime = (DateTime)role.createTime
            };
            return roleEntity;
        }

        /// <summary>
        /// 模型PERMISSION类扩展方法，转换基础信息到PermissionEntity
        /// </summary>
        /// <param name="permission">DAL层权限</param>
        /// <returns>WebApi层权限实体</returns>
        public static PermissionEntity ToPermissionEntity(this PERMISSION permission)
        {
            PermissionEntity permissionEntity = new PermissionEntity
            {
                ID = permission.ID,
                Name = permission.Permission_Name,
                Description = permission.Permission_Description,
                IsDelete = (bool)permission.isDelete,
                CreateTime = (DateTime)permission.createTime
            };
            return permissionEntity;
        }

        /// <summary>
        /// 模型MENU类扩展方法，转换基础信息到MenuEntity
        /// </summary>
        /// <param name="permission">DAL层菜单</param>
        /// <returns>WebApi层菜单实体</returns>
        public static MenuEntity ToMenuEntity(this MENU menu)
        {
            MenuEntity menuEntity = new MenuEntity
            {
                ID = menu.ID,
                Catagory = menu.Menu_Catagory,
                Name = menu.Menu_Name,
                Url = menu.Menu_URL,
                Description = menu.Menu_Description,
                IsDelete = (bool)menu.isDelete,
                CreateTime = (DateTime)menu.createTime
            };
            return menuEntity;
        }

        /// <summary>
        /// 模型ACCOUNTSYS类扩展方法，转换基础信息到AccountEntity
        /// </summary>
        /// <param name="accountSYS">DAL层系统账号</param>
        /// <returns>WebApi层系统账号实体</returns>
        public static AccountSYSEntity ToAccountSYSEntity(this ACCOUNTSYS accountSYS)
        {
            AccountSYSEntity accountSYSEntity = new AccountSYSEntity
            {
                ID = accountSYS.ID,
                Name = accountSYS.AccountSYS_Name,
                Password = accountSYS.AccountSYS_Password,
                Register = accountSYS.AccountSYS_Register,
                Expire = accountSYS.AccountSYS_Expire,
                LastLogin = accountSYS.AccountSYS_LastLogin,
                Age = (int)accountSYS.AccountSYS_Age,                
                IsOnline = (bool)accountSYS.AccountSYS_IsOnline,
                IsLocked = (bool)accountSYS.AccountSYS_IsLocked,
                IsDelete = (bool)accountSYS.isDelete,
                CreateTime = (DateTime)accountSYS.createTime
            };
            return accountSYSEntity;
        }

        /// <summary>
        /// 模型LOG类扩展方法，转换基础信息到LogEntity
        /// </summary>
        /// <param name="log">DAL层日志</param>
        /// <returns>WebApi层日志实体</returns>
        public static LogEntity ToLogEntity(this LOG log)
        {
            LogEntity logEntity = new LogEntity
            {
                ID = log.ID,
                Time = (DateTime)log.Log_Time,
                User = (Guid)log.Log_User,
                Action = log.Log_Action,
                Module = log.Log_Module,
                Message = log.Log_Message,
            };
            return logEntity;
        }

        /// <summary>
        /// 模型GGAHistory类扩展方法，转换基础信息到GGAHistoryEntity
        /// </summary>
        /// <param name="gga">DAL层概略位置</param>
        /// <returns>WebApi层概略位置实体</returns>
        public static GGAHistoryEntity ToGGAHistoryEntity(this GGAHistory gga)
        {
            GGAHistoryEntity ggaHistoryEntity = new GGAHistoryEntity
            {
                ID = gga.ID,
                Account = gga.Account,
                AccountType = gga.AccountType,
                AccountSYS = gga.AccountSYS,
                FixedTime = (DateTime)gga.FixedTime,
                Lng = (double)gga.Lng,
                Lat = (double)gga.Lat,
                Status = (int)gga.Status,
                GGAInfo = gga.GGAInfo
            };
            return ggaHistoryEntity;
        }

        /// <summary>
        /// SessionHistory，转换基础信息到SessionHistoryEntity
        /// </summary>
        /// <param name="session">DAL层会话</param>
        /// <returns>WebApi层会话实体</returns>
        public static SessionHistoryEntity ToSessionHistoryEntity(this SessionHistory session)
        {
            SessionHistoryEntity sessionHistoryEntity = new SessionHistoryEntity
            {
                ID = session.ID,
                AccountName = session.AccountName,
                AccountType = session.AccountType,
                AccountSYSName = session.AccountSYSName,
                MountPoint = session.MountPoint,
                Client = session.Client,
                ClientAddress = session.ClientAddress,
                ConnectionStart = session.ConnectionStart,
                ConnectionEnd = session.ConnectionEnd,
                GGACount = (int)session.GGACount,
                FixedCount = (int)session.FixedCount,
                ErrorInfo = session.ErrorInfo
            };
            return sessionHistoryEntity;
        }
    }
}