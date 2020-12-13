using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TemplateApi.Common;
using TemplateApi.Filters;
using Qj.Core;
using Qj.Utility.Helper;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using Qj.Utility;

namespace TemplateApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }
        public static ILoggerRepository repository { get; set; }

        public Startup(IHostingEnvironment env)
        {

            Configuration = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json")
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables().Build();

            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            #region DB INIT

            //services.AddDbContext<FDbContext>(options =>
            //{
            //    var str = Configuration.GetConnectionString("SqlServerConnection");
            //    options.UseLazyLoadingProxies().UseSqlServer(str);
            //});
            CoreDbContext.ConnectionString = Configuration.GetConnectionString("AlanConnection");
            //CoreDbContext.ConnectionString = Configuration.GetConnectionString("SqlServerConnection");

            #endregion DB INIT

            #region ADD Filter

            services.AddMvc(options =>
            {
                options.Filters.Add<APIResultFilter>();
                options.Filters.Add<TokenValidateFilter>();
            });

            #endregion ADD Filter

            #region 返回json格式自定义控制

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }); //延迟加载避免循环引用;时间格式转换

            #endregion 返回json格式自定义控制

            #region CORS 跨域控制

            //AllowAnyOrigin策略几乎直接完全无视了“同源策略”的限制，所以个人建议尽量不要这么写。AllowSpecificOrigin策略我暂时只放了一个测试用的源

            services.AddCors(c =>
            {
                c.AddPolicy("AllowAnyOrigin", policy =>
                {
                    policy.AllowAnyOrigin()//允许任何源
                    .AllowAnyMethod()//允许任何方式
                    .AllowAnyHeader()//允许任何头
                    .AllowCredentials();//允许cookie
                });

                #region 注释

                //c.AddPolicy("AllowSpecificOrigin", policy =>
                //{
                //    policy.WithOrigins("http://localhost:5000")
                //    .WithMethods("GET", "POST", "PUT", "DELETE")
                //    .WithHeaders("authorization");
                //});

                //c.AddPolicy("AllowAnyOrigin", policy =>policy.AllowAnyOrigin());

                //c.AddPolicy("AllowAnyOrigin", policy =>
                //{
                //    policy.AllowAnyOrigin()//WithOrigins("http://localhost:5000")
                //     .AllowAnyMethod()//允许任何方式
                //       .AllowAnyHeader()//允许任何头
                //        .AllowCredentials();//允许cookie
                //    //.WithMethods("GET", "POST", "PUT", "DELETE")
                //    //.WithHeaders("authorization");
                //});

                #endregion 注释
            });

            #endregion CORS 跨域控制

            #region 配置Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "接口文档",
                    Description = "RESTful API for API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "ContactName", Email = "840356427@qq.com", Url = "www.baidu.com" }
                });

                c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "TemplateApi.xml");
                c.IncludeXmlComments(xmlPath, true);
                xmlPath = Path.Combine(basePath, "Qj.Dto.xml");
                c.IncludeXmlComments(xmlPath);
                xmlPath = Path.Combine(basePath, "Qj.Models.xml");
                c.IncludeXmlComments(xmlPath);
            });

            #endregion 配置Swagger

            #region 静态类赋值
            StaticConfig.FilePath = Environment.CurrentDirectory + "/Upload";
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region Swagger

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(ConfigurationManager.AppSettings["swaggername"] + "/swagger/v1/swagger.json", "Management API V1");
                //c.ShowRequestHeaders();
            });

            #endregion Swagger

            #region 文件访问地址配置

            string FilePath = Environment.CurrentDirectory+ "/Upload";
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);  //创建目录
            }

            app.UseFileServer(new FileServerOptions()//直接开启文件目录访问和文件访问
            {
                EnableDirectoryBrowsing = true,//开启目录访问
                FileProvider = new PhysicalFileProvider(FilePath),
                RequestPath = new PathString("/Upload")
            });

            #endregion



            //app.UseCors("AllowAnyOrigin");
            //app.UseHttpsRedirection();
            app.UseMvc();

            //初始化数据
            //DbInitializer.Initialize(new CoreDbContext());
            //实例化配置文件读取帮助类
            HelpServiceProvider.ServiceProvider = app.ApplicationServices;
        }

      


    }
}