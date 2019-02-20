using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using NtripProxy.DAL.DBDALs;
using NtripProxy.DAL.DBModels;
using NtripProxy.WebApi.Models;

namespace NtripProxy.WebApi.Controllers
{
    // <summary>
    /// 日志Web操作类
    /// </summary>
    [RoutePrefix("api/Log")]
    public class LogController : ApiController
    {
        /// <summary>
        /// 日志数据库操作类
        /// </summary>
        private LogDAL dal = new LogDAL();

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log">新日志信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Route("AddLog")]
        public IHttpActionResult AddAccountSYS(LogEntity log)
        {
            bool isAddSuccess = false;
            ResultEntity result = new ResultEntity();
            try
            {
                LOG temp = log.ToLOG();
                isAddSuccess = dal.AddLog(temp);
                result.Data = log;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/Log/AddLog异常，异常信息为：" + e.Message);
            }

            result.IsSuccess = isAddSuccess;
            return Json<ResultEntity>(result);
        }
    }
}
