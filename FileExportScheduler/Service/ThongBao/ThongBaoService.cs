using FileExportScheduler.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.ThongBao
{
    public static class ThongBaoService
    {
        public static string DsLoi()
        {
            string dongLoi = "";
            var dsLoi = ThongBaoLoi.DsThongBaoLoi.Distinct().ToList();
            if (ThongBaoLoi.TrangThaiHoatDong == EnumTrangThaiHoatDong.CoLoi)
            {

                foreach (string loi in dsLoi)
                {
                    dongLoi += loi + ", ";
                }
                dongLoi = dongLoi.Remove(dongLoi.Length - 2);

            }
            else if(ThongBaoLoi.TrangThaiHoatDong==EnumTrangThaiHoatDong.KhongCoLoi)
            {
                dongLoi = ThongBaoLoi.HoatDongBinhThuong;
            }

            return dongLoi;
        }
    }
}
