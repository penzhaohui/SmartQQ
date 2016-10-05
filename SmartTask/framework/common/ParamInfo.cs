using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 资源的参数的键值对
    /// </summary>
    [Serializable]
    public struct ParamInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ParamInfo(string key, string value)
            : this(key, value, string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        public ParamInfo(string key, string value, string text)
        {
            _key = key;
            _value = value;
            _text = text;
        }

        private string _key;
        /// <summary>
        /// 键名
        /// </summary>
        [XmlAttribute(AttributeName = "key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _value;
        /// <summary>
        /// 值内容
        /// Note:Text的内容会覆盖value原来的值
        /// </summary>
        [XmlAttribute(AttributeName = "value")]
        public string Value
        {
            get { return _text != null && !string.IsNullOrEmpty(_text.Trim()) ? _text : _value; }
            set { _value = value; }
        }

        private string _text;
        /// <summary>
        /// 有特殊字符的大块文本
        /// </summary>
        [XmlElement("text")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

    }
}
