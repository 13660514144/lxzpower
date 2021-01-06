using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HelperTools;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace ServiceConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        DataHandle Mo = new DataHandle();
        ParasGobalGet Paras = new ParasGobalGet();
        public RedisHelper Redis = new RedisHelper();
        public HostReqModel Host = new HostReqModel();
        public string DataConnType = "mssql";
        public string ThisConn;
        dynamic Obj = new DynamicObj();
        public string dir = AppDomain.CurrentDomain.BaseDirectory;
        IDal dal;
        public ReadConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
            ThisConn = _configuration.GetValue<String>("ConnectionStr").ToString();
        }
        
        /// <summary>
        /// 取外联表对应字段中文名称 做下拉取值 
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetOtherCodeName")]
        public string GetOtherCodeName([FromBody] dynamic Param)
        {
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            string[] Struct = RedisCfg["Struct"].ToString().Split('.');
            string[] Fields = RedisCfg["CusField"].ToString().Split(',');
            string UnitCode = ApiObj.postdata.userkey.UnitCode;
            HelpConst Const = new HelpConst();
            Const.ThisConn = ThisConn;
            DataTable Dt = Const.GetOtherCodeName(Struct, Fields,UnitCode);
            Obj.Data = Dt;
            string Result = Mo.GetResult(true, "查询成功", true, Obj._values, true);
            return Result;
        }

        /// <summary>
        /// 取外联表对应字段中文名称 做下拉取值 ,用在级联
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetOtherCodeNameLink")]
        public string GetOtherCodeNameLink([FromBody] dynamic Param)
        {
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            string SelValue = ApiObj.postdata.ParentData["SelValue"].ToString();
            string[] Struct = RedisCfg["Struct"].ToString().Split('.');
            string[] Fields = RedisCfg["CusField"].ToString().Split(',');
            string UnitCode = ApiObj.postdata.userkey.UnitCode;
            HelpConst Const = new HelpConst();
            Const.ThisConn = ThisConn;
            DataTable Dt = Const.GetOtherCodeNameLink(Struct, Fields, SelValue);
            Obj.Data = Dt;
            string Result = Mo.GetResult(true, "查询成功", true, Obj._values, true);
            return Result;
        }

        /// <summary>
        /// 取外联表角色对应字段中文名称 做下拉取值 
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetOtherRoleName")]
        public string GetOtherRoleName([FromBody] dynamic Param)
        {
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            string[] Struct = RedisCfg["Struct"].ToString().Split('.');
            string[] Fields = RedisCfg["CusField"].ToString().Split(',');
            string Role = ApiObj.postdata.userkey.Role;
            HelpConst Const = new HelpConst();
            Const.ThisConn = ThisConn;
            DataTable Dt = Const.GetOtherRoleName(Struct, Fields, Role);
            Obj.Data = Dt;
            string Result = Mo.GetResult(true, "查询成功", true, Obj._values, true);
            return Result;
        }

        [HttpPost("InsSingleData")]
        public string InsSingleData([FromBody] dynamic Param)
        {

            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));
            if (Struct["Tb"].ToString().Trim().ToUpper() == "TBL_USER")
            {
                string Pwd = ApiObj.postdata.ParentData["Pwd"].ToString();
                HelpConst Const = new HelpConst();
                string Account = Const.BuildUserAccount();
                Pwd = CompressByte.MD5_32X(Pwd); ;
                ApiObj.postdata.ParentData["Pwd"] = Pwd;
                ApiObj.postdata.ParentData["Account"] = Account;
            }
            MsIns Ins = new MsIns();
            Ins.ThisConn = ThisConn;
            Result = Task.Run(() => Ins.MsInsSingle(Struct, ApiObj)).Result;

            return Result;
        }
        /// <summary>
        /// 单表查询
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetFormSingleData")]
        public string GetFormSingleData([FromBody] dynamic Param)
        {
            //string Conn = _configuration.GetValue<String>("ConnectionStr").ToString();//取数据库连接            
            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));
            MsFormQuery Queyr = new MsFormQuery();
            Queyr.ThisConn = ThisConn;
            Result = Task.Run(() => Queyr.QuerySingle(Struct, ApiObj)).Result;

            return Result;
        }
        [HttpPost("GuestListData")]
        public string GuestListData([FromBody] dynamic Param)
        {

            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));
            MsListQuery Queyr = new MsListQuery();
            Queyr.ThisConn = ThisConn;
            //Result = Queyr.GuestListFields(Struct, ApiObj);
            Result = Task.Run(() => Queyr.GuestListFields(Struct, ApiObj)).Result;

            return Result;
        }
        /// <summary>
        /// 根据父级节点取子节点 树型结构通用
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetConstNode")]
        public string GetConstNode([FromBody] dynamic Param)
        {
            string Result = string.Empty;
            IDal dal;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            string Code = ApiObj.postdata.ParentData["Code"].ToString();
            int CodeLen = Code.Length + 3;
            #region 表结构
            string _ServerCode = ApiObj.servercode;
            Redis.DbType = "ServerCode";
            JObject KeyRedis = JObject.Parse(Redis.Default.StringGet(_ServerCode));
            string StructKey = KeyRedis["Struct"].ToString();
            string TreePara = string.Empty;//如果是模块表，需要用这个参数做CLick事件
            if (StructKey.Split('.')[1] == "Tbl_SysModel")
            {
                TreePara = ",ServiceType";
            }
            else
            {
                TreePara = ",ServiceType=''";
            }
            #endregion

            WordArea.SqlCmd = $@"SELECT 
                   [Code]
                  ,[CodeCn]
                  ,[UpperCode]
                  ,[ReMarks]{TreePara}                                
                    FROM [{StructKey.Split('.')[0]}].[dbo].[{StructKey.Split('.')[1]}]  WITH(NOLOCK)
                    where Code like '' + @Code+ '%' and len(Code)={CodeLen} and 
                    DelFlg=@DelFlg  order by Code";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Code",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Code
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@DelFlg",
                    ColType = Mo.ConvertSQLType("int"),
                    ColLeng = 8,
                    ColValue = "1"
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            DataTable _DtConst = Task.Run(() =>
                dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            Obj.Data = _DtConst;
            if (_DtConst.Rows.Count > 0)
            {
                Result = Mo.GetResult(true, "执行成功", true, Obj._values, true);
            }
            else
            {
                Result = Mo.GetResult(false, "没有子节点", false, Obj._values, true);
            }
            return Result;
        }
        /// <summary>
        /// 获取全部常量  树形结构通用 等同于普通表LIST
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetConstAll")]
        public string GetConstAll([FromBody] dynamic Param)
        {
            string Result = string.Empty;

            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            string UnitCode = ApiObj.postdata.userkey.UnitCode;
            #region 表结构
            string _ServerCode = ApiObj.servercode;
            Redis.DbType = "ServerCode";
            JObject KeyRedis = JObject.Parse(Redis.Default.StringGet(_ServerCode));
            string StructKey = KeyRedis["Struct"].ToString();
            string _PagePath = $"/Page/{StructKey.Split('.')[0]}/{StructKey.Split('.')[1]}/";//页面目录     
            string _ListFields = new MsGetListFields().GetListFields(StructKey);
            string TreePara = string.Empty;//如果是模块表，需要用这个参数做CLick事件
            if (StructKey.Split('.')[1] == "Tbl_SysModel")
            {
                TreePara = ",ServiceType";
            }
            else
            {
                TreePara = ",ServiceType=''";
            }
            #endregion
            dal = DalFactory.CreateDal(DataConnType, ThisConn);//创建数据操作对象
            #region 读按钮参数
            Redis.DbType = "MenuCode";
            string _BtnParaResult = Redis.Default.StringGet(_ServerCode);
            JArray RedisBtn = JArray.Parse(_BtnParaResult);
            string Role = ApiObj.postdata.userkey.Role;
            JArray BtnJarray = new JArray();

            if (Role != "000000")
            {
                #region 非配置管理员取按钮权限
                WordArea.SqlCmd = $@"
                SELECT 
                       [Code]                      
                      ,[ModelCode]                      
                  FROM [SysConfig].[dbo].[Tbl_RoleRights] WITH(NOLOCK)
                    where [RouleCode]=@RouleCode 
                        and SysCode=@SysCode
                        and InterfaceCode=@InterfaceCode
                        and [IsEnabled]='000001'
                        AND LEN([ModelCode])=9
                    order by ModelCode
                ";
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@RouleCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = Role
                    });
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@SysCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = ApiObj.postdata.userkey.SysCode
                    });
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@InterfaceCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = ApiObj.servercode
                    });
                DataTable Btn = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
                #endregion
                for (int x = 0; x < Btn.Rows.Count; x++)
                {
                    for (int y = 0; y < RedisBtn.Count; y++)
                    {
                        JObject item = (JObject)RedisBtn[y];
                        if (Btn.Rows[x]["ModelCode"].ToString() == item["Code"].ToString())
                        {
                            BtnJarray.Add(item);
                            break;
                        }
                    }
                }
                #region 定义UL Click事件  非配置管理用户需要使用，否则将失去触发事件

                for (int y = 0; y < RedisBtn.Count; y++)
                {
                    JObject item = (JObject)RedisBtn[y];
                    if (item["BtnOwerId"].ToString().Trim() == ""
                        && item["ServiceType"].ToString().Trim() == "005008")
                    {
                        BtnJarray.Add(item);
                        break;
                    }
                }

                #endregion
            }
            else
            {
                BtnJarray = RedisBtn;
            }

            #endregion
            string RoleWhere = string.Empty;
            if (Role != "000000" && StructKey.Split('.')[0] == "SysConfig"
                && StructKey.Split('.')[0] == "Tbl_Uint")

            {
                RoleWhere = $" and len(Code)={UnitCode.Length} and UnitCode='{UnitCode}' ";
            }
            else
            {
                RoleWhere =  " and len(Code)=3 ";
            }
            WordArea.SqlCmd = $@"SELECT 
                   [Code]
                  ,[CodeCn]
                  ,[UpperCode]
                  ,[ReMarks]{TreePara}                               
                    FROM [{StructKey.Split('.')[0]}].[dbo].[{StructKey.Split('.')[1]}] WITH(NOLOCK)
                    where DelFlg=@DelFlg {RoleWhere} order by Code";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@DelFlg",
                    ColType = Mo.ConvertSQLType("int"),
                    ColLeng = 50,
                    ColValue = "1"
                });

            DataTable _DtConst = Task.Run(() =>
                dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            Obj.Data = _DtConst;
            Obj.ListConFig = _ListFields;
            Obj.PagePath = _PagePath;
            Obj.BtnParaResult = BtnJarray;
            if (_DtConst.Rows.Count > 0)
            {
                Result = Mo.GetResult(true, "执行成功", true, Obj._values, true);
            }
            else
            {
                Result = Mo.GetResult(false, "没有节点", false, Obj._values, true);
            }
            return Result;
        }
        /// <summary>
        /// 公用
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("DelGobalDelPara")]
        public string DelGobalDelPara([FromBody] dynamic Param)
        {
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            var ApiObj = Paras.Default.GetParaEnity();
            MsDel Del = new MsDel()
            {
                ThisConn = ThisConn
            };
            string Result = Del.GobalDelPara(ApiObj);
            return Result;
        }
        /// <summary>
        /// 以Code 类删除都用此法 公用
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("DelConstCode")]
        public string DelConstCode([FromBody] dynamic Param)
        {

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            var ApiObj = Paras.Default.GetParaEnity();
            //读表配置KEY
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            //读表结构
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));
            MsDel Del = new MsDel()
            {
                ThisConn = ThisConn
            };
            string Result = Del.DelKeyCode(Struct, ApiObj);
            return Result;
        }
        /// <summary>
        /// 以主键ID删除 公用
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("DelKeyId")]
        public string DelKeyId([FromBody] dynamic Param)
        {

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            var ApiObj = Paras.Default.GetParaEnity();
            //读表配置KEY
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            //读表结构
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));
            MsDel Del = new MsDel()
            {
                ThisConn = ThisConn
            };
            string Result = Del.DelKeyId(Struct, ApiObj);
            return Result;
        }
        /// <summary>
        /// 新增加常量 节点 公用
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("InsConstRootCode")]
        public string InsConstRootCode([FromBody] dynamic Param)
        {
            //string Conn = _configuration.GetValue<String>("ConnectionStr").ToString();//取数据库连接            
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            var ApiObj = Paras.Default.GetParaEnity();
            //读表配置KEY
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            //读表结构
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));

            string PostStr = ApiObj.postdata.ParentData.ToString();//前端数据

            string UnitCode = string.Empty;//F-002000-100125  机构新增服务编码

            HelpConst Queyr = new HelpConst();
            Queyr.ThisConn = ThisConn;
            /*取最后编码*/
            string Json = Queyr.GetConstLastCode(Struct, JObject.Parse(PostStr));

            JObject Obj = JObject.Parse(Json);
            JArray Data = (JArray)Obj["Data"];
            string SourceCode = Obj["SourceCode"].ToString();
            string Code = (Data.Count > 0) ?
                Data[0]["Code"].ToString() : string.Empty;
            /*取最新编码*/
            JObject obj = Queyr.CalculLastCode(Code, SourceCode);


            /*封装常量写入*/
            JObject ClientObj = JObject.Parse(PostStr);
            ClientObj["Code"] = obj["Code"].ToString();
            ClientObj["UpperCode"] = ClientObj["UpperCode"].ToString() == "*"
                        ? "" : ClientObj["UpperCode"].ToString();
            if (ApiObj.servercode == "F-002000-100125")//机构增加CODE=UNITCODE
            {
                ClientObj.Add(new JProperty("UnitCode", obj["Code"].ToString()));                
            }
            /*新增树形CODE非自动生成，且必需有uppercode 默认*代表空字符*/

            /*返回前端寄存数据*/
            JObject NewObj = new JObject(
                    new JProperty("Code", obj["Code"].ToString()),
                    new JProperty("CodeCn", ClientObj["CodeCn"].ToString()),
                    new JProperty("UpperCode", (ClientObj["UpperCode"].ToString() == "*")
                        ? ""
                        : ClientObj["UpperCode"].ToString()),
                    new JProperty("ReMarks", (Mo.JtokenFlg(ClientObj["ReMarks"]))
                        ? ClientObj["ReMarks"].ToString()
                        : "")
                );

            ApiObj.postdata.ParentData = ClientObj;
            MsIns ins = new MsIns();
            ins.ThisConn = ThisConn;
            string Result = Task.Run(() => ins.MsInsSingleNoCode(Struct, ApiObj)).Result;

            Obj = JObject.Parse(Result);
            if ((bool)Obj["scuess"] == true)//执行成功把值返回前端
            {
                dynamic DyObj = new DynamicObj();
                DyObj.Data = NewObj;
                Result = Mo.GetResult(true, "执行成功", true, DyObj._values, true);
            }

            return Result;
        }
        /// <summary>
        /// 单表更新模式
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("UpSingleData")]
        public string UpSingleData([FromBody] dynamic Param)
        {
            //string Conn = _configuration.GetValue<String>("ConnectionStr").ToString();//取数据库连接            
            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));
            if (Struct["Tb"].ToString() == "Tbl_User" &&
                Mo.JtokenFlg(ApiObj.postdata.ParentData["NewValue"]["Pwd"]))
            {
                string Pwd = ApiObj.postdata.ParentData["NewValue"]["Pwd"].ToString();
                Pwd = CompressByte.MD5_32X(Pwd); ;
                ApiObj.postdata.ParentData["NewValue"]["Pwd"] = Pwd;
            }
            MsUp Up = new MsUp();
            Up.ThisConn = ThisConn;
            Result = Task.Run(() => Up.MsUpSingle(Struct, ApiObj)).Result;

            return Result;
        }
        
        /// <summary>
        /// 新增服务配置 Code 需要独立计算
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("ServiceIns")]
        public string ServiceIns([FromBody] dynamic Param)
        {
            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();

            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));

            HelpConst GetCode = new HelpConst();
            GetCode.ThisConn = ThisConn;

            string Code = GetCode.GetServerCode(ApiObj.postdata.ParentData["SysCode"].ToString(),
                ApiObj.postdata.ParentData["CodeType"].ToString());

            ApiObj.postdata.ParentData["Code"] = Code;
            MsIns Ins = new MsIns();
            Ins.ThisConn = ThisConn;
            Result = Task.Run(() => Ins.MsInsSingleNoCode(Struct, ApiObj)).Result;
            return Result;
        }
        /*配置注册  对应tbl_config 缓存更新*/
        [HttpPost("RegServiceUp")]
        public string RegServiceUp([FromBody] dynamic Param)
        {
            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            JObject RedisCfg = JObject.Parse(Host.CashGetKey(ApiObj.servercode, "ServerCode"));
            JObject Struct = JObject.Parse(Host.CashGetKey(RedisCfg["Struct"].ToString(), "TbStruct"));
            MsFormQuery Queyr = new MsFormQuery();
            Queyr.ThisConn = ThisConn;
            string Qresult = Task.Run(() => Queyr.QuerySingle(Struct, ApiObj)).Result;
            JObject Qobj = JObject.Parse(Qresult);
            JArray List = (JArray)Qobj["Result"]["Data"];
            /*Redis 结构定义*/
            ServiceStruct.ServiceModel Reg = new ServiceStruct.ServiceModel()
            {
                Struct = List[0]["ServiceDefinition"].ToString(),
                DataService = List[0]["Url"].ToString(),
                DataUrl = List[0]["Api"].ToString(),
                ReqType = List[0]["RequestMothed"].ToString(),
                IsEnabled = List[0]["IsEnabled"].ToString(),
                Status = List[0]["Status"].ToString(),
                SysCode = List[0]["SysCode"].ToString(),
                Code = List[0]["Code"].ToString(),
                CusField = List[0]["ReMarks"].ToString(),
                Port = List[0]["Port"].ToString()
            };
            Redis.DbType = "ServerCode";
            Redis.Default.StringSet(List[0]["Code"].ToString().Trim(),
                JsonConvert.SerializeObject(Reg));
            /*Redis 结构定义*/

            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlCmd = $@"
                update {Struct["Db"]}.dbo.{Struct["Tb"]}
                    set [Status]='001001',
                        [IsEnabled]='000001'
                where Code=@Code
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Code",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = List[0]["Code"].ToString()
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            int rows = Task.Run(() => dal.UpOrIns(WordArea.SqlCmd, WordArea.SqlParas)).Result;

            Result = Mo.GetResult(true, "注册成功", false, new object(), true);
            return Result;
        }
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetServiceModel")]
        public string GetServiceModel([FromBody] dynamic Param)
        {
            string Result = string.Empty;
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            string MenuCode = ApiObj.postdata.ParentData["Code"].ToString();
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();

            WordArea.SqlCmd = $@"
                SELECT [Code]
                  ,[CodeCn] as Name
                  ,[ServiceCode]
                  ,[ServiceName]
                  ,[ServiceType]
                  ,[ReqServercode]
                  ,[ReqType]
                  ,[IsCreateWin]
                  ,[WinMothed]
                  ,[ParasResult]
                  ,[BtnOwerId]
                  ,[IfGetId]
                  ,[ParentCode]
              FROM [SysConfig].[dbo].[Tbl_SysModel] WITH(NOLOCK)
                where Code like ''+@Code+'%' 
                and DelFlg=1 and [IsEnabled]='000001'
                and len(Code)=9 order by Code
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Code",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = MenuCode
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            DataTable Btn = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            dynamic DyObj = new DynamicObj();
            DyObj.Data = Btn;
            Result = Mo.GetResult(true, "执行成功", true, DyObj._values, true);
            return Result;
        }
        /*模块注册  对应tbl_sysmodel 缓存更新  由前端提交缓存数据*/
        [HttpPost("RegServiceModel")]
        public string RegServiceModel([FromBody] dynamic Param)
        {
            string Result = string.Empty;
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            #region 由前端提交缓存数据,不需要读数据库
            /*
            string MenuCode = ApiObj.postdata.ParentData["Code"].ToString();
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlCmd = $@"
                SELECT [Code]
                  ,[CodeCn] as Name
                  ,[ServiceCode]
                  ,[ServiceName]
                  ,[ServiceType]
                  ,[ReqServercode]
                  ,[ReqType]
                  ,[IsCreateWin]
                  ,[WinMothed]
                  ,[ParasResult]
                  ,[BtnOwerId]
                  ,[IfGetId]
                  ,[ParentCode]
              FROM [SysConfig].[dbo].[Tbl_SysModel]
                where Code like ''+@Code+'%' 
                and DelFlg=1 and [IsEnabled]='000001'
                and len(Code)=9 order by Code
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Code",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = MenuCode
                });
            DataTable Btn = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            JArray BtnPara = new JArray();
            for (int x = 0; x < Btn.Rows.Count; x++)
            {
                //Redis 结构定义
                ServiceStruct.MenuModel Reg = new ServiceStruct.MenuModel()
                {
                    Code= Btn.Rows[x]["Code"].ToString(),
                    Name = Btn.Rows[x]["Name"].ToString(),
                    ServiceCode = Btn.Rows[x]["ServiceCode"].ToString(),
                    ServiceName = Btn.Rows[x]["ServiceName"].ToString(),
                    ServiceType = Btn.Rows[x]["ServiceType"].ToString(),
                    ReqServercode = Btn.Rows[x]["ReqServercode"].ToString(),
                    ReqType = Btn.Rows[x]["ReqType"].ToString(),
                    IsCreateWin = Btn.Rows[x]["IsCreateWin"].ToString(),
                    WinMothed = Btn.Rows[x]["WinMothed"].ToString(),
                    ParasResult = Btn.Rows[x]["ParasResult"].ToString(),
                    BtnOwerId = Btn.Rows[x]["BtnOwerId"].ToString(),
                    IfGetId = Btn.Rows[x]["IfGetId"].ToString()                    
                };
                
                BtnPara.Add(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(Reg)));
            }

            JObject Jobj = JObject.Parse(Result);
            if ((bool)Jobj["scuess"])
            {
                Redis.DbType = "MenuCode";
                Redis.Default.StringSet(MenuCode,
                    JsonConvert.SerializeObject(BtnPara));
            }
            */
            #endregion

            //写缓存
            JObject Btn = ApiObj.postdata.ParentData;
            JArray Para = JArray.Parse(Btn["Code"].ToString());
            string ServerCode = Para[0]["ParentCode"].ToString();
            Redis.DbType = "MenuCode";
            Redis.Default.StringSet(ServerCode, Btn["Code"].ToString());
            //写文件
            string Path = System.Configuration.ConfigurationManager.AppSettings["ConFigPath"];
            Path = $"{Path}\\BtnPara\\";
            FileHelper.FileCreate(Path, Btn["Code"].ToString(), $"{ServerCode}.json");
            Result = Mo.GetResult(true, "注册成功", false, new object(), true);
            return Result;
        }


        /// <summary>
        /// 对应表结构缓存更新
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>

        [HttpPost("RegTableStruct")]
        public string RegTableStruct([FromBody] dynamic Param)
        {
            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();

            //写缓存            

            string ServerCode = ApiObj.servercode;
            Redis.DbType = "ServerCode";

            string TbName = ApiObj.postdata.ParentData["FileName"].ToString();
            string Para = ApiObj.postdata.ParentData["Para"].ToString();
            Redis.DbType = "TbStruct";
            Redis.Default.StringSet(TbName, Para);
            //写文件
            string Path = System.Configuration.ConfigurationManager.AppSettings["ConFigPath"];
            Path = $"{Path}\\Tbstruct\\";
            FileHelper.FileCreate(Path, Para, TbName);
            Result = Mo.GetResult(true, "注册成功", false, new object(), true);
            return Result;

        }
        /// <summary>
        /// 获取系统全部权限
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetRightsAll")]
        public string GetRightsAll([FromBody] dynamic Param)
        {
            string Result = string.Empty;
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            #region  读全部模块功能
            string OwerRole = ApiObj.postdata.userkey.Role;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            string SysCode = ApiObj.postdata.userkey.SysCode;
            string Role = ApiObj.postdata.ParentData["Role"].ToString();
            WordArea.SqlCmd = $@"
                SELECT  
	                  [Code]
                      ,[CodeCn]
                      ,[UpperCode]
                      ,[ParentCode]     
                      ,[ServiceType]
                      ,[SysCode],Flg=0
                  FROM [SysConfig].[dbo].[Tbl_SysModel] WITH(NOLOCK)
                    where Delflg=1 
                    --and SysCode=@SysCode
                  order by Code
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@SysCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = SysCode
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            DataTable RightsAll = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            //存在的角色权限
            WordArea.SqlCmd = string.Empty;
            WordArea.SqlParas.ValuePara.Clear();
            WordArea.SqlCmd = $@"
                SELECT 
                      [CodeCn]
                      ,[RouleCode]
                      ,[ModelCode]
                      ,[SysCode]
                      ,[InterfaceCode]
                  FROM [SysConfig].[dbo].[Tbl_RoleRights] WITH(NOLOCK)
                    where 
                        RouleCode=@RouleCode
                        --and SysCode=@SysCode
                  order by ModelCode
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@RouleCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Role
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@SysCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = SysCode
                });
            DataTable RightsRole = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            if (RightsRole.Rows.Count > 0)
            {
                /*标记已经存在的权限，用于前checkbox*/
                int ExistsLen = RightsRole.Rows.Count;
                int RightsLen = RightsAll.Rows.Count;
                for (int x = 0; x < ExistsLen; x++)
                {
                    for (int y = 0; y < RightsLen; y++)
                    {
                        if (RightsRole.Rows[x]["ModelCode"].ToString().Trim() ==
                            RightsAll.Rows[y]["Code"].ToString().Trim())
                        {
                            RightsAll.Rows[y]["Flg"] = 1;
                            break;
                        }
                    }
                }
            }
            #endregion
            dynamic DyObj = new DynamicObj();
            DyObj.Data = RightsAll;
            Result = Mo.GetResult(true, "执行成功", true, DyObj._values, true);
            return Result;
        }
        [HttpPost("RegRoleRights")]
        public string RegRoleRights([FromBody] dynamic Param)
        {
            string Result = string.Empty;
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            bool UnitFlg = false;
            #region  授权之前，先删除已经存在的全部权限
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();

            string Role = ApiObj.postdata.ParentData["Role"].ToString();

            WordArea.SqlCmd = $@"
                delete from [SysConfig].[dbo].[Tbl_RoleRights]
                    where 
                        RouleCode=@RouleCode  ;                 
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@RouleCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Role
                });


            WordArea.EnityList.ValuePara.Add(WordArea.SqlParas);
            WordArea.HsTb.Add(new DataHandle.SqlParaResult
            {
                SqlCmd = WordArea.SqlCmd,
                ValuePara = WordArea.EnityList
            });
            WordArea = Mo.ClearResult(WordArea, true);
            #endregion
            JArray Data = JArray.Parse(ApiObj.postdata.ParentData["Data"].ToString());
            string SqlCmd = string.Empty;
            SqlCmd = $@"
                insert into [SysConfig].[dbo].[Tbl_RoleRights]
                (
                  [Code]
                  ,[RouleCode]
                  ,[CodeCn]                  
                  ,[ModelCode]
                  ,[SysCode]
                  ,[InterfaceCode])
                values 
                (
                  @Code                  
                  ,@RouleCode
                  ,@CodeCn
                  ,@ModelCode
                  ,@SysCode
                  ,@InterfaceCode);
                ";
            string Code = string.Empty;

            for (int x = 0; x < Data.Count; x++)
            {
                JObject Rows = JObject.Parse(Data[x].ToString());
                Code = CompressByte.GetCustomCode(true, "R");//取一个权限表新Code
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@Code",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = Code
                    });
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@RouleCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = Role
                    });
                foreach (var item in Rows)
                {
                    WordArea.SqlParas.ValuePara.Add(
                        new DataHandle.SQLtype
                        {
                            ColName = $"@{item.Key}",
                            ColType = Mo.ConvertSQLType("nvarchar"),
                            ColLeng = 50,
                            ColValue = $"{item.Value}"
                        });
                }
                WordArea.EnityList.ValuePara.Add(WordArea.SqlParas);
                WordArea.SqlParas = new DataHandle.SqlPara();
            }
            WordArea.HsTb.Add(new DataHandle.SqlParaResult
            {
                SqlCmd = SqlCmd,
                ValuePara = WordArea.EnityList
            });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            bool Flg = Task.Run(() => dal.UpOrIns(WordArea.HsTb)).Result;

            Result = Mo.GetResult(true, "授权成功", false, new object(), true);
            return Result;
        }

        /// <summary>
        /// 获取List  字段集合
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>

        [HttpPost("GetListFieldResult")]
        public string GetListFieldResult([FromBody] dynamic Param)
        {
            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            string TbName = ApiObj.postdata.ParentData["FileName"].ToString();
            MsGetListFields GetFields = new MsGetListFields();
            string TbFields = GetFields.GetListFields(TbName);
            dynamic DyObj = new DynamicObj();
            DyObj.Data = TbFields;
            Result = Mo.GetResult(true, "执行成功", true, DyObj._values, true);
            return Result;

        }
    }
}
