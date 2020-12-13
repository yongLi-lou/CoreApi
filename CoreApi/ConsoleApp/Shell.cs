using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    /// <summary>  
    ///   
    /// </summary>  
    static class Shell
    {



        /// <summary>  
        /// 输出信息  
        /// </summary>  
        /// <param name="count">空行数</param>  
        public static void WriteEmpty(int count)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(@"");
            }
        }


        /// <summary>  
        /// 输出信息  
        /// </summary>  
        /// <param name="output"></param>  
        public static void WriteLineNo(string output)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(@"[{0}]{1}", DateTimeOffset.Now, output);
        }

        /// <summary>  
        /// 输出信息  
        /// </summary>  
        /// <param name="output"></param>  
        public static void WriteLine(string output)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(output);
        }
        /// <summary>  
        /// 输出信息  
        /// </summary>  
        /// <param name="output"></param>  
        public static void WriteLineYellow(string output)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"[{0}]{1}", DateTimeOffset.Now, output);
        }

        /// <summary>  
        /// 输出信息  
        /// </summary>  
        /// <param name="output"></param>  
        public static void WriteLineRed(string output)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"[{0}]{1}", DateTimeOffset.Now, output);
        }
        /// <summary>  
        /// 输出信息  
        /// </summary>  
        /// <param name="output"></param>  
        public static void WriteLineGreen(string output)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"[{0}]{1}", DateTimeOffset.Now, output);
        }
 
    }
}
