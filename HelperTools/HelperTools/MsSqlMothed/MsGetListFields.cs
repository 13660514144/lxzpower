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
    /// 从配置读按钮
    /// </summary>
    public class MsGetListFields
    {
        public int Ipage = 20;
        public string DataConnType = "mssql";
        IDal dal;
        public string ThisConn;
        public DataHandle Mo = new DataHandle();
        public RedisHelper Redis = new RedisHelper() { DbType = "TbStruct" };
        /// <summary>
        /// 读表结构字段
        /// </summary>
        /// <param name="StructKey">Redis中表结构Key</param>
        /// <returns></returns>
        public string GetListFields(string StructKey)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            string Result = string.Empty;
            string Json = Redis.Default.StringGet(StructKey);
            JObject FieldObj = JObject.Parse(Json);
            JArray FieldArr = (JArray)FieldObj["Fields"];
            string TListFields = string.Empty;
            /*
             * ID:0:ID:1:I:0:0:0:0
             * 字段名：是否前端隐藏：字段中文名：是否查询：查询值类型：
             * 是否常量：常量代码：是否转换输出：转换参数
             */
            string ListFields = "ID:0:ID:1:I:0:0:0:0";
            for (int x = 1; x < FieldArr.Count; x++)
            {
                JObject O = (JObject)FieldArr[x];
                if (O["IfConst"].ToString() == "1")
                {
                    ListFields += $"," +
                        $"{O["Field"]}:1:{O["FieldCn"]}:{O["IfSearch"]}:" +
                        $"{Mo.ConvertDataType(O["DataType"].ToString())}:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";
                    ListFields += WordArea.ListFields += $"," +
                        $"{O["Field"]}_X:0:{O["FieldCn"]}:0:N:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";
                }
                else
                {
                    string Hidden = O["IfOutHidden"].ToString();
                    ListFields += $"," +
                        $"{O["Field"]}:{Hidden}:{O["FieldCn"]}:{O["IfSearch"]}:" +
                        $"{Mo.ConvertDataType(O["DataType"].ToString())}:" +
                        $"{O["IfConst"]}:{O["ConstValue"]}:" +
                        $"{O["ChangeCol"]}:{O["ComboRestlt"]}";
                }                    
            }
            /*外联表*/

            JArray OutLink = (JArray)FieldObj["OutLink"];
            if (OutLink.Count > 0)
            {
                for (int x = 0; x < OutLink.Count; x++)
                {
                    //sb.Append($" left join {OutLink[x]["OutLine"]} {AliasStr} WITH(NOLOCK)  " +
                    //    $"on {AliasStr}.{OutLink[x]["onfield"]}=A.{OutLink[x]["owner"]}  ");
                    JArray OutFields = (JArray)OutLink[x]["Fileds"];
                    for (int z = 0; z < OutFields.Count; z++)
                    {
                        //TFields += $",{AliasStr}.{OutFields[z]["Col"]} as" +
                        //    $" {OutFields[z]["Col"]}_{OutLink[x]["owner"]} ";
                        TListFields += $",{OutFields[z]["Col"]}_{OutLink[x]["owner"]}" +
                            $":0:{OutFields[z]["ColName"]}:0:N:0:0:1:" +
                            $"{OutFields[z]["ComboRestlt"]}";
                    }
                    //Alias++;
                    //AliasStr = Mo.GetTbAlias(Alias);
                }
            }

            Result = $"{ListFields}{TListFields}";
            return Result;
        }
    }
}
