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
    public class MsListQuery
    {
        public int Ipage = 20;
        public string DataConnType = "mssql";
        IDal dal;
        public string ThisConn;
        public DataHandle Mo = new DataHandle();
        public RedisHelper Redis = new RedisHelper() { DbType= "TbStruct" };
        /// <summary>
        /// 公共列表  参数查询
        /// </summary>     
        /// <param name="Struct"></param>
        /// <param name="SearchScheme"></param>
        /// <returns></returns>
        public String GuestListFields(JObject Struct, RequestPagePara.AllParaResult SearchScheme)
        {
            dynamic Obj = new DynamicObj();
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            DataHandle.ListObject listcfg = new DataHandle.ListObject();
            DataHandle.Paging PageResult = new DataHandle.Paging();
            WordArea.Result = string.Empty;
            WordArea.SqlCmd = string.Empty;
            WordArea.Fields = string.Empty;

            string Where = string.Empty;
            string order = "order by A.ID desc";
            string Dbo = string.Empty;
            string SubListConFig = string.Empty;//子表列集合
            string Hidden = String.Empty;

            string VagueFlg = string.Empty;// 模糊查询标记
            string ClassFlg = string.Empty;// 下级查询标记 此二标记以Y/N表示有效

            StringBuilder sb = new StringBuilder();
            string TFields = string.Empty;
            string TListFields = string.Empty;
            JToken Exists;

            #region 获取子表结构，如果有
            Exists = Struct["SubList"];
            string Sub=string.Empty;
            JObject SubStruct = new JObject();
            if (Mo.JtokenFlg(Exists))
            {
                Sub = $"{Struct["SubList"]["Db"]}.{Struct["SubList"]["Tb"]}.Config.json";
                SubStruct = JObject.Parse(Redis.Default.GetStringKey(Sub));
                
                JArray SubList = (JArray)SubStruct["Fields"];
                SubListConFig = "ID:0:ID";
                for (int y = 1; y < SubList.Count - 6; y++)
                {
                    /*只输出*/
                    if (SubList[y]["IfConst"].ToString() == "1")
                    {
                        SubListConFig += $",{SubList[y]["Field"]}:" +
                            $"1:{SubList[y]["FieldCn"]}";
                        /*常量转换字段*/
                        SubListConFig += $",{SubList[y]["Field"]}_X:" +
                            $"0:{SubList[y]["FieldCn"]}";
                    }
                    else
                    {
                        string Hiddens = SubList[y]["IfOutHidden"].ToString();
                        SubListConFig += $",{SubList[y]["Field"]}:" +
                            $"{Hiddens}:{SubList[y]["FieldCn"]}";
                    }
                }
            }
            #endregion

            Dbo = $"{Struct["Db"]}.dbo.{Struct["Tb"]}";
            string PagePath = $"/Page/{Struct["Db"]}/{Struct["Tb"]}/";//页面目录                      
            JObject DataObj = SearchScheme.postdata.ParentData;
            int Alias = 1;

            /*封装查询 表查询字段字段封装*/
            if (Mo.JtokenFlg(DataObj))
            {
                foreach (var item in DataObj)
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
                        switch (L)
                        {
                            case "D":
                                Where += $" AND A.{item.Key}>=@{item.Key} AND" +
                                    $" A.{item.Key}<=@{item.Key}_X ";
                                string[] Tdate = item.Value.ToString().Replace(" - ", ",").Split(',');
                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                {
                                    ColName = $"@{item.Key}",
                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                    ColLeng = (int)Col[0]["MaxLeng"],
                                    ColValue = $"{Tdate[0]} 00:00:00.000"
                                });
                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                {
                                    ColName = $"@{item.Key}_X",
                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                    ColLeng = (int)Col[0]["MaxLeng"],
                                    ColValue = $"{Tdate[1]} 23:59:59.999"
                                });
                                break;
                            case "I":
                            case "F":
                                Where += $" AND A.{item.Key}=@{item.Key} ";
                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                {
                                    ColName = $"@{item.Key}",
                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                    ColLeng = (int)Col[0]["MaxLeng"],
                                    ColValue = item.Value.ToString()
                                });
                                break;
                            case "N":
                                Exists = (DataObj["VagueFlg"]);
                                if (!Mo.JtokenFlg(Exists))
                                {
                                    Where += $" AND A.{item.Key}=@{item.Key} ";
                                }
                                else
                                {
                                    Where += $" AND A.{item.Key} LIKE '%'+@{item.Key}+'%' ";
                                }
                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                {
                                    ColName = $"@{item.Key}",
                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                    ColLeng = (int)Col[0]["MaxLeng"],
                                    ColValue = item.Value.ToString()
                                });
                                break;
                        }

                    }
                }
            }
            /*封装查询 表字段封装*/

            /*分页封装*/

            PageResult = Mo.InitPageStart(SearchScheme.postdata.PagePara);

            /*分页封装*/
            Exists = DataObj["ClassFlg"];
            JToken ExistUnitcode= DataObj["UnitCode"];
            if (SearchScheme.postdata.userkey.Role != "000000")
            {
                if (!Mo.JtokenFlg(ExistUnitcode))//前端没有提交UnitCode需要加此条件
                {
                    if (Mo.JtokenFlg(Exists))//是否查询下级
                    {
                        Where = $" A.UnitCode like ''+@UnitCode+'%' " +
                            $" AND A.DELFLG=1 " +
                            $" {Where} ";
                    }
                    else
                    {
                        Where = $" A.UnitCode =@UnitCode " +
                            $" AND A.DELFLG=1 " +
                            $" {Where} ";
                    }
                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@UnitCode",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = SearchScheme.postdata.userkey.UnitCode.ToString()
                    });
                }
                else
                {
                    Where = $" A.DELFLG=1 {Where} ";
                }
            }
            else
            {
                Where = $" A.DELFLG=1 {Where} ";
            }

            dal = DalFactory.CreateDal(DataConnType, ThisConn);

            if (PageResult.Rnum == 0)
            {
                WordArea.SqlCmd = $"select count(*)num from {Dbo} A  WITH(NOLOCK)  where {Where}  ;";
                //WordArea.SqlCmd = $"USE {Struct["Db"]} ; " +
                //    $"SELECT num FROM SYSINDEXES WHERE ID = OBJECT_ID('{Struct["Tb"]}') AND INDID = 1 ; ";
                PageResult.Rnum = Task.Run(() =>
                     dal.ExeScalar(WordArea.SqlCmd, WordArea.SqlParas)).Result;//总记录

                WordArea.SqlCmd = $"select top 1 ID from {Dbo} A  WITH(NOLOCK)  where {Where} {order}  ;";
                PageResult.LastID = Task.Run(() =>
                     dal.ExeScalar(WordArea.SqlCmd, WordArea.SqlParas)).Result;//最大记录ID

                PageResult.Rpage = PageResult.Rnum % Ipage > 0
                    ? PageResult.Rnum / Ipage + 1
                    : PageResult.Rnum / Ipage;//总页数

                PageResult.Spage = 0;
                PageResult.NextPage = PageResult.CurrentPage + 1 < PageResult.Rpage
                    ? PageResult.CurrentPage + 1
                    : (PageResult.Rpage - 1);
                PageResult.PrePage = PageResult.CurrentPage - 1 > 0 ?
                    PageResult.CurrentPage - 1 : 0;
                PageResult.EndPage = PageResult.Rpage - 1;

            }

            /*处理分页记录*/
            switch (PageResult.PagingMode)
            {
                case "S":
                    Where = $" A.ID<=@LastID AND {Where} ";
                    order = " ORDER BY A.ID DESC ";

                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@LastID",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = PageResult.LastID.ToString()
                    });
                    PageResult.CurrentPage = 1;
                    break;
                case "N":
                    Where = $" A.ID<@MinNum AND {Where} ";
                    order = " ORDER BY A.ID DESC ";

                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@MinNum",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = PageResult.MinNum.ToString()
                    });
                    PageResult.CurrentPage += 1;
                    break;
                case "P":
                    Where = $" A.ID>@MaxNum AND {Where} ";
                    order = " ORDER BY A.ID  ";

                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@MaxNum",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = PageResult.MaxNum.ToString()
                    });
                    PageResult.CurrentPage -= 1;
                    break;
                case "E":
                    Where = $" A.ID<@MinNum AND {Where} ";
                    order = " ORDER BY A.ID  ";
                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@MinNum",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = PageResult.MinNum.ToString()
                    });
                    PageResult.CurrentPage = PageResult.Rpage;
                    break;
                case "C"://当前页
                    Where = $" A.ID>=@MinNum AND A.ID<=@MaxNum AND {Where} ";
                    order = " ORDER BY A.ID  ";
                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@MinNum",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = PageResult.MinNum.ToString()
                    });
                    WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                    {
                        ColName = "@MaxNum",
                        ColType = Mo.ConvertSQLType("bigint"),
                        ColLeng = 8,
                        ColValue = PageResult.MaxNum.ToString()
                    });
                    //PageResult.CurrentPage = PageResult.Rpage;
                    break;
            }

            WordArea.SqlCmd = string.Empty;
            /*处理分页记录*/
            WordArea.ListFields = "ID:0:ID:1:I:0:0:0:0";
            WordArea.Fields = "A.ID";
            /*计算输出*/
            JArray list = (JArray)Struct["Fields"];
            for (int x = 1; x < list.Count; x++)
            {
                JObject O = (JObject)list[x];
                if (O["IfConst"].ToString() == "1")
                {
                    WordArea.Fields += $",A.{O["Field"]}";
                    WordArea.ListFields += $"," +
                        $"{O["Field"]}:1:{O["FieldCn"]}:{O["IfSearch"]}:" +
                        $"{Mo.ConvertDataType(O["DataType"].ToString())}:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";

                    WordArea.Fields += $",SysConst.dbo.code2name(A.{O["Field"]}) " +
                        $" AS {O["Field"]}_X";
                    WordArea.ListFields += $",{O["Field"]}_X:0:{O["FieldCn"]}:0:N:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";
                }
                else
                {
                    WordArea.Fields += $",A.{O["Field"]}";
                    Hidden = O["IfOutHidden"].ToString();
                    WordArea.ListFields += $"," +
                        $"{O["Field"]}:{Hidden}:{O["FieldCn"]}:{O["IfSearch"]}:" +
                        $"{Mo.ConvertDataType(O["DataType"].ToString())}:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";
                }
            }

            Alias++;
            string AliasStr = Mo.GetTbAlias(Alias);

            /*外联表*/

            JArray OutLink = (JArray)Struct["OutLink"];
            if (OutLink.Count > 0)
            {
                for (int x = 0; x < OutLink.Count; x++)
                {
                    sb.Append($" left join {OutLink[x]["OutLine"]} {AliasStr} WITH(NOLOCK)  " +
                        $"on {AliasStr}.{OutLink[x]["onfield"]}=A.{OutLink[x]["owner"]}  ");   
                    JArray OutFields = (JArray)OutLink[x]["Fileds"];
                    for (int z=0;z<OutFields.Count;z++)
                    {
                        TFields += $",{AliasStr}.{OutFields[z]["Col"]} as" +
                            $" {OutFields[z]["Col"]}_{OutLink[x]["owner"]} ";
                        TListFields += $",{OutFields[z]["Col"]}_{OutLink[x]["owner"]}" +
                            $":0:{OutFields[z]["ColName"]}:0:N:0:0:1:" +
                            $"{OutFields[z]["ComboRestlt"]}";
                    }
                    Alias++;
                    AliasStr = Mo.GetTbAlias(Alias);
                }
            }

            WordArea.SqlCmd = $" select top {Ipage} " +
                $" {WordArea.Fields} " +
                $" into #P1 " +
                $" from {Dbo} A  WITH(NOLOCK)  where {Where} {order} ; ";

            WordArea.SqlCmd += $" select {WordArea.Fields}{TFields}" +
                $" from #P1 A {sb.ToString()} " +
                $" order by A.ID DESC ; " +
                $" drop table #P1; ";

            DataTable dt = dal.ListData(WordArea.SqlCmd, WordArea.SqlParas);


            /*Button 集合  角色权限的匹配*/
            string BtnParaResult = string.Empty;
            Redis.DbType = "MenuCode";
            BtnParaResult = Redis.Default.StringGet(SearchScheme.servercode);

            JArray RedisBtn = JArray.Parse(BtnParaResult);
            string Role = SearchScheme.postdata.userkey.Role;

            WordArea.SqlCmd = $@"
                SELECT 
                       [Code]                      
                      ,[ModelCode]                      
                  FROM [SysConfig].[dbo].[Tbl_RoleRights]  WITH(NOLOCK) 
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
                    ColValue = SearchScheme.postdata.userkey.SysCode
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@InterfaceCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = SearchScheme.servercode
                });
            DataTable Btn = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            JArray BtnJarray = new JArray();

            if (Role != "000000")
            {
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
            }
            else
            {
                BtnJarray = RedisBtn;
            }
            /*Button 集合*/
            if (dt.Rows.Count > 0)
            {
                int len = dt.Rows.Count - 1;
                PageResult.MinNum = (long)dt.Rows[len]["ID"];
                PageResult.MaxNum = (long)dt.Rows[0]["ID"];
                Obj.Data = dt;
                Obj.PagePara = PageResult;
                Obj.ListConFig = $"{WordArea.ListFields}{TListFields}";
                Obj.PagePath = PagePath;
                Obj.SubListConFig = SubListConFig;
                Obj.BtnParaResult = BtnJarray;// JArray.Parse(BtnParaResult);
                WordArea.Result = Mo.GetResult(true, "执行成功", true, Obj._values, true);
            }
            else
            {
                PageResult.MinNum = 0;
                PageResult.MaxNum = 0;
                Obj.Data = dt;
                Obj.PagePara = PageResult;
                Obj.ListConFig = $"{WordArea.ListFields}{TListFields}";
                Obj.PagePath = PagePath;
                Obj.SubListConFig = SubListConFig;
                Obj.BtnParaResult = BtnJarray;
                WordArea.Result = Mo.GetResult(true, "没有符合条件数据", true, Obj._values, true);
            }

            return WordArea.Result;
        }
        /// <summary>
        /// 非通用列表 用于主表子表的关联增加
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="SearchScheme"></param>
        /// <returns></returns>
        public String GuestListFieldsNoTop(JObject Struct, RequestPagePara.AllParaResult SearchScheme)
        {
            dynamic Obj = new DynamicObj();
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            DataHandle.ListObject listcfg = new DataHandle.ListObject();
            DataHandle.Paging PageResult = new DataHandle.Paging();
            WordArea.Result = string.Empty;
            WordArea.SqlCmd = string.Empty;
            WordArea.Fields = string.Empty;

            string Where = string.Empty;
            string order = "order by A.ID desc";
            string Dbo = string.Empty;
            string Hidden = String.Empty;
            
            string ClassFlg = string.Empty;// 下级查询标记 此二标记以Y/N表示有效

            StringBuilder sb = new StringBuilder();
            string TFields = string.Empty;
            string TListFields = string.Empty;
            JToken Exists;

            Dbo = $"{Struct["Db"]}.dbo.{Struct["Tb"]}";
            string PagePath = $"/Page/{Struct["Db"]}/{Struct["Tb"]}/";//页面目录                      
            JObject DataObj = SearchScheme.postdata.ParentData;
            int Alias = 1;

            /*封装查询 表查询字段字段封装*/
            if (Mo.JtokenFlg(DataObj))
            {
                foreach (var item in DataObj)
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
                        switch (L)
                        {
                            case "D":
                                Where += $" AND A.{item.Key}>=@{item.Key} AND" +
                                    $" A.{item.Key}<=@{item.Key}_X ";
                                string[] Tdate = item.Value.ToString().Replace(" - ", ",").Split(',');
                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                {
                                    ColName = $"@{item.Key}",
                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                    ColLeng = (int)Col[0]["MaxLeng"],
                                    ColValue = $"{Tdate[0]} 00:00:00.000"
                                });
                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                {
                                    ColName = $"@{item.Key}_X",
                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                    ColLeng = (int)Col[0]["MaxLeng"],
                                    ColValue = $"{Tdate[1]} 23:59:59.999"
                                });
                                break;
                            case "I":
                            case "F":
                                Where += $" AND A.{item.Key}=@{item.Key} ";
                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                {
                                    ColName = $"@{item.Key}",
                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                    ColLeng = (int)Col[0]["MaxLeng"],
                                    ColValue = item.Value.ToString()
                                });
                                break;
                            case "N":
                                Exists = (DataObj["VagueFlg"]);
                                Where += $" AND A.{item.Key}=@{item.Key} ";
                                WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
                                {
                                    ColName = $"@{item.Key}",
                                    ColType = Mo.ConvertSQLType(Col[0]["DataType"].ToString()),
                                    ColLeng = (int)Col[0]["MaxLeng"],
                                    ColValue = item.Value.ToString()
                                });
                                break;
                        }

                    }
                }
            }
            /*封装查询 表字段封装*/

            Exists = DataObj["ClassFlg"];
            JToken ExistUnitcode = DataObj["UnitCode"];
            Where = $" A.UnitCode =@UnitCode " +
                $" AND A.DELFLG=1 " +
                $" {Where} ";
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype()
            {
                ColName = "@UnitCode",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = SearchScheme.postdata.userkey.UnitCode.ToString()
            });

            dal = DalFactory.CreateDal(DataConnType, ThisConn);


            WordArea.SqlCmd = string.Empty;
            /*处理分页记录*/
            WordArea.ListFields = "ID:0:ID:1:I:0:0:0:0";
            WordArea.Fields = "A.ID";
            /*计算输出*/
            JArray list = (JArray)Struct["Fields"];
            for (int x = 1; x < list.Count; x++)
            {
                JObject O = (JObject)list[x];
                if (O["IfConst"].ToString() == "1")
                {
                    WordArea.Fields += $",A.{O["Field"]}";
                    WordArea.ListFields += $"," +
                        $"{O["Field"]}:1:{O["FieldCn"]}:{O["IfSearch"]}:" +
                        $"{Mo.ConvertDataType(O["DataType"].ToString())}:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";

                    WordArea.Fields += $",SysConst.dbo.code2name(A.{O["Field"]}) " +
                        $" AS {O["Field"]}_X";
                    WordArea.ListFields += $",{O["Field"]}_X:0:{O["FieldCn"]}:0:N:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";
                }
                else
                {
                    WordArea.Fields += $",A.{O["Field"]}";
                    Hidden = O["IfOutHidden"].ToString();
                    WordArea.ListFields += $"," +
                        $"{O["Field"]}:{Hidden}:{O["FieldCn"]}:{O["IfSearch"]}:" +
                        $"{Mo.ConvertDataType(O["DataType"].ToString())}:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";
                }
            }

            Alias++;
            string AliasStr = Mo.GetTbAlias(Alias);

            /*外联表*/

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
                        TFields += $",{AliasStr}.{OutFields[z]["Col"]} as" +
                            $" {OutFields[z]["Col"]}_{OutLink[x]["owner"]} ";
                        TListFields += $",{OutFields[z]["Col"]}_{OutLink[x]["owner"]}" +
                            $":0:{OutFields[z]["ColName"]}:0:N:0:0:1:" +
                            $"{OutFields[z]["ComboRestlt"]}";
                    }
                    Alias++;
                    AliasStr = Mo.GetTbAlias(Alias);
                }
            }

            WordArea.SqlCmd = $" select  " +
                $" {WordArea.Fields} " +
                $" into #P1 " +
                $" from {Dbo} A  WITH(NOLOCK)  where {Where} {order} ; ";

            WordArea.SqlCmd += $" select {WordArea.Fields}{TFields}" +
                $" from #P1 A {sb.ToString()} " +
                $" order by A.ID DESC ; " +
                $" drop table #P1; ";

            DataTable dt = dal.ListData(WordArea.SqlCmd, WordArea.SqlParas);

            if (dt.Rows.Count > 0)
            {
                Obj.Data = dt;
                Obj.ListConFig = $"{WordArea.ListFields}{TListFields}";                             
                WordArea.Result = Mo.GetResult(true, "执行成功", true, Obj._values, true);
            }
            else
            {
                Obj.Data = dt;
                Obj.ListConFig = $"{WordArea.ListFields}{TListFields}";
                WordArea.Result = Mo.GetResult(true, "没有符合条件数据", true, Obj._values, true);
            }
            return WordArea.Result;
        }

        /// <summary>
        /// 主子表关联子表查询
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="JsonData"></param>
        /// <returns></returns>
        public DataHandle.ListObject GetSubList(JObject Struct, string JsonData)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            DataTable dt = new DataTable();
            DataHandle.ListObject listcfg = new DataHandle.ListObject();

            /*子表*/
            JObject WherePara = JObject.Parse(JsonData);
            WordArea.SqlCmd = $" select ";
            WordArea.Fields = "ID";
            WordArea.ListFields = "ID:0:ID";
            JArray SubList = (JArray)Struct["Fields"];
            for (int y = 1; y < SubList.Count - 6; y++)
            {
                /*只输出*/
                if (SubList[y]["IfConst"].ToString() == "1")
                {
                    WordArea.Fields += $",{SubList[y]["Field"]}";
                    WordArea.ListFields += $",{SubList[y]["Field"]}:" +
                        $"1:{SubList[y]["FieldCn"]}";
                    /*常量转换字段*/
                    WordArea.Fields += $",SysConst.dbo.code2name(" +
                        $"{SubList[y]["Field"]}) as {SubList[y]["Field"]}_X";
                    WordArea.ListFields += $",{SubList[y]["Field"]}_X:" +
                        $"0:{SubList[y]["FieldCn"]}";
                }
                else
                {
                    string Hidden = SubList[y]["IfOutHidden"].ToString();
                    WordArea.Fields += $",{SubList[y]["Field"]}";
                    WordArea.ListFields += $",{SubList[y]["Field"]}:" +
                        $"{Hidden}:{SubList[y]["FieldCn"]}";
                }
            }
            WordArea.SqlCmd += WordArea.Fields;
            WordArea.SqlCmd += $" from {Struct["Db"]}.dbo." +
                $"{Struct["Tb"]}  WITH(NOLOCK)  ";
            WordArea.SqlCmd += $" where ParentCode=@ParentCode and delflg=1 ;";
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype
            {
                ColName = $"@ParentCode",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = WherePara["Code"].ToString()
            });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            listcfg.ListDt = dt;
            listcfg.ListCfg = WordArea.ListFields;

            return listcfg;
        }
        /// <summary>
        /// 关联主表获取操作日志
        /// </summary>
        /// <param name="JsonData">Json包含库名、表名、主表Code</param>
        /// <returns></returns>
        public DataHandle.ListObject GetLogList(string JsonData)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            DataTable dt = new DataTable();
            DataHandle.ListObject listcfg = new DataHandle.ListObject();

            WordArea.RedisKey = $"SysLog.Tbl_Log.Config.json";
            WordArea.Json = Redis.Default.StringGet(WordArea.RedisKey);
            JObject Struct = JObject.Parse(WordArea.Json);
            JObject WherePara = JObject.Parse(JsonData);

            WordArea.SqlCmd = $" select ";
            WordArea.Fields = "ID";
            WordArea.ListFields = "ID:0:ID";
            JArray SubList = (JArray)Struct["Fields"];
            for (int y = 1; y < SubList.Count - 5; y++)
            {
                /*只输出*/
                if (SubList[y]["IfConst"].ToString() == "1")
                {
                    WordArea.Fields += $",{SubList[y]["Field"]}";
                    WordArea.ListFields += $",{SubList[y]["Field"]}:" +
                        $"1:{SubList[y]["FieldCn"]}";
                    /*常量转换字段*/
                    WordArea.Fields += $",SysConst.dbo.code2name(" +
                        $"{SubList[y]["Field"]}) as {SubList[y]["Field"]}_X";
                    WordArea.ListFields += $",{SubList[y]["Field"]}_X:" +
                        $"0:{SubList[y]["FieldCn"]}";
                }
                else
                {
                    string Hidden = SubList[y]["IfOutHidden"].ToString();
                    WordArea.Fields += $",{SubList[y]["Field"]}";
                    WordArea.ListFields += $",{SubList[y]["Field"]}:" +
                        $"{Hidden}:{SubList[y]["FieldCn"]}";
                }
            }

            WordArea.SqlCmd += WordArea.Fields;
            WordArea.SqlCmd += " from SysLog.dbo.Tbl_Log  WITH(NOLOCK) ";
            WordArea.SqlCmd += $" where ParentCode=@ParentCode and Db=@Db and Tb=@Tb;";
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype
            {
                ColName = $"@ParentCode",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = WherePara["Code"].ToString()
            });
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype
            {
                ColName = $"@Db",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = WherePara["Db"].ToString()
            });
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype
            {
                ColName = $"@Tb",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = WherePara["Tb"].ToString()
            });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            listcfg.ListDt = dt;
            listcfg.ListCfg = WordArea.ListFields;

            return listcfg;
        }
        /// <summary>
        /// 关联主表获取文件附件
        /// </summary>
        /// <param name="JsonData">包含库名、表名、主表Code</param>
        /// <returns></returns>
        public DataHandle.ListObject GetAttachList(string JsonData)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            DataTable dt = new DataTable();
            DataHandle.ListObject listcfg = new DataHandle.ListObject();

            WordArea.RedisKey = $"AttachStore.Tbl_AttachList.Config.json";
            WordArea.Json = Redis.Default.StringGet(WordArea.RedisKey);
            JObject Struct = JObject.Parse(WordArea.Json);
            JObject WherePara = JObject.Parse(JsonData);
            WordArea.SqlCmd = $" select ";
            WordArea.Fields = "ID";
            WordArea.ListFields = "ID:0:ID";
            JArray SubList = (JArray)Struct["Fields"];
            for (int y = 2; y < SubList.Count - 5; y++)
            {
                WordArea.Fields += $",{SubList[y]["Field"]}";
                WordArea.ListFields += $",{SubList[y]["Field"]}:" +
                    $"1:{SubList[y]["FieldCn"]}";
            }
            WordArea.SqlCmd += WordArea.Fields;
            WordArea.SqlCmd += " from AttachStore.dbo.Tbl_AttachList  WITH(NOLOCK) ";
            WordArea.SqlCmd += $" where ParentCode=@ParentCode and " +
                $" SourceDb=@SourceDb and SourceTb=@SourceTb ;";
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype
            {
                ColName = $"@ParentCode",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = WherePara["Code"].ToString()
            });
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype
            {
                ColName = $"@SourceDb",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = WherePara["SourceDb"].ToString()
            });
            WordArea.SqlParas.ValuePara.Add(new DataHandle.SQLtype
            {
                ColName = $"@SourceTb",
                ColType = Mo.ConvertSQLType("nvarchar"),
                ColLeng = 50,
                ColValue = WherePara["SourceTb"].ToString()
            });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            listcfg.ListDt = dt;
            listcfg.ListCfg = WordArea.ListFields;

            return listcfg;
        }
    }
}
