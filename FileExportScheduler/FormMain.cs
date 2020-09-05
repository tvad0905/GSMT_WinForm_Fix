using EasyModbus;
using EasyModbus.Exceptions;
using FileExportScheduler.Constant;
using FileExportScheduler.Models;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.FileService;
using FileExportScheduler.Service.Json;
using FileExportScheduler.Service.ThietBi;
using FileExportScheduler.Service.ThongBao;
using Modbus.Device;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FileExportScheduler
{
    public partial class FormMain : Form
    {
        #region Variables Declaration
        bool checkExit = false;
        Dictionary<string, ThietBiGiamSat> dsThietBi = new Dictionary<string, ThietBiGiamSat>();//danh sách các thiêt bị
        Dictionary<string, string> dcExportData = new Dictionary<string, string>();
        ModbusClient modbus = new ModbusClient();
        SerialPort serialPort = new SerialPort();
        #endregion
        public FormMain()
        {
            InitializeComponent();

        }
        #region Button Events
        private void btnStart_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            notifyIcon.ShowBalloonTip(100);
            notifyIcon.Visible = true;

            btnStop.Enabled = true;
            btnStart.Enabled = false;
            btnDataList.Enabled = false;
            btnSetting.Enabled = false;

            //set thời gian đọc dữ liệu của thiết bị 

            //set chu kì đọc dữ liệu
            tmrScheduler.Interval = 3000;

            //set chu kỳ xóa file
            tmrChukyXoaFile.Interval = 30000;

            //set chu kỳ ghi ra file
            tmrReadData.Interval = Service.Json.JsonReader.GetTimeInterval();
            tmrReadData.Start();

            //quét danh sách thông số cho từng thiết bị từ json
            dsThietBi = Service.Json.JsonReader.LayDanhSachThongSoCuaTungThietBi();



            foreach (KeyValuePair<string, ThietBiGiamSat> deviceUnit in dsThietBi)
            {
                if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    serialPort.DtrEnable = true;
                    serialPort.RtsEnable = true;
                    var port = (ThietBiCOM)deviceUnit.Value;
                    serialPort = new SerialPort(port.Com, port.Baud, port.parity, port.Databit, port.stopBits);
                    serialPort.ReadTimeout = 200;
                    serialPort.Handshake = Handshake.None;
                    serialPort.ParityReplace = (byte)'\0';
                    serialPort.ReadBufferSize = 128;
                    serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(sp_SerialErrorReceivedEventHandler);

                    try
                    {
                        if (!serialPort.IsOpen)
                            serialPort.Open();

                    }
                    catch
                    {
                        //Lỗi ko kết nối được
                    }
                }
            }
            tmrScheduler.Start();
            tmrChukyXoaFile.Start();
            //lblStatus.Text = "Hệ thống đang chạy !";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            btnDataList.Enabled = true;
            btnSetting.Enabled = true;
            tmrScheduler.Stop();
            tmrChukyXoaFile.Stop();
            tmrReadData.Stop();
            serialPort.Close();
            modbus.Disconnect();
            //lblStatus.Text = "Hệ thống đã dừng !";
            notifyIcon.ShowBalloonTip(100, "Hệ thống", "Hệ thống đã dừng !", ToolTipIcon.Warning);
            lblTrangThaiThietBi.Text = "Hệ thống đã dừng";
        }

        private void btnDataList_Click(object sender, EventArgs e)
        {
            FormDataList f = new FormDataList();
            f.ShowDialog();

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            FormSetting f = new FormSetting();
            f.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            checkExit = true;

            DialogResult result = MessageBox.Show("Thoát hệ thống  ?", "caption", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            switch (result)
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    Application.Exit();
                    break;
                default:
                    break;
            }
            notifyIcon.Visible = false;
        }
        #endregion

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!checkExit)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                notifyIcon.ShowBalloonTip(1000);
                notifyIcon.Visible = true;
                ShowInTaskbar = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            ShowInTaskbar = true;
        }

        private void FormMain_Load_1(object sender, EventArgs e)
        {
            var path = GetPathJson.getPathConfig("Config.json");
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    var obj = sr.ReadToEnd();
                    CaiDatChung setting = JsonConvert.DeserializeObject<CaiDatChung>(obj.ToString());
                    if (setting.AutoRun == true)
                    {
                        btnStart.PerformClick();
                    }
                }
            }
        }

        /// <summary>
        /// lấy kết nối của thiết bị
        /// </summary>
        /// 
        object objW = new object();
        object objW2 = new object();
        private async void getDeviceConnect()
        {


            foreach (KeyValuePair<string, ThietBiGiamSat> deviceUnit in dsThietBi)
            {
                if (deviceUnit.Value.Protocol == "Modbus TCP/IP" || deviceUnit.Value.Protocol == "Siemens S7-1200")
                {
                    modbus = new ModbusClient(((ThietBiTCPIP)deviceUnit.Value).IP, ((ThietBiTCPIP)deviceUnit.Value).Port);
                    try
                    {
                        this.Invoke(new MethodInvoker(async delegate { await Task.Run(() => IPConnect(/*ListfilePath, */deviceUnit)); }));

                    }
                    catch{}
                }
                else if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    try
                    {
                        this.Invoke(new MethodInvoker(async delegate { await Task.Run(() => COMConnect(/*ListfilePath,*/ deviceUnit)); }));
                    }
                    catch{}
                }
            }

            lblTrangThaiThietBi.Text = ThongBaoService.DsLoi();
            ThongBaoLoi.DsThongBaoLoi.Clear();
            if (lblTrangThaiThietBi.Text == ThongBaoLoi.HoatDongBinhThuong)
            {
                lblTrangThaiThietBi.ForeColor = Color.Green;
            }
            else
            {
                lblTrangThaiThietBi.ForeColor = Color.Red;
            }

        }

        // tạo 1 thread cho connect
        void IPConnect(/*List<string> filePath, */KeyValuePair<string, ThietBiGiamSat> deviceUnit)
        {

            try
            {
                modbus.Connect();
                getDataDeviceIP(deviceUnit);
            }
            catch (ConnectionException ex)
            {
                //Lỗi không có kết nối 
                lock (objW)
                {
                    ThongBaoLoi.DsThongBaoLoi.Add(ThongBaoLoi.KhongKetNoi);
                    deviceUnit = ThietBiGiamSatService.SetTrangThaiBad(deviceUnit);// set trang thai bad cho dư lieu tung thiet bi
                    return;
                }
            }
            catch
            {
            }
        }

        void COMConnect(/*List<string> filePath, */KeyValuePair<string, ThietBiGiamSat> deviceUnit)
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                    getDataCOM(deviceUnit);
                }
                catch
                {
                    lock (objW2)
                    {
                        return;
                    }
                }
            }
            getDataCOM(deviceUnit);
        }

        //lấy dữ liệu của các thiết bị 
        private void getDataDeviceIP(KeyValuePair<string, ThietBiGiamSat> deviceUnit)
        {
            foreach (KeyValuePair<string, DiemDoGiamSat> diemDo in deviceUnit.Value.dsDiemDoGiamSat)
            {
                foreach (KeyValuePair<string, DuLieuGiamSat> dulieu in diemDo.Value.DsDulieu)
                {

                    try//lấy dữ liệu thành công
                    {
                        dulieu.Value.GiaTri = Convert.ToInt32(Data.Data.LayDuLieuTCPIP(modbus, dulieu.Value)).ToString();
                        dulieu.Value.TrangThaiTinHieu = TrangThaiKetNoi.Good;
                    }
                    catch (ModbusException ex)
                    {
                        ThongBaoLoi.DsThongBaoLoi.Add(ThongBaoLoi.VuotQuaDuLieu);
                        dulieu.Value.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                    }
                    catch (Exception ex)//Lỗi lấy dữ liệu thất bại
                    {
                        ThongBaoLoi.DsThongBaoLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                        dulieu.Value.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                    }
                    finally
                    {
                        dulieu.Value.ThoiGianDocGiuLieu = DateTime.Now;
                    }
                }
            }
        }

        private void getDataCOM(KeyValuePair<string, ThietBiGiamSat> deviceUnit)
        {
            foreach (KeyValuePair<string, DiemDoGiamSat> diemDo in deviceUnit.Value.dsDiemDoGiamSat)
            {
                foreach (KeyValuePair<string, DuLieuGiamSat> dulieu in diemDo.Value.DsDulieu)
                {
                    //lấy dữ liệu thành công
                    try
                    {
                        dulieu.Value.GiaTri = Data.Data.LayDuLieuCOM(dulieu.Value, serialPort).ToString();
                        dulieu.Value.TrangThaiTinHieu = TrangThaiKetNoi.Good;

                    }
                    //lấy dữ liệu thất bại
                    catch (TimeoutException ex)
                    {
                        //lỗi không đọc được dữ liệu
                        ThongBaoLoi.DsThongBaoLoi.Add(ThongBaoLoi.KhongKetNoi);
                        dulieu.Value.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                    }
                    catch (Modbus.SlaveException ex)
                    {
                        //lỗi số bản ghi cần đọc vượt quá lượng bản ghi trả về
                        ThongBaoLoi.DsThongBaoLoi.Add(ThongBaoLoi.VuotQuaDuLieu);
                        dulieu.Value.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                    }
                    catch
                    {
                    }
                    finally
                    {
                        dulieu.Value.ThoiGianDocGiuLieu = DateTime.Now;
                    }
                }
            }
        }

        private void sp_SerialErrorReceivedEventHandler(object sender, SerialErrorReceivedEventArgs e)
        {
            if (e.EventType == SerialError.RXParity)
            {
                lblTrangThaiThietBi.Text += ", Error Parity";
            }
        }

        private void tmrScheduler_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrScheduler.Stop();
                getDeviceConnect();
            }
            catch
            {

            }
            finally
            {
                tmrScheduler.Start();
            }
        }

        private void tmrChukyXoaFile_Tick(object sender, EventArgs e)
        {
            int chuKiXoaFile = Service.Json.JsonReader.LayThoiGianXoaFile();
            string duongDanThuMucDuLieu = Service.Json.JsonReader.DuongDanThuMucDuLieu();
            FileCSV.XoaFileVuotQuaChuKy(chuKiXoaFile, duongDanThuMucDuLieu);
        }

        private void tmrReadData_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrReadData.Stop();
                XuatRaFileCSV();
            }
            catch
            {
            }
            finally
            {
                tmrReadData.Start();
            }

        }

        private void XuatRaFileCSV()
        {
            #region lấy danh sách đường dẫn file csv

            List<string> ListfilePath = new List<string>();
            try
            {
                ListfilePath = Service.Json.JsonReader.LayDsDuongDanTheoTenDiemDo(dsThietBi);
            }
            catch//khi đường dẫn export file ko có trong config thì bắt người dùng nhập lại
            {
                tmrScheduler.Stop();
                tmrChukyXoaFile.Stop();
                MessageBox.Show("Chọn đường dẫn đến thư mục");
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                btnStop.PerformClick();
                btnSetting.PerformClick();
            }
            #endregion
            FileCSV.WriteDataToFileCSV(ListfilePath, dsThietBi);

        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout fa = new FormAbout();
            fa.Show();
        }
    }
}
