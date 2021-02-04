using ESProtocolConverter.Models.NhaMay;
using ESProtocolConverter.Models.Slave;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace ESProtocolConverter.Service.Slave
{
    public class SlaveService
    {
        public static Dictionary<string, SlaveModel> GetDsSlave(string nhaMay_name, string thietBi_name)
        {
            Dictionary<string, SlaveModel> dsSlave = new Dictionary<string, SlaveModel>();
            try
            {

                var path = GetPathJson.getPathConfig("DeviceAndData.json");
                JObject jsonObj = JObject.Parse(File.ReadAllText(path));

                Dictionary<string, NhaMayModel> dicNhaMay = jsonObj.ToObject<Dictionary<string, NhaMayModel>>();

                foreach (var nhaMay_item in dicNhaMay)
                {
                    if (nhaMay_item.Value.Name == nhaMay_name)
                    {
                        foreach (var thietBi_item in nhaMay_item.Value.dsThietBi)
                        {
                            if (thietBi_item.Value.Name == thietBi_name)
                            {
                                return thietBi_item.Value.dsSlave;
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
