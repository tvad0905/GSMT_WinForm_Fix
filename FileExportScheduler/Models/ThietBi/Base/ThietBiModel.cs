using FileExportScheduler.Constant;
using FileExportScheduler.Models.DiemDo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Models.ThietBi.Base
{
    public abstract class ThietBiModel
    {

     
        public string Name { get; set; }
        public string Protocol { get; set; }
        public string TrangThaiTinHieu { get; set; }

        public ushort MaxAddressCoils { get; set; }
        public ushort MaxAddressInputs { get; set; }
        public ushort MaxAddressInputRegisters { get; set; }
        public ushort MaxAddressHoldingRegisters { get; set; }

        public Dictionary<string, DiemDoModel> dsDiemDoGiamSat = new Dictionary<string, DiemDoModel>();

    }
}
