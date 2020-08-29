using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    public class FileCu
    {
        public DirectoryInfo thuMucghiDuLieu;
        public FileCu(string duongDanThuMucLuuDuLieu)
        {
            thuMucghiDuLieu = new DirectoryInfo(duongDanThuMucLuuDuLieu);
        }
        public void XoaFileVuotQuaChuKy(int chuKyXoaFile)
        {

            while (true)
            {
                FileSystemInfo fileBiXoa = thuMucghiDuLieu.GetFileSystemInfos().OrderBy(fi => fi.CreationTime).FirstOrDefault();
                if (fileBiXoa != null)
                {
                    DateTime thoiGianFileSinhRa = fileBiXoa.LastWriteTime;
                    if ((DateTime.Now.Minute - thoiGianFileSinhRa.Minute) >= chuKyXoaFile)
                    {
                        
                            fileBiXoa.Delete();
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            }
        }
    }
}
