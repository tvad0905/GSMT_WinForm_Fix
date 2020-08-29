using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Constant
{
    public static class ThongBaoLoi
    {
        public static readonly string KhongKetNoi = " Tồn tại thiết bị không có kết nối";
        public static readonly string VuotQuaDuLieu = " Số lượng bản ghi cần đọc vượt quá ";
        public static readonly string DiaChiKhongTonTai = " Tồn tại địa chỉ không có dữ liệu ";
        public static readonly string KhongCoTinHieuTraVe = "Không có tín hiệu trả về";

        public static List<string> DsThongBaoLoi = new List<string>();
    }
}
