using dyttspider.DLL;
using dyttspider.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dyttspider
{
    public partial class Form1 : Form
    {
        
        private Spider spider = null;
        public Form1()
        {
            spider = new Spider();
            spider.ContentSave += new Spider.ContentSavedHanler(Spider_Contentsave);
            spider. DownloadFinish+=new Spider.DownloadFinishHandler(Spider_Finish);
            InitializeComponent();
        }
        #region show result
        void Spider_Finish(int count)
        {
            spider.Abort();
            if (btnSearch.InvokeRequired)
            {
                Action<int> actionDelegate=(x)=>{
                    this.btnSearch.Enabled=true;
                    this.pgSearch.Visible = false;

                };
                this.btnSearch.Invoke(actionDelegate,count);
            }
            else
            {
                this.btnSearch.Enabled = true;
                this.pgSearch.Visible = false;
            }
        }

         void Spider_Contentsave(List<Model.MovieInfo> infos)
        {
            if (lvResult.InvokeRequired)
            {
                // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                Action<List<MovieInfo>> actionDelegate = (x) => {
                    if (x != null)
                    {
                        foreach (MovieInfo info in x)
                        {
                            ListViewItem lv = new ListViewItem((this.lvResult.Items.Count + 1).ToString());
                            lv.SubItems.Add(info.Title);
                            lv.SubItems.Add(info.Link);
                            this.lvResult.Items.Add(lv);
                        }
                    }
                    
                };
                // 或者
                // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                this.lvResult.Invoke(actionDelegate, infos);
            }
            else
            {
                if (infos != null)
                {
                    foreach (MovieInfo info in infos)
                    {
                        ListViewItem lv = new ListViewItem((this.lvResult.Items.Count+1).ToString());
                        lv.SubItems.Add(info.Title);
                        lv.SubItems.Add(info.Link);
                        this.lvResult.Items.Add(lv);
                    }
                }
            }
                
        }
        #endregion
         #region search
         /// <summary>
        /// 点击搜索的部分，搜索电影资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            String keywords = this.txtTitle.Text.Trim();
            if (keywords == "")
            {
                MessageBox.Show("查询关键字不能为空，请重写");
                return;
            }
            this.btnSearch.Enabled = false;
            this.pgSearch.Visible = true;
            string url=assemUrl(keywords);
            lvResult.Items.Clear();
            Thread thread = new Thread(new ParameterizedThreadStart(Search));
            thread.Start(url);
        }
        /// <summary>
        /// 查询方法的部分
        /// </summary>
        /// <param name="obj"></param>
        private void Search(object obj)
        {
            spider.search((String)obj);
        }
        /// <summary>
        /// 组装关键字成为请求的地址链接
        /// </summary>
        /// <param name="keywords"></param>
        private string assemUrl(string keywords)
        {
            string url = "http://s.dydytt.net/plus/search.php?keyword="+Function.StringToUrlencode(keywords);
            return url;
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        #region download
        /// <summary>
        /// 双击显示下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            downloadRes(); 
        }

        private void download_Click(object sender, EventArgs e)
        {
            downloadRes();
        }
       

        private void lvResult_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.lvResult.SelectedItems.Count == 1 && e.Button == MouseButtons.Right)
            {
                contextlv.Show(MousePosition);
            }
        }
        private void downloadRes()
        {
            try
            {
                ListViewItem item = this.lvResult.SelectedItems[0];
                MovieInfo info = new MovieInfo();
                info.Link = item.SubItems[2].Text.ToString();
                info.Title = item.SubItems[1].Text.ToString();
                Function.AddTaskToThunder(info);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType() + ex.Message);
                MessageBox.Show("添加任务到迅雷失败，请重试");
                return;
            }
        }
        #endregion
    }
}
