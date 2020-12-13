using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qj.Dto
{
    /// <summary>
    /// ID
    /// </summary>
    public class CommonRequest
    {
        /// <summary>
        /// ID
        /// </summary>
        public string[] IDS { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class CommonAndRemarkRequest
    {
        /// <summary>
        /// ID
        /// </summary>
        public string[] IDS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}