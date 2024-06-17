using Connect.Application.Services;
using Connect.Application.Settings;
using Connect.Core.Interfaces;
using Connect.Infrastructure.Repsitory_UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Application.Extentions
{
    public static class ServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IFreelancerService, FreelancerService>();
            services.AddScoped<IUserHelpers, UserHelpers>();
            services.AddScoped<IMailingService, MailingService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<Settings.MailSettings>(configuration.GetSection("Mailing"));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
        }
    }
}
