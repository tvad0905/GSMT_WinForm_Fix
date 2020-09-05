using FileExportScheduler.Controller;
using FileExportScheduler.Models;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.Json;
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
        Dictionary<string, ThietBiGiamSat> deviceDic = new Dictionary<string, ThietBiGiamSat>();
        public TreeNode selectedNode = new TreeNode();
        public TreeNode selectedNodeDouble = new TreeNode();
        #endregion
        public FormDataList()
        {
            InitializeComponent();
        }

        //set context menu strip cho các node
        private void setMenu(TreeNode node)
        {

            if (node.Name.ToLower() == "devices" || node.Name.ToLower() == "protocols")
            {
                node.ContextMenuStrip = ctxMenu;
            }
            else
            {
                node.ContextMenuStrip = tx2;
            }
        }

        //đọc file json ra list
        private void JsonToList()
        {
            try
            {
                var path = GetPathJson.getPathConfig("DeviceAndData.json");
                deviceDic.Clear();
                JObject jsonObj = JObject.Parse(File.ReadAllText(path));
                Dictionary<string, ThietBiTCPIP> deviceIP = jsonObj.ToObject<Dictionary<string, ThietBiTCPIP>>();
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
                }
            }
            catch
            {
            }
        }

        //viết vào file json
        private void WriteListObjectToJson()
        {
            var path = GetPathJson.getPathConfig("DeviceAndData.json");
            string jsonString = (new JavaScriptSerializer()).Serialize((object)deviceDic);
            File.WriteAllText(path, jsonString);

        }

        #region sự kiện của form
        public void LoadTreeView()
        {
            JsonToList();

            foreach (KeyValuePair<string, ThietBiGiamSat> device in deviceDic)
            {
                    TreeNode node = new TreeNode(device.Value.Name);
                    tvMain.Nodes["Protocols"].Nodes.Add(node);
            }
            foreach (TreeNode node in tvMain.Nodes)
            {
                foreach (TreeNode _node in node.Nodes)
                {
                    setMenu(_node);
                }
                setMenu(node);
            }
            WindowState = FormWindowState.Maximized;
        }

        private void FormDataList_Load(object sender, EventArgs e)
        {
            LoadTreeView();
            ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);

            var ports = SerialPort.GetPortNames();
            protocolConfiguration.cbCOM.DataSource = ports;
        }

        private void tvMain_DoubleClick(object sender, EventArgs e)
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

                    //
                    ThietBiTCPIP deviceTemp = deviceDic[protocolConfiguration.txtTenGiaoThuc.Text] as ThietBiTCPIP;
                    if (deviceTemp == null)
                    {
                        ThietBiCOM comTemp = deviceDic[protocolConfiguration.txtTenGiaoThuc.Text] as ThietBiCOM;
                        protocolConfiguration.cbCOM.Text = comTemp.Com;
                        protocolConfiguration.cbBaud.Text = comTemp.Baud.ToString();
                        protocolConfiguration.cbParity.Text = comTemp.parity.ToString();
                        protocolConfiguration.cbDataBit.Text = comTemp.Databit.ToString();
                        protocolConfiguration.cbStopBit.Text = comTemp.stopBits.ToString();
                        protocolConfiguration.cbProtocol.Text = comTemp.Protocol.ToString();
                    }
                    else
                    {
                        protocolConfiguration.txtIPAdress.Text = deviceTemp.IP;
                        protocolConfiguration.txtPort.Text = deviceTemp.Port.ToString();
                        protocolConfiguration.cbProtocol.Text = deviceTemp.Protocol.ToString();

                    }
                    splitContainer.Panel2.Controls.Add(protocolConfiguration);
                }
            }
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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

        }

        private void tvMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                tvMain.SelectedNode = e.Node;
            }

            if (e.Node.Level == 2)
            {
                e.Node.ContextMenuStrip = tx2;
            }
        }

        private void mnuAdd_Click(object sender, EventArgs e)
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
        }
        #endregion

        private void FormDataList_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}

