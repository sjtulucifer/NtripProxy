using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// 角色实体信息
    /// </summary>
    public class RoleEntity
    {
        /// <summary>
        /// 角色ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 角色包含的用户信息
        /// </summary>
        public List<UserEntity> Users { set; get; }
        /// <summary>
        /// 角色包含的权限信息
        /// </summary>
        public List<PermissionEntity> Permissions { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 转换WebApi角色实体为dal层角色，只包括基础信息
        /// </summary>
        /// <returns>dal层角色</returns>
        public ROLE ToROLE()
        {
            ROLE role = new ROLE()
            {
                ID = ID,
                Role_Name = Name,
                Role_Description = Description,
                isDelete = IsDelete,
                createTime = CreateTime
            };
            return role;
        }
    }
}