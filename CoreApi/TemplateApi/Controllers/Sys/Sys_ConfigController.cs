using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Common;
using Qj.Core;
using Qj.Dto;
using Qj.Models;
using Qj.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using Qj.Dto.Base;
using TemplateApi.Filters;
namespace TemplateApi
{
    /// <summary>
    /// 系统配置表
    /// </summary>

    [Route("api/sys_config")]
    [Auth]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class Sys_ConfigController : BaseApiController
    {

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public QjResponse<Sys_ConfigResponse> GetList(int Index = 1, int Size = 15)
        {
            return Excute(() =>
            {
                Expression<Func<Qj.Models.Sys_Config, bool>> queryWhere = ExpressionExtension.True<Qj.Models.Sys_Config>();

                int Datacount = 0;
                var Listdata = new Sys_ConfigDoMain().GetPagedList(Index, Size, ref Datacount, queryWhere, (e => e.InDate), true);

                var list = (from m in Listdata
                            select Sys_ConfigResponse.FromEntity(m)).ToList();
                return Success(list, Datacount);
            });

        }

        /// <summary>
        /// 获取数据配置值
        /// </summary>
        /// <param name="dataGroupTag">分组名称</param>
        /// <param name="dataName">数据名称</param>
        /// <returns></returns>
        [HttpGet, Route("model")]
        public OperateResponse GetModel(string dataGroupTag, string dataName)
        {
            return Excute(() =>
            {
                var resultdata = new Sys_ConfigDoMain().GetDataValue(dataGroupTag, dataName);
                return Success(resultdata);
            });
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        [HttpGet, Route("detail/{ID}")]
        public ResponseEntity<Sys_ConfigResponse> GetModel(int ID)
        {
            return ExcuteEntity(() =>
            {
                var resultdata = new Sys_ConfigDoMain().GetModel(e => e.ID == ID);
                var data = Sys_ConfigResponse.FromEntity(resultdata);
                return SuccessEntity(data);
            });
        }



        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("detail")]
        [Log]
        public OperateResponse ModelEdit(Sys_ConfigRequest model)
        {
            return Excute(() =>
            {

                var datadomain = new Sys_ConfigDoMain();
                Sys_Config data = datadomain.GetModel(e => e.ID == model.ID);
                if (data == null)
                {
                    return Fail("未找到这条数据!");
                }

                data = model.ToEntity(data);
                data.ModelEdit();
                int flag = datadomain.Modify(data);

                return Success();

            });
        }


        /// <summary>
        /// 数据创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("detail")]
        [Log]
        public OperateResponse ModelCreate(Sys_ConfigRequest model)
        {
            return Excute(() =>
            {
                var data = model.ToEntity();
                data.ModelCreate();
                Sys_ConfigDoMain datadomain = new Sys_ConfigDoMain();

                var resultdata = datadomain.Add(data);

                return Success();
            });
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("detail")]
        [Log]
        public OperateResponse RemoveList(CommonRequest Model)
        {
            return Excute(() =>
            {
                Sys_ConfigDoMain datadomain = new Sys_ConfigDoMain();
                Expression<Func<Sys_Config, bool>> queryWhere = (e => Model.IDS.Contains(e.ID.ToString()));
                int rownum = datadomain.DelBy(queryWhere);
                return Success(string.Format("操作成功！影响数据{0}行", rownum));
            });
        }


    }
}
