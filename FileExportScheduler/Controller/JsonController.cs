using FileExportScheduler.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExportScheduler.Controller
{
    public static class JsonController
    {

        /// <summary>
        /// lấy thời gian giữa các lần ghi
        /// </summary>
        /// <returns>thời gian giữa các lần ghi</returns>
        public static int GetTimeInterval()
        {
            int timeInterval = 1;
            var path = JsonController.getPathConfig("Config.json");
            using (System.IO.StreamReader sr = File.OpenText(path))
            {
                var obj = sr.ReadToEnd();
                SettingModel export = JsonConvert.DeserializeObject<SettingModel>(obj.ToString());
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
            var path = JsonController.getPathConfig("Config.json");
            using (System.IO.StreamReader sr = File.OpenText(path))
            {
                var obj = sr.ReadToEnd();
                SettingModel export = JsonConvert.DeserializeObject<SettingModel>(obj.ToString());
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
        public static List<string> LayDsDuongDanTheoTenDiemDo(Dictionary<string, ThietBiGiamSat> dsThietBiGiamSat)
        {
            List<string> dsDuongDanTheoTenThietBi = new List<string>();//

            var path = JsonController.getPathConfig("Config.json");
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    var obj = sr.ReadToEnd();
                    SettingModel export = JsonConvert.DeserializeObject<SettingModel>(obj.ToString());
                    foreach (KeyValuePair<string, ThietBiGiamSat> thietbi in dsThietBiGiamSat)
                    {
                        foreach (KeyValuePair<string, DiemDoGiamSat> diemDo in thietbi.Value.dsDiemDoGiamSat)
                        {
                            string filePath = export.ExportFilePath.Substring(0, export.ExportFilePath.LastIndexOf("\\")) +
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
        public static Dictionary<string, ThietBiGiamSat> LayDanhSachThongSoCuaTungThietBi()
        {
            Dictionary<string, ThietBiGiamSat> dsThietBi = new Dictionary<string, ThietBiGiamSat>();
            try
            {
                var pathData = JsonController.getPathConfig("DeviceAndData.json");
                dsThietBi.Clear();
                JObject jsonObj = JObject.Parse(File.ReadAllText(pathData));
                Dictionary<string, ThietBiTCPIPModel> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiTCPIPModel>>();
                foreach (var deviceIPUnit in deviceIP)
                {
                    if (deviceIPUnit.Value.Protocol == "Modbus TCP/IP" || deviceIPUnit.Value.Protocol == "Siemens S7-1200")
                    {
                        dsThietBi.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                    }
                }
                Dictionary<string, ThietBiCOMModel> deviceCom = jsonObj.ToObject<Dictionary<string, ThietBiCOMModel>>();
                foreach (var deviceComUnit in deviceCom)
                {
                    if (deviceComUnit.Value.Protocol == "Serial Port")
                    {
                        dsThietBi.Add(deviceComUnit.Key, deviceComUnit.Value);
                    }
                }
            }
            catch 
            {
            }
            return dsThietBi;

        }

        public static string DuongDanThuMucDuLieu()
        {
            var path = JsonController.getPathConfig("Config.json");
            using (StreamReader sr = File.OpenText(path))
            {
                var obj = sr.ReadToEnd();
                SettingModel export = JsonConvert.DeserializeObject<SettingModel>(obj.ToString());
                return export.ExportFilePath.Substring(0, export.ExportFilePath.LastIndexOf("\\"));
            }
        }
        public static string getPathConfig(string fileName)
        {
            return Path.Combine(Application.StartupPath, "Configuration", fileName);
        }

    }
}
