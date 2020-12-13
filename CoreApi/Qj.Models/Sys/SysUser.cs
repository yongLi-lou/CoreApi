using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Qj.Models
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SysUser : IntBaseEntity
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户绑定的手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTimes { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public SysUserRole UserType { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentID { get; set; }

        /// <summary>
        /// 是否部门负责人
        /// </summary>
        public bool IsDepartmentChargePerson { get; set; }

     

      

        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            LastLoginTime = DateTime.Now;
            LoginTimes++;
        }
    }
}