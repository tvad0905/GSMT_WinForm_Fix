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
            string dongLoi="";
            var dsLoi= ThongBaoLoi.DsThongBaoLoi.Distinct().ToList();
            foreach(string loi in dsLoi)
            {
                dongLoi += loi + ",";
            }
            if(dongLoi == "")
            {
                dongLoi = ThongBaoLoi.HoatDongBinhThuong;
            }
            return dongLoi;
        }
    }
}
