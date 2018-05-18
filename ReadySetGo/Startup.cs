using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using ReadySetGo.Library;
using ReadySetGo.Library.DataContracts;
using ReadySetGo.Models;

namespace ReadySetGo
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
            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddTransient<ISetlistBuilder, SetlistBuilder>();
            services.AddTransient<ISetlistFmService, SetlistFmService>();
            services.AddTransient<ISpotifyService, SpotifyService>();
            services.AddTransient<IRequestLogger, RequestLogger>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<SetlistConfig>(Configuration.GetSection("SetlistConfig"));
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(new StaticFileOptions() 
            { 
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Images")) ,
                RequestPath = new PathString("/Images")
            });

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{artistName?}");
            });

            loggerFactory.AddFile("Logs/ReadySetGo-{Date}.txt");
        }
    }
}
