using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApi.Models
{
    /// <summary>
    /// 审核请求
    /// </summary>
    public class AuditRequest
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Opinion { get; set; }
    }
}