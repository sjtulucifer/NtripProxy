using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtripProxy.WebApi.Models
{
    /// <summary>
    /// WebApi返回的结果实体
    /// </summary>
    public class ResultEntity
    {
        /// <summary>
        /// WebApi是否返回成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 异常信息，无异常为空
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回的数据主体
        /// </summary>
        public object Data { get; set; }
    }
}