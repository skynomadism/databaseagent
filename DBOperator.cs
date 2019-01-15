using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DataBaseAgent
{
    public abstract class DBOperator
    {
        public abstract int WriteData(string SQLstring, CommandType CommandType, string[,] SqlParams, object[] values);//写数据库操作
        public abstract DataSet ReadData(string SQLstring, CommandType CommandType, string[,] SqlParams, object[] values);//读数据库操作
        public abstract void Open(); //打开数据库连接
        public abstract void Close(); //关闭数据库连接
        public abstract void BeginTrans(); //开始一个事务
        public abstract void CommitTrans(); //提交一个事务
        public abstract void RollbackTrans(); //回滚一个事务
        public abstract int UpdateCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] SqlParams);
        public abstract int InsertCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] SqlParams);
        public abstract int DeleteCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] SqlParams);
        public abstract int SelectCommandBaseDataSet(DataSet SoureDataSet, string SoureTableName, string SQLstring, CommandType CommandType, string[,] SqlParams);
    }
}
