using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HelperTools
{
    public class ServiceStruct
    {
        public class ServiceModel
        {
            /*
             {
                "Struct":"SysConst.Tbl_Const.Config.json", 服务配置文件 Redis Key
                "Service":"localhost:6000",  备用
                "Url":"/api/ReadConfig/ReadCfg", 备用
                "DataService":"localhost:5006",  服务HTTP 路由
                "DataUrl":"/api/ReadConfig/GetConstAll", 最终数据请求API
                "ReqType":"Get" 请求模式 对应前端ParaMothed
                } 
             */
            public string Struct { get; set; } = string.Empty;
            public string Service { get; set; } = string.Empty; 
            public string DataService { get; set; } = string.Empty;
            public string DataUrl { get; set; } = string.Empty;
            public string ReqType { get; set; } = "Get";
            public string IsEnabled { get; set; } = "000001";//是否启用
            public string Status { get; set; } = "001001";//是否运行中
            public string SysCode { get; set; }
            public string Code { get; set; }//服务编码
            public string CusField { get; set; }
            public string Port { get; set; }
        }
        public class MenuModel
        {
            /*
            {
                "Code":"100000000",
                "Name":"修改",
                "ServiceCode":"C100104",
                "ServiceName":"常量修改服务",
                "ServiceType":"Get",
                "ReqServercode":"C100103",
                "ReqType":"Edit",
                "IsCreateWin":"000001",
                "WinMothed":"_CreatAudit",
                "ParasResult":"Edit.aspx?_EditOrBrow=0&_Ifupper=1,1000,500",
                "BtnOwerId":"EditNode",
                "IfGetId":"000001"
            },
             */
            public string Code { get; set; } = string.Empty;//菜单编码
            public string Name { get; set; } = string.Empty;
            public string ServiceCode { get; set; } = string.Empty;
            public string ServiceName { get; set; } = string.Empty;
            public string ServiceType { get; set; } = "Get";
            public string ReqServercode { get; set; } = string.Empty;
            public string ReqType { get; set; } = string.Empty;
            public string IsCreateWin { get; set; } = string.Empty;
            public string WinMothed { get; set; } = string.Empty;
            public string ParasResult { get; set; } = string.Empty;
            public string BtnOwerId { get; set; } = string.Empty;
            public string IfGetId { get; set; } = string.Empty;

        }
    }
}
