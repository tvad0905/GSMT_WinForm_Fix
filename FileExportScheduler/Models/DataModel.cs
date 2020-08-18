using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Models
{
    public class DataModel

    {
        public string donViDo;
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string DonViDo
        {
            get { return donViDo; }
            set
            {
                if (value == null)
                { donViDo = ""; }
                else { donViDo = value; }
            }
        }
        public string GiaTri { get; set; }
        public string Scale { get; set; }
        public string ThietBi { get; set; }
        public DateTime ThoiGianDocGiuLieu { get; set; }
    }
}
