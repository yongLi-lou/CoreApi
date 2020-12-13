using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qj.Dto.Base
{
    /// <summary>
    /// 通用查询返回
    /// </summary>
    public class QjResponse<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        public QjResponse(ResponseType result)
        {
            switch (result)
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
        public IEnumerable<T> data { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 数量
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

    /// <summary>
    /// 通用查询，返回单条数据
    /// </summary>
    public class QjResponseEntity<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        public QjResponseEntity(ResponseType result)
        {
            switch (result)
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
        public T data { get; set; }

        /// <summary>
        /// 数量
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