using NLog;
using NtripProxy.DAL.DBDALs;
using NtripProxy.DAL.DBModels;
using NtripProxy.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NtripProxy.WebApi.Controllers
{
    // <summary>
    /// 系统账号Web操作类
    /// </summary>
    [RoutePrefix("api/AccountSYS")]
    public class AccountSYSController : ApiController
    {
        /// <summary>
        /// 系统账号数据库操作类
        /// </summary>
        private AccountSYSDAL dal = new AccountSYSDAL();

        /// <summary>
        /// 添加新系统账号
        /// </summary>
        /// <param name="accountSYS">新系统账号信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Route("AddAccountSYS")]
        public IHttpActionResult AddAccountSYS(AccountSYSEntity accountSYS)
        {
            bool isAddSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                ACCOUNTSYS temp = accountSYS.ToACCOUNTSYS();
                isAddSuccess = dal.AddAccountSYS(temp);
                result.Data = accountSYS;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/AccountSYS/AddAccountSYS异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isAddSuccess;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 更新系统账号信息
        /// </summary>
        /// <param name="accountSYS">要更新的系统账号信息</param>
        /// <returns>更新是否成功信息</returns>
        [HttpPut]
        [Route("UpdateAccountSYS")]
        public IHttpActionResult UpdateAccountSYS(AccountSYSEntity accountSYS)
        {
            bool isUpdateSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                isUpdateSuccess = this.dal.UpdateAccountSYS(accountSYS.ToACCOUNTSYS());
                result.Data = accountSYS;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/AccountSYS/UpdateAccountSYS异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = isUpdateSuccess;

            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取所有系统账号实体基础信息
        /// </summary>
        /// <returns>所有系统账号实体基础信息</returns>
        [HttpGet]
        [Route("GetAllAccountSYSs")]
        public IHttpActionResult GetAllAccountSYSs()
        {
            ResultEntity result = new ResultEntity();
            List<AccountSYSEntity> accounts = new List<AccountSYSEntity>();
            try
            {
                accounts = dal.FindAllAccountSYSs().ToList<ACCOUNTSYS>().ConvertAll<AccountSYSEntity>(a => a.ToAccountSYSEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/AccountSYS/GetAllAccountSYSs异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = accounts;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取系统账号数量
        /// </summary>
        /// <returns>系统账号数量</returns>
        [HttpGet]
        [Route("GetAllAccountSYSCount")]
        public IHttpActionResult GetAccountSYSCount()
        {
            ResultEntity result = new ResultEntity();
            int count = 0;
            try
            {
                count = dal.FindAllAccountSYSs().Count;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/AccountSYS/GetAccountSYSCount异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = count;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 获取系统账号在线数量
        /// </summary>
        /// <returns>系统账号数量</returns>
        [HttpGet]
        [Route("GetAllAccountSYSOnlineCount")]
        public IHttpActionResult GetAccountSYSOnlineCount()
        {
            ResultEntity result = new ResultEntity();
            int count = 0;
            try
            {
                count = dal.FindAllAccountSYSOnline().Count;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/AccountSYS/GetAccountSYSOnlineCount异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = count;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过ID查找系统账号所有信息
        /// </summary>
        /// <param name="id">系统账号ID号</param>
        /// <returns>找到的系统账号实体</returns>
        [HttpGet]
        [Route("GetAccountSYSByID/{id}")]
        public IHttpActionResult GetAccountSYSByID(string id)
        {
            AccountSYSEntity accountEntity = new AccountSYSEntity();
            ResultEntity result = new ResultEntity();
            try
            {
                ACCOUNTSYS temp = dal.FindAccountSYSByID(new Guid(id));
                accountEntity = temp.ToAccountSYSEntity();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/AccountSYS/GetAccountSYSByID/{id}异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = result.Message == null;
            result.Data = accountEntity;
            return Json<ResultEntity>(result);
        }
    }
}
