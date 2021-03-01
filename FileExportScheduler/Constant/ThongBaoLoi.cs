using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Constant
{
    public static class ThongBaoLoi
    {
        public static readonly string KhongKetNoi = "Tồn tại thiết bị không có kết nối";
        public static readonly string VuotQuaDuLieu = "Số lượng bản ghi cần đọc vượt quá";
        public static readonly string DiaChiKhongTonTai = "Tồn tại địa chỉ không có dữ liệu";
        public static readonly string KhongCoTinHieuTraVe = "Không có tín hiệu trả về";
        public static readonly string HoatDongBinhThuong = "Hoạt động bình thường";

        /// <summary>
        /// key: tên thiết bị
        /// value: tên lỗi
        /// </summary>
        
        public static Dictionary<string,List<string>> DanhSach = new Dictionary<string,List<string>>();

    }
}
