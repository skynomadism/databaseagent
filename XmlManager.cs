using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;


namespace DataBaseAgent
{
    /// <summary>
    /// 自定义类，用于管理xml文件的创建，读写
    /// skynomadism 2008-4-12
    /// nomad21st@163.com
    /// </summary>
    public class XmlManager
    {
        XmlDocument _XmlDocument = new XmlDocument();
        XmlNode _Xmlnode;
        XmlElement _Xmlelem;
        public string _FilePath;
        public XmlManager() 
        {
        }
        /// <summary>
        /// 加载一个xml文件
        /// </summary>
        /// <param name="FileName"></param>
        public void LoadXml(string FileName) 
        {
            _FilePath = Application.StartupPath + @"\" + FileName;
            

            if (File.Exists(_FilePath))
            {
                _XmlDocument.Load(_FilePath);
            }
            else 
            {
                CreateXmlFile(_FilePath,true);
            }
        }
        /// <summary>
        /// 创建指定名称的xml文件
        /// </summary>
        /// <param name="FileName"></param>
        public void CreateXmlFile(string FileName)
        {
            CreateXmlFile(FileName, false);
        }
        /// <summary>
        /// 创建指定名的xml文件
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="IsFullPath">指定的文件名是否为全路径名，默认为当前目录的相对路径</param>
        public void CreateXmlFile(string FileName,bool IsFullPath) 
        {
            if (IsFullPath)
            {
                _FilePath = FileName;
            }
            else
            {
                _FilePath = Application.StartupPath + @"\" + FileName;
           
            }

            if (!File.Exists(_FilePath))
            {
                //XmlTextWriter writer = new XmlTextWriter(filePath, null);
                //writer.WriteComment("RemaidSettion");
                //writer.WriteStartElement("root");
                //writer.WriteEndElement();
                //writer.Flush();
                //writer.Close();

                //加入XML的声明段落
                _Xmlnode = _XmlDocument.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                _XmlDocument.AppendChild(_Xmlnode);
                //加入一个根元素
                _Xmlelem = _XmlDocument.CreateElement("", "Root", "");
                _XmlDocument.AppendChild(_Xmlelem);
                //保存创建好的XML文档
                try
                {
                    _XmlDocument.Save(_FilePath);
                }
                catch (Exception e)
                {
                    //显示错误信息
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }

            }

        }
        /// <summary>
        /// 插入或创建指定节点路径上的节点
        /// </summary>
        /// <param name="NodePath">以点分的方式指出节点全路径</param>
        /// <returns></returns>
        public XmlNode InsertXmlNode(string NodePath)
        {
            ArrayList NodeList = new GetPageCode().GetItemsUseRegex(@"[A-Za-z0-9]+", NodePath);
            XmlNode TemNode1;
            XmlNode TemNode2 = (XmlNode)_XmlDocument;
            if (NodeList != null)
            {

                for (int i = 0; i < NodeList.Count; i++)
                {
                    TemNode1 = TemNode2.SelectSingleNode(NodeList[i].ToString());
                    if (TemNode1 != null)
                    {
                        TemNode2 = TemNode1;
                        TemNode1 = null;
                    }
                    else
                    {
                        TemNode1 = _XmlDocument.CreateElement(NodeList[i].ToString());
                        TemNode2.AppendChild(TemNode1);
                        TemNode2 = TemNode2.SelectSingleNode(NodeList[i].ToString());
                        TemNode1 = null;
                    }
                }
            }
            return TemNode2;
        }
        /// <summary>
        /// 插入或创建指定节点路径上的节点并赋值
        /// </summary>
        /// <param name="NodePath">以点分的方式指出节点全路径</param>
        /// <param name="ValueName"></param>
        /// <param name="mValue"></param>
        /// <returns></returns>
        public XmlNode InsertXmlNode(string NodePath, string ValueName, string mValue)
        {
            XmlNode name = _XmlDocument.CreateElement(ValueName);
            name.InnerText = mValue;
            InsertXmlNode(NodePath).AppendChild(name);
            return SelectNode(NodePath + "." + ValueName);
        }
        /// <summary>
        /// 选中指定节点路径的节点
        /// </summary>
        /// <param name="NodePath"></param>
        /// <returns></returns>
        public XmlNode SelectNode(string NodePath) 
        {
            ArrayList NodeList = new GetPageCode().GetItemsUseRegex(@"[A-Za-z0-9]+", NodePath);
            XmlNode TemNode1;
            XmlNode TemNode2 = (XmlNode)_XmlDocument;
            if (NodeList != null)
            {

                for (int i = 0; i < NodeList.Count;i++ )
                {
                    TemNode1 = TemNode2.SelectSingleNode(NodeList[i].ToString());
                    if (TemNode1 != null)
                    {
                        TemNode2 = TemNode1;
                        TemNode1 = null;
                    }
                    else
                    {
                        throw new Exception("指定的节点(" + NodeList[i].ToString() + ")不存在，请确认你的节点路径是否正确。");
                    }
                }
            }
            return TemNode2;
        }
        /// <summary>
        /// 获取指定节点的值
        /// </summary>
        /// <param name="NodePath"></param>
        /// <returns></returns>
        public string GetNodeValue(string NodePath) 
        {
            XmlNode node = SelectNode(NodePath);
            return node.InnerText;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="NodePath"></param>
        /// <returns></returns>
        public bool DeleteNode(string NodePath) 
        {
            return false;
        }
        /// <summary>
        /// 保存当前xml文件
        /// </summary>
        public void SaveXml() 
        {

            try
            {
                _XmlDocument.Save(_FilePath);
            }
            catch (Exception e)
            {
                //显示错误信息
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }

    }
}
