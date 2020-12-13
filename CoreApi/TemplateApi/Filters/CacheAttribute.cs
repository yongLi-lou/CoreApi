using Qj.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Qj.Utility.Helper;
using System.IO;
using System.Text;
using System.Web;
using Qj.Dto;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using TemplateApi.Common;
using TemplateApi.Auth;
using Qj.Dto.Base;
using Microsoft.AspNetCore.Mvc;

namespace TemplateApi
{
    /// <summary>
    /// 缓存属性
    /// </summary>
    public class CacheAttribute : ActionFilterAttribute
    {

        public CacheAttribute()
        {
        }

        public CacheAttribute(string cachetitle)
        {
            this.CacheTitle = cachetitle;
        }


        private string _cachetitle;
        /// <summary>
        /// 缓存标题
        /// </summary>
        public string CacheTitle
        {
            get
            {
                return string.IsNullOrEmpty(_cachetitle) ? "" : $"{_cachetitle}:";
            }
            set
            {
                this._cachetitle = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {

                var controller = context.Controller as BaseApiController;
                var httpContext = controller.HttpContext;

                var Identity = context.HttpContext.User.Identity;
                var Ip = httpContext.Connection.LocalIpAddress;
                string controllerName = context.ActionDescriptor.RouteValues["controller"];
                string actionName = context.ActionDescriptor.RouteValues["action"];

                string ccid = "";
                if (Identity != null && (Identity is Identity) && ((Identity)Identity).CurrentUser != null)
                {
                    ccid = ((Identity)Identity).CurrentUser.ID.ToString() + ":";
                }

                string url = httpContext.Request.Scheme.ToString();
                url += httpContext.Request.Host.ToString();
                url += httpContext.Request.Path.ToString();
                url += httpContext.Request.QueryString.ToString();
                if (httpContext.Request.Method == "GET")
                {
                    #region 查询Redis

                    string str = "";
                    string textJson = JsonConvert.SerializeObject(url);
                    var aes = new AESEncrypt();
                    var aesString = aes.Encrypt(textJson, StaticConfig.AESKey, StaticConfig.AESIV);
                    var temp = RedisCacheHelper.Get<dynamic>($"{StaticLock.CacheRedisKeyPrefix}{controllerName}:{actionName}:{ccid}{CacheTitle}{aesString}");

                    //清空以什么名称开始的KEY数据
                    //RedisCacheHelper.RemoveAllNameList($"{StaticLock.CacheRedisKeyPrefix}{CacheTitle}");

                    if (temp != null)
                    {
                        dynamic obj = JsonConvert.DeserializeObject<dynamic>(temp);
                        var result = new JsonResult(obj);
                        context.Result = result;
                        return;
                    }
                    #endregion
                }


            }
            catch (Exception ex)
            {
            }
            base.OnActionExecuting(context);
        }

        /// <summary>
        ///执行后
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var controller = context.Controller as BaseApiController;
                var httpContext = controller.HttpContext;

                var Identity = context.HttpContext.User.Identity;
                var Ip = httpContext.Connection.LocalIpAddress;
                string controllerName = context.ActionDescriptor.RouteValues["controller"];
                string actionName = context.ActionDescriptor.RouteValues["action"];

                string ccid = "";
                if (Identity != null && (Identity is Identity) && ((Identity)Identity).CurrentUser != null)
                {
                    ccid = ((Identity)Identity).CurrentUser.ID.ToString() + ":";
                }


                string url = httpContext.Request.Scheme.ToString();
                url += httpContext.Request.Host.ToString();
                url += httpContext.Request.Path.ToString();
                url += httpContext.Request.QueryString.ToString();
                string ResponseRemark = GetResponseValues(context);
                if (httpContext.Request.Method == "GET" && JsonConvert.DeserializeObject<dynamic>(ResponseRemark).code == "SUCCESS")
                {
                    #region 写入Redis
                    #region 加密成远程的key
                    string textJson = JsonConvert.SerializeObject(url);
                    var aes = new AESEncrypt();
                    var aesString = aes.Encrypt(textJson, StaticConfig.AESKey, StaticConfig.AESIV);

                    var temp = Provider.CreateCache($"{StaticLock.CacheRedisKeyPrefix}{controllerName}:{actionName}:{ccid}{CacheTitle}{aesString}", ResponseRemark);
                    //var temp = RedisCacheHelper.Get<List<dynamic>>(aesString);
                    #endregion
                    #endregion
                }

            }
            catch (Exception ex)
            {
            }
            base.OnActionExecuted(context);
        }



        /// <summary>
        /// 读取action返回的result
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetResponseValues(ActionExecutedContext context)
        {
            
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject<dynamic>(context.Result.ToJson()).Value);
        }


    }
}