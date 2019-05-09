using JXCWPlus_Back.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace JXCWPlus_Back.Controllers
{
    /// <summary>
    /// csv控制类
    /// </summary>
    [RoutePrefix("api/csv")]
    public class TestCsvController : ApiController
    {
        [HttpGet]
        [Route("GetCsv")]
        public IHttpActionResult GetCsv()
        {
            var records = GetCsvs(); //irrelevant
            var sb = new StringBuilder();

            sb.Append("StringTest,IntTest,FloatTest,DoubleTest,DateTimeTest,BoolTest\r\n");

            foreach (var record in records)
            {
                sb.AppendFormat("=\"{0}\",", record.StringTest);
                sb.AppendFormat("=\"{0}\",", record.IntTest);
                sb.AppendFormat("=\"{0}\",", record.FloatTest);
                sb.AppendFormat("=\"{0}\",", record.DoubleTest);
                sb.AppendFormat("=\"{0}\",", record.DateTimeTest);
                sb.AppendFormat("=\"{0}\"\r\n", record.BoolTest); //no comma for the last item, but a new line
            }
            /*
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StringContent(sb.ToString(),Encoding.Default);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment"); //attachment will force download
            result.Content.Headers.ContentDisposition.FileName = "CsvExport.csv";
            */
            return Json(sb.ToString());
        }

        private List<TestCsv> GetCsvs()
        {
            List<TestCsv> results = new List<TestCsv>();
            results.Add(new TestCsv
            {
                StringTest = "lucifer1",
                IntTest = 1,
                FloatTest = 1.1F,
                DoubleTest = 1.11,
                DateTimeTest = DateTime.Now,
                BoolTest = true
            }
            );
            results.Add(new TestCsv
            {
                StringTest = "lucifer2",
                IntTest = 2,
                FloatTest = 2.2F,
                DoubleTest = 2.22,
                DateTimeTest = DateTime.Now,
                BoolTest = false
            }
            );
            return results;
        }

        [HttpPost]
        [Route("PostCsv")]
        public HttpResponseMessage UploadFile()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    var fileStream = httpRequest.Files[0].InputStream;

                    using (StreamReader sr = new StreamReader(fileStream))
                    {
                        string text = sr.ReadToEnd();
                        string[] contentArray = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        List<TestCsv> listCSV = new List<TestCsv>();
                        for (int i = 1; i < contentArray.Length - 1; i++)
                        {
                            listCSV.Add(convertTxtToCsvList(contentArray[i]));
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
            return response;
        }

        /// <summary>
        /// 将格式字符串转换为TestCsv
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private TestCsv convertTxtToCsvList(string txt)
        {
            TestCsv result = new TestCsv();
            string[] txtContent = txt.Split(',');
            if (txtContent.Length == 6)
            {
                result.StringTest = txtContent[0];
                result.IntTest = int.Parse(txtContent[1]);
                result.FloatTest = float.Parse(txtContent[2]);
                result.DoubleTest = double.Parse(txtContent[3]);
                result.DateTimeTest = DateTime.Parse(txtContent[4]);
                result.BoolTest = bool.Parse(txtContent[5]);
            }
            return result;
        }
    }
}
