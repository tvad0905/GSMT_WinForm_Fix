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
using System.Windows.Forms;

namespace FileExportScheduler.Service.FileService
{
    public static class FileCSV
    {
        /// <summary>
        /// Sinh ra file csv trong Export Data folder
        /// </summary>
        /// <param name="filePath">danh sách 
        /// file csv</param>
        /// <param name="dsThietBi">danh sách thiết bị</param>
        /// <param name="dsDiemDo">danh sách điểm đo</param>
        public static void XuatFileCSV(List<string> filePath, Dictionary<string, ThietBiModel> dsThietBi)
        {
            FormSetting formSetting = new FormSetting();
            int i = 0;
            foreach (KeyValuePair<string, ThietBiModel> thietBi in dsThietBi)
            {

                foreach (KeyValuePair<string, DiemDoModel> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {
                    StringBuilder csvData = new StringBuilder();
                    csvData.Append("[Data]" + "\n" + "Tagname,TimeStamp,Value,DataQuality" + "\n");
                    foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
                    {
                        if (int.TryParse(duLieu.Value.GiaTri, out _))
                        {
                            csvData.Append(duLieu.Value.DiemDo);
                            csvData.Append(".");
                            csvData.Append(duLieu.Value.Ten);
                            csvData.Append(",");
                            csvData.Append(duLieu.Value.ThoiGianDocGiuLieu.ToString(Service.Json.JsonReader.LayDinhDangThoiGian()));
                            csvData.Append(",");

                            csvData.Append(Convert.ToInt32(duLieu.Value.GiaTri) / Convert.ToDouble(duLieu.Value.Scale));
                            csvData.Append(",");
                            csvData.Append(thietBi.Value.TrangThaiTinHieu);
                            csvData.Append("\n");
                        }
                        else
                        {
                            csvData.Append(duLieu.Value.DiemDo);
                            csvData.Append(".");
                            csvData.Append(duLieu.Value.Ten);
                            csvData.Append(",");
                            csvData.Append(duLieu.Value.ThoiGianDocGiuLieu.ToString(Service.Json.JsonReader.LayDinhDangThoiGian()));
                            csvData.Append(",");
                            csvData.Append(duLieu.Value.GiaTri);
                            csvData.Append(",");
                            csvData.Append(thietBi.Value.TrangThaiTinHieu);
                            csvData.Append("\n");
                        }

                    }
                    /*if (!File.Exists(filePath[i]))
                    {
                        File.AppendAllText(filePath[i], "[Data]" + "\n" + "Tagname,TimeStamp,Value,DataQuality" + "\n");
                    }
                    File.AppendAllText(filePath[i], csvData.ToString());*/
                    File.WriteAllText(filePath[i], csvData.ToString());
                    i++;
                }
            }

        }
        public static void XoaFileVuotQuaChuKy(int chuKyXoaFile, string duongDanThuMucLuuDuLieu)
        {
            DirectoryInfo thuMucghiDuLieu = new DirectoryInfo(duongDanThuMucLuuDuLieu);
            for (int i = 0; i < 200000; i++)
            {
                try
                {
                    FileSystemInfo fileBiXoa = thuMucghiDuLieu.GetFileSystemInfos().OrderBy(fi => fi.CreationTime).FirstOrDefault();
                    if (fileBiXoa != null)
                    {
                        DateTime thoiGianFileSinhRa = fileBiXoa.LastWriteTime;
                        if (chuKyXoaFile != 0)
                        {
                            TimeSpan ts = DateTime.Now.Subtract(thoiGianFileSinhRa);

                            if (ts.Days >= chuKyXoaFile)
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
                catch
                {
                    //MessageBox.Show("đường dẫn thư mục không tồn tại");
                }
            }
        }
    }
}
