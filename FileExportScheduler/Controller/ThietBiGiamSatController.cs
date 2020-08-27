using FileExportScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    public  static class ThietBiGiamSatController
    {
        public static KeyValuePair<string, ThietBiGiamSat> SetTrangThaiBad(KeyValuePair<string, ThietBiGiamSat> deviceUnit)
        {
            foreach (KeyValuePair<string, DiemDoGiamSat> diemDo in deviceUnit.Value.dsDiemDoGiamSat)
            {
                foreach (KeyValuePair<string, DuLieuGiamSat> dulieu in diemDo.Value.DsDulieu)
                {
                    dulieu.Value.TrangThaiTinHieu = "Bad";
                    dulieu.Value.ThoiGianDocGiuLieu = DateTime.Now;
                }
            }
            return deviceUnit;
        }
    }
}
