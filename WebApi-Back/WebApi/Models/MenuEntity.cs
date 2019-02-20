using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    public class MenuEntity
    {
        /// <summary>
        /// 菜单ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 菜单分类
        /// </summary>
        public string Catagory { get; set; }
        /// <summary>
        ///  菜单名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单Url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 菜单描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 菜单相关权限
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
        /// 转换WebApi菜单实体为dal层菜单，只包括基础信息
        /// </summary>
        /// <returns>dal层菜单</returns>
        public MENU ToMENU()
        {
            MENU menu = new MENU()
            {
                ID = ID,
                Menu_Catagory = Catagory,
                Menu_Name = Name,
                Menu_URL = Url,
                Menu_Description = Description,
                isDelete = IsDelete,
                createTime = CreateTime
            };
            return menu;
        }
    }
}