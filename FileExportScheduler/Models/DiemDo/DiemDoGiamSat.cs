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
        public Dictionary<string, DuLieuGiamSatModel> DsDulieu = new Dictionary<string, DuLieuGiamSatModel>();

        public DiemDoGiamSat(string tenDiemDo, Dictionary<string, DuLieuGiamSatModel> DsDulieu)
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
