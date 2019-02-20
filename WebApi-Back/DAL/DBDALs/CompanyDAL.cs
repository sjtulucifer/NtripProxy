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
    /// 公司数据库操作类
    /// </summary>
    public class CompanyDAL
    {
        /// <summary>
        /// 添加公司信息
        /// </summary>
        /// <param name="company">需要添加的公司信息</param>
        /// <returns>用户是否添加成功</returns>
        public bool AddCompany(COMPANY company)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                ctx.COMPANies.Add(company);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 通过ID号逻辑删除公司
        /// </summary>
        /// <param name="id">公司ID号</param>
        /// <returns>是否删除成功</returns>
        public bool SoftDeleteCompanyByID(Guid id)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                var company = ctx.COMPANies.FirstOrDefault<COMPANY>(c => c.ID == id);
                company.isDelete = true;
                result = ctx.SaveChanges() >= 1;
            }
            return result;
        }

        /// <summary>
        /// 更新公司基础资料
        /// </summary>
        /// <param name="company">需要更新的公司</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateCompany(COMPANY company)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (company.ID != null)
                {
                    ctx.COMPANies.Attach(company);
                    ctx.Entry(company).State = EntityState.Modified;
                    //只更新基础信息不更新关联信息
                    ctx.Entry(company).Property("Company_Chief").IsModified = false;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新母公司信息
        /// </summary>
        /// <param name="company">公司信息</param>
        /// <param name="parentCompany">母公司</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateParentCompany(COMPANY company, COMPANY parentCompany)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (company.ID != null && parentCompany.ID != null)
                {
                    company.Company_Chief = parentCompany.ID;
                    ctx.COMPANies.Attach(company);
                    ctx.Entry(company).Property("Company_Chief").IsModified = true;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 查找所有公司基本信息
        /// </summary>
        /// <returns>所有公司列表</returns>
        public List<COMPANY> FindAllCompanies()
        {
            List<COMPANY> result = new List<COMPANY>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.COMPANies.Where<COMPANY>(c => c.isDelete == false).OrderByDescending(c => c.createTime).ToList<COMPANY>();
            }
            return result;
        }

        /// <summary>
        /// 查找除了指定id号以外的公司
        /// </summary>
        /// <param name="id">需要去除的公司的ID号</param>
        /// <returns>查询到的公司列表</returns>
        public List<COMPANY> FindOtherComapnies(Guid id)
        {
            List<COMPANY> result = new List<COMPANY>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.COMPANies.Where<COMPANY>(c => c.isDelete == false && c.ID != id).ToList<COMPANY>();
            }
            return result;
        }

        /// <summary>
        /// 通过ID号查找公司以及全部子公司信息
        /// </summary>
        /// <param name="id">公司ID号</param>
        /// <returns>公司与全部子公司列表</returns>
        public List<COMPANY> FindCompanyAndAllSubCopaniesByID(Guid id)
        {
            List<COMPANY> result = new List<COMPANY>();
            using (var ctx = new NtripProxyDB())
            {
                result.Add(ctx.COMPANies.Include("ACCOUNTs").Where(c=>c.ID == id).FirstOrDefault<COMPANY>());
                result.AddRange(findAllChildren(id));
            }
            return result;
        }

        /// <summary>
        /// 通过ID号查找公司全部信息
        /// </summary>
        /// <param name="id">公司ID号</param>
        /// <returns>公司信息</returns>
        public COMPANY FindCompanyByID(Guid id)
        {
            COMPANY company = new COMPANY();
            using (var ctx = new NtripProxyDB())
            {
                company = ctx.COMPANies.Include("ACCOUNTs").Include("COMPANY1").Include("COMPANY2").Include("USERs").FirstOrDefault(c => c.ID == id && c.isDelete == false);
            }
            return company;
        }
        /// <summary>
        /// 查找指定ID号的公司的所有子公司,并包含账号信息
        /// </summary>
        /// <param name="id">指定的公司ID号</param>
        /// <returns>查找到的所有子公司信息</returns>
        private List<COMPANY> findAllChildren(Guid id)
        {
            List<COMPANY> allSubCompanies = new List<COMPANY>();
            using (var ctx = new NtripProxyDB())
            {
                allSubCompanies = ctx.COMPANies.Include("ACCOUNTs").Where(c => c.COMPANY2 != null && c.COMPANY2.ID == id).ToList<COMPANY>();
                List<COMPANY> tmpList = new List<COMPANY>(allSubCompanies);
                foreach (COMPANY sub in tmpList)
                {
                    List<COMPANY> tmpChildren = findAllChildren(sub.ID);
                    if (tmpChildren.Count != 0)
                    {
                        allSubCompanies.AddRange(tmpChildren);
                    }
                }
            }
            return allSubCompanies;
        }
    }
}
