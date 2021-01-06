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
    public class MsUp
    {
        public int Ipage = 20;
        public string DataConnType = "mssql";
        IDal dal;
        public string ThisConn;
        public DataHandle Mo = new DataHandle();
        public RedisHelper Redis = new RedisHelper() { DbType = "TbStruct" };
        public HostReqModel Host = new HostReqModel();
        /// <summary>
        /// 单表模式  参数模式
        /// </summary>        
        /// <param name="Struct"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string MsUpSingle(JObject Struct, RequestPagePara.AllParaResult Data)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();

            JToken Exists = Data.postdata.ParentData["NewValue"];
            if (!Mo.JtokenFlg(Exists))
            {
                WordArea.Result = Mo.GetResult(false, "参数错误", false, new object(), true);
                LogHelper.Error(JsonConvert.SerializeObject(Data));
            }
            else
            {
                JObject DataObj = (JObject)Data.postdata.ParentData["NewValue"];
                WordArea.ID = DataObj["ID"].ToString();
                WordArea.Code = DataObj["Code"].ToString();
                //WordArea.UnitCode= DataObj["UnitCode"].ToString();
                WordArea.Fields = string.Empty;
                WordArea.Values = string.Empty;
                WordArea.SqlCmd = $" update {Struct["Db"]}.dbo.{Struct["Tb"]} set ";
                DataHandle.SqlPara SqlParas = new DataHandle.SqlPara();

                foreach (var Datakey in DataObj)
                {
                    if (Datakey.Key.ToString().ToUpper() != "ID"                         
                        && Datakey.Key.ToString().ToUpper() != "CODE")
                    {
                        var YesChgCol =
                        from p in Struct["Fields"].Where
                        (
                            p => p["Field"].ToString().ToUpper().
                            Equals(Datakey.Key.ToString().ToUpper())
                        )
                        select p;
                        JArray Col = (JArray)JsonConvert.
                            DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                        if (Col.Count > 0)
                        {
                            WordArea.Fields += (string.IsNullOrEmpty(WordArea.Fields))
                                ? $" {Datakey.Key}=@{Datakey.Key}"
                                : $",{Datakey.Key}=@{Datakey.Key}";

                            SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                            {
                                ColName = $"@{Col[0]["Field"].ToString()}",
                                ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                ColLeng = (int)Col[0]["MaxLeng"],
                                ColValue = Datakey.Value.ToString()
                            });
                        }
                    }
                }
                WordArea.Fields = Mo.PackUpInsField(WordArea.Fields);
                SqlParas = Mo.PackUpInsSqlParas(SqlParas,
                    JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)));
                if (SqlParas.ValuePara.Count > 0)
                {
                    //WordArea.SqlCmd = $"{WordArea.SqlCmd} {WordArea.Fields} where ID=@ID and UnitCode=@UnitCode;";
                    WordArea.SqlCmd = $"{WordArea.SqlCmd} {WordArea.Fields} where ID=@ID ;";
                    SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = $"@ID",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = WordArea.ID
                    });
                    /*
                    SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = $"@UnitCode",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = WordArea.UnitCode
                    });
                    */
                    
                    dal = DalFactory.CreateDal(DataConnType, ThisConn);
                    int rows = Task.Run(() => dal.UpOrIns(WordArea.SqlCmd, SqlParas)).Result;


                    if (rows > 0)
                    {
                        WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);
                        /*日志*/
                        JObject ChangeValue =(JObject)Data.postdata.ParentData["NewValue"];
                        ChangeValue.Remove("ID");
                        ChangeValue.Remove("Code");
                        bool LogFlg = Task.Run(() => MsInsSyslog(
                             WordArea.Code,
                             Struct,
                             ChangeValue.ToString(),
                             Data.postdata.ParentData["OldValue"].ToString(),
                             JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)))).Result;
                    }
                    else
                    {
                        WordArea.Result = Mo.GetResult(false, "写入数据不成功", false, new object(), true);
                    }
                }
                else
                {
                    WordArea.Result = Mo.GetResult(false, "没有操作数据", false, new object(), true);
                }
            }

            return WordArea.Result;
        }

        /// <summary>
        /// 多表模式  参数模式
        /// </summary>       
        /// <param name="Struct"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string MsUpMany(JObject Struct, RequestPagePara.AllParaResult Data)
        {

            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();

            JToken Exists = Data.postdata.ParentData["NewValue"];
            if (Mo.JtokenFlg(Exists))
            {
                JObject DataObj = (JObject)Data.postdata.ParentData["NewValue"];

                WordArea.ID = DataObj["ID"].ToString();
                WordArea.Code = DataObj["Code"].ToString();
                WordArea.UnitCode = DataObj["UnitCode"].ToString();


                #region  主表
                WordArea.SqlCmd = $" update {Struct["Db"]}.dbo.{Struct["Tb"]} set ";

                foreach (var Datakey in DataObj)
                {
                    if (Datakey.Key.ToString() != "ID" &&
                        Datakey.Key.ToString() != "Code"
                        && Datakey.Key.ToString() != "UnitCode")
                    {
                        var YesChgCol =
                        from p in Struct["Fields"].Where
                        (
                            p => p["Field"].ToString().ToUpper().
                            Equals(Datakey.Key.ToString().ToUpper())
                        )
                        select p;
                        JArray Col = (JArray)JsonConvert.
                            DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                        if (Col.Count > 0)
                        {
                            WordArea.Fields += (string.IsNullOrEmpty(WordArea.Fields))
                                ? $" {Datakey.Key}=@{Datakey.Key}"
                                : $",{Datakey.Key}=@{Datakey.Key}";

                            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                            {
                                ColName = $"@{Col[0]["Field"].ToString()}",
                                ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                ColLeng = (int)Col[0]["MaxLeng"],
                                ColValue = Datakey.Value.ToString()
                            });
                        }
                    }
                }
                WordArea.Fields = Mo.PackUpInsField(WordArea.Fields);
                WordArea.SqlParas = Mo.PackUpInsSqlParas(WordArea.SqlParas,
                    JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)));

                if (WordArea.SqlParas.ValuePara.Count > 0)
                {
                    WordArea.SqlCmd = $"{WordArea.SqlCmd} {WordArea.Fields} where ID=@ID;";
                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = $"@ID",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = WordArea.ID
                    });
                    WordArea.EnityList.ValuePara.Add(WordArea.SqlParas);
                    WordArea.HsTb.Add(new DataHandle.SqlParaResult
                    {
                        SqlCmd = WordArea.SqlCmd,
                        ValuePara = WordArea.EnityList
                    });
                }
                WordArea = Mo.ClearResult(WordArea, false);
                #endregion

                #region  子表
                Exists = Struct["SubList"];
                if (Mo.JtokenFlg(Exists))
                {
                    JObject SubList = (JObject)Struct["SubList"];
                    WordArea.SqlCmd = $" update {SubList["Db"]}.dbo.{SubList["Tb"]} set ";
                    Exists = Data.postdata.SubList;
                    if (Mo.JtokenFlg(Exists))
                    {
                        JArray SubData = Data.postdata.SubList;
                        WordArea.RedisKey = $"{SubList["Db"]}.{SubList["Tb"]}.Config.json";
                        WordArea.Json = Host.CashGetKey(WordArea.RedisKey, "TbStruct");
                        JObject SubCfg = JObject.Parse(WordArea.Json);

                        if (SubData.Count > 0)
                        {
                            #region  修改模式
                            for (int x = 0; x < SubData.Count; x++)
                            {
                                JObject SubObj = (JObject)SubData[x];
                                WordArea.ID = (string)SubObj["ID"];
                                if (WordArea.ID != "0")
                                {
                                    foreach (var Datakey in SubObj)
                                    {
                                        if (Datakey.Key.ToString() != "ID")
                                        {
                                            var YesChgCol =
                                            from p in SubCfg["Fields"].Where
                                            (
                                                p => p["Field"].ToString().ToUpper().
                                                Equals(Datakey.Key.ToString().ToUpper())
                                            )
                                            select p;
                                            JArray Col = (JArray)JsonConvert.
                                                DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                                            if (Col.Count > 0)
                                            {
                                                WordArea.Fields +=
                                                    (string.IsNullOrEmpty(WordArea.Fields))
                                                    ? $" {Datakey.Key}=@{Datakey.Key}"
                                                    : $",{Datakey.Key}=@{Datakey.Key}";
                                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                                {
                                                    ColName = $"@{Col[0]["Field"].ToString()}",
                                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                                    ColLeng = (int)Col[0]["MaxLeng"],
                                                    ColValue = Datakey.Value.ToString()
                                                });
                                            }
                                        }
                                    }
                                    WordArea.Fields = Mo.PackUpInsField(WordArea.Fields);
                                    WordArea.SqlParas = Mo.PackUpInsSqlParas(
                                        WordArea.SqlParas,
                                        JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)));
                                    WordArea.SqlCmd = $"{WordArea.SqlCmd} {WordArea.Fields} " +
                                        $"where ID=@ID " +
                                        $" AND ParentCode=@ParentCode;";
                                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                    {
                                        ColName = $"@ID",
                                        ColType = Mo.ConvertSQLType("bigint"),
                                        ColLeng = 8,
                                        ColValue = WordArea.ID
                                    });
                                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                    {
                                        ColName = $"@ParentCode",
                                        ColType = Mo.ConvertSQLType("nvarchar"),
                                        ColLeng = 50,
                                        ColValue = WordArea.Code
                                    });
                                    WordArea.EnityList.ValuePara.Add(WordArea.SqlParas);
                                    if (string.IsNullOrEmpty(WordArea.Up))
                                    {
                                        WordArea.Up = WordArea.SqlCmd;
                                    }
                                    WordArea.ID = string.Empty;
                                    WordArea.SqlCmd = string.Empty;
                                    WordArea.Fields = string.Empty;
                                    WordArea.SqlParas = new DataHandle.SqlPara();
                                }
                            }

                            WordArea.HsTb.Add(new DataHandle.SqlParaResult
                            {
                                SqlCmd = WordArea.Up,
                                ValuePara = WordArea.EnityList
                            });
                            #endregion

                            WordArea = Mo.ClearResult(WordArea, false);

                            #region  新增模式
                            WordArea.SqlCmd = $" insert into {SubList["Db"]}.dbo.{SubList["Tb"]}  ";
                            for (int x = 0; x < SubData.Count; x++)
                            {
                                JObject SubObj = (JObject)SubData[x];
                                WordArea.ID = (string)SubObj["ID"];
                                if (WordArea.ID == "0")
                                {
                                    foreach (var Datakey in SubObj)
                                    {
                                        if (Datakey.Key.ToString() != "ID")
                                        {
                                            var YesChgCol =
                                            from p in SubCfg["Fields"].Where
                                            (
                                                p => p["Field"].ToString().ToUpper().
                                                Equals(Datakey.Key.ToString().ToUpper())
                                            )
                                            select p;
                                            JArray Col = (JArray)JsonConvert.
                                                DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                                            if (Col.Count > 0)
                                            {
                                                WordArea.Fields +=
                                                    (string.IsNullOrEmpty(WordArea.Fields))
                                                    ? $" {Datakey.Key}"
                                                    : $",{Datakey.Key}";
                                                WordArea.Values += (string.IsNullOrEmpty(WordArea.Values))
                                                    ? $" @{Datakey.Key}"
                                                    : $",@{Datakey.Key}";
                                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                                {
                                                    ColName = $"@{Col[0]["Field"].ToString()}",
                                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                                    ColLeng = (int)Col[0]["MaxLeng"],
                                                    ColValue = Datakey.Value.ToString()
                                                });
                                            }
                                        }
                                    }
                                    WordArea.Fields += ",ParentCode,UnitCode";
                                    WordArea.Values += ",@ParentCode,@UnitCode";
                                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                    {
                                        ColName = $"@ParentCode",
                                        ColType = Mo.ConvertSQLType("nvarchar"),
                                        ColLeng = 50,
                                        ColValue = WordArea.Code
                                    });
                                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                    {
                                        ColName = $"@UnitCode",
                                        ColType = Mo.ConvertSQLType("nvarchar"),
                                        ColLeng = 50,
                                        ColValue = WordArea.UnitCode
                                    });
                                    WordArea.Fields = Mo.PackUserField(WordArea.Fields, true);
                                    WordArea.Values = Mo.PackUserValues(WordArea.Values, true);

                                    WordArea.SqlCmd = $"{WordArea.SqlCmd} " +
                                        $"({WordArea.Fields}) " +
                                        $"values ({WordArea.Values});";
                                    WordArea.EnityList.ValuePara.Add(Mo.PackUserParas(
                                        WordArea.SqlParas,
                                        JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)),
                                        true));
                                    if (string.IsNullOrEmpty(WordArea.Up))
                                    {
                                        WordArea.Up = WordArea.SqlCmd;
                                    }
                                    WordArea.SqlCmd = string.Empty;
                                    WordArea.Fields = string.Empty;
                                    WordArea.Values = string.Empty;
                                    WordArea.SqlParas = new DataHandle.SqlPara();
                                }
                            }

                            WordArea.HsTb.Add(new DataHandle.SqlParaResult
                            {
                                SqlCmd = WordArea.Up,
                                ValuePara = WordArea.EnityList
                            });
                            #endregion
                        }
                    }
                }
                WordArea = Mo.ClearResult(WordArea, false);
                #endregion

                #region 附表
                int Snum = 0;
                Exists = Struct["ScheduleList"];
                if (Mo.JtokenFlg(Exists))
                {
                    JArray ScheduleList = (JArray)Struct["ScheduleList"];
                    for (int x = 0; x < ScheduleList.Count; x++)
                    {
                        WordArea.RedisKey = $"{ScheduleList[x]["Db"]}.{ScheduleList[x]["Tb"]}.Config.json";
                        WordArea.Json = Host.CashGetKey(WordArea.RedisKey, "TbStruct");
                        JObject SubCfg = JObject.Parse(WordArea.Json);
                        WordArea.SqlCmd = $" update {ScheduleList[x]["Db"]}.dbo.{ScheduleList[x]["Tb"]} set ";
                        foreach (var Datakey in DataObj)
                        {
                            if (Datakey.Key.ToString().ToUpper() != "ID"
                                && Datakey.Key.ToString().ToUpper() != "UNITCODE"
                                && Datakey.Key.ToString().ToUpper() != "CODE")
                            {
                                var YesChgCol =
                                from p in SubCfg["Fields"].Where
                                (
                                    p => p["Field"].ToString().ToUpper().
                                    Equals(Datakey.Key.ToString().ToUpper())
                                )
                                select p;
                                JArray Col = (JArray)JsonConvert.
                                    DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                                if (Col.Count > 0)
                                {
                                    Snum++;
                                    WordArea.Fields += (string.IsNullOrEmpty(WordArea.Fields))
                                        ? $" {Datakey.Key}=@{Datakey.Key}"
                                        : $",{Datakey.Key}=@{Datakey.Key}";

                                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                    {
                                        ColName = $"@{Col[0]["Field"].ToString()}",
                                        ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                        ColLeng = (int)Col[0]["MaxLeng"],
                                        ColValue = Datakey.Value.ToString()
                                    });
                                }
                            }
                        }
                        if (Snum > 0)
                        {
                            WordArea.Fields = Mo.PackUpInsField(WordArea.Fields);
                            WordArea.SqlParas = Mo.PackUpInsSqlParas(
                                WordArea.SqlParas,
                                JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)));
                            WordArea.SqlCmd = $"{WordArea.SqlCmd} {WordArea.Fields} " +
                                $"where ParentCode=@ParentCode ";
                            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                            {
                                ColName = $"@ParentCode",
                                ColType = Mo.ConvertSQLType("nvarchar"),
                                ColLeng = 50,
                                ColValue = WordArea.Code
                            });
                            WordArea.EnityList.ValuePara.Add(WordArea.SqlParas);
                            WordArea.HsTb.Add(new DataHandle.SqlParaResult
                            {
                                SqlCmd = WordArea.SqlCmd,
                                ValuePara = WordArea.EnityList
                            });
                        }
                        WordArea = Mo.ClearResult(WordArea, false);
                        Snum = 0;
                    }
                }
                #endregion
                WordArea = Mo.ClearResult(WordArea, false);
                #region  附件
                Exists = Struct["AttachList"];
                if (Mo.JtokenFlg(Exists))
                {
                    WordArea.SqlCmd = " insert into AttachStore.dbo.Tbl_AttachList ";
                    WordArea.Fields = "ParentCode,SourceDb,SourceTb,WebFilePath,AgainName,SourceName,UnitCode";
                    WordArea.Values = "@ParentCode,@SourceDb,@SourceTb,@WebFilePath,@AgainName,@SourceName,@UnitCode";
                    WordArea.SqlCmd = $"{WordArea.SqlCmd} (" +
                        $"{Mo.PackUserField(WordArea.Fields, true)})" +
                        $" values  ({Mo.PackUserValues(WordArea.Values, true)});";
                    JArray AttachArr = Data.postdata.AttachList;
                    for (int x = 0; x < AttachArr.Count; x++)
                    {
                        WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                        {
                            ColName = $"@ParentCode",
                            ColType = Mo.ConvertSQLType("nvarchar"),
                            ColLeng = 100,
                            ColValue = WordArea.Code
                        });
                        JObject AttObj = (JObject)AttachArr[x];
                        foreach (var Datakey in AttObj)
                        {
                            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                            {
                                ColName = $"@{Datakey.Key}",
                                ColType = Mo.ConvertSQLType("nvarchar"),
                                ColLeng = 100,
                                ColValue = Datakey.Value.ToString()
                            });
                        }
                        WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                        {
                            ColName = "@UnitCode",
                            ColType = Mo.ConvertSQLType("nvarchar"),
                            ColLeng = 50,
                            ColValue = WordArea.UnitCode
                        });
                        WordArea.EnityList.ValuePara.Add(
                            Mo.PackUserParas(WordArea.SqlParas,
                            JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)),
                            true));
                        WordArea.SqlParas = new DataHandle.SqlPara();
                    }
                    if (AttachArr.Count > 0)
                    {
                        WordArea.HsTb.Add(new DataHandle.SqlParaResult
                        {
                            SqlCmd = WordArea.SqlCmd,
                            ValuePara = WordArea.EnityList
                        });
                    }
                }
                #endregion

                if (WordArea.HsTb.Count > 0)
                {

                    dal = DalFactory.CreateDal(DataConnType, ThisConn);
                    bool Flg = Task.Run(() => dal.UpOrIns(WordArea.HsTb)).Result;

                    if (Flg)
                    {
                        WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);

                        bool LogFlg = Task.Run(() => MsInsSyslog(
                             WordArea.Code,
                             Struct,
                             Data.postdata.ParentData["NewValue"].ToString(),
                             Data.postdata.ParentData["OldValue"].ToString(),
                             JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)))).Result;
                    }
                    else
                    {
                        WordArea.Result = Mo.GetResult(false, "写入数据不成功", false, new object(), true);
                    }
                }
                else
                {
                    WordArea.Result = Mo.GetResult(false, "没有操作数据", false, new object(), true);
                }
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "参数错误", false, new object(), true);
                LogHelper.Error(JsonConvert.SerializeObject(Data));
            }

            return WordArea.Result;
        }

        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Struct"></param>
        /// <param name="NewJson"></param>
        /// <param name="OldJson"></param>
        /// <param name="UserKey"></param>
        /// <returns></returns>
        public bool MsInsSyslog(string Code, JObject Struct, string NewJson, string OldJson, JObject UserKey)
        {
            bool Result = false;

            string Ins = "insert into SysLog.dbo.Tbl_Log ";

            string Fields = "ParentCode,OldRemarks,NewRemarks,Db,Tb";
            string Values = $"@ParentCode,@OldRemarks,@NewRemarks,@Db,@Tb";
            DataHandle.SqlPara SqlParas = new DataHandle.SqlPara();
            SqlParas.ValuePara.Add(new DataHandle.SQLtype()
            {
                ColName = "@ParentCode",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = Code
            });
            SqlParas.ValuePara.Add(new DataHandle.SQLtype()
            {
                ColName = "@OldRemarks",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 4000,
                ColValue = OldJson
            });
            SqlParas.ValuePara.Add(new DataHandle.SQLtype()
            {
                ColName = "@NewRemarks",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 4000,
                ColValue = NewJson
            });
            SqlParas.ValuePara.Add(new DataHandle.SQLtype()
            {
                ColName = "@Db",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = Struct["Db"].ToString()
            });
            SqlParas.ValuePara.Add(new DataHandle.SQLtype()
            {
                ColName = "@Tb",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = Struct["Tb"].ToString()
            });
            Ins = $"{Ins} ({Mo.PackUserField(Fields, false)}) " +
                $"values  ({Mo.PackUserValues(Values, false)});";
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            int rows = dal.UpOrIns(Ins, Mo.PackUserParas(
                SqlParas, UserKey, false));

            Result = rows > 0 ? true : false;

            return Result;
        }
    }
}
