using Qj.Dto.Base;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Qj.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using TemplateApi.Auth;
using System.Web.Http;
using TemplateApi.Common;
using static TemplateApi.Common.ApiJsonResult;
using Microsoft.AspNetCore.Hosting;

namespace TemplateApi.Common
{
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    public class BaseApiController : ControllerBase
    {
        protected static ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(BaseApiController));

        
        /// <summary>
        /// 操作成功
        /// </summary>
        /// <returns></returns>
        protected OperateResponse Success()
        {
            return new OperateResponse(ResponseType.SUCCESS);
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <returns></returns>
        protected OperateResponse Success(string data)
        {
            var result = new OperateResponse(ResponseType.SUCCESS);
            result.data = data;
            return result;
        }

        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <returns></returns>
        protected ResponseEntity<T> SuccessEntity<T>(T data)
        {
            var result = new ResponseEntity<T>(ResponseType.SUCCESS);
            result.data = data;
            return result;
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <returns></returns>
        protected ResponseEntity<T> FailEntity<T>(string msg)
        {
            var result = new ResponseEntity<T>(ResponseType.FAIL);
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <returns></returns>
        protected QjResponse<T> Success<T>(IEnumerable<T> list, int pageCount)
        {
            var result = new QjResponse<T>(ResponseType.SUCCESS);
            result.data = list;
            result.count = pageCount;
            return result;
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <returns></returns>
        protected QjResponse<T> Fail<T>(string msg)
        {
            var result = new QjResponse<T>(ResponseType.FAIL);
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 401
        /// </summary>
        /// <returns></returns>
        protected OperateResponse Unauthorized()
        {
            return new OperateResponse(ResponseType.Unauthorized);
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <returns></returns>
        protected OperateResponse Fail(string msg)
        {
            var result = new OperateResponse(ResponseType.FAIL);
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <returns></returns>
        protected OperateResponse Fail(string data, string msg)
        {
            var result = new OperateResponse(ResponseType.FAIL);

            result.data = data;
            result.msg = msg;

            return result;
        }

        /// <summary>
        /// 操作警告
        /// </summary>
        /// <returns></returns>
        protected QjResponse<T> Error<T>(string msg)
        {
            var result = new QjResponse<T>(ResponseType.ERROR);
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 操作警告
        /// </summary>
        /// <returns></returns>
        protected ResponseEntity<T> ErrorEntity<T>(string msg)
        {
            var result = new ResponseEntity<T>(ResponseType.ERROR);
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 操作警告
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected OperateResponse Error(string msg)
        {
            var result = new OperateResponse(ResponseType.ERROR);
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 操作警告
        /// </summary>
        /// <returns></returns>
        protected OperateResponse Error(string data, string msg)
        {
            var result = new OperateResponse(ResponseType.ERROR);

            result.data = data;
            result.msg = msg;

            return result;
        }

        /// <summary>
        /// 运行操作
        /// </summary>
        /// <returns></returns>
        protected OperateResponse Excute(Action action)
        {
            try
            {
                action();
                return Success();
            }
            catch (QjApiException ex)
            {
                return Fail(ex.Message);
            }
            catch (QjCoreException ex)
            {
                return Fail(ex.Message);
            }
            catch (NotImplementedException ex)
            {
                log.Error("在无法实现请求的方法或操作时引发的异常", ex);
                return Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                log.Error("当为进程或操作分配的时间已过期时引发的异常", ex);
                return Error(ex.Message);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 运行操作
        /// </summary>
        /// <returns></returns>
        protected OperateResponse Excute(Func<OperateResponse> action)
        {
            try
            {
                return action();
            }
            catch (QjApiException ex)
            {
                return Fail(ex.Message);
            }
            catch (QjCoreException ex)
            {
                return Fail(ex.Message);
            }
            catch (NotImplementedException ex)
            {
                log.Error("在无法实现请求的方法或操作时引发的异常", ex);
                return Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                log.Error("当为进程或操作分配的时间已过期时引发的异常", ex);
                return Error(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("出现未处理异常", ex);
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 运行操作
        /// </summary>
        /// <returns></returns>
        protected QjResponse<T> Excute<T>(Func<QjResponse<T>> action)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Fail<T>(this.ExpendErrors());
                }
                return action();
            }
            catch (QjApiException ex)
            {
                return Fail<T>(ex.Message);
            }
            catch (QjCoreException ex)
            {
                return Fail<T>(ex.Message);
            }
            catch (NotImplementedException ex)
            {
                log.Error("在无法实现请求的方法或操作时引发的异常", ex);
                return Error<T>(ex.Message);
            }
            catch (TimeoutException ex)
            {
                log.Error("当为进程或操作分配的时间已过期时引发的异常", ex);
                return Error<T>(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("出现未处理异常", ex);
                return Error<T>(ex.Message);
            }
        }

       

        /// <summary>
        /// 运行操作
        /// </summary>
        /// <returns></returns>
        protected ResponseEntity<T> ExcuteEntity<T>(Func<ResponseEntity<T>> action)
        {
            try
            {
                return action();
            }
            catch (QjApiException ex)
            {
                return FailEntity<T>(ex.Message);
            }
            catch (QjCoreException ex)
            {
                return FailEntity<T>(ex.Message);
            }
            catch (NotImplementedException ex)
            {
                log.Error("在无法实现请求的方法或操作时引发的异常", ex);
                return ErrorEntity<T>(ex.Message);
            }
            catch (TimeoutException ex)
            {
                log.Error("当为进程或操作分配的时间已过期时引发的异常", ex);
                return ErrorEntity<T>(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("出现未处理异常", ex);
                return ErrorEntity<T>(ex.Message);
            }
        }

        /// <summary>
        /// 方法未实现
        /// </summary>
        /// <returns></returns>
        protected OperateResponse UnCode()
        {
            var result = new OperateResponse(ResponseType.FAIL);
            result.msg = "方法未实现";
            return result;
        }

        /// <summary>
        /// 当前登陆用户
        /// </summary>
        protected QjCurrentUser CurrentUser
        {
            get
            {
                var identity = User.Identity as Identity;
                return identity.CurrentUser;
            }
        }
        protected ApiJsonResult JsonSuccess()
        {
            return new ApiJsonResult(ResultType.SUCCESS);
        }

        protected ApiJsonResult JsonSuccess(object data)
        {
            var result = new ApiJsonResult(ResultType.SUCCESS);
            result.data = data;
            return result;
        }

        protected ApiJsonResult JsonSuccess(object data, string msg)
        {
            var result = new ApiJsonResult(ResultType.SUCCESS);
            result.data = data;
            result.msg = msg;
            return result;
        }

        protected ApiJsonResult JsonSuccess(object list, int pageCount)
        {
            var result = new ApiJsonResult(ResultType.SUCCESS);
            result.data = list;
            result.count = pageCount;
            return result;
        }



        #region object


        /// <summary>
        /// 运行操作
        /// </summary>
        /// <returns></returns>
        protected QjObjectResponse Excute(Func<QjObjectResponse> action)
        {
            try
            {
                return action();
            }
            catch (QjApiException ex)
            {
                return FailObject(ex.Message);
            }
            catch (QjCoreException ex)
            {
                return FailObject(ex.Message);
            }
            catch (NotImplementedException ex)
            {
                log.Error("在无法实现请求的方法或操作时引发的异常", ex);
                return ErrorObject("系统请求异常");
            }
            catch (TimeoutException ex)
            {
                log.Error("当为进程或操作分配的时间已过期时引发的异常", ex);
                return ErrorObject("系统处理超时");
            }
            catch (Exception ex)
            {
                log.Error("出现未处理异常", ex);
                return ErrorObject("系统出错了");
            }
        }


        /// <summary>
        /// 操作警告
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected QjObjectResponse ErrorObject(string msg)
        {
            var result = new QjObjectResponse(ResponseType.ERROR);
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 操作警告
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected QjObjectResponseEntity ErrorEntityObject(string msg)
        {
            var result = new QjObjectResponseEntity(ResponseType.ERROR);
            result.msg = msg;
            return result;
        }


        /// <summary>
        /// 操作失败
        /// </summary>
        /// <returns></returns>
        protected QjObjectResponse FailObject(string msg)
        {
            var result = new QjObjectResponse(ResponseType.FAIL);
            result.msg = msg;
            return result;
        }

        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <returns></returns>
        protected QjObjectResponseEntity SuccessEntityObject(object data)
        {
            var result = new QjObjectResponseEntity(ResponseType.SUCCESS);
            result.data = data;
            return result;
        }

        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <returns></returns>
        protected QjObjectResponse SuccessObject(IEnumerable<object> list, int pageCount)
        {
            var result = new QjObjectResponse(ResponseType.SUCCESS);
            result.data = list;
            result.count = pageCount;
            return result;
        }


 

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <returns></returns>
        protected QjObjectResponseEntity FailEntityObject(string msg)
        {
            var result = new QjObjectResponseEntity(ResponseType.FAIL);
            result.msg = msg;
            return result;
        }


        /// <summary>
        /// 运行操作
        /// </summary>
        /// <returns></returns>
        protected QjObjectResponseEntity ExcuteEntity(Func<QjObjectResponseEntity> action)
        {
            try
            {
                return action();
            }
            catch (QjApiException ex)
            {
                return FailEntityObject(ex.Message);
            }
            catch (QjCoreException ex)
            {
                return FailEntityObject(ex.Message);
            }
            catch (NotImplementedException ex)
            {
                log.Error("在无法实现请求的方法或操作时引发的异常", ex);
                return ErrorEntityObject("系统请求异常");
            }
            catch (TimeoutException ex)
            {
                log.Error("当为进程或操作分配的时间已过期时引发的异常", ex);
                return ErrorEntityObject("系统处理超时");
            }
            catch (Exception ex)
            {
                log.Error("出现未处理异常", ex);
                return ErrorEntityObject("系统出错了");
            }
        }

        #endregion

    }
}