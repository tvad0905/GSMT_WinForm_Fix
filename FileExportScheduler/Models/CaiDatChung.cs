﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Models
{
    public class CaiDatChung
    {
        public bool AutoRun { get; set; }
        public int Interval { get; set; }
        public string ExportFilePath { get; set; }

        public int ChuKiXoaDuLieu { get; set; }
        public string FormatTime { get; set; }
    }
}
