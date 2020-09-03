using FileExportScheduler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    public static class FileCSVController
    {
        /// <summary>
        /// Sinh ra file csv trong Export Data folder
        /// </summary>
        /// <param name="filePath">danh sách đường dẫn file csv</param>
        /// <param name="dsThietBi">danh sách thiết bị</param>
        /// <param name="dsDiemDo">danh sách điểm đo</param>
        public static void WriteDataToFileCSV(List<string> filePath, Dictionary<string, ThietBiGiamSat> dsThietBi)
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
                                   Math.Round((Convert.ToInt32(duLieu.Value.GiaTri) / Convert.ToDouble(duLieu.Value.Scale)), 2) + "," +
                                   duLieu.Value.TrangThaiTinHieu + "\n";
                    }
                    File.WriteAllText(filePath[i], csvData);
                    i++;
                }
            }

        }
        public static void XoaFileVuotQuaChuKy(int chuKyXoaFile, string duongDanThuMucLuuDuLieu)
        {
            DirectoryInfo thuMucghiDuLieu = new DirectoryInfo(duongDanThuMucLuuDuLieu);
            for (int i = 0; i < 200000; i++)
            {
                FileSystemInfo fileBiXoa = thuMucghiDuLieu.GetFileSystemInfos().OrderBy(fi => fi.CreationTime).FirstOrDefault();
                if (fileBiXoa != null)
                {
                    DateTime thoiGianFileSinhRa = fileBiXoa.LastWriteTime;
                    if (chuKyXoaFile != 0)
                    {
                        TimeSpan ts = DateTime.Now.Subtract(thoiGianFileSinhRa);

                        if (ts.Minutes >= chuKyXoaFile)
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
                else
                {
                    break;
                }
            }
        }
    }
}
