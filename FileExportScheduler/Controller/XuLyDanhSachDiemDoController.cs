using FileExportScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExportScheduler.Controller
{
    public static class XuLyDanhSachDiemDoController
    {
        public static Dictionary<string, DiemDoGiamSat> LayDsDiemDoTuDgv(DataGridView dgvDataProtocol)
        {
            Dictionary<string, DiemDoGiamSat> dsDiemDoGiamSat = new Dictionary<string, DiemDoGiamSat>();
            foreach (DataGridViewRow dr in dgvDataProtocol.Rows)
            {
                if (dr.Index == dgvDataProtocol.Rows.Count - 1)
                {
                    break;
                }

                DuLieuGiamSat duLieu = new DuLieuGiamSat();
                duLieu.Ten = dr.Cells[0].Value.ToString();
                duLieu.DiemDo = dr.Cells[1].Value.ToString();
                duLieu.DiaChi = dr.Cells[2].Value.ToString();
                duLieu.Scale = dr.Cells[3].Value.ToString();
                duLieu.DonViDo = dr.Cells[4].Value.ToString();

                if (dsDiemDoGiamSat.ContainsKey(duLieu.DiemDo))
                {
                    dsDiemDoGiamSat[duLieu.DiemDo].DsDulieu.Add(duLieu.Ten, duLieu);
                }
                else
                {
                    dsDiemDoGiamSat.Add(
                        duLieu.DiemDo,
                        new DiemDoGiamSat(
                            duLieu.DiemDo,
                            new Dictionary<string, DuLieuGiamSat>()
                            )
                    );
                    dsDiemDoGiamSat[duLieu.DiemDo].DsDulieu.Add(duLieu.Ten, duLieu);
                }
            }
            return dsDiemDoGiamSat;
        }
    }
}
