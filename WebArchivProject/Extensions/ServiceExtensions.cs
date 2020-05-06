using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebArchivProject.Contracts;
using WebArchivProject.Persistance.Contexts;
using WebArchivProject.Persistance.Repos;
using WebArchivProject.Services;

namespace WebArchivProject.Extensions
{
    static class ServiceExtensions
    {
        public static void ConfigureViewLocationFormats(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddRazorOptions(options =>
            {
                options.AreaPageViewLocationFormats.Add("/Views/Shared/InformElements/{0}.cshtml");
                options.AreaPageViewLocationFormats.Add("/Areas/Workspace/Views/Shared/AddPublication/{0}.cshtml");
                options.AreaPageViewLocationFormats.Add("/Areas/Workspace/Views/Shared/SearchPublication/{0}.cshtml");
            });
        }

        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddTransient<IServCryptografy, ServCryptografy>();
            services.AddTransient<IServUserSession, ServUserSession>();
            services.AddTransient<IServAuthorsRows, ServAuthorsRows>();
            services.AddTransient<IServStartItemsCash, ServStartItemsCash>();
            services.AddTransient<IServBookFormCash, ServBookFormCash>();
            services.AddTransient<IAuthManager, AuthManager>();
            services.AddTransient<IServTheses, ServTheses>();
            services.AddTransient<IServBooks, ServBooks>();
            services.AddTransient<IServPosts, ServPosts>();
        }

        public static void AddAppRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ArchivContext>(opt =>
            {
                opt.UseMySql(configuration.GetConnectionString("ArchivConnection"));
            });
            services.AddTransient<IRepoAppUsers, RepoAppUsers>();
            services.AddTransient<IRepoAuthors, RepoAuthors>();
            services.AddTransient<IRepoBooks, RepoBooks>();
            services.AddTransient<IRepoPosts, RepoPosts>();
            services.AddTransient<IRepoTheses, RepoTheses>();
        }
    }
}
