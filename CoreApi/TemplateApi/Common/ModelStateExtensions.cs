using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace TemplateApi
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// 汇总错误信息
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string ExpendErrors(this ControllerBase controller)
        {
            var sbErrors = new StringBuilder();
            foreach (var item in controller.ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    for (int i = item.Errors.Count - 1; i >= 0; i--)
                    {
                        sbErrors.Append(!string.IsNullOrWhiteSpace(item.Errors[i].ErrorMessage) ? item.Errors[i].ErrorMessage : item.Errors[i].Exception.Message);
                    }
                }
            }
            return sbErrors.ToString();
        }
    }
}