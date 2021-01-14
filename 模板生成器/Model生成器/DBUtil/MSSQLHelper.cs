using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Model;
using Model生成器.Utils;
namespace DBUtil
{
    /// <summary>
    /// 操作类
    /// </summary>
    public class MSSQLHelper
    {
        #region 变量
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        //private string connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
        public string connectionString= GetCONN();
        #endregion

        #region Exists
        public bool Exists(string SQLString)
        {
            SqlTransaction tx=null;
            bool IfOk=true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        tx = connection.BeginTransaction();
                        cmd.Transaction = tx;
                        object obj = cmd.ExecuteScalar();
                        tx.Commit();
                        /*
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            IfOk= false;
                        }
                        else
                        {
                            IfOk= true;
                        }
                        */
                    }
                    catch (Exception e)
                    {
                        IfOk = false;
                        tx.Rollback();
                        connection.Close();
                        //throw new Exception(e.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            return IfOk;
        }
        #endregion

        #region 执行SQL语句，返回影响的记录数
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString)
        {
            SqlTransaction tx = null;
            int rows = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        tx = connection.BeginTransaction();
                        cmd.Transaction = tx;
                        rows = cmd.ExecuteNonQuery();
                        tx.Commit();                        
                    }
                    catch (Exception e)
                    {
                        tx.Rollback();
                        connection.Close();
                        //throw new Exception(e.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            return rows;
        }
        #endregion

        #region 执行查询语句，返回DataTable
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    DataSet ds = new DataSet();
                    command.Fill(ds, "ds");
                    return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public int ExecuteSql(string SQLString, List<DataHandle.SQLtype> SqlPara)
        {
            SqlTransaction trans = null;
            int rows = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        foreach (DataHandle.SQLtype item in SqlPara)
                        {
                            cmd.Parameters.Add(item.ColName, item.ColType, item.ColLeng).Value = item.ColValue;
                        }
                        connection.Open();
                        trans = connection.BeginTransaction();
                        cmd.Transaction = trans;
                        rows = cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        connection.Close();
                       // LogHelper.Error(ex);
                        //throw new Exception(ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            return rows;
        }
        #endregion
        #region 执行查询语句，返回DataTable  参数模式
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string SQLString, List<DataHandle.SQLtype> SqlPara)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();

                    cmd.CommandText = SQLString;

                    foreach (DataHandle.SQLtype item in SqlPara)
                    {
                        cmd.Parameters.Add(item.ColName, item.ColType, item.ColLeng).Value = item.ColValue;
                    }

                    cmd.Connection = connection;

                    SqlDataReader read = cmd.ExecuteReader();
                    table = DataReaderToDataTable(read);
                }
                catch (Exception ex)
                {
                    //LogHelper.Error(ex);
                    //throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return table;
        }
        #endregion
        /// <summary>
        /// SqlDataReader 转成 DataTable
        /// 源需要是结果集
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public DataTable DataReaderToDataTable(SqlDataReader dataReader)
        {
            ///定义DataTable  
            DataTable datatable = new DataTable();

            try
            {    ///动态添加表的数据列  
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    DataColumn myDataColumn = new DataColumn();
                    myDataColumn.DataType = dataReader.GetFieldType(i);
                    myDataColumn.ColumnName = dataReader.GetName(i);
                    datatable.Columns.Add(myDataColumn);
                }

                ///添加表的数据  
                while (dataReader.Read())
                {
                    DataRow myDataRow = datatable.NewRow();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        myDataRow[i] = dataReader[i].ToString();
                    }
                    datatable.Rows.Add(myDataRow);
                    myDataRow = null;
                }
                ///关闭数据读取器  
                dataReader.Close();
            }
            catch (Exception ex)
            {
                ///抛出类型转换错误  
                //LogHelper.Error(ex);
                //throw new Exception(ex.Message, ex);
            }
            return datatable;
        }
        public static string GetCONN()
        {
            string Str = string.Empty;
            if (!string.IsNullOrEmpty(Form1.DataCurrent))
            {
                Form1.CONN.Data = $"Initial Catalog={Form1.DataCurrent};";
                Str = $"{Form1.CONN.Host.ToString()}{Form1.CONN.Data.ToString()}{Form1.CONN.Account.ToString()}{Form1.CONN.Pwd.ToString()}{Form1.CONN.ModelType.ToString()}";
            }
            else
            {
                Str = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
            }
            return Str;
        }

    }
}
