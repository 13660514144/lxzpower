using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace HelperTools
{
    public class HostReqModel
    {
        //private readonly IHttpContextAccessor httpContextAccessor;
        public List<DomainUserKey> ListUserKEY;
        private TimeSpan TIME = TimeSpan.FromSeconds(24 * 60 * 60);
        /*认证中心存储  USER在不同域的身份 TOKEN在域通用*/
        public class IdentyCollection
        {
            /*0无1有*/
            public int IsUser { get; set; }
            public int IsDomain { get; set; }
            public int IsToken { get; set; }
        }
        public class DomainUserKey
        {
            public string User { get; set; }
            public string UserCn { get; set; }
            public string UnitCode { get; set; }
            public string UnitCn { get; set; }
            public string Role { get; set; }
            public string UserType { get; set; }
            public string Token { get; set; }
            public string Domain { get; set; }
        }
        public class HostUrl
        {
            public string Host { get; set; }
            public string Ip { get; set; }
            public string Port { get; set; }
            public string Path { get; set; }
            public string Scheme { get; set; }
            public string Header { get; set; }
            public string Domain { get; set; }
        }

        public DomainUserKey GetUserKey(JObject Req)
        {
            DomainUserKey _DomainUserKey = new DomainUserKey()
            {
                User = Req["User"].ToString(),
                UserCn = Req["UserCn"].ToString(),
                UnitCode = Req["UnitCode"].ToString(),
                UnitCn = Req["UnitCn"].ToString(),
                Role = Req["Role"].ToString(),
                UserType = Req["UserType"].ToString(),
                Token = Req["Token"].ToString(),
                Domain = Req["Domain"].ToString()
            };
            return _DomainUserKey;
        }

        public HostUrl GetHostModel(HttpRequest Req)
        {
            HostUrl _HostModel = new HostUrl()
            {
                Host = Req.Host.ToString().Split(':')[0],
                Ip = Req.HttpContext.Connection.RemoteIpAddress.ToString(),
                Port = Req.Host.Port.ToString(),
                Path = Req.Path.ToString(),
                Scheme = Req.Scheme.ToString(),
                Header = Req.Headers.ToString(),
                Domain = Req.Host.ToString().Replace(":", ".")
            };
            return _HostModel;
        }
        /// <summary>
        /// 设置用户KEY缓存在REDIS
        /// </summary>
        /// <param name="HostKey"></param>
        /// <param name="UserKey"></param>
        /// <returns></returns>
        public bool CashSetUserKey(HostUrl HostKey, DomainUserKey UserKey)
        {
            bool Flg;
            string Domain = HostKey.Domain.ToString();
            string User = UserKey.User.ToString();
            string DomainKey = $"{User}.Key";
            string CashKey = CashGetUserKey(HostKey, UserKey);
            ListUserKEY = new List<DomainUserKey>();
            if (!string.IsNullOrEmpty(CashKey))
            {
                JObject Obj = (JObject)JsonConvert.DeserializeObject(CashKey);
                JArray ListKey = (JArray)Obj[DomainKey];
                for (int x = 0; x < ListKey.Count; x++)
                {
                    if (!string.Equals(Obj["Domain"].ToString().Trim(), UserKey.Domain.ToString().Trim()))
                    {
                        DomainUserKey _DomainUserKey = new DomainUserKey()
                        {
                            User = Obj["User"].ToString(),
                            UserCn = Obj["UserCn"].ToString(),
                            UnitCode = Obj["UnitCode"].ToString(),
                            UnitCn = Obj["UnitCn"].ToString(),
                            Role = Obj["Role"].ToString(),
                            UserType = Obj["UserType"].ToString(),
                            Token = Obj["Token"].ToString(),
                            Domain = Obj["Domain"].ToString()
                        };
                        ListUserKEY.Add(_DomainUserKey);
                    }
                }
            }
            ListUserKEY.Add(UserKey);
            Dictionary<string, dynamic> json = new Dictionary<string, dynamic>()
            {
                { User,ListUserKEY}
            };
            Flg = CashSetKey(DomainKey, JsonConvert.SerializeObject(json), "UserKey");
            return Flg;
        }
        public bool CashSetKey(string Key, string Value, string DbType)
        {
            RedisHelper Redis = new RedisHelper();
            Redis.DbType = DbType;
            bool S = Redis.Default.StringSet(Key, Value, TIME);
            return S;
        }
        /// <summary>
        /// 根据传入服务CODE和REDIS 表类型，从REDI读取服务配置
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="DbType"></param>
        /// <returns></returns>
        public string CashGetKey(string Key, string DbType)
        {
            RedisHelper Redis = new RedisHelper();
            Redis.DbType = DbType;
            string S = Redis.Default.StringGet(Key);
            return S;
        }
        /// <summary>
        /// 取当前用户带请求域的KEY含TOKEN
        /// </summary>
        /// <param name="HostKey"></param>
        /// <param name="UserKey"></param>
        /// <returns></returns>
        public string CashGetUserKey(HostUrl HostKey, DomainUserKey UserKey)
        {
            string Flg = string.Empty;
            string Domain = HostKey.Domain.ToString();
            string User = UserKey.User.ToString();
            string DomainKey = $"{User}.Key";
            Flg = CashGetKey(DomainKey, "UserKey");
            return Flg;
        }
        /// <summary>
        /// 用户较验
        /// </summary>
        /// <param name="HostKey"></param>
        /// <param name="UserKey"></param>
        /// <returns></returns>
        public JObject IdentChg(HostUrl HostKey, DomainUserKey UserKey)
        {
            IdentyCollection identy = new IdentyCollection();
            JObject Obj = null;
            string Domain = HostKey.Domain.ToString().Trim();
            string User = UserKey.User.ToString();
            string DomainKey = $"{User}.Key";
            string CurrentToken = UserKey.Token.ToString().Trim();
            if (string.IsNullOrEmpty(User))
            {
                /*客户端未提交有效用户身份==>无效用户*/
                identy.IsDomain = 0;
                identy.IsToken = 0;
                identy.IsUser = 0;
            }
            else
            {
                string CashKey = CashGetUserKey(HostKey, UserKey);
                if (string.IsNullOrEmpty(CashKey))//找不到此用户KEY
                {
                    /*用户过期、无效或伪造*/
                    identy.IsDomain = 0;
                    identy.IsToken = 0;
                    identy.IsUser = 0;
                }
                else
                {
                    JObject Cash = (JObject)JsonConvert.DeserializeObject(CashKey);
                    JArray ListKey = (JArray)Cash[DomainKey];
                    if (string.Equals(CurrentToken, ListKey[0]["Token"].ToString().Trim()))
                    {
                        /*TOKEN存在，需要判断是否同域 非同域需要模拟登录请求域获取UserKey*/
                        int Isdomain = 0;
                        for (int x = 0; x < ListKey.Count; x++)
                        {
                            if (string.Equals(Domain, ListKey[x]["Domain"].ToString().Trim()))
                            {
                                Isdomain = 1;
                                break;
                            }
                        }
                        identy.IsDomain = Isdomain;
                        identy.IsToken = 1;
                        identy.IsUser = 1;
                    }
                    else
                    {
                        /*TOKEN过期、无效或伪造*/
                        identy.IsDomain = 0;
                        identy.IsToken = 0;
                        identy.IsUser = 1;
                    }
                }
            }
            string json = JsonConvert.SerializeObject(identy);
            Obj = (JObject)JsonConvert.DeserializeObject(json);
            return Obj;
        }
        /// <summary>
        /// 用户登录中心
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Url"></param>
        /// <param name="JsonUser"></param>
        /// <returns></returns>
        public string IdentyLogin(string Host, string Url, string JsonUser)
        {
            string Result = string.Empty;
            Dictionary<string, dynamic> o = new Dictionary<string, dynamic>()
            {
                { "ApiPara",JsonConvert.DeserializeObject(JsonUser)}
            };
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "JSON",
                Url = $"http://{Host}{Url}",
                PostData = JsonConvert.SerializeObject(o)
            };
            Result = Req.SendHttpRequest();
            return Result;
        }
        /// <summary>
        /// 同用户不同域，获取用户数据
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="JsonUser"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string IdentyOtherDomainUser(string Host, string JsonUser, string Url)
        {
            string Result = string.Empty;
            Dictionary<string, dynamic> o = new Dictionary<string, dynamic>()
                {
                    { "ApiPara",JsonConvert.DeserializeObject(JsonUser)}
                };
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "JSON",
                Url = $"http://{Host}{Url}",
                PostData = JsonConvert.SerializeObject(o)
            };
            Result = Req.Post();
            return Result;
        }
        /// <summary>
        /// 公共同步提交POST+
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Url"></param>
        /// <param name="PostString"></param>
        /// <returns></returns>
        public string GuestPost(string Host, string Url, string PostString)
        {
            string Result = string.Empty;
            Dictionary<string, dynamic> o = new Dictionary<string, dynamic>()
                {
                    { "ApiPara",PostString}
                };
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "JSON",
                Url = $"http://{Host}{Url}",
                PostData = JsonConvert.SerializeObject(o)
            };
            Result = Req.Post();
            return Result;
        }
        /// <summary>
        /// HttpWebRequerst 模式
        /// </summary>
        /// <param name="requestURI"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public string SendHttpRequest(string requestURI, string json)
        {
            string Result = string.Empty;
            Dictionary<string, string> o = new Dictionary<string, string>()
                {
                    { "ApiPara",json}
                };
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "POST",
                Url = requestURI,
                PostData = json//JsonConvert.SerializeObject(o)
            };
            Result = Req.SendHttpRequest();
            return Result;
        }
        public async Task<string> SendAsyncHttpRequest(string requestURI, string json)
        {
            string Result = string.Empty;
            Dictionary<string, string> o = new Dictionary<string, string>()
                {
                    { "ApiPara",json}
                };
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "POST",
                Url = requestURI,
                PostData = json//JsonConvert.SerializeObject(o)
            };
            Result = await Req.SendAsyncHttpRequest();
            return Result;
        }
        /// <summary>
        /// 公共异步提交POST
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Url"></param>
        /// <param name="PostString"></param>
        /// <returns></returns>
        public async Task<string> GuestPostAsync(string Host, string Url, string PostString)
        {
            string Result = string.Empty;
            Dictionary<string, dynamic> o = new Dictionary<string, dynamic>()
                {
                    { "ApiPara",JObject.Parse(PostString)}
                };
            HttpClientFactory Req = new HttpClientFactory
            {
                RequestMothed = "JSON",
                Url = $"http://{Host}{Url}",
                PostData = JsonConvert.SerializeObject(o)
            };
            Result = await Req.PostAsync();
            return Result;
        }
        /// <summary>
        /// Get提交方法
        /// </summary>
        /// <param name="requestURI"></param>
        /// <param name="Para"></param>
        /// <returns></returns>
        public string HttpGet(string requestURI, string Para)
        {
            WebRequestOption Web = new WebRequestOption()
            {
                Url = requestURI,
                Paras = Para
            };
            HttpClientFactory Req = new HttpClientFactory();
            string Result = Req.HttpGet(Web);
            return Result;
        }
        public async Task<string> HttpGetAsync(string requestURI, string Para)
        {
            WebRequestOption Web = new WebRequestOption()
            {
                Url = requestURI,
                Paras = Para
            };
            HttpClientFactory Req = new HttpClientFactory();
            string Result = await Req.HttpGetAsync(Web);
            return Result;
        }
        /// <summary>
        /// 根据服务CODE取得API URLS
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Ifurl"></param>
        /// <returns></returns>
        public string GetApiUrl(string Code,bool Ifurl=false)
        {
            string Api = string.Empty;
            string RedisApiCash = CashGetKey(Code, "ServerCode");// Redis.Default.StringGet($"{Code}");
            JObject ApiObj = JObject.Parse(RedisApiCash);
            if (Ifurl)
            {
                Api = $"{ApiObj["DataService"]}{ApiObj["DataUrl"]}";
            }
            else
            {
                Api = $"{ApiObj["DataUrl"]}";
            }
            return Api;
        }

    }
}
