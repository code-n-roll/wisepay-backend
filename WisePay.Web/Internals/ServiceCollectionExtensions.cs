using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WisePay.Web.Auth;
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

            return services;
        }
    }
}
