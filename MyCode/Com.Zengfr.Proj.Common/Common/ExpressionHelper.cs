using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using ExpressionBuilder;
// using ServiceStack.ServiceModel.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;
namespace Com.Zengfr.Proj.Common
{
    public class ExpressionHelper
    {
        static ExpressionHelper()
        {

        }
        public static string ToXml(Expression expression)
        {
            var mutableLambda = EditableExpression.CreateEditableExpression(expression);

            string xml = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = Encoding.UTF8;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                using (XmlWriter xdw = XmlWriter.Create(ms, settings))
                {
                    XmlSerializer xs = new XmlSerializer(mutableLambda.GetType());
                    xs.Serialize(xdw, mutableLambda, ns);
                }
                xml = settings.Encoding.GetString(ms.ToArray());
            }
            return xml;
        }
        public static Expression FromXmlToExpression(string xml)
        {
            EditableExpression editableExpression = null;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                using (XmlReader reader = XmlReader.Create(ms))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(EditableLambdaExpression));
                    editableExpression = xs.Deserialize(reader) as EditableExpression;
                }
            }
            return editableExpression.ToExpression();
        }
        public static Expression<T> FromXmlToExpression<T>(string xml)
        {
            return FromXmlToExpression(xml) as Expression<T>;
        }
    }
}
