using FileExportScheduler.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    public static class ThongBaoController
    {
        public static string DsLoi()
        {
            string dongLoi = "";
            var dsLoi = ThongBaoLoi.DsThongBaoLoi.Distinct().ToList();

            if (dsLoi.Count == 1)
            {
                dongLoi += dsLoi[0];
            }
            else if (dsLoi.Count == 0)
            {
                dongLoi = ThongBaoLoi.HoatDongBinhThuong;
            }
            else
            {
                foreach (string loi in dsLoi)
                {
                    dongLoi += loi + ", ";
                }
                dongLoi = dongLoi.Remove(dongLoi.Length - 2);
            }
            return dongLoi;
        }
    }
}
