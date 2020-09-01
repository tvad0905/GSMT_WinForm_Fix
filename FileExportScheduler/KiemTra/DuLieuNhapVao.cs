using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExportScheduler.KiemTra
{
    public static class DuLieuNhapVao
    {
        public static bool KiemTraDuLieuNhapVao(DataGridView dgvDsDuLieu)
        {
            List<string> dsKeyDiemDoVaChat = new List<string>();
            List<string> dsDiaChi = new List<string>();
            dsKeyDiemDoVaChat.Clear();
            dsDiaChi.Clear();
            bool isPassed = true;
            foreach (DataGridViewRow dr in dgvDsDuLieu.Rows)
            {
                if (dr.Index == dgvDsDuLieu.Rows.Count - 1)
                {
                    break;
                }

                if (XetLoiDinhDangNhapLieu(dr.Cells[0].Value) != "" || XetLoiDinhDangNhapLieu(dr.Cells[1].Value) != "")
                {
                    dr.Cells[0].ErrorText = XetLoiDinhDangNhapLieu(dr.Cells[0].Value);
                    dr.Cells[1].ErrorText = XetLoiDinhDangNhapLieu(dr.Cells[1].Value);
                    isPassed = false;
                }
                else
                {
                    dr.Cells[0].ErrorText = "";
                    dr.Cells[1].ErrorText = "";


                    if (dsKeyDiemDoVaChat.Contains(dr.Cells[0].Value.ToString() + dr.Cells[1].Value.ToString()))
                    {

                        dr.Cells[1].ErrorText = "Tên bị trùng";
                        dr.Cells[0].ErrorText = "Tên bị trùng";
                        isPassed = false;
                    }
                }


                if (XetLoiDinhDangNhapLieu(dr.Cells[2].Value) != "")
                {
                    dr.Cells[2].ErrorText = XetLoiDinhDangNhapLieu(dr.Cells[2].Value);
                    isPassed = false;
                }
                else
                {
                    dr.Cells[2].ErrorText = "";

                    if (dr.Cells[2].ColumnIndex == 2)
                    {
                        if (!KiemTraDiaChi(dr.Cells[2].Value.ToString()))
                        {
                            dr.Cells[2].ErrorText = "Sai định dạng";
                            isPassed = false;
                        }
                        else if (dsDiaChi.Contains(dr.Cells[2].Value.ToString()))
                        {
                            dr.Cells[2].ErrorText = "Địa chỉ bị trùng";
                            isPassed = false;
                        }
                        else
                        {
                            dr.Cells[2].ErrorText = "";
                        }
                    }
                }


                if (XetLoiDinhDangNhapLieu(dr.Cells[3].Value) != "")
                {
                    dr.Cells[3].ErrorText = XetLoiDinhDangNhapLieu(dr.Cells[3].Value);
                    isPassed = false;
                }
                else
                {
                    dr.Cells[3].ErrorText = "";

                    if (dr.Cells[3].ColumnIndex == 3)
                    {
                        var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                        if (!regex.IsMatch(dr.Cells[3].Value.ToString()))
                        {
                            dr.Cells[3].ErrorText = "Scale sai định dạng";
                            isPassed = false;
                        }


                    }

                }

                if (dr.Cells[0].Value != null && dr.Cells[1].Value != null && dr.Cells[2].Value != null)
                {
                    dsDiaChi.Add(dr.Cells[2].Value.ToString());
                    dsKeyDiemDoVaChat.Add(dr.Cells[0].Value.ToString() + dr.Cells[1].Value.ToString());
                }


            }
            if (dsKeyDiemDoVaChat.Count == 0 && dsDiaChi.Count == 0)
            {

                isPassed = false;
            }
            return isPassed;
        }

        private static bool KiemTraDiaChi(string diaChi)
        {
            if (diaChi.Length != 5)
                return false;
            bool error = true;
            try
            {
                int address = Convert.ToInt32(diaChi);
                if (address < 0 || (address > 19999 && address < 30000) || (address > 39999 && address < 40000) || address > 49999)
                {
                    error = false;
                }
            }
            catch
            {
                error = false;
            }
            return error;
        }
        private static bool KiemTraDuLieuTrong(object giaTri)
        {
            if (giaTri == null || giaTri.ToString().Trim() == string.Empty)
            {
                return false;
            }
            return true;
        }
        private static bool KiemTraGiaTriHopLe(object giaTri)
        {
            if (!Regex.IsMatch(giaTri.ToString(), @"^[a-zA-Z0-9_.-]+$"))
            {
                return false;
            }
            return true;
        }
        private static string XetLoiDinhDangNhapLieu(object giaTri)
        {
            if (!KiemTraDuLieuTrong(giaTri))
            {
                return "Dữ liệu trống";
            }
            else
            {
                if (!KiemTraGiaTriHopLe(giaTri))
                {
                    return "Sai định dạng";
                }
                else
                {

                    return "";
                }
            }

        }
    }
}
