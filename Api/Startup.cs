using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace PetDeskProject1
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpClient();

            var keyBytes = Encoding.ASCII.GetBytes("Some secret string. Should use something like X509 certs anyway"); // the key should not be hard coded, nor should it be stored in a config file. Instead an installed certificate or a secure storage is the right option
            var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);

            const string apiUrl = "https://localhost:44319/";

            var serviceProvider = services.BuildServiceProvider();
            var hostingEnvironment = serviceProvider.GetRequiredService<IHostingEnvironment>();

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = !hostingEnvironment.IsDevelopment();
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = symmetricSecurityKey,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = !hostingEnvironment.IsDevelopment(),
                        ValidAudience = apiUrl,
                        ValidIssuer = apiUrl
                    };
                });

            services
                .AddAuthorization(ao =>
                {
                    ao.DefaultPolicy = new AuthorizationPolicy(new []{new DenyAnonymousAuthorizationRequirement()}, 
                        new[]{ JwtBearerDefaults.AuthenticationScheme}); //we need to set the default to use JWT or else specify it for each endpoint/controller
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
