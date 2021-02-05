using FileExportScheduler.Models;
using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESProtocolConverter.Models.NhaMay
{
    public class NhaMayModel
    {
        public string Name { get; set; }

        public Dictionary<string, ThietBiModel> dsThietBi;

        public NhaMayModel(string name)
        {
            this.Name = name;
        }

        public NhaMayModel(Dictionary<string, ThietBiTCPIP> thietBiTCPIP)
        {
            foreach(var item in thietBiTCPIP)
            {
                dsThietBi.Add(item.Key, item.Value);
            }
        }

        public NhaMayModel(Dictionary<string, ThietBiCOM> thietBiCOM)
        {
            foreach (var item in thietBiCOM)
            {
                dsThietBi.Add(item.Key, item.Value);
            }
        }

        public NhaMayModel()
        {
            dsThietBi = new Dictionary<string, ThietBiModel>();
        }
    }
}
