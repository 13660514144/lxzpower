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
    public class MsIns
    {
        public int Ipage = 20;
        public string DataConnType = "mssql";
        IDal dal;
        public string ThisConn;
        public DataHandle Mo = new DataHandle();
        public RedisHelper Redis = new RedisHelper() { DbType = "TbStruct" };
        public HostReqModel Host = new HostReqModel();
        /// <summary>
        /// 多表参数模式 数据增加方法
        /// </summary>
        /// <param name="Struct">主表结构配置</param>
        /// <param name="Data">客户端上传数据</param>
        /// <returns></returns>
        public string MsInsMany(JObject Struct, RequestPagePara.AllParaResult Data)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            //客户端是否上传分组标记，无则从UserKey取值
            bool UnitFlg = false;
            //标记是否有数据需要写入 针对主表，主表是单行，附表子表是多行需要加标记
            int RsFlgP = 0;
            bool Flg = false;

            #region 主表
            JToken Exists = Data.postdata.ParentData;

            JObject DataObj = Data.postdata.ParentData;
            WordArea.SqlCmd = $" insert into {Struct["Db"]}.dbo.{Struct["Tb"]} ";
            //主表Insert必取CODE
            WordArea.Code = CompressByte.GetCustomCode();
            WordArea.Fields = "Code";
            WordArea.Values = $"@Code";

            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
            {
                ColName = "@Code",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = WordArea.Code
            });
            /*主表Fields参数封装*/
            foreach (var Datakey in DataObj)
            {
                if (Datakey.Key.ToString().ToUpper() == "UNITCODE")
                {
                    UnitFlg = true;
                    WordArea.UnitCode = Datakey.Value.ToString();
                }
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
                    RsFlgP++;
                    WordArea.Fields += $",{Datakey.Key}";
                    WordArea.Values += $",@{Datakey.Key}";
                }
            }
            WordArea.SqlCmd = $"{WordArea.SqlCmd} (" +
                $"{Mo.PackUserField(WordArea.SqlCmd, UnitFlg)}) " +
                $"values  ({Mo.PackUserValues(WordArea.Values, UnitFlg)});";
            /*主表Values参数封装*/
            foreach (var Datakey in DataObj)
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
                    int MaxLeng = (int)Col[0]["MaxLeng"];
                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = $"@{Col[0]["Field"].ToString()}",
                        ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                        ColLeng = MaxLeng,
                        ColValue = Datakey.Value.ToString()
                    });
                }
            }
            WordArea.EnityList.ValuePara.Add(Mo.PackUserParas
                (WordArea.SqlParas,
                JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)),
                UnitFlg));

            if (RsFlgP > 0)//有记录就加入HsTb
            {
                WordArea.HsTb.Add(new DataHandle.SqlParaResult
                {
                    SqlCmd = WordArea.SqlCmd,
                    ValuePara = WordArea.EnityList
                });
            }
            #endregion

            WordArea = Mo.ClearResult(WordArea, false);

            #region 子表
            int RsFlgSub = 0;
            bool SubFlg = false;
            Exists = Struct["SubList"];
            if (Mo.JtokenFlg(Exists))
            {
                JObject SubList = (JObject)Struct["SubList"];

                WordArea.SqlCmd = $" insert into {SubList["Db"]}.dbo.{SubList["Tb"]} ";
                JArray SubData = Data.postdata.SubList;

                string RedisKey = $"{SubList["Db"]}.{SubList["Tb"]}.Config.json";
                string Json = Host.CashGetKey(RedisKey, "TbStruct");
                /*子表结构*/
                JObject SubCfg = JObject.Parse(Json);

                WordArea.Fields = "ParentCode";
                WordArea.Values = "@ParentCode";
                if (SubData.Count > 0)
                {
                    JObject SubObj = (JObject)SubData[0];
                    RsFlgSub++;
                    /*封装子表字段*/
                    foreach (var Datakey in SubObj)
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
                            WordArea.Fields += $",{Datakey.Key}";
                            WordArea.Values += $",@{Datakey.Key}";
                        }
                    }
                    /*封装子表字段*/
                }
                if (UnitFlg)
                {
                    WordArea.Fields += ",UnitCode";
                    WordArea.Values += ",@UnitCode";
                }
                WordArea.SqlCmd = $"{WordArea.SqlCmd} (" +
                    $"{Mo.PackUserField(WordArea.Fields, UnitFlg)}) values " +
                    $" ({Mo.PackUserValues(WordArea.Values, UnitFlg)});";
                RsFlgSub = 0;
                /*子表字段值 参数*/
                for (int x = 0; x < SubData.Count; x++)
                {
                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@ParentCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = WordArea.Code
                    });
                    JObject SubObj = (JObject)SubData[x];
                    foreach (var Datakey in SubObj)
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
                            int MaxLeng = (int)Col[0]["MaxLeng"];
                            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                            {
                                ColName = $"@{Col[0]["Field"].ToString()}",
                                ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                ColLeng = MaxLeng,
                                ColValue = Datakey.Value.ToString()
                            });
                            RsFlgSub++;
                        }
                    }
                    if (UnitFlg)
                    {
                        WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                        {
                            ColName = "@UnitCode",
                            ColType = Mo.ConvertSQLType("nvarchar"),
                            ColLeng = 50,
                            ColValue = WordArea.UnitCode
                        });
                    }
                    WordArea.EnityList.ValuePara.Add(Mo.PackUserParas(
                        WordArea.SqlParas,
                        JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)),
                        UnitFlg));
                    WordArea.SqlParas = new DataHandle.SqlPara();
                }
                /*子表字段值 参数*/
                if (RsFlgSub > 0)//有记录就加入HsTb
                {
                    WordArea.HsTb.Add(new DataHandle.SqlParaResult
                    {
                        SqlCmd = WordArea.SqlCmd,
                        ValuePara = WordArea.EnityList
                    });
                    SubFlg = true;
                }
                RsFlgSub = 0;
            }
            WordArea = Mo.ClearResult(WordArea, false);
            #endregion

            #region 附表
            Exists = Struct["ScheduleList"];
            if (Mo.JtokenFlg(Exists))
            {
                JArray ScheduleList = (JArray)Struct["ScheduleList"];

                int Snum = 0;
                for (int x = 0; x < ScheduleList.Count; x++)
                {
                    WordArea.RedisKey = $"{ScheduleList[x]["Db"]}.{ScheduleList[x]["Tb"]}.Config.json";
                    WordArea.Json = Host.CashGetKey(WordArea.RedisKey, "TbStruct");
                    JObject SubCfg = JObject.Parse(WordArea.Json);

                    WordArea.SqlCmd = $" insert into {ScheduleList[x]["Db"]}.dbo.{ScheduleList[x]["Tb"]} ";
                    WordArea.Fields = "ParentCode";
                    WordArea.Values = "@ParentCode";
                    foreach (var Datakey in DataObj)
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
                            WordArea.Fields += $",{Datakey.Key}";
                            WordArea.Values += $",@{Datakey.Key}";
                        }
                    }
                    if (UnitFlg)
                    {
                        WordArea.Fields += ",UnitCode";
                        WordArea.Values += ",@UnitCode";
                    }
                    WordArea.SqlCmd = $"{WordArea.SqlCmd} (" +
                        $"{Mo.PackUserField(WordArea.Fields, UnitFlg)}) " +
                        $"values  ({Mo.PackUserValues(WordArea.Values, UnitFlg)});";

                    Snum = 0;

                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@ParentCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = WordArea.Code
                    });

                    foreach (var Datakey in DataObj)
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
                            int MaxLeng = (int)Col[0]["MaxLeng"];
                            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                            {
                                ColName = $"@{Col[0]["Field"].ToString()}",
                                ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                ColLeng = MaxLeng,
                                ColValue = Datakey.Value.ToString()
                            });
                        }
                    }
                    if (UnitFlg)
                    {
                        WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                        {
                            ColName = "@UnitCode",
                            ColType = Mo.ConvertSQLType("nvarchar"),
                            ColLeng = 50,
                            ColValue = WordArea.UnitCode
                        });
                    }
                    WordArea.EnityList.ValuePara.Add(Mo.PackUserParas(
                        WordArea.SqlParas,
                        JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)),
                        UnitFlg));
                    if (Snum > 0) //有记录就加入HsTb
                    {
                        WordArea.HsTb.Add(new DataHandle.SqlParaResult
                        {
                            SqlCmd = WordArea.SqlCmd,
                            ValuePara = WordArea.EnityList
                        });

                    }
                    Snum = 0;
                    WordArea = Mo.ClearResult(WordArea, false);
                }
            }
            #endregion
            WordArea = Mo.ClearResult(WordArea, false);

            #region 附件
            Exists = Struct["AttachList"];
            if (Mo.JtokenFlg(Exists))
            {
                WordArea.SqlCmd = " insert into AttachStore.dbo.Tbl_AttachList ";
                WordArea.Fields = "ParentCode,SourceDb,SourceTb,WebFilePath,AgainName,SourceName";
                WordArea.Values = "@ParentCode,@SourceDb,@SourceTb,@WebFilePath,@AgainName,@SourceName";
                if (UnitFlg)
                {
                    WordArea.Fields += ",UnitCode";
                    WordArea.Values += ",@UnitCode";
                }
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
                    DataObj = (JObject)AttachArr[x];
                    foreach (var Datakey in DataObj)
                    {
                        WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                        {
                            ColName = $"@{Datakey.Key}",
                            ColType = Mo.ConvertSQLType("nvarchar"),
                            ColLeng = 100,
                            ColValue = Datakey.Value.ToString()
                        });
                    }
                    if (UnitFlg)
                    {
                        WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                        {
                            ColName = "@UnitCode",
                            ColType = Mo.ConvertSQLType("nvarchar"),
                            ColLeng = 50,
                            ColValue = WordArea.UnitCode
                        });
                    }
                    WordArea.EnityList.ValuePara.Add(Mo.PackUserParas(
                        WordArea.SqlParas,
                        JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)),
                        UnitFlg));
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
            if (WordArea.HsTb.Count > 0 && RsFlgP > 0)
            {
                dal = DalFactory.CreateDal(DataConnType, ThisConn);

                Flg = Task.Run(() => dal.UpOrIns(WordArea.HsTb)).Result;
                if (Flg)
                {
                    WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);
                }
                else
                {
                    WordArea.Result = Mo.GetResult(true, "数据操作发生错误", false, new object(), true);
                }
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "没有需要操作的数据", false, new object(), true);
            }


            return WordArea.Result;
        }

        /// <summary>
        /// 单表模式  参数模式 生成自编码方法，用在通用表业务
        /// </summary>      
        /// <param name="Struct">表结构JSON对象</param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string MsInsSingle(JObject Struct, RequestPagePara.AllParaResult Data)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();

            bool UnitFlg = false;

            WordArea.SqlCmd = $" insert into {Struct["Db"]}.dbo.{Struct["Tb"]} ";
            string Code = CompressByte.GetCustomCode();
            /*新增无Code 返回一个Code值给前端*/
            JObject CodeObj = new JObject(
                    new JProperty("Code", Code)                    
                );
            /*新增无Code 返回一个Code值给前端*/
            string Fields = "Code";
            string Values = $"@Code";
            JObject DataFields = Data.postdata.ParentData;
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
            {
                ColName = Fields,
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = Code
            });
            foreach (var Datakey in DataFields)
            {
                if (Datakey.Key.ToString().ToUpper() == "UNITCODE")
                {
                    UnitFlg = true;
                    WordArea.UnitCode = Datakey.Value.ToString();
                }
                var YesChgCol =
                from p in Struct["Fields"].Where
                (
                    p => p["Field"].ToString().ToUpper().Equals(Datakey.Key.ToString().ToUpper())
                )
                select p;
                JArray Col = (JArray)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                if (Col.Count > 0)
                {
                    Fields += $",{Datakey.Key}";
                    Values += $",@{Datakey.Key}";
                    int MaxLeng = (int)Col[0]["MaxLeng"];

                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = $"@{Col[0]["Field"].ToString()}",
                        ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                        ColLeng = MaxLeng,
                        ColValue = Datakey.Value.ToString()
                    });
                }
            }
            if (WordArea.SqlParas.ValuePara.Count > 1)
            {
                WordArea.SqlCmd = $"{WordArea.SqlCmd} ({Mo.PackUserField(Fields, UnitFlg)}) " +
                    $"values  ({Mo.PackUserValues(Values, UnitFlg)});";
                dal = DalFactory.CreateDal(DataConnType, ThisConn);
                int rows = Task.Run(() => dal.UpOrIns(WordArea.SqlCmd, Mo.PackUserParas(
                    WordArea.SqlParas,
                    JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)), UnitFlg))).Result;
                if (rows > 0)
                {
                    WordArea.Result = Mo.GetResult(true, "执行成功", true, CodeObj, true);
                }
                else
                {
                    WordArea.Result = Mo.GetResult(true, "写入数据不成功", false, new object(), true);
                }
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "没有操作数据", false, new object(), true);
            }


            return WordArea.Result;
        }
        /// <summary>
        /// 单表模式不生成自编码方法，用在常量、菜单、模块配置
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="Data">前端表单参数</param>
        /// <returns></returns>
        
        public string MsInsSingleNoCode(JObject Struct, RequestPagePara.AllParaResult Data)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            bool UnitFlg = false;

            WordArea.SqlCmd = $" insert into {Struct["Db"]}.dbo.{Struct["Tb"]} ";
            string Fields = string.Empty;
            string Values = string.Empty;
            JObject DataFields = Data.postdata.ParentData;

            foreach (var Datakey in DataFields)
            {
                if (Datakey.Key.ToString().ToUpper() == "UNITCODE")
                {
                    UnitFlg = true;
                    WordArea.UnitCode = Datakey.Value.ToString();
                }
                var YesChgCol =
                from p in Struct["Fields"].Where
                (
                    p => p["Field"].ToString().ToUpper().Equals(Datakey.Key.ToString().ToUpper())
                )
                select p;
                JArray Col = (JArray)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                if (Col.Count > 0)
                {
                    Fields += (!string.IsNullOrEmpty(Fields)) ?
                        $",{Datakey.Key}" : $"{Datakey.Key}";
                    Values += (!string.IsNullOrEmpty(Values)) ?
                        $",@{Datakey.Key}" : $"@{Datakey.Key}";
                    int MaxLeng = (int)Col[0]["MaxLeng"];

                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = $"@{Col[0]["Field"].ToString()}",
                        ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                        ColLeng = MaxLeng,
                        ColValue = Datakey.Value.ToString()
                    });
                }
            }
            if (WordArea.SqlParas.ValuePara.Count > 1)
            {
                WordArea.SqlCmd = $"{WordArea.SqlCmd} ({Mo.PackUserField(Fields, UnitFlg)}) " +
                    $"values  ({Mo.PackUserValues(Values, UnitFlg)});";
                dal = DalFactory.CreateDal(DataConnType, ThisConn);
                int rows = Task.Run(() => dal.UpOrIns(WordArea.SqlCmd, Mo.PackUserParas(
                     WordArea.SqlParas,
                     JObject.Parse(JsonConvert.SerializeObject(Data.postdata.userkey)), UnitFlg))).Result;
                if (rows > 0)
                {
                    WordArea.Result = Mo.GetResult(true, "执行成功", false, new object(), true);
                }
                else
                {
                    WordArea.Result = Mo.GetResult(true, "写入数据不成功", false, new object(), true);
                }
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "没有操作数据", false, new object(), true);
            }
            return WordArea.Result;
        }
    }
}
