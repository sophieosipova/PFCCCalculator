using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dishes.Models;
using Microsoft.EntityFrameworkCore;
using DishesService.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SharedAutorizationOptions;

namespace Dishes
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
            services.AddDbContext<DishesContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DishesDatabase")));
            services.AddScoped<IDishesRepository, DishesRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var accountOptions = new AuthOptions()

            {
                ISSUER = "dishesServer",
                AUDIENCE = "GateWay"
            };
            var tokenGenerator = new TokenGenerator(accountOptions);

            services.AddSingleton<ITokenGenerator>(tokenGenerator);

            services.AddSingleton<AuthOptions>(accountOptions);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(cfg =>
              {
                  cfg.RequireHttpsMetadata = false;
                  cfg.SaveToken = true;
                  cfg.TokenValidationParameters = accountOptions.GetParameters();

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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
