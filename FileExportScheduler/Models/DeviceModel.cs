using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Models
{
    public class DeviceModel
    {
        public string Name { get; set; }
        public string Protocol { get; set; }
        public TypeEnum TypeModel { get; set; }
        public String TrangThaiKetNoi { get; set; }

        public Dictionary<string, DataModel> ListDuLieuChoTungPLC = new Dictionary<string, DataModel>();
    }
}
