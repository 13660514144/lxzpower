using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HelperTools;
using ServiceConfig.Class;
namespace ServiceConfig
{
    public class Program
    {
        public static Initialization O = new Initialization();
        public static void Main(string[] args)
        {
            bool Flg = O.ReadConfig();
            if (Flg)
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            else
            {
                Console.WriteLine("配置错误");
            }
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
