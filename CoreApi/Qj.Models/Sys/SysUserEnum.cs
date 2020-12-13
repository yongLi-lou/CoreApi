using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Qj.Models
{
    /// <summary>
    /// 用户角色枚举
    /// </summary>
    public enum SysUserRole
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        User = 0,

        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 99

       
    }
}