using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WisePay.Web.Auth;
using WisePay.Web.Purchases;
using WisePay.Web.Teams;
using WisePay.Web.Users;

namespace WisePay.Web.Internals
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInAppServices(this IServiceCollection services)
        {
            services.AddScoped<AuthTokenService, AuthTokenService>();
            services.AddScoped<UsersService, UsersService>();
            services.AddScoped<TeamsService, TeamsService>();
            services.AddScoped<PurchasesService, PurchasesService>();

            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
