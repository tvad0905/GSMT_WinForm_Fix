using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExportScheduler.Models.DuLieu;
namespace FileExportScheduler.Models.DiemDo
{
    public class DiemDoGiamSat
    {
        string tenDiemDo;
        public Dictionary<string, DuLieuGiamSat> DsDulieu = new Dictionary<string, DuLieuGiamSat>();

        public DiemDoGiamSat(string tenDiemDo, Dictionary<string, DuLieuGiamSat> DsDulieu)
        {
            this.tenDiemDo = tenDiemDo;
            this.DsDulieu = DsDulieu;
        }
        public string TenDiemDo
        {
            get
            {
                return tenDiemDo;
            }
            set
            {
                tenDiemDo = value;
            }
        }


    }
}
