using FileExportScheduler.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
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
                SettingModel export = JsonConvert.DeserializeObject<SettingModel>(obj.ToString());
                timeInterval = export.Interval * 60000;
            }
            return timeInterval;
        }
        /// <summary>
        /// lấy danh sách đường dẫn theo điểm đo
        /// </summary>
        /// <param name="deviceDic">danh sách thiết bị</param>
        /// <param name="dsDiemDo">danh sách điểm đo</param>
        /// <returns>danh sách đường dẫn theo điểm đo</returns>
        public static List<string> LayDsDuongDanTheoTenDiemDo(Dictionary<string, ThietBiGiamSat> deviceDic, ref Dictionary<String, List<DuLieuGiamSat>> dsDiemDo)
        {
            List<string> dsDuongDanTheoTenThietBi = new List<string>();//
            
            var path = GetPathJson.getPathConfig("Config.json");
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    var obj = sr.ReadToEnd();
                    SettingModel export = JsonConvert.DeserializeObject<SettingModel>(obj.ToString());
                    dsDiemDo.Clear();
                    foreach (KeyValuePair<string, ThietBiGiamSat> deviceUnit in deviceDic)
                    {
                        foreach (KeyValuePair<string, DuLieuGiamSat> duLieuUnit in deviceUnit.Value.ListDuLieuChoTungPLC)
                        {
                            string ThietBi = duLieuUnit.Value.ThietBi;
                            if (dsDiemDo.ContainsKey(ThietBi))
                            {
                                dsDiemDo[ThietBi].Add(duLieuUnit.Value);
                            }
                            else
                            {
                                dsDiemDo.Add(ThietBi, new List<DuLieuGiamSat>());
                                dsDiemDo[ThietBi].Add(duLieuUnit.Value);
                            }
                        }
                    }
                    foreach (KeyValuePair<String, List<DuLieuGiamSat>> dsDiemDoUnit in dsDiemDo)
                    {
                        string filePath = export.ExportFilePath.Substring(0, export.ExportFilePath.LastIndexOf("\\")) +
                               "\\" + $"log({dsDiemDoUnit.Key}){ DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss")}.csv";
                        dsDuongDanTheoTenThietBi.Add(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return dsDuongDanTheoTenThietBi;
        }
        public static Dictionary<string, ThietBiGiamSat> LayDanhSachThongSoCuaTungThietBi()
        {
            Dictionary<string, ThietBiGiamSat> dsThietBi=new Dictionary<string, ThietBiGiamSat>();
            try
            {
                var pathData = GetPathJson.getPathConfig("DeviceAndData.json");
                dsThietBi.Clear();
                JObject jsonObj = JObject.Parse(File.ReadAllText(pathData));
                Dictionary<string, IPConfigModel> deviceIP = jsonObj.ToObject<Dictionary<string, IPConfigModel>>();
                foreach (var deviceIPUnit in deviceIP)
                {
                    if (deviceIPUnit.Value.Protocol == "Modbus TCP/IP" || deviceIPUnit.Value.Protocol == "Siemens S7-1200")
                    {
                        dsThietBi.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                    }
                }
                Dictionary<string, ComConfigModel> deviceCom = jsonObj.ToObject<Dictionary<string, ComConfigModel>>();
                foreach (var deviceComUnit in deviceCom)
                {
                    if (deviceComUnit.Value.Protocol == "Serial Port")
                    {
                        dsThietBi.Add(deviceComUnit.Key, deviceComUnit.Value);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dsThietBi;

        }
    }
}
