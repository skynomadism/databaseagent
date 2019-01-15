using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;

namespace DataBaseAgent
{
    public class TableObject
    {
        private CommandType _SqlCommandType = CommandType.Text;
        /// <summary>
        /// 获取或设置指定的CommandText是sql语句还是存储过程
        /// </summary>
        public CommandType SqlCommandType
        {
            get { return _SqlCommandType; }
            set { _SqlCommandType = value; }
        }
        private int _DataBaseTypeID = 2;
        /// <summary>
        /// 指示本次操作所用的数据库类型，0和1是本地数据库access,2是服务器数据库sqlServer,3是服务器数据库Oracle,4是服务器数据库MySql
        /// 0表示DataBaseNameOrConnStr这个属性传入的是一个mdb文件名，1表示传处的是一个oledb的连接字符串
        /// </summary>
        public int DataBaseTypeID
        {
            get { return _DataBaseTypeID; }
            set { _DataBaseTypeID = value; }
        }
        private int _DataBaseID = 1;
        /// <summary>
        /// 指示程序使用配置中哪个编号的数据库.
        /// 配合本模块有一个名称为conn.xml的文件，其中可以配置多个数据库连接。程序可以通过改变这个值来动态切换数据库
        /// 不过用户也可以使用本模块的DataBaseNameOrConnStr来直接设置连接字符串来达到动态切换数据库的目的
        /// </summary>
        public int DataBaseID
        {
            get { return _DataBaseID; }
            set { _DataBaseID = value; }
        }
        private string _DataBaseName = "";
        /// <summary>
        /// 指定使用程序在同一目录下的哪一个mdb类型的数据库
        /// 可用于在程序运行中生成的mdb数据文件，这些文件无法在配置中进行配置
        /// 另外这个值也可以用来表示connectiongString。一般是在Sqlserver,Oracl,Mysql时把此值当成连接字符串使用。
        /// </summary>
        public string DataBaseNameOrConnStr
        {
            get { return _DataBaseName; }
            set { _DataBaseName = value; }
        }
        private string _SqlString = "";
        /// <summary>
        /// SQL操作语句
        /// </summary>
        public string SqlString
        {
            get { return _SqlString; }
            set { _SqlString = value; }
        }
        private string[,] _SqlStringParams = null;
        /// <summary>
        /// 参数名(此参数主要用于Sql数据库)
        /// 数据的格式是一行为一个数据
        /// 每一行共有五列
        /// [参数名,参数类型,参数长度,参数对应字段,参数的输入输出方向]
        /// </summary>
        public string[,] SqlStringParams
        {
            get { return _SqlStringParams; }
            set { _SqlStringParams = value; }
        }
        private object[] _SqlStringParamsValues = null;
        /// <summary>
        /// 参数值清单
        /// </summary>
        public object[] SqlStringParamsValues
        {
            get { return _SqlStringParamsValues; }
            set { _SqlStringParamsValues = value; }
        }
        private List<ParamObject> _ParamsList;
        /// <summary>
        /// 利用参数对象序列初始化数据查询参数
        /// </summary>
        public List<ParamObject> ParamsList
        {
            get { return _ParamsList; }
            set {
                _ParamsList = value;
                if (_ParamsList != null && _ParamsList.Count > 0) 
                {
                    _SqlStringParams = new string[_ParamsList.Count, 5];
                    _SqlStringParamsValues = new object[_ParamsList.Count];
                    for (int i = 0; i < _ParamsList.Count; i++) 
                    {
                        _SqlStringParams[i, 0] = _ParamsList[i].ParamName;
                        _SqlStringParams[i, 1] = _ParamsList[i].SimpleType;
                        _SqlStringParams[i, 2] = _ParamsList[i].ParamSize == 0 ? "" : _ParamsList[i].ParamSize.ToString();
                        _SqlStringParams[i, 3] = _ParamsList[i].CloumName;
                        _SqlStringParams[i, 4] = _ParamsList[i].OperaterSign;
                        _SqlStringParamsValues[i] = _ParamsList[i].ParamValue;

                    }
                }
            }
        }
        /// <summary>
        /// 这是一个数据库连接代理对像。
        /// 设置好数据库类型（包括连接字符串）后即可使用它的基本增删改查的操作
        /// 它支持access,sqlserver,oracle三种数据库的连接操作。
        /// 支持Sql语句，存储过程等。
        /// </summary>
        public TableObject() { }

        #region 扩展操作

        public int InsertData(string SQLString,List<ParamObject> paramlist)
        {
            _SqlString = SQLString;
            this.ParamsList = paramlist;
            return InsertData();
        }
        public int UpdateData(string SQLString, List<ParamObject> paramlist)
        {
            _SqlString = SQLString;
            this.ParamsList = paramlist;
            return UpdateData();
        }
        public int DeleteData(string SQLString, List<ParamObject> paramlist)
        {
            _SqlString = SQLString;
            this.ParamsList = paramlist;
            return DeleteData();
        }
        public DataSet SearchData(string SQLString, List<ParamObject> paramlist)
        {
            _SqlString = SQLString;
            this.ParamsList = paramlist;
            return SearchData();
        }

        #endregion

        #region 操作
        public int InsertData()
        {
            return Write();
        }
        public int GetCountData()
        {
            return Write();
        }
        public int UpdateData()
        {
            return Write();
        }
        public int DeleteData()
        {
            return Write();
        }
        public DataSet SearchData()
        {
            return Read();
        }
        public bool TestConnect() 
        {
            return IsConnect();
        }
        #endregion

        #region 基础

        private DataBaseAgent.DBOperator GetDBOperator() 
        {
            DataBaseAgent.DBOperator myDB = null;
            if (_DataBaseName != "")
            {
                if (_DataBaseTypeID == 1 || _DataBaseTypeID == 2 || _DataBaseTypeID == 3 || _DataBaseTypeID == 4)
                {
                    myDB = DataBaseAgent.DBFactory.GetDBOperator(_DataBaseTypeID, _DataBaseName);
                }
                else
                {
                    myDB = DataBaseAgent.DBFactory.GetDBOperator(_DataBaseName);
                }
            }
            else
            {
                if (_DataBaseID != 1)
                {
                    myDB = DataBaseAgent.DBFactory.GetDBOperator(_DataBaseTypeID, _DataBaseID);
                }
                else
                {
                    myDB = DataBaseAgent.DBFactory.GetDBOperator(_DataBaseTypeID);
                }
            }
            return myDB;
        }

        private bool IsConnect() 
        {
            DataBaseAgent.DBOperator myDB = GetDBOperator();
            try
            {
                myDB.Open();
                myDB.Close();
                return true;
            }
            catch (Exception exp)
            {
                myDB.RollbackTrans();
                myDB.Close();
                return false;

            }
        }

        private int Write()
        {
            DataBaseAgent.DBOperator myDB = GetDBOperator();
            
            try
            {
                myDB.Open();
                myDB.BeginTrans();
                int count = myDB.WriteData(_SqlString, _SqlCommandType, _SqlStringParams, _SqlStringParamsValues);
                myDB.CommitTrans();
                myDB.Close();
                return count;
                
            }
            catch (Exception exp)
            {

                //myDB.RollbackTrans();
                myDB.Close();
                WriteLog(exp.ToString(), "Write", 1, 5);
                //throw new Exception(exp.Message);
                //new Logs.LogManage().WriteLogs(exp.Message);//日志模块还没有完成
                return 0;

            }
        }
        private DataSet Read()
        {
            DataBaseAgent.DBOperator myDB = GetDBOperator();
            try
            {
                myDB.Open();
                myDB.BeginTrans();
                DataSet ds = myDB.ReadData(_SqlString, _SqlCommandType, _SqlStringParams, _SqlStringParamsValues);
                myDB.Close();
                return ds;
            }
            catch (Exception exp)
            {
                //myDB.RollbackTrans();
                myDB.Close();
                WriteLog(exp.ToString(), "Read", 1, 5);
                //throw new Exception(exp.Message);
                //new Logs.LogManage().WriteLogs(exp.Message);//日志模块还没有完成
                return new DataSet();

            }
        }
        #endregion
        /// <summary>
        /// 将指定信息写入程序日志（当前程序目示下的LOG文件夹内）（本函数只支持应用程序，不支持web程序）
        /// </summary>
        /// <param name="sMsg">需要写入日志的信息</param>
        /// <param name="LogTypeName">日志文件所在的文件夹</param>
        /// <param name="Interval">0表示按小时分日志文件，1表示按天分日志文件，2表示按10分钟为单位分日志文件</param>
        /// <param name="KeepLogDaysCount">自动保留日志的天数(按小时来分日志的话，可以进行正确清理)</param>
        public static void WriteLog(string sMsg, string LogTypeName, int Interval = 0, int KeepLogDaysCount = 3)
        {
            try
            {
                if (sMsg != "")
                {
                    #region 按时间构造日志文件名称
                    //Random randObj = new Random(DateTime.Now.Millisecond);
                    //int file = randObj.Next() + 1;
                    string time = "";
                    if (Interval == 0) //按时分
                    {
                        time = DateTime.Now.ToString("yyyyMMddHHmm").Substring(0, 10);
                    }
                    if (Interval == 1) //按天分
                    {
                        time = DateTime.Now.ToString("yyyyMMddHHmm").Substring(0, 8);
                    }
                    if (Interval == 2) //按十分钟分
                    {
                        time = DateTime.Now.ToString("yyyyMMddHHmm").Substring(0, 11);
                    }
                    if (time == "")
                    {
                        time = DateTime.Now.ToString("yyyyMMddHHmm").Substring(0, 10);
                    }
                    string filename = time + ".log";
                    #endregion

                    try
                    {
                        string FileDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Sky_DB_Agent\\" + LogTypeName + "\\";
                        if (!Directory.Exists(FileDir))
                        { Directory.CreateDirectory(FileDir); }
                        FileInfo fi = new FileInfo(FileDir + filename);
                        if (!fi.Exists)
                        {
                            using (StreamWriter sw = fi.CreateText())
                            {
                                sw.WriteLine(DateTime.Now + "\n" + sMsg + "\n");
                                sw.Close();
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = fi.AppendText())
                            {
                                sw.WriteLine(DateTime.Now + "\n" + sMsg + "\n");
                                sw.Close();
                            }
                        }


                        #region 自动清理日志
                        //用连续清三次的办法保证日志一定会被清除。这段代码是为了保证程序在停止三天时间内任一时间启动都可以保证正常清理不需要的日志
                        string OldFileName = FileDir + DateTime.Now.AddDays(-KeepLogDaysCount).ToString("yyyyMMddHH") + ".log";

                        if (File.Exists(OldFileName))
                        { File.Delete(OldFileName); }

                        OldFileName = FileDir + DateTime.Now.AddDays(-(KeepLogDaysCount + 1)).ToString("yyyyMMddHH") + ".log";

                        if (File.Exists(OldFileName))
                        { File.Delete(OldFileName); }

                        OldFileName = FileDir + DateTime.Now.AddDays(-(KeepLogDaysCount + 2)).ToString("yyyyMMddHH") + ".log";

                        if (File.Exists(OldFileName))
                        { File.Delete(OldFileName); }
                        #endregion
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            catch { }
        }

        internal ParamObject ParamObject
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        internal DBFactory DBFactory
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }


    }
}
