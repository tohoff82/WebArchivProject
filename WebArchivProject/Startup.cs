using AutoMapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebArchivProject.Extensions;
using WebArchivProject.Mappings;
using WebArchivProject.Models;

namespace WebArchivProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// ������������ �������
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
                .ConfigureViewLocationFormats();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddSingleton<ITempDataProvider,
                CookieTempDataProvider>();
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Webarchiv.Session";
            });
            services.AddHttpContextAccessor();

            services.Configure<SecurityCreds>(Configuration.GetSection("Security"));
            services.Configure<MySettings>(Configuration.GetSection("MySettings"));

            services.AddCustomService();
            services.AddAppRepositories(Configuration);
        }

        /// <summary>
        /// ��������� �������� ��������
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
