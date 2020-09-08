using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.DuLieu
{
    class DanhSachDuLieuService
    {
        public static List<DuLieuGiamSatModel> GetDsDuLieuCuaTatCaThietBi(Dictionary<string, ThietBiGiamSatModel> dsThietBi)
        {
            List<DuLieuGiamSatModel> dsDuLieuGiamSatCuaTatCaThietBi = new List<DuLieuGiamSatModel>();
            foreach (KeyValuePair<string, ThietBiGiamSatModel> thietBi in dsThietBi)
            {
                foreach (KeyValuePair<string, DiemDoGiamSat> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {
                    foreach (KeyValuePair<string, DuLieuGiamSatModel> dulieu in diemDo.Value.DsDulieu)
                    {
                        dsDuLieuGiamSatCuaTatCaThietBi.Add(dulieu.Value);
                    }
                }
            }
            return dsDuLieuGiamSatCuaTatCaThietBi;
        }
    }
}
