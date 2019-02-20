using Newtonsoft.Json;
using NtripProxy.DAL.DBDALs;
using NtripProxy.DAL.DBModels;
using NtripProxy.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NtripProxy.WebApi.Controllers
{
    // <summary>
    /// 角色Web操作类
    /// </summary>
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {
        /// <summary>
        /// 角色数据库操作类
        /// </summary>
        private RoleDAL dal = new RoleDAL();

        /// <summary>
        /// 添加新角色
        /// </summary>
        /// <param name="role">新角色信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Route("AddRole")]
        public IHttpActionResult AddRole(RoleEntity role)
        {
            bool isAddSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isAddSuccess = dal.AddRole(role.ToROLE());
                result.Data = role;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Role/AddRole异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isAddSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过角色ID号逻辑删除角色，标志位isDelete设为true
        /// </summary>
        /// <param name="id">角色ID号</param>
        /// <returns>删除是否成功</returns>
        [HttpPut]
        [Route("SoftDeleteRoleByID/{id}")]
        public IHttpActionResult SoftDeleteRoleByID(Guid id)
        {
            bool isDeleteSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isDeleteSuccess = this.dal.SoftDeleteRoleByID(id);
                result.Data = id;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Role/SoftDeleteRoleByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isDeleteSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="role">要更新的角色信息</param>
        /// <returns>更新是否成功信息</returns>
        [HttpPut]
        [Route("UpdateRole")]
        public IHttpActionResult UpdateRole(RoleEntity role)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isUpdateSuccess = this.dal.UpdateRole(role.ToROLE());
                result.Data = role;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Role/UpdateRole异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新角色权限信息
        /// </summary>
        /// <param name="obj">更新信息，传入格式为json{"role":{***},"permissions":[***]}</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        [Route("UpdateRolePermissions")]
        public IHttpActionResult UpdateRolePermissions(dynamic obj)
        {
            RoleEntity role = JsonConvert.DeserializeObject<RoleEntity>(Convert.ToString(obj.role));
            List<PermissionEntity> permissions = new List<PermissionEntity>();
            if (obj.permissions != null && Convert.ToString(obj.permissions) != string.Empty)
            {
                permissions = JsonConvert.DeserializeObject<List<PermissionEntity>>(Convert.ToString(obj.permissions));
            }

            int updateCount = 0;
            ResultEntity result = new ResultEntity();
            try
            {
                updateCount = dal.UpdateRolePermissions(role.ToROLE(), permissions.ConvertAll<PERMISSION>(p => p.ToPERMISSION()));
                role.Permissions = permissions;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Role/UpdateRolePermissions异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = updateCount > 0;
            result.Data = role;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取所有角色实体基础信息
        /// </summary>
        /// <returns>所有角色实体基础信息</returns>
        [HttpGet]
        [Route("GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            ResultEntity result = new ResultEntity();
            List<RoleEntity> roles = new List<RoleEntity>();
            try
            {
                roles = dal.FindAllRoles().ToList<ROLE>().ConvertAll<RoleEntity>(r => r.ToRoleEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Role/GetAllRoles异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = roles;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过ID查找角色所有信息
        /// </summary>
        /// <param name="id">角色ID号</param>
        /// <returns>找到的角色实体</returns>
        [HttpGet]
        [Route("GetRoleByID/{id}")]
        public IHttpActionResult GetRoleByID(string id)
        {
            RoleEntity roleEntity = new RoleEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                ROLE temp = dal.FindRoleByID(new Guid(id));
                roleEntity = temp.ToRoleEntity();
                roleEntity.Users = temp.USERs.ToList<USER>().ConvertAll<UserEntity>(u => u.ToUserEntity());
                roleEntity.Permissions = temp.PERMISSIONs.ToList<PERMISSION>().ConvertAll<PermissionEntity>(p=>p.ToPermissionEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Role/GetRoleByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = result.Message == null;
            result.Data = roleEntity;
            return Json<ResultEntity>(result);
        }
    }
}
