using ESProtocolConverter.Models.Common;
using ESProtocolConverter.Models.NhaMay;
using ESProtocolConverter.Service.Json;
using FileExportScheduler.Models;
using FileExportScheduler.Models.ThietBi.Base;
using FileExportScheduler.Service.Json;
using FileExportScheduler.Service.ThietBi;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
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
        //use for ProtocolConfiguration form
        public TreeNode selectedNodeDouble;

        public TreeNode rightClickNode;
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

            if (node.Name == TreeName.Name.NhaMay.ToString())
            {
                node.ContextMenuStrip = cms_NhaMay;
            }
            else if (node.Name == TreeName.Name.ThietBi.ToString())
            {
                node.ContextMenuStrip = cms_ThietBi;
            }
            else if (node.Name == TreeName.Name.SlaveAddress.ToString())
            {
                node.ContextMenuStrip = cms_Slave;
            }

        }

        //đọc file json ra list
        private void JsonToList()
        {
            try
            {
                dicNhaMay.Clear();

                dicNhaMay = JsonService.GetDicNhaMay();

            }
            catch (Exception ex)
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

            tvMain.Nodes.Clear();

            foreach (KeyValuePair<string, NhaMayModel> nhaMay in dicNhaMay)
            {
                TreeNode node_nhaMay = new TreeNode(nhaMay.Value.Name);

                node_nhaMay.Name = TreeName.Name.NhaMay.ToString();
                node_nhaMay.Text = "Cấu hình";

                node_nhaMay.ImageKey = "network.ico";
                node_nhaMay.SelectedImageKey = "network.ico";

                tvMain.Nodes.Add(node_nhaMay);

                foreach (var thietBi in nhaMay.Value.dsThietBi)
                {
                    TreeNode node_thietBi = new TreeNode(thietBi.Value.Name);

                    node_thietBi.Name = TreeName.Name.ThietBi.ToString(); ;

                    node_nhaMay.Nodes.Add(node_thietBi);

                    foreach (var slave in thietBi.Value.dsSlave)
                    {
                        TreeNode node_slave = new TreeNode(slave.Value.Name);

                        node_slave.Name = TreeName.Name.SlaveAddress.ToString(); ;


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

                setMenu(node_nhaMay);

            }

            WindowState = FormWindowState.Maximized;
        }



        private void FormDataList_Load(object sender, EventArgs e)
        {
            LoadTreeView();
        }

        private void tvMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                rightClickNode = e.Node;
            }

        }

        private void cmsAddThietBi_Click(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            splitContainer.Panel2.Controls.Clear();

            ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);
            protocolConfiguration.Dock = DockStyle.Fill;
            protocolConfiguration.btnEditProtocol.Visible = false;
            protocolConfiguration.btnAddNewProtocol.Visible = true;
            protocolConfiguration.cbCOM.DataSource = ports;
            //protocolConfiguration.dgvDataProtocol.DataSource = null;

            protocolConfiguration.SetThietBiAndSlave(null, null);
            protocolConfiguration.SetDsThietBi(ThietBiGiamSatService.GetDsThietBi("Quang Ninh"));

            protocolConfiguration.HideTabDuLieu();
            protocolConfiguration.HideTabSlave();
            
            splitContainer.Panel2.Controls.Add(protocolConfiguration);


            formProtocolConfiguration = protocolConfiguration;//lưu vào biến toàn cục
            isInFormEdit = true;
            formProtocolConfiguration.isTabConfigHaveAnyChanged = false;
            formProtocolConfiguration.isTabDataHaveAnyChanged = false;

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

        private void tvMain_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.Expand();

            TreeNode node = tvMain.SelectedNode;

            //use for in ProtocolConfiguration
            selectedNodeDouble = tvMain.SelectedNode;

            var ports = SerialPort.GetPortNames();

            if (node != null && (node.Name == TreeName.Name.SlaveAddress.ToString() || node.Name == TreeName.Name.ThietBi.ToString()))
            {
                splitContainer.Panel2.Controls.Clear();

                ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);
                protocolConfiguration.Dock = DockStyle.Fill;

                protocolConfiguration.cbCOM.DataSource = ports;
                protocolConfiguration.btnEditProtocol.Visible = true;
                protocolConfiguration.btnAddNewProtocol.Visible = false;

                string thietBi_name = node.Name == TreeName.Name.ThietBi.ToString() ? node.Text : node.Parent.Text;
                string slave_name = node.Name == TreeName.Name.SlaveAddress.ToString() ? node.Text : null;

                protocolConfiguration.txtTenGiaoThuc.Text = thietBi_name;

                ThietBiModel thietBi_model = ThietBiGiamSatService.GetThietBiGiamSat("Quang Ninh", thietBi_name);

                protocolConfiguration.SetThietBiAndSlave(thietBi_model, slave_name);
                protocolConfiguration.SetDsThietBi(ThietBiGiamSatService.GetDsThietBi("Quang Ninh"));

                if (node.Name == TreeName.Name.SlaveAddress.ToString())
                {
                    protocolConfiguration.LoadDuLieuLenDgv();
                }


                if (node.Name == TreeName.Name.ThietBi.ToString())
                {
                    protocolConfiguration.HideTabDuLieu();
                    protocolConfiguration.HideTabSlave();
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

        private void cms_Xoa_ThietBi(object sender, EventArgs e)
        {
            string oldName_ThietBi = rightClickNode.Text;
            JsonService.RemoveThietBiInNhaMay("Quang Ninh", oldName_ThietBi);

            rightClickNode.Remove();

            if(formProtocolConfiguration.GetCurrentThietBi() != null && formProtocolConfiguration.GetCurrentThietBi().Name == oldName_ThietBi)
            {
                splitContainer.Panel2.Controls.Clear();
                formProtocolConfiguration = null;           
            }

        }

        private void cms_Them_SlaveAddress(object sender, EventArgs e)
        {
            TreeNode node = rightClickNode;

            splitContainer.Panel2.Controls.Clear();

            ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(this);
            protocolConfiguration.Dock = DockStyle.Fill;
            protocolConfiguration.dgvDataProtocol.DataSource = null;
            protocolConfiguration.HideTabCauHinh();

            // get Thiet Bi
            string thietBi_name = node.Name == TreeName.Name.ThietBi.ToString() ? node.Text : "";

            if (!String.IsNullOrEmpty(thietBi_name))
            {
                ThietBiModel thietBi_model = ThietBiGiamSatService.GetThietBiGiamSat("Quang Ninh", thietBi_name);
                protocolConfiguration.SetThietBiAndSlave(thietBi_model, null);
            }
                   
            protocolConfiguration.SetDsThietBi(ThietBiGiamSatService.GetDsThietBi("Quang Ninh"));

            //
            splitContainer.Panel2.Controls.Add(protocolConfiguration);


            formProtocolConfiguration = protocolConfiguration;//lưu vào biến toàn cục
            isInFormEdit = true;
            formProtocolConfiguration.isTabConfigHaveAnyChanged = false;
            formProtocolConfiguration.isTabDataHaveAnyChanged = false;

            formProtocolConfiguration = protocolConfiguration;
            isInFormEdit = false;
        }

        private void cms_Xoa_SlaveAddress(object sender, EventArgs e)
        {

        }

    }
}

