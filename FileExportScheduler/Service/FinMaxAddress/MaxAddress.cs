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
        public static ArrayList Get(ThietBiModel ThietBi)
        {
            ArrayList listMaxAddress = new ArrayList();

            

            foreach (KeyValuePair<string, DiemDoModel> diemDo in ThietBi.dsDiemDoGiamSat)
            {

                foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
                {
                   
                    ushort timDiaChiMax = Convert.ToUInt16(duLieu.Value.DiaChi);
                    if (duLieu.Value.DiaChi.StartsWith("0"))
                    {
                        if (timDiaChiMax > ThietBi.MaxAddressCoils)
                        {
                            ThietBi.MaxAddressCoils = timDiaChiMax;
                        }
                    }
                    else if (duLieu.Value.DiaChi.StartsWith("1"))
                    {
                        if (timDiaChiMax - 10000 > ThietBi.MaxAddressInputs)
                        {
                            ThietBi.MaxAddressInputs = (ushort)(timDiaChiMax - 10000);
                        }
                    }
                    else if (duLieu.Value.DiaChi.StartsWith("3"))
                    {
                        if (timDiaChiMax - 30000 > ThietBi.MaxAddressInputRegisters)
                        {
                            ThietBi.MaxAddressInputRegisters = (ushort)(timDiaChiMax - 30000);
                        }
                    }
                    else if (duLieu.Value.DiaChi.StartsWith("4"))
                    {
                        if (timDiaChiMax - 40000 > ThietBi.MaxAddressHoldingRegisters)
                        {
                            ThietBi.MaxAddressHoldingRegisters = (ushort)(timDiaChiMax - 40000);
                        }
                    }
                }

            }

            listMaxAddress.Add(ThietBi.MaxAddressCoils);
            listMaxAddress.Add(ThietBi.MaxAddressInputs);
            listMaxAddress.Add(ThietBi.MaxAddressInputRegisters);
            listMaxAddress.Add(ThietBi.MaxAddressHoldingRegisters);

            return listMaxAddress;
        }
    }
}
