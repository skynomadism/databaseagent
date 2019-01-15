using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Windows.Forms;

namespace DataBaseAgent
{
    class OleDBOperator : DBOperator
    {
        /*这里之所以用两个是发现这个类写的有点bug，conn这个变量的事务被调用了两次，会引起错误，所以进行了临时处理*/
        private OleDbConnection conn1;//数据库连接
        //private OleDbConnection conn2;//备用数据库连接
        //private OleDbConnection conn3;//数据库连接
        //private OleDbConnection conn4;//备用数据库连接
        //private OleDbConnection conn5;//数据库连接
        //private OleDbConnection conn6;//备用数据库连接
        //private OleDbConnection conn7;//数据库连接
        //private OleDbConnection conn8;//备用数据库连接
        //private OleDbConnection conn9;//数据库连接
        private OleDbConnection conn;//备用数据库连接

        private OleDbTransaction trans;//事务处理
        private Queue<OleDbConnection> ConnQueue = new Queue<OleDbConnection>();//连接队列
        private bool inTransaction = false;//指示是否处理事务中
        /// <summary>
        /// 用于转换数据库字段类型为当前数据库操作可用类型
        /// </summary>
        private TypeConvert myDBConvert;

        public OleDBOperator()
        {
            conn1 = new Conn().GetOleDBConn();
            conn = new Conn().GetOleDBConn();
            ConnQueue.Enqueue(conn1);
            ConnQueue.Enqueue(conn);
        }
        public OleDBOperator(int dbID)
        {
            conn1 = new Conn().GetOleDBConn(dbID);
            conn = new Conn().GetOleDBConn(dbID);
            ConnQueue.Enqueue(conn1);
            ConnQueue.Enqueue(conn);
        }
        /// <summary>
        /// 获取一个OLE类型的数据库连接对像。主要用于Access数据库。
        /// 如果strType是0表示前面的string是一个*.mdb文件的文件名。如果是1表示是一个*.mdb文件的数据库连接字符串
        /// </summary>
        /// <param name="fileNameOrConnectionString"></param>
        /// <param name="strType"></param>
        public OleDBOperator(string fileNameOrConnectionString,int strType=0)
        {
            conn1 = new Conn().GetOleDBConn(fileNameOrConnectionString, strType);
            conn = new Conn().GetOleDBConn(fileNameOrConnectionString, strType);
            ConnQueue.Enqueue(conn1);
            ConnQueue.Enqueue(conn);
        }

        /// <summary>
        /// 执行一个读数据库操作
        /// </summary>
        /// <param name="SQLstring">读数据库的sql语句</param>
        /// <param name="SqlParams">sql语句中的参数</param>
        /// <param name="values">参数的值</param>
        /// <returns></returns>
        public override DataSet ReadData(string SQLstring, CommandType CommandType, string[,] OleDbParams, object[] values)
        {

            DataSet ds = new DataSet();
            OleDbDataAdapter ad = new OleDbDataAdapter();
            try
            {
                ad.SelectCommand = this.CreateCommand(SQLstring, CommandType, OleDbParams, values);
                //ad.SelectCommand.Connection.Open();
                //ad.SelectCommand.Transaction = ad.SelectCommand.Connection.BeginTransaction();
                ad.Fill(ds);
                ad.SelectCommand.Transaction.Commit();
                ad.SelectCommand.Connection.Close();
                ConnQueue.Enqueue(ad.SelectCommand.Connection);
            }
            catch (Exception exp) 
            {
                ad.SelectCommand.Transaction.Rollback();
                if (ad.SelectCommand.Connection.State.ToString().ToUpper() == "OPEN") 
                {
                    ad.SelectCommand.Connection.Close();
                }
                ConnQueue.Enqueue(ad.SelectCommand.Connection);
                System.Reflection.Assembly ass;
                Type type;
                try
                {
                    //ass = System.Reflection.Assembly.LoadFile(Application.StartupPath + @"\SkyToolBox.dll");//要绝对路径
                    //type = ass.GetType("SkyToolBox.FileIO.TxtFile");//必须使用名称空间+类名称
                    //System.Reflection.MethodInfo method = type.GetMethod("WriteLog");//方法的名称
                    //method.Invoke(null, new string[] { "[" + SQLstring + "][读数据错误]:" + exp.Message }); //静态方法的调用
                    //method = null;
                }
                catch (Exception ex)
                {
                  
                }
                finally
                {
                    ass = null;
                    type = null;
                }

            }
            return ds;
        }
        /// <summary>
        /// 执行一个写数据库操作
        /// </summary>
        /// <param name="SQLstring">写数据库操作的sql语句</param>
        /// <param name="SqlParams">sql语句中的参数</param>
        /// <param name="values">参数的值</param>
        /// <returns></returns>
        public override int WriteData(string SQLstring, CommandType CommandType, string[,] OleDbParams, object[] values)
        {
            OleDbCommand cmd = new OleDbCommand();
            try 
            {
                cmd = this.CreateCommand(SQLstring, CommandType, OleDbParams, values);
                //cmd.Connection.Open();
                //cmd.Transaction = cmd.Connection.BeginTransaction();
                int result = cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                cmd.Connection.Close();
                ConnQueue.Enqueue(cmd.Connection);
                return result;
            }
            catch (Exception exp) 
            {
                cmd.Transaction.Rollback();
                if (cmd.Connection.State.ToString().ToUpper() == "OPEN") 
                {
                    cmd.Connection.Close();
                }
                ConnQueue.Enqueue(cmd.Connection);
                System.Reflection.Assembly ass;
                Type type;
                try
                {

                    //ass = System.Reflection.Assembly.LoadFile(Application.StartupPath + @"\SkyToolBox.dll");//要绝对路径
                    //type = ass.GetType("SkyToolBox.FileIO.TxtFile");//必须使用名称空间+类名称
                    //System.Reflection.MethodInfo method = type.GetMethod("WriteLog");//方法的名称
                    //method.Invoke(null, new string[] { "["+SQLstring+"][写数据错误]:" + exp.Message }); //静态方法的调用
                    //method = null;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    ass = null;
                    type = null;
                }
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

        public override int UpdateCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] OleDbParams)
        {
            int n = -1;
            OleDbDataAdapter ad = new OleDbDataAdapter();
            ad.UpdateCommand = this.CreateCommand(SQLstring, CommandType, OleDbParams);
            n = ad.Update(SoureDataSet, SoureTableName);
            ConnQueue.Enqueue(ad.UpdateCommand.Connection);
            if (n > 0)
            {
                SoureDataSet.Tables[SoureTableName].AcceptChanges();
            }
            return n;
        }

        public override int InsertCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] OleDbParams)
        {
            int n = -1;
            OleDbDataAdapter ad = new OleDbDataAdapter();
            ad.InsertCommand = this.CreateCommand(SQLstring, CommandType, OleDbParams);
            n = ad.Update(SoureDataSet, SoureTableName);
            ConnQueue.Enqueue(ad.InsertCommand.Connection);
            if (n > 0)
            {
                SoureDataSet.Tables[SoureTableName].AcceptChanges();
            }
            return n;
        }

        public override int DeleteCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] OleDbParams)
        {
            int n = -1;
            OleDbDataAdapter ad = new OleDbDataAdapter();
            ad.DeleteCommand = this.CreateCommand(SQLstring, CommandType, OleDbParams);
            n = ad.Update(SoureDataSet, SoureTableName);
            ConnQueue.Enqueue(ad.DeleteCommand.Connection);
            if (n > 0)
            {
                SoureDataSet.Tables[SoureTableName].AcceptChanges();
            }
            return n;
        }

        public override int SelectCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] OleDbParams)
        {
            int n = -1;
            OleDbDataAdapter ad = new OleDbDataAdapter();
            ad.SelectCommand = this.CreateCommand(SQLstring, CommandType, OleDbParams);
            n = ad.Fill(SoureDataSet, SoureTableName);
            ConnQueue.Enqueue(ad.SelectCommand.Connection);
            if (n > 0)
            {
                SoureDataSet.Tables[SoureTableName].AcceptChanges();
            }
            return n;
        }
        private OleDbCommand CreateCommand(string SQLstring, CommandType CommandType, string[,] SqlParams)
        {
            try
            {
                myDBConvert = new TypeConvert();
                OleDbCommand cmd = new OleDbCommand();
                while (true)
                {
                    if (ConnQueue.Count > 0)
                    {
                        cmd.Connection = ConnQueue.Dequeue();
                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }
                        cmd.Transaction = cmd.Connection.BeginTransaction();
                        break;
                    }
                }
                //if (inTransaction)
                //{
                //    cmd.Transaction = trans;
                //}
                cmd.CommandType = CommandType;
                cmd.CommandText = SQLstring;

                if (SqlParams != null)
                {
                    for (int i = 0; i < SqlParams.Length; i++)
                    {
                        if (SqlParams[i, 2] == "")
                        {
                            cmd.Parameters.Add(SqlParams[i, 0], myDBConvert.ConvertOleDB(SqlParams[i, 1]));
                        }
                        else
                        {
                            cmd.Parameters.Add(SqlParams[i, 0], myDBConvert.ConvertOleDB(SqlParams[i, 1]), int.Parse(SqlParams[i, 2]), SqlParams[i, 3]);
                        }

                    }
                }
                return cmd;
            }
            catch (Exception exp)
            {
                //throw new Exception(exp);
                return null;
            }
   
        }
        private OleDbCommand CreateCommand(string SQLstring, CommandType CommandType, string[,] SqlParams, object[] values)
        {
            try 
            {
                myDBConvert = new TypeConvert();
                OleDbCommand cmd = new OleDbCommand();
                while (true)
                {
                    if (ConnQueue.Count > 0)
                    {
                        cmd.Connection = ConnQueue.Dequeue();
                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }
                        cmd.Transaction = cmd.Connection.BeginTransaction();
                        break;
                    }
                }
                //if (inTransaction)
                //{
                //    cmd.Transaction = trans;
                //}
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
                            cmd.Parameters.Add(SqlParams[i, 0], myDBConvert.ConvertOleDB(SqlParams[i, 1]));
                        }
                        else
                        {
                            cmd.Parameters.Add(SqlParams[i, 0], myDBConvert.ConvertOleDB(SqlParams[i, 1]), int.Parse(SqlParams[i, 2]), SqlParams[i, 3]);
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
            catch (Exception exp)
            {
                //throw new Exception(exp);
                return null;
            }
            
        }
    }
}
