using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Qj.Utility.Helper
{
    public class XmlHelper
    {
        public static string Serialize(object obj)
        {
            var settings = new XmlWriterSettings
            {
                Indent = false,
                //IndentChars = "",
                NewLineChars = "",
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true // 不生成声明头
            };

            var output = new StringBuilder();

            using (var writer = XmlWriter.Create(output, settings))

            {
                var xml = new XmlSerializer(obj.GetType());

                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                xml.Serialize(writer, obj, ns);
            }

            return output.ToString();
        }

        public static TObject Deserialize<TObject>(string xmlStr)
        {
            using (StringReader sr = new StringReader(xmlStr))
            {
                try
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(TObject));
                    return (TObject)xmldes.Deserialize(sr);
                }
                catch (Exception ex)
                {
                    return default(TObject);
                }
            }
        }
    }
}