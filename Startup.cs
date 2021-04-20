using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudyRedis.Services;
using StudyRedis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
namespace StudyRedis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            ///获取Redis的Default配置信息
            var section = Configuration.GetSection("Redis:Default");
            ///获取连接字串
            string _connectionString = section.GetSection("Connection").Value;
            ///获取实例名称
            string _instanceName = section.GetSection("InstanceName").Value;
            ///获取数据库
            int _defaultDB = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
            services.AddControllers();
            ///中间件Redis服务的注册
            services.AddSingleton(new RedisHelper(_connectionString, _instanceName, _defaultDB));
            ///日志追踪记录服务的注册
            services.AddScoped<IAccessHistoryLogService, AccessHistoryLogService>();
            ///数据库连接服务的注册
            services.AddDbContext<StudyRedisContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            services.AddSession(options =>
                {
                    /*options.IdleTimeout = TimeSpan.FromMilliseconds(10);*/
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(
                options => options.AddPolicy("myCors", 
                    new CorsPolicyBuilder("myCorsOptions")
                    ///允许凭证
                    .AllowCredentials()
                    /// preflight 非简单请求进行正是请求之前的预检请求
                    .SetPreflightMaxAge(new TimeSpan(0, 10, 0))
                    .WithHeaders(Configuration["Cors:AllowHanders"].Split(";"))
                    .WithMethods("GET", "POST", "PUT")
                    .WithOrigins(Configuration["Cors:AllowOrigins"].Split(";"))
                    .WithExposedHeaders(Configuration["Cors:AllowExposedHanders"].Split(";"))
                    .Build()
                    )
                ) ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //在UseHttpsRedirection与UseEndpoints方法被调用之前使用
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
