using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;
namespace Com.Zfrong.Common.Xml
{
   
    /// <summary>
    /// ������XPATH���ʽ����ȡ��Ӧ�ڵ�
    /// ����xpath���Բμ�:
    /// </summary>
    public class MyXml
    {
        /**/
        /// <summary>
        /// xml�ļ�����·������
        /// </summary>
        /// <remarks>xml�ļ�����·������</remarks>
        public enum PathType : byte
        {
            /**/
            /// <summary>
            /// ����·��
            /// </summary>
            AbsolutePath,
            /**/
            /// <summary>
            /// ����·��
            /// </summary>
            VirtualPath
        }
        #region ����
        private string xmlFilePath;
        private PathType xmlFilePathType;
        public XmlDocument XmlDocument = new XmlDocument();
        #endregion

        #region ����
        /**/
        /// <summary>
        /// �ļ�·��
        /// </summary>
        /// <remarks>�ļ�·��</remarks>
        public string XmlFilePath
        {
            get
            {
                return this.xmlFilePath;
            }
            set
            {
                xmlFilePath = value;

            }
        }

        /**/
        /// <summary>
        /// �ļ�·������
        /// </summary>
        public PathType XmlFilePathTyp
        {
            set
            {
                xmlFilePathType = value;
            }
        }
        #endregion

        #region ���캯��
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempXmlFilePath"></param>
        public MyXml(string tempXmlFilePath)
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //

            this.xmlFilePathType = PathType.VirtualPath;
            this.xmlFilePath = tempXmlFilePath;
            GetXmlDocument();
            //XmlDocument.Load( xmlFilePath ) ;
        }

        /**/
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="tempXmlFilePath">�ļ�·��</param>
        /// <param name="tempXmlFilePathType">����</param>
        public MyXml(string tempXmlFilePath, PathType tempXmlFilePathType)
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            this.xmlFilePathType = tempXmlFilePathType;
            this.xmlFilePath = tempXmlFilePath;
            GetXmlDocument();
        }
        #endregion


        /**/
        ///<summary>
        ///��ȡXmlDocumentʵ����
        ///</summary>    
        /// <returns>ָ����XML�����ļ���һ��xmldocumentʵ��</returns>
        private XmlDocument GetXmlDocument()
        {
            XmlDocument doc = null;

            if (this.xmlFilePathType == PathType.AbsolutePath)
            {
                doc = GetXmlDocumentFromFile(xmlFilePath);
            }
            else if (this.xmlFilePathType == PathType.VirtualPath)
            {
                //doc = GetXmlDocumentFromFile(a HttpContext.Current.Server.MapPath(xmlFilePath));
                doc = GetXmlDocumentFromFile(Environment.CurrentDirectory + "\\" + xmlFilePath);//
            }
            return doc;
        }

        private XmlDocument GetXmlDocumentFromFile(string tempXmlFilePath)
        {
            if (!System.IO.File.Exists(tempXmlFilePath))
            {
                if(!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(tempXmlFilePath)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(tempXmlFilePath));//
                XmlDocument.AppendChild(XmlDocument.CreateXmlDeclaration("1.0", "utf-8", null));
                XmlDocument.AppendChild(XmlDocument.CreateElement("root"));//
                XmlDocument.Save(tempXmlFilePath);//
            }
            string xmlFileFullPath = tempXmlFilePath;
            XmlDocument.Load(xmlFileFullPath);
            //�����¼�����
            XmlDocument.NodeChanged += new XmlNodeChangedEventHandler(this.nodeUpdateEvent);
            XmlDocument.NodeInserted += new XmlNodeChangedEventHandler(this.nodeInsertEvent);
            XmlDocument.NodeRemoved += new XmlNodeChangedEventHandler(this.nodeDeleteEvent);
            return XmlDocument;
        }

        #region ��ȡָ���ڵ��ָ������ֵ
        /**/
        /// <summary>
        /// ����:
        /// ��ȡָ���ڵ��ָ������ֵ    
        /// </summary>
        /// <param name="strNode">�ڵ�����</param>
        /// <param name="strAttribute">�˽ڵ������</param>
        /// <returns></returns>
        public string GetXmlNodeAttributeValue(string strNode, string strAttribute)
        {
            string strReturn = "";
            try
            {
                //����ָ��·����ȡ�ڵ�
                XmlNode xmlNode = XmlDocument.SelectSingleNode(strNode);
                if (!(xmlNode == null))
                {//��ȡ�ڵ�����ԣ���ѭ��ȡ����Ҫ������ֵ
                    XmlAttributeCollection xmlAttr = xmlNode.Attributes;

                    for (int i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name == strAttribute)
                        {
                            strReturn = xmlAttr.Item(i).Value;
                            break;
                        }
                    }
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }
        #endregion


        #region ��ȡָ���ڵ��ֵ
        /**/
        /// <summary>
        /// ����:
        /// ��ȡָ���ڵ��ֵ    
        /// </summary>
        /// <param name="strNode">�ڵ�����</param>
        /// <returns></returns>
        public string GetXmlNodeValue(string strNode)
        {
            string strReturn = String.Empty;

            try
            {
                //����·����ȡ�ڵ�
                XmlNode xmlNode = XmlDocument.SelectSingleNode(strNode);
                if (!(xmlNode == null))
                    strReturn = xmlNode.InnerText;
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }
        #endregion

        #region ���ýڵ�ֵ
        /**/
        /// <summary>
        /// ����:
        /// ���ýڵ�ֵ        
        /// </summary>
        /// <param name="strNode">�ڵ������</param>
        /// <param name="newValue">�ڵ�ֵ</param>
        public void SetXmlNodeValue(string xmlNodePath, string xmlNodeValue)
        {
            try
            {
                //��������Ϊ���������Ľڵ���и�ֵ
                XmlNodeList xmlNode = this.XmlDocument.SelectNodes(xmlNodePath);
                if (!(xmlNode == null))
                {
                    foreach (XmlNode xn in xmlNode)
                    {
                        xn.InnerText = xmlNodeValue;
                    }
                }
                /**/
                /*
             * ����ָ��·����ȡ�ڵ�
            XmlNode xmlNode = XmlDocument.SelectSingleNode(xmlNodePath) ;            
            //���ýڵ�ֵ
            if (!(xmlNode==null))
                xmlNode.InnerText = xmlNodeValue ;*/
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        #endregion

        #region ���ýڵ������ֵ
        /**/
        /// <summary>
        /// ����:
        /// ���ýڵ������ֵ    
        /// </summary>
        /// <param name="xmlNodePath">�ڵ�����</param>
        /// <param name="xmlNodeAttribute">��������</param>
        /// <param name="xmlNodeAttributeValue">����ֵ</param>
        public void SetXmlNodeAttributeValue(string xmlNodePath, string xmlNodeAttribute, string xmlNodeAttributeValue)
        {
            try
            {
                //��������Ϊ���������Ľڵ�����Ը�ֵ
                XmlNodeList xmlNode = this.XmlDocument.SelectNodes(xmlNodePath);
                if (!(xmlNode == null))
                {
                    foreach (XmlNode xn in xmlNode)
                    {
                        XmlAttributeCollection xmlAttr = xn.Attributes;
                        for (int i = 0; i < xmlAttr.Count; i++)
                        {
                            if (xmlAttr.Item(i).Name == xmlNodeAttribute)
                            {
                                xmlAttr.Item(i).Value = xmlNodeAttributeValue;
                                break;
                            }
                        }
                    }
                }
                /**/
                /*�����ڵ�
            //����ָ��·����ȡ�ڵ�
            XmlNode xmlNode = XmlDocument.SelectSingleNode(xmlNodePath) ;
            if (!(xmlNode==null))
            {//��ȡ�ڵ�����ԣ���ѭ��ȡ����Ҫ������ֵ
                XmlAttributeCollection xmlAttr = xmlNode.Attributes ;
                for(int i= ; i<xmlAttr.Count ; i++)
                {
                    if ( xmlAttr.Item(i).Name == xmlNodeAttribute )
                    {
                        xmlAttr.Item(i).Value = xmlNodeAttributeValue;
                        break ;
                    }
                }    
            }
            */
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        #endregion

        #region ���
        /**/
        /// <summary>
        /// ��ȡXML�ļ��ĸ�Ԫ��
        /// </summary>
        public XmlNode GetXmlRoot()
        {
            return XmlDocument.DocumentElement;
        }

        /**/
        /// <summary>
        /// �ڸ��ڵ�����Ӹ��ڵ�
        /// </summary>
        public void AddParentNode(string parentNode)
        {
            try
            {
                XmlNode root = GetXmlRoot();
                XmlNode parentXmlNode = XmlDocument.CreateElement(parentNode);
                root.AppendChild(parentXmlNode);
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        /**/
        /// <summary>
        /// ��һ���Ѿ����ڵĸ��ڵ��в���һ���ӽڵ�
        /// </summary>
        /// <param name="parentNodePath">���ڵ�</param>
        /// <param name="childNodePath">�ֽڵ�����</param>
        public void AddChildNode(string parentNodePath, string childnodename)
        {
            try
            {
                XmlNode parentXmlNode = XmlDocument.SelectSingleNode(parentNodePath);
                if (!((parentXmlNode) == null))//����˽ڵ����
                {
                    XmlNode childXmlNode = XmlDocument.CreateElement(childnodename);
                    parentXmlNode.AppendChild(childXmlNode);
                }
                else
                {//��������ھͷŸ��ڵ����
                    //this.GetXmlRoot().AppendChild(childXmlNode);
                }

            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        /**/
        /// <summary>
        /// ��һ���ڵ��������
        /// </summary>
        /// <param name="NodePath">�ڵ�·��</param>
        /// <param name="NodeAttribute">������</param>
        public void AddAttribute(string NodePath, string NodeAttribute)
        {
            privateAddAttribute(NodePath, NodeAttribute, "");
        }
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NodePath"></param>
        /// <param name="NodeAttribute"></param>
        /// <param name="NodeAttributeValue"></param>
        private void privateAddAttribute(string NodePath, string NodeAttribute, string NodeAttributeValue)
        {
            try
            {
                XmlNode nodePath = XmlDocument.SelectSingleNode(NodePath);
                if (!(nodePath == null))
                {
                    XmlAttribute nodeAttribute = this.XmlDocument.CreateAttribute(NodeAttribute);
                    nodeAttribute.Value = NodeAttributeValue;
                    nodePath.Attributes.Append(nodeAttribute);
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        /**/
        /// <summary>
        ///  ��һ���ڵ��������,����ֵ
        /// </summary>
        /// <param name="NodePath">�ڵ�</param>
        /// <param name="NodeAttribute">������</param>
        /// <param name="NodeAttributeValue">����ֵ</param>
        public void AddAttribute(string NodePath, string NodeAttribute, string NodeAttributeValue)
        {
            privateAddAttribute(NodePath, NodeAttribute, NodeAttributeValue);
        }
        #endregion

        #region ɾ��
        /**/
        /// <summary>
        /// ɾ���ڵ��һ������
        /// </summary>
        /// <param name="NodePath">�ڵ����ڵ�xpath���ʽ</param>
        /// <param name="NodeAttribute">������</param>
        public void DeleteAttribute(string NodePath, string NodeAttribute)
        {
            XmlNodeList nodePath = this.XmlDocument.SelectNodes(NodePath);
            if (!(nodePath == null))
            {
                foreach (XmlNode tempxn in nodePath)
                {
                    XmlAttributeCollection xmlAttr = tempxn.Attributes;
                    for (int i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name == NodeAttribute)
                        {
                            tempxn.Attributes.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        /**/
        /// <summary>
        /// ɾ���ڵ�,��������ֵ���ڸ�����ֵʱ
        /// </summary>
        /// <param name="NodePath">�ڵ����ڵ�xpath���ʽ</param>
        /// <param name="NodeAttribute">����</param>
        /// <param name="NodeAttributeValue">ֵ</param>
        public void DeleteAttribute(string NodePath, string NodeAttribute, string NodeAttributeValue)
        {
            XmlNodeList nodePath = this.XmlDocument.SelectNodes(NodePath);
            if (!(nodePath == null))
            {
                foreach (XmlNode tempxn in nodePath)
                {
                    XmlAttributeCollection xmlAttr = tempxn.Attributes;
                    for (int i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name == NodeAttribute && xmlAttr.Item(i).Value == NodeAttributeValue)
                        {
                            tempxn.Attributes.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
        /**/
        /// <summary>
        /// ɾ���ڵ�
        /// </summary>
        /// <param name="tempXmlNode"></param>
        /// <remarks></remarks>
        public void DeleteXmlNode(string tempXmlNode)
        {
            XmlNodeList nodePath = this.XmlDocument.SelectNodes(tempXmlNode);
            if (!(nodePath == null))
            {
                foreach (XmlNode xn in nodePath)
                {
                    xn.ParentNode.RemoveChild(xn);
                }
            }
        }

        #endregion

        #region XML�ĵ��¼�
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        private void nodeInsertEvent(Object src, XmlNodeChangedEventArgs args)
        {
            //��������
            SaveXmlDocument();
        }
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        private void nodeDeleteEvent(Object src, XmlNodeChangedEventArgs args)
        {
            //��������
            SaveXmlDocument();
        }
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        private void nodeUpdateEvent(Object src, XmlNodeChangedEventArgs args)
        {
            //��������
            SaveXmlDocument();
        }
        #endregion

        #region ����XML�ļ�
        /**/
        /// <summary>
        /// ����: 
        /// ����XML�ļ�
        /// 
        /// </summary>
        public void SaveXmlDocument()
        {
            try
            {
                //�������õĽ��
                if (this.xmlFilePathType == PathType.AbsolutePath)
                {
                    Savexml(xmlFilePath);
                }
                else if (this.xmlFilePathType == PathType.VirtualPath)
                {
                    //Savexml(HttpContext.Current.Server.MapPath(xmlFilePath));
                    Savexml(Environment.CurrentDirectory + "\\" + xmlFilePath);//
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }

        /**/
        /// <summary>
        /// ����: 
        /// ����XML�ļ�    
        /// </summary>
        public void SaveXmlDocument(string tempXMLFilePath)
        {
            try
            {
                //�������õĽ��
                Savexml(tempXMLFilePath);
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        private void Savexml(string filepath)
        {
            XmlDocument.Save(filepath);
        }

        #endregion

    }
}
