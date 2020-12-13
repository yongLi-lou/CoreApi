using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApi.Models.Filter
{
    /// <summary>
    /// 补齐补正日志查询条件
    /// </summary>
    public class ReplenishLogFilter
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long? startDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long? endDate { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 是否已补齐补正
        /// </summary>
        public bool IsRelenish { get; set; }
    }
}