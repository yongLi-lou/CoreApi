using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qj.Models
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class Sys_OperationLog : IntBaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 方法参数
        /// </summary>
        public string ActionParameters { get; set; }

        /// <summary>
        /// 方法内容
        /// </summary>
        public string ActionBody { get; set; }

        /// <summary>
        /// 操作模块描述
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 操作动作
        /// </summary>
        public string Option { get; set; }

        /// <summary>
        /// 操作备注
        /// </summary>
        public string OptionRemarks { get; set; }


        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public string LogResult { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string LogUrl { get; set; }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        public void Update(Sys_OperationLog model)
        {
            UserID = model.UserID;
            Name = model.Name;
            ActionName = model.ActionName;
            ControllerName = model.ControllerName;
            ActionParameters = model.ActionParameters;
            ActionBody = model.ActionBody;
            ModuleName = model.ModuleName;
            Option = model.Option;
            OptionRemarks = model.OptionRemarks;
            IP = model.IP;
            LogResult = model.LogResult;
            LogUrl = model.LogUrl;
            ModelCreate(model.UserID);
        }
    }
}