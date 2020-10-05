namespace FileExportScheduler
{
    partial class FormHienThiDuLieu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHienThiDuLieu));
            this.tmrHienThongSoSuLieu = new System.Windows.Forms.Timer(this.components);
            this.dgvHienThiDuLieu = new System.Windows.Forms.DataGridView();
            this.ten = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThietBi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Diachi_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiaChi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Scale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Donvido_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHienThiDuLieu)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrHienThongSoSuLieu
            // 
            this.tmrHienThongSoSuLieu.Tick += new System.EventHandler(this.tmrHienThongSoSuLieu_Tick);
            // 
            // dgvHienThiDuLieu
            // 
            this.dgvHienThiDuLieu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHienThiDuLieu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ten,
            this.ThietBi,
            this.Diachi_P,
            this.DiaChi,
            this.Scale,
            this.Donvido_P});
            this.dgvHienThiDuLieu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHienThiDuLieu.Location = new System.Drawing.Point(0, 0);
            this.dgvHienThiDuLieu.Margin = new System.Windows.Forms.Padding(2);
            this.dgvHienThiDuLieu.Name = "dgvHienThiDuLieu";
            this.dgvHienThiDuLieu.RowHeadersWidth = 51;
            this.dgvHienThiDuLieu.RowTemplate.Height = 24;
            this.dgvHienThiDuLieu.Size = new System.Drawing.Size(1067, 445);
            this.dgvHienThiDuLieu.TabIndex = 4;
            // 
            // ten
            // 
            this.ten.DataPropertyName = "ten";
            this.ten.HeaderText = "Tên";
            this.ten.MinimumWidth = 50;
            this.ten.Name = "ten";
            // 
            // ThietBi
            // 
            this.ThietBi.DataPropertyName = "DiemDo";
            this.ThietBi.HeaderText = "Điểm đo";
            this.ThietBi.MinimumWidth = 6;
            this.ThietBi.Name = "ThietBi";
            this.ThietBi.Width = 250;
            // 
            // Diachi_P
            // 
            this.Diachi_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Diachi_P.DataPropertyName = "ThietBi";
            this.Diachi_P.HeaderText = "Thiết bị";
            this.Diachi_P.MinimumWidth = 50;
            this.Diachi_P.Name = "Diachi_P";
            // 
            // DiaChi
            // 
            this.DiaChi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DiaChi.DataPropertyName = "DiaChi";
            this.DiaChi.HeaderText = "Địa chỉ";
            this.DiaChi.Name = "DiaChi";
            // 
            // Scale
            // 
            this.Scale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Scale.DataPropertyName = "GiaTri";
            this.Scale.HeaderText = "Giá Trị";
            this.Scale.MinimumWidth = 6;
            this.Scale.Name = "Scale";
            // 
            // Donvido_P
            // 
            this.Donvido_P.DataPropertyName = "TrangThaiTinHieu";
            this.Donvido_P.HeaderText = "Trạng Thái Tín Hiệu";
            this.Donvido_P.MinimumWidth = 50;
            this.Donvido_P.Name = "Donvido_P";
            this.Donvido_P.Width = 130;
            // 
            // FormHienThiDuLieu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 445);
            this.Controls.Add(this.dgvHienThiDuLieu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormHienThiDuLieu";
            this.Text = "Realtime Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHienThiDuLieu_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormHienThiDuLieu_FormClosed);
            this.Load += new System.EventHandler(this.FormHienThiDuLieu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHienThiDuLieu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrHienThongSoSuLieu;
        public System.Windows.Forms.DataGridView dgvHienThiDuLieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn ten;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThietBi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Diachi_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiaChi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scale;
        private System.Windows.Forms.DataGridViewTextBoxColumn Donvido_P;
    }
}