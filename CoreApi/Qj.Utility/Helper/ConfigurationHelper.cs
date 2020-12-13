using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Qj.Utility.Helper
{
    public static class ConfigurationHelper
    {
        public static IConfiguration config { get; set; }

        static ConfigurationHelper()
        {
            IHostingEnvironment env = HelpServiceProvider.ServiceProvider.GetRequiredService<IHostingEnvironment>();
            config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static T GetAppSettings<T>(string key) where T : class, new()
        {
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;


            return appconfig;
        }

        public static string GetAppSettings(string key)
        {
            //var data = config["appSettings"];
            ////两种方式读取
            //var defaultcon = config.GetConnectionString("appSettings");
            //var devcon = config["appSettings:app_id"];
            return config.GetValue<string>(key);
        }

    }

    public class HelpServiceProvider
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}