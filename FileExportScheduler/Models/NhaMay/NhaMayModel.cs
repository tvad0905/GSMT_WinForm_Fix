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

        public Dictionary<string, ThietBiModel> dsThietBi = new Dictionary<string, ThietBiModel>();

        public NhaMayModel(string name)
        {
            this.Name = name;
        }
    }
}
