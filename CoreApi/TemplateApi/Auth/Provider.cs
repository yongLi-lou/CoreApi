using Qj.Dto.Base;
using Qj.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApi.Auth
{
    /// <summary>
    /// token 操作
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="qjUser"></param>
        /// <returns></returns>
        public static string CreateToken(QjCurrentUser qjUser)
        {
            lock (StaticLock.TokenLock)
            {
                var token = Guid.NewGuid().ToString("N");
                RedisCacheHelper.Add($"{StaticLock.UserRedisKeyPrefix}{token}", qjUser, DateTime.Now.AddSeconds(86400));
                return token;
            }
        }

        /// <summary>
        /// 创建缓存数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string CreateCache(string key, dynamic str)
        {
            lock (StaticLock.TokenLock)
            {
                RedisCacheHelper.Add($"{key}", str, DateTime.Now.AddMinutes(3));
                return key;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="tokenKey"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static QjCurrentUser ValidateToken(string tokenKey)
        {
            lock (StaticLock.TokenLock)
            {
                var userInfo = RedisCacheHelper.Get<QjCurrentUser>($"{StaticLock.UserRedisKeyPrefix}{tokenKey}");
                if (userInfo != null && userInfo.ID > 0)
                {
                    return userInfo;
                }
                return null;
            }
        }


    }
}