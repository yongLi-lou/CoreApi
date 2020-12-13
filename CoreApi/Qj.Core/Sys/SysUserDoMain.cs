using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Qj.Models;
using Qj.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using QjEntity = Qj.Models.SysUser;

namespace Qj.Core
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class SysUserDoMain : BaseDomain<QjEntity>
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        public SysUser Login(string userName, string pwd)
        {
            //1.查询账号是否存在
            var model = _db.SysUser.FirstOrDefault(e => e.UserName == userName);
            if (model == null)
            {
                throw new QjCoreException("账号或者密码不正确");
            }

            //2.验证密码是否正确
            var md5pwd = EncryptPwd(pwd);
            if (model.Password != md5pwd)
            {
                throw new QjCoreException("账号或密码不正确");
            }

            model.Login();

            _db.SaveChanges();

            return model;
        }

        public static string pwdstr = "efghi";

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string EncryptPwd(string pwd)
        {
            if (string.IsNullOrEmpty(pwd)) throw new ArgumentNullException("pwd");

            return MD5(MD5(pwd) + "efghi");
            //return BaseEncrypt.MD5Encrypt(pwd);
        }

        /// <summary>
        /// MD5加密字符串（32位）
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            string result = BitConverter.ToString(md5.ComputeHash(bytes));
            return result.Replace("-", "").ToLower();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        public void EditPassword(int id, string newPwd, int userId, string oldPwd)
        {
            var model = _db.SysUser.FirstOrDefault(e => e.ID == id);
            if (model == null)
            {
                throw new QjCoreException("找不到该用户信息，无法修改密码。");
            }
            if (string.IsNullOrWhiteSpace(oldPwd))
            {
                throw new QjCoreException("请输入原先的密码。");
            }

            if (string.IsNullOrWhiteSpace(newPwd))
            {
                throw new QjCoreException("请输入修改后的密码。");
            }

            if (oldPwd == newPwd)
            {
                throw new QjCoreException("新密码不能和原密码一样。");
            }
            var oldPwdEncrypt = EncryptPwd(oldPwd);
            if (oldPwdEncrypt != model.Password)
            {
                throw new QjCoreException("原密码错误。");
            }

            model.Password = EncryptPwd(newPwd);
            model.ModelEdit(userId);
            _db.SaveChanges();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        public void EditPassword(SysUser user, int userId)
        {
            var model = _db.SysUser.FirstOrDefault(e => e.ID == user.ID);
            if (model == null)
            {
                throw new QjCoreException("找不到该用户信息，无法修改密码。");
            }
            model.Password = EncryptPwd(user.Password);
            model.ModelEdit(userId);
            _db.SaveChanges();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(SysUser user)
        {
            using (var cont = new CoreDbContext())
            {
                using (var trans = cont.Database.BeginTransaction())
                {
                    try
                    {
                        //1.查询账号是否存在
                        var model = cont.SysUser.FirstOrDefault(e => e.UserName == user.UserName && !e.IsDelete);
                        if (model != null)
                        {
                            throw new QjCoreException("存在相同的账号，无法保存。");
                        }
                        user.Password = EncryptPwd(user.Password);
                        user.ModelCreate();
                        cont.SysUser.Add(user);
                        cont.SaveChanges();

                        cont.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        public void AddUser(SysUser user, int userId)
        {
            using (var cont = new CoreDbContext())
            {
                using (var trans = cont.Database.BeginTransaction())
                {
                    try
                    {
                        //1.查询账号是否存在
                        var model = cont.SysUser.FirstOrDefault(e => e.UserName == user.UserName && !e.IsDelete);
                        if (model != null)
                        {
                            throw new QjCoreException("存在相同的账号，无法保存。");
                        }
                        user.Password = EncryptPwd(user.Password);
                        user.ModelCreate(userId);
                        cont.SysUser.Add(user);
                        cont.SaveChanges();

                        cont.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        public void EditUser(SysUser user)
        {
            using (var cont = new CoreDbContext())
            {
                using (var trans = cont.Database.BeginTransaction())
                {
                    try
                    {
                        //1.查询账号是否存在
                        var chongfu = cont.SysUser.Any(e => e.UserName == user.UserName && e.ID != user.ID && !e.IsDelete);
                        if (chongfu)
                        {
                            throw new QjCoreException("存在相同的账号，无法保存。");
                        }
                        EntityEntry entry = cont.Entry<SysUser>(user);
                        entry.State = EntityState.Modified;

                        cont.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
    }
}