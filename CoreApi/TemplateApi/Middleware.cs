using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TemplateApi
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AAAMiddleware
    {
        private readonly RequestDelegate _next;

        public AAAMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine(1);
            var t = _next(httpContext);
            Console.WriteLine(2);
            return t;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAAAMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AAAMiddleware>();
        }
    }
}
