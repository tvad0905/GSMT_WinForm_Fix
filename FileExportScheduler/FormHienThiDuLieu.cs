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
        static DataTable dt;

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
            dt = ConvertThongSoGiaTriToDataTable(DanhSachDuLieuService.GetThongSoGiaTriCuaTatCaThietBi(dsThietBi));
            dgvHienThiDuLieu.DataSource = dt;
            dgvHienThiDuLieu.AutoGenerateColumns = false;
            tmrHienThongSoSuLieu.Interval = 500;
            tmrHienThongSoSuLieu.Enabled = true;
            tmrHienThongSoSuLieu.Start();

        }

        private void tmrHienThongSoSuLieu_Tick(object sender, EventArgs e)
        {
            try
            {
                var tempTable = ConvertThongSoGiaTriToDataTable(DanhSachDuLieuService.GetThongSoGiaTriCuaTatCaThietBi(dsThietBi));
                foreach (var dr in tempTable.AsEnumerable())
                {
                    dt.LoadDataRow(dr.ItemArray, LoadOption.OverwriteChanges);
                }
                dt.LoadDataRow(tempTable.Rows[0].ItemArray, LoadOption.OverwriteChanges);
            }
            catch (Exception ex)
            {

            }
        }

        private static DataTable ConvertThongSoGiaTriToDataTable(List<Models.DuLieu.ThongSoGiaTriModel> data)
        {
            var table = new DataTable();
            try
            {
                table.Columns.Add("Ten", typeof(string));
                table.Columns.Add("DiemDo", typeof(string));
                table.Columns.Add("ThietBi", typeof(string));
                table.Columns.Add("DiaChi", typeof(string));
                table.Columns.Add("GiaTri", typeof(double));
                table.Columns.Add("TrangThaiTinHieu", typeof(string));
                table.PrimaryKey = new[] { table.Columns["Ten"] ,table.Columns["DiemDo"]};
                

                foreach (Models.DuLieu.ThongSoGiaTriModel thongSoGiaTri in data)
                {
                    DataRow dr = table.NewRow();
                    dr["Ten"] = thongSoGiaTri.Ten;
                    dr["DiemDo"] = thongSoGiaTri.DiemDo;
                    dr["ThietBi"] = thongSoGiaTri.ThietBi;
                    dr["DiaChi"] = thongSoGiaTri.DiaChi;
                    dr["GiaTri"] = Math.Round((Convert.ToDouble(thongSoGiaTri.GiaTri) / Convert.ToDouble(thongSoGiaTri.Scale)), 2).ToString();
                    dr["TrangThaiTinHieu"] = thongSoGiaTri.TrangThaiTinHieu;
                    table.Rows.Add(dr);
                }

            }
            catch (Exception ex)
            {

            }
            return table;
        }
        private void FormHienThiDuLieu_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
