namespace FileExportScheduler
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnDataList = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tmrDocDuLieu = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrXuatFile = new System.Windows.Forms.Timer(this.components);
            this.tmrChukyXoaFile = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTrangThaiThietBi = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnThongSoDuLieu = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(16, 39);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(300, 57);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Chạy";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(325, 39);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(300, 57);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Dừng";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnDataList
            // 
            this.btnDataList.Location = new System.Drawing.Point(16, 103);
            this.btnDataList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDataList.Name = "btnDataList";
            this.btnDataList.Size = new System.Drawing.Size(147, 28);
            this.btnDataList.TabIndex = 2;
            this.btnDataList.Text = "Cấu hình giao thức";
            this.btnDataList.UseVisualStyleBackColor = true;
            this.btnDataList.Click += new System.EventHandler(this.btnDataList_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(171, 103);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(147, 28);
            this.btnSetting.TabIndex = 3;
            this.btnSetting.Text = "Cấu hình lưu file";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(480, 103);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(147, 28);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tmrDocDuLieu
            // 
            this.tmrDocDuLieu.Interval = 1000;
            this.tmrDocDuLieu.Tick += new System.EventHandler(this.tmrDocDuLieu_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "Phần mềm đang chạyy";
            this.notifyIcon.BalloonTipTitle = "Hệ thống";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Phần mềm xuất số liệu định kỳ";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.AutoSize = false;
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.licenseToolStripMenuItem,
            this.toolStripSeparator1,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.ShowCheckMargin = true;
            this.contextMenuStrip.Size = new System.Drawing.Size(100, 80);
            // 
            // licenseToolStripMenuItem
            // 
            this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
            this.licenseToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.licenseToolStripMenuItem.Text = "About";
            this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(96, 6);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.openToolStripMenuItem.Text = "Mở";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click_1);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.closeToolStripMenuItem.Text = "Đóng";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tmrXuatFile
            // 
            this.tmrXuatFile.Interval = 1000;
            this.tmrXuatFile.Tick += new System.EventHandler(this.tmrXuatFile_Tick);
            // 
            // tmrChukyXoaFile
            // 
            this.tmrChukyXoaFile.Tick += new System.EventHandler(this.tmrChukyXoaFile_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblTrangThaiThietBi});
            this.statusStrip1.Location = new System.Drawing.Point(0, 165);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(641, 26);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(78, 20);
            this.toolStripStatusLabel1.Text = "Trạng thái:";
            // 
            // lblTrangThaiThietBi
            // 
            this.lblTrangThaiThietBi.Name = "lblTrangThaiThietBi";
            this.lblTrangThaiThietBi.Size = new System.Drawing.Size(45, 20);
            this.lblTrangThaiThietBi.Text = "None";
            // 
            // btnThongSoDuLieu
            // 
            this.btnThongSoDuLieu.Enabled = false;
            this.btnThongSoDuLieu.Location = new System.Drawing.Point(325, 103);
            this.btnThongSoDuLieu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThongSoDuLieu.Name = "btnThongSoDuLieu";
            this.btnThongSoDuLieu.Size = new System.Drawing.Size(147, 28);
            this.btnThongSoDuLieu.TabIndex = 2;
            this.btnThongSoDuLieu.Text = "Monitor";
            this.btnThongSoDuLieu.UseVisualStyleBackColor = true;
            this.btnThongSoDuLieu.Click += new System.EventHandler(this.btnThongSoDuLieu_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 191);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnThongSoDuLieu);
            this.Controls.Add(this.btnDataList);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ES PROTOCOL CONVERTER";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        public System.Windows.Forms.Button btnDataList;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Timer tmrDocDuLieu;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Timer tmrXuatFile;
        private System.Windows.Forms.Timer tmrChukyXoaFile;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblTrangThaiThietBi;
        private System.Windows.Forms.Button btnThongSoDuLieu;
    }
}

