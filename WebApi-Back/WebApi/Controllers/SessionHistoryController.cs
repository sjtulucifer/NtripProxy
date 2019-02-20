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
    /// 会话历史Web操作类
    /// </summary>
    [RoutePrefix("api/SessionHistory")]
    public class SessionHistoryController : ApiController
    {
        /// <summary>
        /// 会话历史数据数据库操作类
        /// </summary>
        private SessionHistoryDAL dal = new SessionHistoryDAL();
        /// <summary>
        /// 概略位置数据库操作类
        /// </summary>
        private GGAHistoryDAL ggaDAL = new GGAHistoryDAL();
        /// <summary>
        /// 用户数据库操作类
        /// </summary>
        private UserDAL userDAL = new UserDAL();
        /// <summary>
        /// 公司数据库操作类
        /// </summary>
        private CompanyDAL companyDAL = new CompanyDAL();

        /// <summary>
        /// 查找当前在线的会话
        /// </summary>
        /// <returns>当前在线会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSession")]
        public IHttpActionResult GetOnlineSession()
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                foreach (var sessionHistory in temp)
                {
                    SessionHistoryEntity item = sessionHistory.ToSessionHistoryEntity();
                    item.GGAHistories = sessionHistory.GGAHistories.ToList<GGAHistory>().ConvertAll<GGAHistoryEntity>(g => g.ToGGAHistoryEntity());
                    sessionEntityList.Add(item);
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSession异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 查找指定用户ID可查看的当前在线的会话
        /// </summary>
        /// <returns>当前在线会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSessionByUserID/{id}")]
        public IHttpActionResult GetOnlineSessionByUserID(string id)
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                foreach (var sessionHistory in temp)
                {
                    SessionHistoryEntity item = sessionHistory.ToSessionHistoryEntity();
                    item.GGAHistories = sessionHistory.GGAHistories.ToList<GGAHistory>().ConvertAll<GGAHistoryEntity>(g => g.ToGGAHistoryEntity());
                    List<string> accountNameList = this.getAccountNameListByUserID(id);
                    if (accountNameList.Contains(item.AccountName))
                    {
                        sessionEntityList.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionByUserID异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 查找当前在线的会话的总固定率
        /// </summary>
        /// <returns>当前在线会话的总固定率</returns>
        [HttpGet]
        [Route("GetOnlineSessionFixedRate")]
        public IHttpActionResult GetOnlineSessionFixedRate()
        {
            double rate = 0;
            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                int ggaCount = 0;
                int fixedCount = 0;
                foreach (var session in temp)
                {
                    ggaCount += (int)session.GGACount;
                    fixedCount += (int)session.FixedCount;
                }
                if (ggaCount > 0)
                {
                    rate = (double)fixedCount / (double)ggaCount;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSession异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = rate.ToString("P");
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 根据指定ID的用户查找可查看的当前在线会话的总固定率
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>总固定率</returns>
        [HttpGet]
        [Route("GetOnlineSessionFixedRateByUserID/{id}")]
        public IHttpActionResult GetOnlineSessionFixedRateByUserID(string id)
        {
            double rate = 0;
            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                List<string> accountNameList = this.getAccountNameListByUserID(id);
                int ggaCount = 0;
                int fixedCount = 0;
                foreach (var session in temp)
                {
                    if (accountNameList.Contains(session.AccountName))
                    {
                        ggaCount += (int)session.GGACount;
                        fixedCount += (int)session.FixedCount;
                    }
                }
                if (ggaCount > 0)
                {
                    rate = (double)fixedCount / (double)ggaCount;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionFixedRateByUserID异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = rate.ToString("P");
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 查找指定账号名的历史会话数据
        /// </summary>
        /// <param name="account">账号名</param>
        /// <returns>查找到的GGA信息列表,时间倒叙排列</returns>
        [HttpGet]
        [Route("GetSessionHistoryByAccount/{account}")]
        public IHttpActionResult GetSessionHistoryByAccount(string account)
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                sessionEntityList = dal.FindSessionHistoriesByAccount(account).ConvertAll<SessionHistoryEntity>(s => s.ToSessionHistoryEntity());
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetSessionHistoryByAccount异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 查找指定定位状态的在线会话
        /// </summary>
        /// <param name="status">定位状态,1单点解,2差分解,4固定解,5浮点解,其余值为其他解</param>
        /// <returns>找到的会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSessionByStatus/{status}")]
        public IHttpActionResult GetOnlineSessionByStatus(int status)
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                foreach (var sessionHistory in temp)
                {
                    //必须为非第三方账号
                    if (sessionHistory.GGAHistories != null && sessionHistory.GGAHistories.Count > 0)
                    {
                        if (ggaDAL.FindGGAHistoriesBySessionHistory(sessionHistory)[0].Status == status)
                        {
                            sessionEntityList.Add(sessionHistory.ToSessionHistoryEntity());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionByStatus异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 根据指定ID的用户查找可查看的固定状态为固定解的在线会话
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>找到的会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSessionFixedByUserID/{id}")]
        public IHttpActionResult GetOnlineSessionFixedByUserID(string id)
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                List<string> accountNameList = this.getAccountNameListByUserID(id);
                foreach (var sessionHistory in temp)
                {
                    //必须有查看资格,必须为非第三方账号
                    if (accountNameList.Contains(sessionHistory.AccountName) && sessionHistory.GGAHistories != null && sessionHistory.GGAHistories.Count > 0)
                    {
                        //固定解
                        if (ggaDAL.FindGGAHistoriesBySessionHistory(sessionHistory)[0].Status == 4)
                        {
                            sessionEntityList.Add(sessionHistory.ToSessionHistoryEntity());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionFixedByUserID异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 根据指定ID的用户查找可查看的固定状态为浮点解的在线会话
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>找到的会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSessionFloatByUserID/{id}")]
        public IHttpActionResult GetOnlineSessionFloatByUserID(string id)
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                List<string> accountNameList = this.getAccountNameListByUserID(id);
                foreach (var sessionHistory in temp)
                {
                    //必须有查看资格,必须为非第三方账号
                    if (accountNameList.Contains(sessionHistory.AccountName) && sessionHistory.GGAHistories != null && sessionHistory.GGAHistories.Count > 0)
                    {
                        //固定解
                        if (ggaDAL.FindGGAHistoriesBySessionHistory(sessionHistory)[0].Status == 5)
                        {
                            sessionEntityList.Add(sessionHistory.ToSessionHistoryEntity());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionFloatByUserID异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 根据指定ID的用户查找可查看的固定状态为浮点解的在线会话
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>找到的会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSessionSingleByUserID/{id}")]
        public IHttpActionResult GetOnlineSessionSingleByUserID(string id)
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                List<string> accountNameList = this.getAccountNameListByUserID(id);
                foreach (var sessionHistory in temp)
                {
                    //必须有查看资格,必须为非第三方账号
                    if (accountNameList.Contains(sessionHistory.AccountName) && sessionHistory.GGAHistories != null && sessionHistory.GGAHistories.Count > 0)
                    {
                        //固定解
                        if (ggaDAL.FindGGAHistoriesBySessionHistory(sessionHistory)[0].Status == 1)
                        {
                            sessionEntityList.Add(sessionHistory.ToSessionHistoryEntity());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionSingleByUserID异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 根据指定ID的用户查找可查看的固定状态为差分解的在线会话
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>找到的会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSessionDifferenceByUserID/{id}")]
        public IHttpActionResult GetOnlineSessionDifferenceByUserID(string id)
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();

            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                List<string> accountNameList = this.getAccountNameListByUserID(id);
                foreach (var sessionHistory in temp)
                {
                    //必须有查看资格,必须为非第三方账号
                    if (accountNameList.Contains(sessionHistory.AccountName) && sessionHistory.GGAHistories != null && sessionHistory.GGAHistories.Count > 0)
                    {
                        //固定解
                        if (ggaDAL.FindGGAHistoriesBySessionHistory(sessionHistory)[0].Status == 2)
                        {
                            sessionEntityList.Add(sessionHistory.ToSessionHistoryEntity());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionDifferenceByUserID异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 查找其他解定位状态的在线会话
        /// </summary>
        /// <returns>找到的会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSessionByStatusOther")]
        public IHttpActionResult GetOnlineSessionByStatusOther()
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();
            //不包含的定位状态
            int[] statusExcpet = { 1, 2, 4, 5 };
            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                foreach (var sessionHistory in temp)
                {
                    //非第三方账号
                    if (sessionHistory.GGAHistories != null && sessionHistory.GGAHistories.Count > 0)
                    {
                        if (!statusExcpet.Contains((int)ggaDAL.FindGGAHistoriesBySessionHistory(sessionHistory)[0].Status))
                        {
                            sessionEntityList.Add(sessionHistory.ToSessionHistoryEntity());
                        }
                    }
                    //第三方账号
                    else
                    {
                        sessionEntityList.Add(sessionHistory.ToSessionHistoryEntity());
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionByStatusOther异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 根据指定ID的用户查找可查看的固定状态为其他解的在线会话
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>找到的会话列表</returns>
        [HttpGet]
        [Route("GetOnlineSessionOtherByUserID/{id}")]
        public IHttpActionResult GetOnlineSessionOtherByUserID(string id)
        {
            List<SessionHistoryEntity> sessionEntityList = new List<SessionHistoryEntity>();
            //不包含的定位状态
            int[] statusExcpet = { 1, 2, 4, 5 };
            ResultEntity result = new ResultEntity();
            try
            {
                List<SessionHistory> temp = dal.FindAllOnlineSessions();
                List<string> accountNameList = this.getAccountNameListByUserID(id);
                foreach (var sessionHistory in temp)
                {
                    //必须有查看资格,必须为非第三方账号
                    if (accountNameList.Contains(sessionHistory.AccountName) && sessionHistory.GGAHistories != null && sessionHistory.GGAHistories.Count > 0)
                    {
                        if (!statusExcpet.Contains((int)ggaDAL.FindGGAHistoriesBySessionHistory(sessionHistory)[0].Status))
                        {
                            sessionEntityList.Add(sessionHistory.ToSessionHistoryEntity());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                NtripProxyLogger.LogExceptionIntoFile("调用接口api/SessionHistory/GetOnlineSessionOtherByUserID异常，异常信息为：" + e.Message);
            }
            result.IsSuccess = result.Message == null;
            result.Data = sessionEntityList;
            return Json<ResultEntity>(result);
        }

        /// <summary>
        /// 辅助函数，将公司列表转换为公司列表包含的所有账号名信息
        /// </summary>
        /// <param name="listCompany">公司列表</param>
        /// <returns>账号名列表</returns>
        private List<string> convertCompanyToAccountNameList(List<COMPANY> listCompany)
        {
            List<string> result = new List<string>();
            foreach (var company in listCompany)
            {
                if (company.ACCOUNTs != null)
                {
                    foreach (var account in company.ACCOUNTs)
                    {
                        if (!result.Contains(account.Account_Name))
                        {
                            result.Add(account.Account_Name);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 辅助函数，通过用户ID号查找出该用户可查看的账户名列表
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>账号名列表</returns>
        private List<string> getAccountNameListByUserID(string id)
        {
            List<string> result = new List<string>();
            USER user = this.userDAL.FindUserByID(new Guid(id));
            List<COMPANY> companyList = new List<COMPANY>();
            if (user.User_Campany != null)
            {
                companyList = this.companyDAL.FindCompanyAndAllSubCopaniesByID((Guid)user.User_Campany);
            }
            foreach (var company in companyList)
            {
                if (company.ACCOUNTs != null)
                {
                    foreach (var account in company.ACCOUNTs)
                    {
                        if (!result.Contains(account.Account_Name))
                        {
                            result.Add(account.Account_Name);
                        }
                    }
                }
            }
            return result;
        }
    }
}
