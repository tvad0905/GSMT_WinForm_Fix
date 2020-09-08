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
            this.dgvHienThiDuLieu = new System.Windows.Forms.DataGridView();
            this.ten = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThietBi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Diachi_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Scale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Donvido_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHienThiDuLieu)).BeginInit();
            this.SuspendLayout();
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
            this.dgvHienThiDuLieu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHienThiDuLieu.Location = new System.Drawing.Point(0, 0);
            this.dgvHienThiDuLieu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvHienThiDuLieu.Name = "dgvHienThiDuLieu";
            this.dgvHienThiDuLieu.RowHeadersWidth = 51;
            this.dgvHienThiDuLieu.RowTemplate.Height = 24;
            this.dgvHienThiDuLieu.Size = new System.Drawing.Size(1460, 632);
            this.dgvHienThiDuLieu.TabIndex = 4;
            // 
            // ten
            // 
            this.ten.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ten.DataPropertyName = "ten";
            this.ten.HeaderText = "Tên";
            this.ten.MinimumWidth = 50;
            this.ten.Name = "ten";
            // 
            // ThietBi
            // 
            this.ThietBi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThietBi.DataPropertyName = "DiemDo";
            this.ThietBi.HeaderText = "Điểm đo";
            this.ThietBi.MinimumWidth = 6;
            this.ThietBi.Name = "ThietBi";
            // 
            // Diachi_P
            // 
            this.Diachi_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Diachi_P.DataPropertyName = "Diachi";
            this.Diachi_P.HeaderText = "Địa chỉ";
            this.Diachi_P.MinimumWidth = 50;
            this.Diachi_P.Name = "Diachi_P";
            // 
            // Scale
            // 
            this.Scale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Scale.DataPropertyName = "Scale";
            this.Scale.HeaderText = "Scale";
            this.Scale.MinimumWidth = 6;
            this.Scale.Name = "Scale";
            // 
            // Donvido_P
            // 
            this.Donvido_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Donvido_P.DataPropertyName = "Donvido";
            this.Donvido_P.HeaderText = "Đơn vị đo";
            this.Donvido_P.MinimumWidth = 50;
            this.Donvido_P.Name = "Donvido_P";
            // 
            // FormHienThiDuLieu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1460, 632);
            this.Controls.Add(this.dgvHienThiDuLieu);
            this.Name = "FormHienThiDuLieu";
            this.Text = "FormHienThiDuLieu";
            this.Load += new System.EventHandler(this.FormHienThiDuLieu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHienThiDuLieu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvHienThiDuLieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn ten;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThietBi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Diachi_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scale;
        private System.Windows.Forms.DataGridViewTextBoxColumn Donvido_P;
    }
}