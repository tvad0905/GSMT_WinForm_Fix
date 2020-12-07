using System;
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

namespace FileExportScheduler
{
    public partial class ProtocolConfiguration : UserControl
    {
        #region biến toàn cục
        public Dictionary<string, ThietBiModel> dsThietBiGiamSat = new Dictionary<string, ThietBiModel>();
        TreeView TVMain;
        public FormDataList formDataList;
        public string tenDuLieuDuocChon;
        public bool isValidatePassed { get; set; }
        public bool isDataGridViewHaveAnyChanged;
        public bool isFormHaveAnyChanged { get; set; }
        #endregion
        public ProtocolConfiguration(FormDataList formDataList)
        {
            InitializeComponent();
            this.TVMain = formDataList.tvMain;
            this.formDataList = formDataList;
            DocDsThietBiTuFileJson();
            LoadDuLieuLenDgv();
            cbProtocol.SelectedIndex = cbProtocol.Items.IndexOf("Modbus TCP/IP");
            cbCOM.SelectedIndex = cbCOM.Items.IndexOf("COM1");
            cbBaud.SelectedIndex = cbBaud.Items.IndexOf("9600");
            cbDataBit.SelectedIndex = cbDataBit.Items.IndexOf("8");
            cbParity.SelectedIndex = cbParity.Items.IndexOf("Even");
            cbStopBit.SelectedIndex = cbStopBit.Items.IndexOf("1");
        }
        #region Thao tác với json
        //Xuất từ file .json ra 1 list
        private void DocDsThietBiTuFileJson()
        {
            try
            {
                dsThietBiGiamSat.Clear();
                dsThietBiGiamSat = ThietBiGiamSatService.GetDsThietBi();
            }
            catch
            {
            }
        }
        //Viết từ list vào file .json
        public void GhiDsThietBiRaFileJson()
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            string jsonString = (new JavaScriptSerializer()).Serialize((object)dsThietBiGiamSat);
            File.WriteAllText(path, jsonString);
        }
        #endregion

        #region Kiểm tra nhập vào
        //check nhập vào bên cấu hình protocol
        public bool CheckValidateCauHinhThietBi()
        {

            bool error = true;
            if (cbProtocol.SelectedIndex == -1)
            {
                errorGiaoThuc.SetError(cbProtocol, "Chưa chọn giao thức");
                error = false;
            }

            Match PortRegex = Regex.Match(txtPort.Text, @"^()([1-9]|[1-5]?[0-9]{2,4}|6[1-4][0-9]{3}|65[1-4][0-9]{2}|655[1-2][0-9]|6553[1-5])$");
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
        #endregion

        #region Sự kiện nút

        private void txtIPAdress_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        //Thêm dữ liệu protocol
        private void btnAddData_Click(object sender, EventArgs e)
        {
            nhapData();
        }
        private void nhapData()
        {
            isValidatePassed = DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol);
            if (DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol))
            {
                LuuDanhMucDuLieuVaoJson();
                isDataGridViewHaveAnyChanged = false;
                MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lưu dữ liệu không thành công!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //Xóa dữ liệu protocol
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dl = MessageBox.Show("Bạn có muốn xóa ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            try
            {
                if (dl == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dgvDataProtocol.SelectedRows)//đọc danh sách các dòng dữ liệu được chọn
                    {
                        var b = row.Cells[0].Value;
                        if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                        {
                            try
                            {
                                var diemDo = dsThietBiGiamSat[txtTenGiaoThuc.Text].//lấy ra thiết bị
                                                                 dsDiemDoGiamSat[row.Cells[1].Value.ToString()];//lấy ra điểm đo
                                diemDo.DsDulieu.Remove(row.Cells[0].Value.ToString());//xóa 1 dữ liệu trong danh sách dữ liệu
                                if (diemDo.DsDulieu.Count() == 0)// xóa điểm đo khi dữ liệu của điểm đo trống
                                {
                                    dsThietBiGiamSat[txtTenGiaoThuc.Text].//lấy ra thiết bị
                                           dsDiemDoGiamSat.Remove(diemDo.TenDiemDo);
                                }
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            if (row.IsNewRow == false)
                            {
                                dgvDataProtocol.Rows.Remove(row);
                            }
                            continue;
                        }
                        dgvDataProtocol.Rows.Remove(row);
                    }

                    isValidatePassed = DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol);
                    if (DuLieuNhapVao.KiemTraDuLieuNhapVao(dgvDataProtocol))
                    {
                        LuuDanhMucDuLieuVaoJson();
                        MessageBox.Show("Xóa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Dữ liệu nhập vào lỗi vui lòng kiểm tra lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Xóa dữ liệu không thành công!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //lưu cấu hình protocol
        private void btnSaveProtocol_Click(object sender, EventArgs e)
        {
            ThemMoiDuocClick();
        }



        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Nhập dữ liệu sẽ làm mất dữ liệu cũ trên màn hình, bạn có muốn tiếp tục?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    var openFile = openFileDialog1.ShowDialog();
                    if (openFile == DialogResult.OK)
                    {
                        BindDataFromCSV(openFileDialog1.FileName);
                        MessageBox.Show("Nhập dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var thietBiGiamSatDuocChon = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text];
                thietBiGiamSatDuocChon.dsDiemDoGiamSat = DanhSachDiemDoService.LayDsDiemDoTuDgv(dgvDataProtocol);
                var maxAddress = MaxAddress.Get(thietBiGiamSatDuocChon);
                thietBiGiamSatDuocChon.MaxAddressCoils = (ushort)maxAddress[0];
                thietBiGiamSatDuocChon.MaxAddressInputs = (ushort)maxAddress[1];
                thietBiGiamSatDuocChon.MaxAddressInputRegisters = (ushort)maxAddress[2];
                thietBiGiamSatDuocChon.MaxAddressHoldingRegisters = (ushort)maxAddress[3];
                GhiDsThietBiRaFileJson();
            }
            catch (Exception ex) { }

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

        //sửa cấu hình protocol
        private void btnEditProtocol_Click(object sender, EventArgs e)
        {
            EditDuocClick();
        }
        #endregion

        #region sự kiện datagridview
        //Load dữ liệu từ json lên datagridview
        public void LoadDuLieuLenDgv()
        {
            dgvDataProtocol.AutoGenerateColumns = false;
            try
            {
                var thietBiGiamSatDuocChon = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text];
                var dsDuLieuDiemDoHienThi = thietBiGiamSatDuocChon.dsDiemDoGiamSat.ElementAt(0).Value.DsDulieu.Select(x => x.Value).ToList();//lấy danh sách dữ liệu của điểm đo đầu tiên
                for (int i = 1; i < thietBiGiamSatDuocChon.dsDiemDoGiamSat.Count; i++)
                {
                    var dsDuLieuDiemDoThuI = thietBiGiamSatDuocChon.dsDiemDoGiamSat.ElementAt(i).Value.DsDulieu.Select(x => x.Value).ToList();
                    dsDuLieuDiemDoHienThi.AddRange(dsDuLieuDiemDoThuI);
                }
                var bindingSource = new BindingSource();
                bindingSource.DataSource = dsDuLieuDiemDoHienThi;
                dgvDataProtocol.DataSource = bindingSource;
            }
            catch (Exception)
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
                ThemMoiDuocClick();
            }
            else if (isInFormEdit == true)
            {
                EditDuocClick();
            }
        }
        public bool IsFormHaveAnyChanged()
        {
            this.dgvDataProtocol.EndEdit();

            if (isDataGridViewHaveAnyChanged)
            {
                isFormHaveAnyChanged = true;
            }
            
            return isFormHaveAnyChanged;
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
        private void ThemMoiDuocClick()
        {
            DocDsThietBiTuFileJson();

            TreeNode nodeTemp = TVMain.SelectedNode;
            if (nodeTemp.Parent != null)
            {
                if (nodeTemp.Parent.Parent == null)
                {
                    nodeTemp = nodeTemp.Parent;
                }
            }
            if (cbProtocol.SelectedItem.ToString() == "Modbus TCP/IP")
            {
                isValidatePassed = CheckValidateCauHinhThietBi();
                if (CheckValidateCauHinhThietBi() == false)
                {
                    return;
                }
                ThietBiModel deviceObj = new ThietBiTCPIP
                {
                    Name = txtTenGiaoThuc.Text,
                    IP = txtIPAdress.Text,
                    Port = Convert.ToInt32(txtPort.Text),
                    Protocol = cbProtocol.SelectedItem.ToString(),
                    dsDiemDoGiamSat = new Dictionary<string, DiemDoModel>(),
                };

                dsThietBiGiamSat.Add(deviceObj.Name, deviceObj);
            }
            else if (cbProtocol.SelectedItem.ToString() == "Serial Port")
            {

                if (cbCOM.SelectedItem == null)
                {
                    MessageBox.Show("Không có cổng COM được chọn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ThietBiModel deviceObj1 = new ThietBiCOM
                {
                    Name = txtTenGiaoThuc.Text,
                    Com = cbCOM.SelectedItem.ToString(),
                    Baud = int.Parse(cbBaud.SelectedItem.ToString()),
                    Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem.ToString()),
                    Databit = int.Parse(cbDataBit.SelectedItem.ToString()),
                    StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.SelectedItem.ToString()),
                    Protocol = cbProtocol.SelectedItem.ToString(),
                    dsDiemDoGiamSat = new Dictionary<string, DiemDoModel>(),
                };
                if (!KiemTraCongCOM(cbCOM.SelectedItem.ToString()))
                {
                    MessageBox.Show("Không thể chọn cổng này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    dsThietBiGiamSat.Add(deviceObj1.Name, deviceObj1);
                }

            }

            GhiDsThietBiRaFileJson();
            TreeNode node = new TreeNode(txtTenGiaoThuc.Text);
            if (TVMain.SelectedNode.Parent == null)
            {
                TVMain.SelectedNode.Nodes.Add(node);
                formDataList.selectedNodeDouble = node;
            }
            else if (TVMain.SelectedNode.Parent.Parent == null)
            {
                TVMain.SelectedNode.Parent.Nodes.Add(node);
                formDataList.selectedNodeDouble = node;
            }
            node.ContextMenuStrip = formDataList.tx2;
            nhapData();
            isFormHaveAnyChanged = false;
        }
        private void EditDuocClick()
        {
            DocDsThietBiTuFileJson();
            if (cbProtocol.SelectedItem.ToString() == "Modbus TCP/IP")
            {
                isValidatePassed = CheckValidateCauHinhThietBi();
                if (CheckValidateCauHinhThietBi() == false)
                {
                    return;
                }

                ThietBiTCPIP thietBi = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text] as ThietBiTCPIP;
                if (thietBi != null)
                {
                    thietBi.Name = txtTenGiaoThuc.Text;
                    thietBi.IP = txtIPAdress.Text;
                    thietBi.Port = Convert.ToInt32(txtPort.Text);
                    dsThietBiGiamSat.Remove(formDataList.selectedNodeDouble.Text);
                    dsThietBiGiamSat.Add(thietBi.Name, thietBi);
                }
                else
                {
                    ThietBiModel deviceObjIP = new ThietBiTCPIP
                    {
                        Name = txtTenGiaoThuc.Text,
                        IP = txtIPAdress.Text,
                        Port = Convert.ToInt32(txtPort.Text),
                        Protocol = cbProtocol.SelectedItem.ToString(),
                        dsDiemDoGiamSat = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text].dsDiemDoGiamSat
                    };
                    dsThietBiGiamSat.Remove(formDataList.selectedNodeDouble.Text);
                    dsThietBiGiamSat.Add(deviceObjIP.Name, deviceObjIP);
                }
            }
            else if (cbProtocol.SelectedItem.ToString() == "Serial Port")
            {
                if (cbCOM.SelectedItem == null)
                {
                    MessageBox.Show("Không có cổng COM được chọn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ThietBiCOM comTemp = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text] as ThietBiCOM;
                if (comTemp != null)
                {
                    comTemp.Name = txtTenGiaoThuc.Text;
                    comTemp.Com = cbCOM.SelectedItem.ToString();
                    comTemp.Baud = int.Parse(cbBaud.SelectedItem.ToString());
                    comTemp.Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem.ToString());
                    comTemp.Databit = int.Parse(cbDataBit.SelectedItem.ToString());
                    comTemp.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.SelectedItem.ToString());
                    if (!KiemTraCongCOM(cbCOM.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Không thể chọn cổng này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        dsThietBiGiamSat.Remove(formDataList.selectedNodeDouble.Text);
                        dsThietBiGiamSat.Add(comTemp.Name, comTemp);
                    }
                }
                else
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
                        dsDiemDoGiamSat = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text].dsDiemDoGiamSat

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
                }
            }
            GhiDsThietBiRaFileJson();
            formDataList.selectedNodeDouble.Text = txtTenGiaoThuc.Text;
            nhapData();
            isFormHaveAnyChanged = false;
        }
        #endregion


        #region event controller value change
        private void dgvDataProtocol_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                isDataGridViewHaveAnyChanged = true;
            }
        }
        private void txtTenGiaoThuc_TextChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;
        }
        private void cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;

            if (cbProtocol.SelectedIndex == 0)
            {
                gbSerialSettingProtocol.Enabled = false;
                gbTCPIPProtocol.Enabled = true;
            }
            else
            {
                gbSerialSettingProtocol.Enabled = true;
                gbTCPIPProtocol.Enabled = false;
            }
        }
        private void txtIPAdress_TextChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;
        }
        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;
        }
        private void cbCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;
        }
        private void cbBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;
        }
        private void cbDataBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;
        }
        private void cbParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;
        }
        private void cbStopBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            isFormHaveAnyChanged = true;
        }
        #endregion


    }
}
