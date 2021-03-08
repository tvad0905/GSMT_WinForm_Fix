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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHienThiDuLieu));
            this.tmrHienThongSoSuLieu = new System.Windows.Forms.Timer(this.components);
            this.dgvHienThiDuLieu = new System.Windows.Forms.DataGridView();
            this.ten = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThietBi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Diachi_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiaChi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Donvido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GiaTri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrangThaiTinHieu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_thietBi = new System.Windows.Forms.ComboBox();
            this.cb_slaveAddress = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHienThiDuLieu)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrHienThongSoSuLieu
            // 
            this.tmrHienThongSoSuLieu.Tick += new System.EventHandler(this.tmrHienThongSoSuLieu_Tick);
            // 
            // dgvHienThiDuLieu
            // 
            this.dgvHienThiDuLieu.AllowUserToDeleteRows = false;
            this.dgvHienThiDuLieu.AllowUserToResizeColumns = false;
            this.dgvHienThiDuLieu.AllowUserToResizeRows = false;
            this.dgvHienThiDuLieu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHienThiDuLieu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ten,
            this.ThietBi,
            this.Diachi_P,
            this.DiaChi,
            this.Donvido,
            this.GiaTri,
            this.TrangThaiTinHieu});
            this.dgvHienThiDuLieu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvHienThiDuLieu.Location = new System.Drawing.Point(0, 75);
            this.dgvHienThiDuLieu.Margin = new System.Windows.Forms.Padding(2);
            this.dgvHienThiDuLieu.Name = "dgvHienThiDuLieu";
            this.dgvHienThiDuLieu.ReadOnly = true;
            this.dgvHienThiDuLieu.RowHeadersWidth = 51;
            this.dgvHienThiDuLieu.RowTemplate.Height = 24;
            this.dgvHienThiDuLieu.Size = new System.Drawing.Size(1067, 370);
            this.dgvHienThiDuLieu.TabIndex = 4;
            // 
            // ten
            // 
            this.ten.DataPropertyName = "ten";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ten.DefaultCellStyle = dataGridViewCellStyle1;
            this.ten.HeaderText = "Tên";
            this.ten.MinimumWidth = 50;
            this.ten.Name = "ten";
            this.ten.ReadOnly = true;
            this.ten.Width = 125;
            // 
            // ThietBi
            // 
            this.ThietBi.DataPropertyName = "DiemDo";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ThietBi.DefaultCellStyle = dataGridViewCellStyle2;
            this.ThietBi.HeaderText = "Điểm đo";
            this.ThietBi.MinimumWidth = 6;
            this.ThietBi.Name = "ThietBi";
            this.ThietBi.ReadOnly = true;
            this.ThietBi.Width = 250;
            // 
            // Diachi_P
            // 
            this.Diachi_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Diachi_P.DataPropertyName = "ThietBi";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Diachi_P.DefaultCellStyle = dataGridViewCellStyle3;
            this.Diachi_P.HeaderText = "Thiết bị";
            this.Diachi_P.MinimumWidth = 50;
            this.Diachi_P.Name = "Diachi_P";
            this.Diachi_P.ReadOnly = true;
            // 
            // DiaChi
            // 
            this.DiaChi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DiaChi.DataPropertyName = "DiaChi";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DiaChi.DefaultCellStyle = dataGridViewCellStyle4;
            this.DiaChi.HeaderText = "Địa chỉ";
            this.DiaChi.MinimumWidth = 6;
            this.DiaChi.Name = "DiaChi";
            this.DiaChi.ReadOnly = true;
            // 
            // Donvido
            // 
            this.Donvido.DataPropertyName = "DonViDo";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Donvido.DefaultCellStyle = dataGridViewCellStyle5;
            this.Donvido.HeaderText = "Đơn Vị Đo";
            this.Donvido.Name = "Donvido";
            this.Donvido.ReadOnly = true;
            // 
            // GiaTri
            // 
            this.GiaTri.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GiaTri.DataPropertyName = "GiaTri";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.GiaTri.DefaultCellStyle = dataGridViewCellStyle6;
            this.GiaTri.HeaderText = "Giá Trị";
            this.GiaTri.MinimumWidth = 6;
            this.GiaTri.Name = "GiaTri";
            this.GiaTri.ReadOnly = true;
            // 
            // TrangThaiTinHieu
            // 
            this.TrangThaiTinHieu.DataPropertyName = "TrangThaiTinHieu";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.TrangThaiTinHieu.DefaultCellStyle = dataGridViewCellStyle7;
            this.TrangThaiTinHieu.HeaderText = "Trạng Thái Tín Hiệu";
            this.TrangThaiTinHieu.MinimumWidth = 50;
            this.TrangThaiTinHieu.Name = "TrangThaiTinHieu";
            this.TrangThaiTinHieu.ReadOnly = true;
            this.TrangThaiTinHieu.Width = 130;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Thiết bị : ";
            // 
            // cb_thietBi
            // 
            this.cb_thietBi.FormattingEnabled = true;
            this.cb_thietBi.Location = new System.Drawing.Point(130, 25);
            this.cb_thietBi.Name = "cb_thietBi";
            this.cb_thietBi.Size = new System.Drawing.Size(121, 21);
            this.cb_thietBi.TabIndex = 6;
            // 
            // cb_slaveAddress
            // 
            this.cb_slaveAddress.FormattingEnabled = true;
            this.cb_slaveAddress.Location = new System.Drawing.Point(414, 25);
            this.cb_slaveAddress.Name = "cb_slaveAddress";
            this.cb_slaveAddress.Size = new System.Drawing.Size(121, 21);
            this.cb_slaveAddress.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(305, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Slave Address : ";
            // 
            // FormHienThiDuLieu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 445);
            this.Controls.Add(this.cb_slaveAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_thietBi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvHienThiDuLieu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormHienThiDuLieu";
            this.Text = "Realtime Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHienThiDuLieu_FormClosing);
            this.Load += new System.EventHandler(this.FormHienThiDuLieu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHienThiDuLieu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tmrHienThongSoSuLieu;
        public System.Windows.Forms.DataGridView dgvHienThiDuLieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn ten;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThietBi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Diachi_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiaChi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Donvido;
        private System.Windows.Forms.DataGridViewTextBoxColumn GiaTri;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrangThaiTinHieu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_thietBi;
        private System.Windows.Forms.ComboBox cb_slaveAddress;
        private System.Windows.Forms.Label label2;
    }
}