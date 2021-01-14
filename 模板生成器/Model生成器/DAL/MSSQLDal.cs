using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using DBUtil;
using Model生成器.Utils;
namespace DAL
{
    /// <summary>
    /// MSSQL数据库DAL
    /// </summary>
    public class MSSQLDal : IDal
    {
        public string CONN;
        #region 获取所有表信息
        /// <summary>
        /// 获取所有表信息
        /// </summary>
        public List<Dictionary<string, string>> GetAllTables()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
            MSSQLHelper dbHelper = new MSSQLHelper();
            dbHelper.connectionString = CONN;
            DataTable dt = dbHelper.Query(string.Format(@"
                SELECT distinct tbs.name as TABLE_NAME,ds.value as COMMENTS 
                FROM sys.tables tbs
                left join sys.extended_properties ds on ds.major_id=tbs.object_id 
                Where ds.minor_id=0"));
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("table_name", dr["TABLE_NAME"].ToString());
                dic.Add("comments", dr["COMMENTS"].ToString());
                result.Add(dic);
            }
            return result;
        }
        #endregion

        #region 获取表的所有字段名及字段类型
        /// <summary>
        /// 获取表的所有字段名及字段类型
        /// </summary>
        public List<Dictionary<string, string>> GetAllColumns(string tableName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
            MSSQLHelper dbHelper = new MSSQLHelper();
            dbHelper.connectionString = CONN;
            DataTable dt = dbHelper.Query(string.Format(@"
                select c.name,c.is_nullable,ds.value,ts.name as column_type,c.max_length,c.precision,c.scale
                from sys.columns c
                left join sys.extended_properties ds on ds.major_id=c.object_id and ds.minor_id=c.column_id
                left join sys.types ts on c.system_type_id=ts.system_type_id and ts.user_type_id=c.user_type_id
                left join sys.tables tbs on tbs.object_id=c.object_id
                where tbs.name='{0}' 
                order by c.column_id", tableName));
            
            DataTable dtPK = dbHelper.Query(string.Format(@"
                select b.column_name 
                from  information_schema.table_constraints a
                inner join information_schema.constraint_column_usage b
                on a.constraint_name = b.constraint_name
                where a.constraint_type = 'PRIMARY KEY' 
                and a.table_name = '{0}'", tableName));
            string strPK = string.Empty;
            if (dtPK.Rows.Count > 0)
            {
                strPK = dtPK.Rows[0]["column_name"].ToString();
            }
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("columns_name", dr["name"].ToString());
                dic.Add("notnull", dr["is_nullable"].ToString() == "False" ? "0" : "1");
                dic.Add("comments", dr["value"].ToString());
                string dataType = dr["column_type"].ToString();
                dic.Add("data_type", dataType);
                dic.Add("data_scale", dr["scale"].ToString());
                dic.Add("data_precision", dr["precision"].ToString());
                dic.Add("maxleng", dr["max_length"].ToString());
                if (dr["name"].ToString() == strPK)
                {
                    dic.Add("constraint_type", "P");
                }
                else
                {
                    dic.Add("constraint_type", "");
                }
                result.Add(dic);
            }
            return result;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 类型转换
        /// </summary>
        public string ConvertDataType(Dictionary<string, string> column)
        {
            string data_type = "string";
            switch (column["data_type"])
            {
                case "int":
                    if (column["notnull"] == "1")
                    {
                        data_type = "int";
                    }
                    else
                    {
                        data_type = "int?";
                    }
                    break;
                case "bigint":
                    if (column["notnull"] == "1")
                    {
                        data_type = "long";
                    }
                    else
                    {
                        data_type = "long?";
                    }
                    break;
                case "decimal":
                    if (column["notnull"] == "1")
                    {
                        data_type = "decimal";
                    }
                    else
                    {
                        data_type = "decimal?";
                    }
                    break;
                case "nvarchar":
                    data_type = "string";
                    break;
                case "varchar":
                    data_type = "string";
                    break;
                case "text":
                    data_type = "string";
                    break;
                case "ntext":
                    data_type = "string";
                    break;
                case "datetime":
                    if (column["notnull"] == "1")
                    {
                        data_type = "DateTime";
                    }
                    else
                    {
                        data_type = "DateTime?";
                    }
                    break;
                default:
                    throw new Exception("Model生成器未实现数据库字段类型" + column["data_type"] + "的转换");
            }
            return data_type;
        }
        #endregion

        #region  获取全部数据库

        public DataTable GetDataBase()
        {
            
            string connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
            MSSQLHelper dbHelper = new MSSQLHelper();
            dbHelper.connectionString = CONN;
            DataTable dt = dbHelper.Query(string.Format(@"
                SELECT name,dbid FROM Master..SysDatabases ORDER BY Name "));
            return dt;
        }
        #endregion

        public DataTable ListData(string tb)
        {
            DataTable dt = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
            MSSQLHelper dbHelper = new MSSQLHelper();
            dbHelper.connectionString = CONN;
            dt = dbHelper.Query(string.Format(@"
                SELECT top 20 * 
                FROM Tbl_ParityUnit"
                ));
            return dt;
        }

        public int UpOrIns()
        {
            int FLG = 0;
            MSSQLHelper dbHelper = new MSSQLHelper();
            dbHelper.connectionString = CONN;
            FLG = dbHelper.ExecuteSql(string.Format(@"
                INSERT INTO [Collection].[dbo].[Tbl_ParityUnit]
                       ([Name],[UnitCode]) VALUES ('aaa','aaa') ;
                INSERT INTO [Collection].[dbo].[Tbl_ParityUnit]
                       ([Name],[UnitCode]) VALUES ('bbb','bbb') ;
                INSERT INTO [Collection].[dbo].[Tbl_ParityUnit]
                       ([Name],[UnitCode]) VALUES ('ccc','ccc') ;
                INSERT INTO [Collection].[dbo].[Tbl_ParityUnit]
                       ([Name],[UnitCode]) VALUES ('ccc') ;              
                "));
            return FLG;
        }
        public DataTable ListData(string SQLstring, List<DataHandle.SQLtype> SqlPara)
        {
            DataTable dt = null;
            MSSQLHelper dbHelper = new MSSQLHelper();
            dbHelper.connectionString = CONN;
            dt = dbHelper.Query(SQLstring, SqlPara);
            return dt;
        }
        public int UpOrIns(string SQLstring, List<DataHandle.SQLtype> SqlPara)
        {
            int FLG = 0;
            MSSQLHelper dbHelper = new MSSQLHelper();
            dbHelper.connectionString = CONN;
            FLG = dbHelper.ExecuteSql(SQLstring, SqlPara);
            return FLG;
        }
    }
}
