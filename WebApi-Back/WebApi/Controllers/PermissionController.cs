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
    /// 权限Web操作类
    /// </summary>
    [RoutePrefix("api/Permission")]
    public class PermissionController : ApiController
    {
        /// <summary>
        /// 权限数据库操作类
        /// </summary>
        private PermissionDAL dal = new PermissionDAL();

        /// <summary>
        /// 添加新权限
        /// </summary>
        /// <param name="permission">新权限信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Route("AddPermission")]
        public IHttpActionResult AddPermission(PermissionEntity permission)
        {
            bool isAddSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isAddSuccess = dal.AddPermission(permission.ToPERMISSION());
                result.Data = permission;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Permission/AddPermission异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isAddSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过权限ID号逻辑删除权限，标志位isDelete设为true
        /// </summary>
        /// <param name="id">权限ID号</param>
        /// <returns>删除是否成功</returns>
        [HttpPut]
        [Route("SoftDeletePermissionByID/{id}")]
        public IHttpActionResult SoftDeletePermissionByID(Guid id)
        {
            bool isDeleteSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isDeleteSuccess = this.dal.SoftDeletePermissionByID(id);
                result.Data = id;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Permission/SoftDeletePermissionByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isDeleteSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新权限信息
        /// </summary>
        /// <param name="permission">要更新的权限信息</param>
        /// <returns>更新是否成功信息</returns>
        [HttpPut]
        [Route("UpdatePermission")]
        public IHttpActionResult UpdatePermission(PermissionEntity permission)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isUpdateSuccess = this.dal.UpdatePermission(permission.ToPERMISSION());
                result.Data = permission;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Permission/UpdatePermission异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新权限菜单信息
        /// </summary>
        /// <param name="obj">更新信息，传入格式为json{"permission":{***},"menus":[***]}</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        [Route("UpdatePermissionMenus")]
        public IHttpActionResult UpdatePermissionMenus(dynamic obj)
        {
            PermissionEntity permission = JsonConvert.DeserializeObject<PermissionEntity>(Convert.ToString(obj.permission));
            List<MenuEntity> menus = new List<MenuEntity>();
            if (obj.menus != null && Convert.ToString(obj.menus) != string.Empty)
            {
                menus = JsonConvert.DeserializeObject<List<MenuEntity>>(Convert.ToString(obj.menus));
            }

            int updateCount = 0;
            ResultEntity result = new ResultEntity();
            try
            {
                updateCount = dal.UpdatePermissionMenus(permission.ToPERMISSION(), menus.ConvertAll<MENU>(m => m.ToMENU()));
                permission.Menus = menus;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Permission/UpdatePermissionMenus异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = updateCount > 0;
            result.Data = permission;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取所有权限实体基础信息
        /// </summary>
        /// <returns>所有权限实体基础信息</returns>
        [HttpGet]
        [Route("GetAllPermissions")]
        public IHttpActionResult GetAllPermissions()
        {
            ResultEntity result = new ResultEntity();
            List<PermissionEntity> permissions = new List<PermissionEntity>();
            try
            {
                permissions = dal.FindAllPermissions().ToList<PERMISSION>().ConvertAll<PermissionEntity>(p => p.ToPermissionEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Permission/GetAllPermissions异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = permissions;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过ID查找权限所有信息
        /// </summary>
        /// <param name="id">权限ID号</param>
        /// <returns>找到的权限实体</returns>
        [HttpGet]
        [Route("GetPermissionByID/{id}")]
        public IHttpActionResult GetPermissionByID(string id)
        {
            PermissionEntity permissionEntity = new PermissionEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                PERMISSION temp = dal.FindPermissionByID(new Guid(id));
                permissionEntity = temp.ToPermissionEntity();
                permissionEntity.Roles = temp.ROLEs.ToList<ROLE>().ConvertAll<RoleEntity>(r => r.ToRoleEntity());
                permissionEntity.Menus = temp.MENUs.ToList<MENU>().ConvertAll<MenuEntity>(m => m.ToMenuEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Permission/GetPermissionByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = result.Message == null;
            result.Data = permissionEntity;
            return Json<ResultEntity>(result);
        }

    }
}
