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
    /// <summary>
    /// 表单数据查询
    /// </summary>
    public class MsFormQuery
    {
        public int Ipage = 20;
        public string DataConnType = "mssql";
        IDal dal;
        public string ThisConn;
        public DataHandle Mo = new DataHandle();
        public RedisHelper Redis = new RedisHelper() { DbType = "TbStruct" };
        /// <summary>
        /// 单表模式（带附表）
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="Json">GetParaEnity 方法返回集合</param>
        /// <returns></returns>
        public string QuerySingle(JObject Struct, RequestPagePara.AllParaResult Json)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();

            int Alias = 1;
            StringBuilder sb = new StringBuilder();
            JToken Exists = Struct["Fields"];
            if (Mo.JtokenFlg(Exists))
            {
                JArray FieldArr = (JArray)Struct["Fields"];
                WordArea.SqlCmd = " select ";
                for (int x = 0; x < FieldArr.Count; x++)
                {
                    WordArea.Fields += string.IsNullOrEmpty(WordArea.Fields)
                        ? $"A.{FieldArr[x]["Field"].ToString()}"
                        : $",A.{FieldArr[x]["Field"].ToString()}";
                    if (FieldArr[x]["IfConst"].ToString() == "1")
                    {
                        WordArea.Fields += $",SysConst.dbo.code2name(" +
                            $"A.{FieldArr[x]["Field"]}) as {FieldArr[x]["Field"]}_X";
                    }
                }
                WordArea.SqlCmd += WordArea.Fields;
                WordArea.Fields = string.Empty;
                sb.Append($" FROM {Struct["Db"]}.dbo.{Struct["Tb"]} A  WITH(NOLOCK) ");
                /*外联表*/
                Alias++;  //表单不需要外联表
                string AliasStr = Mo.GetTbAlias(Alias);               
                JArray OutLink = (JArray)Struct["OutLink"];
                if (OutLink.Count > 0)
                {
                    for (int x = 0; x < OutLink.Count; x++)
                    {
                        sb.Append($" left join {OutLink[x]["OutLine"]} {AliasStr}  WITH(NOLOCK) " +
                            $"on {AliasStr}.{OutLink[x]["onfield"]}=A.{OutLink[x]["owner"]}  ");
                        JArray OutFields = (JArray)OutLink[x]["Fileds"];
                        for (int z = 0; z < OutFields.Count; z++)
                        {
                            WordArea.Fields += $",{AliasStr}.{OutFields[z]["Col"]} as" +
                                $" {OutFields[z]["Col"]}_{OutLink[x]["owner"]} ";                 
                        }
                        Alias++;
                        AliasStr = Mo.GetTbAlias(Alias);
                    }
                }
                Alias++;
                WordArea.SqlCmd += WordArea.Fields;
                WordArea.Fields = string.Empty;
                /*附表*/
                Exists = Struct["ScheduleList"];
                if (Mo.JtokenFlg(Exists))
                {
                    FieldArr = (JArray)Struct["ScheduleList"];
                    for (int x = 0; x < FieldArr.Count; x++)
                    {
                        WordArea.RedisKey = $"{FieldArr[x]["Db"]}.{FieldArr[x]["Tb"]}.Config.json";
                        WordArea.Json = Redis.Default.StringGet(WordArea.RedisKey);
                        JObject SubCfg = JObject.Parse(WordArea.Json);
                        JArray SubArr = (JArray)SubCfg["Fields"];
                        int LFields = SubArr.Count - 6;//ID,COD及6个系统字段不需要
                        string T = Mo.GetTbAlias(Alias);
                        for (int y = 2; y < LFields; y++)
                        {
                            WordArea.Fields += $",{T}.{SubArr[y]["Field"]}";
                        }
                        WordArea.SqlCmd += WordArea.Fields;
                        WordArea.Fields = string.Empty;
                        sb.Append($" left JOIN " +
                            $"{FieldArr[x]["Db"]}.dbo.{FieldArr[x]["Tb"]} {T}  WITH(NOLOCK) " +
                            $" ON {T}.ParentCode=A.Code  ");
                        Alias++;
                    }
                }
                sb.Append(" where ");

                WordArea.SqlCmd += sb.ToString();
                string where = string.Empty;
                foreach (var item in Json.postdata.ParentData)
                {
                    var YesChgCol =
                     from p in Struct["Fields"].Where
                     (
                         p => p["Field"].ToString().ToUpper().
                         Equals(item.Key.ToString().ToUpper())
                     )
                     select p;
                    JArray Col = (JArray)JsonConvert.
                        DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                    if (Col.Count > 0)
                    {
                        where += string.IsNullOrEmpty(where)
                            ? $" A.{item.Key}=@{item.Key} "
                            : $" and A.{item.Key}=@{item.Key} ";
                        WordArea.SqlParas.ValuePara.Add(
                            new DataHandle.SQLtype
                            {
                                ColName = $"@{Col[0]["Field"].ToString()}",
                                ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                ColLeng = (int)Col[0]["MaxLeng"],
                                ColValue = item.Value.ToString()
                            });
                    }
                }
                WordArea.SqlCmd += where;
                dal = DalFactory.CreateDal(DataConnType, ThisConn);
                DataTable Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
                dynamic obj = new DynamicObj();
                obj.Data = Dt;
                if (Dt.Rows.Count > 0)
                {
                    WordArea.Result = Mo.GetResult(true, "查询成功", true, obj._values, true);
                }
                else
                {
                    WordArea.Result = Mo.GetResult(true, "此范围没有数据", false, Dt, true);
                }
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "参数错误", false, new object(), true);
            }
            return WordArea.Result;
        }
        /// <summary>
        /// 多表模式（带附表、子表）
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="Json">GetParaEnity 方法返回集合</param>
        /// <returns></returns>
        public string QueryMany(JObject Struct, RequestPagePara.AllParaResult Json)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            int Alias = 1;
            StringBuilder sb = new StringBuilder();
            JToken Exists = Struct["Fields"];
            if (Mo.JtokenFlg(Exists))
            {
                JArray FieldArr = (JArray)Struct["Fields"];
                WordArea.SqlCmd = " select ";
                for (int x = 0; x < FieldArr.Count; x++)
                {
                    WordArea.Fields += string.IsNullOrEmpty(WordArea.Fields)
                        ? $"A.{FieldArr[x]["Field"].ToString()}"
                        : $",A.{FieldArr[x]["Field"].ToString()}";
                    if (FieldArr[x]["IfConst"].ToString() == "1")
                    {
                        WordArea.Fields += $",SysConst.dbo.code2name(" +
                            $"A.{FieldArr[x]["Field"]}) as {FieldArr[x]["Field"]}_X";
                    }
                }
                WordArea.SqlCmd += WordArea.Fields;
                WordArea.Fields = string.Empty;
                sb.Append($" FROM {Struct["Db"]}.dbo.{Struct["Tb"]} A WITH(NOLOCK) ");
                /*外联表*/
                Alias++;
                string AliasStr = Mo.GetTbAlias(Alias);
                JArray OutLink = (JArray)Struct["OutLink"];
                if (OutLink.Count > 0)
                {
                    for (int x = 0; x < OutLink.Count; x++)
                    {
                        sb.Append($" left join {OutLink[x]["OutLine"]} {AliasStr} WITH(NOLOCK)  " +
                            $"on {AliasStr}.{OutLink[x]["onfield"]}=A.{OutLink[x]["owner"]}  ");
                        JArray OutFields = (JArray)OutLink[x]["Fileds"];
                        for (int z = 0; z < OutFields.Count; z++)
                        {
                            WordArea.Fields += $",{AliasStr}.{OutFields[z]["Col"]} as" +
                                $" {OutFields[z]["Col"]}_{OutLink[x]["owner"]} ";
                        }
                        Alias++;
                        AliasStr = Mo.GetTbAlias(Alias);
                    }
                }
                WordArea.SqlCmd += WordArea.Fields;
                WordArea.Fields = string.Empty;
                /*附表*/
                Alias++;
                Exists = Struct["ScheduleList"];
                if (Mo.JtokenFlg(Exists))
                {
                    FieldArr = (JArray)Struct["ScheduleList"];
                    for (int x = 0; x < FieldArr.Count; x++)
                    {
                        WordArea.RedisKey = $"{FieldArr[x]["Db"]}.{FieldArr[x]["Tb"]}.Config.json";
                        WordArea.Json = Redis.Default.StringGet(WordArea.RedisKey);
                        JObject SubCfg = JObject.Parse(WordArea.Json);
                        JArray SubArr = (JArray)SubCfg["Fields"];
                        int LFields = SubArr.Count - 6;
                        string T = Mo.GetTbAlias(Alias);
                        for (int y = 2; y < LFields; y++)
                        {
                            WordArea.Fields += $",{T}.{SubArr[y]["Field"]}";
                        }
                        WordArea.SqlCmd += WordArea.Fields;
                        WordArea.Fields = string.Empty;
                        sb.Append($" left JOIN " +
                            $"{FieldArr[x]["Db"]}.dbo.{FieldArr[x]["Tb"]} {T} WITH(NOLOCK) " +
                            $" ON {T}.ParentCode=A.Code  ");
                        Alias++;
                    }
                }
                sb.Append(" where ");

                WordArea.SqlCmd += sb.ToString();
                string where = string.Empty;
                foreach (var item in Json.postdata.ParentData)
                {
                    var YesChgCol =
                     from p in Struct["Fields"].Where
                     (
                         p => p["Field"].ToString().ToUpper().
                         Equals(item.Key.ToString().ToUpper())
                     )
                     select p;
                    JArray Col = (JArray)JsonConvert.
                        DeserializeObject(JsonConvert.SerializeObject(YesChgCol));
                    if (Col.Count > 0)
                    {
                        where += string.IsNullOrEmpty(where)
                            ? $" A.{item.Key}=@{item.Key} "
                            : $" and A.{item.Key}=@{item.Key} ";
                        WordArea.SqlParas.ValuePara.Add(
                            new DataHandle.SQLtype
                            {
                                ColName = $"@{Col[0]["Field"].ToString()}",
                                ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                ColLeng = (int)Col[0]["MaxLeng"],
                                ColValue = item.Value.ToString()
                            });
                    }
                }
                WordArea.SqlCmd += where;
                dal = DalFactory.CreateDal(DataConnType, ThisConn);
                DataTable Dt = Task.Run(() =>
                     dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
                WordArea = Mo.ClearResult(WordArea, false);
                /*子表*/
                WordArea.RedisKey = $"{Struct["SubList"]["Db"]}." +
                    $"{Struct["SubList"]["Tb"]}.Config.json";
                WordArea.Json = Redis.Default.StringGet(WordArea.RedisKey);
                JObject SubObj = JObject.Parse(WordArea.Json);

                MsListQuery SubGet = new MsListQuery();
                SubGet.ThisConn = ThisConn;
                DataHandle.ListObject dtsub = SubGet.GetSubList(
                    SubObj, "{'Code':'" + Dt.Rows[0]["Code"].ToString().Trim() + "'}");
                /*子表*/
                dynamic obj = new DynamicObj();
                obj.Data = Dt;
                obj.SubData = dtsub.ListDt;
                obj.ListConFig = dtsub.ListCfg;

                if (Dt.Rows.Count > 0)
                {
                    WordArea.Result = Mo.GetResult(true, "查询成功", true, obj._values, true);
                }
                else
                {
                    WordArea.Result = Mo.GetResult(true, "此范围没有数据", false, Dt, true);
                }
            }
            else
            {
                WordArea.Result = Mo.GetResult(false, "参数错误", false, new object(), true);
            }
            return WordArea.Result;
        }
    }
}
