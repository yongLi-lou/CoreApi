using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using TemplateApi.Common;
using Qj.Utility;
using TemplateApi.Auth;
using Qj.Core;
using Qj.Models;

namespace TemplateApi
{
    /// <summary>
    /// 日志属性
    /// </summary>
    public class LogAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LogAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogAttribute(string title)
        {
            LogTitle = title;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogAttribute(string title, string modelName) : this(title)
        {
            ModelName = modelName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogAttribute(string title, string modelName, string propName) : this(title, modelName)
        {
            PropName = propName;
        }

        /// <summary>
        /// 日志标题
        /// </summary>
        public string LogTitle { get; set; }

        /// <summary>
        /// 模型名
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropName { get; set; }

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

                string text = "";

                if (httpContext.Request.QueryString.ToString().Length > 0)
                {
                    text = httpContext.Request.QueryString.ToString();
                }
                else
                {
                    foreach (string item in context.ActionArguments.Keys)
                    {
                        if (context.ActionArguments.ContainsKey(item))
                        {
                            text = context.ActionArguments[item].ToJson();
                        }
                        break;
                    }
                }


                //插入数据库操作
                var logDomain = new Sys_OperationLogDoMain();

                var logModel = new Sys_OperationLog
                {
                    ActionName = actionName,
                    ControllerName = controllerName,
                    ModuleName = LogTitle,
                    ActionParameters = text,
                    IP = Ip.ToString(),
                    Option = LogTitle,
                };

                if (Identity != null && (Identity is Identity) && ((Identity)Identity).CurrentUser != null)
                {
                    logModel.UserID = ((Identity)Identity).CurrentUser.ID;
                    logModel.Name = ((Identity)Identity).CurrentUser.Name;
                }

                logDomain.Log(logModel);
            }
            catch (Exception ex)
            {
            }
            base.OnActionExecuting(context);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
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

                string text = "";

                if (httpContext.Request.QueryString.ToString().Length > 0)
                {
                    text = httpContext.Request.QueryString.ToString();
                }
                else
                {

                    //foreach (string item in context.ActionDescriptor.Keys)
                    //{
                    //    if (context.ActionArguments.ContainsKey(item))
                    //    {
                    //        text = context.ActionArguments[item].ToJson();
                    //    }
                    //    break;
                    //}
                }

                var textresult = context.Result.ToJson();
                //插入数据库操作
                var logDomain = new Sys_OperationLogDoMain();

                var logModel = new Sys_OperationLog
                {
                    ActionName = actionName,
                    ControllerName = controllerName,
                    ModuleName = LogTitle,
                    ActionParameters = text,
                    IP = Ip.ToString() + "end",
                    Option = LogTitle,
                    LogResult = textresult,
                };

                if (Identity != null && (Identity is Identity) && ((Identity)Identity).CurrentUser != null)
                {
                    logModel.UserID = ((Identity)Identity).CurrentUser.ID;
                    logModel.Name = ((Identity)Identity).CurrentUser.Name;
                }

                logDomain.Log(logModel);
            }
            catch (Exception ex)
            {
            }
            base.OnActionExecuted(context);
        }


        /// <summary>
        /// 过滤属性日志记录
        /// </summary>
        /// <returns></returns>
        private bool FilterPropName(string propName)
        {
            List<string> listFilter = new List<string> { "IsDelete", "InDate", "InUser", "EditDate", "EditUser" };

            return listFilter.Contains(PropName);
        }
    }
}