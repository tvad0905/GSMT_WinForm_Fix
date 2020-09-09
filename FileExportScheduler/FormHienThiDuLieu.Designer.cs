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
            this.tmrHienThongSoSuLieu = new System.Windows.Forms.Timer(this.components);
            this.Donvido_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Scale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Diachi_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThietBi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ten = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHienThiDuLieu = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHienThiDuLieu)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrHienThongSoSuLieu
            // 
            this.tmrHienThongSoSuLieu.Tick += new System.EventHandler(this.tmrHienThongSoSuLieu_Tick);
            // 
            // Donvido_P
            // 
            this.Donvido_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Donvido_P.DataPropertyName = "TrangThaiTinHieu";
            this.Donvido_P.HeaderText = "Trạng Thái Tín Hiệu";
            this.Donvido_P.MinimumWidth = 50;
            this.Donvido_P.Name = "Donvido_P";
            // 
            // Scale
            // 
            this.Scale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Scale.DataPropertyName = "GiaTri";
            this.Scale.HeaderText = "Giá Trị";
            this.Scale.MinimumWidth = 6;
            this.Scale.Name = "Scale";
            // 
            // Diachi_P
            // 
            this.Diachi_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Diachi_P.DataPropertyName = "ThietBi";
            this.Diachi_P.HeaderText = "Thiết bị";
            this.Diachi_P.MinimumWidth = 50;
            this.Diachi_P.Name = "Diachi_P";
            // 
            // ThietBi
            // 
            this.ThietBi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThietBi.DataPropertyName = "DiemDo";
            this.ThietBi.HeaderText = "Điểm đo";
            this.ThietBi.MinimumWidth = 6;
            this.ThietBi.Name = "ThietBi";
            // 
            // ten
            // 
            this.ten.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ten.DataPropertyName = "ten";
            this.ten.HeaderText = "Tên";
            this.ten.MinimumWidth = 50;
            this.ten.Name = "ten";
            // 
            // dgvHienThiDuLieu
            // 
            this.dgvHienThiDuLieu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHienThiDuLieu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ten,
            this.ThietBi,
            this.Diachi_P,
            this.Scale,
            this.Donvido_P});
            this.dgvHienThiDuLieu.Location = new System.Drawing.Point(0, 0);
            this.dgvHienThiDuLieu.Margin = new System.Windows.Forms.Padding(2);
            this.dgvHienThiDuLieu.Name = "dgvHienThiDuLieu";
            this.dgvHienThiDuLieu.RowHeadersWidth = 51;
            this.dgvHienThiDuLieu.RowTemplate.Height = 24;
            this.dgvHienThiDuLieu.Size = new System.Drawing.Size(1095, 514);
            this.dgvHienThiDuLieu.TabIndex = 4;
            // 
            // FormHienThiDuLieu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 514);
            this.Controls.Add(this.dgvHienThiDuLieu);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormHienThiDuLieu";
            this.Text = "FormHienThiDuLieu";
            this.Load += new System.EventHandler(this.FormHienThiDuLieu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHienThiDuLieu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrHienThongSoSuLieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn Donvido_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scale;
        private System.Windows.Forms.DataGridViewTextBoxColumn Diachi_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThietBi;
        private System.Windows.Forms.DataGridViewTextBoxColumn ten;
        public System.Windows.Forms.DataGridView dgvHienThiDuLieu;
    }
}