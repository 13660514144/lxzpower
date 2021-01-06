using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HelperTools;
namespace GateWay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LogHelper.Configure();
            /*
            JArray a = new JArray();
            a.Add(
                new JObject( new JProperty("aa","aa1"),new JProperty("bb","bb1"))
                );
            Console.WriteLine(JsonConvert.SerializeObject(a));
            */
            /* 仅测试
            string Syscode = string.Empty;
            string Cash = string.Empty;
            var valuesSection = Configuration.GetSection("ApiServer");
            foreach (IConfigurationSection section in valuesSection.GetChildren())
            {
                Syscode = section.GetValue<string>("SysCode");
                var list = section.GetSection("CodeList");
                foreach (IConfigurationSection sys in list.GetChildren())
                {
                    Cash = $"{Syscode}-{sys.GetValue<string>("Port")}-";                    
                    var roule = sys.GetSection("UrlList");
                    foreach (IConfigurationSection dom in roule.GetChildren())
                    {
                        string rs= $"{Cash}{dom.GetValue<string>("Http")}";
                        Console.WriteLine(rs);
                    }
                }              
            }
            */
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpReports().AddHttpTransport();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc();
            //JobSchedulerWork.Work();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpReports();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ErrGobalLogHandling();//添加全局异常处理机制
            app.UseMvc();
        }
    }
}
