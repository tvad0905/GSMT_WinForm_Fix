﻿using FileExportScheduler.Models;
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
        public static void WriteDataToFileCSV(List<string> filePath, Dictionary<string, ThietBiGiamSat> dsThietBi)
        {

            foreach (KeyValuePair<string, ThietBiGiamSat> thietBi in dsThietBi)
            {
                int i = 0;
                foreach (KeyValuePair<string, DiemDoGiamSat> diemDo in thietBi.Value.dsDiemDoGiamSat)
                {
                    string csvData = "[Data]" + "\n" + "Tagname,TimeStamp,Value,DataQuality" + "\n";
                    foreach (KeyValuePair<string, DuLieuGiamSat> duLieu in diemDo.Value.DsDulieu)
                    {
                        csvData +=
                                   duLieu.Key + "." + duLieu.Value.Ten + "," +
                                   duLieu.Value.ThoiGianDocGiuLieu.ToString("mm:ss.fff") + "," +
                                   Math.Round((Convert.ToDouble(duLieu.Value.GiaTri) / Convert.ToDouble(duLieu.Value.Scale)), 2) + "," +
                                   thietBi.Value.TrangThaiKetNoi + "\n";
                    }
                    File.WriteAllText(filePath[i], csvData);
                    i++;
                }
            }

        }
    }
}