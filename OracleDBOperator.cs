using DataBaseAgent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

using System.Text;


namespace DataBase
{
    public class OracleDBOperator : DBOperator
    {
        private OracleConnection conn; //数据库连接 
        private OracleTransaction trans; //事务处理类 
        private bool inTransaction = false;//指示当前是否正处于事务中 
        /// <summary>
        /// 用于转换数据库字段类型为当前数据库操作可用类型
        /// </summary>
        private TypeConvert myDBConvert;

        public OracleDBOperator()
        {
            conn = new Conn().GetOracleConn();
        }
        public OracleDBOperator(int dbID)
        {
            conn = new Conn().GetOracleConn(dbID);
        }
        public OracleDBOperator(string ConnectString) 
        {
            conn = new Conn().GetOracleConn(ConnectString);
        }

        /// <summary>
        /// 执行一个读数据库操作
        /// </summary>
        /// <param name="SQLstring">读数据库的sql语句</param>
        /// <param name="SqlParams">sql语句中的参数</param>
        /// <param name="values">参数的值</param>
        /// <returns></returns>
        public override System.Data.DataSet ReadData(string SQLstring, CommandType CommandType, string[,] SqlParams, object[] values)
        {
            SQLstring = SQLstring.Replace("[","`").Replace("]","`");
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = this.conn;
            //if (inTransaction)
            //{
            //    cmd.Transaction = trans;
            //}
            //if ((SqlParams != null) && (SqlParams.Length != values.Length))
            //{
            //    //throw new ParamValueNotMatchException("查询参数和值不对应!");
            //}
            //cmd.CommandType = CommandType;
            //cmd.CommandText = SQLstring;
            //if (SqlParams != null)
            //{
            //    for (int i = 0; i < SqlParams.Length; i++)
            //    {
            //        cmd.Parameters.AddWithValue(SqlParams[i], values[i]);
            //    }
            //}
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = this.CreateCommand(SQLstring, CommandType, SqlParams, values);
            ad.Fill(ds);
            return ds;

        }
        /// <summary>
        /// 执行一个写数据库操作
        /// </summary>
        /// <param name="SQLstring">写数据库操作的sql语句</param>
        /// <param name="SqlParams">sql语句中的参数</param>
        /// <param name="values">参数的值</param>
        /// <returns></returns>
        public override int WriteData(string SQLstring, CommandType CommandType, string[,] SqlParams, object[] values)
        {
            try
            {
                SQLstring = SQLstring.Replace("[", "`").Replace("]", "`");
                OracleCommand cmd = new OracleCommand();
                cmd = this.CreateCommand(SQLstring, CommandType, SqlParams, values);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception exp) 
            {
                return 0;
            }

        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public override void Open()
        {
            if (conn.State.ToString().ToUpper() != "OPEN")
            {
                this.conn.Open();
            }
        }
        /// <summary>
        /// 开始一个事务
        /// </summary>
        public override void BeginTrans()
        {
            trans = conn.BeginTransaction();
            inTransaction = true;
        }
        /// <summary>
        /// 提交一个事务
        /// </summary>
        public override void CommitTrans()
        {
            trans.Commit();
            inTransaction = false;
        }
        /// <summary>
        /// 关闭一个数据库连接
        /// </summary>
        public override void Close()
        {
            if (conn.State.ToString().ToUpper() == "OPEN")
            {
                this.conn.Close();
            }
        }
        /// <summary>
        /// 回滚一个事务
        /// </summary>
        public override void RollbackTrans()
        {
            if (trans != null)
            {
                trans.Rollback();
                inTransaction = false;
            }

        }
        /// <summary>
        /// 把一个DataSet中的修改更新到数据库中
        /// </summary>
        /// <param name="SoureDataSet"></param>
        /// <param name="SoureTableName"></param>
        /// <param name="SQLstring"></param>
        /// <param name="SqlParams"></param>
        /// <param name="SoureColunmsTypes"></param>
        /// <param name="SoureColunmsSize"></param>
        public override int UpdateCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] SqlParams)
        {
            int n = -1;
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.UpdateCommand = this.CreateCommand(SQLstring, CommandType, SqlParams);
            n = ad.Update(SoureDataSet, SoureTableName);
            if (n > 0)
            {
                SoureDataSet.Tables[SoureTableName].AcceptChanges();
            }
            return n;


        }
        /// <summary>
        /// 把一个DataSet中的修改(插入)更新到数据库中
        /// </summary>
        /// <param name="SoureDataSet"></param>
        /// <param name="SoureTableName"></param>
        /// <param name="SQLstring"></param>
        /// <param name="SqlParams"></param>
        /// <param name="SoureColunmsTypes"></param>
        /// <param name="SoureColunmsSize"></param>
        public override int InsertCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] SqlParams)
        {
            int n = -1;
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.InsertCommand = this.CreateCommand(SQLstring, CommandType, SqlParams);
            n = ad.Update(SoureDataSet, SoureTableName);
            if (n > 0)
            {
                SoureDataSet.Tables[SoureTableName].AcceptChanges();
            }
            return n;


        }
        /// <summary>
        /// 把一个DataSet中的修改(删除)更新到数据库中
        /// </summary>
        /// <param name="SoureDataSet"></param>
        /// <param name="SoureTableName"></param>
        /// <param name="SQLstring"></param>
        /// <param name="SqlParams"></param>
        /// <param name="SoureColunmsTypes"></param>
        /// <param name="SoureColunmsSize"></param>
        public override int DeleteCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] SqlParams)
        {
            int n = -1;
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.DeleteCommand = this.CreateCommand(SQLstring, CommandType, SqlParams);
            n = ad.Update(SoureDataSet, SoureTableName);
            if (n > 0)
            {
                SoureDataSet.Tables[SoureTableName].AcceptChanges();
            }
            return n;
        }
        /// <summary>
        /// 把一个DataSet中的修改更新到数据库中
        /// </summary>
        /// <param name="SoureDataSet"></param>
        /// <param name="SoureTableName"></param>
        /// <param name="SQLstring"></param>
        /// <param name="SqlParams"></param>
        /// <param name="SoureColunmsTypes"></param>
        /// <param name="SoureColunmsSize"></param>
        public override int SelectCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] SqlParams)
        {
            int n = -1;
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = this.CreateCommand(SQLstring, CommandType, SqlParams);
            n = ad.Fill(SoureDataSet, SoureTableName);
            if (n > 0)
            {
                SoureDataSet.Tables[SoureTableName].AcceptChanges();
            }
            return n;
        }
        /// <summary>
        /// 依据参数生成一个命令对像
        /// </summary>
        /// <param name="SQLstring">查询语句或存储过程名</param>
        /// <param name="CommandType">指示本语句是查询语句还是存储过程</param>
        /// <param name="SqlParams">参数各清单,第一列是参数名，第二列是参数类型，第三列是列字长，第四列是数据源列名，第五列是返回还是输入还是其它</param>
        /// <returns></returns>
        private OracleCommand CreateCommand(string SQLstring, CommandType CommandType, string[,] SqlParams)
        {
            myDBConvert = new TypeConvert();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = this.conn;
            if (inTransaction)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType;
            cmd.CommandText = SQLstring;
            if (SqlParams != null)
            {
                int length = SqlParams.GetLength(0);
                for (int i = 0; i < length; i++)
                {
                    if (SqlParams[i, 2] == "")
                    {
                        cmd.Parameters.Add(SqlParams[i, 0], myDBConvert.ConvertSqlDB(SqlParams[i, 1]));
                    }
                    else
                    {
                        cmd.Parameters.Add(SqlParams[i, 0], myDBConvert.ConvertOracleDB(SqlParams[i, 1]), int.Parse(SqlParams[i, 2]), SqlParams[i, 3]);
                    }

                }
            }

            return cmd;
        }
        /// <summary>
        /// 依据参数生成一个命令对像
        /// </summary>
        /// <param name="SQLstring">查询语句或存储过程名</param>
        /// <param name="CommandType">指示本语句是查询语句还是存储过程</param>
        /// <param name="SqlParams">参数各清单,第一列是参数名，第二列是参数类型，第三列是列字长，第四列是数据源列名，第五列是返回还是输入还是其它</param>
        /// <param name="values">参数的值</param>
        /// <returns></returns>
        private OracleCommand CreateCommand(string SQLstring, CommandType CommandType, string[,] SqlParams, object[] values)
        {
            myDBConvert = new TypeConvert();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = this.conn;
            if (inTransaction)
            {
                cmd.Transaction = trans;
            }
            if ((SqlParams != null) && (SqlParams.GetLength(0) != values.Length))
            {
                throw new Exception("查询的参数与参数值的数量不一致！");
            }
            cmd.CommandType = CommandType;
            cmd.CommandText = SQLstring;
            if (SqlParams != null && values != null)
            {
                int length = SqlParams.GetLength(0);
                for (int i = 0; i < length; i++)
                {
                    if (SqlParams[i, 2] == "")
                    {
                        cmd.Parameters.Add(SqlParams[i, 0], myDBConvert.ConvertSqlDB(SqlParams[i, 1]));
                    }
                    else
                    {
                        cmd.Parameters.Add(SqlParams[i, 0], myDBConvert.ConvertOracleDB(SqlParams[i, 1]), int.Parse(SqlParams[i, 2]),
                            (SqlParams[i, 3]==""?SqlParams[i,0].Replace("@",""):SqlParams[i,3]));
                    }

                    if (SqlParams[i, 4] == "Output")
                    {
                        cmd.Parameters[SqlParams[i, 0]].Direction = ParameterDirection.Output;
                    }
                    else
                    {
                        cmd.Parameters[SqlParams[i, 0]].Value = values[i];
                    }
                }
            }
            return cmd;
        }
    }
}
