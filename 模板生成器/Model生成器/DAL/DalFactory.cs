﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    /// <summary>
    /// 数据库操作工厂类
    /// </summary>
    public class DalFactory
    {
        /// <summary>
        /// 创建Dal
        /// </summary>
        /// <param name="databaseType">数据库类型，如SQLite、MySql</param>
        public static IDal CreateDal(string databaseType, string CONN)
        {
            switch (databaseType.ToLower())
            {
                //case "mysql":
                //    return new MySqlDal();
                //case "sqlite":
                //    return new SQLiteDal();
                //case "oracle":
                //    return new OracleDal();
                case "mssql":
                    MSSQLDal MsDal = new MSSQLDal();
                    MsDal.CONN = CONN;
                    return MsDal;
                default:
                    throw new Exception("数据库类型错误");
            }
        }
    }
}
