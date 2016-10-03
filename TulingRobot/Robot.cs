using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TulingRobot
{
    internal class Robot : IRobot
    {
        // 在图灵机器官网上注册 http://www.tuling123.com/
        // 测试帐号
        // API URL: http://www.tuling123.com/openapi/api
        // API Key: a4cbd07df23e40b08833bf92765e5dbc
        // Secret: fc141e01ec7bb594
        private readonly string _APIUrl = "";
        private readonly string _APIKey = "";
        private readonly string _BaseUrl = "";
        private DateTime _lastTalkTime = DateTime.Now;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="apiKey"></param>
        internal Robot(string apiUrl, string apiKey)
        {
            this._APIUrl = apiUrl;
            this._APIKey = apiKey;
            this._BaseUrl = apiUrl + "?key=" + apiKey + "&info=";
        }

        /// <summary>
        /// 向机器人发送咨询问题
        /// </summary>
        /// <param name="question">咨询问题</param>
        /// <returns>机器人的回答</returns>
        public string Consult(string question)
        {
            string dat = "";
            HttpWebResponse res = null;
            HttpWebRequest req;

            try
            {
                req = (HttpWebRequest)WebRequest.Create(this._BaseUrl + question);
                req.AllowAutoRedirect = false;
                req.Timeout = 100000;
                req.Referer = null;
                req.Proxy = null;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0;%20WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";
                res = (HttpWebResponse)req.GetResponse();
            }
            catch (HttpException)
            {
                return "";
            }
            catch (WebException)
            {
                return "";
            }

            StreamReader reader;
            reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            dat = reader.ReadToEnd();

            res.Close();
            req.Abort();

            string answer = "";
            AnswerInfo info = (AnswerInfo)JsonConvert.DeserializeObject(dat, typeof(AnswerInfo));
            if (info.code == "100000")
            {
                answer = "图灵机器人[" + this._APIKey + "]：" + info.text;

                // 每一次访问，都需要记录为最后一次成功访问时间
                _lastTalkTime = DateTime.Now;
            }

            return answer;
        }

        /// <summary>
        /// 用API Key 作为机器人的唯一键
        /// </summary>
        internal string UniqueKey
        {
            get 
            {
                return this._APIKey;
            }
        }

        /// <summary>
        /// 返回闲置时间长度（单位：分钟）
        /// </summary>
        public int IdledMinutes
        {
            get 
            {
                TimeSpan idle = DateTime.Now.Subtract(_lastTalkTime);
                return idle.Minutes;
            }
        }

        /// <summary>
        /// 内部类，用于解释从图灵机器人返回的JSON格式数据
        /// </summary>
        private class AnswerInfo
        {
            public string code { get; set; }
            public string text { get; set; }
        }
    }
}
