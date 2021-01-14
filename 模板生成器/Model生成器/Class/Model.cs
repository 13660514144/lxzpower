using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace AllModel
{
    public class HttpModel
    {
        public class UserInfo
        {
            public string User { get; set; } = string.Empty;
            public string UserCn { get; set; } = string.Empty;
            public string UnitCode { get; set; } = string.Empty;
            public string UnitCn { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string UserType { get; set; } = string.Empty;
            public string Token { get; set; } = string.Empty;
        }
        public class Page
        {
            public int StartPapge { get; set; } = 0;
            public int Rpage { get; set; } = 0;
            public int Rnum { get; set; } = 0;
            public int MaxNum { get; set; } = 0;
            public int MinNum { get; set; } = 0;
            public int LastID { get; set; } = 0;
            public string PagingMode { get; set; } = "None";
        }

        public class Json
        {
            public string ParaMethod { get; set; } = string.Empty;
            public string servercode { get; set; } = string.Empty;
            public UserInfo userkey { get; set; }
            public ClientPara ParentData { get; set; }
            public JArray SubList { get; set; } = new JArray();
            public JArray AttachList { get; set; } = new JArray();
        }
        public class ClientPara
        {
            public JObject ParentData { get; set; }
            public Page PagePara { get; set; } = new Page();
        }
    }
}
