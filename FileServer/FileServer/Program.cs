using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            string Path = System.Configuration.ConfigurationManager.AppSettings["ConFigPath"];
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile($@"{Path}\Host.json")
                .Build();
            var url = configuration["server.urls"];
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((c, h) =>
                {
                    h.AddJsonFile($@"{Path}\ConfigSet.json", true, true);
                })
                .UseUrls(url)
                .UseStartup<Startup>();
        }
    }
}
