using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Qj.Core;
using Qj.Utility.Helper;
using Qj.Utility;
using LinqToExcel;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Qj.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp
{
    internal class Program
    {
        public static Thread Thread;
        /// <summary>
        /// 程序名称
        /// </summary>
        public static string ToProgramName = "程序名称";
        /// <summary>
        /// 邮件提醒
        /// </summary>
        public static string ToEmail = "840356427@qq.com";
        /// <summary>
        /// 重启次数存放文件名称
        /// </summary>
        public static string RetryFileName = "FailCount";
        /// <summary>
        /// 重启次数
        /// </summary>
        public static int RetryCount = 5;
        /// <summary>
        /// 重启间隔(毫秒)
        /// </summary>
        public static int RetryIntervalTime = 300000;
        /// <summary>
        /// 程序运行多久(毫秒)次数重置
        /// </summary>
        public static int ResetRetryCountTime = 600000;



        private static void Main(string[] args)
        {
            //new IISConfigDisPose().AppStart(); //IIS读写
            //new SqlDiff().AppStart();//数据机构对比




          






            Console.ReadKey();

        }

       




        private static void Main2(string[] args)
        {
            //重启
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Thread = new Thread(StartTrue);
            Thread.Start();

            CoreDbContext.ConnectionString = ConfigurationManager.AppSettings["SqlConnection"];
            RetryCount = ConfigurationManager.AppSettings["RetryCount"].FormatToInt(RetryCount);
            RetryIntervalTime = ConfigurationManager.AppSettings["RetryIntervalTime"].FormatToInt(RetryIntervalTime);
            ResetRetryCountTime = ConfigurationManager.AppSettings["ResetRetryCountTime"].FormatToInt(RetryIntervalTime);
            Shell.WriteLine($"{ToProgramName}程序开始运行");
            Shell.WriteLine($"连接字符串：{CoreDbContext.ConnectionString}");
            Shell.WriteLine($"重启次数：{RetryCount}");
            Shell.WriteLine($"重启间隔(毫秒)：{RetryIntervalTime}");
            Shell.WriteLine($" 程序运行多久(毫秒) 次数重置：{ResetRetryCountTime}");
            Shell.WriteEmpty(3);




        }





        /// <summary>
        /// 奔溃重启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="x"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs x)
        {
            Thread.Sleep(300000);

            int count = LogHelper.GetLogText(RetryFileName).FormatToInt(0);
            count++;
            LogHelper.SetLogText(count.ToString(), RetryFileName);
            if (count > RetryCount)
            {
                //重启失败
                EmailSentHelper.SentEmail(ToEmail, $"{ToProgramName}重启失败", "进程重启失败超过5次 ,请检查");
                Environment.Exit(-1);
            }
            try
            {
                Process m_Process = null;
                m_Process = new Process();
                m_Process.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName; ;
                m_Process.Start();
                EmailSentHelper.SentEmail(ToEmail, $"{ToProgramName}重启成功", "进程重启成功");
            }
            catch (Exception ex)
            {
                //重启失败
                EmailSentHelper.SentEmail(ToEmail, $"{ToProgramName}重启失败！", $"进程重启失败，失败原因：{ex.Message}");
                Environment.Exit(-1);
            }
        }


        /// <summary>
        /// 启动成功N分钟没奔溃就将失败次数清零
        /// </summary>
        private static void StartTrue()
        {
            Thread.Sleep(600000);
            LogHelper.SetLogText("0", RetryFileName);
        }

    }


}