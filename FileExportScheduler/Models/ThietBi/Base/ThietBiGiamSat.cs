using FileExportScheduler.Models.DiemDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Models.ThietBi.Base
{
    public abstract class ThietBiGiamSatModel
    {
        public string Name { get; set; }
        public string Protocol { get; set; }
        public Dictionary<string, DiemDoGiamSat> dsDiemDoGiamSat = new Dictionary<string, DiemDoGiamSat>();

    }
}
