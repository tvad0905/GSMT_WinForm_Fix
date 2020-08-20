using EasyModbus;
using FileExportScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    class DataRecivedIP
    {
        private void getDataDeviceIP(KeyValuePair<string, DeviceModel> deviceUnit, ModbusClient modbus)
        {
            //string[] output = new string[deviceUnit.Value.ListDuLieuChoTungPLC.Count];
            //doc tung dong trong list data cua 1 device
            for (int i = 0; i < deviceUnit.Value.ListDuLieuChoTungPLC.Count; i++)
            {
                DataModel duLieuTemp = deviceUnit.Value.ListDuLieuChoTungPLC.ElementAt(i).Value;
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

                    try
                    {
                        duLieuTemp.GiaTri = Convert.ToInt32(giaTriDuLieu) + "";
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
    }
}
