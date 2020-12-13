using Qj.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qj.Models
{
    /// <summary>
    /// Entity基类
    /// </summary>
    public class IntBaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //设置自增
        [Description("主键ID")]
        public virtual int ID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Description("创建日期")]
        public long? InDate { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        [Description("创建用户")]
        public int InUser { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        [Description("修改日期")]
        public long? EditDate { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        [Description("修改用户")]
        public int EditUser { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        public bool IsDelete { get; set; }

        public void ModelCreate(int userId)
        {
            IsDelete = false;
            InDate = DateTime.Now.ToUtc();
            InUser = userId;
        }

        public void ModelCreate()
        {
            IsDelete = false;
            InDate = DateTime.Now.ToUtc();
        }

        public void ModelEdit(int userId)
        {
            EditDate = DateTime.Now.ToUtc();
            EditUser = userId;
        }

        public void ModelEdit()
        {
            EditDate = DateTime.Now.ToUtc();
        }
    }
}