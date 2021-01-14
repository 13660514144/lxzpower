using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
namespace Reptile
{
    public class RequestMothed
    {
        public static string GetOrder(string Url,string PostData)
        {
            
            //string Url = $"{ConfigurationManager.AppSettings["RequestHost"]}{ConfigurationManager.AppSettings["Order"]}";

            //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"请求开始==>{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}");
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "JSON",
                Url = Url,
                paras = PostData
            };
            string Rmsg = Req.Post();
            //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"解析开始==>{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}");           
            
            return Rmsg;
        }
        public static async Task<string> GetOrderAsync(string Url, string PostData,int Num,int x)
        {            
            
            string s = await Task.Run(()=> {
                //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"请求开始==>{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}");
                string Rmsg = "";
                try
                {
                    HttpClientFactory Req = new HttpClientFactory
                    {
                        RequestMothed = "JSON",
                        Url = Url,
                        paras = PostData
                    };
                    Rmsg = Req.Post();
                }
                catch (Exception ex)
                {
                    //TaskConter.Msg(Form1.FrmMe, "TxtMsg", ex.Message.ToString());
                    //LogHelp.errlog(ex, "Stock Error", Form1.LogsFile);
                }
                return Rmsg;
            });
            //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"GetOrderAsync==>{s} \r\n  Form1.ConterNum={Num.ToString()}   For in FLG=>{x.ToString()}");
            return s;
        }
        public static  bool UpOrder()
        {
            bool Flg = false;
            try
            {
                string Url = $"{ConfigurationManager.AppSettings["RequestHost"]}{ConfigurationManager.AppSettings["UpOrder"]}";

                //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"订单状态更新==>{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}");
                HttpClientFactory Req = new HttpClientFactory
                {
                    RequestMothed = "JSON",
                    Url = Url,
                    //PostData = JsonConvert.SerializeObject(Form1.Root)
                };
                string Rmsg = Req.Post();
                if (!string.IsNullOrEmpty(Rmsg))
                {
                    JObject Obj = (JObject)JsonConvert.DeserializeObject(Rmsg);
                    if (Obj["code"].ToString() == "200")
                    {
                        //Form1.Root=null;
                        Flg = true;
                        //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"订单状态更新完成==>{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}");
                    }
                    else
                    {
                        //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"订单状态更新错误==>{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}");
                    }
                }
                else
                {
                    //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"订单状态更新错误==>无正确返回结果");
                }
            }
            catch(Exception ex)
            {
                //TaskConter.Msg(Form1.FrmMe, "TxtMsg", $"订单状态更新错误==>{ex.Message.ToString()}");
                //LogHelp.errlog(ex, "订单状态更新错误", Form1.LogsFile);
            }
            return Flg;
        }
        public static string SendHttpRequest(string requestURI, string json)
        {
            string Result = string.Empty;
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "JSON",
                Url = requestURI,
                paras = json//JsonConvert.SerializeObject(o)
            };
            Result = Req.SendHttpRequest();
            return Result;
        }
        public static string ReqPost(string requestURI, string json)
        {
            string Result = string.Empty;
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "FORM",
                Url = requestURI,
                paras = json//JsonConvert.SerializeObject(o)
            };
            Result = Req.ReqPost();
            return Result;
        }
    }
}
