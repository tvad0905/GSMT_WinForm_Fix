using EasyModbus;
using EasyModbus.Exceptions;
using ESProtocolConverter.Models.Slave;
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
using Modbus.Message;
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
using System.Reflection.Emit;
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
        private bool heThongDangChay = false;
        #endregion
        public FormMain()
        {
            InitializeComponent();
        }
        #region Button Events
        private void btnStart_Click(object sender, EventArgs e)
        {
            heThongDangChay = true;
            LanDoc.isLanDau = true;
            if (!Directory.Exists(Service.Json.JsonReader.DuongDanThuLog()))
            {
                MessageBox.Show("Đường dẫn thư mục không tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            notifyIcon.ShowBalloonTip(100, "Hệ thống", "Ứng dụng đang chạy", ToolTipIcon.None);
            notifyIcon.Visible = true;
            btnThongSoDuLieu.Enabled = true;
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            btnDataList.Enabled = false;
            btnSetting.Enabled = false;

            //set chu kì đọc dữ liệu
            tmrDocDuLieu.Interval = 600;

            //set chu kỳ xóa file
            tmrChukyXoaFile.Interval = 30000;

            //set chu kỳ ghi ra file
            //tmrXuatFile.Interval = Service.Json.JsonReader.GetTimeInterval();
            tmrXuatFile.Interval = 500;

            //quét danh sách thông số cho từng thiết bị từ json
            dsThietBi = Service.Json.JsonReader.LayDanhSachThongSoCuaTungThietBi();

            foreach (KeyValuePair<string, ThietBiModel> deviceUnit in dsThietBi)
            {
                if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    serialPort.DtrEnable = true;
                    serialPort.RtsEnable = true;
                    var port = (ThietBiCOM)deviceUnit.Value;
                    serialPort = new SerialPort(port.Com, port.Baud, port.Parity, port.Databit, port.StopBits);
                    serialPort.ReadTimeout = 100;
                    //serialPort.WriteTimeout = 100;
                    //serialPort.Handshake = Handshake.None;
                    //serialPort.ParityReplace = (byte)'\0';
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
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            FormSetting f = new FormSetting();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            checkExit = true;

            DialogResult result = MessageBox.Show("Thoát hệ thống ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                notifyIcon.BalloonTipText = "Phần mềm đã ẩn";
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

        private void FormMain_Load(object sender, EventArgs e)
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
        private async Task GetDeviceConnect()
        {

            foreach (KeyValuePair<string, ThietBiModel> deviceUnit in dsThietBi)
            {
                if (deviceUnit.Value.Protocol == "Modbus TCP/IP" || deviceUnit.Value.Protocol == "Siemens S7-1200")
                {
                    modbusTCP = new ModbusClient(((ThietBiTCPIP)deviceUnit.Value).IP, ((ThietBiTCPIP)deviceUnit.Value).Port);
                    try
                    {
                        //this.Invoke(new MethodInvoker(async delegate { await Task.Run(() => IPConnect(/*ListfilePath, */deviceUnit)); }));
                        await Task.Run(() => IPConnect(deviceUnit));

                    }
                    catch
                    {

                    }
                }
                else
                if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    try
                    {
                        //this.Invoke(new MethodInvoker(async delegate { await Task.Run(() => COMConnect(/*ListfilePath,*/ deviceUnit)); }));
                        await Task.Run(() => COMConnect(deviceUnit));
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
        void IPConnect(KeyValuePair<string, ThietBiModel> deviceUnit)
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
                    //StopSystem("Vui lòng kiểm tra lại kết nối!");
                    return;
                }
            }
            catch (Exception ex)
            {

            }

        }

        void COMConnect(KeyValuePair<string, ThietBiModel> deviceUnit)
        {
            try
            {
                GetDataDeviceCOM(deviceUnit);
            }
            catch
            {
                lock (objW2)
                {
                    return;
                }
            }
        }

        //lấy dữ liệu của các thiết bị 
        private void GetDataDeviceIP(KeyValuePair<string, ThietBiModel> deviceUnit)
        {
            ArrayList dsDuLieuNhanDuoc = new ArrayList();
            //Danh sách lỗi trong quá trình  đọc dữ liệu
            List<string> danhSachLoi = new List<string>();
            //Read Coils
            if (deviceUnit.Value.MaxAddressCoils > 0)
            {
                try
                {
                    dsDuLieuNhanDuoc.Add(Data.DataTCPIP.LayDuLieuTCPCoils(modbusTCP, deviceUnit.Value.MaxAddressCoils, deviceUnit.Value.MinAddressCoils, deviceUnit.Value));
                    AddDataTCP(dsDuLieuNhanDuoc, deviceUnit);
                }
                catch (Exception ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                    //StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
                }
            }
            //Read Inputs
            else if (deviceUnit.Value.MaxAddressInputs > 0)
            {
                try
                {
                    dsDuLieuNhanDuoc.Add(Data.DataTCPIP.LayDuLieuTCPInputs(modbusTCP, deviceUnit.Value.MaxAddressInputs, deviceUnit.Value.MinAddressInputs, deviceUnit.Value));
                    AddDataTCP(dsDuLieuNhanDuoc, deviceUnit);
                }
                catch (Exception ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                    //StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
                }
            }
            //Read Input Registers
            else if (deviceUnit.Value.MaxAddressInputRegisters > 0)
            {
                try
                {
                    dsDuLieuNhanDuoc.Add(Data.DataTCPIP.LayDuLieuTCPInputRegister(modbusTCP, deviceUnit.Value.MaxAddressInputRegisters, deviceUnit.Value.MinAddressInputRegisters, deviceUnit.Value));
                    AddDataTCP(dsDuLieuNhanDuoc, deviceUnit);
                }
                catch (Exception ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                    //StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
                }
            }
            //Read Holding Registers
            else if (deviceUnit.Value.MaxAddressHoldingRegisters > 0)
            {
                try
                {
                    dsDuLieuNhanDuoc.Add(Data.DataTCPIP.LayDuLieuTCPHoldingRegister(modbusTCP, deviceUnit.Value.MaxAddressHoldingRegisters, deviceUnit.Value.MinAddressHoldingRegisters, deviceUnit.Value));
                    AddDataTCP(dsDuLieuNhanDuoc, deviceUnit);
                }
                catch (Exception ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                    //StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
                }
            }
            else
            {
                StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
            }
            //Add danh sách lỗi vào biến danh sách lỗi static
            ThongBaoLoi.DanhSach[deviceUnit.Key] = danhSachLoi;
        }

        private void GetDataDeviceCOM(KeyValuePair<string, ThietBiModel> deviceUnit)
        {
            List<string> danhSachLoi = new List<string>();
            ArrayList dsDuLieuNhanDuoc = new ArrayList();
            //Read Coils
            if (deviceUnit.Value.MaxAddressCoils > 0)
            {
                try
                {
                    dsDuLieuNhanDuoc.Add(Data.DataCOM.LayDuLieuCOMCoils(serialPort, deviceUnit.Value.MaxAddressCoils, deviceUnit.Value.MinAddressCoils, deviceUnit.Value));
                    AddDataCOM(dsDuLieuNhanDuoc, deviceUnit);
                }
                catch (TimeoutException ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongKetNoi);
                    //StopSystem("Vui lòng kiểm tra kết nối");
                }
                catch (Exception ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                    //StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
                }
            }
            //Read Inputs
            else if (deviceUnit.Value.MaxAddressInputs > 0)
            {
                try
                {
                    dsDuLieuNhanDuoc.Add(Data.DataCOM.LayDuLieuCOMInputs(serialPort, deviceUnit.Value.MaxAddressInputs, deviceUnit.Value.MinAddressCoils, deviceUnit.Value));
                    AddDataCOM(dsDuLieuNhanDuoc, deviceUnit);

                }
                catch (TimeoutException ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongKetNoi);
                    //StopSystem("Vui lòng kiểm tra kết nối");
                }
                catch (Exception ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                    //StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
                }
            }
            //Read Input Registers
            else if (deviceUnit.Value.MaxAddressInputRegisters > 0)
            {
                try
                {
                    dsDuLieuNhanDuoc.Add(Data.DataCOM.LayDuLieuCOMInputRegisters(serialPort, deviceUnit.Value.MaxAddressInputRegisters, deviceUnit.Value.MinAddressCoils, deviceUnit.Value));
                    AddDataCOM(dsDuLieuNhanDuoc, deviceUnit);

                }
                catch (TimeoutException ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongKetNoi);
                    //StopSystem("Vui lòng kiểm tra kết nối");
                }
                catch (Exception ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                    //StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
                }
            }
            //Read Holding Registers
            else if (deviceUnit.Value.MaxAddressHoldingRegisters > 0)
            {
                try
                {
                    dsDuLieuNhanDuoc.Add(Data.DataCOM.LayDuLieuCOMHoldingRegisters(serialPort, deviceUnit.Value.MaxAddressHoldingRegisters, deviceUnit.Value.MinAddressHoldingRegisters, deviceUnit.Value));
                    AddDataCOM(dsDuLieuNhanDuoc, deviceUnit);
                }
                catch (TimeoutException ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongKetNoi);
                    //StopSystem("Vui lòng kiểm tra kết nối");
                }
                catch (Exception ex)
                {
                    danhSachLoi.Add(ThongBaoLoi.KhongCoTinHieuTraVe);
                    //StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
                }
            }
            else
            {
                StopSystem("Vui lòng kiểm tra lại địa chỉ nhập vào");
            }
            ThongBaoLoi.DanhSach[deviceUnit.Key] = danhSachLoi;

        }

        private void AddDataCOM(ArrayList dsDuLieuNhanDuoc, KeyValuePair<string, ThietBiModel> deviceUnit)
        {
            #region Gán dữ liệu
            foreach (KeyValuePair<string, SlaveModel> slave in deviceUnit.Value.dsSlave)
            {
                foreach (KeyValuePair<string, DiemDoModel> diemDo in slave.Value.dsDiemDoGiamSat)
                {
                    foreach (KeyValuePair<string, DuLieuModel> dulieu in diemDo.Value.DsDulieu)
                    {
                        //lấy dữ liệu thành công
                        LuuTruDuLieuCOM(dulieu.Value, deviceUnit.Value, dsDuLieuNhanDuoc);
                    }
                }
            }
            #endregion
        }

        private void AddDataTCP(ArrayList dsDuLieuNhanDuoc, KeyValuePair<string, ThietBiModel> deviceUnit)
        {
            #region Gán dữ liệu
            foreach (KeyValuePair<string, SlaveModel> slave in deviceUnit.Value.dsSlave)
            {
                foreach (KeyValuePair<string, DiemDoModel> diemDo in slave.Value.dsDiemDoGiamSat)
                {
                    foreach (KeyValuePair<string, DuLieuModel> dulieu in diemDo.Value.DsDulieu)
                    {
                        //lấy dữ liệu thành công
                        LuuTruDuLieuTCP(dulieu.Value, deviceUnit.Value, dsDuLieuNhanDuoc);
                    }
                }
            }
            #endregion
        }

        private void LuuTruDuLieuTCP(DuLieuModel duLieu, ThietBiModel thietBi, ArrayList DsDuLieuNhanDuoc)
        {
            try
            {
                if (duLieu.DiaChi.StartsWith("0"))
                {
                    bool[] DsDuLieuCoils = DsDuLieuNhanDuoc[0] as bool[];
                    if (DsDuLieuCoils != null)
                    {
                        int diaChiCoils = Convert.ToInt32(duLieu.DiaChi) - thietBi.MinAddressCoils;
                        duLieu.GiaTri = DsDuLieuCoils[diaChiCoils].ToString();
                    }
                }
                else if (duLieu.DiaChi.StartsWith("1"))
                {
                    bool[] DsDuLieuInputs = DsDuLieuNhanDuoc[0] as bool[];
                    if (DsDuLieuInputs != null)
                    {
                        int diaChiInputs = Convert.ToInt32(duLieu.DiaChi) - 10000 - thietBi.MinAddressInputs;
                        duLieu.GiaTri = DsDuLieuInputs[diaChiInputs].ToString();
                    }
                }
                else if (duLieu.DiaChi.StartsWith("3"))
                {
                    int[] DsDuLieuInputRegisters = DsDuLieuNhanDuoc[0] as int[];
                    if (DsDuLieuInputRegisters != null)
                    {
                        int diaChiInputRegisters = Convert.ToInt32(duLieu.DiaChi) - 30000 - thietBi.MinAddressInputRegisters;
                        duLieu.GiaTri = DsDuLieuInputRegisters[diaChiInputRegisters].ToString();
                    }
                }
                else if (duLieu.DiaChi.StartsWith("4"))
                {
                    int[] DsDuLieuHoldingRegisters = DsDuLieuNhanDuoc[0] as int[];
                    if (DsDuLieuHoldingRegisters != null)
                    {
                        int diaChiHoldingRegisters = Convert.ToInt32(duLieu.DiaChi) - 40000 - thietBi.MinAddressHoldingRegisters;
                        duLieu.GiaTri = DsDuLieuHoldingRegisters[diaChiHoldingRegisters].ToString();
                    }
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

        private void LuuTruDuLieuCOM(DuLieuModel duLieu, ThietBiModel thietBi, ArrayList DsDuLieuNhanDuoc)
        {
            try
            {
                if (duLieu.DiaChi.StartsWith("0"))
                {
                    bool[] DsDuLieuCoils = DsDuLieuNhanDuoc[0] as bool[];
                    if (DsDuLieuCoils != null)
                    {
                        int diaChiCoils = Convert.ToInt32(duLieu.DiaChi) - thietBi.MinAddressCoils;
                        duLieu.GiaTri = DsDuLieuCoils[diaChiCoils].ToString();
                    }

                }
                else if (duLieu.DiaChi.StartsWith("1"))
                {
                    bool[] DsDuLieuInputs = DsDuLieuNhanDuoc[0] as bool[];
                    if (DsDuLieuInputs != null)
                    {
                        int diaChiInputs = Convert.ToInt32(duLieu.DiaChi) - 10000 - thietBi.MaxAddressInputs;
                        duLieu.GiaTri = DsDuLieuInputs[diaChiInputs].ToString();
                    }
                }
                else if (duLieu.DiaChi.StartsWith("3"))
                {
                    int[] DsDuLieuInputRegisters = DsDuLieuNhanDuoc[0] as int[];
                    if (DsDuLieuInputRegisters != null)
                    {
                        int diaChiInputRegisters = Convert.ToInt32(duLieu.DiaChi) - 30000 - thietBi.MinAddressInputRegisters;
                        duLieu.GiaTri = DsDuLieuInputRegisters[diaChiInputRegisters].ToString();
                    }
                }
                else if (duLieu.DiaChi.StartsWith("4"))
                {
                    int[] DsDuLieuHoldingRegisters = DsDuLieuNhanDuoc[0] as int[];
                    if (DsDuLieuHoldingRegisters != null)
                    {
                        int diaChiHoldingRegisters = Convert.ToInt32(duLieu.DiaChi) - 40000 - thietBi.MinAddressHoldingRegisters;
                        duLieu.GiaTri = DsDuLieuHoldingRegisters[diaChiHoldingRegisters].ToString();
                    }
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

        private async void tmrDocDuLieu_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrDocDuLieu.Stop();
                await GetDeviceConnect();
                formHienThiDuLieu.DsThietBi = dsThietBi;
                /*if (LanDoc.isLanDau)
                {
                    await GetDeviceConnect();
                    formHienThiDuLieu.DsThietBi = dsThietBi; //hien thi du lieu doc duoc len view 
                    LanDoc.isLanDau = false;
                }
                else
                {
                    await GetDeviceConnect();
                    formHienThiDuLieu.DsThietBi = dsThietBi; //hien thi du lieu doc duoc len view 
                    LanDoc.isLanDau = false;
                }*/


            }
            catch
            {

            }
            finally
            {
                tmrDocDuLieu.Start();
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

                if (DateTime.Now.Minute % Service.Json.JsonReader.GetTimeInterval() == 0 && DateTime.Now.Second == 0)
                {
                    XuatRaFileCSV();
                }
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
            List<string> ListfilePath = new List<string>();
            try
            {
                ListfilePath = Service.Json.JsonReader.LayDsDuongDanTheoTenDiemDo(dsThietBi);
            }
            catch//khi đường dẫn export file ko có trong config thì bắt người dùng nhập lại
            {
                tmrDocDuLieu.Stop();
                tmrChukyXoaFile.Stop();
                MessageBox.Show("Chọn đường dẫn đến thư mục!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                btnStop.PerformClick();
                btnSetting.PerformClick();
            }
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
            formHienThiDuLieu.StartPosition = FormStartPosition.CenterScreen;
            formHienThiDuLieu.ShowDialog();
            btnDataList.Enabled = false;
        }

        private void StopSystem(String loaiLoi)
        {
            if (heThongDangChay)
            {
                if (LanDoc.isLanDau == true)
                {
                    if (btnStop.InvokeRequired)
                    {
                        btnStop.Invoke(new MethodInvoker(delegate
                        {
                            WindowState = FormWindowState.Normal;
                            ShowInTaskbar = true;
                            btnStop.PerformClick();

                        }));
                    }

                    this.Invoke(new Action(delegate ()
                    {
                        MessageBox.Show(loaiLoi, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            }
        }
    }
}
