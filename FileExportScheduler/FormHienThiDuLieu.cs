using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.DuLieu;
using System;
using System.Collections.Generic;
using System.Data;
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
            //dgvHienThiDuLieu.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dt = ConvertThongSoGiaTriToDataTable(DanhSachDuLieuService.GetThongSoGiaTriCuaTatCaThietBi(dsThietBi));
            dgvHienThiDuLieu.DataSource = dt;
            dgvHienThiDuLieu.AutoGenerateColumns = false;
            tmrHienThongSoSuLieu.Interval = 1000;
            tmrHienThongSoSuLieu.Enabled = true;
            tmrHienThongSoSuLieu.Start();

        }

        private void tmrHienThongSoSuLieu_Tick(object sender, EventArgs e)
        {
            try
            {
                var dsDuLieu = DanhSachDuLieuService.UpdateThongSoDuLieu(dsThietBi, dt.Rows.Count);
             
                if (dgvHienThiDuLieu.Rows.Count > 0)
                {
                    for (int i = 0; i < dsDuLieu.GetLength(0); i++)
                    {
                        dgvHienThiDuLieu.Rows[i].Cells["GiaTri"].Value = dsDuLieu[i,0];
                        dgvHienThiDuLieu.Rows[i].Cells["TrangThaiTinHieu"].Value = dsDuLieu[i,1];
                    }
                }
                else
                {
                }  
            }
            catch (Exception ex)
            {

            }
        }
        private static DataTable ConvertThongSoGiaTriToDataTable(List<Models.DuLieu.ThongSoGiaTriModel> data)
        {
            var table = new DataTable();
            table.Columns.Add("Ten", typeof(string));
            table.Columns.Add("DiemDo", typeof(string));
            table.Columns.Add("ThietBi", typeof(string));
            table.Columns.Add("DiaChi", typeof(string));
            table.Columns.Add("GiaTri", typeof(string));
            table.Columns.Add("TrangThaiTinHieu", typeof(string));
            table.Columns.Add("DonViDo", typeof(string));
            table.PrimaryKey = new[] { table.Columns["Ten"], table.Columns["DiemDo"] };


            foreach (Models.DuLieu.ThongSoGiaTriModel thongSoGiaTri in data)
            {
                try
                {
                    DataRow dr = table.NewRow();
                    dr["Ten"] = thongSoGiaTri.Ten;
                    dr["DiemDo"] = thongSoGiaTri.DiemDo;
                    dr["ThietBi"] = thongSoGiaTri.ThietBi;
                    dr["DiaChi"] = thongSoGiaTri.DiaChi;
                    dr["DonViDo"] = thongSoGiaTri.DonViDo;
                    if (int.TryParse(thongSoGiaTri.GiaTri, out _))
                    {
                        dr["GiaTri"] = (Convert.ToDouble(thongSoGiaTri.GiaTri) / Convert.ToDouble(thongSoGiaTri.Scale)).ToString();

                    }
                    else
                    {
                        if (thongSoGiaTri.GiaTri != null)
                        {
                            dr["GiaTri"] = (Convert.ToDouble(thongSoGiaTri.GiaTri) / Convert.ToDouble(thongSoGiaTri.Scale)).ToString();

                        }
                        else
                        {
                            dr["GiaTri"] = "0";
                        }
                    }
                    dr["TrangThaiTinHieu"] = thongSoGiaTri.TrangThaiTinHieu;
                    table.Rows.Add(dr);
                }
                catch (NullReferenceException)
                {

                }

            }


            return table;
        }
        private void FormHienThiDuLieu_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
