using System;
using System.Collections.Generic;
using System.Linq;
using Qj.Models;
using QjEntity = Qj.Models.Sys_OperationLog;
namespace Qj.Core
{

    public class Sys_OperationLogDoMain : BaseDomain<QjEntity>
    {

        /// <summary>
        /// ´´½¨
        /// </summary>
        /// <param name="entity"></param>
        public bool Log(QjEntity entity)
        {
            var model = new QjEntity();

            model.Update(entity);
            _db.Sys_OperationLog.Add(model);
            return _db.SaveChanges() > 0;
        }
    }
}
