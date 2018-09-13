using dyttspider.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dyttspider.DLL
{
    /// <summary>
    /// 解析html获取数据
    /// </summary>
    public class ParseHtml
    {
        public ParseHtml()
        {

        }
        /// <summary>
        /// 解析html的部分
        /// </summary>
        /// <param name="html"></param>
        public static void parsehtml(string html)
        {

        }
        /// <summary>
        /// 解析获取网页的链接
        /// </summary>
        /// <param name="html"></param>
      
        /// <returns></returns>
        internal static string[] getLinks(string html)
        {
            List<String> links = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode rootnode = doc.GetElementbyId("header");
            string xpath = "//div[@class='co_content8']/ul//a";
            HtmlNodeCollection collection  = rootnode.SelectNodes(xpath);
           
            foreach (HtmlNode item in collection)
            {
               
                 links.Add( "http://www.ygdy8.com" + item.Attributes["href"].Value);
                
            }
            return links.ToArray();
        }
        /// <summary>
        /// 根据规则解析网页文档的部分代码
        /// </summary>
        /// <param name="titlepattern"></param>
        /// <param name="ftppattern"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        internal static List<Model.MovieInfo> getInfo(string titlepattern, string ftppattern, string html)
        {
           try
            {
                List<MovieInfo> infos = new List<MovieInfo>();
                MovieInfo info = new MovieInfo();
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode rootnode = doc.GetElementbyId("header");
                string xpath = @"//div[@class='title_all']/h1/font";
                HtmlNode title = rootnode.SelectSingleNode(xpath);
                string LinkXpath = @"//div[@id='Zoom']/span/td//table//a";//游戏的部分
              
                HtmlNodeCollection game = rootnode.SelectNodes(LinkXpath);
               
                    HtmlNodeCollection anodes=game;
                    int i = 1;
                    
                    foreach (HtmlNode node in anodes)
                    {
                         info = new MovieInfo();
                        info.Title = title.InnerHtml+"_"+i;
                        info.Link =  ConvertToThunderLink(node.Attributes["href"].Value.ToString());
                        infos.Add(info);
                        i++;
                    }
                /*}
                else
                {
                    HtmlNode downlink =  DownloadLink;
                    info.Link = ConvertToThunderLink(downlink.Attributes["href"].Value.ToString());
                    info.Title = title.InnerHtml;
                    infos.Add(info);
                }*/
                
                return infos;
            }
            catch (NullReferenceException e)
            {
             Console.WriteLine(e.GetType()+e.Message);
             return null;
            }
            
        }
        /// <summary>
        /// 切换为迅雷的链接地址
        /// </summary>
        /// <returns></returns>
        private static string ConvertToThunderLink(string url)
        {
            string thunderlink = "AA" + url + "ZZ";
            string result = "thunder://";
            byte[] str = System.Text.Encoding.Default.GetBytes(thunderlink);
            result += Convert.ToBase64String(str);
            return result;
        }

    }
}
