using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// Xml序列化
    /// </summary>
    public class XmlSerializor
    {
        /// <summary>
        /// 对象序列化成 XML String
        /// </summary>
        public static string Serialize<T>(T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                var xmlString = Encoding.UTF8.GetString(ms.ToArray());
                return xmlString;
            }
        }

        /// <summary>
        /// XML String 反序列化成对象
        /// <remarks>
        /// Note:Xml序列化时是大小区分的
        /// </remarks>
        /// </summary>
        public static T Deserialize<T>(string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            using (var xmlReader = XmlReader.Create(xmlStream))
            {
                var obj = xmlSerializer.Deserialize(xmlReader);
                return (T)obj;
            }
        }
    }
}
