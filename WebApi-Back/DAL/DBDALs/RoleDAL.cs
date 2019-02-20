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
    /// 角色数据库操作类
    /// </summary>
    public class RoleDAL
    {
        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="role">需要添加的角色信息</param>
        /// <returns>角色是否添加成功</returns>
        public bool AddRole(ROLE role)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                ctx.ROLEs.Add(role);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 通过ID号逻辑删除角色
        /// </summary>
        /// <param name="id">角色ID号</param>
        /// <returns>是否删除成功</returns>
        public bool SoftDeleteRoleByID(Guid id)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                var role = ctx.ROLEs.FirstOrDefault<ROLE>(r => r.ID == id);
                role.isDelete = true;
                result = ctx.SaveChanges() >= 1;
            }
            return result;
        }

        /// <summary>
        /// 更新角色基础信息
        /// </summary>
        /// <param name="role">需要更新的角色</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateRole(ROLE role)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (role.ID != null)
                {
                    ctx.ROLEs.Attach(role);
                    ctx.Entry(role).State = EntityState.Modified;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新角色相关权限
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <param name="permissions">权限列表</param>
        /// <returns>更新权限数量</returns>
        public int UpdateRolePermissions(ROLE role, List<PERMISSION> permissions)
        {
            int result = 0;
            using (var ctx = new NtripProxyDB())
            {
                var updateItem = ctx.ROLEs.Include("PERMISSIONs").FirstOrDefault<ROLE>(r => r.ID == role.ID);
                updateItem.PERMISSIONs.Clear();
                if (permissions != null && permissions.Count > 0)
                {
                    foreach (var permission in permissions)
                    {
                        updateItem.PERMISSIONs.Add(ctx.PERMISSIONs.Find(permission.ID));
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
        /// 查找所有角色基本信息
        /// </summary>
        /// <returns>所有角色列表</returns>
        public List<ROLE> FindAllRoles()
        {
            List<ROLE> result = new List<ROLE>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.ROLEs.Where<ROLE>(r => r.isDelete == false).OrderByDescending(r=>r.createTime).ToList<ROLE>();
            }
            return result;
        }

        /// <summary>
        /// 通过角色ID查找角色全部信息
        /// </summary>
        /// <param name="id">角色ID号</param>
        /// <returns>查找到的角色信息</returns>
        public ROLE FindRoleByID(Guid id)
        {
            ROLE role = new ROLE();
            using (var ctx = new NtripProxyDB())
            {
                role = ctx.ROLEs.Include("PERMISSIONs").Include("USERs").FirstOrDefault(r => r.ID == id && r.isDelete == false);
            }
            return role;
        }
    }
}
