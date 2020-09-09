using FileExportScheduler.Models;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.FileService
{
    public static class FileCSV
    {
        /// <summary>
        /// Sinh ra file csv trong Export Data folder
        /// </summary>
        /// <param name="filePath">danh sách đường dẫn file csv</param>
        /// <param name="dsThietBi">danh sách thiết bị</param>
        /// <param name="dsDiemDo">danh sách điểm đo</param>
        public static void XuatFileCSV(List<string> filePath, Dictionary<string, ThietBiModel> dsThietBi)
        {
            int i = 0;
            foreach (KeyValuePair<string, ThietBiModel> thietBi in dsThietBi)
            {

                foreach (KeyValuePair<string, DiemDoModel> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {
                    string csvData = "[Data]" + "\n" + "Tagname,TimeStamp,Value,DataQuality" + "\n";
                    foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
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
