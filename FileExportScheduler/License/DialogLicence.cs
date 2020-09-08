namespace FileExportScheduler
{
    //using Axon.Test.Properties;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    public class DialogLicence : Form
    {
        private string _Pass;
        private string[] _PassMoreDays;
        public int _Runed;
        private Button btnOK;
        private Button btnTrial;
        private IContainer components;
        public int DaysRestore;
        private GroupBox grbRegister;
        private GroupBox groupBox1;
        private Label label2;
        private Label lblDays;
        private Label lblID;
        private Label lblSerial;
        private Label lblText;
        public int NumRestore;
        private TextBox sebBaseString;
        private TextBox sebPassword;
        //public int _DaysToEnd;

        public DialogLicence(string BaseString, string Password, int DaysToEnd, int Runed, string info, string[] passMoreDays)
        {
            this.InitializeComponent();
            this.sebBaseString.Text = BaseString;
            //this.sebBaseString.Text = "003B10AFBBF203AF7429S0020US648E039F3";
            //this.sebBaseString.Text = "40FF01B0SBYF2B52RE02261024A00FCS504006";
            
            this._Pass = Password;
            this._Runed = Runed;
            //this._DaysToEnd = DaysToEnd;
            this.lblDays.Text = DaysToEnd.ToString() + " Day(s)";
            this.lblText.Text = info;
            if ((DaysToEnd <= 0) || (Runed <= 0))
            {
                this.lblDays.Text = "Finished";
                this.btnTrial.Enabled = false;
                //this.btnFree.Enabled = true;
                //this.btnFree.ForeColor = Color.ForestGreen;
            }
            this.sebPassword.Select();
            this._PassMoreDays = passMoreDays;
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            Process.Start("www.e-solutions.com.vn");
        }

        private void btnFree_Click(object sender, EventArgs e)
        {
            this.type = TrialMaker.RunTypes.Freeware;
            base.DialogResult = DialogResult.OK;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this._Pass == this.sebPassword.Text)
            {
                MessageBox.Show("Đăng ký bản quyền thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.type = TrialMaker.RunTypes.Licensed;
                base.DialogResult = DialogResult.OK;
            }
            else if (this.isMoreDays())
            {
                MessageBox.Show("Password valid, you have " + this.DaysRestore + " days of Trial", "Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.type = TrialMaker.RunTypes.Trial;
                base.DialogResult = DialogResult.Retry;
            }
            else
            {
                MessageBox.Show("Đăng ký không thành công", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnTrial_Click_1(object sender, EventArgs e)
        {
            this.type = TrialMaker.RunTypes.Trial;
            base.DialogResult = DialogResult.OK;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.grbRegister = new System.Windows.Forms.GroupBox();
            this.sebPassword = new System.Windows.Forms.TextBox();
            this.sebBaseString = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.lblSerial = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDays = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTrial = new System.Windows.Forms.Button();
            this.grbRegister.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnOK.Location = new System.Drawing.Point(230, 83);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&Active";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grbRegister
            // 
            this.grbRegister.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grbRegister.Controls.Add(this.sebPassword);
            this.grbRegister.Controls.Add(this.sebBaseString);
            this.grbRegister.Controls.Add(this.lblText);
            this.grbRegister.Controls.Add(this.lblSerial);
            this.grbRegister.Controls.Add(this.lblID);
            this.grbRegister.Controls.Add(this.btnOK);
            this.grbRegister.ForeColor = System.Drawing.Color.White;
            this.grbRegister.Location = new System.Drawing.Point(10, 7);
            this.grbRegister.Name = "grbRegister";
            this.grbRegister.Size = new System.Drawing.Size(331, 114);
            this.grbRegister.TabIndex = 1;
            this.grbRegister.TabStop = false;
            this.grbRegister.Text = "Thông tin đăng ký";
            // 
            // sebPassword
            // 
            this.sebPassword.Location = new System.Drawing.Point(51, 55);
            this.sebPassword.Name = "sebPassword";
            this.sebPassword.Size = new System.Drawing.Size(254, 20);
            this.sebPassword.TabIndex = 8;
            // 
            // sebBaseString
            // 
            this.sebBaseString.Location = new System.Drawing.Point(51, 28);
            this.sebBaseString.Name = "sebBaseString";
            this.sebBaseString.ReadOnly = true;
            this.sebBaseString.Size = new System.Drawing.Size(254, 20);
            this.sebBaseString.TabIndex = 7;
            // 
            // lblText
            // 
            this.lblText.Location = new System.Drawing.Point(9, 184);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(296, 35);
            this.lblText.TabIndex = 6;
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(7, 57);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(36, 13);
            this.lblSerial.TabIndex = 3;
            this.lblSerial.Text = "Serial:";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(20, 28);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(21, 13);
            this.lblID.TabIndex = 1;
            this.lblID.Text = "ID:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDays);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnTrial);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 126);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 46);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dùng thử";
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = true;
            this.lblDays.ForeColor = System.Drawing.Color.Cyan;
            this.lblDays.Location = new System.Drawing.Point(152, 17);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(37, 13);
            this.lblDays.TabIndex = 5;
            this.lblDays.Text = "[Days]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Số ngày dùng thử còn lại:";
            // 
            // btnTrial
            // 
            this.btnTrial.BackColor = System.Drawing.SystemColors.Control;
            this.btnTrial.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnTrial.Location = new System.Drawing.Point(230, 14);
            this.btnTrial.Name = "btnTrial";
            this.btnTrial.Size = new System.Drawing.Size(75, 23);
            this.btnTrial.TabIndex = 2;
            this.btnTrial.Text = "&Trial";
            this.btnTrial.UseVisualStyleBackColor = false;
            this.btnTrial.Click += new System.EventHandler(this.btnTrial_Click_1);
            // 
            // DialogLicence
            // 
            this.AcceptButton = this.btnOK;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.ClientSize = new System.Drawing.Size(349, 182);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grbRegister);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogLicence";
            this.ShowIcon = false;
            this.Text = "Đăng ký license";
            this.grbRegister.ResumeLayout(false);
            this.grbRegister.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private bool isMoreDays()
        {
            try
            {
                if (this._PassMoreDays != null)
                {
                    string text = this.sebPassword.Text;
                    int startIndex = (text.Length - 2) / 2;
                    string s = text.Substring(startIndex, 2);
                    this.DaysRestore = int.Parse(s, NumberStyles.HexNumber);
                    text = text.Remove((text.Length - 2) / 2, 2);
                    int num2 = 0;
                    if (this._Runed == 50)
                    {
                        this._Runed = 0;
                    }
                    foreach (string str3 in this._PassMoreDays)
                    {
                        if ((str3 == text) && (num2 == this._Runed))
                        {
                            this.NumRestore = num2 + 1;
                            return true;
                        }
                        num2++;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        public TrialMaker.RunTypes type { get; set; }
    }
}

