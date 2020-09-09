using FileExportScheduler.Models;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.DuLieu;
using FileExportScheduler.Service.Json;
using FileExportScheduler.Service.ThietBi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExportScheduler
{
    public partial class FormHienThiDuLieu : Form
    {
        Dictionary<string, ThietBiModel> dsThietBi;
        public FormHienThiDuLieu(Dictionary<string, ThietBiModel> dsThietBi)
        {
            InitializeComponent();
            this.dsThietBi = dsThietBi;
        }


        private void FormHienThiDuLieu_Load(object sender, EventArgs e)
        {
            dgvHienThiDuLieu.AutoGenerateColumns = false;
            try
            {
                var bindingSource = new BindingSource();
                bindingSource.DataSource = DanhSachDuLieuService.GetDsDuLieuCuaTatCaThietBi(dsThietBi);

                for (int i = 0; i < 10; i++)
                {
                    bindingSource.DataSource = null;
                    bindingSource.DataSource = bindingSource.DataSource = DanhSachDuLieuService.GetDsDuLieuCuaTatCaThietBi(dsThietBi);
                    dgvHienThiDuLieu.DataSource = bindingSource;

                    System.Threading.Thread.Sleep(5000);
                }
            }
            catch
            { }
        }
    }
}
