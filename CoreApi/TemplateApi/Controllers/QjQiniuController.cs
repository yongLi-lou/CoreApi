using TemplateApi.Common;
using Microsoft.AspNetCore.Mvc;
using Qj.Utility;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace TemplateApi
{
    /// <summary>
    /// nuget 引用Qiniu
    /// </summary>
    [Route("api/QiNiu")]
    [ApiController]
    public class QjQiniuController : BaseApiController
    {
        /// <summary>
        /// 创建私库上传token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPrivateToken")]
        public ApiJsonResult GetPrivateToken()
        {
            //var mac = new Mac("K8B1R3TwxhFW7yevlWiQRKeagxUrbimLd51KbpiP", "0UkA0bjK4FO2g6wHj282pHHG6AcW5V369MniL6E7");
            //var putPolicy = new PutPolicy();
            //putPolicy.Scope = "qjbucket";
            //putPolicy.SetExpires(7200);

            //string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            return JsonSuccess(QiNiuHelper.GetPrivateToken());
        }

        /// <summary>
        /// 创建公库上传token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPublicToken")]
        public ApiJsonResult GetPublicToken()
        {
            //var mac = new Mac("K8B1R3TwxhFW7yevlWiQRKeagxUrbimLd51KbpiP", "0UkA0bjK4FO2g6wHj282pHHG6AcW5V369MniL6E7");
            //var putPolicy = new PutPolicy();
            //putPolicy.Scope = "qjpublic";
            //putPolicy.SetExpires(7200);

            //string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            //return JsonSuccess(token);

            return JsonSuccess(QiNiuHelper.GetPublicToken());
        }

        /// <summary>
        /// 创建私有图片地址
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPrivateUrl")]
        public ApiJsonResult GetPrivateUrl(string key)
        {
            ////"FqCuFHoyT2VxMWwwH0NEuM5Jyycs"
            //Mac mac = new Mac("K8B1R3TwxhFW7yevlWiQRKeagxUrbimLd51KbpiP", "0UkA0bjK4FO2g6wHj282pHHG6AcW5V369MniL6E7");
            //string domain = "https://qny.qjnice.com";
            //string privateUrl = DownloadManager.CreatePrivateUrl(mac, domain, key, 3600);

            return JsonSuccess(QiNiuHelper.GetPrivateUrl(key));
        }

        /// <summary>
        /// 创建公共图片地址
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPublicUrl")]
        public ApiJsonResult GetPublicUrl(string key)
        {
            //string publicUrl = string.Format("{0}/{1}", "https://qnc.qjnice.com", key);
            //return JsonSuccess(publicUrl);

            return JsonSuccess(QiNiuHelper.GetPublicUrl(key));
        }



        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        [HttpGet, Route("UploadFile")]
        public string UploadFile(byte[] byteData, string FileName)
        {
            var tokens = GetPublicToken();
            string token = tokens.data.ToString();

            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("token", token));
            var content = new FormUrlEncodedContent(list);
            var bufferContent = new ByteArrayContent(byteData);
            var client = new HttpClient();
            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new StringContent(token), "token");
            multipartFormDataContent.Add(bufferContent, "file");
            var resp = client.PostAsync("https://upload.qiniup.com", multipartFormDataContent).Result;
            var body = resp.Content.ReadAsStringAsync().Result.FromJson<ImgResponse>().key + $"?attname= {FileName}";
            return "https://qnc.qjnice.com/" + body;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        [HttpPost, Route("UploadFilePath")]
        public string UploadFilePath(string FileName)
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return "请选择文件";
            }
            var excelfile = files[0];

            var steam = excelfile.OpenReadStream(); 
            byte[] byteData = new byte[steam.Length];
            steam.Read(byteData, 0, byteData.Length);
            steam.Close();

            var tokens = GetPublicToken();
            string token = tokens.data.ToString();

            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("token", token));
            var content = new FormUrlEncodedContent(list);
            var bufferContent = new ByteArrayContent(byteData);
            var client = new HttpClient();
            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new StringContent(token), "token");
            multipartFormDataContent.Add(bufferContent, "file");
            var resp = client.PostAsync("https://upload.qiniup.com", multipartFormDataContent).Result;
            var body = resp.Content.ReadAsStringAsync().Result.FromJson<ImgResponse>().key + $"?attname= {FileName}";
            return "https://qnc.qjnice.com/" + body;
        }


    }


    /// <summary>
    /// 上传文件返回
    /// </summary>
    public class ImgResponse
    {
        /// <summary>
        ///
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string key { get; set; }
    }
}