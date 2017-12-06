using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WisePay.DataAccess;
using WisePay.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WisePay.Web.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WisePay.Web.Internals;

namespace WisePay
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
            var pgConnectionString = Configuration.GetConnectionString("PgConnection");

            services.AddDbContext<WiseContext>(options => options.UseNpgsql(pgConnectionString));

            services.AddCors();

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WiseContext>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthConfig.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthConfig.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthConfig.SymmetricSecurityKey,
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAutoMapper(GetType().Assembly);
            services.AddInAppServices();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidatorActionFilter));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
