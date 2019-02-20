using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// 公司实体信息
    /// </summary>
    public class CompanyEntity
    {
        /// <summary>
        /// 公司ID号
        /// </summary>
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司法人
        /// </summary>
        public string Corporation { get; set; }
        /// <summary>
        /// 公司资质
        /// </summary>
        public string Qualification { get; set; }
        /// <summary>
        /// 公司资质号
        /// </summary>
        public string QNo { get; set; }
        /// <summary>
        /// 公司行业
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 公司联系人
        /// </summary>
        public string Contract { get; set; }
        /// <summary>
        /// 联系人手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 公司拥有的账号
        /// </summary>
        public List<AccountEntity> Accounts { get; set; }
        /// <summary>
        /// 公司的子公司列表
        /// </summary>
        public List<CompanyEntity> SubCompanies { get; set; }
        /// <summary>
        /// 公司的母公司
        /// </summary>
        public CompanyEntity ParentCompany { set; get; }
        /// <summary>
        /// 公司的员工
        /// </summary>
        public List<UserEntity> Users { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 转换WebApi公司实体为dal层公司，只包括基础信息
        /// </summary>
        /// <returns>dal层公司</returns>
        public COMPANY ToCOMPANY()
        {
            COMPANY company = new COMPANY()
            {
                ID = ID,
                Company_Name = Name,
                Company_Corporation = Corporation,
                Company_Qualification = Qualification,
                Company_QNo = QNo,
                Company_Field = Field,
                Company_Contract = Contract,
                Company_Phone = Phone,
                Company_Address = Address,
                isDelete = IsDelete,
                createTime = CreateTime
            };
            return company;
        }
    }
}