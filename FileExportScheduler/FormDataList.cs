using ESProtocolConverter.Models.Common;
using ESProtocolConverter.Models.NhaMay;
using ESProtocolConverter.Service.Json;
using FileExportScheduler.Models;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.Json;
using FileExportScheduler.Service.ThietBi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace FileExportScheduler
{
    public partial class FormDataList : Form
    {
        #region biến toàn cục
        // List<Device> lstDevices;
        //Dictionary<string, ThietBiModel> deviceDic = new Dictionary<string, ThietBiModel>();
        Dictionary<string, NhaMayModel> dicNhaMay = new Dictionary<string, NhaMayModel>();
        public TreeNode selectedNode = new TreeNode();
        public TreeNode selectedNodeDouble ;
        private ProtocolConfiguration formProtocolConfiguration;
        private bool isInFormEdit;

        #endregion
        public FormDataList()
        {
            InitializeComponent();
        }

        //set context menu strip cho các node
        private void setMenu(TreeNode node)
        {

            if (node.Name == TreeName.Name.root.ToString())
            {
                //node.ContextMenuStrip = themThietBi_Menu;
            }
            else if (node.Name == TreeName.Name.ThietBi.ToString())
            {
                node.ContextMenuStrip = themThietBi_Menu;
            }
            else if (node.Name == TreeName.Name.DiemDo.ToString())
            {
                node.ContextMenuStrip = xoa_Menu;
            }


            /*if (node.Name.ToLower() == "devices" || node.Name.ToLower() == "protocols")
            {
                node.ContextMenuStrip = ctxMenu;
            }
            else
            {
                node.ContextMenuStrip = tx2;
            }*/
        }

        //đọc file json ra list
        private void JsonToList()
        {
            try
            {
                dicNhaMay.Clear();

                dicNhaMay = JsonService.GetDicNhaMay();

                /*foreach (var nhaMay in dicNhaMay)
                {
                    foreach (var thietbi in nhaMay.Value.dsThietBi)
                    {
                        if (thietbi.Value.Protocol == "Modbus TCP/IP" || thietbi.Value.Protocol == "Siemens S7-1200")
                        {
                            deviceDic.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                        }

                        if (deviceComUnit.Value.Protocol == "Serial Port")
                        {
                            deviceDic.Add(deviceComUnit.Key, deviceComUnit.Value);
                        }
                    }
                    *//*Dictionary<string, ThietBiTCPIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiTCPIP>>();
                    foreach (var deviceIPUnit in deviceIP)
                    {
                        if (deviceIPUnit.Value.Protocol == "Modbus TCP/IP" || deviceIPUnit.Value.Protocol == "Siemens S7-1200")
                        {
                            deviceDic.Add(deviceIPUnit.Key, deviceIPUnit.Value);
                        }
                    }
                    Dictionary<string, ThietBiCOM> deviceCom = jsonObj.ToObject<Dictionary<string, ThietBiCOM>>();
                    foreach (var deviceComUnit in deviceCom)
                    {
                        if (deviceComUnit.Value.Protocol == "Serial Port")
                        {
                            deviceDic.Add(deviceComUnit.Key, deviceComUnit.Value);
                        }
                    }*//*
                }*/

            }
            catch(Exception ex)
            {

            }
        }

        //viết vào file json
        private void WriteListObjectToJson()
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            string jsonString = (new JavaScriptSerializer()).Serialize((object)dicNhaMay);
            File.WriteAllText(path, jsonString);

        }

        #region sự kiện của form
        public void LoadTreeView()
        {
            JsonToList();

            foreach (KeyValuePair<string, NhaMayModel> nhaMay in dicNhaMay)
            {
                TreeNode node_nhaMay = new TreeNode(nhaMay.Value.Name);

                node_nhaMay.Name = TreeName.Name.NhaMay.ToString();

                //tvMain.Nodes[TreeName.Name.root.ToString()].Nodes.Add(node_nhaMay);

                foreach (var thietBi in nhaMay.Value.dsThietBi)
                {
                    TreeNode node_thietBi = new TreeNode(thietBi.Value.Name);

                    node_thietBi.Name = TreeName.Name.ThietBi.ToString(); ;

                    //node_nhaMay.Nodes.Add(node_thietBi);
                    tvMain.Nodes[TreeName.Name.root.ToString()].Nodes.Add(node_thietBi);

                    foreach (var slave in thietBi.Value.dsSlave)
                    {
                        TreeNode node_slave = new TreeNode(slave.Value.Name);

                        node_slave.Name = TreeName.Name.SlaveAddress.ToString(); ;

                        //node_thietBi.Nodes.Add(node_slave);

                        node_thietBi.Nodes.Add(node_slave);

                        /*foreach (var diemDo in slave.Value.dsDiemDoGiamSat)
                        {
                            TreeNode node_diemDo = new TreeNode(diemDo.Value.TenDiemDo);

                            node_diemDo.Name = TreeName.Name.DiemDo.ToString(); ;

                            //node_slave.Nodes.Add(node_diemDo);
                            node_slave.Nodes.Add(node_diemDo);

                            setMenu(node_diemDo);
                        }*/

                        setMenu(node_slave);
                    }

                    setMenu(node_thietBi);
                }

                //setMenu(node_nhaMay);
            }

            /*foreach (KeyValuePair<string, ThietBiModel> device in deviceDic)
            {
                TreeNode node = new TreeNode(device.Value.Name);

                node.Name = "";

                tvMain.Nodes["Protocols"].Nodes.Add(node);
            }
            foreach (TreeNode node in tvMain.Nodes)
            {
                foreach (TreeNode _node in node.Nodes)
                {
                    setMenu(_node);
                }
                setMenu(node);
            }*/
            WindowState = FormWindowState.Maximized;
        }



        private void FormDataList_Load(object sender, EventArgs e)
        {
            LoadTreeView();
            /*ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);
            protocolConfiguration.cbCOM.DataSource = SerialPort.GetPortNames();

            formProtocolConfiguration = protocolConfiguration;
            formProtocolConfiguration.isTabConfigHaveAnyChanged = false;
            formProtocolConfiguration.isTabDataHaveAnyChanged = false;*/
        }

        /*private void addThietBi(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            //selectedNodeDouble = sender.;
            var ports = SerialPort.GetPortNames();
            //JsonToList();

            splitContainer.Panel2.Controls.Clear();

            ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);
            protocolConfiguration.Dock = DockStyle.Fill;
            protocolConfiguration.txtTenGiaoThuc.Text = node.Text;
            protocolConfiguration.cbCOM.DataSource = ports;
            protocolConfiguration.btnEditProtocol.Visible = true;
            protocolConfiguration.btnSaveProtocol.Visible = false;
            ThietBiTCPIP deviceTemp = deviceDic[protocolConfiguration.txtTenGiaoThuc.Text] as ThietBiTCPIP;
            if (deviceTemp == null)
            {
                ThietBiCOM comTemp = deviceDic[protocolConfiguration.txtTenGiaoThuc.Text] as ThietBiCOM;
                protocolConfiguration.cbCOM.Text = comTemp.Com;
                protocolConfiguration.cbBaud.Text = comTemp.Baud.ToString();
                protocolConfiguration.cbParity.Text = comTemp.Parity.ToString();
                protocolConfiguration.cbDataBit.Text = comTemp.Databit.ToString();
                protocolConfiguration.cbStopBit.Text = comTemp.StopBits.ToString();
                protocolConfiguration.cbProtocol.Text = comTemp.Protocol.ToString();
            }
            else
            {
                protocolConfiguration.txtIPAdress.Text = deviceTemp.IP;
                protocolConfiguration.txtPort.Text = deviceTemp.Port.ToString();
                protocolConfiguration.cbProtocol.Text = deviceTemp.Protocol.ToString();

            }
            splitContainer.Panel2.Controls.Add(protocolConfiguration);
            formProtocolConfiguration = protocolConfiguration;//lưu vào biến toàn cục
            isInFormEdit = true;
            formProtocolConfiguration.isTabConfigHaveAnyChanged = false;
            formProtocolConfiguration.isTabDataHaveAnyChanged = false;
        }*/

        private void tvMain_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = tvMain.SelectedNode;
            selectedNodeDouble = tvMain.SelectedNode;
            var ports = SerialPort.GetPortNames();
            //JsonToList();
            splitContainer.Panel2.Controls.Clear();

            if (node != null && ( node.Name == TreeName.Name.SlaveAddress.ToString() || node.Name == TreeName.Name.ThietBi.ToString()))
            {

                ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);
                protocolConfiguration.Dock = DockStyle.Fill;

                protocolConfiguration.cbCOM.DataSource = ports;
                protocolConfiguration.btnEditProtocol.Visible = true;
                protocolConfiguration.btnSaveProtocol.Visible = false;

                string thietBi_name = node.Name == TreeName.Name.ThietBi.ToString() ? node.Text : node.Parent.Text;
                string slave_name = node.Name == TreeName.Name.SlaveAddress.ToString() ? node.Text : null;

                protocolConfiguration.txtTenGiaoThuc.Text = thietBi_name;

                ThietBiModel thietBi_model = ThietBiGiamSatService.GetThietBiGiamSat("Quang Ninh", thietBi_name);
        
                protocolConfiguration.SetThietBiAndSlave(thietBi_model, slave_name);
                protocolConfiguration.SetDsThietBi(ThietBiGiamSatService.GetDsThietBi("Quang Ninh1"));
                
                if(node.Name == TreeName.Name.SlaveAddress.ToString())
                {
                    protocolConfiguration.LoadDuLieuLenDgv();
                }
                
                
                if(node.Name == TreeName.Name.ThietBi.ToString())
                {
                    protocolConfiguration.HideTabDuLieu();
                }
                
                try
                {
                    ThietBiTCPIP deviceTemp = (ThietBiTCPIP)thietBi_model;
                    protocolConfiguration.txtIPAdress.Text = deviceTemp.IP;
                    protocolConfiguration.txtPort.Text = deviceTemp.Port.ToString();
                    protocolConfiguration.cbProtocol.Text = deviceTemp.Protocol.ToString();
                }
                catch
                {
                    ThietBiCOM deviceTemp = (ThietBiCOM)thietBi_model;
                    protocolConfiguration.cbCOM.Text = deviceTemp.Com;
                    protocolConfiguration.cbBaud.Text = deviceTemp.Baud.ToString();
                    protocolConfiguration.cbParity.Text = deviceTemp.Parity.ToString();
                    protocolConfiguration.cbDataBit.Text = deviceTemp.Databit.ToString();
                    protocolConfiguration.cbStopBit.Text = deviceTemp.StopBits.ToString();
                    protocolConfiguration.cbProtocol.Text = deviceTemp.Protocol.ToString();
                }


                splitContainer.Panel2.Controls.Add(protocolConfiguration);
                formProtocolConfiguration = protocolConfiguration;//lưu vào biến toàn cục
                isInFormEdit = true;
                formProtocolConfiguration.isTabConfigHaveAnyChanged = false;
                formProtocolConfiguration.isTabDataHaveAnyChanged = false;

            }
        }


        /*private void tvMain_DoubleClick(object sender, EventArgs e)
        {
            selectedNodeDouble = tvMain.SelectedNode;
            var ports = SerialPort.GetPortNames();
            JsonToList();
            if (tvMain.SelectedNode.Parent != null)
            {
                if (tvMain.SelectedNode.Parent.Name.ToLower() == "protocols")
                {
                    splitContainer.Panel2.Controls.Clear();

                    ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);
                    protocolConfiguration.Dock = DockStyle.Fill;
                    protocolConfiguration.txtTenGiaoThuc.Text = tvMain.SelectedNode.Text;
                    protocolConfiguration.cbCOM.DataSource = ports;
                    protocolConfiguration.btnEditProtocol.Visible = true;
                    protocolConfiguration.btnSaveProtocol.Visible = false;
                    ThietBiTCPIP deviceTemp = deviceDic[protocolConfiguration.txtTenGiaoThuc.Text] as ThietBiTCPIP;
                    if (deviceTemp == null)
                    {
                        ThietBiCOM comTemp = deviceDic[protocolConfiguration.txtTenGiaoThuc.Text] as ThietBiCOM;
                        protocolConfiguration.cbCOM.Text = comTemp.Com;
                        protocolConfiguration.cbBaud.Text = comTemp.Baud.ToString();
                        protocolConfiguration.cbParity.Text = comTemp.Parity.ToString();
                        protocolConfiguration.cbDataBit.Text = comTemp.Databit.ToString();
                        protocolConfiguration.cbStopBit.Text = comTemp.StopBits.ToString();
                        protocolConfiguration.cbProtocol.Text = comTemp.Protocol.ToString();
                    }
                    else
                    {
                        protocolConfiguration.txtIPAdress.Text = deviceTemp.IP;
                        protocolConfiguration.txtPort.Text = deviceTemp.Port.ToString();
                        protocolConfiguration.cbProtocol.Text = deviceTemp.Protocol.ToString();

                    }
                    splitContainer.Panel2.Controls.Add(protocolConfiguration);
                    formProtocolConfiguration = protocolConfiguration;//lưu vào biến toàn cục
                    isInFormEdit = true;
                    formProtocolConfiguration.isTabConfigHaveAnyChanged = false;
                    formProtocolConfiguration.isTabDataHaveAnyChanged = false;
                }
            }
        }*/

        /*private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    if (tvMain.SelectedNode != null)
                    {
                        if (tvMain.SelectedNode.Parent == null)
                        {
                            //tvMain.Nodes.Remove(tvMain.SelectedNode);
                            return;
                        }
                        else
                        {
                            deviceDic.Remove(tvMain.SelectedNode.Text);
                            WriteListObjectToJson();
                            tvMain.SelectedNode.Parent.Nodes.Remove(tvMain.SelectedNode);
                        }
                    }
                    splitContainer.Panel2.Controls.Clear();
                    break;
                default:
                    break;
            }

        }*/

        /*private void tvMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                tvMain.SelectedNode = e.Node;
            }

            if (e.Node.Level == 2)
            {
                e.Node.ContextMenuStrip = xoa_Menu;
            }
        }*/

        private void cmsAddThietBi_Click(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            splitContainer.Panel2.Controls.Clear();

            ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);
            protocolConfiguration.Dock = DockStyle.Fill;
            protocolConfiguration.btnEditProtocol.Visible = false;
            protocolConfiguration.btnSaveProtocol.Visible = true;
            splitContainer.Panel2.Controls.Add(protocolConfiguration);
            protocolConfiguration.cbCOM.DataSource = ports;
            protocolConfiguration.dgvDataProtocol.DataSource = null;

            formProtocolConfiguration = protocolConfiguration;
            isInFormEdit = false;
        }
        #endregion

        private void FormDataList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formProtocolConfiguration == null)
            {
                e.Cancel = false;
            }
            else if (formProtocolConfiguration.IsFormHaveAnyChanged())
            {
                DialogResult dr = MessageBox.Show("Bạn muốn lưu những thay đổi trước khi đóng form không ?", "Chú ý", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (dr == DialogResult.Yes)
                {
                    formProtocolConfiguration.DongForm(isInFormEdit);
                    if (!formProtocolConfiguration.isValidatePassed)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
    }
}

