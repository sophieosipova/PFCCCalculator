﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using CommentsService.Models;
using CommentsService.Database;

namespace Comments
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
            // string con = "Server=(localdb)\\mssqllocaldb;Database=DBComments;Trusted_Connection=True;MultipleActiveResultSets=true";
            //  services.AddDbContext<CommentsContext>(options => options.UseSqlServer(con));

            services.AddDbContext<CommentsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CommentsDatabase")));

            services.AddScoped<ICommentsRepository, CommentsRepository>(); 
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
