using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qj.Models;
using AutoMapper;
namespace Qj.Dto
{
    /// <summary>
    /// 系统配置表
    /// </summary>

    public class Sys_ConfigResponse : IntBaseDataDto
    {
        /// <summary>
        /// 数据编码
        /// </summary>
        public string DataCode { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        public string DataName { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        public string DataValue { get; set; }

        /// <summary>
        /// 数据分组标识
        /// </summary>
        public string DataGroupTag { get; set; }

        /// <summary>
        /// 数据分组说明
        /// </summary>
        public string DataGroupMemo { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// FromEntity
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Sys_ConfigResponse FromEntity(Sys_Config m)
        {
            var result = new Sys_ConfigResponse();
            result.DataCode = m.DataCode;
            result.DataName = m.DataName;
            result.DataValue = m.DataValue;
            result.DataGroupTag = m.DataGroupTag;
            result.DataGroupMemo = m.DataGroupMemo;
            result.Sequence = m.Sequence;
            result.ID = m.ID;
            result.IsDelete = m.IsDelete;
            result.InDate = m.InDate;
            result.InUser = m.InUser;
            result.EditDate = m.EditDate;
            result.EditUser = m.EditUser;
            return result;
        }
    }
}
