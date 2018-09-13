using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dyttspider.Model
{
    /// <summary>
    /// 爬虫信息的抽象数据类型
    /// </summary>
    public class MovieInfo
    {
        private string _title;
        /// <summary>
        /// 影片名称
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _link;
        /// <summary>
        /// 电影的迅雷下载链接
        /// </summary>
        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }

    }
}
