using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Qj.Utility
{
    public static class LogHelper
    {
        public static string GetDir()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + "/Logs";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(Exception ex)
        {
            try
            {
                string path = GetDir();

                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);

                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-->" + ex.ToString());
                    //  sw.WriteLine("----------------------------------------");
                    sw.Close();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Log(string msg)
        {
            try
            {
                string path = GetDir();

                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);

                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "INFO-->" + msg);
                    //  sw.WriteLine("----------------------------------------");
                    sw.Close();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="msg"></param>
        public static string GetLogText(string FileName)
        {
            string strData = "";
            try
            {
                string path = GetDir();
                path = path + "\\" + FileName + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    string line;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                    {
                        // 从文件读取并显示行，直到文件的末尾
                        while ((line = sr.ReadLine()) != null)
                        {
                            //Console.WriteLine(line);
                            strData = line;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strData = null;
            }
            return strData;
        }

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="msg"></param>
        public static void SetLogText(string msg, string FileName)
        {
            try
            {
                string path = GetDir();

                path = path + "\\" + FileName + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default);
                    sw.Write(msg);
                    sw.Close();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Log(string msg, string FileName)
        {
            try
            {
                string path = GetDir();

                path = path + "\\" + FileName + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);

                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "INFO-->" + msg);
                    //  sw.WriteLine("----------------------------------------");
                    sw.Close();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="msg"></param>
        public static void LogTest(string msg)
        {
            try
            {
                string path = GetDir();

                path = path + "\\Test" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);

                    sw.WriteLine(msg);
                    sw.Close();
                }
            }
            catch
            {
            }
        }
    }
}