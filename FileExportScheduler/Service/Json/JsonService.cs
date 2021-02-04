using ESProtocolConverter.Models.NhaMay;
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
        public static void ToJsonAfterUpdateThietBi(ThietBiModel thietBi, string nhaMay_name)
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            Dictionary<string, NhaMayModel> dicNhaMay = GetDicNhaMay();
            foreach (var nhaMay_item in dicNhaMay)
            {
                if (nhaMay_item.Value.Name == nhaMay_name)
                {
                    foreach (var thietBi_item in nhaMay_item.Value.dsThietBi)
                    {
                        if (thietBi_item.Value.Name == thietBi.Name)
                        {
                            nhaMay_item.Value.dsThietBi.Remove(thietBi_item.Key);
                            nhaMay_item.Value.dsThietBi.Add(thietBi.Name, thietBi);
                        }
                    }
                }
            }

            string jsonString = (new JavaScriptSerializer()).Serialize((object)dicNhaMay);
            File.WriteAllText(path, jsonString);
        }

        public static Dictionary<string, NhaMayModel> GetDicNhaMay()
        {
            try
            {
                var path = GetPathJson.getPathConfig("DeviceAndData.json");
                JObject jsonObj = JObject.Parse(File.ReadAllText(path));

                return jsonObj.ToObject<Dictionary<string, NhaMayModel>>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
