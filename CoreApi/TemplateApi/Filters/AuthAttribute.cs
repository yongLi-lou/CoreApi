using TemplateApi.Auth;
using TemplateApi.Common;
using Qj.Dto.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TemplateApi.Filters
{
    /// <summary>
    /// 权限判断
    /// </summary>
    public class AuthAttribute : ActionFilterAttribute, IActionFilter
    {

        public AuthAttribute()
        {
            Validate = UserRoleEnum.Other;
        }

        public AuthAttribute(UserRoleEnum type)
        {
            Validate = type;
        }

        public HttpContent StringContent { get; private set; }

        /// <summary>
        /// 用户权限
        /// </summary>
        public UserRoleEnum Validate { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as BaseApiController;
            var httpContext = controller.HttpContext;

            string controllerName = context.ActionDescriptor.RouteValues["controller"];
            string actionName = context.ActionDescriptor.RouteValues["action"];

            if (controllerName == "")
            {
                return;
            }
            else
            {

                if (!(context.HttpContext.User.Identity is Identity))
                {
                    var result = new JsonResult(new OperateResponse(ResponseType.Unauthorized));
                    context.Result = result;
                    return;
                }

                //权限判断
                if(Validate!= UserRoleEnum.Other)
                {
                    List<UserRoleEnum> listRole = ((Identity)context.HttpContext.User.Identity).CurrentUser.UserRole;
                    if (!listRole.Contains(Validate))
                    {
                        var result = new JsonResult(new OperateResponse(ResponseType.Unauthorized));
                        context.Result = result;
                        return;
                    }

                }



               
            }
        }
    }
}