using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qj.Dto
{
    /// <summary>
    /// 泛型Dto基类
    /// </summary>
    public abstract class StrBaseDataDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual string ID { get; set; }
   
        /// <summary>
        /// 是否删除
        /// </summary>
        [DisplayName("是否删除")]
        public bool IsDelete { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        [DisplayName("录入时间")]
        public long? InDate { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        [DisplayName("录入人")]
        public string InUser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DisplayName("修改时间")]
        public long? EditDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [DisplayName("修改人")]
        public string EditUser { get; set; }
    }
}