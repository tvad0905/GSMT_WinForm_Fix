namespace FileExportScheduler
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class TrialMaker
    {
        private string _BaseString;
        private string _BaseStringOld = "";
        private int _DefDays;
        private string _HideFilePath;
        private string _Identifier;
        private string _Pass1 = "";
        private string _Pass2 = "";
        private string _Pass3 = "";
        private string _Password;
        private string _RegFilePath;
        private int _Runed;
        private string _SoftName;
        private string _Text;

        public TrialMaker(string SoftwareName, string RegFilePath, string HideFilePath, string Text, int TrialDays, int TrialRunTimes, string Identifier)
        {
            this._SoftName = SoftwareName;
            this._Identifier = Identifier;
            this.SetDefaults();
            this._DefDays = TrialDays;
            this._Runed = TrialRunTimes;
            this._RegFilePath = RegFilePath;
            this._HideFilePath = HideFilePath;
            this._Text = Text;
        }

        private int CheckHideFile()
        {
            long days;
            int num2;
            string[] strArray = FileReadWrite.ReadFile(this._HideFilePath).Split(new char[] { ';' });
            this._BaseStringOld = strArray[3];
            if ((strArray.Length == 5) && (this._BaseString == strArray[4]))
            {
                if (Convert.ToInt32(strArray[1]) <= 0)
                {
                    this._DefDays = 0;
                    return 0;
                }
                DateTime time = new DateTime(Convert.ToInt64(strArray[0]));
                if (time == DateTime.Now)
                {
                    this._DefDays = 0;
                    return this._DefDays;
                }
                TimeSpan span = (TimeSpan)(DateTime.Now.Date - time.Date);
                days = span.Days;
                num2 = Convert.ToInt32(strArray[1]);
                this._Runed = Convert.ToInt32(strArray[2]);
                days = Math.Abs(days);
                this._DefDays = num2 - Convert.ToInt32(days);
            }
            else if (strArray.Length == 4)
            {
                if (Convert.ToInt32(strArray[1]) <= 0)
                {
                    this._DefDays = 0;
                    return 0;
                }
                DateTime time2 = new DateTime(Convert.ToInt64(strArray[0]));
                if (time2 == DateTime.Now)
                {
                    this._DefDays = 0;
                    return this._DefDays;
                }
                TimeSpan span2 = (TimeSpan)(DateTime.Now.Date - time2.Date);
                days = span2.Days;
                num2 = Convert.ToInt32(strArray[1]);
                this._Runed = Convert.ToInt32(strArray[2]);
                days = Math.Abs(days);
                this._DefDays = num2 - Convert.ToInt32(days);
            }
            return this._DefDays;
        }

        public bool CheckRegister()
        {
            string str = FileReadWrite.ReadFile(this._RegFilePath);            
            return (this._Password == str);
            //return false;
        }

        public int DaysToEnd()
        {
            FileInfo info = new FileInfo(this._HideFilePath);
            if (!info.Exists)
            {
                this.MakeHideFile();
                return this._DefDays;
            }
            return this.CheckHideFile();
        }

        private void MakeBaseString()
        {
            //ManhNH: For runtime
            //Hướng Linh 2
            //this._BaseString = "38B4450E0FFMB0BF20E103P6QHFO98E20";
            //Dakmi3 key
            //this._BaseString = "6B0461HFB0BMF30FOF3450003EQ0P4E5E";
            //Vĩnh Tân 1 key
            //this._BaseString = "M6BF0F1BBFH053FQ83005O5P450846EEE";
            //Thuận Hòa
            //this._BaseString="0B069HEFBBF00F4O2F33E00QM3P8E7205";
            this._BaseString = Encryption.Boring(Encryption.InverseByBase(SystemInfo.GetSystemInfo(this._SoftName), 10));
            //ManhNH: For crack
            //this._BaseString = "003B10AFBBF203AF7429S0020US648E039F3";
            //TuDV V0FF8FP1TB00CEBBB7O3E3500OHI2374610S1FSXIN1
            //this._BaseString = "003B10AFBBF203AF7429S0020US648E039F3";
            //Sử Thành: this._BaseString = "000K31773108SAA2806L56900260A";0N000IE704A70S0FB6BFEEAF1P1OX7365HLE1B38RSOEFI54
            //Longnv: FL20F0B4BF2A318E096033KAA9FBS04030720A
            //HieuPT: 40FF01B0SBYF2B52RE02261024A00FCS504006
            //this._BaseString = Encryption.Boring(Encryption.InverseByBase(SystemInfo.GetSystemInfo(this._SoftName), 10));
            //this._BaseString = "40FF01B0SBYF2B52RE02261024A00FCS504006";

        }

        private void MakeHideFile()
        {
            object obj2 = DateTime.Now.Ticks + ";";
            string data = string.Concat(new object[] { obj2, this._DefDays, ";", this._Runed, ";", this._BaseStringOld, ";", this._BaseString });
            FileReadWrite.WriteFile(this._HideFilePath, data);
        }

        private void MakePassword()
        {
            this._Password = Encryption.MakePassword(this._BaseString, this._Identifier);
            //License Hướng Linh 2: Q5BB4P8958FM597B35E7F0EF5HF53O61E
        }

        private void MakeRegFile()
        {
            FileReadWrite.WriteFile(this._RegFilePath, this._Password);
        }

        private string[] newsDays()
        {
            try
            {
                this._Pass1 = Encryption.MakePassword(this._BaseString, "2358777");
                this._Pass2 = Encryption.MakePassword(this._BaseString, "2345678");
                this._Pass3 = Encryption.MakePassword(this._BaseString, "34353637");
                return new string[] { this._Pass1, this._Pass2, this._Pass3 };
            }
            catch
            {
                return null;
            }
        }

        private void SetDefaults()
        {
            SystemInfo.UseBaseBoardManufacturer = false;
            SystemInfo.UseBaseBoardProduct = false;
            SystemInfo.UseBiosManufacturer = false;
            SystemInfo.UseBiosVersion = true;
            SystemInfo.UseDiskDriveSignature = true;
            SystemInfo.UsePhysicalMediaSerialNumber = false;
            SystemInfo.UseProcessorID = true;
            SystemInfo.UseVideoControllerCaption = false;
            SystemInfo.UseWindowsSerialNumber = false;
            SystemInfo.UseMac = false;
            this.MakeBaseString();
            this.MakePassword();
        }

        public RunTypes ShowDialog()
        {
            if (this.CheckRegister())
            {
                //ManhNH: For runtime
                return RunTypes.Licensed;
                //ManhNH: For crack
                //return RunTypes.Expired;
            }
            string[] passMoreDays = this.newsDays();
            DialogLicence licence = new DialogLicence(this._BaseString, this._Password, this.DaysToEnd(), this._Runed, this._Text, passMoreDays);
            this.MakeHideFile();
            switch (licence.ShowDialog())
            {
                case DialogResult.OK:
                    if (licence.type == RunTypes.Licensed)
                    {
                        this.MakeRegFile();
                    }
                    return licence.type;

                case DialogResult.Retry:
                    this._Runed = licence.NumRestore;
                    this._DefDays = licence.DaysRestore;
                    this.MakeHideFile();
                    return licence.type;
            }
            return RunTypes.Expired;
        }

        public string HideFilePath
        {
            get
            {
                return this._HideFilePath;
            }
            set
            {
                this._HideFilePath = value;
            }
        }

        public string RegFilePath
        {
            get
            {
                return this._RegFilePath;
            }
            set
            {
                this._RegFilePath = value;
            }
        }

        public int TrialPeriodDays()
        {
           return this._DefDays;
        }

        public byte[] TripleDESKey
        {
            get
            {
                return FileReadWrite.key;
            }
            set
            {
                FileReadWrite.key = value;
            }
        }

        public bool UseBaseBoardManufacturer
        {
            get
            {
                return SystemInfo.UseBiosManufacturer;
            }
            set
            {
                SystemInfo.UseBiosManufacturer = value;
            }
        }

        public bool UseBaseBoardProduct
        {
            get
            {
                return SystemInfo.UseBaseBoardProduct;
            }
            set
            {
                SystemInfo.UseBaseBoardProduct = value;
            }
        }

        public bool UseBiosManufacturer
        {
            get
            {
                return SystemInfo.UseBiosManufacturer;
            }
            set
            {
                SystemInfo.UseBiosManufacturer = value;
            }
        }

        public bool UseBiosVersion
        {
            get
            {
                return SystemInfo.UseBiosVersion;
            }
            set
            {
                SystemInfo.UseBiosVersion = value;
            }
        }

        public bool UseDiskDriveSignature
        {
            get
            {
                return SystemInfo.UseDiskDriveSignature;
            }
            set
            {
                SystemInfo.UseDiskDriveSignature = value;
            }
        }

        public bool UsePhysicalMediaSerialNumber
        {
            get
            {
                return SystemInfo.UsePhysicalMediaSerialNumber;
            }
            set
            {
                SystemInfo.UsePhysicalMediaSerialNumber = value;
            }
        }

        public bool UseProcessorID
        {
            get
            {
                return SystemInfo.UseProcessorID;
            }
            set
            {
                SystemInfo.UseProcessorID = value;
            }
        }

        public bool UseVideoControllerCaption
        {
            get
            {
                return SystemInfo.UseVideoControllerCaption;
            }
            set
            {
                SystemInfo.UseVideoControllerCaption = value;
            }
        }

        public bool UseWindowsSerialNumber
        {
            get
            {
                return SystemInfo.UseWindowsSerialNumber;
            }
            set
            {
                SystemInfo.UseWindowsSerialNumber = value;
            }
        }

        public enum RunTypes
        {
            Trial,
            Licensed,
            Expired,
            UnKnown,
            Freeware
        }
    }
}

