using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;


namespace DataBaseAgent
{
    /// <summary>
    /// 功能：取得Internet上的URL页的源码
    /// 创建：2004-03-22
    /// 作者：Rexsp MSN:yubo@x263.net
    /// </summary>
    public class GetPageCode
    {
        #region 私有变量
        /// <summary>
        /// 网页URL地址
        /// </summary>
        private string url = null;
        /// <summary>
        /// 是否使用代码服务器：0 不使用 1 使用代理服务器
        /// </summary>
        private int proxyState = 0;
        /// <summary>
        /// 代理服务器地址
        /// </summary>
        private string proxyAddress = null;
        /// <summary>
        /// 代理服务器端口
        /// </summary>
        private string proxyPort = null;
        /// <summary>
        /// 代理服务器用户名
        /// </summary>
        private string proxyAccount = null;
        /// <summary>
        /// 代理服务器密码
        /// </summary>
        private string proxyPassword = null;
        /// <summary>
        /// 代理服务器域
        /// </summary>
        private string proxyDomain = null;
        /// <summary>
        /// 输出文件路径
        /// </summary>
        private string outFilePath = null;
        /// <summary>
        /// 输出的字符串
        /// </summary>
        private string outString = null;
        /// <summary>
        /// 提示信息
        /// </summary>
        private string noteMessage;

        #endregion

        #region 公共属性
        /// <summary>
        /// 欲读取的URL地址
        /// </summary>
        public string Url
        {
            get { return url; }
            set{ url =value;}
        }
        /// <summary>
        /// 是否使用代理服务器标志
        /// </summary>
        public int ProxyState
        {
            get { return proxyState; }
            set { proxyState = value; }
        }
        /// <summary>
        /// 代理服务器地址
        /// </summary>
        public string ProxyAddress
        {
            get { return proxyAddress; }
            set { proxyAddress = value; }
        }
        /// <summary>
        /// 代理服务器端口
        /// </summary>
        public string ProxyPort
        {
            get { return proxyPort; }
            set { proxyPort = value; }
        }
        /// <summary>
        /// 代理服务器账号
        /// </summary>
        public string ProxyAccount
        {
            get { return proxyAccount; }
            set { proxyAccount = value; }
        }
        /// <summary>
        /// 代理服务器密码
        /// </summary>
        public string ProxyPassword
        {
            get { return proxyPassword; }
            set { proxyPassword = value; }
        }
        /// <summary>
        /// 代理服务器域
        /// </summary>
        public string ProxyDomain
        {
            get { return proxyDomain; }
            set { proxyDomain = value; }
        }
        /// <summary>
        /// 输出文件路径
        /// </summary>
        public string OutFilePath
        {
            get { return outFilePath; }
            set { outFilePath = value; }
        }
        /// <summary>
        /// 返回的字符串
        /// </summary>
        public string OutString
        {
            get { return outString; }

        }
        /// <summary>
        /// 返回提示信息
        /// </summary>
        public string NoteMessage
        {
            get { return noteMessage; }

        }

        #endregion

        #region 构造函数
        public GetPageCode()
        {
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 读取指定URL地址，存到指定文件中
        /// </summary>
        public void GetSource(Encoding PageEncoding)
        {
            WebRequest request = WebRequest.Create(this.url);
            //使用代理服务器的处理
            if (this.proxyState == 1)
            {
                //默认读取80端口的数据
                if (this.proxyPort == null)
                    this.ProxyPort = "80";

                WebProxy myProxy = new WebProxy();
                myProxy = (WebProxy)request.Proxy;
                myProxy.Address = new Uri(this.ProxyAddress + ":" + this.ProxyPort);
                myProxy.Credentials = new NetworkCredential(this.proxyAccount, this.proxyPassword, this.ProxyDomain);
                request.Proxy = myProxy;
            }
            try
            {
                //请求服务
                WebResponse response = request.GetResponse();
                //返回信息
                Stream resStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(resStream, PageEncoding);
                string tempCode = sr.ReadToEnd();
                resStream.Close();
                sr.Close();

                //如果输出文件路径为空，便将得到的内容赋给OutString属性
                if (this.outFilePath == null)
                {
                    this.outString = tempCode;
                }
                else
                {

                    FileInfo fi = new FileInfo(this.outFilePath);
                    //如果存在文件则先干掉
                    if (fi.Exists)
                        fi.Delete();

                    StreamWriter sw = new StreamWriter(this.outFilePath, true, Encoding.Default);
                    sw.Write(tempCode);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                this.noteMessage = "出错了，请检查网络是否连通;";
            }


        }
        public void GetSource()
        {
            GetSource(Encoding.Default);
        }
        /// <summary>
        /// 读出给定的页面代码中的超链接
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <returns></returns>
        public ArrayList GetHyperLinks(string htmlCode)
        {
            ArrayList al = new ArrayList();
            string strRegex = @"<div Class=""tit(.*?)>(.*?)</div>";
            //http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?   （微软件提供的）
            //http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?
            //<a[^>]*href=("([^"]*)"|'([^']*)'|([^s>]*))[^>]*>(.*?)</a>
            //(<a )([^\s>]*)(<\/a>)
            //<a .*?>.*?</a>
            //<\\s*(\\S+)(\\s[^>]*)?>[\\s\\S]*<\\s*\\/\\1\\s*>
            Regex r = new Regex(strRegex, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(htmlCode);
            for (int i = 0; i <= m.Count - 1; i++)
            {
                bool rep = false;
                string strNew = m[i].ToString();
                // 过滤重复的URL
                foreach (string str in al)
                {
                    if (strNew == str)
                    {
                        rep = true;
                        break;
                    }
                }
                if (!rep) al.Add(strNew);
            }
            //al.Sort();
            return al;
        }

        /// <summary>
        /// 读出给定的页面代码中的超链接
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <returns></returns>
        public ArrayList GetItemsUseRegex(string Regex, string htmlCode)
        {
            ArrayList al = new ArrayList();
            if (Regex != null && htmlCode !=null)
            {
                
                string strRegex = Regex;
                Regex r = new Regex(strRegex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                
                MatchCollection m = r.Matches(htmlCode);
                for (int i = 0; i <= m.Count - 1; i++)
                {
                    bool rep = false;
                    string strNew = m[i].ToString();
                    // 过滤重复的URL
                    foreach (string str in al)
                    {
                        if (strNew == str)
                        {
                            rep = true;
                            break;
                        }
                    }
                    if (!rep) al.Add(strNew);
                }
                
            }
            //al.Sort();
            return al;
            
        }

        public void WriteToXml(string strURL, ArrayList alHyperLinks)
        {
            XmlTextWriter writer = new XmlTextWriter("HyperLinks.xml", Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(false);
            writer.WriteDocType("HyperLinks", null, "urls.dtd", null);
            writer.WriteComment("提取自" + strURL + "的超链接");
            writer.WriteStartElement("HyperLinks");
            writer.WriteStartElement("HyperLinks", null);
            writer.WriteAttributeString("DateTime", DateTime.Now.ToString());
            foreach (string str in alHyperLinks)
            {
                string title = GetDomain(str);
                string body = str;
                writer.WriteElementString(title, null, body);
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

        }

        // 获取网址的域名后缀
        private string GetDomain(string strURL)
        {
            string retVal;
            string strRegex = @"(\.com/|\.net/|\.cn/|\.org/|\.gov/)";
            Regex r = new Regex(strRegex, RegexOptions.IgnoreCase);
            Match m = r.Match(strURL);
            retVal = m.ToString();
            strRegex = @"\.|/$";
            retVal = Regex.Replace(retVal, strRegex, "").ToString();
            if (retVal == "") 
            {
                retVal = "other";
            }
            return retVal;
        }

        #endregion

    }


}
