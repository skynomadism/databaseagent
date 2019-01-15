using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

namespace DataBaseAgent
{
    /// <summary>
    /// 把指定的类型名转化成sqldbtype,或oledtype类型以便在程序中使用
    /// </summary>
    class TypeConvert
    {
        public TypeConvert() 
        {
        
        }
        public OleDbType ConvertOleDB(string TypeName) 
        {
            switch (TypeName) 
            {
                case SimpleType.BigInt:
                    return OleDbType.BigInt;
                case SimpleType.Binary:
                    return OleDbType.Binary ;
                case SimpleType.Bool:
                    return OleDbType.Boolean;
                case SimpleType.Char:
                    return OleDbType.Char;
                case SimpleType.Time:
                    return OleDbType.Date;
                case SimpleType.Float:
                    return OleDbType.Double;
                case SimpleType.GUID:
                    return OleDbType.Guid;
                case SimpleType.Int:
                    return OleDbType.Integer;
                case SimpleType.Image:
                    return OleDbType.LongVarBinary;
                case SimpleType.Text:
                    return OleDbType.LongVarChar;
                case SimpleType.NText:
                    return OleDbType.LongVarWChar;
                case SimpleType.VarBinary:
                    return OleDbType.VarBinary;
                case SimpleType.Str:
                    return OleDbType.VarChar;
                default:
                    return OleDbType.VarChar;
            #region 不是常用的类型
                //case "":
                //    return OleDbType.Currency;
                //case "":
                //    return OleDbType.IDispatch;
                //case "":
                //    return OleDbType.BSTR;
                //case "":
                //    return OleDbType.DBDate;
                //case "":
                //    return OleDbType.DBTime;
                //case "":
                //    return OleDbType.DBTimeStamp;
                //case "":
                //    return OleDbType.Decimal;
                //case "":
                //    return OleDbType.Empty;
                //case "":
                //    return OleDbType.Error;
                //case "":
                //    return OleDbType.Filetime;
                //case "":
                //    return OleDbType.IUnknown;
                //case "":
                //    return OleDbType.Numeric;
                //case "":
                //    return OleDbType.PropVariant;
                //case "":
                //    return OleDbType.Single;
                //case "":
                //    return OleDbType.SmallInt;
                //case "":
                //    return OleDbType.TinyInt;
                //case "":
                //    return OleDbType.UnsignedBigInt;
                //case "":
                //    return OleDbType.UnsignedInt;
                //case "":
                //    return OleDbType.UnsignedSmallInt;
                //case "":
                //    return OleDbType.UnsignedTinyInt;
                //case "":
                //    return OleDbType.Variant;
                //case "":
                //    return OleDbType.VarNumeric;
                //case "":
                //    return OleDbType.VarWChar;
                //case "":
                //    return OleDbType.WChar;
            #endregion
            }
        }

        public SqlDbType ConvertSqlDB(string TypeName)
        {
            switch (TypeName)
            {
                case SimpleType.BigInt:
                    return SqlDbType.BigInt;
                case SimpleType.Binary:
                    return SqlDbType.Binary;
                case SimpleType.Image:
                    return SqlDbType.Image;
                case SimpleType.Bool:
                    return SqlDbType.Bit;
                case SimpleType.Char:
                    return SqlDbType.Char;
                case SimpleType.Time:
                    return SqlDbType.DateTime;
                case SimpleType.Float:
                    return SqlDbType.Float;
                case SimpleType.NText:
                    return SqlDbType.NText;
                case SimpleType.Int:
                    return SqlDbType.Int;
                case SimpleType.Text:
                    return SqlDbType.Text;
                case SimpleType.GUID:
                    return SqlDbType.UniqueIdentifier;
                case SimpleType.VarBinary:
                    return SqlDbType.VarBinary;
                case SimpleType.Str:
                    return SqlDbType.VarChar;
                default:
                    return SqlDbType.VarChar;

                #region　不常用的类型
                //case "":
                //    return SqlDbType.Decimal;
               
                //case "":
                //    return SqlDbType.Money;
                //case "":
                //    return SqlDbType.NChar;
                //case "":
                //    return SqlDbType.NVarChar;
                //case "":
                //    return SqlDbType.Real;
                //case "":
                //    return SqlDbType.SmallDateTime;
                //case "":
                //    return SqlDbType.SmallInt;
                //case "":
                //    return SqlDbType.SmallMoney;
                //case "":
                //    return SqlDbType.Timestamp;
                //case "":
                //    return SqlDbType.TinyInt;
                //case "":
                //    return SqlDbType.Udt;
                //case "":
                //    return SqlDbType.Variant;
                //case "":
                //    return SqlDbType.Xml;
                #endregion
            }
        }

        public MySqlDbType ConvertMySqlDB(string TypeName)
        {
            switch (TypeName)
            {
                case SimpleType.BigInt:
                    return MySqlDbType.Int64;
                case SimpleType.Binary:
                    return MySqlDbType.Binary;
                case SimpleType.Image:
                    return MySqlDbType.Blob;
                case SimpleType.Bool:
                    return MySqlDbType.Bit;
                case SimpleType.Char:
                    return MySqlDbType.VarChar;
                case SimpleType.Time:
                    return MySqlDbType.DateTime;
                case SimpleType.Float:
                    return MySqlDbType.Float;
                case SimpleType.NText:
                    return MySqlDbType.LongText;
                case SimpleType.Int:
                    return MySqlDbType.Int32;
                case SimpleType.Text:
                    return MySqlDbType.Text;
                case SimpleType.GUID:
                    return MySqlDbType.Guid;
                case SimpleType.VarBinary:
                    return MySqlDbType.VarBinary;
                case SimpleType.Str:
                    return MySqlDbType.VarString;
                default:
                    return MySqlDbType.VarChar;

                #region　不常用的类型
                //case "":
                //    return MySqlDbType.Decimal;

                //case "":
                //    return MySqlDbType.JSON;
                //case "":
                //    return MySqlDbType.Byte;
                //case "":
                //    return MySqlDbType.Date;
                //case "":
                //    return MySqlDbType.Double;
                //case "":
                //    return MySqlDbType.Enum;
                //case "":
                //    return MySqlDbType.Set;
                //case "":
                //    return MySqlDbType.SmallMoney;
                //case "":
                //    return MySqlDbType.Timestamp;
                //case "":
                //    return MySqlDbType.TinyInt;
                //case "":
                //    return MySqlDbType.Udt;
                //case "":
                //    return MySqlDbType.Variant;
                //case "":
                //    return MySqlDbType.Xml;
                #endregion
            }
        }

        internal OracleType ConvertOracleDB(string TypeName)
        {
            switch (TypeName)
            {
                case SimpleType.BigInt:
                    return OracleType.Number;
                case SimpleType.Binary:
                    return OracleType.Blob;
                case SimpleType.Image:
                    return OracleType.Blob;
                case SimpleType.Bool:
                    return OracleType.NVarChar;
                case SimpleType.Char:
                    return OracleType.VarChar;
                case SimpleType.Time:
                    return OracleType.DateTime;
                case SimpleType.Float:
                    return OracleType.Float;
                case SimpleType.NText:
                    return OracleType.NClob;
                case SimpleType.Int:
                    return OracleType.Int32;
                case SimpleType.Text:
                    return OracleType.Clob;
                case SimpleType.GUID:
                    return OracleType.VarChar;
                case SimpleType.VarBinary:
                    return OracleType.Blob;
                case SimpleType.Str:
                    return OracleType.NVarChar;
                default:
                    return OracleType.NVarChar;

                #region　不常用的类型
                //case "":
                //    return OracleType.Decimal;

                //case "":
                //    return OracleType.JSON;
                //case "":
                //    return OracleType.Byte;
                //case "":
                //    return OracleType.Date;
                //case "":
                //    return OracleType.Double;
                //case "":
                //    return OracleType.Enum;
                //case "":
                //    return OracleType.Set;
                //case "":
                //    return OracleType.SmallMoney;
                //case "":
                //    return OracleType.Timestamp;
                //case "":
                //    return OracleType.TinyInt;
                //case "":
                //    return OracleType.Udt;
                //case "":
                //    return OracleType.Variant;
                //case "":
                //    return MySqlDbType.Xml;
                #endregion
            }
        }
    }

}
