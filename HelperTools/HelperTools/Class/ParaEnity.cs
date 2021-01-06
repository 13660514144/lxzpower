using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace HelperTools
{
    
    public class ParasGobalGet
    {
        public string AipPara;
        public RequestPagePara Default { get { return new RequestPagePara(AipPara); } }
    }
    public class RequestPagePara
    {
        public DataHandle Mo = new DataHandle();
        public JObject _JSON { get; set; }

        public RequestPagePara(string para)
        {
            _JSON = (JObject)JsonConvert.DeserializeObject(para);
        }
        public JObject GetGateWay()
        {
            return _JSON;
        }
        public AllParaResult GetParaEnity()
        {
            JObject post = JObject.Parse(CompressByte.DecodeBase64("UTF-8", _JSON["postdata"].ToString()));
            JToken Exists;
            UserKey userkey = new UserKey()
            {
                User = post["userkey"]["User"].ToString(),
                UserCn = post["userkey"]["UserCn"].ToString(),
                UnitCode = post["userkey"]["UnitCode"].ToString(),
                UnitCn = post["userkey"]["UnitCn"].ToString(),
                Role = post["userkey"]["Role"].ToString(),
                UserType = post["userkey"]["UserType"].ToString(),
                Token = post["userkey"]["Token"].ToString(),
                SysCode= post["userkey"]["SysCode"].ToString()
            };
            PageParaPostdata pagepara = new PageParaPostdata()
            {
                userkey = userkey,
                ParentData = (JObject)post["ParentData"]["ParentData"],
                SubList = (Mo.JtokenFlg(post["SubList"])==true)
                    ?(JArray)post["SubList"]:new JArray(),
                AttachList = (Mo.JtokenFlg(post["AttachList"]) == true)
                    ?(JArray)post["AttachList"]:new JArray(),
                PagePara= (Mo.JtokenFlg(post["ParentData"]["PagePara"]) == true)?
                    (JObject)post["ParentData"]["PagePara"]:new JObject()
            };
            AllParaResult Result = new AllParaResult()
            {
                postdata = pagepara,
                ParaMethod = post["ParaMethod"].ToString(),
                servercode = post["servercode"].ToString()
            };
            return Result;
        }
        public class AllParaResult
        {
            public PageParaPostdata postdata { get; set; }//表单参数 
            public string ParaMethod { get; set; }//请求类型 内部，外部，登录，数据请求
            public string servercode { get; set; }//服务编码
        }

        public class PageParaPostdata
        {
            public UserKey userkey { get; set; }//用户身份
            public JObject ParentData { get; set; }//表单参数 
            public JArray SubList { get; set; }//子表JSON
            public JArray AttachList { get; set; }//附件Josn
            public JObject PagePara { get; set; } //分页参数
        }
        /// <summary>
        /// 用户身份集合
        /// </summary>
        public class UserKey
        {
            public string User { get; set; }
            public string UserCn { get; set; }
            public string UnitCode { get; set; }
            public string UnitCn { get; set; }
            public string Role { get; set; }
            public string UserType { get; set; }
            public string Token { get; set; }
            public string SysCode { get; set; }
        }

    }
}
