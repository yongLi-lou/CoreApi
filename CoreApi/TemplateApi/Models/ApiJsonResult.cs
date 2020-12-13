using TemplateApi.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Web;

namespace TemplateApi.Common
{
    /// <summary>
    /// ApiJson
    /// </summary>
    public class ApiJsonResult
    {
        /// <summary>
        /// ApiJson
        /// </summary>
        /// <param name="result"></param>
        public ApiJsonResult(ResultType result)
        {
            switch (result)
            {
                case ResultType.SUCCESS:
                    code = "SUCCESS";
                    break;

                case ResultType.FAIL:
                    code = "FAIL";
                    break;

                case ResultType.ERROR:
                    code = "ERROR";
                    break;

                case ResultType.Unauthorized:
                    code = "401";
                    break;

                default:
                    throw new QjApiException("ResultType 类型异常");
            }

            msg = string.Empty;
        }

        /// <summary>
        /// 结果代码
        /// </summary>
        public string code { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 消息文本
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 输出json
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 结果
        /// </summary>
        public enum ResultType
        {
            /// <summary>
            /// 成功
            /// </summary>
            SUCCESS = 0,

            /// <summary>
            /// 失败
            /// </summary>
            FAIL = 1,

            /// <summary>
            /// 异常
            /// </summary>
            ERROR = 2,

            /// <summary>
            /// 认证失败
            /// </summary>
            Unauthorized = 401
        }
    }
}