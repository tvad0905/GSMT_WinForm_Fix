using FileExportScheduler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    public static class ExportFileCSV
    {
        /// <summary>
        /// Sinh ra file csv trong Export Data folder
        /// </summary>
        /// <param name="filePath">danh sách đường dẫn file csv</param>
        /// <param name="dsThietBi">danh sách thiết bị</param>
        /// <param name="dsDiemDo">danh sách điểm đo</param>
        public static void WriteDataToFileCSV(List<string> filePath, Dictionary<string, DeviceModel> dsThietBi, Dictionary<string, List<DataModel>> dsDiemDo)
        {

            foreach (KeyValuePair<string, DeviceModel> deviceUnit in dsThietBi)
            {
                int i = 0;
                foreach (KeyValuePair<string, List<DataModel>> duLieuUnit in dsDiemDo)
                {
                    string csvData = "[Data]" + "\n" + "Tagname,TimeStamp,Value,DataQuality" + "\n";
                    foreach (DataModel dt in duLieuUnit.Value)
                    {
                        csvData +=
                                   duLieuUnit.Key + "." + dt.Ten + "," +
                                   dt.ThoiGianDocGiuLieu.ToString("mm:ss.fff") + "," +
                                   Math.Round((Convert.ToDouble(dt.GiaTri) / Convert.ToDouble(dt.Scale)), 2) + "," +
                                   deviceUnit.Value.TrangThaiKetNoi + "\n";
                    }
                    File.WriteAllText(filePath[i], csvData);
                    i++;
                }
            }

        }
    }
}
