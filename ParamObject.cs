using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseAgent
{
    public class ParamObject
    {
        string _ParamName;
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName
        {
            get { return _ParamName; }
            set { _ParamName = value; }
        }
        string _SimpleType;
        /// <summary>
        /// 参数类型
        /// </summary>
        public string SimpleType
        {
            get { return _SimpleType; }
            set { _SimpleType = value; }
        }
        int _ParamSize;
        /// <summary>
        /// 参数长度
        /// </summary>
        public int ParamSize
        {
            get { return _ParamSize; }
            set { _ParamSize = value; }
        }
        string _CloumName;
        /// <summary>
        /// 参数对应的列名
        /// </summary>
        public string CloumName
        {
            get { return _CloumName; }
            set { _CloumName = value; }
        }
        string _OperaterSign;
        /// <summary>
        /// 操作标记:Output,Input
        /// </summary>
        public string OperaterSign
        {
            get { return _OperaterSign; }
            set { _OperaterSign = value; }
        }
        object _ParamValue;
        /// <summary>
        /// 参数值
        /// </summary>
        public object ParamValue
        {
            get { return _ParamValue; }
            set { _ParamValue = value; }
        }
       

        public ParamObject(string paramname, object paramvalue) :this(paramname,DataBaseAgent.SimpleType.Str,paramvalue)
        {
            
        }
        public ParamObject(string paramname,string simpletype, object paramvalue):this(paramname,paramvalue,"")
        {
            this._SimpleType = simpletype;
            switch (this._SimpleType) 
            {
                case DataBaseAgent.SimpleType.Int:
                    this._ParamSize = 4;
                    break;
                case DataBaseAgent.SimpleType.Str:
                    this._ParamSize = 250;
                    break;
                default :
                    this._ParamSize = 0;
                    break;
            }
        }
        public ParamObject(string paramname, object paramvalue,string operatersign)
            : this(paramname, DataBaseAgent.SimpleType.Int, 4, paramvalue, operatersign)
        {
            if (operatersign == "") 
            {
                this._SimpleType = DataBaseAgent.SimpleType.Str;
                this._ParamSize = 250;
            }
        }
        public ParamObject(string paramname,string simpletype,int paramsize,object paramvalue)
            :this(paramname,simpletype,paramsize,paramvalue,"")
        {

        }
        public ParamObject(string paramname, string simpletype, int paramsize, object paramvalue ,string operatersign)
            :this(paramname,simpletype,paramsize,paramname,operatersign,paramvalue)
        {

        }
        public ParamObject(string paramname, string simpletype, int paramsize)
            : this(paramname, simpletype, paramsize, paramname, "")
        {

        }
        public ParamObject(string paramname, string simpletype, int paramsize, string operatersign)
            : this(paramname, simpletype, paramsize, paramname, operatersign, null)
        {

        }
        public ParamObject(string paramname, string simpletype, int paramsize,string cloumname,string operatersign, object paramvalue)
        {
            this._CloumName = cloumname.Replace("@","");
            this._OperaterSign = operatersign;
            this._ParamName = paramname;
            this._ParamSize = paramsize;
            this._ParamValue = paramvalue;
            this._SimpleType = simpletype;
        }

    }
}
