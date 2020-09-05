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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            //sfd.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            sfd.FileName = "DataLog.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                txtExportFilePath.Text = sfd.FileName;
            }
            else
            {
                txtExportFilePath.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CaiDatChung settingModel = new CaiDatChung();

            settingModel.AutoRun = chkAutoRun.Checked;
            settingModel.Interval = Int32.Parse(udInterval.Value.ToString());
            settingModel.ExportFilePath = txtExportFilePath.Text;
            settingModel.ChuKiXoaDuLieu = Int32.Parse(numChuKiXoaDuLieu.Value.ToString());

            var path = GetPathJson.getPathConfig("Config.json");

            if (txtExportFilePath.Text == "")
            {
                errorProvider1.SetError(txtExportFilePath, "Không được để trống !");
                return;
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                var loadData = JsonConvert.SerializeObject(settingModel);
                sw.WriteLine(loadData);
            }

            MessageBox.Show("Đã lưu thành công!");
            this.Close();
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
                    udInterval.Value = export.Interval;
                    txtExportFilePath.Text = export.ExportFilePath;
                    chkAutoRun.Checked = export.AutoRun;
                    numChuKiXoaDuLieu.Value = export.ChuKiXoaDuLieu;
                }
            }
        }
    }
}
