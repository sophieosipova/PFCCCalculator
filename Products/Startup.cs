
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ProductsService.Models;
using ProductsService.Database;
using SharedAutorizationOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Products
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
            // string con = "Server=(localdb)\\mssqllocaldb;Database=DBProducts;Trusted_Connection=True;MultipleActiveResultSets=true";
            //     string s = Configuration.GetConnectionString("ProductsDatabase");
            //     services.AddDbContext<ProductsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProductsDatabase")));

            // string con = "Server=(localdb)\\mssqllocaldb;Database=DBProducts;Trusted_Connection=True;MultipleActiveResultSets=true";
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddDbContext<ProductsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProductsDatabase")));

            services.AddScoped<IProductRepository, ProductRepository>();

            var accountOptions = new AuthOptions()

            {
                ISSUER = "productServer",
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
