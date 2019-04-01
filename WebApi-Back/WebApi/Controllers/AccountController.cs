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
    /// 账号Web操作类
    /// </summary>
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        /// <summary>
        /// 账号数据库操作类
        /// </summary>
        private AccountDAL dal = new AccountDAL();

        /// <summary>
        /// 添加新账号
        /// </summary>
        /// <param name="account">新账号信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Route("AddAccount")]
        public IHttpActionResult AddAccount(AccountEntity account)
        {
            bool isAddSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                ACCOUNT temp = account.ToACCOUNT();
                //记录添加用户信息
                temp.Account_AddUser = account.AddUser.ID;
                //记录添加公司信息
                temp.Account_Company = account.Company.ID;
                isAddSuccess = dal.AddAccount(temp);
                result.Data = account;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Account/AddAccount异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isAddSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="account">要更新的账号信息</param>
        /// <returns>更新是否成功信息</returns>
        [HttpPut]
        [Route("UpdateAccount")]
        public IHttpActionResult UpdateAccount(AccountEntity account)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isUpdateSuccess = this.dal.UpdateAccount(account.ToACCOUNT());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Account/UpdateAccount异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新账号公司信息
        /// </summary>
        /// <param name="obj">更新信息，传入格式为json{"account":{***},"company":{***}}</param>
        /// <returns>更新结果</returns>
        [HttpPut]
        [Route("UpdateAccountCompany")]
        public IHttpActionResult UpdateAccountCompany(dynamic obj)
        {
            AccountEntity account = JsonConvert.DeserializeObject<AccountEntity>(Convert.ToString(obj.account));
            CompanyEntity company = JsonConvert.DeserializeObject<CompanyEntity>(Convert.ToString(obj.company));

            bool updateResult = false;
            ResultEntity result = new ResultEntity();
            try
            {
                updateResult = dal.UpdateAccountCompany(account.ToACCOUNT(), company.ToCOMPANY());
                account.Company = company;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Account/UpdateAccountCompany异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = updateResult;
            result.Data = account;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取所有账号实体基础信息
        /// </summary>
        /// <returns>所有账号实体基础信息</returns>
        [HttpGet]
        [Route("GetAllAccounts")]
        public IHttpActionResult GetAllAccounts()
        {
            ResultEntity result = new ResultEntity();
            List<AccountEntity> accounts = new List<AccountEntity>();
            try
            {
                accounts = dal.FindAllAccounts().ToList<ACCOUNT>().ConvertAll<AccountEntity>(a => a.ToAccountEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Account/GetAllAccounts异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = accounts;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 查找所有在线账号
        /// </summary>
        /// <returns>查找到的在线账户列表</returns>
        [HttpGet]
        [Route("GetAccountsOnline")]
        public IHttpActionResult GetAccountsOnline()
        {
            ResultEntity result = new ResultEntity();
            List<AccountEntity> accounts = new List<AccountEntity>();
            try
            {
                accounts = dal.FindAccountOnline().ToList<ACCOUNT>().ConvertAll<AccountEntity>(a => a.ToAccountEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Account/GetAccountsOnline异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = accounts;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过ID查找账号所有信息
        /// </summary>
        /// <param name="id">账号ID号</param>
        /// <returns>找到的账号实体</returns>
        [HttpGet]
        [Route("GetAccountByID/{id}")]
        public IHttpActionResult GetAccountByID(string id)
        {
            AccountEntity accountEntity = new AccountEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                ACCOUNT temp = dal.FindAccountByID(new Guid(id));
                accountEntity = temp.ToAccountEntity();
                accountEntity.AddUser = temp.USER.ToUserEntity();
                accountEntity.Company = temp.COMPANY.ToCompanyEntity();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Account/GetAccountByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = result.Message == null;
            result.Data = accountEntity;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过账号名查找所有信息
        /// </summary>
        /// <param name="name">账号名</param>
        /// <returns>找到的账号实体</returns>
        [HttpGet]
        [Route("GetAccountByName/{name}")]
        public IHttpActionResult GetAccountByName(string name)
        {
            AccountEntity accountEntity = new AccountEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                ACCOUNT temp = dal.FindAccountByName(name);
                accountEntity = temp.ToAccountEntity();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Account/GetAccountByName/{name}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = result.Message == null;
            result.Data = accountEntity;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过公司ID查找公司所有账号信息
        /// </summary>
        /// <param name="id">公司ID号</param>
        /// <returns>找到的账号实体</returns>
        [HttpGet]
        [Route("GetAccountByCompanyID/{id}")]
        public IHttpActionResult GetAccountByCompanyID(string id)
        {
            ResultEntity result = new ResultEntity();
            List<AccountEntity> accounts = new List<AccountEntity>();
            try
            {
                accounts = dal.FindAccountByCompanyID(new Guid(id)).ToList<ACCOUNT>().ConvertAll<AccountEntity>(a => a.ToAccountEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Account/GetAccountByCompanyID/{id}异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = accounts;
            return Json<ResultEntity>(result);
        }
    }
}
