﻿using FileExportScheduler.Models;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExportScheduler.Service.DiemDo
{
    public static class DanhSachDiemDoService
    {
        public static Dictionary<string, DiemDoModel> LayDsDiemDoTuDgv(DataGridView dgvDataProtocol)
        {
            Dictionary<string, DiemDoModel> dsDiemDoGiamSat = new Dictionary<string, DiemDoModel>();
            foreach (DataGridViewRow dr in dgvDataProtocol.Rows)
            {
                if (dr.Index == dgvDataProtocol.Rows.Count - 1)
                {
                    break;
                }

                DuLieuModel duLieu = new DuLieuModel();
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
                        new DiemDoModel(
                            duLieu.DiemDo,
                            new Dictionary<string, DuLieuModel>()
                            )
                    );
                    dsDiemDoGiamSat[duLieu.DiemDo].DsDulieu.Add(duLieu.Ten, duLieu);
                }
            }
            return dsDiemDoGiamSat;
        }
    }
}
