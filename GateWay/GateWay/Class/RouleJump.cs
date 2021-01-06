using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
namespace GateWay.Class
{
    public class RouleJump
    {
        public DataHandle Mo = new DataHandle();
        public ParasGobalGet Paras = new ParasGobalGet();
        public RedisHelper Redis = new RedisHelper() { DbType = "ApiServer" };
        public string dir = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 从Redis读写
        /// </summary>
        /// <param name="ApiRoule"></param>
        /// <param name="ChkGuest"></param>
        /// <returns></returns>
        public string HttpUrl(JArray ApiRoule, JObject ChkGuest)
        {
            string Result = string.Empty;
            string HttpUrl = string.Empty;
            string Syscode = string.Empty;
            string Port = string.Empty;
            string UrlMd5 = string.Empty;
            
            #region 路由分发
            for (int x = 0; x < ApiRoule.Count; x++)
            {
                JObject Sys = JObject.Parse(ApiRoule[x].ToString());
                if (Sys["SysCode"].ToString() == ChkGuest["SysCode"].ToString())
                {
                    UrlMd5 = Sys["SysCode"].ToString();//系统编码
                    JArray CodeList = JArray.Parse(Sys["CodeList"].ToString());
                    for (int y = 0; y < CodeList.Count; y++)
                    {
                        JObject Roule = JObject.Parse(CodeList[y].ToString());
                        if (Roule["Port"].ToString() == ChkGuest["Port"].ToString())
                        {
                            UrlMd5 += $"-{Roule["Port"].ToString()}-";//服务路由
                            JArray Url = JArray.Parse(Roule["UrlList"].ToString());
                            int counter = 0;
                            HttpUrl = Url[0]["Http"].ToString();
                            #region 把组合转换MD5 写进缓存做分发计数器
                            string Md5 = $"{UrlMd5}{Url[0]["Http"].ToString()}";
                            Md5 = CompressByte.MD5_16D(Md5);
                            string C = Redis.Default.StringGet(Md5);
                            if (string.IsNullOrEmpty(C))
                            {
                                counter = 0;
                                Redis.Default.StringSet(Md5, "{\"Counter\":\"0\"," +
                                    "\"Sys-Service-Port\":\"" + $"{UrlMd5}{Url[0]["Http"].ToString()}" + "\"}");
                            }
                            else
                            {
                                counter = (int)JObject.Parse(C)["Counter"];
                            }
                            #endregion
                            int counter2;
                            int FlgCounter = 0;//记录哪一个路由被 转发，需要+1回写
                            for (int z = 1; z < Url.Count; z++)
                            {
                                if (Url[z]["IsEnabled"].ToString() == "000001")
                                {
                                    Md5 = $"{UrlMd5}{Url[z]["Http"].ToString()}";
                                    Md5 = CompressByte.MD5_16D(Md5);
                                    C = Redis.Default.StringGet(Md5);
                                    if (string.IsNullOrEmpty(C))
                                    {
                                        counter2 = 0;
                                        Redis.Default.StringSet(Md5, "{\"Counter\":\"0\"," +
                                            "\"Sys-Service-Port\":\"" +
                                            $"{UrlMd5}{Url[z]["Http"].ToString()}" + "\"}");
                                    }
                                    else
                                    {
                                        counter2 = (int)JObject.Parse(C)["Counter"];
                                    }
                                    if (counter2 < counter)
                                    {
                                        HttpUrl = Url[z]["Http"].ToString();
                                        counter = counter2;
                                        FlgCounter = z;
                                    }
                                }
                            }
                            Md5 = $"{UrlMd5}{Url[FlgCounter]["Http"].ToString()}";
                            Md5 = CompressByte.MD5_16D(Md5);
                            Redis.Default.StringSet(Md5, "{\"Counter\":\"" + (counter + 1).ToString() + "\"," +
                                "\"Sys-Service-Port\":\"" +
                                $"{UrlMd5}{Url[FlgCounter]["Http"].ToString()}" + "\"}");
                            break;
                        }
                    }
                    break;
                }
            }
            #endregion
            return HttpUrl;
        }
        /// <summary>
        /// 从内存读写
        /// </summary>
        /// <param name="ApiRoule"></param>
        /// <param name="ChkGuest"></param>
        /// <returns></returns>
        public string HttpUrlMem(JObject ChkGuest)
        {
            string Result = string.Empty;
            string HttpUrl = string.Empty;
            string Syscode = ChkGuest["SysCode"].ToString();
            string Port = ChkGuest["Port"].ToString();
            string UrlMd5 = CompressByte.MD5_16D($"{Syscode}-{Port}");
            int Md5counter=0;
            int Hostcounter=0;
            long Scounter=-1;
            for (int x = 0; x < Program.UrlRoule.Count; x++)
            {
                JObject Md5 = (JObject)Program.UrlRoule[x];
                if (Md5["UrlKey"].ToString() == UrlMd5)
                {
                    Md5counter = x;                    
                    JArray List = JArray.Parse(Md5["HostList"].ToString());
                    Scounter = -1;                    
                    for (int y = 0; y < List.Count; y++)
                    {
                        if (Scounter == -1)
                        {
                            if ((string)List[y]["IsEnabled"] == "000001")
                            {
                                Scounter = (long)List[y]["Counter"];
                                HttpUrl = (string)List[y]["Host"];
                                Hostcounter = y;
                            }
                        }
                        else
                        {
                            if ((string)List[y]["IsEnabled"] == "000001")
                            {
                                if ((long)List[y]["Counter"] < Scounter)
                                {
                                    Scounter = (long)List[y]["Counter"];
                                    HttpUrl = (string)List[y]["Host"];
                                    Hostcounter = y;
                                }
                            }
                        }
                    }
                }
            }
            //LogHelper.Error(HttpUrl);
            if (!string.IsNullOrEmpty(HttpUrl))
            {
                Program.UrlRoule[Md5counter]["HostList"][Hostcounter]["Counter"] = Scounter + 1;
            }
            #region 路由分发
            
            #endregion
            return HttpUrl;
        }
        /// <summary>
        /// 更新网关配置
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public bool UpSetGateWay(JObject Config)
        {
            string Path = System.Configuration.ConfigurationManager.AppSettings["ConFigPath"];
            bool Result =false;            
            JObject GateWay = JObject.Parse(Config["Para"].ToString());
            string FileName = Config["FileName"].ToString();
            
            Redis.Default.StringSet("ApiServer", JsonConvert.SerializeObject((JArray)GateWay["ApiServer"]));
            Program.ApiServer = (JArray)GateWay["ApiServer"];
            Initalimt();
            FileHelper.FileCreate($"{Path}", Config["Para"].ToString(), FileName);
            Result = true;
            return Result;
        }
        /// <summary>
        /// 初始化网关配置
        /// </summary>
        public void Initalimt()
        {
            Program.UrlRoule.Clear();
            JArray HostList;
            string UrlKey = string.Empty;
            string UrlPort = string.Empty;
            Int64 Counter = 0;
            for (int x = 0; x < Program.ApiServer.Count; x++)
            {
                JObject Sys = (JObject)Program.ApiServer[x];
                UrlKey = Sys["SysCode"].ToString();
                JArray CodeList = (JArray)Sys["CodeList"];
                for (int y = 0; y < CodeList.Count; y++)
                {
                    UrlPort = $"-{CodeList[y]["Port"].ToString()}";
                    string Md5 = $"{UrlKey}{UrlPort}";
                    string Md5ec = CompressByte.MD5_16D(Md5);
                    JArray UrlList = (JArray)CodeList[y]["UrlList"];
                    HostList = new JArray();
                    for (int z = 0; z < UrlList.Count; z++)
                    {
                        HostList.Add(
                            new JObject {
                                    new JProperty("Host",UrlList[z]["Http"].ToString()),
                                    new JProperty("Counter",Counter),
                                    new JProperty("IsEnabled",UrlList[z]["IsEnabled"].ToString())
                            }
                            );
                    }
                    Program.UrlRoule.Add(
                        new JObject {
                                    new JProperty("UrlKey",Md5ec),
                                    new JProperty("HostList",HostList)
                        }
                        );
                }
            }
        }
    }
}
