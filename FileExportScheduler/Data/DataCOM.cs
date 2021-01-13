using EasyModbus;
using EasyModbus.Exceptions;
using FileExportScheduler.Constant;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
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
    public static class DataCOM
    {
        private static readonly int DonViQuantityMoiLanDoc = 125;
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
        public static bool[] LayDuLieuCOMCoils(SerialPort serialPort, ushort quantityCoils, ushort minAddressCoils,ThietBiModel thietBiModel)
        {
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            List<bool> readCoil = new List<bool>();
            if (quantityCoils != 0)
            {
                try
                {
                    byte slaveAddress = 1;
                    int soNguyenSauChia = quantityCoils / DonViQuantityMoiLanDoc;
                    for (int i = 0; i <= soNguyenSauChia; i++)
                    {

                        if (i != soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressCoils;
                            int quantity = DonViQuantityMoiLanDoc - minAddressCoils;
                            var temp = master.ReadCoils(slaveAddress, (ushort)startAddress, (ushort)(quantity));
                            readCoil.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressCoils;
                            int quantity = quantityCoils % DonViQuantityMoiLanDoc - minAddressCoils;
                            if(quantity != 0)
                            {
                                var temp = master.ReadCoils(slaveAddress, (ushort)startAddress, (ushort)(quantity));
                                readCoil.AddRange(temp.ToList());
                            }
                            
                        }

                    }
                }
                catch (TimeoutException ex)
                {
                    ExceptionTimeOut(ex, thietBiModel);
                    throw;
                }
                catch (Modbus.SlaveException ex)
                {
                    ExceptionErrorSlave(ex, thietBiModel);
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return readCoil.ToArray();
        }
        public static bool[] LayDuLieuCOMInputs(SerialPort serialPort, ushort quantityInputs, ushort minAddressInputs, ThietBiModel thietBiModel)
        {
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            List<bool> discreteInput = new List<bool>();
            if (quantityInputs != 0)
            {
                try
                {
                    byte slaveAddress = 1;
                    int soNguyenSauChia = quantityInputs / DonViQuantityMoiLanDoc;
                    for (int i = 0; i <= soNguyenSauChia; i++)
                    {

                        if (i != soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressInputs;
                            int quantity = DonViQuantityMoiLanDoc - minAddressInputs;
                            var temp = master.ReadInputs(slaveAddress, (ushort)startAddress, (ushort)(quantity));
                            discreteInput.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressInputs;
                            int quantity = quantityInputs % DonViQuantityMoiLanDoc - minAddressInputs;
                            if(quantity != 0)
                            {
                                var temp = master.ReadInputs(slaveAddress, (ushort)startAddress, (ushort)(quantity));
                                discreteInput.AddRange(temp.ToList());
                            }
                            
                        }

                    }
                }
                catch (TimeoutException ex)
                {
                    ExceptionTimeOut(ex, thietBiModel);
                    throw;
                }
                catch (Modbus.SlaveException ex)
                {
                    ExceptionErrorSlave(ex, thietBiModel);
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return discreteInput.ToArray();
        }
        public static int[] LayDuLieuCOMInputRegisters(SerialPort serialPort, ushort quantityInputRegisters, ushort minAddressInputRegisters, ThietBiModel thietBiModel)
        {
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            List<ushort> readRegister = new List<ushort>();
            if (quantityInputRegisters != 0)
            {
                try
                {
                    byte slaveAddress = 1;
                    int soNguyenSauChia = quantityInputRegisters / DonViQuantityMoiLanDoc;
                    for (int i = 0; i <= soNguyenSauChia; i++)
                    {

                        if (i != soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressInputRegisters;
                            int quantity = DonViQuantityMoiLanDoc - minAddressInputRegisters;
                            var temp = master.ReadInputRegisters(slaveAddress, (ushort)startAddress, (ushort)(quantity));
                            readRegister.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressInputRegisters;
                            int quantity = quantityInputRegisters % DonViQuantityMoiLanDoc - minAddressInputRegisters;
                            if(quantity != 0)
                            {
                                var temp = master.ReadInputRegisters(slaveAddress, (ushort)startAddress, (ushort)(quantity));
                                readRegister.AddRange(temp.ToList());
                            }
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                    ExceptionTimeOut(ex, thietBiModel);
                    throw;
                    //lỗi không đọc được dữ liệu
                }
                catch (Modbus.SlaveException ex)
                {
                    ExceptionErrorSlave(ex, thietBiModel);
                    throw;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            return ConvertArrayUshortToIntArray(readRegister);
        }
        public static int[] LayDuLieuCOMHoldingRegisters(SerialPort serialPort, ushort quantityHoldingRegisters, ushort minAddressHoldingRegisters, ThietBiModel thietBiModel)
        {
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            List<ushort> readHoldingRegistersUshortTpye = new List<ushort>();
            if (quantityHoldingRegisters != 0)
            {
                try
                {
                    byte slaveAddress = 1;
                    int soNguyenSauChia = quantityHoldingRegisters / DonViQuantityMoiLanDoc;
                    for (int i = 0; i <= soNguyenSauChia; i++)
                    {

                        if (i != soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressHoldingRegisters;
                            int quantity = DonViQuantityMoiLanDoc - minAddressHoldingRegisters;
                            var temp = master.ReadHoldingRegisters(slaveAddress, (ushort)startAddress, (ushort)(quantity));

                            readHoldingRegistersUshortTpye.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressHoldingRegisters;
                            int quantity = quantityHoldingRegisters % DonViQuantityMoiLanDoc - minAddressHoldingRegisters;
                            if (quantity != 0)
                            {
                                var temp = master.ReadHoldingRegisters(slaveAddress, (ushort)startAddress, (ushort)(quantity));
                                readHoldingRegistersUshortTpye.AddRange(temp.ToList());
                            }
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                    ExceptionTimeOut(ex, thietBiModel);
                    throw;
                    //lỗi không đọc được dữ liệu
                }
                catch (Modbus.SlaveException ex)
                {
                    ExceptionErrorSlave(ex, thietBiModel);
                    throw;
                    //lỗi số bản ghi cần đọc vượt quá lượng bản ghi trả về
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return ConvertArrayUshortToIntArray(readHoldingRegistersUshortTpye);
        }

        private static int[] ConvertArrayUshortToIntArray(List<ushort> UshortList )
        {
            List<int> readHoldingRegistersIntTpye = new List<int>();
            foreach (var ushortValue in UshortList)
            {
               int intValue = ushortValue - ((ushortValue > 32767) ? 65536 : 0);
                readHoldingRegistersIntTpye.Add(intValue);
            }
            return readHoldingRegistersIntTpye.ToArray();
        }
        private static void ExceptionTimeOut(Exception exceptionMessage, ThietBiModel thietBiModel)
        {
            if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.KhongKetNoi))
            {
                ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.KhongKetNoi);
            }
            thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
        }

        private static void ExceptionErrorSlave(Exception exceptionMessage, ThietBiModel thietBiModel)
        {
            if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.VuotQuaDuLieu))
            {
                ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.VuotQuaDuLieu);
            }
            thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
        }
    }
}
