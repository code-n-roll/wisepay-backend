using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WisePay.Web.Account;
using WisePay.Web.Auth;
using WisePay.Web.Avatars;
using WisePay.Web.ExternalServices;
using WisePay.Web.ExternalServices.Bank;
using WisePay.Web.ExternalServices.Crawler;
using WisePay.Web.Purchases;
using WisePay.Web.Teams;
using WisePay.Web.Users;

namespace WisePay.Web.Internals
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInAppServices(this IServiceCollection services)
        {
            services.AddScoped<AuthTokenService>();
            services.AddScoped<UsersService>();
            services.AddScoped<TeamsService>();
            services.AddScoped<PurchasesService>();
            services.AddScoped<AccountService>();
            services.AddScoped<AvatarsService>();
            services.AddScoped<StoreOrdersService>();

            services.AddScoped<BankApi>();
            services.AddScoped<CrawlerApi>();

            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<JsonConfig>();

            return services;
        }
    }
}
