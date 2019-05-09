using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JXCWPlus_Back.DTOs
{
    /// <summary>
    /// 测试csv的数据交换类
    /// </summary>
    public class TestCsv
    {
        /// <summary>
        /// 字符串字段
        /// </summary>
        public string StringTest { get; set; }

        /// <summary>
        /// 整形字段
        /// </summary>
        public int IntTest { get; set; }

        /// <summary>
        /// 单精度字段
        /// </summary>
        public float FloatTest { get; set; }

        /// <summary>
        /// 双精度字段
        /// </summary>
        public double DoubleTest { get; set; }

        /// <summary>
        /// 时间字段
        /// </summary>
        public DateTime DateTimeTest { get; set; }

        /// <summary>
        /// 布尔字段
        /// </summary>
        public bool BoolTest { get; set; }
    }
}