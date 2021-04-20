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
            ///��ȡRedis��Default������Ϣ
            var section = Configuration.GetSection("Redis:Default");
            ///��ȡ�����ִ�
            string _connectionString = section.GetSection("Connection").Value;
            ///��ȡʵ������
            string _instanceName = section.GetSection("InstanceName").Value;
            ///��ȡ���ݿ�
            int _defaultDB = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
            services.AddControllers();
            ///�м��Redis�����ע��
            services.AddSingleton(new RedisHelper(_connectionString, _instanceName, _defaultDB));
            ///��־׷�ټ�¼�����ע��
            services.AddScoped<IAccessHistoryLogService, AccessHistoryLogService>();
            ///���ݿ����ӷ����ע��
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
                    ///����ƾ֤
                    .AllowCredentials()
                    /// preflight �Ǽ����������������֮ǰ��Ԥ������
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

            //��UseHttpsRedirection��UseEndpoints����������֮ǰʹ��
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
