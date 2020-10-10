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
                if (quantityCoils != 0)
                {
                    bool[] readCoil = modbus.ReadCoils(0, (ushort)(quantityCoils));
                    listMangGiaTriDuLieu.Add(readCoil);
                }
                if (quantityInputs != 0)
                {
                    bool[] discreteInput = modbus.ReadDiscreteInputs(0, (ushort)(quantityInputs));
                    listMangGiaTriDuLieu.Add(discreteInput);
                }
                if (quantityInputRegisters != 0)
                {
                    int[] readRegister = modbus.ReadInputRegisters(0, (ushort)(quantityInputRegisters));
                    listMangGiaTriDuLieu.Add(readRegister);
                }
                if (quantityHoldingRegisters != 0)
                {
                    int[] readHoldingRegister = modbus.ReadHoldingRegisters(0, (ushort)(quantityHoldingRegisters));
                    listMangGiaTriDuLieu.Add(readHoldingRegister);
                }
            }
            catch (Exception ex)
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

                if (quantityCoils != 0)
                {
                    bool[] readCoil = master.ReadCoils(slaveAddress, 0, (ushort)(quantityCoils + 1));
                    listMangGiaTriDuLieu.Add(readCoil);
                }
                if (quantityInputs != 0)
                {
                    bool[] discreteInput = master.ReadInputs(slaveAddress, 0, (ushort)(quantityInputs + 1));
                    listMangGiaTriDuLieu.Add(discreteInput);
                }
                if (quantityInputRegisters != 0)
                {
                    ushort[] readRegister = master.ReadInputRegisters(slaveAddress, 0, (ushort)(quantityInputRegisters + 1));
                    listMangGiaTriDuLieu.Add(readRegister);
                }
                if (quantityHoldingRegisters != 0)
                {
                    ushort[] readHoldingRegisters = master.ReadHoldingRegisters(slaveAddress, 0, (ushort)(quantityHoldingRegisters + 1));
                    listMangGiaTriDuLieu.Add(readHoldingRegisters);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listMangGiaTriDuLieu;
        }
    }
}
