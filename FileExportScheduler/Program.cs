using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace FileExportScheduler
{
    static class Program
    {
        private static string AppGuid = "83dcd467-2558-49f5-8293-a8b2d580107a";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            if (PriorProcess() != null)
            {

                MessageBox.Show("Ứng dụng đang chạy", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TrialMaker maker = new TrialMaker("FileExportScheduler", Application.StartupPath + @"\Register.reg", Application.StartupPath + @"\ESSetp.dbf", "", 0xA, 10, "246813579");
            //TrialMaker maker = new TrialMaker("ESMRServer", Application.StartupPath + @"\Register.reg", Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\ATSetp.dbf", "", 0x2d, 0, "246813579");
            maker.TripleDESKey = new byte[] {
                    100, 0x86, 0x41, 1, 20, 0x26, 0x47, 0x62, 0x43, 12, 0x4c, 0x58, 11, 9, 14, 0x4c,
                    0x36, 0x15, 0x57, 0x7b, 0xe9, 6, 0xc7, 5
                };
            TrialMaker.RunTypes types = maker.ShowDialog();
            int a = maker.DaysToEnd();
            //types = TrialMaker.RunTypes.Licensed;
            bool flag2 = false;
            switch (types)
            {
                case TrialMaker.RunTypes.Trial:
                    {
                        if (a != 0)
                            flag2 = true;
                        break;
                    }
                case TrialMaker.RunTypes.Licensed:
                    flag2 = true;
                    break;

                default:
                    Application.Exit();
                    return;
            }
            if (flag2)
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);
                using (System.Threading.Mutex mutex = new System.Threading.Mutex(false, @"Global\" + AppGuid))
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        MessageBox.Show("Chương trình đang chạy", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    GC.Collect();


                    Application.Run(new FormMain());
                }
            }
            else
            {
                MessageBox.Show("Chương trình hết hạn sử dụng", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        public static Process PriorProcess()
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string[] typeinfo = args.Name.Split(',');
            foreach (Assembly it in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (it.GetName().Name.Equals(typeinfo[0]) ||
                    it.GetName().FullName.Equals(typeinfo[0]))
                {
                    return it;
                }
            }
            string[] tmp = args.Name.Split(',');
            string path = "";
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                path = Path.Combine("/usr", "lib");
                path = Path.Combine(path, tmp[0].ToLower().Replace(".", "-"));
                if (Directory.Exists(path))
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    foreach (FileInfo fi in di.GetFiles(tmp[0] + ".dll"))
                    {
                        System.Diagnostics.Trace.WriteLine("CurrentDomain_AssemblyResolve: Returning assembly from(3):" + fi.FullName);
                        Assembly assembly = Assembly.LoadFile(fi.FullName);
                        return assembly;
                    }
                }
            }
            return null;
        }


        private static Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            string ns = "";
            int pos = args.Name.LastIndexOf('.');
            if (pos != -1)
            {
                ns = args.Name.Substring(0, pos);
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.GetName().Name == ns)
                    {
                        if (assembly.GetType(args.Name, false, true) != null)
                        {
                            return assembly;
                        }
                    }
                }
            }
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == args.Name ||
                        type.FullName == args.Name)
                    {
                        return assembly;
                    }
                }
            }
            try
            {
                Assembly asm = Assembly.LoadFrom(ns + ".dll");
                return asm;
            }
            catch
            {
                //Ignore error.
            }
            return null;
        }
    }
}
