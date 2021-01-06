using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HelperTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GateWay.Class;

namespace GateWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DataHandle Mo = new DataHandle();
        public ParasGobalGet Paras = new ParasGobalGet();
        public RedisHelper Redis = new RedisHelper() { DbType = "ServerCode" };
        private TimeSpan TIME = TimeSpan.FromSeconds(24 * 60 * 60);
        public RouleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 路由跳转分发
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("JumpUrl")]
        public async Task<string> JumpUrlAsync([FromBody] dynamic Param)
        {
            
            JObject Form = JObject.Parse(JsonConvert.SerializeObject(Param));
            string Base64 = CompressByte.DecodeBase64("UTF-8", Form["postdata"].ToString());
            JObject Obj = JObject.Parse(Base64);

            string ServerCode = Obj["servercode"].ToString();
            string ParaMothed = Obj["ParaMethod"].ToString();
            string Result = string.Empty;

            string HttpUrl = string.Empty;
            HostReqModel HostReq = new HostReqModel();
            RouleJump Req = new RouleJump();

            JObject ChkGuest = JObject.Parse(Redis.Default.StringGet(ServerCode));

            if (ServerCode == "F-002000-100154")//网关配置更新
            {
                bool Flg = Req.UpSetGateWay((JObject)Obj["ParentData"]["ParentData"]);
                if (Flg)
                {
                    Result = Mo.GetResult(true, "配置成功", false, new object(), true);
                }
                else
                {
                    Result = Mo.GetResult(false, "配置失败", false, new object(), true);
                }
                return Result;
            }

            Redis.DbType = "ApiServer";
            JArray ApiRoule = JArray.Parse(Redis.Default.StringGet("ApiServer"));
            string Api = HostReq.GetApiUrl(ServerCode);
            

            HttpUrl = Task.Run(() => Req.HttpUrlMem(ChkGuest)).Result;
            Paras.AipPara = JsonConvert.SerializeObject(Param);

            if (string.IsNullOrEmpty(HttpUrl))
            {
                Result = Mo.GetResult(false, "找不到服务", false, new object(), true);
            }
            else
            {
                if (Mo.JtokenFlg(ChkGuest["IsEnabled"]) && ChkGuest["IsEnabled"].ToString() == "000001")
                {
                    
                    if (ParaMothed == "0005007")
                    {
                        if (ChkGuest["ReqType"].ToString() == "005007")
                        {

                            Result = await HostReq.SendAsyncHttpRequest($"" +
                                $"{HttpUrl}{Api}", Paras.AipPara);
                        }
                        else
                        {
                            Result = Mo.GetResult(false, "服务编码、类型不匹配", false, new object(), true);
                        }
                    }
                    else
                    {
                        
                        #region  用户较验参数
                        var ApiObj = Paras.Default.GetParaEnity();
                        string UserToken = ApiObj.postdata.userkey.Token;
                        string UserKey = ApiObj.postdata.userkey.User;
                        string UserRoule = ApiObj.postdata.userkey.Role;
                        string UserSysCode = ApiObj.postdata.userkey.SysCode;
                        #endregion
                        switch (ParaMothed)
                        {
                            case "005000"://login
                                JObject Para = ApiObj.postdata.ParentData;
                                string Acc = Para["Username"].ToString();
                                string Pwd = Para["Password"].ToString();
                                if (string.IsNullOrEmpty(Acc) || string.IsNullOrEmpty(Pwd))
                                {
                                    Result = Mo.GetResult(false, "用户或密码为空", false, new object(), false);
                                }
                                else
                                {
                                    Result = await HostReq.SendAsyncHttpRequest($"" +
                                        $"{HttpUrl}{Api}", Paras.AipPara);
                                    JObject Data = JObject.Parse(Result);
                                    bool Flg = (bool)Data["scuess"];
                                    if (Flg)
                                    {
                                        JArray Arr = (JArray)Data["Result"]["Data"];
                                        /*成功需要返回前端，并在redis缓存UserKey*/
                                        JObject User = new JObject
                                        {
                                            { "User",Arr[0]["User"].ToString()},
                                            { "UserCn",Arr[0]["UserCn"].ToString()},
                                            { "UnitCode",Arr[0]["UnitCode"].ToString()},
                                            { "UnitCn",Arr[0]["UnitCn"].ToString()},
                                            { "Role",Arr[0]["Role"].ToString()},
                                            { "UserType",Arr[0]["UserType"].ToString()},
                                            { "Token",Arr[0]["Token"].ToString()},
                                            { "SysCode",Arr[0]["SysCode"].ToString()}
                                        };
                                        /*注册用户KEY*/
                                        Redis.DbType = "UserKey";
                                        Redis.Default.StringSet(Arr[0]["Token"].ToString(),
                                            JsonConvert.SerializeObject(User), TIME);
                                    }
                                }
                                break;
                            default:
                                //判断userkey 及 TOKEN
                                if (string.IsNullOrEmpty(UserKey) || string.IsNullOrEmpty(UserToken))
                                {
                                    Result = Mo.GetResult(false, "用户或Token不存在", false, new object(), false);
                                }
                                else
                                {
                                    Redis.DbType = "UserKey";
                                    string T = Redis.Default.StringGet(UserToken);//从缓存读TOKEN
                                    if (string.IsNullOrEmpty(T))
                                    {
                                        Result = Mo.GetResult(false, "无法确认用户Token", false, new object(), false);
                                    }
                                    else
                                    {
                                        JObject O = JObject.Parse(T);
                                        if (UserToken == O["Token"].ToString() &&
                                            UserRoule == O["Role"].ToString() &&
                                            UserKey == O["User"].ToString() &&
                                            UserSysCode == O["SysCode"].ToString())
                                        //TOKEN ROULE USER SYSCODE 存在并相同
                                        {
                                            /*UserKey 续期*/
                                            Redis.Default.StringSet(O["Token"].ToString().Trim(),
                                                JsonConvert.SerializeObject(O), TIME);
                                            //LogHelper.Error($"{HttpUrl}{Api}");
                                            Result = await HostReq.SendAsyncHttpRequest($"" +
                                                           $"{HttpUrl}{Api}", Paras.AipPara);
                                        }
                                        else//TOKEN 存在但与UserKey缓存中不相同 可能提交时被串改
                                        {
                                            Result = Mo.GetResult(false, "没有通过用户认证", false, new object(), false);
                                        }
                                    }
                                }
                                break;
                        }
                    }            
                }
                else
                {
                    Result = Mo.GetResult(false, "服务没有启动", false, new object(), true);
                }
            }

            return Result;
        }
    }
}
