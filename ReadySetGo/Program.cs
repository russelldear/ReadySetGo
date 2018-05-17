using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ReadySetGo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            if (Environment.GetEnvironmentVariable("ReadySetGoLocation") != null &&
                Environment.GetEnvironmentVariable("ReadySetGoLocation") == "Public")
            {
                return WebHost.CreateDefaultBuilder(args)
                              .UseUrls("http://localhost:80/")
                              .UseIISIntegration()
                              .UseStartup<Startup>()
                              .Build();
            }
            else
            {
                return WebHost.CreateDefaultBuilder(args)
                       .UseStartup<Startup>()
                       .Build();
            }
        }
    }
}
