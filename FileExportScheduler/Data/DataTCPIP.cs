using EasyModbus;
using EasyModbus.Exceptions;
using FileExportScheduler.Constant;
using FileExportScheduler.Models.ThietBi.Base;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Data
{
    public static class DataTCPIP
    {
        private static readonly int DonViQuantityMoiLanDoc = 100;
        public static bool[] LayDuLieuTCPCoils(ModbusClient modbus, ushort quantityCoils, ushort minAddressCoils, ThietBiModel thietBiModel)
        {
            List<bool> readCoil = new List<bool>();

            if (quantityCoils != 0)
            {
                try
                {
                    int soNguyenSauChia = quantityCoils / DonViQuantityMoiLanDoc;
                    for (int i = 0; i <= soNguyenSauChia; i++)
                    {

                        if (i != soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressCoils;
                            int quantity = DonViQuantityMoiLanDoc - minAddressCoils;
                            var temp = modbus.ReadCoils(startAddress, (ushort)(quantity));
                            readCoil.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressCoils;
                            int quantity = quantityCoils % DonViQuantityMoiLanDoc - minAddressCoils;
                            if(quantity != 0)
                            {
                                var temp = modbus.ReadCoils(startAddress, (ushort)(quantity));
                                readCoil.AddRange(temp.ToList());
                            }
                        }

                    }
                }
                catch (ModbusException ex)
                {
                    ExceptionFunctionCode(ex, thietBiModel);
                    throw;

                }
                catch (Exception ex)//Lỗi lấy dữ liệu thất bại
                {
                    ExceptionErrorConnection(ex, thietBiModel);
                    throw;
                }
            }
            return readCoil.ToArray();
        }

        public static bool[] LayDuLieuTCPInputs(ModbusClient modbus, ushort quantityInputs, ushort minAddressInputs, ThietBiModel thietBiModel)
        {
            List<bool> readDiscreteInputs = new List<bool>();

            if (quantityInputs != 0)
            {
                try
                {
                    int soNguyenSauChia = quantityInputs / DonViQuantityMoiLanDoc;
                    for (int i = 0; i <= soNguyenSauChia; i++)
                    {

                        if (i != soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressInputs;
                            int quantity = DonViQuantityMoiLanDoc - minAddressInputs;
                            var temp = modbus.ReadDiscreteInputs(startAddress, (ushort)(quantity));
                            readDiscreteInputs.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressInputs;
                            int quantity = quantityInputs % DonViQuantityMoiLanDoc - minAddressInputs;
                            if(quantity != 0)
                            {
                                var temp = modbus.ReadDiscreteInputs(startAddress, (ushort)(quantity));
                                readDiscreteInputs.AddRange(temp.ToList());
                            }
                            
                        }

                    }
                }
                catch (ModbusException ex)
                {
                    ExceptionFunctionCode(ex, thietBiModel);
                    throw;

                }
                catch (Exception ex)//Lỗi lấy dữ liệu thất bại
                {
                    ExceptionErrorConnection(ex, thietBiModel);
                    throw;
                }
            }
            return readDiscreteInputs.ToArray();
        }

        public static int[] LayDuLieuTCPInputRegister(ModbusClient modbus, ushort quantityInputRegisters, ushort minAddressInputRegister, ThietBiModel thietBiModel)
        {
            List<int> readInputRegisters = new List<int>();

            if (quantityInputRegisters != 0)
            {
                try
                {
                    int soNguyenSauChia = quantityInputRegisters / DonViQuantityMoiLanDoc;
                    for (int i = 0; i <= soNguyenSauChia; i++)
                    {

                        if (i != soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressInputRegister;
                            int quantity = DonViQuantityMoiLanDoc - minAddressInputRegister;
                            var temp = modbus.ReadInputRegisters(startAddress, (ushort)(quantity));
                            readInputRegisters.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressInputRegister;
                            int quantity = quantityInputRegisters % DonViQuantityMoiLanDoc - minAddressInputRegister;
                            if(quantity != 0)
                            {
                                var temp = modbus.ReadInputRegisters(startAddress, (ushort)(quantity));
                                readInputRegisters.AddRange(temp.ToList());
                            }
                            
                        }

                    }
                }
                catch (ModbusException ex)
                {
                    ExceptionFunctionCode(ex, thietBiModel);
                    throw;
                }
                catch (Exception ex)//Lỗi lấy dữ liệu thất bại
                {
                    ExceptionErrorConnection(ex, thietBiModel);
                    throw;
                }
            }
            return readInputRegisters.ToArray();
        }

        public static int[] LayDuLieuTCPHoldingRegister(ModbusClient modbus, ushort quantityHoldingRegisters, ushort minAddressHoldingRegister, ThietBiModel thietBiModel)
        {
            List<int> readHoldingRegister=new List<int>();

            if (quantityHoldingRegisters != 0)
            {
                try
                {
                    int soNguyenSauChia = quantityHoldingRegisters / DonViQuantityMoiLanDoc;
                    for (int i = 0; i <= soNguyenSauChia; i++)
                    {
                        
                        if (i != soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressHoldingRegister;
                            int quantity = DonViQuantityMoiLanDoc - minAddressHoldingRegister;
                            var temp= modbus.ReadHoldingRegisters(startAddress, (ushort)(quantity));
                            readHoldingRegister.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc + minAddressHoldingRegister;
                            int quantity = quantityHoldingRegisters % DonViQuantityMoiLanDoc - minAddressHoldingRegister;
                            if(quantity != 0)
                            {
                                var temp = modbus.ReadHoldingRegisters(startAddress, (ushort)(quantity));
                                readHoldingRegister.AddRange(temp.ToList());
                            }
                            
                        }

                    }
                }
                catch (ModbusException ex)
                {
                    ExceptionFunctionCode(ex, thietBiModel);
                    throw;

                }
                catch (Exception ex)//Lỗi lấy dữ liệu thất bại
                {
                    ExceptionErrorConnection(ex, thietBiModel);
                    throw;
                }
            }
            return readHoldingRegister.ToArray();
        }

        private static void ExceptionFunctionCode(Exception exceptionMessage, ThietBiModel thietBiModel)
        {
            if (exceptionMessage.Message == "Function code not supported by master" && !ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.DiaChiKhongTonTai))
            {
                ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.DiaChiKhongTonTai);
            }
            else if (exceptionMessage.Message == "Starting address invalid or starting address + quantity invalid" && !ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.VuotQuaDuLieu))
            {
                ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.DiaChiKhongTonTai);
            }

            thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
        }

        private static void ExceptionErrorConnection(Exception exceptionMessage, ThietBiModel thietBiModel)
        {
            if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.KhongCoTinHieuTraVe))
            {
                ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.KhongCoTinHieuTraVe);
            }
            thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
        }
    }
}
