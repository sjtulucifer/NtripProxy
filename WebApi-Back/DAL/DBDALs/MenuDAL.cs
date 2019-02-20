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
    /// 菜单数据库操作类
    /// </summary>
    public class MenuDAL
    {
        /// <summary>
        /// 添加菜单信息
        /// </summary>
        /// <param name="menu">需要添加的菜单信息</param>
        /// <returns>权限是否添加成功</returns>
        public bool AddMenu(MENU menu)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                ctx.MENUs.Add(menu);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 通过ID号逻辑删除菜单
        /// </summary>
        /// <param name="id">菜单ID号</param>
        /// <returns>是否删除成功</returns>
        public bool SoftDeleteMenuByID(Guid id)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                var menu = ctx.MENUs.FirstOrDefault<MENU>(m => m.ID == id);
                menu.isDelete = true;
                result = ctx.SaveChanges() >= 1;
            }
            return result;
        }

        /// <summary>
        /// 更新菜单基础信息
        /// </summary>
        /// <param name="menu">需要更新的菜单</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateMenu(MENU menu)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (menu.ID != null)
                {
                    ctx.MENUs.Attach(menu);
                    ctx.Entry(menu).State = EntityState.Modified;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 查找所有菜单基本信息
        /// </summary>
        /// <returns>所有菜单列表</returns>
        public List<MENU> FindAllMenus()
        {
            List<MENU> result = new List<MENU>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.MENUs.Where<MENU>(m => m.isDelete == false).OrderByDescending(m=>m.createTime).ToList<MENU>();
            }
            return result;
        }

        /// <summary>
        /// 通过权限ID查找菜单全部信息
        /// </summary>
        /// <param name="id">菜单ID号</param>
        /// <returns>查找到的菜单信息</returns>
        public MENU FindMenuByID(Guid id)
        {
            MENU menu = new MENU();
            using (var ctx = new NtripProxyDB())
            {
                menu = ctx.MENUs.Include("PERMISSIONs").FirstOrDefault(m => m.ID == id && m.isDelete == false);
            }
            return menu;
        }
    }
}
