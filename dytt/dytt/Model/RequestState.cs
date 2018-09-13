using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace dyttspider.Model
{
    /// <summary>
    /// 请求的状态的部分
    /// </summary>
    public class RequestState
    {
        public string Url { private set; get; }
        public int Depth { private set; get; }
        public HttpWebRequest req { private set; get; }
        public int Index { private set; get; }
        private const int BUFFER_SIZE = 131072;

        public int BufferSize
        {
            get { return BUFFER_SIZE; }
        }

        private byte[] _data = new byte[BUFFER_SIZE];

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        
        private StringBuilder _sb = new StringBuilder();

        public StringBuilder html
        {
            get { return _sb; }
            set { _sb = value; }
        }
        public RequestState(string url,int depth,HttpWebRequest Req,int index)
        {
            Url = url;
            Depth = depth;
            req = Req;
            Index = index;
        }
        /// <summary>
        /// 接收到数据流大小
        /// </summary>
        public System.IO.Stream Stream { get;  set; }
    }
}
