using SmartQQ.model;
using SmartQQ.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SmartQQ
{
    public class LoginService
    {
        private static Random rand = new Random();

        /// <summary>
        /// Get the image file of Login QR Code
        /// </summary>
        /// <returns></returns>
        public Stream GetQRCodeStream()
        {
            Stream stream = null;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://ssl.ptlogin2.qq.com/ptqrshow?appid=501004106&e=0&l=M&s=5&d=72&v=4&t=#{t}".Replace("#{t}", rand.NextDouble().ToString()));
                req.CookieContainer = HTTP.cookies;

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                stream = res.GetResponseStream();
            }
            catch (Exception) { return null; }

            return stream;
        }

        /// <summary>
        /// Check the QR Code status
        /// </summary>
        /// <returns></returns>
        public QRStatus CheckQRCodeStatus()
        {
            QRStatus status = new QRStatus();
            string dat = HTTP.Get("https://ssl.ptlogin2.qq.com/ptqrlogin?webqq_type=10&remember_uin=1&login2qq=1&aid=501004106&u1=http%3A%2F%2Fw.qq.com%2Fproxy.html%3Flogin2qq%3D1%26webqq_type%3D10 &ptredirect=0&ptlang=2052&daid=164&from_ui=1&pttype=1&dumy=&fp=loginerroralert &action=0-0-157510&mibao_css=m_webqq&t=1&g=1&js_type=0&js_ver=10143&login_sig=&pt_randsalt=0", 
                                  "https://ui.ptlogin2.qq.com/cgi-bin/login?daid=164&target=self&style=16&mibao_css=m_webqq&appid=501004106&enable_qlogin=0&no_verifyimg=1 &s_url=http%3A%2F%2Fw.qq.com%2Fproxy.html&f_url=loginerroralert &strong_login=1&login_state=10&t=20131024001");

            System.Console.Out.WriteLine("The response data for checking login QR code status :" + dat);

            if (dat == null || dat.Trim().Length == 0) 
            {
                return null;
            }

            dat = dat.Substring(dat.IndexOf("(") + 1);
            dat = dat.Substring(0, dat.IndexOf(");"));
            string[] temp = dat.Split(',');

            status.StatusCode = temp[0].Replace("'","");
            status.StatusText = temp[4].Replace("'", ""); ;
            status.RedirectUrl = temp[2].Replace("'", ""); ;

            return status;
        }

        /// <summary>
        /// Process login request
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public SmartQQWrapper ProcessLoginRequest(string redirectUrl)
        {
            string url = "";
            string dat = "";

            // 1.
            string ptwebqq = "";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    dat = HTTP.Get(redirectUrl, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");
                    Uri uri = new Uri("http://web2.qq.com/");
                    ptwebqq = HTTP.cookies.GetCookies(uri)["ptwebqq"].Value;
                    break;
                }
                catch (Exception ex)
                { 
                }
            }

            // 2.
            string vfwebqq = "";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    url = String.Format("http://s.web2.qq.com/api/getvfwebqq?ptwebqq={0}&clientid=53999199&psessionid=&t={1}", ptwebqq, HTTP.AID_TimeStamp());
                    dat = HTTP.Get(url, "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1");
                    vfwebqq = dat.Substring(dat.IndexOf("\"vfwebqq\":") + "\"vfwebqq\":".Length + 1);
                    vfwebqq = vfwebqq.Substring(0, vfwebqq.IndexOf("\""));
                    break;
                }
                catch (Exception ex)
                { 
                }
            }

            // 3.
            string psessionid = "";
            string qqNbr = "";

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    url = "http://d1.web2.qq.com/channel/login2";
                    string url1 = "{\"ptwebqq\":\"#{ptwebqq}\",\"clientid\":53999199,\"psessionid\":\"\",\"status\":\"online\"}".Replace("#{ptwebqq}", ptwebqq);
                    url1 = "r=" + HttpUtility.UrlEncode(url1);
                    dat = HTTP.Post(url, url1, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");
                    psessionid = dat.Substring(dat.IndexOf("\"psessionid\":") + "\"psessionid\":".Length + 1);
                    psessionid = psessionid.Substring(0, psessionid.IndexOf("\""));

                    qqNbr = dat.Substring(dat.IndexOf("\"uin\":") + "\"uin\":".Length);
                    qqNbr = qqNbr.Substring(0, qqNbr.IndexOf(","));
                    break;
                }
                catch (Exception ex)
                { 
                }
            }

            string hash = "";
            hash = HTTP.AID_Hash(qqNbr, ptwebqq);

            SmartQQWrapper smartQQ = new SmartQQWrapper();
            smartQQ.PTWebQQ = ptwebqq;
            smartQQ.VFWebQQ = vfwebqq;
            smartQQ.PSessionId = psessionid;
            smartQQ.QQAccount = qqNbr;
            smartQQ.Hash = hash;

            return smartQQ;
        }        
    }
}
