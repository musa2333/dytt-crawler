using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace dyttspider.DLL
{
    /// <summary>
    /// http请求的帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 返回GET请求的响应的部分
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <returns></returns>
        public static String httpGetResponse(String url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            string html = "";
            using(HttpWebResponse response=(HttpWebResponse)req.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("GB2312")))
                        {
                            html = reader.ReadToEnd();
                            Console.WriteLine(html);
                        }
                    }
                }
            }
            return html;
        }

    }
}
