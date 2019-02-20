using Newtonsoft.Json;
using NLog;
using NtripProxy.DAL.DBDALs;
using NtripProxy.DAL.DBModels;
using NtripProxy.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NtripProxy.WebApi.Controllers
{
    /// <summary>
    /// 用户Web操作类
    /// </summary>
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

        /// <summary>
        /// 用户数据库操作类
        /// </summary>
        private UserDAL dal = new UserDAL();

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="user">新用户信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Route("AddUser")]
        public IHttpActionResult AddUser(UserEntity user)
        {
            bool isAddSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isAddSuccess = dal.AddUser(user.ToUSER());
                result.Data = user;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/AddUser异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isAddSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 用户登陆验证
        /// </summary>
        /// <param name="user">要验证的用户包含Login和Password</param>
        /// <returns>是否验成功</returns>
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(UserEntity user)
        {
            //是否允许登陆
            bool allowLogin = false;
            ResultEntity result = new ResultEntity();
            try
            {
                allowLogin = dal.LoginCheck(user.ToUSER());
                if(allowLogin)
                {
                    USER temp = dal.FindUserByLoginName(user.Login);
                    UserEntity data = new UserEntity();
                    data = temp.ToUserEntity();
                    //不返回密码信息
                    data.Password = "";
                    //转换添加的账号信息
                    data.Accounts = temp.ACCOUNTs.ToList<ACCOUNT>().ConvertAll<AccountEntity>(a => a.ToAccountEntity());
                    //转换公司信息
                    data.Company = temp.COMPANY.ToCompanyEntity();
                    //转换角色信息
                    data.Roles = temp.ROLEs.ToList<ROLE>().ConvertAll<RoleEntity>(r => r.ToRoleEntity());
                    foreach(var role in data.Roles)
                    {
                        //转换角色中的权限信息
                        role.Permissions = temp.ROLEs.FirstOrDefault<ROLE>(r => r.ID == role.ID)
                                           .PERMISSIONs.ToList<PERMISSION>().ConvertAll<PermissionEntity>(p => p.ToPermissionEntity());
                        foreach(var permission in role.Permissions)
                        {
                            //转换权限中的目录信息
                            permission.Menus = temp.ROLEs.FirstOrDefault<ROLE>(r => r.ID == role.ID)
                                               .PERMISSIONs.FirstOrDefault<PERMISSION>(p => p.ID == permission.ID)
                                               .MENUs.ToList<MENU>().ConvertAll<MenuEntity>(m => m.ToMenuEntity());
                        }
                    }
                    result.Data = data;
                    //获取访问的ip地址
                    string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    //NLog记录日志
                    LogEventInfo logInfo = new LogEventInfo(LogLevel.Info, "", "");
                    logInfo.Properties["Log_Time"] = DateTime.Now;
                    logInfo.Properties["Log_User"] = user.ID;
                    logInfo.Properties["Log_Action"] = "用户登录";
                    logInfo.Properties["Log_Module"] = "登录模块";
                    logInfo.Properties["Log_Message"] = "登录ip地址为" + ip;
                    NtripProxyLogger.LogActionIntoDatabase(logInfo);
                }
                else
                {
                    result.IsSuccess = false;
                    result.Data = "用户名或密码错误";
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/Login异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = allowLogin;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过用户ID号逻辑删除用户，标志位isDelete设为true
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>删除是否成功</returns>
        [HttpPut]
        [Route("SoftDeleteUserByID/{id}")]
        public IHttpActionResult SoftDeleteUserByID(Guid id)
        {
            bool isDeleteSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isDeleteSuccess = this.dal.SoftDeleteUserByID(id);
                result.Data = id;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/SoftDeleteUserByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isDeleteSuccess;
            return Json<ResultEntity>(result);
        }

        [HttpPut]
        [Route("ResetUserPassword")]
        public IHttpActionResult ResetUserPassword(UserEntity user)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isUpdateSuccess = this.dal.ResetUserPassword(user.ToUSER());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/ResetUserPassword异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;
            return Json<ResultEntity>(result);
        }

        [HttpPut]
        [Route("ResetUserPasswordProfile")]
        public IHttpActionResult ResetUserPasswordProfile(UserEntity user)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                bool passwordRight = dal.LoginCheck(user.ToUSER());
                if(passwordRight)
                {
                    isUpdateSuccess = this.dal.ResetUserPassword(user.ToUSER());
                }
                else
                {
                    result.Data = "旧密码错误";
                }
                
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/ResetUserPasswordProfile异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;
            return Json<ResultEntity>(result);
        }


        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">要更新的用户信息</param>
        /// <returns>更新是否成功信息</returns>
        [HttpPut]
        [Route("UpdateUser")]
        public IHttpActionResult UpdateUser(UserEntity user)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isUpdateSuccess = this.dal.UpdateUser(user.ToUSER());
                result.Data = user;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/UpdateUser异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新用户公司信息
        /// </summary>
        /// <param name="obj">更新信息，传入格式为json{"user":{***},"company":{***}}</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        [Route("UpdateUserCompany")]
        public IHttpActionResult UpdateUserCompany(dynamic obj)
        {
            UserEntity user = JsonConvert.DeserializeObject<UserEntity>(Convert.ToString(obj.user));
            CompanyEntity company = JsonConvert.DeserializeObject<CompanyEntity>(Convert.ToString(obj.company));

            bool updateResult = false;
            ResultEntity result = new ResultEntity();
            try
            {
                updateResult = dal.UpdateUserCompany(user.ToUSER(), company.ToCOMPANY());
                user.Company = company;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/UpdateUserCompany异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = updateResult;
            result.Data = user;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新用户角色信息
        /// </summary>
        /// <param name="obj">更新信息，传入格式为json{"user":{***},"roles":[***]}</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        [Route("UpdateUserRoles")]
        public IHttpActionResult UpdateUserRoles(dynamic obj)
        {
            UserEntity user = JsonConvert.DeserializeObject<UserEntity>(Convert.ToString(obj.user));
            List<RoleEntity> roles = new List<RoleEntity>();
            if (obj.roles != null && Convert.ToString(obj.roles) != string.Empty)
            {
                roles = JsonConvert.DeserializeObject<List<RoleEntity>>(Convert.ToString(obj.roles));
            }

            int updateCount = 0;
            ResultEntity result = new ResultEntity();
            try
            {
                updateCount = dal.UpdateUserRoles(user.ToUSER(), roles.ConvertAll<ROLE>(r => r.ToROLE()));
                user.Roles = roles;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/UpdateUserRoles异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = updateCount>0;
            result.Data = user;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取所有用户实体基础信息
        /// </summary>
        /// <returns>所有用户实体基础信息</returns>
        [HttpGet]
        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers()
        {
            ResultEntity result = new ResultEntity();
            List<UserEntity> users = new List<UserEntity>();
            try
            {
                users = dal.FindAllUsers().ToList<USER>().ConvertAll<UserEntity>(u => u.ToUserEntity());
                //密码置空
                users.ForEach(u => u.Password="");
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/GetAllUsers异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = users;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过ID查找用户所有信息
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>找到的用户实体</returns>
        [HttpGet]
        [Route("GetUserByID/{id}")]
        public IHttpActionResult GetUserByID(string id)
        {
            UserEntity userEntity = new UserEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                USER temp = dal.FindUserByID(new Guid(id));
                userEntity = temp.ToUserEntity();
                //不返回密码信息
                userEntity.Password = "";
                if(temp.COMPANY != null)
                {
                    userEntity.Company = temp.COMPANY.ToCompanyEntity();
                }
                userEntity.Accounts = temp.ACCOUNTs.ToList<ACCOUNT>().ConvertAll<AccountEntity>(a=>a.ToAccountEntity());
                userEntity.Roles = temp.ROLEs.ToList<ROLE>().ConvertAll<RoleEntity>(r => r.ToRoleEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/GetUserByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = result.Message == null;
            result.Data = userEntity;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过用户登陆名查找用户所有信息
        /// </summary>
        /// <param name="loginName">用户ID号</param>
        /// <returns>找到的用户实体</returns>
        [HttpGet]
        [Route("GetUserByLoginName/{loginName}")]
        public IHttpActionResult GetUserByLoginName(string loginName)
        {
            UserEntity userEntity = new UserEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                USER temp = dal.FindUserByLoginName(loginName);
                userEntity = temp.ToUserEntity();
                //不返回密码信息
                userEntity.Password = "";
                if (temp.COMPANY != null)
                {
                    userEntity.Company = temp.COMPANY.ToCompanyEntity();
                }
                userEntity.Accounts = temp.ACCOUNTs.ToList<ACCOUNT>().ConvertAll<AccountEntity>(a => a.ToAccountEntity());
                userEntity.Roles = temp.ROLEs.ToList<ROLE>().ConvertAll<RoleEntity>(r => r.ToRoleEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/User/GetUserByLoginName/{loginName}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = result.Message == null;
            result.Data = userEntity;
            return Json<ResultEntity>(result);
        }

    }
}
