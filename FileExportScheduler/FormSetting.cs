using FileExportScheduler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using FileExportScheduler.Service.Json;
using FileExportScheduler.Service.KiemTra;

namespace FileExportScheduler
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtExportFilePath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CaiDatChung setting = new CaiDatChung();

            setting.AutoRun = chkAutoRun.Checked;
            setting.Interval = Int32.Parse(numChukyLuuDuLieu.Value.ToString());
            setting.ExportFilePath = txtExportFilePath.Text;
            setting.ChuKiXoaDuLieu = Int32.Parse(numChuKiXoaDuLieu.Value.ToString());

            var path = GetPathJson.getPathConfig("Config.json");

            if (txtExportFilePath.Text == "")
            {
                errorProvider1.SetError(txtExportFilePath, "Không được để trống !");
                return;
            }
            if (KiemTraDuongDan.TonTaiKhiLuu(txtExportFilePath.Text,setting))
            {
                MessageBox.Show("Đã lưu thành công!", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Đường dẫn thư mục không tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {

            var path = GetPathJson.getPathConfig("Config.json");

            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    var obj = sr.ReadToEnd();
                    CaiDatChung export = JsonConvert.DeserializeObject<CaiDatChung>(obj.ToString());
                    numChukyLuuDuLieu.Value = export.Interval;
                    txtExportFilePath.Text = export.ExportFilePath;
                    chkAutoRun.Checked = export.AutoRun;
                    numChuKiXoaDuLieu.Value = export.ChuKiXoaDuLieu;
                }
            }
        }

        
    }
}
