using FileExportScheduler.Models;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    public  static class ThietBiGiamSatService
    {
        public static KeyValuePair<string, ThietBiGiamSat> SetTrangThaiBad(KeyValuePair<string, ThietBiGiamSat> deviceUnit)
        {
            foreach (KeyValuePair<string, DiemDoGiamSat> diemDo in deviceUnit.Value.dsDiemDoGiamSat)
            {
                foreach (KeyValuePair<string, DuLieuGiamSat> dulieu in diemDo.Value.DsDulieu)
                {
                    dulieu.Value.TrangThaiTinHieu = Constant.TrangThaiKetNoi.Bad;
                    dulieu.Value.ThoiGianDocGiuLieu = DateTime.Now;
                }
            }
            return deviceUnit;
        }
    }
}
