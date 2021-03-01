using FileExportScheduler.Models.DiemDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESProtocolConverter.Models.Slave
{
    public class SlaveModel
    {
        public string Name { get; set; }
        public int ScanRate { get; set; }
        public Dictionary<string, DiemDoModel> dsDiemDoGiamSat = new Dictionary<string, DiemDoModel>();
    }
}
