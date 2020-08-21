﻿using EasyModbus;
using FileExportScheduler.Controller;
using FileExportScheduler.Models;
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
        Dictionary<string, DeviceModel> dsThietBi = new Dictionary<string, DeviceModel>();//danh sách các thiêt bị
        Dictionary<String, List<DataModel>> dsDiemDo = new Dictionary<string, List<DataModel>>();// danh sách các điểm đo
        Dictionary<string, string> dcExportData = new Dictionary<string, string>();
        ModbusClient mobus = new ModbusClient();
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
            notifyIcon.ShowBalloonTip(1000);
            notifyIcon.Visible = true;

            btnStop.Enabled = true;
            btnStart.Enabled = false;
            btnDataList.Enabled = false;
            btnSetting.Enabled = false;

            //xét chu kì ghi file theo json 
            tmrScheduler.Interval = Controller.JsonReader.GetTimeInterval();

            //quét danh sách thông số cho từng thiết bị từ json
            dsThietBi = Controller.JsonReader.LayDanhSachThongSoCuaTungThietBi();

            foreach (KeyValuePair<string, DeviceModel> deviceUnit in dsThietBi)
            {
                if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    serialPort.DtrEnable = true;
                    serialPort.RtsEnable = true;
                    serialPort = new SerialPort(((ComConfigModel)deviceUnit.Value).Com, ((ComConfigModel)deviceUnit.Value).Baud, ((ComConfigModel)deviceUnit.Value).parity, ((ComConfigModel)deviceUnit.Value).Databit, ((ComConfigModel)deviceUnit.Value).stopBits);
                    try
                    {
                        if (!serialPort.IsOpen)
                            serialPort.Open();
                    }
                    catch (Exception ex)
                    {
                        //Lỗi ko kết nối được
                    }

                }
            }
            tmrScheduler.Start();
            lblStatus.Text = "Hệ thống đang chạy !";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            btnDataList.Enabled = true;
            btnSetting.Enabled = true;
            tmrScheduler.Stop();
            lblStatus.Text = "Hệ thống đã dừng !";
            notifyIcon.ShowBalloonTip(1000, "Hệ thống", "Hệ thống đã dừng !", ToolTipIcon.Warning);
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
                    SettingModel setting = JsonConvert.DeserializeObject<SettingModel>(obj.ToString());
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

            #region lấy danh sách đường dẫn file csv

            List<string> ListfilePath = new List<string>();
            try
            {
                ListfilePath = Controller.JsonReader.LayDsDuongDanTheoTenDiemDo(dsThietBi, ref dsDiemDo);
            }
            catch (Exception ex)//khi đường dẫn export file ko có trong config thì bắt người dùng nhập lại
            {
                tmrScheduler.Stop();
                MessageBox.Show("Chọn đường dẫn đến thư mục");
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                btnStop.PerformClick();
                btnSetting.PerformClick();
            }
            #endregion


            foreach (KeyValuePair<string, DeviceModel> deviceUnit in dsThietBi)
            {
                if (deviceUnit.Value.Protocol == "Modbus TCP/IP" || deviceUnit.Value.Protocol == "Siemens S7-1200")
                {
                    mobus = new ModbusClient(((IPConfigModel)deviceUnit.Value).IP, ((IPConfigModel)deviceUnit.Value).Port);
                    try
                    {
                        await Task.Run(() => ThreadIPConnect(ListfilePath, deviceUnit));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    try
                    {
                        await Task.Run(() => ThreadCOMConnect(ListfilePath, deviceUnit));

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            Controller.ExportFileCSV.WriteDataToFileCSV(ListfilePath, dsThietBi, dsDiemDo);


        }

        // tạo 1 thread cho connect
        void ThreadIPConnect(List<string> filePath, KeyValuePair<string, DeviceModel> deviceUnit)
        {
            try
            {
                mobus.Connect();

            }
            catch (Exception ex)
            {
                lock (objW)
                {
                    deviceUnit.Value.TrangThaiKetNoi = "Bad";
                    Controller.ExportFileCSV.WriteDataToFileCSV(filePath, dsThietBi, dsDiemDo);
                    return;
                }
            }
            getDataDeviceIP(deviceUnit);
        }

        void ThreadCOMConnect(List<string> filePath, KeyValuePair<string, DeviceModel> deviceUnit)
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                    getDataCOM(deviceUnit);
                }
                catch (Exception ex)
                {
                    serialPort = new SerialPort(((ComConfigModel)deviceUnit.Value).Com, ((ComConfigModel)deviceUnit.Value).Baud, ((ComConfigModel)deviceUnit.Value).parity, ((ComConfigModel)deviceUnit.Value).Databit, ((ComConfigModel)deviceUnit.Value).stopBits);

                    deviceUnit.Value.TrangThaiKetNoi = "Bad";
                    Controller.ExportFileCSV.WriteDataToFileCSV(filePath, dsThietBi, dsDiemDo);
                }
            }
            getDataCOM(deviceUnit);

        }
        //lấy dữ liệu của các thiết bị 
        private void getDataDeviceIP(KeyValuePair<string, DeviceModel> deviceUnit)
        {
            for (int i = 0; i < deviceUnit.Value.ListDuLieuChoTungPLC.Count; i++)
            {

                DataModel duLieuTemp = deviceUnit.Value.ListDuLieuChoTungPLC.ElementAt(i).Value;
                try//lấy dữ liệu thành công
                {
                    duLieuTemp.GiaTri = Convert.ToInt32(Data.Data.LayDuLieuTCPIP(mobus, duLieuTemp)) + "";

                    deviceUnit.Value.TrangThaiKetNoi = "Good";
                }
                catch (Exception ex)//lấy dữ liệu thất bại
                {
                    deviceUnit.Value.TrangThaiKetNoi = "Bad";
                }
                finally
                {
                    duLieuTemp.ThoiGianDocGiuLieu = DateTime.Now;

                }

            }
        }

        private void getDataCOM(KeyValuePair<string, DeviceModel> deviceUnit)
        {
            for (int i = 0; i < deviceUnit.Value.ListDuLieuChoTungPLC.Count; i++)
            {

                DataModel duLieuTemp = deviceUnit.Value.ListDuLieuChoTungPLC.ElementAt(i).Value;
                
                //lấy dữ liệu thành công
                try
                {
                    duLieuTemp.GiaTri = ushort.Parse(Data.Data.LayDuLieuCOM(duLieuTemp, serialPort)) + "";
                    deviceUnit.Value.TrangThaiKetNoi = "Good";
                }
                //lấy dữ liệu thất bại
                catch (Exception ex)
                {
                    deviceUnit.Value.TrangThaiKetNoi = "Bad";

                    serialPort = new SerialPort(((ComConfigModel)deviceUnit.Value).Com, ((ComConfigModel)deviceUnit.Value).Baud, ((ComConfigModel)deviceUnit.Value).parity, ((ComConfigModel)deviceUnit.Value).Databit, ((ComConfigModel)deviceUnit.Value).stopBits);

                }
                finally
                {
                    duLieuTemp.ThoiGianDocGiuLieu = DateTime.Now;
                }

            }
        }
        private void tmrScheduler_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrScheduler.Stop();
                getDeviceConnect();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                tmrScheduler.Start();
            }
        }
    }
}
