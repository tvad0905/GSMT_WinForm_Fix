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
        public static bool[] LayDuLieuCOMCoils(SerialPort serialPort, ushort quantityCoils,ThietBiModel thietBiModel )
        {
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            bool[] readCoil = new bool[quantityCoils];
            try
            {
                byte slaveAddress = 1;
                readCoil = master.ReadCoils(slaveAddress, 0, (ushort)(quantityCoils));
            }
            catch (TimeoutException ex)
            {
                if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.KhongKetNoi))
                {
                    ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.KhongKetNoi);
                }
                thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi không đọc được dữ liệu
            }
            catch (Modbus.SlaveException ex)
            {
                if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.VuotQuaDuLieu))
                {
                    ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.VuotQuaDuLieu);
                }
                thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi số bản ghi cần đọc vượt quá lượng bản ghi trả về
            }
            catch (Exception ex)
            {
                throw;
            }
            return readCoil;
        }
        public static bool[] LayDuLieuCOMInputs(SerialPort serialPort, ushort quantityInputs, ThietBiModel thietBiModel)
        {
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            bool[] discreteInput = new bool[quantityInputs];
            try
            {
                byte slaveAddress = 1;
                discreteInput = master.ReadInputs(slaveAddress, 0, (ushort)(quantityInputs));
            }
            catch (TimeoutException ex)
            {
                if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.KhongKetNoi))
                {
                    ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.KhongKetNoi);
                }
                thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi không đọc được dữ liệu
            }
            catch (Modbus.SlaveException ex)
            {
                if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.VuotQuaDuLieu))
                {
                    ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.VuotQuaDuLieu);
                }
                thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi số bản ghi cần đọc vượt quá lượng bản ghi trả về
            }
            catch (Exception ex)
            {
                throw;
            }
            return discreteInput;
        }
        public static ushort[] LayDuLieuCOMInputRegisters(SerialPort serialPort, ushort quantityInputRegisters, ThietBiModel thietBiModel)
        {
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            ushort[] readRegister = new ushort[quantityInputRegisters];
            try
            {
                byte slaveAddress = 1;
                readRegister = master.ReadInputRegisters(slaveAddress, 0, (ushort)(quantityInputRegisters));
            }
            catch (TimeoutException ex)
            {
                if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.KhongKetNoi))
                {
                    ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.KhongKetNoi);
                }
                thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi không đọc được dữ liệu
            }
            catch (Modbus.SlaveException ex)
            {
                if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.VuotQuaDuLieu))
                {
                    ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.VuotQuaDuLieu);
                }
                thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi số bản ghi cần đọc vượt quá lượng bản ghi trả về
            }
            catch (Exception ex)
            {

                throw;
            }
            return readRegister;
        }
        public static ushort[] LayDuLieuCOMHoldingRegisters(SerialPort serialPort, ushort quantityHoldingRegisters, ThietBiModel thietBiModel)
        {
            IModbusMaster master = ModbusSerialMaster.CreateRtu(serialPort);
            ushort[] readHoldingRegisters = new ushort[quantityHoldingRegisters];
            try
            {
                byte slaveAddress = 1;
                readHoldingRegisters = master.ReadHoldingRegisters(slaveAddress, 0, (ushort)(quantityHoldingRegisters ));

            }
            catch (TimeoutException ex)
            {
                if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.KhongKetNoi))
                {
                    ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.KhongKetNoi);
                }
                thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi không đọc được dữ liệu
            }
            catch (Modbus.SlaveException ex)
            {
                if (!ThongBaoLoi.DanhSach[thietBiModel.Name].Contains(ThongBaoLoi.VuotQuaDuLieu))
                {
                    ThongBaoLoi.DanhSach[thietBiModel.Name].Add(ThongBaoLoi.VuotQuaDuLieu);
                }
                thietBiModel.TrangThaiTinHieu = TrangThaiKetNoi.Bad;
                //lỗi số bản ghi cần đọc vượt quá lượng bản ghi trả về
            }
            catch (Exception ex)
            {

                throw;
            }
            return readHoldingRegisters;
        }

    }
}
