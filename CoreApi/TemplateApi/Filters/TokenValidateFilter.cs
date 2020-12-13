using TemplateApi.Auth;
using TemplateApi.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TemplateApi.Filters
{
    /// <summary>
    /// 读取Token
    /// </summary>
    public class TokenValidateFilter : ActionFilterAttribute, IActionFilter, IOrderedFilter
    {
        /// <summary>
        /// 越小优先级越高
        /// </summary>
        public new int Order => -99;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as BaseApiController;
            var httpContext = controller.HttpContext;
            //Http头部获取Authorization
            var authorization = httpContext.Request.Headers["Authorization"].ToList();
            if (authorization != null && authorization.Count > 0)
            {
                //带请求头
                var key = authorization[0];
                var token = Provider.ValidateToken(key);
                if (token != null)
                {
                    var identity = new Identity(token);
                    var principal = new ClaimsPrincipal(identity);
                    httpContext.User = principal;
                }
            }
            //base.OnActionExecuting(context);
        }
    }
}