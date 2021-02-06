using ESProtocolConverter.Models.NhaMay;
using ESProtocolConverter.Models.Slave;
using FileExportScheduler.Models;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ESProtocolConverter.Service.Json
{
    public static class JsonService
    {
        public static void ReplaceOldTbByNewTb(string oldName_thietBi, ThietBiModel thietBi, string nhaMay_name)
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            Dictionary<string, NhaMayModel> dicNhaMay = GetDicNhaMay();

            NhaMayModel nhaMay = dicNhaMay[nhaMay_name];
            if (nhaMay.dsThietBi.ContainsKey(oldName_thietBi))
            {
                nhaMay.dsThietBi.Remove(oldName_thietBi);
                nhaMay.dsThietBi.Add(thietBi.Name, thietBi);
            }
            /*foreach (var nhaMay_item in dicNhaMay)
            {
                if (nhaMay_item.Value.Name == nhaMay_name)
                {
                    if (nhaMay_item.Value.dsThietBi.ContainsKey(oldName_thietBi))
                    {
                        nhaMay_item.Value.dsThietBi.Remove(oldName_thietBi);
                        nhaMay_item.Value.dsThietBi.Add(thietBi.Name, thietBi);
                    }
                }
            }*/

            string jsonString = (new JavaScriptSerializer()).Serialize((object)dicNhaMay);
            File.WriteAllText(path, jsonString);
        }

        public static Dictionary<string, NhaMayModel> GetDicNhaMay()
        {
            try
            {
                var path = GetPathJson.getPathConfig("DeviceAndData.json");
                JObject jsonObj = JObject.Parse(File.ReadAllText(path));

                Dictionary<string, NhaMayModel> dicNhaMay = new Dictionary<string, NhaMayModel>();

                foreach (var nm_item in jsonObj)
                {
                    NhaMayModel nmM = new NhaMayModel();
                    nmM.Name = nm_item.Key;

                    try
                    {
                        JObject jOb_nm = JObject.Parse(nm_item.Value.ToString());
                        foreach (var job_nm_item in jOb_nm)
                        {
                            if (job_nm_item.Key == "dsThietBi")
                            {
                                JObject jOb_tb = JObject.Parse(job_nm_item.Value.ToString());

                                Dictionary<string, ThietBiTCPIP> deviceTCPIP = jOb_tb.ToObject<Dictionary<string, ThietBiTCPIP>>();
                                foreach (var deviceIPUnit in deviceTCPIP)
                                {
                                    if (deviceIPUnit.Value.Protocol == "Modbus TCP/IP" || deviceIPUnit.Value.Protocol == "Siemens S7-1200")
                                    {
                                        nmM.dsThietBi.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                                    }
                                }

                                Dictionary<string, ThietBiCOM> deviceCom = jOb_tb.ToObject<Dictionary<string, ThietBiCOM>>();
                                foreach (var deviceComUnit in deviceCom)
                                {
                                    if (deviceComUnit.Value.Protocol == "Serial Port")
                                    {
                                        nmM.dsThietBi.Add(deviceComUnit.Key, deviceComUnit.Value);
                                    }
                                }


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }


                    dicNhaMay.Add(nmM.Name, nmM);

                }


                return dicNhaMay;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void UpdateDuLieuSlave(string tenNhaMay, string tenThietBi, string oldNameSlave, SlaveModel newSlave)
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            Dictionary<string, NhaMayModel> dicNhaMay = GetDicNhaMay();

            ThietBiModel thietBi = dicNhaMay[tenNhaMay].dsThietBi[tenThietBi];
            if (thietBi.dsSlave.ContainsKey(oldNameSlave))
            {
                thietBi.dsSlave.Remove(oldNameSlave);
                thietBi.dsSlave.Add(newSlave.Name, newSlave);
            }

            string jsonString = (new JavaScriptSerializer()).Serialize((object)dicNhaMay);
            File.WriteAllText(path, jsonString);
        }

        public static void AddThietBiToNhaMay(string tenNhaMay, ThietBiModel newThietBi)
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            Dictionary<string, NhaMayModel> dicNhaMay = GetDicNhaMay();

            if (dicNhaMay.ContainsKey(tenNhaMay))
            {
                dicNhaMay[tenNhaMay].dsThietBi.Add(newThietBi.Name, newThietBi);
            }
            

            string jsonString = (new JavaScriptSerializer()).Serialize((object)dicNhaMay);
            File.WriteAllText(path, jsonString);
        }

        public static void RemoveThietBiInNhaMay(string tenNhaMay, string oldNameThietBi)
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            Dictionary<string, NhaMayModel> dicNhaMay = GetDicNhaMay();

            if (dicNhaMay.ContainsKey(tenNhaMay))
            {
                NhaMayModel nm = dicNhaMay[tenNhaMay];
                if (nm.dsThietBi.ContainsKey(oldNameThietBi))
                {
                    nm.dsThietBi.Remove(oldNameThietBi);
                }
            }


            string jsonString = (new JavaScriptSerializer()).Serialize((object)dicNhaMay);
            File.WriteAllText(path, jsonString);
        }
    }
}
