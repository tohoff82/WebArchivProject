using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Persistance.Contexts;
using WebArchivProject.Persistance.Repos;

namespace WebArchivProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
                .ConfigureViewLocationFormats();

            services.AddDbContext<ArchivContext>(opt =>
            {
                opt.UseMySql(Configuration.GetConnectionString("ArchivConnection"));
            });
            services.AddTransient<IRepoAppUsers, RepoAppUsers>();
            services.AddTransient<IRepoAuthors, RepoAuthors>();
            services.AddTransient<IRepoBooks, RepoBooks>();
            services.AddTransient<IRepoPosts, RepoPosts>();
            services.AddTransient<IRepoTheses, RepoTheses>();

            services.AddCustomService();
        }

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
