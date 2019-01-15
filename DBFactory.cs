using DataBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseAgent
{
    public class DBFactory
    {
        internal SqlDBOperator SqlDBOperator
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        internal OleDBOperator OleDBOperator
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        internal MySqlDBOperator mySqlDBOperator 
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        
        public static DBOperator GetDBOperator()
        {
            return new OleDBOperator();
        }
        public static DBOperator GetDBOperator(string FileName)
        {
            return new OleDBOperator(FileName,0);
        }
        /// <summary>
        /// 依据connectionIndex规定的数据库类型返回数据库操作对像。1表示要的是ole类型的，2表示要的是sql类型的。3表示oracle类型的（暂时没有实现）。
        /// 其它数字均表示使用conn.xml规定的*.mdb文件为数据库类型的操作对像
        /// connectionString的值只有在connectionIndex为1或2时才有效
        /// </summary>
        /// <param name="connectionIndex"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DBOperator GetDBOperator(int connectionIndex,string connectionString="")
        {
            switch (connectionIndex)
            {
                case 1:
                    return connectionString == "" ? new OleDBOperator() : new OleDBOperator(connectionString,1);
                case 2:
                    return connectionString == "" ? new SqlDBOperator() : new SqlDBOperator(connectionString);
                case 3:
                    return connectionString == "" ? new OracleDBOperator() : new OracleDBOperator(connectionString);
                case 4:
                    return connectionString == "" ? new MySqlDBOperator() : new MySqlDBOperator(connectionString);
                default:
                    return new OleDBOperator();
            }
        }
        public static DBOperator GetDBOperator(int connectionIndex,int DataBaseID)
        {
            switch (connectionIndex)
            {
                case 1:
                    return new OleDBOperator(DataBaseID);
                case 2:
                    return new SqlDBOperator(DataBaseID);
                case 3:
                    return new OracleDBOperator(DataBaseID);
                case 4:
                    return new MySqlDBOperator(DataBaseID);
                default:
                    return new OleDBOperator();
            }
        }
    }
}
