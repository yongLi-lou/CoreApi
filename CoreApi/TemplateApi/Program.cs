using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TemplateApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var NewConfig = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("hosting.json", optional: true)
             .Build();
            
            CreateWebHostBuilder(args, NewConfig).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration NewConfig) =>
            WebHost.CreateDefaultBuilder(args).UseConfiguration(NewConfig)//.UseUrls("http://localhost:8201", "http://localhost:8202")
                .UseStartup<Startup>();
    }
}
