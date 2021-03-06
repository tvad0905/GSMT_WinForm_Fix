﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileExportScheduler.Models;
using System.IO;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.IO.Ports;
using FileExportScheduler.KiemTraDauVao;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Service.Json;
using FileExportScheduler.Service.DiemDo;
using FileExportScheduler.Service.ThietBi;
using FileExportScheduler.Service.FinMaxAddress;
using ESProtocolConverter.Models.Slave;
using ESProtocolConverter.Models.NhaMay;
using ESProtocolConverter.Service.Slave;
using ESProtocolConverter.Service.Json;
using ESProtocolConverter.Models.Common;

namespace FileExportScheduler
{
    public partial class ProtocolConfiguration : UserControl
    {
        #region biến toàn cục
        Dictionary<string, ThietBiModel> dsThietBiGiamSat = new Dictionary<string, ThietBiModel>();
        ThietBiModel thietBi;
        SlaveModel slave = null;
        public FormDataList formDataList;
        private bool isSaved = true;
        public bool isClicked { get; set; }
        public bool isValidatePassed { get; set; }
        public bool isTabConfigHaveAnyChanged { get; set; }
        public bool isTabDataHaveAnyChanged { get; set; }
        public bool isTabSlaveChanged { get; set; }
        public bool isAddSlave { get; set; }
        #endregion

        public void SetThietBiAndSlave(ThietBiModel thietBi, string slave_name)
        {
            this.thietBi = thietBi;
            if (slave_name != null)
            {
                this.slave = this.thietBi.dsSlave[slave_name];
            }
        }

        public ThietBiModel GetCurrentThietBi()
        {
            return this.thietBi;
        }

        public void SetDsThietBi(Dictionary<string, ThietBiModel> dsThietBiGiamSat)
        {
            this.dsThietBiGiamSat = dsThietBiGiamSat;
        }

        public ProtocolConfiguration(FormDataList formDataList)
        {
            InitializeComponent();
            this.formDataList = formDataList;
            //DocDsThietBiTuFileJson();
            //LoadDuLieuLenDgv();
            cbProtocol.SelectedIndex = cbProtocol.Items.IndexOf("Modbus TCP/IP");
            cbCOM.SelectedIndex = cbCOM.Items.IndexOf(1);
            cbBaud.SelectedIndex = cbBaud.Items.IndexOf("9600");
            cbDataBit.SelectedIndex = cbDataBit.Items.IndexOf("8");
            cbParity.SelectedIndex = cbParity.Items.IndexOf("Even");
            cbStopBit.SelectedIndex = cbStopBit.Items.IndexOf("1");
            isClicked = false;
            isTabConfigHaveAnyChanged = false;
            isTabDataHaveAnyChanged = false;
            isAddSlave = false;
            isTabSlaveChanged = false;
        }

        #region Thao tác với json
        //Xuất từ file .json ra 1 list
        /*private void DocDsThietBiTuFileJson()
        {
            try
            {
                dsThietBiGiamSat.Clear();
                dsThietBiGiamSat = ThietBiGiamSatService.GetDsThietBi(new NhaMayModel("Quang Ninh"));
            }
            catch
            {
            }
        }*/

        //Viết từ list vào file .json
        /*public void GhiDsThietBiRaFileJson()
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            string jsonString = (new JavaScriptSerializer()).Serialize((object)dsThietBiGiamSat);
            File.WriteAllText(path, jsonString);
        }*/
        #endregion

        #region Kiểm tra nhập vào

        public bool CheckValidateCauHinhCOM()
        {
            errorIP.SetError(txtIPAdress, "");
            errorPort.SetError(txtPort, "");
            bool error = true;
            if (cbProtocol.SelectedIndex == -1)
            {
                errorGiaoThuc.SetError(cbProtocol, "Chưa chọn giao thức");
                error = false;
            }
            if (txtTenGiaoThuc.Text.Trim() == "")
            {
                errorTenGiaoThuc.SetError(txtTenGiaoThuc, "Chưa nhập tên thiết bị");
                error = false;
            }
            else
            {
                errorTenGiaoThuc.SetError(txtTenGiaoThuc, "");
            }

            if (dsThietBiGiamSat.ContainsKey(txtTenGiaoThuc.Text) && txtTenGiaoThuc.Text != formDataList.selectedNodeDouble.Text)
            {
                errorTenGiaoThuc.SetError(txtTenGiaoThuc, "Tên thiết bị trùng lặp");
                error = false;
            }
            else
            {
                errorTenGiaoThuc.SetError(txtTenGiaoThuc, "");
            }

            /*if (thietBi.dsSlave.ContainsKey(txt_SlaveAddress.Text))
            {
                errorTenGiaoThuc.SetError(txtTenGiaoThuc, "Tên slave trùng lặp");
                error = false;
            }*/

            return error;
        }

        public bool CheckValidateCauHinhIP()
        {
            Match PortRegex = Regex.Match(txtPort.Text, @"^()([1-9]|[1-5]?[0-9]{2,4}|6[1-4][0-9]{3}|65[1-4][0-9]{2}|655[1-2][0-9]|6553[1-5])$");
            bool error = true;

            if (cbProtocol.SelectedIndex == -1)
            {
                errorGiaoThuc.SetError(cbProtocol, "Chưa chọn giao thức");
                error = false;
            }
            if (txtTenGiaoThuc.Text.Trim() == "")
            {
                errorTenGiaoThuc.SetError(txtTenGiaoThuc, "Chưa nhập tên thiết bị");
                error = false;
            }
            else
            {
                errorTenGiaoThuc.SetError(txtTenGiaoThuc, "");
            }

            if (dsThietBiGiamSat.ContainsKey(txtTenGiaoThuc.Text) && txtTenGiaoThuc.Text != formDataList.selectedNodeDouble.Text)
            {
                errorTenGiaoThuc.SetError(txtTenGiaoThuc, "Tên thiết bị trùng lặp");
                error = false;
            }

            if (txtIPAdress.Text.Trim() == "...")
            {
                errorIP.SetError(txtIPAdress, "Chưa nhập IP");
                error = false;
            }
            else
            {
                errorIP.SetError(txtIPAdress, "");
            }

            if (txtPort.Text.Trim() == "")
            {
                errorPort.SetError(txtPort, "Chưa nhập cổng địa chỉ");
                error = false;
            }
            else if (!PortRegex.Success)
            {
                errorPort.SetError(txtPort, "Chưa nhập đúng dạng cổng địa chỉ");
                error = false;
            }
            else
            {
                errorPort.SetError(txtPort, "");
            }

            return error;
        }

        string regexSlaveAddress = @"^[1-9][0-9]{0,2}(?:,[0-9]{3}){0,3}$";
        string regexScanRate = @"^[1-9]\d*$";
        public bool CheckValidateCauHinhSlave()
        {
            bool isError = true;
            if (txtSlaveAddress.Text.Trim() == "")
            {
                errorSlaveAddress.SetError(txtSlaveAddress, "Slave Address trống!");
                isError = false;
            }
            else if (!Regex.IsMatch(txtSlaveAddress.Text, regexSlaveAddress))
            {
                errorSlaveAddress.SetError(txtSlaveAddress, "Slave Address là số nguyên dương");
                isError = false;
            }
            else if (int.Parse(txtSlaveAddress.Text) > 255)
            {
                errorSlaveAddress.SetError(txtSlaveAddress, "Slave Address chỉ từ 1 - 255");
                isError = false;
            }
            else
            {
                errorSlaveAddress.SetError(txtSlaveAddress, "");
            }

            if(txtScanRate.Text.Trim() == "")
            {
                errorScanRate.SetError(txtScanRate, "ScanRate trống!");
                isError = false;
            }
            else if (!Regex.IsMatch(txtScanRate.Text, regexScanRate))
            {
                errorScanRate.SetError(txtScanRate, "ScanRate là số nguyên dương");
                isError = false;
            }
            else
            {
                errorScanRate.SetError(txtScanRate, "");
            }

            return isError;
        }
        #endregion

        #region Sự kiện nút

        private void txtIPAdress_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        //Thêm dữ liệu protocol
        private void btnAddData_Click(object sender, EventArgs e)
        {
            isClicked = true;
            SaveData();
        }

        private void SaveData()
        {
            isValidatePassed = DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol);
            if (DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol))
            {
                LuuDanhMucDuLieuVaoJson();
                isSaved = true;
                isTabDataHaveAnyChanged = false;
                if (isClicked == true)
                {
                    MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isClicked = false;
                }
            }
            else
            {
                MessageBox.Show("Lưu dữ liệu không thành công!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isTabDataHaveAnyChanged = true;
            }
        }

        //Xóa dữ liệu protocol
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (slave == null) return;
            if (dgvDataProtocol.SelectedRows.Count > 0)
            {
                try
                {
                    DialogResult dl = MessageBox.Show("Bạn có muốn xóa ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dl == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dgvDataProtocol.SelectedRows)//đọc danh sách các dòng dữ liệu được chọn
                        {
                            if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                            {
                                try
                                {
                                    /*var diemDo = dsThietBiGiamSat[txtTenGiaoThuc.Text].dsDiemDoGiamSat[row.Cells[1].Value.ToString()];*/
                                    var diemDo = slave.dsDiemDoGiamSat[row.Cells[1].Value.ToString()];
                                    if (diemDo.DsDulieu.Count() == 0)
                                    {
                                        /*dsThietBiGiamSat[txtTenGiaoThuc.Text].dsDiemDoGiamSat.Remove(diemDo.TenDiemDo);*/
                                        slave.dsDiemDoGiamSat.Remove(diemDo.TenDiemDo);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //MessageBox.Show("Lỗi: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                dgvDataProtocol.Rows.Remove(row);

                            }
                            else
                            {
                                if (row.IsNewRow == false)
                                {
                                    dgvDataProtocol.Rows.Remove(row);
                                }
                                continue;
                            }
                            //dgvDataProtocol.Rows.Remove(row);
                        }
                        /*isValidatePassed = DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol);
                        if (DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol))
                        {
                            LuuDanhMucDuLieuVaoJson();
                            MessageBox.Show("Xóa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isSaved = true;
                            isTabDataHaveAnyChanged = false;
                        }
                        else
                        {
                            MessageBox.Show("Dữ liệu nhập vào lỗi vui lòng kiểm tra lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            isSaved = false;
                            isTabDataHaveAnyChanged = true;
                        }*/
                        isTabDataHaveAnyChanged = true;
                        isSaved = false;
                    }
                    else
                    {
                        isTabDataHaveAnyChanged = false;
                        isSaved = true;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Xóa dữ liệu không thành công!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isSaved = true;
                    isTabDataHaveAnyChanged = false;
                }
            }

        }

        //lưu cấu hình protocol
        private void btnAddNewProtocol_Click(object sender, EventArgs e)
        {
            isClicked = true;
            ThemMoiThietBi();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Nhập dữ liệu sẽ làm mất dữ liệu cũ trên màn hình. Tiếp tục nhập file?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    var openFile = openFileDialog1.ShowDialog();
                    if (openFile == DialogResult.OK)
                    {
                        BindDataFromCSV(openFileDialog1.FileName);
                        isTabDataHaveAnyChanged = true;
                        isSaved = false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Nhập dữ liệu không thành công!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LuuDanhMucDuLieuVaoJson()
        {
            try
            {
                if (slave == null) return;
                slave.dsDiemDoGiamSat = DanhSachDiemDoService.LayDsDiemDoTuDgv(dgvDataProtocol);
                var maxAddress = MaxAddress.GetMax(thietBi);
                var minAddress = MaxAddress.GetMin(thietBi);
                thietBi.MaxAddressCoils = (ushort)maxAddress[0];
                thietBi.MaxAddressInputs = (ushort)maxAddress[1];
                thietBi.MaxAddressInputRegisters = (ushort)maxAddress[2];
                thietBi.MaxAddressHoldingRegisters = (ushort)maxAddress[3];

                thietBi.MinAddressCoils = (ushort)minAddress[0];
                thietBi.MinAddressInputs = (ushort)minAddress[1];
                thietBi.MinAddressInputRegisters = (ushort)minAddress[2];
                thietBi.MinAddressHoldingRegisters = (ushort)minAddress[3];

                thietBi.dsSlave[slave.Name] = slave;

                JsonService.ReplaceOldTbByNewTb(thietBi.Name, thietBi, "Quang Ninh");
                //JsonService.UpdateDuLieuSlave("Quang Ninh", thietBi.Name, slave.Name, slave);
                //JsonService.ToJsonAfterUpdateThietBi(thietBi, "Quang Ninh");
                //GhiDsThietBiRaFileJson();

                /*var thietBiGiamSatDuocChon = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text];
                thietBiGiamSatDuocChon.dsDiemDoGiamSat = DanhSachDiemDoService.LayDsDiemDoTuDgv(dgvDataProtocol);
                var maxAddress = MaxAddress.GetMax(thietBiGiamSatDuocChon);
                var minAddress = MaxAddress.GetMin(thietBiGiamSatDuocChon);
                thietBiGiamSatDuocChon.MaxAddressCoils = (ushort)maxAddress[0];
                thietBiGiamSatDuocChon.MaxAddressInputs = (ushort)maxAddress[1];
                thietBiGiamSatDuocChon.MaxAddressInputRegisters = (ushort)maxAddress[2];
                thietBiGiamSatDuocChon.MaxAddressHoldingRegisters = (ushort)maxAddress[3];

                thietBiGiamSatDuocChon.MinAddressCoils = (ushort)minAddress[0];
                thietBiGiamSatDuocChon.MinAddressInputs = (ushort)minAddress[1];
                thietBiGiamSatDuocChon.MinAddressInputRegisters = (ushort)minAddress[2];
                thietBiGiamSatDuocChon.MinAddressHoldingRegisters = (ushort)minAddress[3];
                GhiDsThietBiRaFileJson();*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BindDataFromCSV(string filePath)
        {
            try
            {
                //dgvShowDuLieu.Columns.Clear();
                DataTable dt = new DataTable();
                string[] lines = File.ReadAllLines(filePath);

                dt.Columns.Add("Ten", typeof(string));
                dt.Columns.Add("DiemDo", typeof(string));
                dt.Columns.Add("DiaChi", typeof(string));
                dt.Columns.Add("Scale", typeof(string));
                dt.Columns.Add("DonViDo", typeof(string));

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] t = lines[i].Split(',');
                    if (t.Length > 1)
                    {
                        dt.Rows.Add(t[0], t[1], t[2], t[3], t[4]);
                    }
                }
                dgvDataProtocol.DataSource = dt;
                isValidatePassed = DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol);
                if (DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol))
                {

                    MessageBox.Show("Đọc dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Dữ liệu sai định dạng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File đang mở, vui lòng đóng file và thử lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Lưu dữ liệu từ datagridview vào list
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (isSaved == true && isTabDataHaveAnyChanged == false)
            {
                ExportDataToCSV();
            }
            else if (isTabDataHaveAnyChanged == true && isSaved == false)
            {
                DialogResult dialog = MessageBox.Show("Dữ liệu trên màn hình chưa được lưu. Bạn có muốn lưu lại trước khi xuất file?", "Lưu ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    SaveData();
                    ExportDataToCSV();
                    isSaved = true;
                    isTabDataHaveAnyChanged = false;
                }
                else
                {
                    LoadDuLieuLenDgv();
                    if (dgvDataProtocol.RefreshEdit())
                    {
                        isSaved = true;
                        ExportDataToCSV();
                        isTabDataHaveAnyChanged = false;
                    }
                }
            }

        }

        //sửa cấu hình protocol
        private void btnEditProtocol_Click(object sender, EventArgs e)
        {
            isClicked = true;
            EditThietBi();

        }
        #endregion

        #region sự kiện datagridview
        private void ExportDataToCSV()
        {
            if (dgvDataProtocol.Rows.Count > 0)
            {
                if (DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol))
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "CSV (*.csv)|*.csv";
                    sfd.FileName = "DulieuCauhinh.csv";
                    bool fileError = false;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(sfd.FileName))
                        {
                            try
                            {
                                File.Delete(sfd.FileName);
                            }
                            catch (IOException ex)
                            {
                                fileError = true;
                                MessageBox.Show("Xuất dữ liệu không thành công: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        if (!fileError)
                        {
                            try
                            {
                                int columnCount = dgvDataProtocol.Columns.Count;
                                string columnNames = "";
                                string[] outputCsv = new string[dgvDataProtocol.Rows.Count];
                                int rowCount = dgvDataProtocol.Rows.Count - 1;
                                for (int i = 0; i < columnCount; i++)
                                {
                                    columnNames += dgvDataProtocol.Columns[i].HeaderText.ToString() + ",";
                                }
                                outputCsv[0] += columnNames;
                                for (int i = 1; (i - 1) < rowCount; i++)
                                {
                                    for (int j = 0; j < columnCount; j++)
                                    {
                                        outputCsv[i] += dgvDataProtocol.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                    }
                                }
                                File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                                MessageBox.Show("Xuất dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi dữ liệu nhập vào, kiểm tra lại trước khi xuất", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Không có bản ghi để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Load dữ liệu từ json lên datagridview
        public void LoadDuLieuLenDgv()
        {
            if (slave == null) return;
            dgvDataProtocol.AutoGenerateColumns = false;
            try
            {
                if (slave.dsDiemDoGiamSat.Count != 0)
                {
                    var dsDuLieuDiemDoHienThi = slave.dsDiemDoGiamSat.ElementAt(0).Value.DsDulieu.Select(x => x.Value).ToList();

                    for (int i = 1; i < slave.dsDiemDoGiamSat.Count; i++)
                    {
                        var dsDuLieuDiemDoThuI = slave.dsDiemDoGiamSat.ElementAt(i).Value.DsDulieu.Select(x => x.Value).ToList();
                        dsDuLieuDiemDoHienThi.AddRange(dsDuLieuDiemDoThuI);
                    }
                    var bindingSource = new BindingSource();
                    bindingSource.DataSource = dsDuLieuDiemDoHienThi;
                    dgvDataProtocol.DataSource = bindingSource;
                }
                else
                {
                    int rowCount = dgvDataProtocol.Rows.Count;
                    for (int n = 0; n < rowCount; n++)
                    {
                        if (dgvDataProtocol.Rows[0].IsNewRow == false)
                            dgvDataProtocol.Rows.RemoveAt(0);
                    }
                }


                /*var thietBiGiamSatDuocChon = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text];
                if (thietBiGiamSatDuocChon.dsDiemDoGiamSat.Count != 0)
                {
                    var dsDuLieuDiemDoHienThi = thietBiGiamSatDuocChon.dsDiemDoGiamSat.ElementAt(0).Value.DsDulieu.Select(x => x.Value).ToList();

                    for (int i = 1; i < thietBiGiamSatDuocChon.dsDiemDoGiamSat.Count; i++)
                    {
                        var dsDuLieuDiemDoThuI = thietBiGiamSatDuocChon.dsDiemDoGiamSat.ElementAt(i).Value.DsDulieu.Select(x => x.Value).ToList();
                        dsDuLieuDiemDoHienThi.AddRange(dsDuLieuDiemDoThuI);
                    }
                    var bindingSource = new BindingSource();
                    bindingSource.DataSource = dsDuLieuDiemDoHienThi;
                    dgvDataProtocol.DataSource = bindingSource;
                }
                else
                {
                    int rowCount = dgvDataProtocol.Rows.Count;
                    for (int n = 0; n < rowCount; n++)
                    {
                        if (dgvDataProtocol.Rows[0].IsNewRow == false)
                            dgvDataProtocol.Rows.RemoveAt(0);
                    }
                }*/
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvDataProtocol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                btnDelete.PerformClick();
            }
        }
        #endregion

        #region Sự kiện với form
        public void DongForm(bool isInFormEdit)
        {

            if (isInFormEdit == false)
            {
                if (isTabDataHaveAnyChanged == true && isTabConfigHaveAnyChanged == true && isTabSlaveChanged == true)
                {
                    SaveData();
                    ThemMoiThietBi();
                    LuuSlaveAddress();
                }
                else if(isTabSlaveChanged == true)
                {
                    LuuSlaveAddress();
                }
                else if (isTabConfigHaveAnyChanged == true)
                {
                    ThemMoiThietBi();
                }
                else
                {
                    SaveData();
                }
            }
            else if (isInFormEdit == true)
            {
                if (isTabDataHaveAnyChanged == true && isTabConfigHaveAnyChanged == true && isTabSlaveChanged == true)
                {
                    SaveData();
                    EditThietBi();
                    LuuSlaveAddress();
                }
                else if (isTabSlaveChanged == true)
                {
                    LuuSlaveAddress();
                }
                else if (isTabConfigHaveAnyChanged == true)
                {
                    EditThietBi();
                }
                else
                {
                    SaveData();
                }
            }
        }
        public bool IsFormHaveAnyChanged()
        {
            this.dgvDataProtocol.EndEdit();
            if (isTabDataHaveAnyChanged == true || isTabConfigHaveAnyChanged == true || isTabSlaveChanged == true)
            {
                return true;
            }
            return false;
        }

        #endregion

        public bool KiemTraCongCOM(String COM)
        {
            using (SerialPort serialPort = new SerialPort(COM))
            {
                try
                {
                    serialPort.Open();
                    serialPort.Close();
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #region hàm sử lý nút lưu
        private void ThemMoiThietBi()
        {
            //DocDsThietBiTuFileJson();

            TreeNode nodeTemp = this.formDataList.tvMain.SelectedNode;
            if (nodeTemp.Parent != null)
            {
                if (nodeTemp.Parent.Parent == null)
                {
                    nodeTemp = nodeTemp.Parent;
                }
            }
            if (cbProtocol.SelectedItem.ToString() == "Modbus TCP/IP")
            {
                //isValidatePassed = CheckValidateCauHinhIP();
                if (CheckValidateCauHinhIP() == false)
                {
                    MessageBox.Show("Kiểm tra dữ liệu nhập vào!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    thietBi = new ThietBiTCPIP
                    {
                        Name = txtTenGiaoThuc.Text,
                        IP = txtIPAdress.Text,
                        Port = Convert.ToInt32(txtPort.Text),
                        Protocol = cbProtocol.SelectedItem.ToString(),
                        dsSlave = new Dictionary<string, SlaveModel>(),
                    };

                    dsThietBiGiamSat.Add(thietBi.Name, thietBi);
                }
            }
            else if (cbProtocol.SelectedItem.ToString() == "Serial Port")
            {
                //isValidatePassed = CheckValidateCauHinhCOM();
                if (CheckValidateCauHinhCOM() == false)
                {
                    MessageBox.Show("Kiểm tra lại dữ liệu nhập vào!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (cbCOM.SelectedItem == null)
                    {
                        MessageBox.Show("Không có cổng COM được chọn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    thietBi = new ThietBiCOM
                    {
                        Name = txtTenGiaoThuc.Text,
                        Com = cbCOM.SelectedItem.ToString(),
                        Baud = int.Parse(cbBaud.SelectedItem.ToString()),
                        Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem.ToString()),
                        Databit = int.Parse(cbDataBit.SelectedItem.ToString()),
                        StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.SelectedItem.ToString()),
                        Protocol = cbProtocol.SelectedItem.ToString(),
                        dsSlave = new Dictionary<string, SlaveModel>(),
                    };
                    if (!KiemTraCongCOM(cbCOM.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Không thể chọn cổng này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        dsThietBiGiamSat.Add(thietBi.Name, thietBi);
                    }
                }
            }

            JsonService.AddThietBiToNhaMay("Quang Ninh", thietBi);
            //GhiDsThietBiRaFileJson();
            TreeNode node = new TreeNode(txtTenGiaoThuc.Text);
            node.Name = TreeName.Name.ThietBi.ToString();
            /*if (TVMain.SelectedNode.Parent == null)
            {
                TVMain.SelectedNode.Nodes.Add(node);
                formDataList.selectedNodeDouble = node;
            }
            else if (TVMain.SelectedNode.Parent.Parent == null)
            {
                TVMain.SelectedNode.Parent.Nodes.Add(node);
                formDataList.selectedNodeDouble = node;
            }*/
            if (isClicked == true)
            {
                MessageBox.Show("Lưu cấu hình thiết bị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isClicked = false;
            }
            node.ContextMenuStrip = formDataList.cms_ThietBi;

            formDataList.LoadTreeView();
            formDataList.tvMain.ExpandAll();
            //SaveData();
            isTabConfigHaveAnyChanged = false;
        }

        private void EditThietBi()
        {
            //DocDsThietBiTuFileJson();
            if (cbProtocol.SelectedItem.ToString() == "Modbus TCP/IP")
            {
                if (thietBi.GetType() == typeof(ThietBiTCPIP))
                {
                    ThietBiTCPIP thietBi_TCPIP = (ThietBiTCPIP)thietBi;
                    isValidatePassed = CheckValidateCauHinhIP();
                    if (CheckValidateCauHinhIP() == false)
                    {
                        MessageBox.Show("Kiểm tra lại dữ liệu nhập vào!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        if (thietBi_TCPIP != null)
                        {
                            string onlName_thietBi = thietBi.Name;

                            thietBi_TCPIP.Name = txtTenGiaoThuc.Text;
                            thietBi_TCPIP.IP = txtIPAdress.Text;
                            thietBi_TCPIP.Port = Convert.ToInt32(txtPort.Text);
                            dsThietBiGiamSat.Remove(onlName_thietBi);
                            dsThietBiGiamSat.Add(thietBi_TCPIP.Name, thietBi_TCPIP);

                            JsonService.ReplaceOldTbByNewTb(onlName_thietBi, thietBi_TCPIP, "Quang Ninh");
                            thietBi = thietBi_TCPIP;
                        }
                        /*else
                        {
                            ThietBiModel deviceObjIP = new ThietBiTCPIP
                            {
                                Name = txtTenGiaoThuc.Text,
                                IP = txtIPAdress.Text,
                                Port = Convert.ToInt32(txtPort.Text),
                                Protocol = cbProtocol.SelectedItem.ToString(),
                                dsSlave = thietBi_TCPIP.dsSlave
                            };
                            dsThietBiGiamSat.Remove(thietBi_TCPIP.Name);
                            dsThietBiGiamSat.Add(deviceObjIP.Name, deviceObjIP);
                        }*/
                    }
                }
            }
            else if (cbProtocol.SelectedItem.ToString() == "Serial Port")
            {
                isValidatePassed = CheckValidateCauHinhCOM();
                if (isValidatePassed == false)
                {
                    MessageBox.Show("Kiểm tra lại dữ liệu nhập vào!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (thietBi.GetType() == typeof(ThietBiCOM))
                    {
                        //ThietBiCOM comTemp = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text] as ThietBiCOM;
                        string onlName_thietBi = thietBi.Name;
                        ThietBiCOM thietBiCOM = (ThietBiCOM)thietBi;
                        if (thietBiCOM != null)
                        {
                            thietBiCOM.Name = txtTenGiaoThuc.Text;
                            thietBiCOM.Com = cbCOM.SelectedItem.ToString();
                            thietBiCOM.Baud = int.Parse(cbBaud.SelectedItem.ToString());
                            thietBiCOM.Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem.ToString());
                            thietBiCOM.Databit = int.Parse(cbDataBit.SelectedItem.ToString());
                            thietBiCOM.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.SelectedItem.ToString());
                            if (cbCOM.SelectedItem == null)
                            {
                                MessageBox.Show("Không có cổng COM được chọn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else if (!KiemTraCongCOM(cbCOM.SelectedItem.ToString()))
                            {
                                MessageBox.Show("Không thể chọn cổng này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                dsThietBiGiamSat.Remove(onlName_thietBi);
                                dsThietBiGiamSat.Add(thietBiCOM.Name, thietBiCOM);

                                JsonService.ReplaceOldTbByNewTb(onlName_thietBi, thietBiCOM, "Quang Ninh");
                                thietBi = thietBiCOM;
                            }
                        }
                        /*else
                        {
                            ThietBiModel deviceObjCOM = new ThietBiCOM
                            {
                                Name = txtTenGiaoThuc.Text,
                                Com = cbCOM.SelectedItem.ToString(),
                                Baud = int.Parse(cbBaud.SelectedItem.ToString()),
                                Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem.ToString()),
                                Databit = int.Parse(cbDataBit.SelectedItem.ToString()),
                                StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.SelectedItem.ToString()),
                                Protocol = cbProtocol.SelectedItem.ToString(),
                                dsSlave = thietBi_TCPIP.dsSlave
                                //dsDiemDoGiamSat = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text].dsDiemDoGiamSat

                            };
                            if (!KiemTraCongCOM(cbCOM.SelectedItem.ToString()))
                            {
                                MessageBox.Show("Không thể chọn cổng này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else
                            {
                                dsThietBiGiamSat.Remove(formDataList.selectedNodeDouble.Text);
                                dsThietBiGiamSat.Add(deviceObjCOM.Name, deviceObjCOM);
                            }
                        }*/
                    }
                }
            }

            //GhiDsThietBiRaFileJson();
            formDataList.selectedNodeDouble.Text = txtTenGiaoThuc.Text;
            if (isClicked == true)
            {
                MessageBox.Show("Lưu cấu hình thiết bị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isClicked = false;
            }
            //SaveData();
            isTabConfigHaveAnyChanged = false;
        }

        private void LuuSlaveAddress()
        {
            if (!CheckValidateCauHinhSlave())
            {
                MessageBox.Show("Lỗi lưu slave!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            var dsSlaveItem = dsThietBiGiamSat[thietBi.Name].dsSlave;
            if (dsSlaveItem.ContainsKey(txtSlaveAddress.Text) && txtSlaveAddress.Text != formDataList.selectedNodeDouble.Text)
            {
                errorSlaveAddress.SetError(txtSlaveAddress, "Slave bị trùng");
                MessageBox.Show("SlaveAddress bị trùng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (isAddSlave)
                {

                    slave = new SlaveModel();

                    // set data cho slave
                    slave.Name = txtSlaveAddress.Text;
                    slave.ScanRate = int.Parse(txtScanRate.Text);
                    //
                    try
                    {
                        thietBi.dsSlave.Add(slave.Name, slave);

                        var rs = JsonService.AddSlaveToTb("Quang Ninh", thietBi.Name, slave);
                        if (rs)
                        {
                            if(isClicked == true)
                            {
                                MessageBox.Show("Lưu slave thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                isClicked = false;
                            }
                            formDataList.LoadTreeView();
                            formDataList.tvMain.ExpandAll();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi lưu slave!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    //slave = new SlaveModel();

                    // set data cho slave
                    string old_name_slave = slave.Name;
                    slave.Name = txtSlaveAddress.Text;
                    slave.ScanRate = int.Parse(txtScanRate.Text);
                    //
                    try
                    {
                        //thietBi.dsSlave.Add(slave.Name, slave);

                        var rs = JsonService.EditSlave("Quang Ninh", thietBi.Name, old_name_slave, slave);
                        if (rs)
                        {
                            if(isClicked == true)
                            {
                                MessageBox.Show("Lưu slave thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                isClicked = false;
                            }
                            formDataList.LoadTreeView();
                            formDataList.tvMain.ExpandAll();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi lưu slave!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            isValidatePassed = true;
            isTabSlaveChanged = false;
        }
        #endregion

        

        private void dgvDataProtocol_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cellCanCheck = dgvDataProtocol[e.ColumnIndex, e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 0://tên
                    DuLieuNhapVao.KiemTraTungCellCotTen(dgvDataProtocol, cellCanCheck);
                    checkLaiTrungTenSauKhiSua();
                    checkLaiTrungDiemDoSauKhiSua();
                    break;
                case 1://điểm đo
                    DuLieuNhapVao.KiemTraTungCellCotDiemDo(dgvDataProtocol, cellCanCheck);
                    checkLaiTrungTenSauKhiSua();
                    checkLaiTrungDiemDoSauKhiSua();
                    break;
                case 2://địa chỉ
                       //kiểm tra trùng lặp kết hợp kiểm tra định dạng
                    if (DuLieuNhapVao.KiemTraTungCellCotDiaChi(dgvDataProtocol, cellCanCheck))
                    {
                        //nếu sau khi trung lặp được sửa check lại 1 lần nữa để xóa hết error message trung lặp
                        checkLaiTrungDiaChiSauKhiSua();
                    }
                    break;
                case 3://sacle
                    DuLieuNhapVao.KiemTraTungCellCotScale(dgvDataProtocol, cellCanCheck);
                    break;
                case 4://don vi đo
                    DuLieuNhapVao.KiemTraTungCellCotDonViDo(dgvDataProtocol, cellCanCheck);
                    break;
            }
        }
        private void checkLaiTrungDiaChiSauKhiSua()
        {
            foreach (DataGridViewRow rowUnit in dgvDataProtocol.Rows)
            {
                //break when in last row because last row is null row
                if (rowUnit.Index == dgvDataProtocol.Rows.Count - 1)
                {
                    break;
                }
                DataGridViewCell cellDiaChiUnit = rowUnit.Cells["diaChi"];
                if (cellDiaChiUnit.ErrorText == "Địa chỉ đã tồn tại")
                {
                    DuLieuNhapVao.KiemTraTungCellCotDiaChi(dgvDataProtocol, cellDiaChiUnit);
                }
            }
        }
        private void checkLaiTrungTenSauKhiSua()
        {
            foreach (DataGridViewRow rowUnit in dgvDataProtocol.Rows)
            {
                //break when in last row because last row is null row
                if (rowUnit.Index == dgvDataProtocol.Rows.Count - 1)
                {
                    break;
                }
                DataGridViewCell cellTenUnit = rowUnit.Cells["ten"];
                if (cellTenUnit.ErrorText == "Cặp tên và điểm đo đã tồn tại")
                {
                    DuLieuNhapVao.KiemTraTrungTenTheoDiemDo(dgvDataProtocol, cellTenUnit);
                }
            }
        }
        private void checkLaiTrungDiemDoSauKhiSua()
        {
            foreach (DataGridViewRow rowUnit in dgvDataProtocol.Rows)
            {
                //break when in last row because last row is null row
                if (rowUnit.Index == dgvDataProtocol.Rows.Count - 1)
                {
                    break;
                }
                DataGridViewCell cellDiemDoUnit = rowUnit.Cells["diemDo"];
                if (cellDiemDoUnit.ErrorText == "Cặp tên và điểm đo đã tồn tại")
                {
                    DuLieuNhapVao.KiemTraTrungDiemDoTheoTen(dgvDataProtocol, cellDiemDoUnit);
                }
            }
        }
        private void dgvDataProtocol_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            e.Control.KeyPress -= new KeyPressEventHandler(DiaChi_KeyPress);
            e.Control.KeyPress -= new KeyPressEventHandler(Scale_KeyPress);

            //nhap du lieu cot dia chi
            if (dgvDataProtocol.CurrentCell.ColumnIndex == 2)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(DiaChi_KeyPress);
                }
            }
            //nhap du lieu cot scale
            if (dgvDataProtocol.CurrentCell.ColumnIndex == 3)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Scale_KeyPress);
                }
            }

        }
        private void DiaChi_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void Scale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !(e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        public void HideTabDuLieu()
        {
            this.tabControl1.TabPages.Remove(this.tabPageDuLieu);
        }

        public void HideTabSlave()
        {
            this.tabControl1.TabPages.Remove(this.tabPageSlave);
        }

        public void HideTabCauHinh()
        {
            this.tabControl1.TabPages.Remove(this.tabPageCauHinh);
        }

        private void btnSaveSlave_Click(object sender, EventArgs e)
        {
            isClicked = true;
            LuuSlaveAddress();
        }

        public void AfterRemoveSlave()
        {
            try
            {
                this.dsThietBiGiamSat[thietBi.Name].dsSlave.Remove(slave.Name);
                this.slave = null;
                tabControl1.TabPages.Remove(tabPageSlave);
                tabControl1.TabPages.Remove(tabPageDuLieu);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        #region event controller value change
        private void dgvDataProtocol_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                isTabDataHaveAnyChanged = true;
                isSaved = false;
            }
        }

        private void txtTenGiaoThuc_TextChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;
        }

        private void cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;

            if (cbProtocol.SelectedIndex == 0)
            {
                gbSerialSettingProtocol.Enabled = false;
                gbTCPIPProtocol.Enabled = true;

            }
            else
            {
                gbSerialSettingProtocol.Enabled = true;
                gbTCPIPProtocol.Enabled = false;
                txtIPAdress.Clear();
                txtPort.Text = "";
            }
        }

        private void txtIPAdress_TextChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;
        }

        private void cbCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;
        }

        private void cbBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;
        }

        private void cbDataBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;
        }

        private void cbParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;
        }

        private void cbStopBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            isTabConfigHaveAnyChanged = true;
        }

        #endregion

        private void txtSlaveAddress_TextChanged(object sender, EventArgs e)
        {
            isTabSlaveChanged = true;
        }

        private void txtScanRate_TextChanged(object sender, EventArgs e)
        {
            isTabSlaveChanged = true;
        }
    }

}
