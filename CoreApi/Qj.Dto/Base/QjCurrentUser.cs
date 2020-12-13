using Newtonsoft.Json;
using Qj.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qj.Dto.Base
{
    public class QjCurrentUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public List<UserRoleEnum> UserRole { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public List<string> MenuName { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        public void FromUser(SysUser user)
        {
            ID = user.ID;
            Nickname = user.Name;
            UserType = (int)user.UserType;
            Name = user.UserName;
            DepartmentID = user.DepartmentID;
        }

        /// <summary>
        /// 转json
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// 用户角色类型
    /// </summary>
    public enum UserRoleEnum
    {
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 99,

        /// <summary>
        /// 未知
        /// </summary>
        Other = 0,

        /// <summary>
        /// 部门负责人
        /// </summary>
        Principal = 1,

        /// <summary>
        /// 分管领导
        /// </summary>
        FenGuanLeader = 2,

        /// <summary>
        /// 主要领导
        /// </summary>
        ZhuYaoleader = 3,

        /// <summary>
        /// 中心分管领导
        /// </summary>
        CentreFenGuanLeader = 4,

        /// <summary>
        /// 中心主要领导
        /// </summary>
        CentreZhuYaoleader = 5,

        /// <summary>
        /// 大厅管理员
        /// </summary>
        LobbyAdmin = 6,
    }
}