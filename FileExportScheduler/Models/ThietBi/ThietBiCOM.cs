using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Models
{
    public class ThietBiCOM : ThietBiGiamSat
    {
        public string Com { get; set; }
        public int Baud { get; set; }
        public Parity parity { get; set; }
        public int Databit { get; set; }
        public StopBits stopBits { get; set; }
    }
}
