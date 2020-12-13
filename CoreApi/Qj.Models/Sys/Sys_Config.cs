using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qj.Models
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    [Description("系统配置表")]
    public  class Sys_Config : IntBaseEntity
    {
        /// <summary>
        /// 数据编码
        /// </summary>
        [Description("数据编码")]
        public string DataCode { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        [Description("数据名称")]
        public string DataName { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        [Description("数据值")]
        public string DataValue { get; set; }

        /// <summary>
        /// 数据分组标识
        /// </summary>
        [Description("数据分组标识")]
        public string DataGroupTag { get; set; }

        /// <summary>
        /// 数据分组说明
        /// </summary>
        [Description("数据分组说明")]
        public string DataGroupMemo { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Description("排序号")]
        public int Sequence { get; set; }

   
    }

}