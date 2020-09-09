using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.DuLieu
{
    class DanhSachDuLieuService
    {
        public static List<DuLieuModel> GetDsDuLieuCuaTatCaThietBi(Dictionary<string, ThietBiModel> dsThietBi)
        {
            List<DuLieuModel> dsDuLieuGiamSatCuaTatCaThietBi = new List<DuLieuModel>();
            foreach (KeyValuePair<string, ThietBiModel> thietBi in dsThietBi)
            {
                foreach (KeyValuePair<string, DiemDoModel> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {
                    foreach (KeyValuePair<string, DuLieuModel> dulieu in diemDo.Value.DsDulieu)
                    {
                        dsDuLieuGiamSatCuaTatCaThietBi.Add(dulieu.Value);
                    }
                }
            }
            return dsDuLieuGiamSatCuaTatCaThietBi;
        }
        public static List<ThongSoGiaTriModel>GetThongSoGiaTriCuaTatCaThietBi(Dictionary<string, ThietBiModel> dsThietBi)
        {
            List<ThongSoGiaTriModel> dsThongSoGiaTri = new List<ThongSoGiaTriModel>();
            ThongSoGiaTriModel thongSoGiaTriTemp = new ThongSoGiaTriModel();
            foreach (KeyValuePair<string, ThietBiModel> thietBi in dsThietBi)
            {

                foreach (KeyValuePair<string, DiemDoModel> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {
                    string csvData = "[Data]" + "\n" + "Tagname,TimeStamp,Value,DataQuality" + "\n";
                    foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
                    {
                        thongSoGiaTriTemp.ThietBi = thietBi.Value.Name;
                        thongSoGiaTriTemp.DiemDo  = diemDo.Value.TenDiemDo;
                        thongSoGiaTriTemp.Ten = duLieu.Value.Ten;
                        thongSoGiaTriTemp.GiaTri = duLieu.Value.GiaTri;
                        thongSoGiaTriTemp.TrangThaiTinHieu = duLieu.Value.TrangThaiTinHieu;

                        dsThongSoGiaTri.Add(thongSoGiaTriTemp);
                    }
                }
            }

            return dsThongSoGiaTri;
        }
    }
}
