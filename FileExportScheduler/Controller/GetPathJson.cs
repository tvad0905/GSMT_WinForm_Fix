using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExportScheduler.Controller
{
    public static class GetPathJson
    {
        public static string getPathConfig(string fileName)
        {            
            return Path.Combine(Application.StartupPath, "Configuration", fileName);
        }
    }
}
