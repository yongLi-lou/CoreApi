using Qj.Dto.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateApi.Filters
{
    public class APIResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //传入参数验证
                StringBuilder sb = new StringBuilder();
                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        sb.Append(error.ErrorMessage).Append(';');
                        if (sb.Length == 1) sb.Append(error.Exception == null ? "" : error.Exception.Message).Append(';');
                        break;
                    }
                }
                var result = new OperateResponse(ResponseType.FAIL);
                result.msg = sb.ToString();
                context.Result = new JsonResult(result);
            }
        }
    }
}