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
    /// <summary>
    /// GGA历史数据Web操作类
    /// </summary>
    [RoutePrefix("api/GGAHistory")]
    public class GGAHistoryController : ApiController
    {
        /// <summary>
        /// GGA历史数据数据库操作类
        /// </summary>
        private GGAHistoryDAL dal = new GGAHistoryDAL();

        /// <summary>
        /// 查找指定账号名的gga信息
        /// </summary>
        /// <param name="account">账号名</param>
        /// <returns>查找到的GGA信息列表,时间倒叙排列</returns>
        [HttpGet]
        [Route("GetGGAHistoryByAccount/{account}")]
        public IHttpActionResult GetGGAHistoryByAccount(string account)
        {
            List<GGAHistoryEntity> ggaEntityList = new List<GGAHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                List<GGAHistory> temp = dal.FindGGAHistoriesByAccount(account);
                foreach (var ggaHistory in temp)
                {
                    GGAHistoryEntity item = ggaHistory.ToGGAHistoryEntity();
                    item.Session = ggaHistory.SessionHistory.ToSessionHistoryEntity();
                    ggaEntityList.Add(item);
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/GGAHistory/GetGGAHistoryByAccount异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = ggaEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 通过账号名查找账号的最新在线概略位置
        /// </summary>
        /// <param name="account">账号名</param>
        /// <returns>查找到的GGA信息</returns>
        [HttpGet]
        [Route("GetOnlineNewestGGAByAccount/{account}")]
        public IHttpActionResult GetOnlineNewestGGAByAccount(string account)
        {
            GGAHistoryEntity ggaEntity = new GGAHistoryEntity();

            ResultEntity result = new ResultEntity();
            try
            {
                //找到指定账号的最近gga信息
                GGAHistory temp = dal.FindGGAHistoriesByAccount(account)[0];
                //判断GGA相关会话是否在线
                if(temp.SessionHistory.ConnectionEnd == null)
                {
                    ggaEntity = temp.ToGGAHistoryEntity();
                    ggaEntity.Session = temp.SessionHistory.ToSessionHistoryEntity();
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/GGAHistory/GetOnlineNewestGGAByAccount异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = ggaEntity;
            return Json<ResultEntity>(result);
        }
    }
}
