using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Qj.Dto.User
{
    /// <summary>
    /// SysUser登录
    /// </summary>
    public class SysUserLoginRequest
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "帐号验证错误"), Display(Name = "账号")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "账号必须是字母和数字组成")]
        public string userName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码验证错误"), Display(Name = "密码")]
        [StringLength(maximumLength: 16, MinimumLength = 3)]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "密码必须是字母和数字组成")]
        public string password { get; set; }
    }
}