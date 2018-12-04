using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductsService.Models;
using ProductsService.Database;

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
