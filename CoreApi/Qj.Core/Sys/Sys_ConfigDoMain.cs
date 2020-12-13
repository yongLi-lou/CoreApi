using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Qj.Models;
using QjEntity = Qj.Models.Sys_Config;
namespace Qj.Core
{
    /// <summary>
    /// 系统配置表
    /// </summary>

    public class Sys_ConfigDoMain : BaseDomain<QjEntity>
    {
        /// <summary>
        /// 获得数据分组
        /// </summary>
        /// <returns></returns>
        public List<QjEntity> GetDataGroup(string dataGroupTag, string dataCode = "")
        {
            return _db.Set<QjEntity>().Where(
                p => p.DataGroupTag.ToUpper() == dataGroupTag.ToUpper()
                && (string.IsNullOrEmpty(dataCode) || (!string.IsNullOrEmpty(dataCode) && p.DataCode.StartsWith(dataCode))) && !p.IsDelete)
                .OrderBy(c => c.Sequence).ToList();
        }

        /// <summary>
        /// 获得数据配置值
        /// </summary>
        /// <param name="dataGroupTag"></param>
        /// <param name="dataName"></param>
        /// <returns></returns>
        public string GetDataValue(string dataGroupTag, string dataName)
        {
            var dataconfig = _db.Set<QjEntity>().FirstOrDefault(d => d.DataGroupTag.ToUpper() == dataGroupTag.ToUpper() && d.DataName == dataName && !d.IsDelete);
            if (dataconfig == null)
            {
                throw new Exception("您还未配置数据项：" + dataGroupTag);
            }
            return dataconfig.DataValue;
        }



        /// <summary>
        /// 获得数据配置名称
        /// </summary>
        /// <param name="dataGroupTag"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public string GetDataName(string dataGroupTag, string dataValue)
        {
            var dataconfig = _db.Set<QjEntity>().FirstOrDefault(d => d.DataGroupTag.ToUpper() == dataGroupTag.ToUpper() && d.DataValue == dataValue && !d.IsDelete);

            return dataconfig == null ? string.Empty : dataconfig.DataName;
        }

        public object DataGroupTree(Expression<Func<QjEntity, bool>> queryWhere = null)
        {
            var query = _db.Set<QjEntity>().AsQueryable().Where(e => !e.IsDelete);
            if (queryWhere != null)
            {
                query = query.Where(queryWhere);
            }

            return query.GroupBy(e => new { e.DataGroupTag, e.DataGroupMemo }).Select(e => new { e.Key.DataGroupMemo, e.Key.DataGroupTag }).ToList();
        }

        /// <summary>
        /// 获得数据配置值
        /// </summary>
        /// <param name="dataGroupTag"></param>
        /// <param name="dataName"></param>
        /// <returns></returns>
        public QjEntity GetItem(string dataGroupTag, string dataName)
        {
            return _db.Set<QjEntity>().FirstOrDefault(d => d.DataGroupTag.ToUpper() == dataGroupTag.ToUpper() && d.DataName == dataName && !d.IsDelete);
        }
    }
}
