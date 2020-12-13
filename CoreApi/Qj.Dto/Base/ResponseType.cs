using System;
using System.Collections.Generic;
using System.Text;

namespace Qj.Dto.Base
{
    /// <summary>
    /// 结果
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCCESS = 0,

        /// <summary>
        /// 失败
        /// </summary>
        FAIL = 1,

        /// <summary>
        /// 异常
        /// </summary>
        ERROR = 2,

        /// <summary>
        /// 认证失败
        /// </summary>
        Unauthorized = 401
    }
}