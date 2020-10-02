namespace FileExportScheduler
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.lblTenSanPham = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblPhienBan = new System.Windows.Forms.Label();
            this.lblBanQuyen = new System.Windows.Forms.Label();
            this.lblDonViCap = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTenSanPham
            // 
            this.lblTenSanPham.AutoSize = true;
            this.lblTenSanPham.Location = new System.Drawing.Point(183, 10);
            this.lblTenSanPham.Name = "lblTenSanPham";
            this.lblTenSanPham.Size = new System.Drawing.Size(78, 13);
            this.lblTenSanPham.TabIndex = 0;
            this.lblTenSanPham.Text = "Tên Sản Phẩm";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::FileExportScheduler.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(177, 174);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(172, 180);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblPhienBan
            // 
            this.lblPhienBan.AutoSize = true;
            this.lblPhienBan.Location = new System.Drawing.Point(183, 35);
            this.lblPhienBan.Name = "lblPhienBan";
            this.lblPhienBan.Size = new System.Drawing.Size(56, 13);
            this.lblPhienBan.TabIndex = 0;
            this.lblPhienBan.Text = "Phiên Bản";
            // 
            // lblBanQuyen
            // 
            this.lblBanQuyen.AutoSize = true;
            this.lblBanQuyen.Location = new System.Drawing.Point(183, 60);
            this.lblBanQuyen.Name = "lblBanQuyen";
            this.lblBanQuyen.Size = new System.Drawing.Size(60, 13);
            this.lblBanQuyen.TabIndex = 0;
            this.lblBanQuyen.Text = "Bản Quyền";
            // 
            // lblDonViCap
            // 
            this.lblDonViCap.AutoSize = true;
            this.lblDonViCap.Location = new System.Drawing.Point(183, 85);
            this.lblDonViCap.Name = "lblDonViCap";
            this.lblDonViCap.Size = new System.Drawing.Size(61, 13);
            this.lblDonViCap.TabIndex = 0;
            this.lblDonViCap.Text = "Đơn Vị Cấp";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(183, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(198, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Website: http://www.e-solutions.com.vn";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 211);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblDonViCap);
            this.Controls.Add(this.lblBanQuyen);
            this.Controls.Add(this.lblPhienBan);
            this.Controls.Add(this.lblTenSanPham);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAbout";
            this.Text = "Thông tin phần mềm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTenSanPham;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblPhienBan;
        private System.Windows.Forms.Label lblBanQuyen;
        private System.Windows.Forms.Label lblDonViCap;
        private System.Windows.Forms.Label label5;
    }
}