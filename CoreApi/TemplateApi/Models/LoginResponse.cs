using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApi.Models
{
    /// <summary>
    /// 登录返回
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// 登录Token
        /// </summary>
        public string Token { get; set; }
    }
}