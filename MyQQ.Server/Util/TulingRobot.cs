using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MyQQ.Util
{
    public class TulingRobot
    {
        public static readonly string TulingBaseURL = "http://www.tuling123.com/openapi/api?key=a0471ae861f34009a20950aadd93c88f&info=";

        public static string Answer(string question)
        {
            string dat = "";
            HttpWebResponse res = null;
            HttpWebRequest req;

            try
            {
                req = (HttpWebRequest)WebRequest.Create(TulingBaseURL + question);               
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
                answer = "图灵机器人：" + info.text;
            }

            return answer;
        }

        public class AnswerInfo
        {
            public string code { get; set; }
            public string text { get; set; }
        }
    }
}