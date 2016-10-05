using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 配置文件基类
    /// </summary>
    public class ConfigBase
    {
        /// <summary>
        /// 文件名，要重写
        /// </summary>
        protected virtual string FileName
        {
            get
            {
                return "TaskConfig.xml";
            }
        }

        /// <summary>
        /// 保存运行状态
        /// </summary>
        public virtual void Save()
        {
            using (var xmlWriter = new XmlTextWriter(GetFullPath(FileName), Encoding.UTF8))
            {
                var serializer = new XmlSerializer(GetType());
                xmlWriter.Formatting = Formatting.Indented;                
                serializer.Serialize(xmlWriter, this);
                xmlWriter.Close();
            }
        }

        /// <summary>
        /// 配置内容转换为Xml格式的字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ToXml()
        {
            return XmlSerializor.Serialize(this);
        }

        /// <summary>
        /// 文件全路径
        /// </summary>
        protected static string GetFullPath(string fileName)
        {
            //载入配置
            var tmp = ConfigurationManager.AppSettings["TaskConfigFile"];
            var fullPath = (string.IsNullOrEmpty(tmp)) ? AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName : tmp;
            return fullPath;
        }
    }

}
