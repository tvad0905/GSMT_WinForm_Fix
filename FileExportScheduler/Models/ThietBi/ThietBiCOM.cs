﻿using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Models
{
    public class ThietBiCOM : ThietBiModel
    {
        public string Com { get; set; }
        public int Baud { get; set; }
        public Parity Parity { get; set; }
        public int Databit { get; set; }
        public StopBits StopBits { get; set; }
    }
}
