using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApi.Models.Filter
{
    /// <summary>
    /// 业务受理查询条件
    /// </summary>
    public class BusinessAcceptanceFilter
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// 每頁数量
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 事项名称
        /// </summary>
        public string eventName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long? startDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long? endDate { get; set; }

        /// <summary>
        /// 是否已受理
        /// </summary>
        public bool IsAccepance { get; set; }
    }
}