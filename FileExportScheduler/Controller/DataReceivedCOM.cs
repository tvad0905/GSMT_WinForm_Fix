using FileExportScheduler.Models;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    class DataReceivedCOM
    {
        private void getDataCOM(KeyValuePair<string, DeviceModel> deviceUnit, SerialPort serialPort)
        {
            for (int i = 0; i < deviceUnit.Value.ListDuLieuChoTungPLC.Count; i++)
            {
                if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    var a = serialPort.IsOpen;

                    DataModel duLieuTemp = deviceUnit.Value.ListDuLieuChoTungPLC.ElementAt(i).Value;

                    string giaTriDuLieu = "";
                    try
                    {
                        byte slaveAddress = 1;
                        ushort numberOfPoint = 1;

                        IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
                        if (Convert.ToInt32(duLieuTemp.DiaChi) <= 9999)
                        {
                            bool[] readCoil = master.ReadCoils(slaveAddress, Convert.ToUInt16(duLieuTemp.DiaChi), numberOfPoint);
                            giaTriDuLieu = readCoil[0].ToString();
                        }
                        else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 19999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 10000)
                        {
                            bool[] discreteInput = master.ReadInputs(slaveAddress, Convert.ToUInt16(Convert.ToInt32(duLieuTemp.DiaChi) - 10000), numberOfPoint);
                            giaTriDuLieu = discreteInput[0].ToString();
                        }
                        else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 39999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 30000)
                        {
                            ushort[] readRegister = master.ReadInputRegisters(slaveAddress, Convert.ToUInt16(Convert.ToInt32(duLieuTemp.DiaChi) - 30000), numberOfPoint);
                            giaTriDuLieu = readRegister[0].ToString();
                        }
                        else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 49999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 40000)
                        {
                            ushort[] result = master.ReadHoldingRegisters(slaveAddress, Convert.ToUInt16(Convert.ToInt32(duLieuTemp.DiaChi) - 40000), numberOfPoint);
                            giaTriDuLieu = result[0].ToString();
                        }
                        try
                        {
                            duLieuTemp.GiaTri = ushort.Parse(giaTriDuLieu) + "";
                        }
                        catch (Exception)
                        {
                            duLieuTemp.GiaTri = giaTriDuLieu;
                        }
                        deviceUnit.Value.TrangThaiKetNoi = "Good";

                    }
                    catch (Exception ex)
                    {
                        deviceUnit.Value.TrangThaiKetNoi = "Bad";
                    }
                    finally
                    {
                        duLieuTemp.ThoiGianDocGiuLieu = DateTime.Now;
                    }
                }

            }
            //serialPort.Close();
        }
    }
}
