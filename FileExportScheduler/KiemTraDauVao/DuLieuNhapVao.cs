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

        /// <summary>
        /// remove cảnh báo lỗi đã từng có trên 1 cell
        /// </summary>
        /// <param name="cell">cell cần xóa đi cái lỗi đã từng có</param>
        private static void RemoveErrorText(DataGridViewCell cell)
        {
            cell.ErrorText = "";
        }
       
        public static bool KiemTraDuLieuNhapVao(DataGridView dgvDsDuLieu)
        {

            bool isValidate = true;

            foreach (DataGridViewRow dr in dgvDsDuLieu.Rows)
            {
                //break when in last row because last row is null row
                if (dr.Index == dgvDsDuLieu.Rows.Count - 1)
                {
                    break;
                }

                foreach (DataGridViewCell cellCanCheck in dr.Cells)
                {
                    bool isValidateTemp = true;
                    switch (cellCanCheck.ColumnIndex)
                    {

                        case 0:
                            isValidateTemp = KiemTraTungCellCotTen(dgvDsDuLieu, cellCanCheck);
                            break;
                        case 1:
                            isValidateTemp = KiemTraTungCellCotDiemDo(dgvDsDuLieu, cellCanCheck);
                            break;
                        case 2:
                            isValidateTemp = KiemTraTungCellCotDiaChi(dgvDsDuLieu, cellCanCheck);
                            break;
                        case 3:
                            isValidateTemp = KiemTraTungCellCotScale(dgvDsDuLieu, cellCanCheck);
                            break;
                        case 4:
                            isValidateTemp = KiemTraTungCellCotDonViDo(dgvDsDuLieu, cellCanCheck);
                            break;
                    }
                    if (isValidateTemp == false)
                    {
                        isValidate = false;
                    }
                }
            }
            return isValidate;
        }
        

        /// <summary>
        /// kiểm tra từng cell cột scale
        /// </summary>
        /// <param name="dgvDsDuLieu">datagridview cần validate</param>
        /// <param name="cell">cell cần kiểm tra validate trong datagridview</param>
        /// <returns></returns>
        public static bool KiemTraTungCellCotScale(DataGridView dgvDsDuLieu, DataGridViewCell cell)
        {
            //Kiem tra null
            if (IsNullOrBlankValue(cell))
            {
                cell.ErrorText = "Scale không để trống";
                return false;
            }

            //Kiem tra dinh dang
            if (!DinhDangScale(cell.Value.ToString()))
            {
                cell.ErrorText = "Scale sai định dạng";
                return false;
            }

            RemoveErrorText(cell);
            return true;
        }

        /// <summary>
        /// kiểm tra từng cell cột đơn vị đo
        /// </summary>
        /// <param name="dgvDsDuLieu">datagridview cần validate</param>
        /// <param name="cell">cell cần kiểm tra validate trong datagridview</param>
        /// <returns></returns>
        public static bool KiemTraTungCellCotDonViDo(DataGridView dgvDsDuLieu, DataGridViewCell cell)
        {
            //Kiem tra null
            if (IsNullOrBlankValue(cell))
            {
                cell.ErrorText = "Đơn vị đo không để trống";
                return false;
            }

            //Kiem tra dinh dang
            var regex = new Regex(@"^[a-zA-Z0-9_.-]+$");
            if (!regex.IsMatch(cell.Value.ToString()))
            {
                cell.ErrorText = "Đơn vị đo sai định dạng";
                return false;
            }

            RemoveErrorText(cell);
            return true;
        }

        /// <summary>
        /// kiểm tra từng cell cột địa chỉ
        /// </summary>
        /// <param name="dgvDsDuLieu">datagridview cần validate</param>
        /// <param name="cellCanCheck">cell cần kiểm tra validate trong datagridview</param>
        /// <returns></returns>
        public static bool KiemTraTungCellCotDiaChi(DataGridView dgvDsDuLieu, DataGridViewCell cellCanCheck)
        {
            //Kiem tra null
            if (IsNullOrBlankValue(cellCanCheck))
            {
                cellCanCheck.ErrorText = "Địa chỉ không được để trống";
                return false;
            }

            //Kiểm tra định dạng
            if (!DinhDangDiaChi(cellCanCheck.Value.ToString()))
            {
                cellCanCheck.ErrorText = "Địa chỉ sai định dạng";
                return false;
            }

            //Kiểm tra trùng lặp địa chỉ
            List<String> dsDiaChiExist = new List<string>();

            foreach (DataGridViewRow dr in dgvDsDuLieu.Rows)
            {
                //break when in last row because last row is null row
                if (dr.Index == dgvDsDuLieu.Rows.Count - 1)
                {
                    break;
                }

                DataGridViewCell cellUnit = dr.Cells["diaChi"];
                if (cellUnit != cellCanCheck)
                {
                    if (IsNullOrBlankValue(cellUnit))
                    {
                        cellCanCheck.ErrorText = "Địa chỉ không để trống";
                        return false;
                    }

                    dsDiaChiExist.Add(cellUnit.Value.ToString());
                }
            }

            if (dsDiaChiExist.Contains(cellCanCheck.Value.ToString()))
            {
                cellCanCheck.ErrorText = "Địa chỉ đã tồn tại";
                return false;
            }

            RemoveErrorText(cellCanCheck);
            return true;
        }

        /// <summary>
        /// kiểm tra từng cell cột điểm đo
        /// </summary>
        /// <param name="dgvDsDuLieu">datagridview cần validate</param>
        /// <param name="cellCanCheck">cell cần kiểm tra validate trong datagridview</param>
        /// <returns></returns>
        public static bool KiemTraTungCellCotDiemDo(DataGridView dgvDsDuLieu, DataGridViewCell cellCanCheck)
        {

            //Kiem tra null
            if (IsNullOrBlankValue(cellCanCheck))
            {
                cellCanCheck.ErrorText = "Điểm đo không để trống";
                return false;
            }

            //Kiểm tra định dạng
            var regex = new Regex(@"^[a-zA-Z0-9_.-]+$");
            if (!regex.IsMatch(cellCanCheck.Value.ToString()))
            {
                cellCanCheck.ErrorText = "Tên điểm đo sai định dạng";
                return false;
            }

            //Kiểm tra trùng lặp địa chỉ
            List<String> dsTenVaDiemDoExist = new List<string>();

            string capTenVaDiemDoCanCheck = "";//cặp tên và điểm đo tương ứng đang cần check

            foreach (DataGridViewRow dr in dgvDsDuLieu.Rows)
            {
                //break when in last row because last row is null row
                if (dr.Index == dgvDsDuLieu.Rows.Count - 1)
                {
                    break;
                }

                DataGridViewCell cellDiemDoUnit = dr.Cells["diemDo"];
                DataGridViewCell cellTenUnit = dr.Cells["ten"];
                if (cellDiemDoUnit != cellCanCheck)
                {
                    if (cellTenUnit.Value != null && cellDiemDoUnit.Value != null)
                    {
                        dsTenVaDiemDoExist.Add(cellTenUnit.Value.ToString() + cellDiemDoUnit.Value.ToString());
                    }
                }
                else
                {
                    //lấy cặp tên điểm đo đang modify
                    DataGridViewCell cellDiemDoDangModify = dr.Cells["diemDo"];
                    DataGridViewCell cellTenDangModify = dr.Cells["ten"];
                    if (!IsNullOrBlankValue(cellTenDangModify) && !IsNullOrBlankValue(cellDiemDoDangModify) && DinhDangTenOrDiemDo(cellTenDangModify.Value.ToString()))
                    {
                        RemoveErrorText(cellTenDangModify);
                        capTenVaDiemDoCanCheck = cellTenDangModify.Value.ToString() + cellDiemDoDangModify.Value.ToString();
                    }
                }
            }

            if (dsTenVaDiemDoExist.Contains(capTenVaDiemDoCanCheck))
            {
                cellCanCheck.ErrorText = "Cặp tên và điểm đo đã tồn tại";
                return false;
            }

            RemoveErrorText(cellCanCheck);

            return true;

        }

        /// <summary>
        /// kiểm tra từng cell cột tên
        /// </summary>
        /// <param name="dgvDsDuLieu">datagridview cần validate</param>
        /// <param name="cellCanCheck">cell cần kiểm tra validate trong datagridview</param>
        /// <returns></returns>
        public static bool KiemTraTungCellCotTen(DataGridView dgvDsDuLieu, DataGridViewCell cellTenCanCheck)
        {
            //Kiem tra blank or null
            if (IsNullOrBlankValue(cellTenCanCheck))
            {
                cellTenCanCheck.ErrorText = "Tên không để trống";
                return false;
            }

            //Kiểm tra định dạng
            if (!DinhDangTenOrDiemDo(cellTenCanCheck.Value.ToString()))
            {
                cellTenCanCheck.ErrorText = "Tên sai định dạng";
                return false;
            }

            //Kiểm tra trùng lặp địa chỉ
            List<String> dsTenVaDiemDoExist = new List<string>();

            string capTenVaDiemDoCanCheck = "";//cặp tên và điểm đo tương ứng đang cần check

            foreach (DataGridViewRow dr in dgvDsDuLieu.Rows)
            {

                //break when in last row because last row is null row
                if (dr.Index == dgvDsDuLieu.Rows.Count - 1)
                {
                    break;
                }

                DataGridViewCell cellDiemDoUnit = dr.Cells["diemDo"];
                DataGridViewCell cellTenUnit = dr.Cells["ten"];
                if (cellTenUnit != cellTenCanCheck)//thêm các cặp tên và điểm đo vào danh sách exist (loại trừ tên đang modify)
                {
                    if (cellTenUnit.Value != null && cellDiemDoUnit.Value != null)
                    {
                        dsTenVaDiemDoExist.Add(cellTenUnit.Value.ToString() + cellDiemDoUnit.Value.ToString());
                    }
                }
                else
                {
                    //lấy cặp tên điểm đo đang modify
                    DataGridViewCell cellDiemDoDangModify = dr.Cells["diemDo"];
                    DataGridViewCell cellTenDangModify = dr.Cells["ten"];
                    if (!IsNullOrBlankValue(cellTenDangModify) && !IsNullOrBlankValue(cellDiemDoDangModify) && DinhDangTenOrDiemDo(cellDiemDoDangModify.Value.ToString()))
                    {
                        RemoveErrorText(cellDiemDoDangModify);
                        capTenVaDiemDoCanCheck = cellTenDangModify.Value.ToString() + cellDiemDoDangModify.Value.ToString();
                    }
                }
            }

            if (dsTenVaDiemDoExist.Contains(capTenVaDiemDoCanCheck))
            {
                cellTenCanCheck.ErrorText = "Cặp tên và điểm đo đã tồn tại";
                return false;
            }

            RemoveErrorText(cellTenCanCheck);
            return true;

        }
        #region Ham kiem tra cap do local
        private static bool IsNullOrBlankValue(DataGridViewCell cell)
        {
            //Kiem tra null
            if (cell.Value == null)
            {
                return true;
            }

            //Kiem tra white space
            if (string.IsNullOrWhiteSpace(cell.Value.ToString()))
            {
                return true;
            }

            return false;
        }
        private static bool DinhDangDiaChi(string diaChi)
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
        private static bool DinhDangTenOrDiemDo(string ten)
        {
            var regex = new Regex(@"^[a-zA-Z0-9_.-]+$");

            if (!regex.IsMatch(ten))
            {
                return false;
            }
            return true;
        }
        private static bool DinhDangScale(string scale)
        {
            Double scaleNumber;
            try
            {
                 scaleNumber = Double.Parse(scale);
            }catch(Exception ex)
            {
                return false;
            }
            if (scaleNumber <= 0)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
