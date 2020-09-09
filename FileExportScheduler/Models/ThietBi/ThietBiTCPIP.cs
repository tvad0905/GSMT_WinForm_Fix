using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Models
{
    public class ThietBiTCPIP : ThietBiModel
    {
        public string IP { get; set; }
        public int Port { get; set; }
    }
}
