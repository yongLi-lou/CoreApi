using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateApi.Common;
using TemplateApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qj.Dto.Base;

namespace TemplateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseApiController
    {

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet, Route("Test")]
        [Log]
        [Cache]
        public OperateResponse Test(string pamrn)
        {
            return Excute(() =>
            {
                return Success("测试成功");
            });
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("Test")]
        [Auth]
        public OperateResponse Test([FromForm]testmodel model)
        {
            return Excute(() =>
            {
                return Success(model.conn);
            });
        }
    }


    public class testmodel
    {
        public string conn { get; set; }
    }
}