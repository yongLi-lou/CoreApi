using Qj.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qj.Utility
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public static class StaticDataConfig
    {
        public static string IsTest = ConfigurationManager.AppSettings["IsTest"];
    }
}
