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
using FileExportScheduler.Controller;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.IO.Ports;

namespace FileExportScheduler
{
    public partial class ProtocolConfiguration : UserControl
    {
        #region biến toàn cục
        public Dictionary<string, ThietBiGiamSat> dsThietBiGiamSat = new Dictionary<string, ThietBiGiamSat>();
        TreeView TVMain;
        public FormDataList formDataList;
        public string tenDuLieuDuocChon;

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
        #region thao tác với json
        //Xuất từ file .json ra 1 list
        private void DocDsThietBiTuFileJson()
        {
            try
            {
                var path = GetPathJson.getPathConfig("DeviceAndData.json");
                dsThietBiGiamSat.Clear();
                JObject jsonObj = JObject.Parse(File.ReadAllText(path));
                Dictionary<string, ThietBiIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiIP>>();
                foreach (var deviceIPUnit in deviceIP)
                {
                    if (deviceIPUnit.Value.Protocol == "Modbus TCP/IP" || deviceIPUnit.Value.Protocol == "Siemens S7-1200")
                    {
                        dsThietBiGiamSat.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                    }
                }
                Dictionary<string, ComConfigModel> deviceCom = jsonObj.ToObject<Dictionary<string, ComConfigModel>>();
                foreach (var deviceComUnit in deviceCom)
                {
                    if (deviceComUnit.Value.Protocol == "Serial Port")
                    {
                        dsThietBiGiamSat.Add(deviceComUnit.Key, deviceComUnit.Value);
                    }
                }
            }
            catch (Exception ex)
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
                errorPort.SetError(txtPort, "Chưa nhập cổng đỉa chị");
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
        //check nhập vào bên dữ liệu protocol
        private bool validation()
        {
            List<string> dsKeyDiemDoVaChat = new List<string>();
            dsKeyDiemDoVaChat.Clear();
            bool isPassed = true;
            foreach (DataGridViewRow dr in dgvDataProtocol.Rows)
            {
                if (dr.Index == dgvDataProtocol.Rows.Count - 1)
                {
                    break;
                }

                for (int i = 0; i < dr.Cells.Count - 1; i++)
                {
                    if (dr.Cells[i].ColumnIndex == 5)
                    {
                        break;
                    }

                    if (dr.Cells[i].Value == null || dr.Cells[i].Value.ToString().Trim() == string.Empty)
                    {
                        dr.Cells[i].ErrorText = "Không được để trống";
                        isPassed = false;
                        continue;
                    }
                    else
                    {
                        if (!Regex.IsMatch(dr.Cells[i].Value.ToString(), @"^[a-zA-Z0-9_.-]+$"))
                        {
                            dr.Cells[i].ErrorText = "Sai định dạng";
                            isPassed = false;
                        }
                        else
                        {
                            dr.Cells[i].ErrorText = "";
                        }
                    }

                    if (dsKeyDiemDoVaChat.Contains(dr.Cells[0].Value.ToString() + dr.Cells[1].Value.ToString()))
                    {
                        if (dr.Cells[i].ColumnIndex == 0 || dr.Cells[i].ColumnIndex == 1)
                        {
                            dr.Cells[i].ErrorText = "Tên bị trùng";
                        }

                        isPassed = false;
                    }

                    if (dr.Cells[i].ColumnIndex == 2)
                    {
                        if (!CheckAddress(dr.Cells[i].Value.ToString()))
                        {
                            dr.Cells[i].ErrorText = "Sai định dạng";
                            isPassed = false;
                        }
                        else
                        {
                            dr.Cells[i].ErrorText = "";
                        }
                    }
                    if (dr.Cells[i].ColumnIndex == 3)
                    {
                        var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                        if (!regex.IsMatch(dr.Cells[i].Value.ToString()))
                        {
                            dr.Cells[i].ErrorText = "Scale Sai định dạng";
                            isPassed = false;
                        }

                    }

                }

                dsKeyDiemDoVaChat.Add(dr.Cells[0].Value.ToString() + dr.Cells[1].Value.ToString());
            }
            if (dsKeyDiemDoVaChat.Count == 0)
            {

                isPassed = false;
            }
            return isPassed;
        }
        //check nhập vào địa chỉ dữ liệu protocol
        private bool CheckAddress(string addressStr)
        {
            if (addressStr.Length != 5)
                return false;
            bool error = true;
            try
            {
                int address = Convert.ToInt32(addressStr);
                if (address < 0 || (address > 19999 && address < 30000) || (address > 39999 && address < 40000) || address > 49999)
                {
                    error = false;
                }
            }
            catch (Exception ex)
            {
                error = false;
            }
            return error;
        }
        #endregion

        #region Sự kiện nút
        //button đóng tab
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }
        private void txtIPAdress_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        //thêm dữ liệu protocol
        private void btnAddData_Click(object sender, EventArgs e)
        {
            var thietBiGiamSatDuocChon = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text];
            if (validation())
            {
                thietBiGiamSatDuocChon.dsDiemDoGiamSat = XuLyDanhSachDiemDo.LayDsDiemDoTuDgv(dgvDataProtocol);
                GhiDsThietBiRaFileJson();
                MessageBox.Show("Lưu dữ liệu thành công!", "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lỗi lưu dữ liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //Xóa dữ liệu protocol
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            DialogResult dl = MessageBox.Show("Bạn có muốn xóa ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            try
            {
                if (dl == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dgvDataProtocol.SelectedRows)//đọc danh sách các dòng dữ liệu được chọn
                    {
                        if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                        {
                            dsThietBiGiamSat[txtTenGiaoThuc.Text].//lấy ra thiết bị
                                 dsDiemDoGiamSat[row.Cells[1].Value.ToString()].//lấy ra điểm đo
                                     DsDulieu.Remove(row.Cells[0].Value.ToString());//xóa 1 dữ liệu trong danh sách dữ liệu
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
                    GhiDsThietBiRaFileJson();
                }
            }
            catch (Exception ex)
            {

            }

        }
        //lưu cấu hình protocol
        private void btnSaveProtocol_Click_1(object sender, EventArgs e)
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
                if (CheckValidateCauHinhThietBi() == false)
                {
                    return;
                }
                ThietBiGiamSat deviceObj = new ThietBiIP
                {
                    Name = txtTenGiaoThuc.Text,
                    IP = txtIPAdress.Text,
                    Port = Convert.ToInt32(txtPort.Text),
                    Protocol = cbProtocol.SelectedItem.ToString(),
                    TypeModel = TypeEnum.Protocol,
                    dsDiemDoGiamSat = new Dictionary<string, DiemDoGiamSat>(),
                };

                dsThietBiGiamSat.Add(deviceObj.Name, deviceObj);
            }
            else if (cbProtocol.SelectedItem.ToString() == "Serial Port")
            {
                ThietBiGiamSat deviceObj1 = new ComConfigModel
                {
                    Name = txtTenGiaoThuc.Text,
                    Com = cbCOM.SelectedItem.ToString(),
                    Baud = int.Parse(cbBaud.SelectedItem.ToString()),
                    parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem.ToString()),
                    Databit = int.Parse(cbDataBit.SelectedItem.ToString()),
                    stopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.SelectedItem.ToString()),
                    TypeModel = TypeEnum.Protocol,
                    Protocol = cbProtocol.SelectedItem.ToString(),
                    dsDiemDoGiamSat = new Dictionary<string, DiemDoGiamSat>(),
                };
                dsThietBiGiamSat.Add(deviceObj1.Name, deviceObj1);
            }

            GhiDsThietBiRaFileJson();
            TreeNode node = new TreeNode(txtTenGiaoThuc.Text);
            if (TVMain.SelectedNode.Parent == null)
            {
                TVMain.SelectedNode.Nodes.Add(node);
            }
            else if (TVMain.SelectedNode.Parent.Parent == null)
            {
                TVMain.SelectedNode.Parent.Nodes.Add(node);
            }

            node.ContextMenuStrip = formDataList.tx2;

            MessageBox.Show("Lưu thành công !", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void btnImport_Click(object sender, EventArgs e)
        {
         var openFile=   openFileDialog1.ShowDialog();
            if(openFile = DialogResult.OK)
            {
                BindDataFromCSV(openFileDialog1.FileName);

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
                    dt.Rows.Add(t[0], t[1], t[2], t[3], t[4]);
                }

                dgvDataProtocol.DataSource = dt;
                if (validation())
                {
                    MessageBox.Show("Đọc dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Dữ liệu sai định dạng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (IOException ex)
            {

                MessageBox.Show("File đang mở, vui lòng đóng file và thử lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //Lưu dữ liệu từ datagridview vào list
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvDataProtocol.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Dulieu_cauhinh.csv";
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
                            MessageBox.Show("Không thể xuất dữ liệu" + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dgvDataProtocol.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dgvDataProtocol.Rows.Count + 1];
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
                            MessageBox.Show("Xuất dữ liệu thành công", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có bản ghi", "Info");
            }
        }

        //sửa cấu hình protocol
        private void btnEditProtocol_Click_1(object sender, EventArgs e)
        {
            DocDsThietBiTuFileJson();

            if (cbProtocol.SelectedItem.ToString() == "Modbus TCP/IP")
            {
                if (!CheckValidateCauHinhThietBi())//kiểm tra validation
                {
                    return;
                }

                ThietBiIP thietBi = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text] as ThietBiIP;
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
                    ThietBiGiamSat deviceObj = new ThietBiIP
                    {
                        Name = txtTenGiaoThuc.Text,
                        IP = txtIPAdress.Text,
                        Port = Convert.ToInt32(txtPort.Text),
                        Protocol = cbProtocol.SelectedItem.ToString(),
                        TypeModel = TypeEnum.Protocol,
                        dsDiemDoGiamSat = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text].dsDiemDoGiamSat
                    };
                    dsThietBiGiamSat.Remove(formDataList.selectedNodeDouble.Text);
                    dsThietBiGiamSat.Add(deviceObj.Name, deviceObj);
                }
            }
            else if (cbProtocol.SelectedItem.ToString() == "Serial Port")
            {
                ComConfigModel comTemp = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text] as ComConfigModel;
                if (comTemp != null)
                {
                    comTemp.Name = txtTenGiaoThuc.Text;
                    comTemp.Com = cbCOM.SelectedItem.ToString();
                    comTemp.Baud = int.Parse(cbBaud.SelectedItem.ToString());
                    comTemp.parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem.ToString());
                    comTemp.Databit = int.Parse(cbDataBit.SelectedItem.ToString());
                    comTemp.stopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.SelectedItem.ToString());
                    dsThietBiGiamSat.Remove(formDataList.selectedNodeDouble.Text);
                    dsThietBiGiamSat.Add(comTemp.Name, comTemp);
                }
                else
                {
                    ThietBiGiamSat deviceObj1 = new ComConfigModel
                    {
                        Name = txtTenGiaoThuc.Text,
                        Com = cbCOM.SelectedItem.ToString(),
                        Baud = int.Parse(cbBaud.SelectedItem.ToString()),
                        parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem.ToString()),
                        Databit = int.Parse(cbDataBit.SelectedItem.ToString()),
                        stopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.SelectedItem.ToString()),
                        TypeModel = TypeEnum.Protocol,
                        Protocol = cbProtocol.SelectedItem.ToString(),
                        dsDiemDoGiamSat = dsThietBiGiamSat[formDataList.selectedNodeDouble.Text].dsDiemDoGiamSat

                    };
                    dsThietBiGiamSat.Remove(formDataList.selectedNodeDouble.Text);
                    dsThietBiGiamSat.Add(deviceObj1.Name, deviceObj1);
                }

            }
            GhiDsThietBiRaFileJson();
            formDataList.selectedNodeDouble.Text = txtTenGiaoThuc.Text;

            MessageBox.Show("Lưu thành công !", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            catch (Exception ex)
            {

            }
        }
        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
