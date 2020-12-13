using System;
using System.Collections.Generic;
using System.Text;

namespace Qj.Dto.Base
{
    public static class StaticLock
    {
        /// <summary>
        /// Token锁
        /// </summary>
        public static object TokenLock = new object();

        /// <summary>
        /// Redis key User
        /// </summary>
        public static string UserRedisKeyPrefix = "FUser:";


        /// <summary>
        /// Redis key User
        /// </summary>
        public static string CacheRedisKeyPrefix = "Cache:";
        
    }
}