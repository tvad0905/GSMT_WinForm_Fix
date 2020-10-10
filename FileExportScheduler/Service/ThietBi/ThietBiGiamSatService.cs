using FileExportScheduler.Models;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.ThietBi
{
    public static class ThietBiGiamSatService
    {
        public static KeyValuePair<string, ThietBiModel> SetTrangThaiBad(KeyValuePair<string, ThietBiModel> deviceUnit)
        {
            foreach (KeyValuePair<string, DiemDoModel> diemDo in deviceUnit.Value.dsDiemDoGiamSat)
            {
                foreach (KeyValuePair<string, DuLieuModel> dulieu in diemDo.Value.DsDulieu)
                {
                    dulieu.Value.ThoiGianDocGiuLieu = DateTime.Now;
                }
            }
            deviceUnit.Value.TrangThaiTinHieu = Constant.TrangThaiKetNoi.Bad;

            return deviceUnit;
        }
        public static Dictionary<string, ThietBiModel> GetDsThietBi()
        {
            Dictionary<string, ThietBiModel> dsThietBiGiamSat = new Dictionary<string, ThietBiModel>();
            try
            {

                var path = GetPathJson.getPathConfig("DeviceAndData.json");
                JObject jsonObj = JObject.Parse(File.ReadAllText(path));
                Dictionary<string, ThietBiTCPIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiTCPIP>>();
                foreach (var deviceIPUnit in deviceIP)
                {
                    if (deviceIPUnit.Value.Protocol == "Modbus TCP/IP" || deviceIPUnit.Value.Protocol == "Siemens S7-1200")
                    {
                        dsThietBiGiamSat.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                    }
                }
                Dictionary<string, ThietBiCOM> deviceCom = jsonObj.ToObject<Dictionary<string, ThietBiCOM>>();
                foreach (var deviceComUnit in deviceCom)
                {
                    if (deviceComUnit.Value.Protocol == "Serial Port")
                    {
                        dsThietBiGiamSat.Add(deviceComUnit.Key, deviceComUnit.Value);
                    }
                }
            }
            catch { }
            return dsThietBiGiamSat;
        }
    }
}
