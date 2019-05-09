using JXCWPlus_Back.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace JXCWPlus_Back.Controllers
{
    /// <summary>
    /// 水准拟合控制类
    /// </summary>
    [RoutePrefix("api/highLevel")]
    public class HighLevelController : ApiController
    {
        /// <summary>
        /// 水准拟合单个高程点
        /// </summary>
        /// <param name="highLevel">待拟合的高程点</param>
        /// <returns>拟合后的高程点</returns>
        [HttpPost]
        [Route("ComputeHighLevel")]
        public IHttpActionResult ComputeHighLevel(HighLevel highLevel)
        {
            HighLevel result = new HighLevel();
            result = Helper.ComputeHighLevel(highLevel);
            return Json(result);
        }

        /// <summary>
        /// 上传水准高层拟合文件
        /// </summary>
        /// <param name="fileName">上传到服务器的文件名</param>
        /// <returns>上传文件错误行数，0为无错误</returns>
        [HttpPost]
        [Route("UpLoadHighLevelFile/{fileName}")]
        public IHttpActionResult UpLoadHighLevelFile(string fileName)
        {
            //文件第几行出错
            int result = 0;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var fileStream = httpRequest.Files[0].InputStream;
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    string txt = sr.ReadToEnd();
                    result = Helper.CheckTxtHighLevelList(txt);
                    if (result == 0)
                    {
                        //将上传文件转化成高程点列表
                        List<HighLevel> highLevels = Helper.ConvertTxtToHighLevelList(txt);
                        string uploadFolderPath = HttpContext.Current.Server.MapPath("~/App_Data/Upload");
                        //如果路径不存在，创建路径
                        if (!Directory.Exists(uploadFolderPath))
                            Directory.CreateDirectory(uploadFolderPath);
                        string uploadFilePath = uploadFolderPath + "/" + fileName + ".txt";
                        //List<HighLevel> listHighLevel = Helper.ConvertTxtToHighLevelList(txt);
                        //List<HighLevel> listHighLevelResult = Helper.ComputeHighLevel(listHighLevel);
                        //string txtResult = Helper.ConvertHighLevelListToTxt(listHighLevelResult);
                        byte[] myByte = Encoding.UTF8.GetBytes(txt);
                        using (FileStream fsWrite = new FileStream(uploadFilePath, FileMode.Create))
                        {
                            fsWrite.Write(myByte, 0, myByte.Length);
                        };
                    }
                }
            }
            return Json<int>(result);
        }

        /// <summary>
        /// 通过文件名计算水准高层拟合文件
        /// </summary>
        /// <param name="fileName">计算的服务器文件名</param>
        /// <returns>计算的结果csv文件字符</returns>
        [HttpGet]
        [Route("ComputeHighLevelFile/{fileName}")]
        public IHttpActionResult ComputeHighLevelFile(string fileName)
        {
            //结果文件字符
            string result = string.Empty;
            string uploadFolderPath = HttpContext.Current.Server.MapPath("~/App_Data/Upload");
            string uploadFilePath = uploadFolderPath + "/" + fileName + ".txt";
            using (StreamReader rs = new StreamReader(uploadFilePath))
            {
                string txt = rs.ReadToEnd();
                result = Helper.ComputeTxtToTxt(txt);
                byte[] myByte = Encoding.UTF8.GetBytes(result);
                string computeFilePath = uploadFolderPath + "/" + fileName + "(Computed).txt";
                using (FileStream fsWrite = new FileStream(computeFilePath, FileMode.Create))
                {
                    fsWrite.Write(myByte, 0, myByte.Length);
                };
            }
            return Json(result);
        }

    }
}
