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

        // --- HÀM LOAD CHÍNH (ĐÃ GỘP CODE TỪ _Load_1 VÀO ĐÂY) ---
        private void UC_KhenThuong_KyLuat_Load(object sender, EventArgs e)
        {
            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng thêm/sửa/xóa
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnLamMoi.Enabled = false;
               // btnInHD.Enabled = false;
                // (Nếu có nút Hủy hay Làm mới thì tắt nốt nếu muốn)

                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox) ((TextBox)c).ReadOnly = true;
                }

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

        /// <summary>
        /// Tải ComboBox Nhân Viên cho cả 2 tab
        /// </summary>
        private void LoadNhanVienComboBox()
        {
            string sQueryNhanVien = @"SELECT * FROM tb_NHANVIEN WHERE DATHOIVIEC = 0 OR DATHOIVIEC IS NULL"; ;
            daNhanVien = new SqlDataAdapter(sQueryNhanVien, conn);
            daNhanVien.Fill(ds, "tblNHANVIEN");

            DataTable dtNVKhenThuong = ds.Tables["tblNHANVIEN"].Copy();
            dtNVKhenThuong.TableName = "tblNV_KT";
            ds.Tables.Add(dtNVKhenThuong);

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

                // 4. Gán lại DataSource để ComboBox nhận diện thay đổi
                cboMaNVkt.DataSource = ds.Tables["tblNV_KT"];
                cboMaNVkt.DisplayMember = "MANV";
                cboMaNVkt.ValueMember = "MANV";

                cboMaNVkl.DataSource = ds.Tables["tblNV_KL"];
                cboMaNVkl.DisplayMember = "MANV";
                cboMaNVkl.ValueMember = "MANV";

                // Reset chọn
                cboMaNVkt.SelectedIndex = -1;
                cboMaNVkl.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật danh sách nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Tải dữ liệu tĩnh cho các ComboBox "Loại"
        /// </summary>
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

        #region === KHEN THƯỞNG (TAB 1) ===

        private void SetupKhenThuong()
        {
            dgvKhenThuong.AutoGenerateColumns = false;
            dgvKhenThuong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            string sQuery = @"SELECT kt.*, nv.HOTEN 
                              FROM TB_KHENTHUONG kt
                              LEFT JOIN TB_NHANVIEN nv ON kt.MANV = nv.MANV";
            daKhenThuong = new SqlDataAdapter(sQuery, conn);
            daKhenThuong.Fill(ds, "tblKHENTHUONG");
            dgvKhenThuong.DataSource = ds.Tables["tblKHENTHUONG"];

            dgvKhenThuong.Columns["SoQuyetDinh"].DataPropertyName = "SOQD";
            dgvKhenThuong.Columns["TenNhanVien"].DataPropertyName = "HOTEN";
            dgvKhenThuong.Columns["Ngayky"].DataPropertyName = "NGAYKY";
            dgvKhenThuong.Columns["LoaiKhenThuong"].DataPropertyName = "LOAIKT";
            dgvKhenThuong.Columns["NoidungKhenThuong"].DataPropertyName = "NOIDUNG";

            ds.Tables["tblKHENTHUONG"].PrimaryKey = new DataColumn[] { ds.Tables["tblKHENTHUONG"].Columns["SOQD"] };

            string sThem = @"INSERT INTO TB_KHENTHUONG (SOQD, MANV, NGAYKY, LOAIKT, NOIDUNG) 
                             VALUES (@SOQD, @MANV, @NGAYKY, @LOAIKT, @NOIDUNG)";
            daKhenThuong.InsertCommand = new SqlCommand(sThem, conn);
            daKhenThuong.InsertCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");
            daKhenThuong.InsertCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daKhenThuong.InsertCommand.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
            daKhenThuong.InsertCommand.Parameters.Add("@LOAIKT", SqlDbType.NVarChar, 50, "LOAIKT");
            daKhenThuong.InsertCommand.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");

            string sSua = @"UPDATE TB_KHENTHUONG SET MANV=@MANV, NGAYKY=@NGAYKY, LOAIKT=@LOAIKT, NOIDUNG=@NOIDUNG 
                            WHERE SOQD=@SOQD";
            daKhenThuong.UpdateCommand = new SqlCommand(sSua, conn);
            daKhenThuong.UpdateCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daKhenThuong.UpdateCommand.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
            daKhenThuong.UpdateCommand.Parameters.Add("@LOAIKT", SqlDbType.NVarChar, 50, "LOAIKT");
            daKhenThuong.UpdateCommand.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
            daKhenThuong.UpdateCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");

            string sXoa = @"DELETE FROM TB_KHENTHUONG WHERE SOQD=@SOQD";
            daKhenThuong.DeleteCommand = new SqlCommand(sXoa, conn);
            daKhenThuong.DeleteCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");

           /* btnLuu.Click += btnLuu_Click;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            dgvKhenThuong.Click += dgvKhenThuong_Click;*/
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
            dgvKhenThuong.ClearSelection();
        }

        // Tự động điền Tên NV (ĐÃ GỘP CODE TỪ _1 VÀO ĐÂY)
        private void cboMaNVkt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNVkt.SelectedItem != null)
            {
                DataRowView drv = cboMaNVkt.SelectedItem as DataRowView;
                txtTenNVkt.Text = drv["HOTEN"].ToString();
            }
            else
            {
                txtTenNVkt.Text = "";
            }
        }

        // Nút Lưu (Tab 1)
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

        // Nút Thêm (Tab 1)
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoQDKT.Text) || cboMaNVkt.SelectedValue == null)
            {
                MessageBox.Show("Số Quyết Định và Mã Nhân Viên là bắt buộc!");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoQDKT.Text, @"^QDKT/\d+$"))
            {
                MessageBox.Show("Định dạng không hợp lệ!\nVui lòng nhập theo mẫu: QD/xxxx (Ví dụ: QDKT/2025)", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoQDKT.Focus();
                return; // <--- Thêm lệnh này để dừng việc thêm mới
            }
            if (ds.Tables["tblKHENTHUONG"].Rows.Find(txtSoQDKT.Text) != null)
            {
                MessageBox.Show("Số Quyết Định này đã tồn tại!");
                return;
            }

            DataRow newRow = ds.Tables["tblKHENTHUONG"].NewRow();
            newRow["SOQD"] = txtSoQDKT.Text;
            newRow["MANV"] = cboMaNVkt.SelectedValue;
            newRow["HOTEN"] = txtTenNVkt.Text;
            newRow["NGAYKY"] = dtpNgayKyKT.Value;
            newRow["LOAIKT"] = cboLoaiKT.Text;
            newRow["NOIDUNG"] = txtNoiDungKT.Text;

            ds.Tables["tblKHENTHUONG"].Rows.Add(newRow);
            MessageBox.Show("Đã thêm vào bộ nhớ. Nhấn 'Lưu' để cập nhật.");
            LamMoiControlsKT();
        }

        // Nút Sửa (Tab 1)
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtSoQDKT.Enabled == true || string.IsNullOrEmpty(txtSoQDKT.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục từ lưới để sửa.");
                return;
            }

            DataRow rowToUpdate = ds.Tables["tblKHENTHUONG"].Rows.Find(txtSoQDKT.Text);
            if (rowToUpdate != null)
            {
                rowToUpdate.BeginEdit();
                rowToUpdate["MANV"] = cboMaNVkt.SelectedValue;
                rowToUpdate["HOTEN"] = txtTenNVkt.Text;
                rowToUpdate["NGAYKY"] = dtpNgayKyKT.Value;
                rowToUpdate["LOAIKT"] = cboLoaiKT.Text;
                rowToUpdate["NOIDUNG"] = txtNoiDungKT.Text;
                rowToUpdate.EndEdit();
                MessageBox.Show("Đã sửa trong bộ nhớ. Nhấn 'Lưu' để cập nhật.");
            }
        }

        // Nút Xóa (Tab 1)
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtSoQDKT.Enabled == true || string.IsNullOrEmpty(txtSoQDKT.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục từ lưới để xóa.");
                return;
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

        // Nút Làm Mới (Tab 1)
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiControlsKT();
            ds.Tables["tblKHENTHUONG"].RejectChanges();
        }

        // Click DGV (Tab 1)
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

                    txtSoQDKT.Enabled = false; // Khóa PK
                }
            }
        }

        #endregion

        #region === KỶ LUẬT (TAB 2) ===

        private void SetupKyLuat()
        {
            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng thêm/sửa/xóa
                btnThemKL.Enabled = false;
                btnSuaKL.Enabled = false;
                btnXoaKL.Enabled = false;
                btnLuuKL.Enabled = false;
                btnLamMoiKL.Enabled = false;
             //   btnInHD.Enabled = false;
                // (Nếu có nút Hủy hay Làm mới thì tắt nốt nếu muốn)

                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox) ((TextBox)c).ReadOnly = true;
                }

                // 3. GridView chỉ cho xem
                dgvKyLuat.ReadOnly = true;
            }
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

            ds.Tables["tblKYLUAT"].PrimaryKey = new DataColumn[] { ds.Tables["tblKYLUAT"].Columns["SOQD"] };

            string sThem = @"INSERT INTO TB_KYLUAT (SOQD, MANV, NGAYKY, LOAIKL, NOIDUNG) 
                             VALUES (@SOQD, @MANV, @NGAYKY, @LOAIKL, @NOIDUNG)";
            daKyLuat.InsertCommand = new SqlCommand(sThem, conn);
            daKyLuat.InsertCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");
            daKyLuat.InsertCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daKyLuat.InsertCommand.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
            daKyLuat.InsertCommand.Parameters.Add("@LOAIKL", SqlDbType.NVarChar, 50, "LOAIKL");
            daKyLuat.InsertCommand.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");

            string sSua = @"UPDATE TB_KYLUAT SET MANV=@MANV, NGAYKY=@NGAYKY, LOAIKL=@LOAIKL, NOIDUNG=@NOIDUNG 
                            WHERE SOQD=@SOQD";
            daKyLuat.UpdateCommand = new SqlCommand(sSua, conn);
            daKyLuat.UpdateCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daKyLuat.UpdateCommand.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
            daKyLuat.UpdateCommand.Parameters.Add("@LOAIKL", SqlDbType.NVarChar, 50, "LOAIKL");
            daKyLuat.UpdateCommand.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
            daKyLuat.UpdateCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");

            string sXoa = @"DELETE FROM TB_KYLUAT WHERE SOQD=@SOQD";
            daKyLuat.DeleteCommand = new SqlCommand(sXoa, conn);
            daKyLuat.DeleteCommand.Parameters.Add("@SOQD", SqlDbType.NVarChar, 10, "SOQD");

         /*  btnLuuKL.Click += btnLuuKL_Click;
            btnThemKL.Click += btnThemKL_Click;
            btnSuaKL.Click += btnSuaKL_Click;
            btnXoaKL.Click += btnXoaKL_Click;
            btnLamMoiKL.Click += btnLamMoiKL_Click;
            dgvKyLuat.Click += dgvKyLuat_Click;*/
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
            dgvKyLuat.ClearSelection();
        }

        // Tự động điền Tên NV (Tab 2) (ĐÃ GỘP CODE TỪ _1 VÀO ĐÂY)
        private void cboMaNVkl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNVkl.SelectedItem != null)
            {
                DataRowView drv = cboMaNVkl.SelectedItem as DataRowView;
                txtTenNVkl.Text = drv["HOTEN"].ToString();
            }
            else
            {
                txtTenNVkl.Text = "";
            }
        }

        // Nút Lưu (Tab 2)
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

        // Nút Thêm (Tab 2)
        private void btnThemKL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoQDKL.Text) || cboMaNVkl.SelectedValue == null)
            {
                MessageBox.Show("Số Quyết Định và Mã Nhân Viên là bắt buộc!");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoQDKL.Text, @"^QD/\d+$"))
            {
                MessageBox.Show("Định dạng không hợp lệ!\nVui lòng nhập theo mẫu: QD/xxxx (Ví dụ: QD/2025)", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoQDKL.Focus();
                return; // <--- Thêm lệnh này để dừng việc thêm mới
            }
            if (ds.Tables["tblKYLUAT"].Rows.Find(txtSoQDKL.Text) != null)
            {
                MessageBox.Show("Số Quyết Định này đã tồn tại!");
                return;
            }

            DataRow newRow = ds.Tables["tblKYLUAT"].NewRow();
            newRow["SOQD"] = txtSoQDKL.Text;
            newRow["MANV"] = cboMaNVkl.SelectedValue;
            newRow["HOTEN"] = txtTenNVkl.Text;
            newRow["NGAYKY"] = dtpNgayKy.Value;
            newRow["LOAIKL"] = cboLoaiKL.Text;
            newRow["NOIDUNG"] = txtNoiDung.Text;

            ds.Tables["tblKYLUAT"].Rows.Add(newRow);
            MessageBox.Show("Đã thêm vào bộ nhớ. Nhấn 'Lưu' để cập nhật.");
            LamMoiControlsKL();
        }

        // Nút Sửa (Tab 2)
        private void btnSuaKL_Click(object sender, EventArgs e)
        {
            if (txtSoQDKL.Enabled == true || string.IsNullOrEmpty(txtSoQDKL.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục từ lưới để sửa.");
                return;
            }

            DataRow rowToUpdate = ds.Tables["tblKYLUAT"].Rows.Find(txtSoQDKL.Text);
            if (rowToUpdate != null)
            {
                rowToUpdate.BeginEdit();
                rowToUpdate["MANV"] = cboMaNVkl.SelectedValue;
                rowToUpdate["HOTEN"] = txtTenNVkl.Text;
                rowToUpdate["NGAYKY"] = dtpNgayKy.Value;
                rowToUpdate["LOAIKL"] = cboLoaiKL.Text;
                rowToUpdate["NOIDUNG"] = txtNoiDung.Text;
                rowToUpdate.EndEdit();
                MessageBox.Show("Đã sửa trong bộ nhớ. Nhấn 'Lưu' để cập nhật.");
            }
        }

        // Nút Xóa (Tab 2)
        private void btnXoaKL_Click(object sender, EventArgs e)
        {
            if (txtSoQDKL.Enabled == true || string.IsNullOrEmpty(txtSoQDKL.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục từ lưới để xóa.");
                return;
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

        // Nút Làm Mới (Tab 2)
        private void btnLamMoiKL_Click(object sender, EventArgs e)
        {
            LamMoiControlsKL();
            ds.Tables["tblKYLUAT"].RejectChanges();
        }

        // Click DGV (Tab 2)
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

                    txtSoQDKL.Enabled = false; // Khóa PK
                }
            }
        }
        private void LoadSearchComboBox()
        {
            var searchOptions = new[] {
                new { Text = "Tìm theo SOQD", Value = "SOQD" },
                new { Text = "Tìm theo Họ Tên", Value = "HOTEN" }
    };
            // (Sửa lại tên 'cboTimKiemTheo' nếu tên của bạn khác)
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

            if (string.IsNullOrEmpty(searchTerm))
            {
                dv.RowFilter = string.Empty;
                return;
            }

            // Kiểm tra kiểu dữ liệu của cột
            Type columnType = ds.Tables["tblKHENTHUONG"].Columns[searchColumn].DataType;

            try
            {
                if (columnType == typeof(string))
                {
                    // Nếu là chuỗi → dùng LIKE
                    dv.RowFilter = $"{searchColumn} LIKE '%{searchTerm}%'";
                }
                else if (columnType == typeof(int) || columnType == typeof(double) || columnType == typeof(float))
                {
                    // Nếu là số → dùng so sánh chính xác
                    dv.RowFilter = $"{searchColumn} = {searchTerm}";
                }
                else
                {
                    // Nếu kiểu khác → xử lý tùy trường hợp
                    MessageBox.Show("Không hỗ trợ tìm kiếm với kiểu dữ liệu này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Giá trị tìm kiếm không hợp lệ với kiểu dữ liệu của cột", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        #endregion

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
            // (Sửa lại tên 'cboTimKiemTheo' nếu tên của bạn khác)
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

            if (string.IsNullOrEmpty(searchTerm))
            {
                dv.RowFilter = string.Empty;
                return;
            }

            // Kiểm tra kiểu dữ liệu của cột
            Type columnType = ds.Tables["tblKYLUAT"].Columns[searchColumn].DataType;

            try
            {
                if (columnType == typeof(string))
                {
                    // Nếu là chuỗi → dùng LIKE
                    dv.RowFilter = $"{searchColumn} LIKE '%{searchTerm}%'";
                }
                else if (columnType == typeof(int) || columnType == typeof(double) || columnType == typeof(float))
                {
                    // Nếu là số → dùng so sánh chính xác
                    dv.RowFilter = $"{searchColumn} = {searchTerm}";
                }
                else
                {
                    // Nếu kiểu khác → xử lý tùy trường hợp
                    MessageBox.Show("Không hỗ trợ tìm kiếm với kiểu dữ liệu này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Giá trị tìm kiếm không hợp lệ với kiểu dữ liệu của cột", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnHienAll_Click(object sender, EventArgs e)
        {
            txtTuKhoaKL.Text = "";
            ds.Tables["tblKYLUAT"].DefaultView.RowFilter = string.Empty;
        }
    }
}