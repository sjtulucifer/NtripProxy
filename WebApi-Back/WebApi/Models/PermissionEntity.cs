using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    public class PermissionEntity
    {
        /// <summary>
        /// 权限ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 权限名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 相关权限角色
        /// </summary>
        public List<RoleEntity> Roles { get; set; }
        /// <summary>
        /// 相关权限菜单
        /// </summary>
        public List<MenuEntity> Menus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 转换WebApi权限实体为dal层权限，只包括基础信息
        /// </summary>
        /// <returns>dal层权限</returns>
        public PERMISSION ToPERMISSION()
        {
            PERMISSION permission = new PERMISSION()
            {
                ID = ID,
                Permission_Name = Name,
                Permission_Description = Description,
                isDelete = IsDelete,
                createTime = CreateTime
            };
            return permission;
        }
    }
}