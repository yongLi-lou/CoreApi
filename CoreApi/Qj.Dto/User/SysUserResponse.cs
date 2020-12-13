using Qj.Models;
using Qj.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qj.Dto
{
    /// <summary>
    /// 用户
    /// </summary>
    public class SysUserResponse
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
        /// 用户类型编号
        /// </summary>
        public int UserTypeNo { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentID { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int[] RoleIDs { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 是否部门负责人
        /// </summary>
        public bool IsDepartmentChargePerson { get; set; }


        /// <summary>
        /// FromEntity
        /// </summary>
        /// <returns></returns>
        public static SysUserResponse FromEntity(SysUser entity)
        {
            var response = new SysUserResponse();
            response.Id = entity.ID;
            response.UserName = entity.UserName;
            response.Phone = entity.Phone;
            response.Name = entity.Name;
            response.LastLoginTime = entity.LastLoginTime;
            response.LoginTimes = entity.LoginTimes;
            response.UserTypeNo = entity.UserType.GetValue();
            response.UserType = entity.UserType.GetDescription();
            response.LoginTimes = entity.LoginTimes;
            response.DepartmentID = entity.DepartmentID;
            response.IsDepartmentChargePerson = entity.IsDepartmentChargePerson;
            return response;
        }
    }
}