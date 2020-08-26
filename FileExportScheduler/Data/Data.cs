using EasyModbus;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExportScheduler.Data
{
    public static class Data
    {
        public static string LayDuLieuTCPIP(ModbusClient mobus, Models.DuLieuGiamSat duLieuTemp)
        {
            string giaTriDuLieu = "";
            try
            {

                if (Convert.ToInt32(duLieuTemp.DiaChi) <= 9999)
                {
                    bool[] readCoil = mobus.ReadCoils(Convert.ToInt32(duLieuTemp.DiaChi), 1);
                    giaTriDuLieu = readCoil[0].ToString();
                }
                else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 19999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 10000)
                {
                    bool[] discreteInput = mobus.ReadDiscreteInputs(Convert.ToInt32(duLieuTemp.DiaChi) - 10000, 1);
                    giaTriDuLieu = discreteInput[0].ToString();
                }
                else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 39999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 30000)
                {
                    int[] readRegister = mobus.ReadInputRegisters(Convert.ToInt32(duLieuTemp.DiaChi) - 30000, 1);
                    giaTriDuLieu = readRegister[0].ToString();
                }
                else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 49999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 40000)
                {
                    int[] readHoldingRegister = mobus.ReadHoldingRegisters(Convert.ToInt32(duLieuTemp.DiaChi) - 40000, 1);
                    giaTriDuLieu = readHoldingRegister[0].ToString();
                }
            }
            catch
            {
                throw;
            }
            return giaTriDuLieu;
        }
        public static string LayDuLieuCOM(Models.DuLieuGiamSat duLieuTemp, SerialPort serialPort)
        {
            string giaTriDuLieu = ""; 
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            try
            {

                byte slaveAddress = 1;
                ushort numberOfPoint = 1;

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
                    ushort[] readHoldingRegisters = master.ReadHoldingRegisters(slaveAddress, Convert.ToUInt16(Convert.ToInt32(duLieuTemp.DiaChi) - 40000), numberOfPoint);
                    giaTriDuLieu = readHoldingRegisters[0].ToString();
                }

            }
             catch (Exception ex)
            {
                
                throw;
            }
            return giaTriDuLieu;
        }
    }
}
