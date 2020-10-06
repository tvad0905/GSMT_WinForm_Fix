using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.DuLieu
{
    class DanhSachDuLieuService
    {
        public static List<ThongSoGiaTriModel> GetThongSoGiaTriCuaTatCaThietBi(Dictionary<string, ThietBiModel> dsThietBi)
        {
            List<ThongSoGiaTriModel> dsThongSoGiaTri = new List<ThongSoGiaTriModel>();


            foreach (KeyValuePair<string, ThietBiModel> thietBi in dsThietBi)
            {

                foreach (KeyValuePair<string, DiemDoModel> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {

                    foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
                    {
                        ThongSoGiaTriModel thongSoGiaTriTemp = new ThongSoGiaTriModel();
                        thongSoGiaTriTemp.ThietBi = thietBi.Value.Name;
                        thongSoGiaTriTemp.DiemDo = diemDo.Value.TenDiemDo;
                        thongSoGiaTriTemp.Ten = duLieu.Value.Ten;
                        if (int.TryParse(duLieu.Value.GiaTri, out _))
                        {
                            thongSoGiaTriTemp.GiaTri = Convert.ToInt32(duLieu.Value.GiaTri).ToString();
                        }
                        else
                        {
                            thongSoGiaTriTemp.GiaTri = duLieu.Value.GiaTri;
                        }

                        thongSoGiaTriTemp.DiaChi = duLieu.Value.DiaChi;
                        thongSoGiaTriTemp.Scale = duLieu.Value.Scale;
                        dsThongSoGiaTri.Add(thongSoGiaTriTemp);

                        //set max address
                        ushort timDiaChiMax = Convert.ToUInt16(duLieu.Value.DiaChi);
                        if (duLieu.Value.DiaChi.StartsWith("0"))
                        {
                            if (timDiaChiMax > thietBi.Value.MaxAddressCoils)
                            {
                                thietBi.Value.MaxAddressCoils = timDiaChiMax;
                            }
                        }
                        else if (duLieu.Value.DiaChi.StartsWith("1"))
                        {
                            if (timDiaChiMax - 10000 > thietBi.Value.MaxAddressInputs)
                            {
                                thietBi.Value.MaxAddressInputs = (ushort)(timDiaChiMax - 10000);
                            }
                        }
                        else if (duLieu.Value.DiaChi.StartsWith("3"))
                        {
                            if (timDiaChiMax - 30000 > thietBi.Value.MaxAddressInputRegisters)
                            {
                                thietBi.Value.MaxAddressInputRegisters = (ushort)(timDiaChiMax - 30000);
                            }
                        }
                        else if (duLieu.Value.DiaChi.StartsWith("4"))
                        {
                            if (timDiaChiMax - 40000 > thietBi.Value.MaxAddressHoldingRegisters)
                            {
                                thietBi.Value.MaxAddressHoldingRegisters = (ushort)(timDiaChiMax - 40000);
                            }
                        }
                    }
                }
            }


            return dsThongSoGiaTri;
        }
        public static string[,] UpdateThongSoDuLieu(Dictionary<string, ThietBiModel> dsThietBi, int doDai)
        {
            string[,] dsThongSoGiaTri = new string[doDai, 2];

            int row = 0;
            foreach (KeyValuePair<string, ThietBiModel> thietBi in dsThietBi)
            {

                foreach (KeyValuePair<string, DiemDoModel> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {

                    foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
                    {

                        if (int.TryParse(duLieu.Value.GiaTri, out _))
                        {
                            dsThongSoGiaTri[row, 0] = Convert.ToInt32(duLieu.Value.GiaTri).ToString();

                        }
                        else
                        {
                            dsThongSoGiaTri[row, 0] = duLieu.Value.GiaTri;
                        }

                        dsThongSoGiaTri[row, 1] = thietBi.Value.TrangThaiTinHieu;
                        row++;
                    }
                }
            }
            return dsThongSoGiaTri;
        }
    }
}
