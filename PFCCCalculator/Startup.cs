
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            //     services.AddHttpClient<IProductsService, PFCCCalculatorService.Services.ProductsService>();
            //     services.AddHttpClient<ICommentsService, PFCCCalculatorService.Services.CommentsService>();
            //     services.AddHttpClient<IDishesService, PFCCCalculatorService.Services.DishesService>();

            services.AddSingleton<IAutorizationService>(new PFCCCalculatorService.Services.AutorizationService(""));
            services.AddSingleton<IProductsService>(new PFCCCalculatorService.Services.ProductsService());
            services.AddSingleton<ICommentsService>(new PFCCCalculatorService.Services.CommentsService(""));
            services.AddSingleton<IDishesService>(new PFCCCalculatorService.Services.DishesService(""));

            services.AddSingleton<IGatewayService, GatewayService>();
            //  var dbService = new PFCCCalculatorService.Services.ProductsService (new System.Net.Http.HttpClient());

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });
            //services.AddSingleton<IProductsService>(dbService);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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
            app.UseCors("CorsPolicy");

            /*    app.MapWhen(context =>
                {
                    return (context.Request.Method == "POST" || context.Request.Method == "PUT") & !context.Request.Headers.Values.Contains("Authorization");
                }, HandleId); */

            app.UseStaticFiles();
            app.UseMvc();


            loggerFactory.AddDebug();
            loggerFactory.AddConsole();
        }

      /*  private static void HandleId(IApplicationBuilder app)
        {
            AutorizationService autorizationService = new AutorizationService("");
            await  autorizationService.ValidateToken("");

        } */
    }
}
