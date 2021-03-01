using ESProtocolConverter.Models.Slave;
using FileExportScheduler.Models.DiemDo;
using FileExportScheduler.Models.DuLieu;
using FileExportScheduler.Models.ThietBi.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Service.FinMaxAddress
{
    public static class MaxAddress
    {
        public static ArrayList GetMax(ThietBiModel ThietBi)
        {
            ArrayList listMaxAddress = new ArrayList();
            ushort quantityMaxCoils = 0;
            ushort quantityMaxInputs = 0;
            ushort quantityMaxInputRegister = 0;
            ushort quantityMaxHoldingRegister = 0;
            foreach (KeyValuePair<string, SlaveModel> slave in ThietBi.dsSlave)
            {

                foreach (KeyValuePair<string, DiemDoModel> diemDo in slave.Value.dsDiemDoGiamSat)
                {

                    foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
                    {

                        ushort timDiaChiMax = (ushort)(Convert.ToUInt16(duLieu.Value.DiaChi) + 1);
                        if (duLieu.Value.DiaChi.StartsWith("0"))
                        {
                            if (timDiaChiMax > quantityMaxCoils)
                            {
                                quantityMaxCoils = (ushort)(timDiaChiMax - ThietBi.MinAddressCoils);
                            }

                        }
                        else if (duLieu.Value.DiaChi.StartsWith("1"))
                        {
                            if (timDiaChiMax - 10000 > quantityMaxInputs)
                            {
                                quantityMaxInputs = (ushort)(timDiaChiMax - 10000);
                            }
                        }
                        else if (duLieu.Value.DiaChi.StartsWith("3"))
                        {
                            if (timDiaChiMax - 30000 > quantityMaxInputRegister)
                            {
                                quantityMaxInputRegister = (ushort)(timDiaChiMax - 30000);
                            }
                        }
                        else if (duLieu.Value.DiaChi.StartsWith("4"))
                        {
                            if (timDiaChiMax - 40000 > quantityMaxHoldingRegister)
                            {
                                quantityMaxHoldingRegister = (ushort)(timDiaChiMax - 40000);
                            }
                        }
                    }
                }
            }

            listMaxAddress.Add(quantityMaxCoils);
            listMaxAddress.Add(quantityMaxInputs);
            listMaxAddress.Add(quantityMaxInputRegister);
            listMaxAddress.Add(quantityMaxHoldingRegister);

            return listMaxAddress;
        }

        public static ArrayList GetMin(ThietBiModel ThietBi)
        {
            ArrayList listMinAddress = new ArrayList();
            ushort minAddressCoils = 10000;
            ushort minAddressInputs = 10000;
            ushort minAddressInputRegister = 10000;
            ushort minAddressHoldingRegister = 10000;
            foreach (KeyValuePair<string, SlaveModel> slave in ThietBi.dsSlave)
            {
                foreach (KeyValuePair<string, DiemDoModel> diemDo in slave.Value.dsDiemDoGiamSat)
                {

                    foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
                    {
                        var min = (ushort)(Convert.ToUInt16(duLieu.Value.DiaChi));
                        if (duLieu.Value.DiaChi.StartsWith("0"))
                        {
                            if (minAddressCoils > min)
                            {
                                minAddressCoils = min;
                            }

                        }
                        else if (duLieu.Value.DiaChi.StartsWith("1"))
                        {
                            if (min - 10000 < minAddressInputs)
                            {
                                minAddressInputs = (ushort)(min - 10000);
                            }
                        }
                        else if (duLieu.Value.DiaChi.StartsWith("3"))
                        {
                            if (min - 30000 < minAddressInputRegister)
                            {
                                minAddressInputRegister = (ushort)(min - 30000);
                            }
                        }
                        else if (duLieu.Value.DiaChi.StartsWith("4"))
                        {

                            if (min - 40000 < minAddressHoldingRegister)
                            {
                                minAddressHoldingRegister = (ushort)(min - 40000);
                            }
                        }
                    }
                }

            }
            listMinAddress.Add(minAddressCoils);
            listMinAddress.Add(minAddressInputs);
            listMinAddress.Add(minAddressInputRegister);
            listMinAddress.Add(minAddressHoldingRegister);
            return listMinAddress;

        }
    }
}
