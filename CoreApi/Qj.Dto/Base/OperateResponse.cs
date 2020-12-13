using Newtonsoft.Json;
using System;

namespace Qj.Dto.Base
{
    /// <summary>
    /// 通用操作返回
    /// </summary>
    public class OperateResponse
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        public OperateResponse(ResponseType type)
        {
            switch (type)
            {
                case ResponseType.SUCCESS:
                    code = "SUCCESS";
                    break;

                case ResponseType.FAIL:
                    code = "FAIL";
                    break;

                case ResponseType.ERROR:
                    code = "ERROR";
                    break;

                case ResponseType.Unauthorized:
                    code = "401";
                    break;

                default:
                    throw new Exception("ResultType 类型异常");
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
        public string data { get; set; }

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
    }
}