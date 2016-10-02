using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/*
  https://github.com/zeruniverse/QQRobot
  重要：群聊被TX认为是极度危险的行为，因此如果账号被怀疑被盗号（异地登陆），群聊消息会发不出去。
  表现为程序能收到群聊消息，群聊消息发送返回值为发送成功，但其他群成员无法看到您发出的消息。
  大约登陆10分钟后您会收到QQ提醒提示账号被盗，要求改密码，同时账号被临时冻结。不知为何该程序刚运行时总是被怀疑异地登陆，
  当您重复解冻3次后（就是改密码），TX基本就不再怀疑您了，一般一次能稳定挂机2-3天。强烈推荐您用小号挂QQ小黄鸡！
*/

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