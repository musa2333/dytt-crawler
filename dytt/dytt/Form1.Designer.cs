namespace dyttspider
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.labTile = new System.Windows.Forms.Label();
            this.lvResult = new System.Windows.Forms.ListView();
            this.num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.link = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pgSearch = new System.Windows.Forms.ProgressBar();
            this.contextlv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.download = new System.Windows.Forms.ToolStripMenuItem();
            this.contextlv.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(59, 24);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(296, 21);
            this.txtTitle.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(361, 23);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(73, 22);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // labTile
            // 
            this.labTile.AutoSize = true;
            this.labTile.Location = new System.Drawing.Point(12, 27);
            this.labTile.Name = "labTile";
            this.labTile.Size = new System.Drawing.Size(41, 12);
            this.labTile.TabIndex = 2;
            this.labTile.Text = "关键字";
            // 
            // lvResult
            // 
            this.lvResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.num,
            this.Title,
            this.link});
            this.lvResult.FullRowSelect = true;
            this.lvResult.GridLines = true;
            this.lvResult.HideSelection = false;
            this.lvResult.Location = new System.Drawing.Point(12, 50);
            this.lvResult.Name = "lvResult";
            this.lvResult.Size = new System.Drawing.Size(631, 236);
            this.lvResult.TabIndex = 3;
            this.lvResult.UseCompatibleStateImageBehavior = false;
            this.lvResult.View = System.Windows.Forms.View.Details;
            this.lvResult.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvResult_MouseClick);
            this.lvResult.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvResult_MouseDoubleClick);
            // 
            // num
            // 
            this.num.Text = "编号";
            this.num.Width = 43;
            // 
            // Title
            // 
            this.Title.Text = "资源名称";
            this.Title.Width = 280;
            // 
            // link
            // 
            this.link.Text = "资源地址";
            this.link.Width = 270;
            // 
            // pgSearch
            // 
            this.pgSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pgSearch.Location = new System.Drawing.Point(543, 292);
            this.pgSearch.Name = "pgSearch";
            this.pgSearch.Size = new System.Drawing.Size(100, 23);
            this.pgSearch.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgSearch.TabIndex = 4;
            this.pgSearch.Visible = false;
            // 
            // contextlv
            // 
            this.contextlv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.download});
            this.contextlv.Name = "contextlv";
            this.contextlv.Size = new System.Drawing.Size(101, 26);
            // 
            // download
            // 
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(152, 22);
            this.download.Text = "下载";
            this.download.Click += new System.EventHandler(this.download_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 316);
            this.Controls.Add(this.pgSearch);
            this.Controls.Add(this.lvResult);
            this.Controls.Add(this.labTile);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtTitle);
            this.Name = "Form1";
            this.Text = "电影资源搜索";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextlv.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label labTile;
        private System.Windows.Forms.ListView lvResult;
        private System.Windows.Forms.ProgressBar pgSearch;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ColumnHeader link;
        private System.Windows.Forms.ColumnHeader num;
        private System.Windows.Forms.ContextMenuStrip contextlv;
        private System.Windows.Forms.ToolStripMenuItem download;
    }
}

