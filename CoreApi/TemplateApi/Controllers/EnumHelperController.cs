using Microsoft.AspNetCore.Mvc;
using Qj.Core;
using Qj.Dto;
using Qj.Dto.Base;
using Qj.Models;
using Qj.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using Qj.Utility.Helper;
using TemplateApi.Common;

namespace Qj.Api
{
    /// <summary>
    /// 枚举获取
    /// </summary>
	[Route("api/EnumHelper")]
    [ApiController]
    public class EnumHelperController : BaseApiController
    {
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("RepeatEnum")]
        public QjResponse<EnumEntity> RepeatEnum()
        {
            return Excute(() =>
            {
                var list = EnumHelper.EnumToList<SysUserRole>().OrderBy(e => e.Value);
                return Success(list, list.Count());
            });
        }

  
    }
}