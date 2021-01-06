using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
namespace HelperTools
{
    public class MsDel
    {
        public int Ipage = 20;
        public string DataConnType = "mssql";
        IDal dal;
        public string ThisConn;
        public DataHandle Mo = new DataHandle();
        public RedisHelper Redis = new RedisHelper() { DbType = "TbStruct" };
        public HostReqModel Host = new HostReqModel();
        public DelLog Del = new DelLog();
        /// <summary>
        /// 以CODE做删除主键
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string DelKeyCode(JObject Struct, RequestPagePara.AllParaResult Data)
        {
            string UnitCode = Data.postdata.userkey.UnitCode;
            string Roule = Data.postdata.userkey.Role;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlCmd = $"update  {Struct["Db"]}.dbo.{Struct["Tb"]}" +
                $" set DelFlg=0 " +
                $" where Code=@Code ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Code",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Data.postdata.ParentData["Code"].ToString()
                });
            if (Roule != "000000")//非配置管理员，只可以删除本群组及下级组信息
            {
                WordArea.SqlCmd += " and UnitCode like ''+@UnitCode+'%' ";
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@UnitCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = UnitCode
                    });
            }
            
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            int Flg = Task.Run(() =>
                dal.UpOrIns(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            
            if (Flg > 0)
            {
                WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);
                InsLog(Struct["Db"].ToString(), 
                    Struct["Tb"].ToString(), 
                    Data.postdata.userkey.User,""
                    ,Data.postdata.ParentData["Code"].ToString());
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "执行失败", false, new object(), true);
                LogHelper.Error(WordArea.SqlCmd);
            }
            return WordArea.Result;
        }
        /// <summary>
        /// 以ID做删除主键
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string DelKeyId(JObject Struct, RequestPagePara.AllParaResult Data)
        {
            string UnitCode = Data.postdata.userkey.UnitCode;
            string Roule = Data.postdata.userkey.Role;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlCmd = $"update  {Struct["Db"]}.dbo.{Struct["Tb"]} " +
                $" set DelFLG=0" +
                $" where ID=@ID  ";//AND UnitCode=@UnitCode
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@ID",
                    ColType = Mo.ConvertSQLType("bigint"),
                    ColLeng = 8,
                    ColValue = Data.postdata.ParentData["ID"].ToString()
                });
            if (Roule != "000000")//非配置管理员，只可以删除本群组及下级组信息
            {
                WordArea.SqlCmd += " and UnitCode =@UnitCode ";
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@UnitCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = UnitCode
                    });
            }
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            int Flg = Task.Run(() =>
                dal.UpOrIns(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            if (Flg > 0)
            {
                WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);
                InsLog(Struct["Db"].ToString(),
                    Struct["Tb"].ToString(),
                    Data.postdata.userkey.User,
                    Data.postdata.ParentData["ID"].ToString()
                    , "");
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "执行失败", false, new object(), true);
                LogHelper.Error(WordArea.SqlCmd);
            }
            return WordArea.Result;
        }
        /// <summary>
        /// 以参数做删除条件，参数包括库、表、WHERE 字段及值
        /// 参数只能以ID,Code,ParentCode 三个任意一个或全部
        /// </summary>
        /// <returns></returns>
        public string GobalDelPara(RequestPagePara.AllParaResult Data)
        {
            string UnitCode = Data.postdata.userkey.UnitCode;
            string Roule = Data.postdata.userkey.Role;
            string WherePara = string.Empty;
            JObject Para = Data.postdata.ParentData;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            
            WordArea.SqlCmd = $"update {Para["Db"]}.dbo.{Para["Tb"]} " +
                $"set DelFlg=0  " ;
            foreach (var item in Para)
            {
                if (item.Key != "Db" && item.Key != "Tb")
                {
                    if (item.Key == "ID")
                    {
                        WherePara += (string.IsNullOrEmpty(WherePara)) 
                            ? " ID=@ID" : " and ID=@ID";
                        WordArea.SqlParas.ValuePara.Add(
                            new DataHandle.SQLtype
                            {
                                ColName = $"@ID",
                                ColType = Mo.ConvertSQLType("bigint"),
                                ColLeng = 8,
                                ColValue = item.Value.ToString()
                            });
                        break;
                    }
                    else
                    {
                        WherePara += (string.IsNullOrEmpty(WherePara))
                            ? $" {item.Key}=@{item.Key}" : $" and {item.Key}=@{item.Key}";
                        WordArea.SqlParas.ValuePara.Add(
                            new DataHandle.SQLtype
                            {
                                ColName = $"@{item.Key}",
                                ColType = Mo.ConvertSQLType("nvarchar"),
                                ColLeng = 50,
                                ColValue = item.Value.ToString()
                            });
                        break;
                    }
                }
            }
            WordArea.SqlCmd += $" where {WherePara} ";
            if (Roule != "000000")//非配置管理员，只可以删除本群组及下级组信息
            {
                WordArea.SqlCmd += " and UnitCode like ''+@UnitCode+'%' ";
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@UnitCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = UnitCode
                    });
            }
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            int Flg = Task.Run(() =>
                dal.UpOrIns(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            if (Flg > 0)
            {
                WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);
                foreach (var item in Para)
                {
                    if (item.Key != "Db" && item.Key != "Tb")
                    {
                        if (item.Key == "ID")
                        {
                            InsLog(Para["Db"].ToString(),
                                Para["Tb"].ToString(),
                                Data.postdata.userkey.User,
                                item.Value.ToString()
                                , "");
                            break;
                        }
                        else 
                        {
                            InsLog(Para["Db"].ToString(),
                                Para["Tb"].ToString(),
                                Data.postdata.userkey.User,
                                ""
                                , item.Value.ToString());
                            break;
                        }                  
                    }
                }
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "执行失败", false, new object(), true);
                LogHelper.Error(WordArea.SqlCmd);
            }
            return WordArea.Result;
        }
        /// <summary>
        /// 物理删除 附件删除专用
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string DelAttachPara(JObject Struct, RequestPagePara.AllParaResult Data)
        {
            string UnitCode = Data.postdata.userkey.UnitCode;
            string Roule = Data.postdata.userkey.Role;
            JObject Para = Data.postdata.ParentData;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlCmd = $@"
                delete from {Struct["Db"]}.dbo.{Struct["Tb"]}
                where AgainName=@AgainName
            ";
            WordArea.SqlParas.ValuePara.Add(
            new DataHandle.SQLtype
            {
                ColName = $"@AgainName",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 100,
                ColValue = Para["AgainName"].ToString()
            });

            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            int Flg = Task.Run(() =>
                dal.UpOrIns(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            if (Flg > 0)
            {
                WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "执行失败", false, new object(), true);
            }
            return WordArea.Result;
        }
        /// <summary>
        /// 物理删除 
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string DelTruePara(JObject Struct, RequestPagePara.AllParaResult Data)
        {
            string UnitCode = Data.postdata.userkey.UnitCode;
            string Roule = Data.postdata.userkey.Role;
            JObject Para = Data.postdata.ParentData;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlCmd = $"delete from {Struct["Db"]}.dbo.{Struct["Tb"]}";
            string Where = string.Empty;
            foreach (var item in Para)
            {

                var ColObj =
                    from p in
                        Struct["Fields"].Where
                        (
                            p => p["Field"].ToString().ToUpper().
                            Equals(item.Key.ToString().ToUpper())
                        )
                    select p;

                JArray Col = (JArray)JsonConvert.
                    DeserializeObject(JsonConvert.SerializeObject(ColObj));
                if (Col.Count > 0)
                {
                    string L = Mo.ConvertDataType(Col[0]["DataType"].ToString());
                    Where += (string.IsNullOrEmpty(Where))
                        ? $@" {item.Key}=@{item.Key} "
                        : $@" and {item.Key}=@{item.Key} ";
                    WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@{item.Key}",
                        ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                        ColLeng = (int)Col[0]["MaxLeng"],
                        ColValue = item.Value.ToString()
                    });
                }
            }
            WordArea.SqlCmd += $" where {Where}";
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            int Flg = Task.Run(() =>
                dal.UpOrIns(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            if (Flg > 0)
            {
                WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "执行失败", false, new object(), true);
            }
            return WordArea.Result;
        }
        public void InsLog(string Db,string Tb,string Author, string ParentId="",string ParentCode="")
        { 
            string InsLog = $@"
                INSERT INTO [SysLog].[dbo].[Tbl_Log]
                           ([ParentId]
                           ,[ParentCode]
                           ,[OldRemarks]
                           ,[NewRemarks]
                           ,[Db]
                           ,[Tb]
                           ,[Author]) values
                (@ParentId,@ParentCode,@OldRemarks,@NewRemarks,@Db,@Tb,@Author)
            ";
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@ParentId",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = ParentId
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@ParentCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = ParentCode
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@OldRemarks",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Del.Oldvalue
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@NewRemarks",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Del.Newvalue
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Db",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Db
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Tb",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Tb
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Author",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Author
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            int Flg = Task.Run(() =>
                dal.UpOrIns(InsLog, WordArea.SqlParas)).Result;
        }
        public class DelLog
        {
            public string Oldvalue { get; set; } = "{\"DelFlg\":1}";
            public string Newvalue { get; set; } = "{\"DelFlg\":0}";
        }
    }
}
