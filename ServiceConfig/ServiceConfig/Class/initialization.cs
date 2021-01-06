using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HelperTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ServiceConfig.Class
{
    public class Initialization
    {
        public string dir = AppDomain.CurrentDomain.BaseDirectory;
        public RedisHelper Redis = new RedisHelper();
        public string DataConnType = "mssql";
        public DataHandle Mo = new DataHandle();
        private string ThisConn;
        private string ImintFlg;
        IDal dal;

        public bool ReadConfig()
        {

            bool Flg = false;
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            string ConfigPath = System.Configuration.ConfigurationManager.AppSettings["ConFigPath"];
            try
            {
                string CONN = $@"{ConfigPath}\ConfigSet.json";
                JObject o = JObject.Parse(FileHelper.ReadFile(CONN));
                ImintFlg = o["ImintConfig"].ToString();
                ThisConn = o["ConnectionStr"].ToString();
                //是否需要读入初始化配置缓存，在系统新部署时候需要，
                //其他应用通过配置修改缓存
                if (ImintFlg == "1")
                {
                    #region 初始化配置文件  表结构，读入redis
                    Console.WriteLine($"表结构缓存开始{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
                    string Path = $"{ConfigPath}\\Tbstruct\\";
                    DirectoryInfo theDir = new DirectoryInfo(Path);
                    FileInfo[] fileInfo = theDir.GetFiles();
                    string[] Keys = new string[0];
                    string[] Values = new string[0];
                    foreach (FileInfo fInfo in fileInfo)
                    {
                        string F = fInfo.Name;
                        string Json = FileHelper.ReadFile($"{Path}{F}");

                        Array.Resize<string>(ref Keys, Keys.Length + 1);
                        Keys[Keys.Length - 1] = F;

                        Array.Resize<string>(ref Values, Values.Length + 1);
                        Values[Values.Length - 1] = Json;
                    }
                    Redis.DbType = "TbStruct";
                    Redis.Default.StringSetMany(Keys, Values);
                    Console.WriteLine($"缓存表结构完成{DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff")}");
                    #endregion
                    Console.WriteLine($"缓存服务配置开始{DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff")}");
                    #region  初始化服务 对应 TBL_CONFIG
                    WordArea.SqlCmd = $@"
                        SELECT [Code],
                                ServiceDefinition,
                                Url,
                                Api,
                                RequestMothed,
                                IsEnabled,
                                Status,SysCode,ReMarks,Port
                        from SysConfig.dbo.Tbl_Config WITH(NOLOCK)
                        where DelFlg=@DelFlg
                        order by  SysCode,Code
                    ";
                    WordArea.SqlParas.ValuePara.Add(
                        new DataHandle.SQLtype
                        {
                            ColName = $"@DelFlg",
                            ColType = Mo.ConvertSQLType("int"),
                            ColLeng = 4,
                            ColValue = "1"
                        });
                    dal = DalFactory.CreateDal(DataConnType, ThisConn);
                    DataTable Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
                    Keys = new string[0];
                    Values = new string[0];
                    for (int x = 0; x < Dt.Rows.Count; x++)
                    {
                        ServiceStruct.ServiceModel Reg = new ServiceStruct.ServiceModel()
                        {
                            Struct = Dt.Rows[x]["ServiceDefinition"].ToString(),
                            DataService = Dt.Rows[x]["Url"].ToString(),
                            DataUrl = Dt.Rows[x]["Api"].ToString(),
                            ReqType = Dt.Rows[x]["RequestMothed"].ToString(),
                            IsEnabled = Dt.Rows[x]["IsEnabled"].ToString(),
                            Status = Dt.Rows[x]["Status"].ToString(),
                            SysCode = Dt.Rows[x]["SysCode"].ToString(),
                            Code = Dt.Rows[x]["Code"].ToString(),
                            CusField = Dt.Rows[x]["ReMarks"].ToString(),
                            Port = Dt.Rows[x]["Port"].ToString()
                        };
                        Array.Resize<string>(ref Keys, Keys.Length + 1);
                        Keys[Keys.Length - 1] = Dt.Rows[x]["Code"].ToString();

                        Array.Resize<string>(ref Values, Values.Length + 1);
                        Values[Values.Length - 1] = JsonConvert.SerializeObject(Reg);
                    }
                    Redis.DbType = "ServerCode";
                    Redis.Default.StringSetMany(Keys, Values);
                    Console.WriteLine($"缓存服务配置完成{DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff")}");
                    #endregion

                    #region  初始化菜单 对应 TBL_sysmodel
                    Console.WriteLine($"菜单缓存开始{DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff")}");
                    Path = $"{ConfigPath}\\BtnPara\\";
                    theDir = new DirectoryInfo(Path);
                    fileInfo = theDir.GetFiles();
                    Keys = new string[0];
                    Values = new string[0];
                    foreach (FileInfo fInfo in fileInfo)
                    {
                        string F = fInfo.Name;
                        string Json = FileHelper.ReadFile($"{Path}{F}");

                        Array.Resize<string>(ref Keys, Keys.Length + 1);
                        Keys[Keys.Length - 1] = F.Split('.')[0];//过滤文件名后缀

                        Array.Resize<string>(ref Values, Values.Length + 1);
                        Values[Values.Length - 1] = Json;
                    }

                    Redis.DbType = "MenuCode";
                    Redis.Default.StringSetMany(Keys, Values);
                    Console.WriteLine($"菜单缓存结束{DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff")}");
                    #endregion

                    Flg = true;
                }
                else
                {
                    Flg = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return Flg;

        }
        public static string JsonParse(DataTable dt)
        {
            DataRowCollection drc = dt.Rows;
            if (drc.Count == 0) { return "[]"; }
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = JsonFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        public static string JsonFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < str.Length; i++)
                {
                    char c = str.ToCharArray()[i];
                    switch (c)
                    {
                        case '\"':
                            sb.Append("\\\""); break;
                        case '\\':
                            sb.Append("\\\\"); break;
                        case '/':
                            sb.Append("\\/"); break;
                        case '\b':
                            sb.Append("\\b"); break;
                        case '\f':
                            sb.Append("\\f"); break;
                        case '\n':
                            sb.Append("\\n"); break;
                        case '\r':
                            sb.Append("\\r"); break;
                        case '\t':
                            sb.Append("\\t"); break;
                        default:
                            sb.Append(c); break;
                    }
                }
                str = sb.ToString();
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                if (IsDateTime(str))
                {
                    DateTime dt = DateTime.Parse(str);
                    str = "\"" + dt.GetDateTimeFormats('s')[0].ToString() + "\"";
                }
                else
                {
                    str = "\"" + str + "\"";
                }
            }
            else
            {
                str = "\"" + str + "\"";
            }
            return str;
        }
        /// <summary>
        /// 是否是日期格式(包含时间)
        /// </summary>
        /// <param name="str">DateTime Value</param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            if (string.IsNullOrEmpty(str)) { return false; }
            //日期
            if (str.Trim().IndexOf(" ") < 0)
            {
                return IsDate(str);
            }
            else
            {
                string[] Astr = str.Split(' '); //日期+时间
                if (Astr.Length == 2)
                {
                    return IsDate(Astr[0]) && System.Text.RegularExpressions.Regex.IsMatch(Astr[1], @"^(20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 日期正较验
        /// </summary>
        /// <param name="StrSource"></param>
        /// <returns></returns>
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }
    }
}
