namespace FileExportScheduler
{
    partial class FormSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetting));
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.txtExportFilePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.numChukyLuuDuLieu = new System.Windows.Forms.NumericUpDown();
            this.numChuKiXoaDuLieu = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFormatTime = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChukyLuuDuLieu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChuKiXoaDuLieu)).BeginInit();
            this.SuspendLayout();
            // 
            // chkAutoRun
            // 
            this.chkAutoRun.AutoSize = true;
            this.chkAutoRun.Location = new System.Drawing.Point(32, 203);
            this.chkAutoRun.Name = "chkAutoRun";
            this.chkAutoRun.Size = new System.Drawing.Size(116, 17);
            this.chkAutoRun.TabIndex = 5;
            this.chkAutoRun.Text = "Tự động khởi chạy";
            this.chkAutoRun.UseVisualStyleBackColor = true;
            // 
            // txtExportFilePath
            // 
            this.txtExportFilePath.Location = new System.Drawing.Point(21, 112);
            this.txtExportFilePath.Name = "txtExportFilePath";
            this.txtExportFilePath.Size = new System.Drawing.Size(262, 20);
            this.txtExportFilePath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Đường dẫn lưu dữ liệu";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(312, 112);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(37, 20);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(72, 226);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 27);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(242, 226);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 27);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chu kỳ lưu dữ liệu (phút)";
            // 
            // numChukyLuuDuLieu
            // 
            this.numChukyLuuDuLieu.Location = new System.Drawing.Point(21, 24);
            this.numChukyLuuDuLieu.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numChukyLuuDuLieu.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numChukyLuuDuLieu.Name = "numChukyLuuDuLieu";
            this.numChukyLuuDuLieu.Size = new System.Drawing.Size(328, 20);
            this.numChukyLuuDuLieu.TabIndex = 0;
            this.numChukyLuuDuLieu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numChukyLuuDuLieu.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numChuKiXoaDuLieu
            // 
            this.numChuKiXoaDuLieu.Location = new System.Drawing.Point(21, 68);
            this.numChuKiXoaDuLieu.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numChuKiXoaDuLieu.Name = "numChuKiXoaDuLieu";
            this.numChuKiXoaDuLieu.Size = new System.Drawing.Size(328, 20);
            this.numChuKiXoaDuLieu.TabIndex = 6;
            this.numChuKiXoaDuLieu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numChuKiXoaDuLieu.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Chu kì xóa dữ liệu (ngày)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Định dạng thời gian:";
            // 
            // txtFormatTime
            // 
            this.txtFormatTime.Location = new System.Drawing.Point(21, 161);
            this.txtFormatTime.Name = "txtFormatTime";
            this.txtFormatTime.Size = new System.Drawing.Size(262, 20);
            this.txtFormatTime.TabIndex = 9;
            this.txtFormatTime.Text = "yyyy-MM-dd HH:mm:ss";
            this.txtFormatTime.TextChanged += new System.EventHandler(this.txtFormatTime_TextChanged);
            // 
            // FormSetting
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(368, 265);
            this.Controls.Add(this.txtFormatTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numChuKiXoaDuLieu);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtExportFilePath);
            this.Controls.Add(this.numChukyLuuDuLieu);
            this.Controls.Add(this.chkAutoRun);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cấu hình hệ thống";
            this.Load += new System.EventHandler(this.FormSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChukyLuuDuLieu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChuKiXoaDuLieu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkAutoRun;
        private System.Windows.Forms.TextBox txtExportFilePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.NumericUpDown numChuKiXoaDuLieu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numChukyLuuDuLieu;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtFormatTime;
        private System.Windows.Forms.Label label4;
    }
}