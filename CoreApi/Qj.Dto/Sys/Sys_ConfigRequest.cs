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

    public class Sys_ConfigRequest : IntBaseDataDto
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
        /// ToEntity
        /// </summary>
        /// <returns></returns>
        public Sys_Config ToEntity()
        {
            var model = new Sys_Config();
            model.DataCode = this.DataCode;
            model.DataName = this.DataName;
            model.DataValue = this.DataValue;
            model.DataGroupTag = this.DataGroupTag;
            model.DataGroupMemo = this.DataGroupMemo;
            model.Sequence = this.Sequence;
            model.ID = this.ID;
            model.IsDelete = this.IsDelete;
            model.InDate = this.InDate;
            model.InUser = this.InUser;
            model.EditDate = this.EditDate;
            model.EditUser = this.EditUser;
            return model;
        }
        /// <summary>
        /// ToEntity
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Sys_Config ToEntity(Sys_Config m)
        {
            m.DataCode = this.DataCode;
            m.DataName = this.DataName;
            m.DataValue = this.DataValue;
            m.DataGroupTag = this.DataGroupTag;
            m.DataGroupMemo = this.DataGroupMemo;
            m.Sequence = this.Sequence;
            m.ID = this.ID;
            m.IsDelete = this.IsDelete;
            m.InDate = this.InDate;
            m.InUser = this.InUser;
            m.EditDate = this.EditDate;
            m.EditUser = this.EditUser;
            return m;
        }
    }
}
