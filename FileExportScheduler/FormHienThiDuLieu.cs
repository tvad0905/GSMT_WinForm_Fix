using FileExportScheduler.Models;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.DuLieu;
using FileExportScheduler.Service.Json;
using FileExportScheduler.Service.ThietBi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
        DataTable dt;

        BindingSource bindingSource = new BindingSource();
        public FormHienThiDuLieu()
        {
            InitializeComponent();

        }
        public FormHienThiDuLieu(Dictionary<string, ThietBiModel> dsThietBi)
        {
            InitializeComponent();
            this.dsThietBi = dsThietBi;
        }
        public Dictionary<string, ThietBiModel> DsThietBi
        {
            set { dsThietBi = value; }
            get { return dsThietBi; }
        }

        private void FormHienThiDuLieu_Load(object sender, EventArgs e)
        {
            dt = ToDataTable(DanhSachDuLieuService.GetThongSoGiaTriCuaTatCaThietBi(dsThietBi));
            dgvHienThiDuLieu.DataSource = ToDataTable(DanhSachDuLieuService.GetThongSoGiaTriCuaTatCaThietBi(dsThietBi));
            dgvHienThiDuLieu.AutoGenerateColumns = false;
            tmrHienThongSoSuLieu.Interval = 1000;
            tmrHienThongSoSuLieu.Start();

        }

        private void tmrHienThongSoSuLieu_Tick(object sender, EventArgs e)
        {

            try
            {
                dt = ToDataTable(DanhSachDuLieuService.GetThongSoGiaTriCuaTatCaThietBi(dsThietBi));
                foreach (var dr in dt.AsEnumerable())
                    dt.LoadDataRow(dr.ItemArray, LoadOption.OverwriteChanges);
            }
            catch (Exception ex)
            {
            }
        }

        private DataTable ToDataTable<ThongSoGiaTriModel>(List<ThongSoGiaTriModel> data)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Ten", typeof(string));
            table.Columns.Add("DiemDo", typeof(string));
            table.Columns.Add("ThietBi", typeof(string));
            table.Columns.Add("DiaChi", typeof(string));
            table.Columns.Add("GiaTri", typeof(string));
            table.Columns.Add("TrangThaiTinHieu", typeof(string));

            foreach (ThongSoGiaTriModel thongSoGiaTri in data)
            {
               
                table.Rows.Add(thongSoGiaTri);
            }
            return table;
        }
        private void FormHienThiDuLieu_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
