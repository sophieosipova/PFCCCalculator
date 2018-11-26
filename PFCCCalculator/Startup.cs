﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Http;
using PFCCCalculatorService.Services;

namespace PFCCCalculator
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
            //services.AddTransient<IProductsService, PFCCCalculatorService.Services.ProductsService>();

           // var dbService = new PFCCCalculatorService.Services.ProductsService(new System.Net.Http.HttpClient());
            services.AddHttpClient<IProductsService, PFCCCalculatorService.Services.ProductsService>();

          //  var dbService = new PFCCCalculatorService.Services.ProductsService (new System.Net.Http.HttpClient());

            //services.AddSingleton<IProductsService>(dbService);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           
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
