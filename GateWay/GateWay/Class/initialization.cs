using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HelperTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GateWay.Class
{
    public class initialization
    {
        public string dir = AppDomain.CurrentDomain.BaseDirectory;
        public RedisHelper Redis = new RedisHelper();
        public string DataConnType = "mssql";
        public DataHandle Mo = new DataHandle();

        IDal dal;
        public bool ReadConfig()
        {
            string Path = System.Configuration.ConfigurationManager.AppSettings["ConFigPath"];
            bool Flg = false;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            try
            {
                #region  初始化路由表
                Redis.DbType = "ApiServer";
                string Api = Redis.Default.StringGet("ApiServer");
                if (string.IsNullOrEmpty(Api))
                {
                    JObject Apistr =JObject.Parse(FileHelper.ReadFile($@"{Path}\ConfigSet.json"));
                    Api = JsonConvert.SerializeObject(Apistr["ApiServer"]);
                }
                Program.ApiServer = JArray.Parse(Api);
                RouleJump Req = new RouleJump();
                Req.Initalimt();
                //Console.WriteLine(JsonConvert.SerializeObject(Program.UrlRoule));
                //Program.UrlRoule[0]["HostList"][1]["Counter"] = 10;
                //Program.UrlRoule[0]["HostList"][1]["IsEnabled"] = "000001";
                //Console.WriteLine(JsonConvert.SerializeObject(Program.UrlRoule));
                //HelpConst co = new HelpConst();
                
                //for (int x = 0; x < 100; x++)
                //{
                //    //Console.WriteLine(co.BuildTraceCode());
                //    Console.WriteLine(co.BuildTaceBatchNumber());                   
                //}
                
                #endregion

                Flg = true;

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return Flg;

        }
    }
}
