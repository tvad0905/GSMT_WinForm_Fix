using EasyModbus;
using EasyModbus.Exceptions;
using FileExportScheduler.Constant;
using FileExportScheduler.Models.DuLieu;
using Modbus.Device;
using System;
using System.Collections;
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
        public static ArrayList LayDuLieuTCPIP(ModbusClient modbus, ushort quantityCoils, ushort quantityInputs, ushort quantityInputRegisters, ushort quantityHoldingRegisters)
        {
            var listMangGiaTriDuLieu = new ArrayList();
            try
            {

                bool[] readCoil = modbus.ReadCoils(0, quantityCoils);
                listMangGiaTriDuLieu.Add(readCoil);

                bool[] discreteInput = modbus.ReadDiscreteInputs(0, quantityInputs);
                listMangGiaTriDuLieu.Add(discreteInput);

                int[] readRegister = modbus.ReadInputRegisters(0, quantityInputRegisters);
                listMangGiaTriDuLieu.Add(readRegister);

                int[] readHoldingRegister = modbus.ReadHoldingRegisters(0, quantityHoldingRegisters);
                listMangGiaTriDuLieu.Add(readHoldingRegister);

            }
            catch(Exception ex)
            {
                throw;
            }
            return listMangGiaTriDuLieu;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duLieuTemp"></param>
        /// <param name="serialPort"></param>
        /// <param name="quantityCoils"></param>
        /// <param name="quantityInputs"></param>
        /// <param name="quantityInputRegisters"></param>
        /// <param name="quantityHoldingRegisters"></param>
        /// <returns></returns>
        public static ArrayList LayDuLieuCOM(SerialPort serialPort, ushort quantityCoils, ushort quantityInputs, ushort quantityInputRegisters, ushort quantityHoldingRegisters)
        {
            var listMangGiaTriDuLieu = new ArrayList();
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            try
            {
                byte slaveAddress = 1;

                bool[] readCoil = master.ReadCoils(slaveAddress, 0, quantityCoils);
                listMangGiaTriDuLieu.Add(readCoil);

                bool[] discreteInput = master.ReadInputs(slaveAddress, 0, quantityInputs);
                listMangGiaTriDuLieu.Add(discreteInput);

                ushort[] readRegister = master.ReadInputRegisters(slaveAddress, 0, quantityInputRegisters);
                listMangGiaTriDuLieu.Add(readRegister);

                ushort[] readHoldingRegisters = master.ReadHoldingRegisters(slaveAddress, 0, quantityHoldingRegisters);
                listMangGiaTriDuLieu.Add(readHoldingRegisters);

            }
            catch (Exception ex)
            {

                throw;
            }
            return listMangGiaTriDuLieu;
        }
    }
}
