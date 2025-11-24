using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class UC_KhenThuong_KyLuat : UserControl
    {
        // --- Biến toàn cục ---
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daNhanVien;
        SqlDataAdapter daKhenThuong;
        SqlDataAdapter daKyLuat;

        public UC_KhenThuong_KyLuat()
        {
            InitializeComponent();
        }

        // --- HÀM LOAD CHÍNH ---
        private void UC_KhenThuong_KyLuat_Load(object sender, EventArgs e)
        {
            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng
                btnThem.Enabled = false; btnSua.Enabled = false; btnXoa.Enabled = false; btnLuu.Enabled = false; btnLamMoi.Enabled = false;
                btnThemKL.Enabled = false; btnSuaKL.Enabled = false; btnXoaKL.Enabled = false; btnLuuKL.Enabled = false; btnLamMoiKL.Enabled = false;

                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls) { if (c is TextBox) ((TextBox)c).ReadOnly = true; }

                // 3. GridView chỉ cho xem
                dgvKhenThuong.ReadOnly = true;
                dgvKyLuat.ReadOnly = true;
            }
            if (this.DesignMode) return;

            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                LoadSearchComboBox();
                LoadSearchComboBoxKL();

                // --- Tải Dữ liệu chung ---
                LoadNhanVienComboBox();
                LoadStaticComboBoxes();

                // --- Tải Tab Khen Thưởng ---
                SetupKhenThuong();

                // --- Tải Tab Kỷ Luật ---
                SetupKyLuat();

                // --- Cài đặt hiển thị ---
                txtTenNVkt.ReadOnly = true;
                txtTenNVkl.ReadOnly = true;
                LamMoiControlsKT();
                LamMoiControlsKL();
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI GỐC KHI TẢI FORM: " + ex.Message, "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ReloadNhanVien()
        {
            try
            {
                // 1. Xóa dữ liệu cũ trong bảng gốc và tải lại từ CSDL
                if (ds.Tables.Contains("tblNHANVIEN"))
                    ds.Tables["tblNHANVIEN"].Clear();

                if (daNhanVien == null)
                {
                    string sql = "SELECT MANV, HOTEN FROM TB_NHANVIEN";
                    daNhanVien = new SqlDataAdapter(sql, conn);
                }
                daNhanVien.Fill(ds, "tblNHANVIEN");

                // 2. Cập nhật bảng copy cho Khen Thưởng (tblNV_KT)
                if (ds.Tables.Contains("tblNV_KT"))
                {
                    ds.Tables["tblNV_KT"].Clear(); // Xóa dòng cũ
                    ds.Tables["tblNV_KT"].Merge(ds.Tables["tblNHANVIEN"]); // Nạp dòng mới từ bảng gốc
                }
                else
                {
                    // Nếu chưa có thì tạo mới (trường hợp hiếm)
                    DataTable dtNVKhenThuong = ds.Tables["tblNHANVIEN"].Copy();
                    dtNVKhenThuong.TableName = "tblNV_KT";
                    ds.Tables.Add(dtNVKhenThuong);
                }

                // 3. Cập nhật bảng copy cho Kỷ Luật (tblNV_KL)
                if (ds.Tables.Contains("tblNV_KL"))
                {
                    ds.Tables["tblNV_KL"].Clear();
                    ds.Tables["tblNV_KL"].Merge(ds.Tables["tblNHANVIEN"]);
                }
                else
                {
                    DataTable dtNVKyLuat = ds.Tables["tblNHANVIEN"].Copy();
                    dtNVKyLuat.TableName = "tblNV_KL";
                    ds.Tables.Add(dtNVKyLuat);
                }
            }
            catch (Exception ex)
            {

            }
        }
                
            
        private void LoadNhanVienComboBox()
        {
            string sQueryNhanVien = @"SELECT * FROM tb_NHANVIEN WHERE DATHOIVIEC = 0 OR DATHOIVIEC IS NULL";
            daNhanVien = new SqlDataAdapter(sQueryNhanVien, conn);
            daNhanVien.Fill(ds, "tblNHANVIEN");

            // Tạo bản sao cho Tab Khen Thưởng
            DataTable dtNVKhenThuong = ds.Tables["tblNHANVIEN"].Copy();
            dtNVKhenThuong.TableName = "tblNV_KT";
            ds.Tables.Add(dtNVKhenThuong);

            // Tạo bản sao cho Tab Kỷ Luật
            DataTable dtNVKyLuat = ds.Tables["tblNHANVIEN"].Copy();
            dtNVKyLuat.TableName = "tblNV_KL";
            ds.Tables.Add(dtNVKyLuat);

            cboMaNVkt.DataSource = ds.Tables["tblNV_KT"];
            cboMaNVkt.DisplayMember = "MANV";
            cboMaNVkt.ValueMember = "MANV";

            cboMaNVkl.DataSource = ds.Tables["tblNV_KL"];
            cboMaNVkl.DisplayMember = "MANV";
            cboMaNVkl.ValueMember = "MANV";
        }


        private void LoadStaticComboBoxes()
        {
            cboLoaiKT.Items.Add("Thưởng Lễ");
            cboLoaiKT.Items.Add("Thưởng Thành Tích");
            cboLoaiKT.Items.Add("Thưởng Chuyên Cần");

            cboLoaiKL.Items.Add("Khiển trách");
            cboLoaiKL.Items.Add("Cảnh cáo");
            cboLoaiKL.Items.Add("Hạ bậc lương");
            cboLoaiKL.Items.Add("Sa thải");
        }

        #region === KHEN THƯỞNG (TAB 1 - ĐÃ CẬP NHẬT TIỀN) ===

        private void SetupKhenThuong()
        {
            dgvKhenThuong.AutoGenerateColumns = false;
            dgvKhenThuong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 1. SELECT: Lấy thêm SOTIEN
            string sQuery = @"SELECT kt.*, nv.HOTEN 
                              FROM TB_KHENTHUONG kt
                              LEFT JOIN TB_NHANVIEN nv ON kt.MANV = nv.MANV";
            daKhenThuong = new SqlDataAdapter(sQuery, conn);
            daKhenThuong.Fill(ds, "tblKHENTHUONG");
            dgvKhenThuong.DataSource = ds.Tables["tblKHENTHUONG"];

            // 2. MAP DỮ LIỆU VÀO GRID
            dgvKhenThuong.Columns["SoQuyetDinh"].DataPropertyName = "SOQD";
            dgvKhenThuong.Columns["TenNhanVien"].DataPropertyName = "HOTEN";
            dgvKhenThuong.Columns["Ngayky"].DataPropertyName = "NGAYKY";
            dgvKhenThuong.Columns["LoaiKhenThuong"].DataPropertyName = "LOAIKT";
            dgvKhenThuong.Columns["NoidungKhenThuong"].DataPropertyName = "NOIDUNG";

            // Map cột Số Tiền (Cột này bạn đã thêm trong Designer)
            if (dgvKhenThuong.Columns.Contains("SoTienKT"))
            {
                dgvKhenThuong.Columns["SoTienKT"].DataPropertyName = "SOTIEN";
                dgvKhenThuong.Columns["SoTienKT"].DefaultCellStyle.Format = "N0"; // Định dạng 500,000
            }

            ds.Tables["tblKHENTHUONG"].PrimaryKey = new DataColumn[] { ds.Tables["tblKHENTHUONG"].Columns["SOQD"] };

            // 3. INSERT COMMAND
            string sThem = @"INSERT INTO TB_KHENTHUONG (SOQD, MANV, NGAYKY, LOAIKT, NOIDUNG, SOTIEN) 
                             VALUES (@SOQD, @MANV, @NGAYKY, @LOAIKT, @NOIDUNG, @SOTIEN)";
            daKhenThuong.InsertCommand = new SqlCommand(sThem, conn);
            daKhenThuong.InsertCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");
            daKhenThuong.InsertCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daKhenThuong.InsertCommand.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
            daKhenThuong.InsertCommand.Parameters.Add("@LOAIKT", SqlDbType.NVarChar, 50, "LOAIKT");
            daKhenThuong.InsertCommand.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
            daKhenThuong.InsertCommand.Parameters.Add("@SOTIEN", SqlDbType.Float, 0, "SOTIEN"); // Thêm tham số tiền

            // 4. UPDATE COMMAND
            string sSua = @"UPDATE TB_KHENTHUONG SET MANV=@MANV, NGAYKY=@NGAYKY, LOAIKT=@LOAIKT, NOIDUNG=@NOIDUNG, SOTIEN=@SOTIEN 
                            WHERE SOQD=@SOQD";
            daKhenThuong.UpdateCommand = new SqlCommand(sSua, conn);
            daKhenThuong.UpdateCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daKhenThuong.UpdateCommand.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
            daKhenThuong.UpdateCommand.Parameters.Add("@LOAIKT", SqlDbType.NVarChar, 50, "LOAIKT");
            daKhenThuong.UpdateCommand.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
            daKhenThuong.UpdateCommand.Parameters.Add("@SOTIEN", SqlDbType.Float, 0, "SOTIEN"); // Thêm tham số tiền
            daKhenThuong.UpdateCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");

            // 5. DELETE COMMAND
            string sXoa = @"DELETE FROM TB_KHENTHUONG WHERE SOQD=@SOQD";
            daKhenThuong.DeleteCommand = new SqlCommand(sXoa, conn);
            daKhenThuong.DeleteCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");
        }

        private void LamMoiControlsKT()
        {
            txtSoQDKT.Text = "";
            txtSoQDKT.Enabled = true;
            cboMaNVkt.SelectedIndex = -1;
            txtTenNVkt.Text = "";
            dtpNgayKyKT.Value = DateTime.Now;
            cboLoaiKT.SelectedIndex = -1;
            txtNoiDungKT.Text = "";
            txtTienKT.Text = "0"; // Reset ô tiền
            dgvKhenThuong.ClearSelection();
        }

        private void cboMaNVkt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNVkt.SelectedItem != null)
            {
                DataRowView drv = cboMaNVkt.SelectedItem as DataRowView;
                txtTenNVkt.Text = drv["HOTEN"].ToString();
            }
            else { txtTenNVkt.Text = ""; }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = daKhenThuong.Update(ds, "tblKHENTHUONG");
                MessageBox.Show($"Đã lưu {rowsAffected} thay đổi (Khen Thưởng) vào CSDL!");
                ds.Tables["tblKHENTHUONG"].Clear();
                daKhenThuong.Fill(ds, "tblKHENTHUONG");
                LamMoiControlsKT();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu Khen Thưởng: " + ex.Message);
                ds.Tables["tblKHENTHUONG"].RejectChanges();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoQDKT.Text) || cboMaNVkt.SelectedValue == null)
            {
                MessageBox.Show("Số Quyết Định và Mã Nhân Viên là bắt buộc!"); return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoQDKT.Text, @"^QDKT/\d+$"))
            {
                MessageBox.Show("Định dạng không hợp lệ!\nVui lòng nhập theo mẫu: QDKT/xxxx", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoQDKT.Focus(); return;
            }
            if (ds.Tables["tblKHENTHUONG"].Rows.Find(txtSoQDKT.Text) != null)
            {
                MessageBox.Show("Số Quyết Định này đã tồn tại!"); return;
            }

            // Xử lý lấy số tiền từ TextBox
            double soTien = 0;
            if (!double.TryParse(txtTienKT.Text, out soTien)) soTien = 0;

            DataRow newRow = ds.Tables["tblKHENTHUONG"].NewRow();
            newRow["SOQD"] = txtSoQDKT.Text;
            newRow["MANV"] = cboMaNVkt.SelectedValue;
            newRow["HOTEN"] = txtTenNVkt.Text;
            newRow["NGAYKY"] = dtpNgayKyKT.Value;
            newRow["LOAIKT"] = cboLoaiKT.Text;
            newRow["NOIDUNG"] = txtNoiDungKT.Text;
            newRow["SOTIEN"] = soTien; // Lưu tiền vào Row

            ds.Tables["tblKHENTHUONG"].Rows.Add(newRow);
            MessageBox.Show("Đã thêm vào bộ nhớ. Nhấn 'Lưu' để cập nhật.");
            LamMoiControlsKT();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtSoQDKT.Enabled == true || string.IsNullOrEmpty(txtSoQDKT.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục từ lưới để sửa."); return;
            }

            DataRow rowToUpdate = ds.Tables["tblKHENTHUONG"].Rows.Find(txtSoQDKT.Text);
            if (rowToUpdate != null)
            {
                // Xử lý lấy số tiền
                double soTien = 0;
                double.TryParse(txtTienKT.Text, out soTien);

                rowToUpdate.BeginEdit();
                rowToUpdate["MANV"] = cboMaNVkt.SelectedValue;
                rowToUpdate["HOTEN"] = txtTenNVkt.Text;
                rowToUpdate["NGAYKY"] = dtpNgayKyKT.Value;
                rowToUpdate["LOAIKT"] = cboLoaiKT.Text;
                rowToUpdate["NOIDUNG"] = txtNoiDungKT.Text;
                rowToUpdate["SOTIEN"] = soTien; // Sửa tiền
                rowToUpdate.EndEdit();
                MessageBox.Show("Đã sửa trong bộ nhớ. Nhấn 'Lưu' để cập nhật.");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtSoQDKT.Enabled == true || string.IsNullOrEmpty(txtSoQDKT.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục từ lưới để xóa."); return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa QĐ: " + txtSoQDKT.Text + "?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow rowToDelete = ds.Tables["tblKHENTHUONG"].Rows.Find(txtSoQDKT.Text);
                if (rowToDelete != null)
                {
                    rowToDelete.Delete();
                    MessageBox.Show("Đã xóa trong bộ nhớ. Nhấn 'Lưu' để cập nhật.");
                    LamMoiControlsKT();
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiControlsKT();
            ds.Tables["tblKHENTHUONG"].RejectChanges();
        }

        private void dgvKhenThuong_Click(object sender, EventArgs e)
        {
            if (dgvKhenThuong.SelectedRows.Count > 0)
            {
                DataRowView drv = dgvKhenThuong.SelectedRows[0].DataBoundItem as DataRowView;
                if (drv != null)
                {
                    txtSoQDKT.Text = drv["SOQD"].ToString();
                    cboMaNVkt.SelectedValue = drv["MANV"];
                    txtTenNVkt.Text = drv["HOTEN"].ToString();
                    dtpNgayKyKT.Value = Convert.ToDateTime(drv["NGAYKY"]);
                    cboLoaiKT.SelectedItem = drv["LOAIKT"].ToString();
                    txtNoiDungKT.Text = drv["NOIDUNG"].ToString();

                    // Hiển thị tiền lên TextBox (Format số đẹp)
                    if (drv.Row.Table.Columns.Contains("SOTIEN") && drv["SOTIEN"] != DBNull.Value)
                        txtTienKT.Text = double.Parse(drv["SOTIEN"].ToString()).ToString("N0");
                    else
                        txtTienKT.Text = "0";

                    txtSoQDKT.Enabled = false;
                }
            }
        }

        #endregion

        #region === KỶ LUẬT (TAB 2 - ĐÃ CẬP NHẬT TIỀN) ===

        private void SetupKyLuat()
        {
            dgvKyLuat.AutoGenerateColumns = false;
            dgvKyLuat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            string sQuery = @"SELECT kl.*, nv.HOTEN 
                              FROM TB_KYLUAT kl
                              LEFT JOIN TB_NHANVIEN nv ON kl.MANV = nv.MANV";
            daKyLuat = new SqlDataAdapter(sQuery, conn);
            daKyLuat.Fill(ds, "tblKYLUAT");
            dgvKyLuat.DataSource = ds.Tables["tblKYLUAT"];

            dgvKyLuat.Columns["dataGridViewTextBoxColumn1"].DataPropertyName = "SOQD";
            dgvKyLuat.Columns["dataGridViewTextBoxColumn2"].DataPropertyName = "HOTEN";
            dgvKyLuat.Columns["dataGridViewTextBoxColumn3"].DataPropertyName = "NGAYKY";
            dgvKyLuat.Columns["LoaiKyLuat"].DataPropertyName = "LOAIKL";
            dgvKyLuat.Columns["NoiDungKyLuat"].DataPropertyName = "NOIDUNG";

            // Map cột Số Tiền Kỷ Luật (Bạn đã thêm trong Designer)
            if (dgvKyLuat.Columns.Contains("SoTienKL"))
            {
                dgvKyLuat.Columns["SoTienKL"].DataPropertyName = "SOTIEN";
                dgvKyLuat.Columns["SoTienKL"].DefaultCellStyle.Format = "N0";
            }

            ds.Tables["tblKYLUAT"].PrimaryKey = new DataColumn[] { ds.Tables["tblKYLUAT"].Columns["SOQD"] };

            // INSERT
            string sThem = @"INSERT INTO TB_KYLUAT (SOQD, MANV, NGAYKY, LOAIKL, NOIDUNG, SOTIEN) 
                             VALUES (@SOQD, @MANV, @NGAYKY, @LOAIKL, @NOIDUNG, @SOTIEN)";
            daKyLuat.InsertCommand = new SqlCommand(sThem, conn);
            daKyLuat.InsertCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");
            daKyLuat.InsertCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daKyLuat.InsertCommand.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
            daKyLuat.InsertCommand.Parameters.Add("@LOAIKL", SqlDbType.NVarChar, 50, "LOAIKL");
            daKyLuat.InsertCommand.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
            daKyLuat.InsertCommand.Parameters.Add("@SOTIEN", SqlDbType.Float, 0, "SOTIEN"); // Thêm tiền

            // UPDATE
            string sSua = @"UPDATE TB_KYLUAT SET MANV=@MANV, NGAYKY=@NGAYKY, LOAIKL=@LOAIKL, NOIDUNG=@NOIDUNG, SOTIEN=@SOTIEN 
                            WHERE SOQD=@SOQD";
            daKyLuat.UpdateCommand = new SqlCommand(sSua, conn);
            daKyLuat.UpdateCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daKyLuat.UpdateCommand.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
            daKyLuat.UpdateCommand.Parameters.Add("@LOAIKL", SqlDbType.NVarChar, 50, "LOAIKL");
            daKyLuat.UpdateCommand.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
            daKyLuat.UpdateCommand.Parameters.Add("@SOTIEN", SqlDbType.Float, 0, "SOTIEN"); // Thêm tiền
            daKyLuat.UpdateCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");

            // DELETE
            string sXoa = @"DELETE FROM TB_KYLUAT WHERE SOQD=@SOQD";
            daKyLuat.DeleteCommand = new SqlCommand(sXoa, conn);
            daKyLuat.DeleteCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");
        }

        private void LamMoiControlsKL()
        {
            txtSoQDKL.Text = "";
            txtSoQDKL.Enabled = true;
            cboMaNVkl.SelectedIndex = -1;
            txtTenNVkl.Text = "";
            dtpNgayKy.Value = DateTime.Now;
            cboLoaiKL.SelectedIndex = -1;
            txtNoiDung.Text = "";
            txtTienKL.Text = "0"; // Reset tiền
            dgvKyLuat.ClearSelection();
        }

        private void cboMaNVkl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNVkl.SelectedItem != null)
            {
                DataRowView drv = cboMaNVkl.SelectedItem as DataRowView;
                txtTenNVkl.Text = drv["HOTEN"].ToString();
            }
            else { txtTenNVkl.Text = ""; }
        }

        private void btnLuuKL_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = daKyLuat.Update(ds, "tblKYLUAT");
                MessageBox.Show($"Đã lưu {rowsAffected} thay đổi (Kỷ Luật) vào CSDL!");
                ds.Tables["tblKYLUAT"].Clear();
                daKyLuat.Fill(ds, "tblKYLUAT");
                LamMoiControlsKL();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu Kỷ Luật: " + ex.Message);
                ds.Tables["tblKYLUAT"].RejectChanges();
            }
        }

        private void btnThemKL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoQDKL.Text) || cboMaNVkl.SelectedValue == null)
            {
                MessageBox.Show("Số Quyết Định và Mã Nhân Viên là bắt buộc!"); return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoQDKL.Text, @"^QD/\d+$"))
            {
                MessageBox.Show("Định dạng không hợp lệ!\nVui lòng nhập theo mẫu: QD/xxxx", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoQDKL.Focus(); return;
            }
            if (ds.Tables["tblKYLUAT"].Rows.Find(txtSoQDKL.Text) != null)
            {
                MessageBox.Show("Số Quyết Định này đã tồn tại!"); return;
            }

            // Xử lý tiền
            double soTien = 0;
            if (!double.TryParse(txtTienKL.Text, out soTien)) soTien = 0;

            DataRow newRow = ds.Tables["tblKYLUAT"].NewRow();
            newRow["SOQD"] = txtSoQDKL.Text;
            newRow["MANV"] = cboMaNVkl.SelectedValue;
            newRow["HOTEN"] = txtTenNVkl.Text;
            newRow["NGAYKY"] = dtpNgayKy.Value;
            newRow["LOAIKL"] = cboLoaiKL.Text;
            newRow["NOIDUNG"] = txtNoiDung.Text;
            newRow["SOTIEN"] = soTien; // Lưu tiền

            ds.Tables["tblKYLUAT"].Rows.Add(newRow);
            MessageBox.Show("Đã thêm vào bộ nhớ. Nhấn 'Lưu' để cập nhật.");
            LamMoiControlsKL();
        }

        private void btnSuaKL_Click(object sender, EventArgs e)
        {
            if (txtSoQDKL.Enabled == true || string.IsNullOrEmpty(txtSoQDKL.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục từ lưới để sửa."); return;
            }

            DataRow rowToUpdate = ds.Tables["tblKYLUAT"].Rows.Find(txtSoQDKL.Text);
            if (rowToUpdate != null)
            {
                // Xử lý tiền
                double soTien = 0;
                double.TryParse(txtTienKL.Text, out soTien);

                rowToUpdate.BeginEdit();
                rowToUpdate["MANV"] = cboMaNVkl.SelectedValue;
                rowToUpdate["HOTEN"] = txtTenNVkl.Text;
                rowToUpdate["NGAYKY"] = dtpNgayKy.Value;
                rowToUpdate["LOAIKL"] = cboLoaiKL.Text;
                rowToUpdate["NOIDUNG"] = txtNoiDung.Text;
                rowToUpdate["SOTIEN"] = soTien; // Sửa tiền
                rowToUpdate.EndEdit();
                MessageBox.Show("Đã sửa trong bộ nhớ. Nhấn 'Lưu' để cập nhật.");
            }
        }

        private void btnXoaKL_Click(object sender, EventArgs e)
        {
            if (txtSoQDKL.Enabled == true || string.IsNullOrEmpty(txtSoQDKL.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục từ lưới để xóa."); return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa QĐ: " + txtSoQDKL.Text + "?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow rowToDelete = ds.Tables["tblKYLUAT"].Rows.Find(txtSoQDKL.Text);
                if (rowToDelete != null)
                {
                    rowToDelete.Delete();
                    MessageBox.Show("Đã xóa trong bộ nhớ. Nhấn 'Lưu' để cập nhật.");
                    LamMoiControlsKL();
                }
            }
        }

        private void btnLamMoiKL_Click(object sender, EventArgs e)
        {
            LamMoiControlsKL();
            ds.Tables["tblKYLUAT"].RejectChanges();
        }

        private void dgvKyLuat_Click(object sender, EventArgs e)
        {
            if (dgvKyLuat.SelectedRows.Count > 0)
            {
                DataRowView drv = dgvKyLuat.SelectedRows[0].DataBoundItem as DataRowView;
                if (drv != null)
                {
                    txtSoQDKL.Text = drv["SOQD"].ToString();
                    cboMaNVkl.SelectedValue = drv["MANV"];
                    txtTenNVkl.Text = drv["HOTEN"].ToString();
                    dtpNgayKy.Value = Convert.ToDateTime(drv["NGAYKY"]);
                    cboLoaiKL.SelectedItem = drv["LOAIKL"].ToString();
                    txtNoiDung.Text = drv["NOIDUNG"].ToString();

                    // Hiển thị tiền lên TextBox
                    if (drv.Row.Table.Columns.Contains("SOTIEN") && drv["SOTIEN"] != DBNull.Value)
                        txtTienKL.Text = double.Parse(drv["SOTIEN"].ToString()).ToString("N0");
                    else
                        txtTienKL.Text = "0";

                    txtSoQDKL.Enabled = false;
                }
            }
        }

        #endregion

        #region === TÌM KIẾM ===

        private void LoadSearchComboBox()
        {
            var searchOptions = new[] {
                new { Text = "Tìm theo SOQD", Value = "SOQD" },
                new { Text = "Tìm theo Họ Tên", Value = "HOTEN" }
            };
            cboLoaiTKkt.DataSource = searchOptions;
            cboLoaiTKkt.DisplayMember = "Text";
            cboLoaiTKkt.ValueMember = "Value";
            cboLoaiTKkt.SelectedIndex = 0;
        }

        private void btnTimKiemKT_Click(object sender, EventArgs e)
        {
            string searchTerm = txtTuKhoaKT.Text.Trim().Replace("'", "''");
            string searchColumn = cboLoaiTKkt.SelectedValue.ToString();
            DataView dv = ds.Tables["tblKHENTHUONG"].DefaultView;

            if (string.IsNullOrEmpty(searchTerm)) { dv.RowFilter = string.Empty; return; }
            Type columnType = ds.Tables["tblKHENTHUONG"].Columns[searchColumn].DataType;
            try
            {
                if (columnType == typeof(string)) dv.RowFilter = $"{searchColumn} LIKE '%{searchTerm}%'";
                else dv.RowFilter = $"{searchColumn} = {searchTerm}";
            }
            catch { MessageBox.Show("Giá trị tìm kiếm không hợp lệ"); }
        }

        private void btnShowALL_Click(object sender, EventArgs e)
        {
            txtTuKhoaKT.Text = "";
            ds.Tables["tblKHENTHUONG"].DefaultView.RowFilter = string.Empty;
        }

        private void LoadSearchComboBoxKL()
        {
            var searchOptions = new[] {
                new { Text = "Tìm theo SOQD", Value = "SOQD" },
                new { Text = "Tìm theo Họ Tên", Value = "HOTEN" }
            };
            cboLoaiTK.DataSource = searchOptions;
            cboLoaiTK.DisplayMember = "Text";
            cboLoaiTK.ValueMember = "Value";
            cboLoaiTK.SelectedIndex = 0;
        }

        private void btnTimKiemKL_Click(object sender, EventArgs e)
        {
            string searchTerm = txtTuKhoaKL.Text.Trim().Replace("'", "''");
            string searchColumn = cboLoaiTK.SelectedValue.ToString();
            DataView dv = ds.Tables["tblKYLUAT"].DefaultView;

            if (string.IsNullOrEmpty(searchTerm)) { dv.RowFilter = string.Empty; return; }
            Type columnType = ds.Tables["tblKYLUAT"].Columns[searchColumn].DataType;
            try
            {
                if (columnType == typeof(string)) dv.RowFilter = $"{searchColumn} LIKE '%{searchTerm}%'";
                else dv.RowFilter = $"{searchColumn} = {searchTerm}";
            }
            catch { MessageBox.Show("Giá trị tìm kiếm không hợp lệ"); }
        }

        private void btnHienAll_Click(object sender, EventArgs e)
        {
            txtTuKhoaKL.Text = "";
            ds.Tables["tblKYLUAT"].DefaultView.RowFilter = string.Empty;
        }

        #endregion
    }
}