using Qiniu.Storage;
using Qiniu.Util;

namespace TemplateApi
{
    /// <summary>
    /// 七牛云
    /// </summary>
    public class QiNiuHelper
    {
        /// <summary>
        ///
        /// </summary>
        private static Mac Mac { get; set; } = new Mac("K8B1R3TwxhFW7yevlWiQRKeagxUrbimLd51KbpiP", "0UkA0bjK4FO2g6wHj282pHHG6AcW5V369MniL6E7");

        private static string PrivateDomain { get; set; } = "https://qny.qjnice.com";

        private static string PublicDomain { get; set; } = "https://qnc.qjnice.com";

        private static string PrivateBucket { get; set; } = "qjbucket";

        private static string PublicBucket { get; set; } = "qjpublic";

        /// <summary>
        /// 创建私库上传token
        /// </summary>
        /// <returns></returns>
        public static string GetPrivateToken()
        {
            return GetToken(PrivateBucket);
        }

        /// <summary>
        /// 创建私库上传token
        /// </summary>
        /// <returns></returns>
        public static string GetPublicToken()
        {
            return GetToken(PublicBucket);
        }

        /// <summary>
        /// 获得私有图片地址
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPrivateUrl(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            return DownloadManager.CreatePrivateUrl(Mac, PrivateDomain, key, 3600);
        }

        /// <summary>
        /// 获得私有图片地址
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPublicUrl(string key)
        {
            return string.Format("{0}/{1}", PublicDomain, key);
        }

        /// <summary>
        /// 获得token
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns></returns>
        private static string GetToken(string bucket)
        {
            var putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(7200);

            string token = Qiniu.Util.Auth.CreateUploadToken(Mac, putPolicy.ToJsonString());
            return token;
        }
    }
}