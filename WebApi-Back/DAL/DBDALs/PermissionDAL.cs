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
    /// 权限数据库操作类
    /// </summary>
    public class PermissionDAL
    {
        /// <summary>
        /// 添加权限信息
        /// </summary>
        /// <param name="permission">需要添加的权限信息</param>
        /// <returns>权限是否添加成功</returns>
        public bool AddPermission(PERMISSION permission)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                ctx.PERMISSIONs.Add(permission);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 通过ID号逻辑删除权限
        /// </summary>
        /// <param name="id">权限ID号</param>
        /// <returns>是否删除成功</returns>
        public bool SoftDeletePermissionByID(Guid id)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                var permission = ctx.PERMISSIONs.FirstOrDefault<PERMISSION>(p => p.ID == id);
                permission.isDelete = true;
                result = ctx.SaveChanges() >= 1;
            }
            return result;
        }

        /// <summary>
        /// 更新权限基础信息
        /// </summary>
        /// <param name="permission">需要更新的权限</param>
        /// <returns>是否更新成功</returns>
        public bool UpdatePermission(PERMISSION permission)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (permission.ID != null)
                {
                    ctx.PERMISSIONs.Attach(permission);
                    ctx.Entry(permission).State = EntityState.Modified;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新权限相关菜单
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <param name="menus">菜单列表</param>
        /// <returns>更新菜单数量</returns>
        public int UpdatePermissionMenus(PERMISSION permission, List<MENU> menus)
        {
            int result = 0;
            using (var ctx = new NtripProxyDB())
            {
                var updateItem = ctx.PERMISSIONs.Include("MENUs").FirstOrDefault<PERMISSION>(p => p.ID == permission.ID);
                updateItem.MENUs.Clear();
                if (menus != null && menus.Count > 0)
                {
                    foreach (var menu in menus)
                    {
                        updateItem.MENUs.Add(ctx.MENUs.Find(menu.ID));
                    }
                    result = ctx.SaveChanges();
                }
                else
                {
                    result = ctx.SaveChanges();
                }
            }
            return result;
        }

        /// <summary>
        /// 查找所有权限基本信息
        /// </summary>
        /// <returns>所有权限列表</returns>
        public List<PERMISSION> FindAllPermissions()
        {
            List<PERMISSION> result = new List<PERMISSION>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.PERMISSIONs.Where<PERMISSION>(p => p.isDelete == false).OrderByDescending(p=>p.createTime).ToList<PERMISSION>();
            }
            return result;
        }

        /// <summary>
        /// 通过权限ID查找权限全部信息
        /// </summary>
        /// <param name="id">权限ID号</param>
        /// <returns>查找到的权限信息</returns>
        public PERMISSION FindPermissionByID(Guid id)
        {
            PERMISSION permission = new PERMISSION();
            using (var ctx = new NtripProxyDB())
            {
                permission = ctx.PERMISSIONs.Include("MENUs").Include("ROLEs").FirstOrDefault(p => p.ID == id && p.isDelete == false);
            }
            return permission;
        }
    }
}
