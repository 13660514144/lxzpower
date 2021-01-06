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
    public class HelpConst
    {
        public int Ipage = 20;
        public string DataConnType = "mssql";
        IDal dal;
        public string ThisConn;
        public DataHandle Mo = new DataHandle();
        /// <summary>
        /// 获取最后一个常量编码 公用
        /// </summary>
        /// <returns></returns>
        public string GetConstLastCode(JObject Struct, JObject WhereForm)
        {
            DataTable Dt = null;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            string Result = string.Empty;
            WordArea.SqlCmd = $@"
                select top 1 A.ID,A.Code,A.CodeCn 
                    from {Struct["Db"]}.dbo.{Struct["Tb"]} A WITH(NOLOCK) ";

            string Code = WhereForm["UpperCode"].ToString();
            string where = string.Empty;
            if (Code == "*")//根节点
            {
                WordArea.SqlCmd += $" WHERE LEN(Code)=3  order by A.Code desc ";
            }
            else//子节点
            {
                int CodeLen = Code.Length;
                WordArea.SqlCmd +=
                    $" WHERE left(Code,LEN(Code)-3)=@Code" +
                    $" order by A.ID desc ";
                WordArea.SqlParas.ValuePara.Add(
                    new DataHandle.SQLtype
                    {
                        ColName = $"@Code",
                        ColType = Mo.ConvertSQLType("nvarchar"),
                        ColLeng = 50,
                        ColValue = Code
                    });
            }

            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            dynamic obj = new DynamicObj();
            obj.Data = Dt;
            obj.SourceCode = Code;
            Result = JsonConvert.SerializeObject(obj._values);
            return Result;
        }
        /// <summary>
        /// 计算根节点
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="SourceCode"></param>
        /// <returns></returns>
        public JObject CalculLastCode(string Code, string SourceCode)
        {
            string Result = string.Empty;

            if (SourceCode == "*")//根节点
            {
                if (string.IsNullOrEmpty(Code))//第一个根节点，从100开始
                {
                    Result = $"000";
                }
                else
                {
                    int num = Convert.ToInt32(Code);
                    num++;
                    if (num <= 999)
                    {
                        Result = $"{num.ToString().PadLeft(3,'0')}";
                    }
                    else
                    {
                        Result = "NO";
                    }
                }
            }
            else
            {

                if (string.IsNullOrEmpty(Code))
                {
                    Result = $"{SourceCode}000";
                }
                else
                {
                    string RightStr = Code.Substring(Code.Length - 3, 3);
                    int CodeNum = Convert.ToInt32(RightStr);
                    CodeNum++;
                    if (CodeNum <= 999)
                    {
                        Result = $"{SourceCode}{CodeNum.ToString().PadLeft(3, '0')}";
                    }
                    else
                    {
                        Result = "NO";
                    }
                }
            }
            JObject obj = new JObject(
                new JProperty("Code", Result)
                );
            return obj;
        }
        /// <summary>
        /// 外联表Select key Value
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public DataTable GetOtherCodeName(string[] Struct, string[] Fields,string UnitCode="")
        {
  
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            string AndStr = string.Empty;
            AndStr = " and UnitCode like ''+@UnitCode+'%' ";
            WordArea.SqlCmd = $@"
                    SELECT [{Fields[0]}] as Code,
                    [{Fields[1]}] as CodeCn                                  
                    FROM {Struct[0]}.[dbo].[{Struct[1]}] WITH(NOLOCK)
                    where DelFlg=@DelFlg {AndStr} ORDER by [{Fields[0]}]";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@DelFlg",
                    ColType = Mo.ConvertSQLType("int"),
                    ColLeng = 50,
                    ColValue = "1"
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@UnitCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = UnitCode
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            DataTable Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            return Dt;
        }
        /// <summary>
        /// 外联表Select key Value  用于级联选择
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public DataTable GetOtherCodeNameLink(string[] Struct, string[] Fields,string WherValue)
        {
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            string AndStr = $" and {Fields[2]}=@{Fields[2]} ";
            
            WordArea.SqlCmd = $@"
                    SELECT [{Fields[0]}] as Code,
                    [{Fields[1]}] as CodeCn                                  
                    FROM {Struct[0]}.[dbo].[{Struct[1]}] WITH(NOLOCK)
                    where DelFlg=@DelFlg {AndStr} ORDER by [{Fields[0]}]";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@DelFlg",
                    ColType = Mo.ConvertSQLType("int"),
                    ColLeng = 50,
                    ColValue = "1"
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@{Fields[2]}",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = WherValue
                });
            LogHelper.Error(WordArea.SqlCmd);
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            DataTable Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            return Dt;
        }

        /// <summary>
        /// 外联表Select 取角色
        /// </summary>
        /// <param name="Struct"></param>
        /// <param name="Fields"></param>
        /// <param name="Role"></param>
        /// <returns></returns>
        public DataTable GetOtherRoleName(string[] Struct, string[] Fields, string Role = "")
        {

            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            string AndStr = string.Empty;
            AndStr = " and Code like ''+@Code+'%' ";
            WordArea.SqlCmd = $@"
                    SELECT [{Fields[0]}] as Code,
                    (space(len(Code))+[{Fields[1]}]) as CodeCn                                  
                    FROM {Struct[0]}.[dbo].[{Struct[1]}] WITH(NOLOCK)
                    where DelFlg=@DelFlg {AndStr} ORDER by [{Fields[0]}]";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@DelFlg",
                    ColType = Mo.ConvertSQLType("int"),
                    ColLeng = 50,
                    ColValue = "1"
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@Code",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = Role
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            DataTable Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            return Dt;
        }
        /// <summary>
        /// 服务配置编码 算号器
        /// </summary>
        /// <param name="SysCode"></param>
        /// <param name="CodeType"></param>
        /// <returns></returns>
        public string GetServerCode(string SysCode,string CodeType)
        {
            string Result = string.Empty;
            string CodeTypeStr = string.Empty;
            switch (CodeType)
            {
                case "007000":
                    CodeTypeStr = "F";
                    break;
                case "007001":
                    CodeTypeStr = "U";
                    break;
                case "007002":
                    CodeTypeStr = "L";
                    break;
                case "007003":
                    CodeTypeStr = "M";
                    break;
                case "007004":
                    CodeTypeStr = "G";
                    break;
                case "007005":
                    CodeTypeStr = "A";
                    break;
            }
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlCmd = $@"
                SELECT top 1 [SysCode]
                  ,[Code]
                FROM [SysConfig].[dbo].[Tbl_Config] WITH(NOLOCK)
                 order by ID DESC
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
            DataTable Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            string NewCode = string.Empty;
            /*编码规则 F-SysCode-100101 F 服务 SysCode 系统编码 100101 开始序列号 */
            if (Dt.Rows.Count < 1)
            {
                NewCode = $"{CodeTypeStr}-{SysCode}-100101";
            }
            else
            {
                string[] OldCode = Dt.Rows[0]["Code"].ToString().Trim().Split('-');
                string Number = (Convert.ToInt32(OldCode[2])+1).ToString();
                NewCode = $"{CodeTypeStr}-{SysCode}-{Number}";
            }
            return NewCode;
        }
        /// <summary>
        /// 生成源源码 按日期-随机码（6位）-较验码1位
        /// </summary>
        /// <returns></returns>
        public string BuildTraceCode()
        {
            string Result = string.Empty;
            string s1 = RandomChars.NumChar(6, RandomChars.UppLowType.random);
            string s2 = RandomChars.NumChar(1, RandomChars.UppLowType.random);            
            string s4 = RandomChars.ChangeTimeArea(DateTime.Now.ToString("yyyy-MM-dd"));
            Result = $"{s4}{s1}{s2}";
            return Result;
        }
        /// <summary>
        /// 标签批号
        /// </summary>
        /// <returns></returns>
        public string BuildTaceBatchNumber()
        {
            string Result = string.Empty;
            string d1=RandomChars.ChangeTimeArea(DateTime.Now.ToString("yyyy-MM-dd"));
            string t1 = DateTime.Now.ToString("HH:mm:ss:ffff");
            string[] t = t1.Split(':');
            string Bach = (Convert.ToInt32(t[0]) * 3600 +
                Convert.ToInt32(t[1]) * 60 + 
                Convert.ToInt32(t[2])+Convert.ToInt32(t[3])).ToString().PadLeft(5,'0');     
            string s3 = RandomChars.OnlyChar(2, RandomChars.UppLowType.upper);
            Result = $"{d1}{Bach}{s3}";
            return Result ;
        }
        /// <summary>
        /// 生成用户帐号
        /// </summary>
        /// <returns></returns>
        public string BuildUserAccount()
        {
            string Result = string.Empty;
            Result = RandomChars.NumChar(6, RandomChars.UppLowType.random);
            Result = RandomChars.CaseConvert(Result,RandomChars.UppLowType.upper);
            return Result;
        }
    }
}
