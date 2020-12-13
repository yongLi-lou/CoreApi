using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TemplateApi.Auth;
using TemplateApi.Common;
using TemplateApi.Filters;
using TemplateApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qj.Core;
using Qj.Dto.Base;
using Qj.Dto.User;
using Qj.Models;
using Qj.Utility;
using Qj.Utility.Helper;

namespace TemplateApi.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [Route("api/User")]
    public class UserLoginController : BaseApiController
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("Login")]
        [Log("登录", "model")]
        public OperateResponse UserLogin(SysUserLoginRequest model)
        {
            return Excute(() =>
            {


                SysUserDoMain domain = new SysUserDoMain();

                if (model == null)
                {
                    return Fail("请填写帐号和密码");
                }

                var user = domain.Login(model.userName, model.password);
                if (user == null)
                {
                    return Fail("账号或密码不正确");
                }

                var currentUser = new QjCurrentUser();
                currentUser.FromUser(user);

                var token = Provider.CreateToken(currentUser);
                return Success(token);
            });
        }

        /// <summary>
        /// 获得登录信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getLoginInfo")]
        [Auth]
        public ResponseEntity<QjCurrentUser> GetLoginInfo()
        {
            return ExcuteEntity(() =>
            {
                return SuccessEntity(CurrentUser);
            });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("EditPassword")]
        [Log("修改用户密码", "model")]
        public OperateResponse EditPassword(SysUserEditPwdRequest model)
        {
            return Excute(() =>
            {
                var datadomain = new SysUserDoMain();
                var usermodel = datadomain.GetModel(e => e.UserName == model.UserNmae);
                if (usermodel != null)
                {
                    datadomain.EditPassword(usermodel.ID, model.NewPwd, usermodel.ID, model.OldPwd);
                }
                return Success();
            });
        }

        /// <summary>
        /// 数据创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("AddUser")]
        [Log("用户管理系统添加用户", "model")]
        public OperateResponse UserDetail(SysUserRequest model)
        {
            return Excute(() =>
            {
                if (!ModelState.IsValid)
                {
                    return Fail(this.ExpendErrors());
                }
                SysUserDoMain datadomain = new SysUserDoMain();

                var data = model.ToEntity();

                var UserModel = datadomain.GetModel(e => e.ID == data.ID);
                if (UserModel == null)
                {
                    //添加
                    data.ModelCreate();
                    data.DepartmentID = 0;
                    datadomain.AddUser(data, -1);
                }
                //else
                //{
                //    data = model.ToEntity(UserModel);
                //    //编辑
                //    data.ModelEdit();
                //    data.DepartmentID = 0;
                //    datadomain.EditUser(data, -1);
                //}

                return Success();
            });
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPut("EditUser")]
        [Log("修改用户信息","id")]
        public OperateResponse EditUser(SysUserRequest m) {
            return Excute(()=> {
                var domain = new SysUserDoMain();
                var data = m.ToEntity();
                data.ModelEdit();
                domain.EditUser(data);
                return Success();
            });

        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpDelete, Route("UserDelete")]
        [Log("用户管理系统删除用户", "id")]
        public OperateResponse UserDelete(string UserName)
        {
            var datadomain = new SysUserDoMain();
            var data = datadomain.GetModel(e => e.UserName == UserName);
            if (data == null)
            {
                return Fail("未找到这条数据!");
            }

            Expression<Func<SysUser, bool>> where = e => e.UserName == UserName;
            int flag = datadomain.DelBy(where);

            return Success();
        }
    }

    /// <summary>
    /// 修改密码请求
    /// </summary>
    public class SysUserEditPwdRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserNmae { get; set; }

        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPwd { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPwd { get; set; }
    }
}