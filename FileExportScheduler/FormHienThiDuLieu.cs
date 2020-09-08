using FileExportScheduler.Models;
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
        public FormHienThiDuLieu()
        {
            InitializeComponent();
        }


        private void FormHienThiDuLieu_Load(object sender, EventArgs e)
        {

            Dictionary<string, ThietBiGiamSatModel> dsThietBiGiamSat = ThietBiGiamSatService.getDsThietBi();

            dgvHienThiDuLieu.AutoGenerateColumns = false;
            try
            {
                var bindingSource = new BindingSource();
                bindingSource.DataSource = DanhSachDuLieuService.GetDsDuLieuCuaTatCaThietBi(dsThietBiGiamSat);
                dgvHienThiDuLieu.DataSource = bindingSource;
            }
            catch
            { }
        }
    }
}
