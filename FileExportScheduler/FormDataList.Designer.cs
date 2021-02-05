namespace FileExportScheduler
{
    partial class FormDataList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataList));
            this.cms_NhaMay = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsAddThietBi = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tvMain = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.cms_Slave = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.delToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_ThietBi = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.thêmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xóaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_NhaMay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.cms_Slave.SuspendLayout();
            this.cms_ThietBi.SuspendLayout();
            this.SuspendLayout();
            // 
            // cms_NhaMay
            // 
            this.cms_NhaMay.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_NhaMay.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsAddThietBi});
            this.cms_NhaMay.Name = "ctxMenu";
            this.cms_NhaMay.Size = new System.Drawing.Size(105, 26);
            // 
            // cmsAddThietBi
            // 
            this.cmsAddThietBi.Name = "cmsAddThietBi";
            this.cmsAddThietBi.Size = new System.Drawing.Size(104, 22);
            this.cmsAddThietBi.Text = "Thêm";
            this.cmsAddThietBi.Click += new System.EventHandler(this.cmsAddThietBi_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tvMain);
            this.splitContainer.Size = new System.Drawing.Size(1105, 633);
            this.splitContainer.SplitterDistance = 250;
            this.splitContainer.TabIndex = 0;
            // 
            // tvMain
            // 
            this.tvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMain.ImageIndex = 0;
            this.tvMain.ImageList = this.imageList;
            this.tvMain.Location = new System.Drawing.Point(0, 0);
            this.tvMain.Name = "tvMain";
            this.tvMain.SelectedImageIndex = 0;
            this.tvMain.Size = new System.Drawing.Size(250, 633);
            this.tvMain.TabIndex = 0;
            this.tvMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvMain_NodeMouseClick);
            this.tvMain.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvMain_NodeMouseDoubleClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Protocol.ico");
            this.imageList.Images.SetKeyName(1, "Device.ico");
            this.imageList.Images.SetKeyName(2, "network.ico");
            // 
            // cms_Slave
            // 
            this.cms_Slave.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_Slave.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.delToolStripMenuItem});
            this.cms_Slave.Name = "tx2";
            this.cms_Slave.Size = new System.Drawing.Size(181, 48);
            // 
            // delToolStripMenuItem
            // 
            this.delToolStripMenuItem.Name = "delToolStripMenuItem";
            this.delToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.delToolStripMenuItem.Text = "Xóa";
            this.delToolStripMenuItem.Click += new System.EventHandler(this.cms_Xoa_SlaveAddress);
            // 
            // cms_ThietBi
            // 
            this.cms_ThietBi.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thêmToolStripMenuItem,
            this.xóaToolStripMenuItem});
            this.cms_ThietBi.Name = "them_xoa_Menu";
            this.cms_ThietBi.Size = new System.Drawing.Size(105, 48);
            // 
            // thêmToolStripMenuItem
            // 
            this.thêmToolStripMenuItem.Name = "thêmToolStripMenuItem";
            this.thêmToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.thêmToolStripMenuItem.Text = "Thêm";
            this.thêmToolStripMenuItem.Click += new System.EventHandler(this.cms_Them_SlaveAddress);
            // 
            // xóaToolStripMenuItem
            // 
            this.xóaToolStripMenuItem.Name = "xóaToolStripMenuItem";
            this.xóaToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.xóaToolStripMenuItem.Text = "Xóa";
            this.xóaToolStripMenuItem.Click += new System.EventHandler(this.cms_Xoa_ThietBi);
            // 
            // FormDataList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 633);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormDataList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý dữ liệu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDataList_FormClosing);
            this.Load += new System.EventHandler(this.FormDataList_Load);
            this.cms_NhaMay.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.cms_Slave.ResumeLayout(false);
            this.cms_ThietBi.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        public System.Windows.Forms.TreeView tvMain;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip cms_NhaMay;
        private System.Windows.Forms.ToolStripMenuItem cmsAddThietBi;
        public System.Windows.Forms.ContextMenuStrip cms_Slave;
        private System.Windows.Forms.ToolStripMenuItem delToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip cms_ThietBi;
        private System.Windows.Forms.ToolStripMenuItem thêmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xóaToolStripMenuItem;
    }
}