using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JXCWPlus_Back.DTOs
{
    /// <summary>
    /// 高层点
    /// </summary>
    public class HighLevel
    {
        /// <summary>
        /// 点名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lon { get; set; }
        /// <summary>
        /// 大地高(相对高度)
        /// </summary>
        public double HIGH { get; set; }
        /// <summary>
        /// 水准高(海拔高度)
        /// </summary>
        public double high { get; set; }
    }
}