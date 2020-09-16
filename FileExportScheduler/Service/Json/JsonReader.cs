using FileExportScheduler.Constant;
using FileExportScheduler.Models;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.Json
{
    public static class JsonReader
    {

        /// <summary>
        /// lấy thời gian giữa các lần ghi
        /// </summary>
        /// <returns>thời gian giữa các lần ghi</returns>
        public static int GetTimeInterval()
        {
            int timeInterval = 1;
            var path = GetPathJson.getPathConfig("Config.json");
            using (System.IO.StreamReader sr = File.OpenText(path))
            {
                var obj = sr.ReadToEnd();
                CaiDatChung export = JsonConvert.DeserializeObject<CaiDatChung>(obj.ToString());
                timeInterval = export.Interval * 60000;
            }
            return timeInterval;
        }

        /// <summary>
        /// lấy thời gian để xóa File cũ
        /// </summary>
        /// <returns>thời gian giữa các lần ghi</returns>
        public static int LayThoiGianXoaFile()
        {
            int thoiGianXoa = 1;
            var path = GetPathJson.getPathConfig("Config.json");
            using (System.IO.StreamReader sr = File.OpenText(path))
            {
                var obj = sr.ReadToEnd();
                CaiDatChung export = JsonConvert.DeserializeObject<CaiDatChung>(obj.ToString());
                thoiGianXoa = export.ChuKiXoaDuLieu;
            }
            return thoiGianXoa;
        }

        /// <summary>
        /// lấy danh sách đường dẫn theo điểm đo
        /// </summary>
        /// <param name="dsThietBiGiamSat">danh sách thiết bị</param>
        /// <param name="dsDiemDo">danh sách điểm đo</param>
        /// <returns>danh sách đường dẫn theo điểm đo</returns>
        public static List<string> LayDsDuongDanTheoTenDiemDo(Dictionary<string, ThietBiModel> dsThietBiGiamSat)
        {
            List<string> dsDuongDanTheoTenThietBi = new List<string>();//

            var path = GetPathJson.getPathConfig("Config.json");
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    var obj = sr.ReadToEnd();
                    CaiDatChung export = JsonConvert.DeserializeObject<CaiDatChung>(obj.ToString());
                    foreach (KeyValuePair<string, ThietBiModel> thietbi in dsThietBiGiamSat)
                    {
                        foreach (KeyValuePair<string, DiemDoModel> diemDo in thietbi.Value.dsDiemDoGiamSat)
                        {
                            string filePath = export.ExportFilePath +
                               "\\" + $"log({diemDo.Value.TenDiemDo}){ DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss")}.csv";
                            dsDuongDanTheoTenThietBi.Add(filePath);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return dsDuongDanTheoTenThietBi;
        }
        /// <summary>
        /// -add danh sách static ThongBaoLoi Thiết bị với lỗi null
        /// -trả về danh sách thông số cho thiêt bị
        /// </summary>
        /// <returns>danh sách thông số cho thiêt bị</returns>
        public static Dictionary<string, ThietBiModel> LayDanhSachThongSoCuaTungThietBi()
        {
            Dictionary<string, ThietBiModel> dsThietBi = new Dictionary<string, ThietBiModel>();
            try
            {
                var pathData = GetPathJson.getPathConfig("DeviceAndData.json");
                dsThietBi.Clear();
                JObject jsonObj = JObject.Parse(File.ReadAllText(pathData));
                Dictionary<string, ThietBiTCPIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiTCPIP>>();
                foreach (var deviceIPUnit in deviceIP)
                {
                    if (deviceIPUnit.Value.Protocol == "Modbus TCP/IP" || deviceIPUnit.Value.Protocol == "Siemens S7-1200")
                    {
                        dsThietBi.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                        ThongBaoLoi.DanhSach.Add(deviceIPUnit.Key, new List<string>());
                    }
                }
                Dictionary<string, ThietBiCOM> deviceCom = jsonObj.ToObject<Dictionary<string, ThietBiCOM>>();
                foreach (var deviceComUnit in deviceCom)
                {
                    if (deviceComUnit.Value.Protocol == "Serial Port")
                    {
                        
                        dsThietBi.Add(deviceComUnit.Key, deviceComUnit.Value);
                        ThongBaoLoi.DanhSach.Add(deviceComUnit.Key, new List<string>());
                    }
                }
            }
            catch{}
            return dsThietBi;
        }

        public static string DuongDanThuLog()
        {
            var path = GetPathJson.getPathConfig("Config.json");
            using (StreamReader sr = File.OpenText(path))
            {
                var obj = sr.ReadToEnd();
                CaiDatChung export = JsonConvert.DeserializeObject<CaiDatChung>(obj.ToString());
                return export.ExportFilePath;
            }
        }
    }
}
