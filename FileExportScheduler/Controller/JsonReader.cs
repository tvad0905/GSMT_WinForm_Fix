﻿using FileExportScheduler.Models;
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
        /// <param name="dsThietBiGiamSat">danh sách thiết bị</param>
        /// <param name="dsDiemDo">danh sách điểm đo</param>
        /// <returns>danh sách đường dẫn theo điểm đo</returns>
        public static List<string> LayDsDuongDanTheoTenDiemDo(Dictionary<string, ThietBiGiamSat> dsThietBiGiamSat)
        {
            List<string> dsDuongDanTheoTenThietBi = new List<string>();//

            var path = GetPathJson.getPathConfig("Config.json");
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
                               "\\" + $"log({diemDo}){ DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss")}.csv";
                            dsDuongDanTheoTenThietBi.Add(filePath);
                        }
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
            Dictionary<string, ThietBiGiamSat> dsThietBi = new Dictionary<string, ThietBiGiamSat>();
            try
            {
                var pathData = GetPathJson.getPathConfig("DeviceAndData.json");
                dsThietBi.Clear();
                JObject jsonObj = JObject.Parse(File.ReadAllText(pathData));
                Dictionary<string, ThietBiIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiIP>>();
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