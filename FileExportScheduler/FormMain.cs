﻿using EasyModbus;
using EasyModbus.Exceptions;
using FileExportScheduler.Constant;
using FileExportScheduler.Models;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.FileService;
using FileExportScheduler.Service.Json;
using FileExportScheduler.Service.KiemTra;
using FileExportScheduler.Service.ThietBi;
using FileExportScheduler.Service.ThongBao;
using Modbus.Device;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        public Dictionary<string, ThietBiModel> dsThietBi = new Dictionary<string, ThietBiModel>();//danh sách các thiêt bị
        Dictionary<string, string> dcExportData = new Dictionary<string, string>();
        ModbusClient modbusTCP = new ModbusClient();
        SerialPort serialPort = new SerialPort();
        FormHienThiDuLieu formHienThiDuLieu = new FormHienThiDuLieu();
        bool heThongDangChay = false;

        #endregion


        public FormMain()
        {
            InitializeComponent();
        }
        #region Button Events
        private void btnStart_Click(object sender, EventArgs e)
        {
            heThongDangChay = true;
            if (!Directory.Exists(Service.Json.JsonReader.DuongDanThuLog()))
            {
                MessageBox.Show("Đường dẫn thư mục không tồn tại");
                return;
            }

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            notifyIcon.ShowBalloonTip(100);
            notifyIcon.Visible = true;
            btnThongSoDuLieu.Enabled = true;
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            btnDataList.Enabled = false;
            btnSetting.Enabled = false;

            //set chu kì đọc dữ liệu
            tmrDocDuLieu.Interval = 1000;

            //set chu kỳ xóa file
            tmrChukyXoaFile.Interval = 30000;

            //set chu kỳ ghi ra file
            tmrXuatFile.Interval = Service.Json.JsonReader.GetTimeInterval();

            //quét danh sách thông số cho từng thiết bị từ json
            dsThietBi = Service.Json.JsonReader.LayDanhSachThongSoCuaTungThietBi();

            foreach (KeyValuePair<string, ThietBiModel> deviceUnit in dsThietBi)
            {
                if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    serialPort.DtrEnable = true;
                    serialPort.RtsEnable = true;
                    var port = (ThietBiCOM)deviceUnit.Value;
                    serialPort = new SerialPort(port.Com, port.Baud, port.parity, port.Databit, port.stopBits);
                    serialPort.ReadTimeout = 100;
                    serialPort.WriteTimeout = 100;
                    serialPort.Handshake = Handshake.None;
                    serialPort.ParityReplace = (byte)'\0';
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
            tmrDocDuLieu.Start();
            tmrChukyXoaFile.Start();
            tmrXuatFile.Start();
            //lblStatus.Text = "Hệ thống đang chạy !";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            heThongDangChay = false;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            btnDataList.Enabled = true;
            btnSetting.Enabled = true;
            btnThongSoDuLieu.Enabled = false;

            serialPort.Close();
            tmrDocDuLieu.Stop();
            tmrChukyXoaFile.Stop();
            tmrXuatFile.Stop();

            //lblStatus.Text = "Hệ thống đã dừng !";
            notifyIcon.ShowBalloonTip(100, "Hệ thống", "Hệ thống đã dừng !", ToolTipIcon.Warning);

            lblTrangThaiThietBi.Text = "Hệ thống đã dừng";
            lblTrangThaiThietBi.ForeColor = Color.Black;
            Thread.Sleep(1000);

            modbusTCP.Disconnect();
            serialPort.Dispose();
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
                    FormCollection fc = Application.OpenForms;

                    foreach (Form frm in fc)
                    {

                        if (frm.Name == "FormHienThiDuLieu")
                        {
                            formHienThiDuLieu.Close();
                            break;
                        }
                    }
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
        private async void GetDeviceConnect()
        {

            foreach (KeyValuePair<string, ThietBiModel> deviceUnit in dsThietBi)
            {
                if (deviceUnit.Value.Protocol == "Modbus TCP/IP" || deviceUnit.Value.Protocol == "Siemens S7-1200")
                {
                    modbusTCP = new ModbusClient(((ThietBiTCPIP)deviceUnit.Value).IP, ((ThietBiTCPIP)deviceUnit.Value).Port);
                    try
                    {
                        //this.Invoke(new MethodInvoker(async delegate { await Task.Run(() => IPConnect(/*ListfilePath, */deviceUnit)); }));
                        await Task.Run(() => IPConnect(/*ListfilePath, */deviceUnit));

                    }
                    catch { }
                }
                else
                if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    try
                    {
                        //this.Invoke(new MethodInvoker(async delegate { await Task.Run(() => COMConnect(/*ListfilePath,*/ deviceUnit)); }));
                        await Task.Run(() => COMConnect(/*ListfilePath,*/ deviceUnit));
                    }
                    catch
                    {

                    }
                }
            }
            if (heThongDangChay)
            {
                //set thong bao loi
                lblTrangThaiThietBi.Text = ThongBaoService.DsLoi();

                if (lblTrangThaiThietBi.Text == ThongBaoLoi.HoatDongBinhThuong)
                {
                    lblTrangThaiThietBi.ForeColor = Color.Green;
                }
                else
                {
                    lblTrangThaiThietBi.ForeColor = Color.Red;
                }
            }
        }

        // tạo 1 thread cho connect
        void IPConnect(/*List<string> filePath, */KeyValuePair<string, ThietBiModel> deviceUnit)
        {

            try
            {
                if (!modbusTCP.Connected)
                {

                    modbusTCP.Connect();
                    GetDataDeviceIP(deviceUnit);
                }

            }
            catch (ConnectionException ex)
            {
                //Lỗi không có kết nối 
                lock (objW)
                {

                    List<string> danhSachLoi = new List<string>();
                    danhSachLoi.Add(ThongBaoLoi.KhongKetNoi);
                    ThongBaoLoi.DanhSach[deviceUnit.Key] = danhSachLoi;

                    deviceUnit = ThietBiGiamSatService.SetTrangThaiBad(deviceUnit);// set trang thai bad cho dư lieu tung thiet bi
                    return;
                }
            }
            catch (Exception ex)
            {

            }

        }

        void COMConnect(/*List<string> filePath, */KeyValuePair<string, ThietBiModel> deviceUnit)
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
        private async void GetDataDeviceIP(KeyValuePair<string, ThietBiModel> deviceUnit)
        {
            ArrayList dsDuLieuNhanDuoc = new ArrayList();
            //Danh sách lỗi trong quá trình  đọc dữ liệu
            List<string> danhSachLoi = new List<string>();
            try
            {
                dsDuLieuNhanDuoc = Data.Data.LayDuLieuTCPIP(modbusTCP, deviceUnit.Value.MaxAddressCoils, deviceUnit.Value.MaxAddressInputs, deviceUnit.Value.MaxAddressInputRegisters, deviceUnit.Value.MaxAddressHoldingRegisters);
            }
            catch (Exception ex)
            {

            }
            foreach (KeyValuePair<string, DiemDoModel> diemDo in deviceUnit.Value.dsDiemDoGiamSat)
            {
                foreach (KeyValuePair<string, DuLieuModel> dulieu in diemDo.Value.DsDulieu)
                {

                   
                    luuTruDuLieuTCP(dulieu.Value, deviceUnit.Value, dsDuLieuNhanDuoc);
                }
            }
            //Add danh sách lỗi vào biến danh sách lỗi static
            ThongBaoLoi.DanhSach[deviceUnit.Key] = danhSachLoi;
        }

        private void getDataCOM(KeyValuePair<string, ThietBiModel> deviceUnit)
        {
            ArrayList dsDuLieuNhanDuoc = new ArrayList();
            //đọc dữ liệu
            try
            {
               
                dsDuLieuNhanDuoc = Data.Data.LayDuLieuCOM(serialPort, deviceUnit.Value.MaxAddressCoils, deviceUnit.Value.MaxAddressInputs, deviceUnit.Value.MaxAddressInputRegisters, deviceUnit.Value.MaxAddressHoldingRegisters);
              
            }
            //lấy dữ liệu thất bại
            catch (TimeoutException ex)
            {
                deviceUnit.Value.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi không đọc được dữ liệu
            }
            catch (Modbus.SlaveException ex)
            {
                deviceUnit.Value.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi số bản ghi cần đọc vượt quá lượng bản ghi trả về
            }
            catch (Exception ex)
            {

            }
            #region Gán dữ liệu
            //Danh sách lỗi trong quá trình  đọc dữ liệu
            List<string> danhSachLoi = new List<string>();

            foreach (KeyValuePair<string, DiemDoModel> diemDo in deviceUnit.Value.dsDiemDoGiamSat)
            {
                foreach (KeyValuePair<string, DuLieuModel> dulieu in diemDo.Value.DsDulieu)
                {
                    //lấy dữ liệu thành công
                    luuTruDuLieuCOM(dulieu.Value, deviceUnit.Value, dsDuLieuNhanDuoc);
                }
            }
            #endregion
            ThongBaoLoi.DanhSach[deviceUnit.Key] = danhSachLoi;

        }
        private void luuTruDuLieuTCP(DuLieuModel duLieu, ThietBiModel thietBi, ArrayList DsDuLieuNhanDuoc)
        {
            try
            {
                if (duLieu.DiaChi.StartsWith("0"))
                {
                    var DsDuLieuCoils = DsDuLieuNhanDuoc[0] as bool[];
                    int diaChiCoils = Convert.ToInt32(duLieu.DiaChi);

                    duLieu.GiaTri = DsDuLieuCoils[diaChiCoils].ToString();
                }
                else if (duLieu.DiaChi.StartsWith("1"))
                {
                    var DsDuLieuInputs = DsDuLieuNhanDuoc[0] as bool[];
                    int diaChiInputs = Convert.ToInt32(duLieu.DiaChi) - 10000;

                    duLieu.GiaTri = DsDuLieuInputs[diaChiInputs].ToString();
                }
                else if (duLieu.DiaChi.StartsWith("3"))
                {
                    var DsDuLieuInputRegisters = DsDuLieuNhanDuoc[0] as int[];
                    int diaChiInputRegisters = Convert.ToInt32(duLieu.DiaChi) - 30000;

                    duLieu.GiaTri = DsDuLieuInputRegisters[diaChiInputRegisters].ToString();
                }
                else if (duLieu.DiaChi.StartsWith("4"))
                {
                    var DsDuLieuHoldingRegisters = DsDuLieuNhanDuoc[0] as int[];
                    int diaChiHoldingRegisters = Convert.ToInt32(duLieu.DiaChi) - 40000;

                    duLieu.GiaTri = DsDuLieuHoldingRegisters[diaChiHoldingRegisters].ToString();
                }
                thietBi.TrangThaiTinHieu = TrangThaiKetNoi.Good;
            }
            
            catch (Exception ex)//Lỗi lấy dữ liệu thất bại
            {
                
            }
            finally
            {
                duLieu.ThoiGianDocGiuLieu = DateTime.Now;
            }
        }
        private void luuTruDuLieuCOM(DuLieuModel duLieu, ThietBiModel thietBi, ArrayList DsDuLieuNhanDuoc)
        {
            try
            {
                if (duLieu.DiaChi.StartsWith("0"))
                {
                    var DsDuLieuCoils = DsDuLieuNhanDuoc[0] as bool[];
                    int diaChiCoils = Convert.ToInt32(duLieu.DiaChi);
                    duLieu.GiaTri = DsDuLieuCoils[diaChiCoils].ToString();
                }
                else if (duLieu.DiaChi.StartsWith("1"))
                {
                    var DsDuLieuInputs = DsDuLieuNhanDuoc[0] as bool[];
                    int diaChiInputs = Convert.ToInt32(duLieu.DiaChi) - 10000;
                    duLieu.GiaTri = DsDuLieuInputs[diaChiInputs].ToString();
                }
                else if (duLieu.DiaChi.StartsWith("3"))
                {
                    var DsDuLieuInputRegisters = DsDuLieuNhanDuoc[0] as ushort[];
                    int diaChiInputRegisters = Convert.ToInt32(duLieu.DiaChi) - 30000;
                    duLieu.GiaTri = DsDuLieuInputRegisters[diaChiInputRegisters].ToString();
                }
                else if (duLieu.DiaChi.StartsWith("4"))
                {
                    var DsDuLieuHoldingRegisters = DsDuLieuNhanDuoc[0] as ushort[];
                    int diaChiHoldingRegisters = Convert.ToInt32(duLieu.DiaChi) - 40000;
                    duLieu.GiaTri = DsDuLieuHoldingRegisters[diaChiHoldingRegisters].ToString();
                }
                thietBi.TrangThaiTinHieu = TrangThaiKetNoi.Good;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                duLieu.ThoiGianDocGiuLieu = DateTime.Now;
            }
        }
        private void tmrDocDuLieu_Tick(object sender, EventArgs e)
        {
            try
            {
                //tmrDocDuLieu.Stop();
                GetDeviceConnect();
                formHienThiDuLieu.DsThietBi = dsThietBi; //hien thi du lieu doc duoc len view
              
                
            }
            catch
            {

            }
            finally
            {
                //tmrDocDuLieu.Start();
            }
        }


        private void tmrChukyXoaFile_Tick(object sender, EventArgs e)
        {
            int chuKiXoaFile = Service.Json.JsonReader.LayThoiGianXoaFile();
            string duongDanThuMucDuLieu = Service.Json.JsonReader.DuongDanThuLog();
            FileCSV.XoaFileVuotQuaChuKy(chuKiXoaFile, duongDanThuMucDuLieu);
        }

        private void tmrXuatFile_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrXuatFile.Stop();
                XuatRaFileCSV();

            }
            catch
            {
            }
            finally
            {
                tmrXuatFile.Start();
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
                tmrDocDuLieu.Stop();
                tmrChukyXoaFile.Stop();
                MessageBox.Show("Chọn đường dẫn đến thư mục");
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                btnStop.PerformClick();
                btnSetting.PerformClick();
            }
            #endregion
            FileCSV.XuatFileCSV(ListfilePath, dsThietBi);

        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout fa = new FormAbout();
            fa.Show();
        }

        private void btnThongSoDuLieu_Click(object sender, EventArgs e)
        {
            formHienThiDuLieu = new FormHienThiDuLieu(dsThietBi);
            formHienThiDuLieu.Show();
            btnDataList.Enabled = false;
        }
    }
}
