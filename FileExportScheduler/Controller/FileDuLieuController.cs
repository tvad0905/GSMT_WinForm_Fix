using FileExportScheduler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    public class FileDuLieuController
    {
        
        public static void XoaFileVuotQuaChuKy(int chuKyXoaFile, string duongDanThuMucLuuDuLieu)
        {
            DirectoryInfo thuMucghiDuLieu = new DirectoryInfo(duongDanThuMucLuuDuLieu);
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
        public static void XuatFileDuLieuCSV(List<string> filePath, Dictionary<string, ThietBiGiamSat> dsThietBi)
        {
            int i = 0;
            foreach (KeyValuePair<string, ThietBiGiamSat> thietBi in dsThietBi)
            {

                foreach (KeyValuePair<string, DiemDoGiamSat> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {
                    string csvData = "[Data]" + "\n" + "Tagname,TimeStamp,Value,DataQuality" + "\n";
                    foreach (KeyValuePair<string, DuLieuGiamSat> duLieu in diemDo.Value.DsDulieu)
                    {
                        csvData +=
                                   duLieu.Value.DiemDo + "." + duLieu.Value.Ten + "," +
                                   duLieu.Value.ThoiGianDocGiuLieu.ToString("mm:ss.fff") + "," +
                                   Math.Round((Convert.ToDouble(duLieu.Value.GiaTri) / Convert.ToDouble(duLieu.Value.Scale)), 2) + "," +
                                   duLieu.Value.TrangThaiTinHieu + "\n";
                    }
                    File.WriteAllText(filePath[i], csvData);
                    i++;
                }
            }

        }
    }
}
