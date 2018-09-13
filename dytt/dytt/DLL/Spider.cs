using dyttspider.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace dyttspider.DLL
{
    /// <summary>
    /// 电影天堂的爬虫部分
    /// </summary>
    public class Spider
    {
        #region type
        
        private string _path = null;
        private Dictionary<string, int> _isSearchUrl = new Dictionary<string, int>();//已经爬去的链接的部分
        private Dictionary<string, int> _isUnSearchUrl = new Dictionary<string, int>();//没有爬去的链接的部分
        public event ContentSavedHanler ContentSave = null;//保存到本地触发
        public event DownloadFinishHandler DownloadFinish = null;//结束爬去 返回条数
        private int MaxDepth = 2;//最大深度
        private int _reqCount = 4;
        private bool[] _req = null;
        private readonly object _lock = new object();
        private bool _stop = false;
        private string _agent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
        private string _method = "get";
        private int _timeout = 2 * 60 * 1000;
        private int _index;
        private string _accept = "text/html";
        private WorkingUnitCollection workstate ;
        private Timer _checkTimer = null;
        private class WorkingUnitCollection
        {
            private int _count;
            //private AutoResetEvent[] _works;
            private bool[] _busy;
            public WorkingUnitCollection(int count)
            {
                _count = count;
                _busy=new bool[count];
                for (int i = 0; i < count; i++)
                {
                    _busy[i] = true;
                }
            }
            public void StartWorking(int index)
            {
                if (!_busy[index])
                {
                    _busy[index] = true;
                }
            }
            public  void Finishwork(int index)
            {
                if (_busy[index])
                {
                    _busy[index] = false;
                }
            }
            public void WaitAllFinished()
            {
                while (true)
                {
                    if (IsFinished())
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }

            public  bool IsFinished()
            {
                bool notEnd = false;
                foreach (var b in _busy)
                {
                    notEnd |= b;
                }
                return !notEnd;
            }
            /// <summary>
            /// 停止下载的部分代码显示
            /// </summary>
            internal void AbortAllWork()
            {
                for (int i = 0; i < _count; i++)
                {
                    _busy[i] = false;
                }
            }
        }
        #endregion
        /// <summary>
        /// 开始爬取资源的部分
        /// </summary>
        /// <param name="path"></param>
        public void search(string path)
        {
            this._path = path;
            Init();
            StartSearch();
        }
        /// <summary>
        /// 开始爬取数据的部分
        /// </summary>
        private void StartSearch()
        {
            _checkTimer = new System.Threading.Timer(new TimerCallback(CheckFinish),null,0,300);
            
            DispatchWork();
        }

        private void CheckFinish(object state)
        {
            if (workstate.IsFinished())
            {
                if (_checkTimer != null)
                {
                    _checkTimer.Dispose();
                    _checkTimer = null;
                    if (DownloadFinish != null)
                    {
                        DownloadFinish(_index);
                    }
                }
            }
        }
        /// <summary>
        /// 初始化数组的部分
        /// </summary>
        private void Init()
        {
            _stop = false;
            _isSearchUrl.Clear();
            _isUnSearchUrl.Clear();
            _req=new bool[_reqCount];
            AddUrl(new string[1] {_path},0);
            workstate = new WorkingUnitCollection(_reqCount);
        }
        /// <summary>
        /// 爬去没有用到的url到对应的数组中
        /// </summary>
        /// <param name="urls">对应的链接组</param>
        /// <param name="depth">深度</param>
        private void AddUrl(string[] urls, int depth)
        {
            if (depth >= MaxDepth)
            {
                return;
            }
            foreach (string url in urls)
            {
                string cleanurl = url.Trim();
                int end = cleanurl.IndexOf(" ");
                if (end > 0)
                {
                    cleanurl.Substring(0,end);
                }
                cleanurl=cleanurl.TrimEnd('/');
                if (UrlAvalibale(cleanurl))
                {
                    if (cleanurl.IndexOf("/plus/search.php")>0)
                    {
                        _isUnSearchUrl.Add(cleanurl, 0);//下一页的部分
                    }
                    else
                    {
                        _isUnSearchUrl.Add(cleanurl, depth);
                    }
                }
            }
        }
        /// <summary>
        /// 判断当前url是否已经爬去过了
        /// </summary>
        /// <param name="cleanurl"></param>
        /// <returns></returns>
        private bool UrlAvalibale(string cleanurl)
        {
            bool result = _isUnSearchUrl.ContainsKey(cleanurl);
            result |= _isSearchUrl.ContainsKey(cleanurl);
            return !result;
        }
        /// <summary>
        /// 并发控制
        /// </summary>
        private void DispatchWork()
        {
            if (_stop)
            {
                return;
            }
            for (int i = 0; i < _reqCount; i++)
            {
                if (!_req[i])
                {
                    RequestResource(i);
                }
            }
        }
        /// <summary>
        /// 每个实例开始执行解析
        /// </summary>
        /// <param name="i"></param>
        private void RequestResource(int i)
        {
            int depth = 0;
            string url = "";
            if (_stop)
            {
                return;
            }
            try
            {
                lock (_lock)
                {
                    if (_isUnSearchUrl.Count() <= 0)
                    {
                        workstate.Finishwork(i);
                        return;
                    }
                    _req[i] = true;
                    workstate.StartWorking(i);
                    depth = _isUnSearchUrl.First().Value;
                    url = _isUnSearchUrl.First().Key;
                    //方法在此添加
                    //string html = HttpHelper.httpGetResponse(url);

                    _isSearchUrl.Add(url,depth);
                    _isUnSearchUrl.Remove(url);
                }
                //执行web解析
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = _method;
                req.UserAgent = _agent;
                req.Timeout = _timeout;
                req.Accept = _accept;
                RequestState requestState = new RequestState(url,depth,req,i);//异步传递的对象
                var result = req.BeginGetResponse(new AsyncCallback(ReciveData),requestState);//异步处理请求
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle,TimeoutHandler,requestState,_timeout,true);//注册超时的方法
            }
            catch (Exception we)
            {
                Console.WriteLine(we);
            }
        }
        /// <summary>
        /// 超时处理方法
        /// </summary>
        /// <param name="state"></param>
        /// <param name="timedOut"></param>
        private void TimeoutHandler(object state, bool timedOut)
        {
            if (timedOut)
            {
                RequestState rs = state as RequestState;
                if (rs != null)
                {
                    rs.req.Abort();

                }
                _req[rs.Index] = false;
                DispatchWork();
            }
        }
        /// <summary>
        /// 异步获取数据的部分
        /// </summary>
        /// <param name="ar"></param>
        private void ReciveData(IAsyncResult ar)
        {
            RequestState rs = (RequestState)ar.AsyncState;
            HttpWebRequest req = rs.req;
            string url = rs.Url;
            try
            {
                HttpWebResponse res = (HttpWebResponse)req.EndGetResponse(ar);
                /*if (_stop)
                {
                    res.Close();
                    req.Abort();
                    return;
                }
                else*/ 
                if (res != null && res.StatusCode == HttpStatusCode.OK)
                {
                    //进行处理网页数据的解析
                    Stream stream = res.GetResponseStream();
                    rs.Stream = stream;
                    var result = stream.BeginRead(rs.Data,0,rs.BufferSize,new AsyncCallback(ReciveResource),rs);
                }
                else
                {
                    res.Close();
                    req.Abort();
                    _req[rs.Index]= false;
                    DispatchWork();//重新分配工作

                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="ar"></param>
        private void ReciveResource(IAsyncResult ar)
        {
            RequestState rs = (RequestState)ar.AsyncState;
            
            string html = "";
            Stream resStream = rs.Stream;
            string url=rs.Url;
            int read = 0;
            try
            {
                read = rs.Stream.EndRead(ar);
                if (read > 0)
                {
                    MemoryStream ms = new MemoryStream(rs.Data,0,read);
                    StreamReader reader = new StreamReader(ms, Encoding.GetEncoding("GB2312"));
                    string str = reader.ReadToEnd();
                    rs.html.Append(str);
                    var result = resStream.BeginRead(rs.Data, 0, rs.BufferSize, new AsyncCallback(ReciveResource), rs);
                    return;
                }
                html = rs.html.ToString();
                if (rs.Depth == 0)
                {
                    string[] links = GetLinks(html);
                    AddUrl(links, rs.Depth + 1);
                    //执行页面解析和对应的
                }
                else if (rs.Depth == 1)
                {
                    ParseHtmlResources(html);
                }
                _req[rs.Index] = false;
                DispatchWork();
            }
            catch (WebException e)
            {
                Console.WriteLine("ReceivedData Web " + e.Message + url + e.Status);
                workstate.Finishwork(rs.Index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().ToString() + e.Message);//记录日志出现的问题的部分
                workstate.Finishwork(rs.Index);
            }

         
        }
        /// <summary>
        /// 解析网页中的相关信息
        /// </summary>
        private void ParseHtmlResources(string html)
        {
            List<MovieInfo> infos = new List<MovieInfo>();
            lock(_lock)//并发锁机制控制解析
            {
            string titlepattern = @"<title>(.*?)</title>";
            string ftppattern = @"<a href='(ftp.*?)'";
            infos = ParseHtml.getInfo(titlepattern,ftppattern,html);
            }
            if (ContentSave != null)
            {
                ContentSave(infos);
            }

        }
        /// <summary>
        /// 通过搜索界面获取链接
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string[] GetLinks(string html)
        {
            
            string[] Links = ParseHtml.getLinks(html);
             return Links;
        }

        /// <summary>
        /// 终止下载
        /// </summary>
        public void Abort()
        {
            _stop = true;
            if (workstate != null)
            {
                workstate.AbortAllWork();
            }
        }
        
        public delegate void ContentSavedHanler(List<MovieInfo> infos );
        public delegate void DownloadFinishHandler(int count);
    }
}
