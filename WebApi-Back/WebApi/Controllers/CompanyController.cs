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
    /// 公司Web操作类
    /// </summary>
    [RoutePrefix("api/Company")]
    public class CompanyController : ApiController
    {
        /// <summary>
        /// 公司数据库操作类
        /// </summary>
        private CompanyDAL dal = new CompanyDAL();

        /// <summary>
        /// 添加新公司
        /// </summary>
        /// <param name="company">新公司信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Route("AddCompany")]
        public IHttpActionResult AddCompany(CompanyEntity company)
        {
            bool isAddSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isAddSuccess = dal.AddCompany(company.ToCOMPANY());
                result.Data = company;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Company/AddCompany异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isAddSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过公司ID号逻辑删除公司，标志位isDelete设为true
        /// </summary>
        /// <param name="id">公司ID号</param>
        /// <returns>删除是否成功</returns>
        [HttpPut]
        [Route("SoftDeleteCompanyByID/{id}")]
        public IHttpActionResult SoftDeleteCompanyByID(Guid id)
        {
            bool isDeleteSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isDeleteSuccess = this.dal.SoftDeleteCompanyByID(id);
                result.Data = id;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Role/SoftDeleteCompanyByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isDeleteSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新公司信息
        /// </summary>
        /// <param name="company">要更新的公司信息</param>
        /// <returns>更新是否成功信息</returns>
        [HttpPut]
        [Route("UpdateCompany")]
        public IHttpActionResult UpdateCompany(CompanyEntity company)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isUpdateSuccess = this.dal.UpdateCompany(company.ToCOMPANY());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Company/UpdateCompany异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新母公司信息
        /// </summary>
        /// <param name="obj">更新信息，传入格式为json{"company":{***},"parentCompany":{***}}</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        [Route("UpdateParentCompany")]
        public IHttpActionResult UpdateParentCompany(dynamic obj)
        {
            CompanyEntity company = JsonConvert.DeserializeObject<CompanyEntity>(Convert.ToString(obj.company));
            CompanyEntity parentCompany = JsonConvert.DeserializeObject<CompanyEntity>(Convert.ToString(obj.parentCompany));

            bool updateResult = false;
            ResultEntity result = new ResultEntity();
            try
            {
                updateResult = dal.UpdateParentCompany(company.ToCOMPANY(), parentCompany.ToCOMPANY());
                company.ParentCompany = parentCompany;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Company/UpdateParentCompany异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = updateResult;
            result.Data = company;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取所有公司实体基础信息
        /// </summary>
        /// <returns>所有公司实体基础信息</returns>
        [HttpGet]
        [Route("GetAllCompanies")]
        public IHttpActionResult GetAllCompanies()
        {
            ResultEntity result = new ResultEntity();
            List<CompanyEntity> companies = new List<CompanyEntity>();
            try
            {
                companies = dal.FindAllCompanies().ToList<COMPANY>().ConvertAll<CompanyEntity>(c => c.ToCompanyEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Company/GetAllCompanies异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = companies;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取指定ID公司和全部子公司实体，包含账号信息
        /// </summary>
        /// <param name="id">指定公司的ID号</param>
        /// <returns>返回的公司列表</returns>
        [HttpGet]
        [Route("GetCompanyAndAllSubCompaniesByID/{id}")]
        public IHttpActionResult GetCompanyAndAllSubCompaniesByID(string id)
        {
            ResultEntity result = new ResultEntity();
            List<CompanyEntity> companies = new List<CompanyEntity>();
            try
            {
                List<COMPANY> temp = this.dal.FindCompanyAndAllSubCopaniesByID(new Guid(id));
                foreach(var company in temp)
                {
                    CompanyEntity companyEntity = new CompanyEntity();
                    companyEntity = company.ToCompanyEntity();
                    companyEntity.Accounts = company.ACCOUNTs.ToList<ACCOUNT>().ConvertAll<AccountEntity>(a=>a.ToAccountEntity());
                    companies.Add(companyEntity);
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Company/GetCompanyAndAllSubCompaniesByID异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = companies;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取除指定id以外的其他公司信息
        /// </summary>
        /// <returns>获取到的公司实体列表</returns>
        [HttpGet]
        [Route("GetOtherCompanies/{id}")]
        public IHttpActionResult GetOtherCompanies(string id)
        {
            ResultEntity result = new ResultEntity();
            List<CompanyEntity> companies = new List<CompanyEntity>();
            try
            {
                companies = dal.FindOtherComapnies(new Guid(id)).ToList<COMPANY>().ConvertAll<CompanyEntity>(c => c.ToCompanyEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Company/GetOtherCompanies异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = companies;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过ID查找公司所有信息
        /// </summary>
        /// <param name="id">公司ID号</param>
        /// <returns>找到的公司实体</returns>
        [HttpGet]
        [Route("GetCompanyByID/{id}")]
        public IHttpActionResult GetCompanyByID(string id)
        {
            CompanyEntity companyEntity = new CompanyEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                COMPANY temp = dal.FindCompanyByID(new Guid(id));
                companyEntity = temp.ToCompanyEntity();
                if(temp.COMPANY1 != null)
                {
                    companyEntity.SubCompanies = temp.COMPANY1.ToList<COMPANY>().ConvertAll<CompanyEntity>(c => c.ToCompanyEntity());
                }
                if(temp.COMPANY2 != null)
                {
                    companyEntity.ParentCompany = temp.COMPANY2.ToCompanyEntity();
                }
                if(temp.USERs != null)
                {
                    companyEntity.Users = temp.USERs.ToList<USER>().ConvertAll<UserEntity>(u => u.ToUserEntity());
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Company/GetCompanyByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = result.Message == null;
            result.Data = companyEntity;
            return Json<ResultEntity>(result);
        }
    }
}
