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
    /// 菜单Web操作类
    /// </summary>
    [RoutePrefix("api/Menu")]
    public class MenuController : ApiController
    {
        /// <summary>
        /// 菜单数据库操作类
        /// </summary>
        private MenuDAL dal = new MenuDAL();

        /// <summary>
        /// 添加新菜单
        /// </summary>
        /// <param name="menu">新权限信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Route("AddMenu")]
        public IHttpActionResult AddMenu(MenuEntity menu)
        {
            bool isAddSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isAddSuccess = dal.AddMenu(menu.ToMENU());
                result.Data = menu;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Menu/AddMenu异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isAddSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过菜单ID号逻辑删除菜单，标志位isDelete设为true
        /// </summary>
        /// <param name="id">菜单ID号</param>
        /// <returns>删除是否成功</returns>
        [HttpPut]
        [Route("SoftDeleteMenuByID/{id}")]
        public IHttpActionResult SoftDeleteMenuByID(Guid id)
        {
            bool isDeleteSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isDeleteSuccess = this.dal.SoftDeleteMenuByID(id);
                result.Data = id;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Menu/SoftDeleteMenuByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isDeleteSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="menu">要更新的菜单信息</param>
        /// <returns>更新是否成功信息</returns>
        [HttpPut]
        [Route("UpdateMenu")]
        public IHttpActionResult UpdateMenu(MenuEntity menu)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isUpdateSuccess = this.dal.UpdateMenu(menu.ToMENU());
                result.Data = menu;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Menu/UpdateMenu异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取所有菜单实体基础信息
        /// </summary>
        /// <returns>所有菜单实体基础信息</returns>
        [HttpGet]
        [Route("GetAllMenus")]
        public IHttpActionResult GetAllMenus()
        {
            ResultEntity result = new ResultEntity();
            List<MenuEntity> menus = new List<MenuEntity>();
            try
            {
                menus = dal.FindAllMenus().ToList<MENU>().ConvertAll<MenuEntity>(m => m.ToMenuEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Menu/GetAllMenus异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = menus;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过ID查找菜单所有信息
        /// </summary>
        /// <param name="id">菜单ID号</param>
        /// <returns>找到的菜单实体</returns>
        [HttpGet]
        [Route("GetMenuByID/{id}")]
        public IHttpActionResult GetMenuByID(string id)
        {
            MenuEntity permissionEntity = new MenuEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                MENU temp = dal.FindMenuByID(new Guid(id));
                permissionEntity = temp.ToMenuEntity();
                permissionEntity.Permissions = temp.PERMISSIONs.ToList<PERMISSION>().ConvertAll<PermissionEntity>(p => p.ToPermissionEntity());
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
