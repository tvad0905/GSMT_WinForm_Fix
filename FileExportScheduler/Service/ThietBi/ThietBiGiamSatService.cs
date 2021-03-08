using ESProtocolConverter.Models.NhaMay;
using ESProtocolConverter.Models.Slave;
using ESProtocolConverter.Service.Json;
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
            foreach (KeyValuePair<string, SlaveModel> slave in deviceUnit.Value.dsSlave)
            {
                foreach (KeyValuePair<string, DiemDoModel> diemDo in slave.Value.dsDiemDoGiamSat)
                {
                    foreach (KeyValuePair<string, DuLieuModel> dulieu in diemDo.Value.DsDulieu)
                    {
                        dulieu.Value.ThoiGianDocGiuLieu = DateTime.Now;
                    }
                }
                deviceUnit.Value.TrangThaiTinHieu = Constant.TrangThaiKetNoi.Bad;
            }
            return deviceUnit;
        }
        public static Dictionary<string, ThietBiModel> GetDsThietBi(string nhaMay_name)
        {
            Dictionary<string, ThietBiModel> dsThietBiGiamSat = new Dictionary<string, ThietBiModel>();
            try
            {

                Dictionary<string, NhaMayModel> dicNhaMay = JsonService.GetDicNhaMay();
                foreach (var nhaMay_item in dicNhaMay)
                {
                    if (nhaMay_item.Value.Name == nhaMay_name)
                    {
                        dsThietBiGiamSat = nhaMay_item.Value.dsThietBi;
                    }
                }

                /*Dictionary<string, ThietBiTCPIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiTCPIP>>();
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
                }*/
            }
            catch { }
            return dsThietBiGiamSat;
        }

        public static ThietBiModel GetThietBiGiamSat(string nhamay_name, string thietBi_name)
        {
            try
            {

                Dictionary<string, NhaMayModel> dicNhaMay = JsonService.GetDicNhaMay();
                foreach (var nhaMay_item in dicNhaMay)
                {
                    if (nhaMay_item.Value.Name == nhamay_name)
                    {
                        foreach (var thietBi_item in nhaMay_item.Value.dsThietBi)
                        {
                            if (thietBi_item.Value.Name == thietBi_name)
                            {
                                return thietBi_item.Value;
                            }
                        }
                    }
                }

                /*Dictionary<string, ThietBiTCPIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiTCPIP>>();
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
                }*/
            }
            catch
            {
                return null;
            }
            return null;
        }

        public static SlaveModel GetDsSlave(string nhamay_name, string thietbi_name, string slave_name)
        {
            try
            {
                Dictionary<string, NhaMayModel> dicNhaMay = JsonService.GetDicNhaMay();

                if (dicNhaMay.ContainsKey(nhamay_name))
                {
                    NhaMayModel nhaMayModel = dicNhaMay[nhamay_name];
                    var nhamay_item = nhaMayModel.dsThietBi;
                    foreach (var thietBi_item in nhamay_item)
                    {
                        if (thietBi_item.Value.Name == thietbi_name)
                        {
                            foreach (var slave_item in thietBi_item.Value.dsSlave)
                            {
                                if (slave_item.Value.Name == slave_name)
                                {
                                    return slave_item.Value;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}
