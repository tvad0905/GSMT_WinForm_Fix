using FileExportScheduler.Models;
using FileExportScheduler.Service.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.KiemTra
{
    /// <summary>
    /// 
    /// </summary>
    public static class KiemTraDuongDan
    {
    /// <summary>
    /// Kiem tra duong dan lưu file CSV có tồn tại trong máy hay không
    /// </summary>
    /// <param name="duongDan">Đường dẫn của thư mục xuất file</param>
    /// <param name="setting">Cấu hình cài đặt chung</param>
    /// <returns></returns>
        public static bool TonTaiKhiLuu(string duongDan, CaiDatChung setting)
        {
            var path = GetPathJson.getPathConfig("Config.json");
            if (Directory.Exists(duongDan))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    var loadData = JsonConvert.SerializeObject(setting);
                    sw.WriteLine(loadData);
                }
                return true;
               
            }
            else
            {
                
                return false;
            }
        }

    }
}
