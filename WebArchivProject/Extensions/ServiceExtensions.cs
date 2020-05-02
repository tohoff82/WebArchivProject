using Microsoft.Extensions.DependencyInjection;
using WebArchivProject.Contracts;
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
        }
    }
}
