using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GateWay.Class;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace GateWay
{
    public class Program
    {
        public static initialization O = new initialization();
        public static JArray ApiServer;
        public static JArray UrlRoule = new JArray();
       
        public static void Main(string[] args)
        {
            bool Flg = O.ReadConfig();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
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
