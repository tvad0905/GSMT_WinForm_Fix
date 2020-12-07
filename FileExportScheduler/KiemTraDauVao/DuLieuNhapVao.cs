using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExportScheduler.KiemTraDauVao
{
    public static class DuLieuNhapVao
    {
        public static bool KiemTraDuLieuNhapVao(DataGridView dgvDsDuLieu)
        {
            dgvDsDuLieu.EndEdit();
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

                if (XetLoiDinhDangNhapLieu(dr.Cells["ten"].Value) != "" || XetLoiDinhDangNhapLieu(dr.Cells["diemDo"].Value) != "")
                {
                    dr.Cells["ten"].ErrorText = XetLoiDinhDangNhapLieu(dr.Cells["ten"].Value);
                    dr.Cells["diemDo"].ErrorText = XetLoiDinhDangNhapLieu(dr.Cells["diemDo"].Value);
                    isPassed = false;
                }
                else
                {
                    dr.Cells["ten"].ErrorText = "";
                    dr.Cells["diemDo"].ErrorText = "";


                    if (dsKeyDiemDoVaChat.Contains(dr.Cells["ten"].Value.ToString() + dr.Cells["diemDo"].Value.ToString()))
                    {

                        dr.Cells["diemDo"].ErrorText = "Tên bị trùng";
                        dr.Cells["ten"].ErrorText = "Tên bị trùng";
                        isPassed = false;
                    }
                }


                if (XetLoiDinhDangNhapLieu(dr.Cells["diaChi"].Value) != "")
                {
                    dr.Cells["diaChi"].ErrorText = XetLoiDinhDangNhapLieu(dr.Cells["diaChi"].Value);
                    isPassed = false;
                }
                else
                {
                    dr.Cells["diaChi"].ErrorText = "";

                    if (dr.Cells["diaChi"].ColumnIndex == 2)
                    {
                        if (!KiemTraDiaChi(dr.Cells["diaChi"].Value.ToString()))
                        {
                            dr.Cells["diaChi"].ErrorText = "Sai định dạng";
                            isPassed = false;
                        }
                        else if (dsDiaChi.Contains(dr.Cells["diaChi"].Value.ToString()))
                        {
                            dr.Cells["diaChi"].ErrorText = "Địa chỉ bị trùng";
                            isPassed = false;
                        }
                        else
                        {
                            dr.Cells["diaChi"].ErrorText = "";
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

                if (dr.Cells["ten"].Value != null && dr.Cells["diemDo"].Value != null && dr.Cells["diaChi"].Value != null)
                {
                    dsDiaChi.Add(dr.Cells["diaChi"].Value.ToString());
                    dsKeyDiemDoVaChat.Add(dr.Cells["ten"].Value.ToString() + dr.Cells["diemDo"].Value.ToString());
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
