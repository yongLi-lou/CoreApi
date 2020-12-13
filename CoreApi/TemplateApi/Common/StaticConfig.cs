using Qj.Core;
using Qj.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApi
{
    /// <summary>
    /// 配置数据
    /// </summary>
    public static class StaticConfig
    {

        /// <summary>
        /// 文件地址
        /// </summary>
        public static string FilePath { get; set; }

        /// <summary>
        /// 密钥AES加密
        /// </summary>
        public const string AESKey = "FBG091ER870M83YX";

        /// <summary>
        /// 初始向量AES加密
        /// </summary>
        public const string AESIV = "4QDD5M0KW2D4LJTB";
    }
}
