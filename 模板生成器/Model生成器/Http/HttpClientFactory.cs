using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HelperTools;
namespace Reptile
{
    public class HttpClientFactory
    {
        public string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36 MicroMessenger/7.0.9.501 NetType/WIFI MiniProgramEnv/Windows WindowsWechat";
        public string Refer = "";
        public string Accept = "application/json, text/javascript, */*; q=0.01";
        public string paras = "";
        public string Url = "";
        public string RequestMothed;
        public const int RWtime = 1000 * 60 * 5;
        public const int OverTime = 1000 * 30;
        public string Post()
        {
            string Result = "";
            try
            {
                using (var Provider = new ServiceCollection().AddHttpClient().BuildServiceProvider())
                {
                    var httpClientFactory = Provider.GetService<IHttpClientFactory>();
                    var client = httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri(Url);
                    client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                    
                    client.Timeout = new TimeSpan(0, 2, 0);

                    HttpRequestMessage message = new HttpRequestMessage();
                    message.Headers.Add("Accept", Accept);
                    //message.Headers.Add("Referer", Refer);
                    //  application/json   multipart/form-data text/plain ：纯文本格式 text/html ： HTML格式
                    //   https://www.cjavapy.com/article/723/  
                    if (RequestMothed.ToUpper() == "FORM")
                    {
                        message.Content = new StringContent(paras, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                    }
                    else if (RequestMothed.ToUpper() == "JSON")
                    {
                        message.Content = new StringContent(paras, System.Text.Encoding.UTF8, "application/json");
                    }
                    else if (RequestMothed.ToUpper() == "FILE")
                    {
                        //message.Content.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(PostData)), "file", "123.png");
                        message.Content = new StringContent(paras, System.Text.Encoding.UTF8, "multipart/form-data");
                    }
                    message.Method = HttpMethod.Post;

                    message.RequestUri = new Uri(client.BaseAddress.ToString());
                    HttpResponseMessage response = client.SendAsync(message).Result;
                    Result = response.Content.ReadAsStringAsync().Result;                    
                }
            }
            catch (Exception ex)
            {
                Result = "";// BuildResult("2", false, $"请求发生错误：{ex.Message.ToString()}");
                //LogHelp.errlog(ex, ex.Message.ToString(), Form1.LogsFile);
            }
            return Result;
        }

        public async Task<string> PostAsync(long Num,long Conter)
        {
            string Result = "";
            try
            {
                using (var Provider = new ServiceCollection().AddHttpClient().BuildServiceProvider())
                {
                    var httpClientFactory = Provider.GetService<IHttpClientFactory>();
                    var client = httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri(Url);
                    client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                    client.Timeout = new TimeSpan(0, 2, 0);

                    HttpRequestMessage message = new HttpRequestMessage();
                    message.Headers.Add("Accept", Accept);
                    //message.Headers.Add("Referer", Refer);
                    //  application/json   multipart/form-data text/plain ：纯文本格式 text/html ： HTML格式
                    //   https://www.cjavapy.com/article/723/  
                    if (RequestMothed.ToUpper() == "FORM")
                    {
                        message.Content = new StringContent(paras, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                    }
                    else if (RequestMothed.ToUpper() == "JSON")
                    {
                        message.Content = new StringContent(paras, System.Text.Encoding.UTF8, "application/json");
                    }
                    else if (RequestMothed.ToUpper() == "FILE")
                    {
                        //message.Content.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(PostData)), "file", "123.png");
                        message.Content = new StringContent(paras, System.Text.Encoding.UTF8, "multipart/form-data");
                    }
                    message.Method = HttpMethod.Post;

                    message.RequestUri = new Uri(client.BaseAddress.ToString());
                    HttpResponseMessage response =await client.SendAsync(message);
                    Result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Result = $"ERR:{ex.Message.ToString()}==>Num:{Num.ToString()}=>Congter:{Conter}"; 
                //LogHelp.errlog(ex, ex.Message.ToString(), Form1.LogsFile);
            }
            
            return Result;
        }
        private string BuildResult(string Scuess, bool Flg, string Result)
        {
            string ReturnStr = "";
            string FlgValue = (Flg == true) ? "0" : "1";
            string Msg = (Flg == true) ? "提交完成" : "请求返错误";
            Dictionary<string, string> Dict = new Dictionary<string, string>()
            {
                {"scuess", Scuess},
                {"Msg",Msg},
                {"Result",Result}
            };
            ReturnStr = JsonConvert.SerializeObject(Dict);
            return ReturnStr;
        }
        public static string HttpPost(string url, string postStr = null)
        {
            string result=string.Empty;

            try
            {
                var webClient = new WebClient { Encoding = Encoding.UTF8 };
                /*
                if (encode != null)
                    webClient.Encoding = encode;
                */
                var sendData = Encoding.GetEncoding("UTF-8").GetBytes(postStr);

                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("ContentLength", sendData.Length.ToString(CultureInfo.InvariantCulture));

                var readData = webClient.UploadData(url, "POST", sendData);

                result = Encoding.GetEncoding("UTF-8").GetString(readData);

            }
            catch (Exception ex)
            {               
                
            }
            return result;
        }
        public string SendHttpRequest()
        {
            string ReqResult = string.Empty;
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;
            //json格式请求数据
            string requestData = paras;
            //拼接URL
            string serviceUrl = Url;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            //post请求
            myRequest.Method = RequestMothed;
            //utf-8编码
            byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(requestData);

            myRequest.ContentLength = buf.Length;
            myRequest.Timeout = 5000;
            //指定为json否则会出错
            myRequest.ContentType = "application/json";
            myRequest.MaximumAutomaticRedirections = 1;
            myRequest.AllowAutoRedirect = true;
            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(buf, 0, buf.Length);
            newStream.Close();

            //获得接口返回值
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            ReqResult = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();

            return ReqResult;
        }
        /// <summary>
        /// 同步POST请求
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <param name="content">Post提交数据内容(utf-8编码的)</param>
        /// <returns></returns>
        public  string ReqPost()
        {
            string result = "";
            string RequestStr = "";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            try
            {
                Dictionary<string, string> Jdict = null;
                switch (RequestMothed)
                {
                    case "JSON":
                        Jdict = new Dictionary<string, string>
                                {
                                    {"paras",paras}
                                };
                        RequestStr = JsonConvert.SerializeObject(Jdict);
                        break;
                    case "FORM":
                        RequestStr = "paras="+ paras;
                        break;
                }

                byte[] data = Encoding.UTF8.GetBytes(RequestStr);
                req.ContentLength = data.Length;
                //直接关闭第一步验证
                req.ServicePoint.Expect100Continue = false;
                //是否使用Nagle：不使用，提高效率 
                req.ServicePoint.UseNagleAlgorithm = false;
                //设置最大连接数
                req.ServicePoint.ConnectionLimit = 65500;
                //指定压缩方法
                //req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                req.KeepAlive = false;

                switch (RequestMothed)
                {
                    case "JSON":
                        req.ContentType = "application/json;charset=utf-8";
                        break;
                    case "FORM":
                        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                        break;
                }
                req.Method = "POST";
                req.Timeout = OverTime;
                req.ReadWriteTimeout = RWtime;
                req.Credentials = CredentialCache.DefaultCredentials;
                //关闭缓存
                req.AllowWriteStreamBuffering = false;
                req.Proxy = null;
                //多线程并发调用时默认2个http连接数限制的问题，讲其设为512
                ServicePoint currentServicePoint = req.ServicePoint;
                currentServicePoint.ConnectionLimit = 512;
                //BindRequest.BindHead(req, FrmStrup.Userinto.Token);


                #region 添加Post 参数   
                /*
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }
                */
                using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                {
                    streamWriter.Write(RequestStr);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                #endregion

            }
            catch (Exception ex)
            {
                result = "";                
            }
            finally
            {
                req.Abort();
                req = null;
            }

            return result;
        }

    }
}
