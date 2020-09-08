using EasyModbus;
using EasyModbus.Exceptions;
using FileExportScheduler.Models.DuLieu;
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
        public static string LayDuLieuTCPIP(ModbusClient modbus, DuLieuGiamSatModel duLieuTemp)
         {
            string giaTriDuLieu = "";
            try
            {
               
                if (Convert.ToInt32(duLieuTemp.DiaChi) <= 9999)
                {
                    bool[] readCoil = modbus.ReadCoils(Convert.ToInt32(duLieuTemp.DiaChi), 1);
                    giaTriDuLieu = readCoil[0].ToString();
                }
                else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 19999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 10000)
                {
                    bool[] discreteInput = modbus.ReadDiscreteInputs(Convert.ToInt32(duLieuTemp.DiaChi) - 10000, 1);
                    giaTriDuLieu = discreteInput[0].ToString();
                }
                else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 39999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 30000)
                {
                    int[] readRegister = modbus.ReadInputRegisters(Convert.ToInt32(duLieuTemp.DiaChi) - 30000, 1);
                    giaTriDuLieu = readRegister[0].ToString();
                }
                else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 49999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 40000)
                {
                    int[] readHoldingRegister = modbus.ReadHoldingRegisters(Convert.ToInt32(duLieuTemp.DiaChi) - 40000, 1);
                    giaTriDuLieu = readHoldingRegister[0].ToString();
                }
            }
            catch
            {
                throw;
            }
            return giaTriDuLieu;
        }
        public static string LayDuLieuCOM(DuLieuGiamSatModel duLieuTemp, SerialPort serialPort)
        {
            string giaTriDuLieu = ""; 
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            try
            {
                var b = serialPort.Parity.ToString();
                var a  = serialPort.ParityReplace.ToString();
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
                    giaTriDuLieu = (Convert.ToInt32(readRegister[0].ToString()) - ((Convert.ToInt32(readRegister[0].ToString()) > 32767) ? 65536 : 0)).ToString();
                }
                else if (Convert.ToInt32(duLieuTemp.DiaChi) <= 49999 && Convert.ToInt32(duLieuTemp.DiaChi) >= 40000)
                {
                    ushort[] readHoldingRegisters = master.ReadHoldingRegisters(slaveAddress, Convert.ToUInt16(Convert.ToInt32(duLieuTemp.DiaChi) - 40000), numberOfPoint);
                    giaTriDuLieu = (Convert.ToInt32(readHoldingRegisters[0].ToString()) - ((Convert.ToInt32(readHoldingRegisters[0].ToString()) > 32767) ? 65536 : 0)).ToString();
                   
                }

            }
            catch
            {
                throw;
            }
            return giaTriDuLieu;
        }
    }
}
