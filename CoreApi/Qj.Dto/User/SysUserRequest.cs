using Qj.Models;
using Qj.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qj.Dto.User
{
    public class SysUserRequest
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

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
        /// 名称
        /// </summary>
        public string Name { get; set; }

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
        /// 角色id集合
        /// </summary>
        public int[] RoleIDs { get; set; }

        /// <summary>
        /// ToEntity
        /// </summary>
        /// <returns></returns>
        public SysUser ToEntity()
        {
            var user = new SysUser();
            user.ID = this.Id;
            user.UserName = this.UserName;
            user.Password = this.Password;
            user.Phone = this.Phone;
            user.Name = this.Name;
            user.UserType = this.UserType;
            user.DepartmentID = this.DepartmentID;
            user.IsDepartmentChargePerson = this.IsDepartmentChargePerson;
            user.Password = string.IsNullOrWhiteSpace(this.Password) ? "000000" : this.Password;
            return user;
        }

        /// <summary>
        /// ToEntity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public SysUser ToEntity(SysUser user)
        {
            user.ID = this.Id;
            user.UserName = this.UserName;
            user.Phone = this.Phone;
            user.Name = this.Name;
            user.UserType = this.UserType;
            user.DepartmentID = this.DepartmentID;
            user.IsDepartmentChargePerson = this.IsDepartmentChargePerson;
            return user;
        }
    }
}