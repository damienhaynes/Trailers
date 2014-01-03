using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Trailers.Extensions
{
    public static class XmlExtensions
    {
        /// <summary>
        /// Deserialize from XML
        /// </summary>
        public static T FromXML<T>(this string xml)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (StringReader reader = new StringReader(xml))
                {
                    T ser = (T)serializer.Deserialize(reader);
                    reader.Close();
                    return ser;
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static string ToXML<T>(this T data)
        {
            if (data == null) return null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, data);
                    }
                    return textWriter.ToString();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
