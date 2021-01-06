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

namespace IdentityCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
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
        public IdentityController(IConfiguration configuration)
        {
            _configuration = configuration;
            ThisConn = _configuration.GetValue<String>("ConnectionStr").ToString();
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public string Login([FromBody] dynamic Param)
        {
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            JObject Para = ApiObj.postdata.ParentData;
            string Acc = Para["Username"].ToString();
            string Pwd = Para["Password"].ToString();
            string SysCode = Para["SysCode"].ToString();
            Pwd = CompressByte.MD5_32X(Pwd);

            string Uuid = Guid.NewGuid().ToString("N");
            WordArea.SqlCmd = $@"
              SELECT A.[Account] as [User],
                  A.[Name] as UserCn,
                  B.[Code] AS UnitCode,
                  B.[CodeCn] AS UnitCn,
                  A.[Type] AS UserType,
                  A.[RoleCode] AS Role,
                  Token='{Uuid}',
                  A.SysCode,B.IsEnabled
              FROM [SysUser].[dbo].[Tbl_User] A  WITH(NOLOCK)
              INNER JOIN [SysConfig].[dbo].[Tbl_Uint] B  WITH(NOLOCK)
              ON A.UnitCode=B.Code
              where a.DelFlg=1 and a.IsEnabled=@IsEnabled 
                and A.[Account]=@Account and A.Pwd=@Pwd and SysCode=@SysCode
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@IsEnabled",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = "000001"
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Account",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Acc
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Pwd",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Pwd
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@SysCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = SysCode
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            DataTable Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            string Result = string.Empty;
            Obj.Data = Dt;
            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["IsEnabled"].ToString() == "000001")
                {
                    Result = Mo.GetResult(true, "登录成功", true, Obj._values, true);
                }
                else
                {
                    Result = Mo.GetResult(false, "机构没有启用", false, new object(), false);
                }
            }
            else
            {
                Result = Mo.GetResult(false, "帐号或者密码错误", false, Obj._values, false);
            }
            return Result;
        }


        /// <summary>
        /// 菜单加载
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("LoadMenu")]
        public string LoadMenu([FromBody] dynamic Param)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            #region 对应角色权限
            string Role = ApiObj.postdata.userkey.Role;
            string ScoendMenu = string.Empty;
            string FirstMenu = string.Empty;
            if (Role != "000000")
            {
                WordArea.SqlCmd = $@"
                SELECT 
                       [Code]                      
                      ,[ModelCode]                      
                  FROM [SysConfig].[dbo].[Tbl_RoleRights] WITH(NOLOCK)
                    where [RouleCode]=@RouleCode
                        and SysCode=@SysCode
                        and [IsEnabled]='000001'
                        AND LEN([ModelCode])=3
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
                DataTable RoleMenu = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;

                for (int x = 0; x < RoleMenu.Rows.Count; x++)
                {
                    FirstMenu += string.IsNullOrEmpty(FirstMenu)
                        ? $"'{RoleMenu.Rows[x]["ModelCode"].ToString().Trim()}'"
                        : $",'{RoleMenu.Rows[x]["ModelCode"].ToString().Trim()}'";
                }
                if (string.IsNullOrEmpty(FirstMenu))
                {
                    FirstMenu = " and Code in ('***') ";
                }
                else
                {
                    FirstMenu = $" and Code in ({FirstMenu}) ";
                }
                WordArea.SqlCmd = $@"
                SELECT 
                       [Code]                      
                      ,[ModelCode]                      
                  FROM [SysConfig].[dbo].[Tbl_RoleRights] WITH(NOLOCK)
                    where [RouleCode] =@RouleCode 
                        and SysCode=@SysCode
                        and [IsEnabled]='000001'
                        AND LEN([ModelCode])=6
                    order by ModelCode
            ";
                DataTable RoleBtn = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;

                for (int x = 0; x < RoleBtn.Rows.Count; x++)
                {
                    ScoendMenu += string.IsNullOrEmpty(ScoendMenu)
                        ? $"'{RoleBtn.Rows[x]["ModelCode"].ToString().Trim()}'"
                        : $",'{RoleBtn.Rows[x]["ModelCode"].ToString().Trim()}'";
                }
                if (string.IsNullOrEmpty(ScoendMenu))
                {
                    ScoendMenu = " and Code in ('******') ";
                }
                else
                {
                    ScoendMenu = $" and Code in ({ScoendMenu}) ";
                }
            }

            #endregion 对应角色权限
            WordArea.SqlParas.ValuePara.Clear();
            #region 取菜单
            WordArea.SqlCmd = $@"SELECT 
                    Code,
                    [ParasResult] as dataurl,
                    [UnCODE] as dataicon,
                    [CodeCn] as datatitle                                  
                    FROM [SysConfig].[dbo].[Tbl_SysModel]  WITH(NOLOCK)
                    where DelFlg=@DelFlg  AND len(Code)=3 
                        {FirstMenu}
                        and SysCode=@SysCode 
                        and IsEnabled='000001'
                        order by SortNum,Code";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@DelFlg",
                    ColType = Mo.ConvertSQLType("int"),
                    ColLeng = 8,
                    ColValue = "1"
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@SysCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = ApiObj.postdata.userkey.SysCode
                });

            DataTable Dt3 = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            WordArea.SqlCmd = $@"SELECT 
                    Code,
                    [ParasResult] as dataurl,
                    [UnCODE] as dataicon,
                    [CodeCn] as datatitle                                  
                    FROM [SysConfig].[dbo].[Tbl_SysModel]  WITH(NOLOCK)
                    where DelFlg=@DelFlg  AND len(Code)=6 
                        {ScoendMenu}
                        and SysCode=@SysCode  
                        and IsEnabled='000001'
                        order by SortNum,Code";
            DataTable Dt6 = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            JArray LiArr = new JArray();
            JArray DlArr = new JArray();
            JObject DD = null;
            int IdNum = 1;

            string Code = string.Empty;
            for (int x = 0; x < Dt3.Rows.Count; x++)
            {
                Code = Dt3.Rows[x]["Code"].ToString().Trim();
                DlArr = new JArray();
                for (int y = 0; y < Dt6.Rows.Count; y++)
                {
                    if (Code == Dt6.Rows[y]["Code"].ToString().Trim().Substring(0, 3))
                    {
                        DD = new JObject {
                            { "dataurl", Dt6.Rows[y]["dataurl"].ToString().Trim() },
                            {"dataicon", Dt6.Rows[y]["dataicon"].ToString().Trim() },
                            {"datatitle", Dt6.Rows[y]["datatitle"].ToString().Trim()},
                            {"dataid", IdNum.ToString()},
                            {"layuiicon", Dt6.Rows[y]["dataicon"].ToString().Trim() },
                            {"span", Dt6.Rows[y]["datatitle"].ToString().Trim() }
                        };
                        IdNum++;
                        DlArr.Add(DD);
                    }
                }
                DD = new JObject();
                DD.Add(new JProperty("li", Dt3.Rows[x]["datatitle"].ToString().Trim()));
                DD.Add(new JProperty("liico", Dt3.Rows[x]["dataicon"].ToString().Trim()));
                DD.Add(new JProperty("dl", DlArr));
                LiArr.Add(DD);
            }
            #endregion
            WordArea.SqlParas.ValuePara.Clear();
            #region 取常量
            WordArea.SqlCmd = $@"SELECT [Code],[CodeCn]                                  
                    FROM [SysConst].[dbo].[Tbl_Const]  WITH(NOLOCK)
                    where DelFlg=1 and IsEnabled=@IsEnabled ORDER by Code";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@IsEnabled",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = "000001"
                });
            DataTable DtConst = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            #endregion
            WordArea.SqlParas.ValuePara.Clear();
            #region 取机构
            string UnitCode = ApiObj.postdata.userkey.UnitCode;
            WordArea.SqlCmd = $@"
              SELECT [Code] as UnitCode,
                  [CodeCn] as UnitCn,
                  [Address] as UnitAdders,
                  [Tel] as UnitTel,
                  [Type] as UnitType,
                  [LinkMan] as UnitLinkMan      
              FROM [SysConfig].[dbo].[Tbl_Uint]   WITH(NOLOCK)
                where [Code]=@UnitCode
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@UnitCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = UnitCode
                });
            DataTable DtUnit = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            #endregion
            Obj.Data = LiArr;
            Obj.DtConst = DtConst;
            Obj.DtUnit = DtUnit;
            string Result = Mo.GetResult(true, "查询成功", true, Obj._values, true);
            return Result;
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("CancelUserKey")]
        public string CancelUserKey([FromBody] dynamic Param)
        {
            string Result = string.Empty;

            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            Redis.DbType = "UserKey";
            bool Del = Redis.Default.KeyDelete(ApiObj.postdata.userkey.Token.ToString());
            Result = Mo.GetResult(true, "成功注销", false, new object(), true);
            return Result;
        }
    }
}
