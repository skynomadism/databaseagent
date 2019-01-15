using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

namespace DataBaseAgent
{
    class Conn
    {

        string strConnectionSql = "";
        private static string str = "";
        static string strConnectionOleDB = @"provider=microsoft.jet.oledb.4.0;data source=" + Application.StartupPath+"\\";

        public Conn() 
        {
            
        }
        /// <summary>
        /// 获得配置文件里的默认数据库
        /// SQLServer类型的
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetSqlConn()
        {
            //XmlManager Xml = new XmlManager();
            //Xml.LoadXml("Conn.xml");
            //strConnectionSql = "user id="
            //    +Xml.GetNodeValue("Root.UID")+";password="
            //    +Xml.GetNodeValue("Root.PAW")+";initial catalog="
            //    +Xml.GetNodeValue("Root.DataSoure")+";server="
            //    +Xml.GetNodeValue("Root.Server")+";connect timeout=30";
            //return new SqlConnection(strConnectionSql);
            strConnectionSql = System.Configuration.ConfigurationManager.ConnectionStrings["wechat"].ConnectionString;
            return new SqlConnection(strConnectionSql);
        }
        /// <summary>
        /// 获得配置文件里的默认数据库
        /// MDB类型的
        /// </summary>
        /// <returns></returns>
        public OleDbConnection GetOleDBConn() 
        {
             XmlManager Xml = new XmlManager();
            Xml.LoadXml("Conn.xml");
            str = Xml.GetNodeValue("Root.DataFile");
            
            return new OleDbConnection(strConnectionOleDB+str);
        }
        /// <summary>
        /// 获得配置文件里的指定代号数据库
        /// SQLServer类型的
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetSqlConn(int dbID) 
        {
            XmlManager Xml = new XmlManager();
            Xml.LoadXml("Conn.xml");
            strConnectionSql = "user id="
                + Xml.GetNodeValue("Root.DataBase"+dbID.ToString()+".UID") + ";password="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".PAW") + ";initial catalog="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".DataSoure") + ";server="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".Server") + ";connect timeout=30";
            return new SqlConnection(strConnectionSql);
        }
        /// <summary>
        /// 依据连接字符串得到一个SqlServer数据库的连接对像
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public SqlConnection GetSqlConn(string ConnectionString)
        {
            strConnectionSql = ConnectionString;
            return new SqlConnection(strConnectionSql);
        }
        /// <summary>
        /// 获得配置文件里的指定代号数据库
        /// MDB类型的
        /// </summary>
        /// <returns></returns>
        public OleDbConnection GetOleDBConn(int dbID) 
        {
            XmlManager Xml = new XmlManager();
            Xml.LoadXml("Conn.xml");
            str = Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".DataFile");

            return new OleDbConnection(strConnectionOleDB + str);
        }
        /// <summary>
        /// 获得配置文件里的指定名称的数据库
        /// MDB类型的
        /// strType如果是0表示只是传入了一个*.mdb文件的文件名，需要用默认的连接字符串进行构造连接字符串
        /// 如果不是0则表示传入的是一个连接字符串，不用再重新构造。
        /// </summary>
        /// <returns></returns>
        public OleDbConnection GetOleDBConn(string fileName,int StrType=0) 
        {
            return StrType == 0 ? new OleDbConnection(strConnectionOleDB + fileName) : new OleDbConnection(fileName);
        }

        //mySql连接
        public MySqlConnection GetMySqlConn()
        {
            String mysqlStr = System.Configuration.ConfigurationManager.ConnectionStrings["mySqlConnectionString"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mysqlStr);
            return mysql;
        }

        public MySqlConnection GetMySqlConn(int dbID) 
        {
            //String mysqlStr = "Database=gps;Data Source=192.168.1.245;User Id=root;Password=Diaoyudao;
            //pooling=false;CharSet=utf8;port=3306";
            XmlManager Xml = new XmlManager();
            Xml.LoadXml("Conn.xml");
            strConnectionSql = "User Id="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".UID") + ";Password="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".PAW") + ";Database="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".DataSoure") + ";Data Source="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".Server") + ";pooling=false;CharSet=utf8;port=3306";
            return new MySqlConnection(strConnectionSql);

        }

        public MySqlConnection GetMySqlConn(string ConnectString)
        {
            MySqlConnection mysql = new MySqlConnection(ConnectString);
            return mysql;
        }

        internal OracleConnection GetOracleConn()
        {
            String myconnStr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;
            OracleConnection oracle = new OracleConnection(myconnStr);
            return oracle;
        }

        internal OracleConnection GetOracleConn(int dbID)
        {
            //String mysqlStr = "Database=gps;Data Source=192.168.1.245;User Id=root;Password=Diaoyudao;
            //pooling=false;CharSet=utf8;port=3306";
            XmlManager Xml = new XmlManager();
            Xml.LoadXml("Conn.xml");
            strConnectionSql = "User Id="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".UID") + ";Password="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".PAW") + ";Database="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".DataSoure") + ";Data Source="
                + Xml.GetNodeValue("Root.DataBase" + dbID.ToString() + ".Server") + ";pooling=false;CharSet=utf8;port=3306";
            return new OracleConnection(strConnectionSql);
        }

        internal OracleConnection GetOracleConn(string ConnectString)
        {
            OracleConnection oracle = new OracleConnection(ConnectString);
            return oracle;
        }
    }
}
