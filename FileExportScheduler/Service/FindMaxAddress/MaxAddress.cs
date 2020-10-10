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
            ushort quantityMaxCoils = 0;
            ushort quantityMaxInputs = 0;
            ushort quantityMaxInputRegister = 0;
            ushort quantityMaxHoldingRegister = 0;
            foreach (KeyValuePair<string, DiemDoModel> diemDo in ThietBi.dsDiemDoGiamSat)
            {

                foreach (KeyValuePair<string, DuLieuModel> duLieu in diemDo.Value.DsDulieu)
                {
                    
                    ushort timDiaChiMax = (ushort)(Convert.ToUInt16(duLieu.Value.DiaChi)+1) ;
                    if (duLieu.Value.DiaChi.StartsWith("0"))
                    {
                        if (timDiaChiMax > quantityMaxCoils)
                        {
                            quantityMaxCoils = timDiaChiMax;
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

            listMaxAddress.Add(quantityMaxCoils);
            listMaxAddress.Add(quantityMaxInputs);
            listMaxAddress.Add(quantityMaxInputRegister);
            listMaxAddress.Add(quantityMaxHoldingRegister);

            return listMaxAddress;
        }
    }
}
