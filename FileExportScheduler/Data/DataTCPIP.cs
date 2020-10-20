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
        public static bool[] LayDuLieuTCPCoils(ModbusClient modbus, ushort quantityCoils, ThietBiModel thietBiModel)
        {
            bool[] readCoil = new bool[quantityCoils];
            if (quantityCoils != 0)
            {
                try
                {
                    readCoil = modbus.ReadCoils(0, (ushort)(quantityCoils));
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
            return readCoil;
        }

        public static bool[] LayDuLieuTCPInputs(ModbusClient modbus, ushort quantityInputs, ThietBiModel thietBiModel)
        {
            bool[] readDiscreteInputs = new bool[quantityInputs];
            if (quantityInputs != 0)
            {
                try
                {
                    readDiscreteInputs = modbus.ReadDiscreteInputs(0, (ushort)(quantityInputs));
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
            return readDiscreteInputs;
        }

        public static int[] LayDuLieuTCPInputRegister(ModbusClient modbus, ushort quantityInputRegisters, ThietBiModel thietBiModel)
        {
            int[] readInputRegisters = new int[quantityInputRegisters];
            if (quantityInputRegisters != 0)
            {
                try
                {
                    readInputRegisters = modbus.ReadInputRegisters(0, (ushort)(quantityInputRegisters));
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
            return readInputRegisters;
        }

        public static int[] LayDuLieuTCPHoldingRegister(ModbusClient modbus, ushort quantityHoldingRegisters, ThietBiModel thietBiModel)
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
                            int startAddress = i * DonViQuantityMoiLanDoc;
                            int quantity = DonViQuantityMoiLanDoc;
                            var temp= modbus.ReadHoldingRegisters(startAddress, (ushort)(quantity));
                            readHoldingRegister.AddRange(temp.ToList());
                        }
                        else if (i == soNguyenSauChia)
                        {
                            int startAddress = i * DonViQuantityMoiLanDoc;
                            int quantity = quantityHoldingRegisters % DonViQuantityMoiLanDoc;
                            var temp = modbus.ReadHoldingRegisters(startAddress, (ushort)(quantity));
                            readHoldingRegister.AddRange(temp.ToList());
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
