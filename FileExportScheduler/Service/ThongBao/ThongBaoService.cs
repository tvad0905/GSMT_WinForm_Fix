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
            var dsLoi =new List<string>();

            foreach(KeyValuePair<string,List<string>> loiCua1ThietBi in ThongBaoLoi.DanhSach)
            {
                dsLoi.AddRange( loiCua1ThietBi.Value);
            }
            dsLoi.Distinct().ToList();

            if (dsLoi.Count>0)
            {

                foreach (string loi in dsLoi)
                {
                    dongLoi += loi + ", ";
                }
                dongLoi = dongLoi.Remove(dongLoi.Length - 2);

            }
            else if(dsLoi.Count==0)
            {
                dongLoi = ThongBaoLoi.HoatDongBinhThuong;
            }

            return dongLoi;
        }
    }
}
