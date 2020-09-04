namespace FileExportScheduler
{
    using System;
    using System.Management;
    using System.Net.NetworkInformation;

    internal static class SystemInfo
    {
        public static bool UseBaseBoardManufacturer;
        public static bool UseBaseBoardProduct;
        public static bool UseBiosManufacturer;
        public static bool UseBiosVersion;
        public static bool UseDiskDriveSignature;
        public static bool UseMac;
        public static bool UsePhysicalMediaSerialNumber;
        public static bool UseProcessorID;
        public static bool UseVideoControllerCaption;
        public static bool UseWindowsSerialNumber;

        public static string GetSystemInfo(string SoftwareName)
        {
            SoftwareName = "";
            if (UseProcessorID)
            {
                SoftwareName = SoftwareName + RunQuery("Processor", "ProcessorId");
            }
            if (UseBaseBoardProduct)
            {
                SoftwareName = SoftwareName + RunQuery("BaseBoard", "Product");
            }
            if (UseBaseBoardManufacturer)
            {
                SoftwareName = SoftwareName + RunQuery("BaseBoard", "Manufacturer");
            }
            if (UseDiskDriveSignature)
            {
                SoftwareName = SoftwareName + RunQuery("DiskDrive where InterfaceType != 'USB'", "Signature");
            }
            if (UseVideoControllerCaption)
            {
                SoftwareName = SoftwareName + RunQuery("VideoController", "Caption");
            }
            if (UsePhysicalMediaSerialNumber)
            {
                SoftwareName = SoftwareName + RunQuery("PhysicalMedia", "SerialNumber");
            }
            if (UseBiosVersion)
            {
                SoftwareName = SoftwareName + RunQuery("BIOS", "Version");
            }
            if (UseWindowsSerialNumber)
            {
                SoftwareName = SoftwareName + RunQuery("OperatingSystem", "SerialNumber");
            }
            if (UseMac)
            {
                SoftwareName = SoftwareName + leerMACaddress();
            }
            SoftwareName = RemoveUseLess(SoftwareName);
            if (SoftwareName.Length < 0x19)
            {
                return SoftwareName.ToUpper();
            }
            return SoftwareName.ToUpper();
            //return "48090RYF2B3E06BBF55A3S280F003CC18FS5407";
        }

        public static string leerMACaddress()
        {
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            string str = "";
            foreach (NetworkInterface interface2 in allNetworkInterfaces)
            {
                interface2.GetIPProperties();
                byte[] addressBytes = interface2.GetPhysicalAddress().GetAddressBytes();
                for (int i = 0; i < addressBytes.Length; i++)
                {
                    str = str + addressBytes[i].ToString("X2");
                    if (i != (addressBytes.Length - 1))
                    {
                        str = str + "-";
                    }
                }
            }
            return str;
        }

        private static string RemoveUseLess(string st)
        {
            for (int i = st.Length - 1; i >= 0; i--)
            {
                char ch = char.ToUpper(st[i]);
                if (((ch < 'A') || (ch > 'Z')) && ((ch < '0') || (ch > '9')))
                {
                    st = st.Remove(i, 1);
                }
            }
            return st;
        }

        private static string RunQuery(string TableName, string MethodName)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_" + TableName);
            foreach (ManagementObject obj2 in searcher.Get())
            {
                try
                {
                    return obj2[MethodName].ToString();
                }
                catch (Exception)
                {
                }
            }
            return "";
        }
    }
}

