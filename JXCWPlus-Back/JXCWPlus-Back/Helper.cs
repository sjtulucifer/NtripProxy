using JXCWPlus_Back.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JXCWPlus_Back
{
    /// <summary>
    /// JXCW帮助类
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 检查批量点文件是否符合规则
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static int CheckTxtHighLevelList(string txt)
        {
            int result = 0;
            string[] contentArray = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            if (contentArray.Length > 0)
            {
                //去除第一行为表头
                for (int i = 1; i < contentArray.Length; i++)
                {
                    string[] txtContent = contentArray[i].Split(',');
                    //判断是否有大于3个逗号
                    if (txtContent.Length == 4)
                    {
                        //double temp = 0;
                        if (!double.TryParse(txtContent[1], out double temp1) &&
                            !double.TryParse(txtContent[2], out double temp2) &&
                            !double.TryParse(txtContent[3], out double temp3))
                        {
                            result = i + 1;
                        }
                    }
                    else
                    {
                        result = i + 1;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 将格式文件字符转换成HighLevel列表
        /// </summary>
        /// <param name="txt">格式文件字符</param>
        /// <returns>高程对象列表</returns>
        public static List<HighLevel> ConvertTxtToHighLevelList(string txt)
        {
            List<HighLevel> result = new List<HighLevel>();
            string[] contentArray = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (contentArray.Length > 0)
            {
                //去除第一行为表头
                for (int i = 1; i < contentArray.Length; i++)
                {
                    HighLevel temp = ConvertTxtToHighLevel(contentArray[i]);
                    result.Add(temp);
                }
            }
            return result;
        }

        /// <summary>
        /// 将HighLevel列表转换成格式文件字符
        /// </summary>
        /// <param name="highLevels">高程点列表</param>
        /// <returns>高程对象列表</returns>
        public static string ConvertHighLevelListToTxt(List<HighLevel> highLevels)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("点名,纬度,经度,大地高,水准高\r\n");
            foreach (var highLevel in highLevels)
            {
                sb.AppendFormat("{0},", highLevel.Name);
                sb.AppendFormat("{0},", highLevel.Lat);
                sb.AppendFormat("{0},", highLevel.Lon);
                sb.AppendFormat("{0},", highLevel.HIGH);
                sb.AppendFormat("{0} \r\n", highLevel.high);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 通过EGM08计算单个水准高
        /// </summary>
        /// <param name="highLevel">未计算水准高对象</param>
        /// <returns>计算完的水准高对象</returns>
        public static HighLevel ComputeHighLevel(HighLevel highLevel)
        {
            HighLevel result = new HighLevel();
            result.Name = highLevel.Name;
            result.Lon = highLevel.Lon;
            result.Lat = highLevel.Lat;
            result.HIGH = highLevel.HIGH;
            //Task.Delay(10000);
            result.high = highLevel.HIGH * 100;
            return result;
        }

        /// <summary>
        /// 通过EGM08计算水准高列表
        /// </summary>
        /// <param name="highLevels">待计算的水准高列表</param>
        /// <returns>计算后的结果</returns>
        public static List<HighLevel> ComputeHighLevels(List<HighLevel> highLevels)
        {
            List<HighLevel> result = new List<HighLevel>();
            foreach (var item in highLevels)
            {
                //计算过程
                result.Add(ComputeHighLevel(item));
            }
            return result;
        }

        /// <summary>
        /// 输入非水准拟合文本生成水准拟合文本
        /// </summary>
        /// <param name="inputTxt">非水准拟合文本</param>
        /// <returns>水准拟合结果文本</returns>
        public static string ComputeTxtToTxt(string inputTxt)
        {
            string result = string.Empty;
            List<HighLevel> highLevels = ConvertTxtToHighLevelList(inputTxt);
            result = ConvertHighLevelListToTxt(ComputeHighLevels(highLevels));
            return result;
        }

        /// <summary>
        /// 将格式字符串转换为HighLevel
        /// </summary>
        /// <param name="txt">单行高程点</param>
        /// <returns>高程对象</returns>
        private static HighLevel ConvertTxtToHighLevel(string txt)
        {
            HighLevel result = new HighLevel();
            string[] txtContent = txt.Split(',');
            if (txtContent.Length > 0)
            {
                result.Name = txtContent[0];
                result.Lat = double.Parse(txtContent[1]);
                result.Lon = double.Parse(txtContent[2]);
                result.HIGH = double.Parse(txtContent[3]);
                //上传的批量点数据没有水准高
                //result.high = double.Parse(txtContent[4]);
            }
            return result;
        }


    }
}