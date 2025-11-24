using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class UC_QuanLyTaiKhoan : UserControl
    {
        // 1. Chuỗi kết nối chuẩn
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        SqlConnection conn;
        SqlDataAdapter daTK;
        DataTable dtTK;

        // --- BIẾN CỜ ĐỂ XÁC ĐỊNH TRẠNG THÁI ---
        bool flagThem = false; // True: Đang thêm, False: Đang sửa

        public UC_QuanLyTaiKhoan()
        {
            InitializeComponent();
        }

        // --- SỰ KIỆN LOAD ---
        private void UC_QuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
           
            if (this.DesignMode) return; // Chặn lỗi designer

            conn = new SqlConnection(connString);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                TaiDuLieuLenCombobox();
                TaiDanhSachTaiKhoan();

                // Mặc định khóa các ô nhập, chỉ hiện nút Thêm/Sửa/Xóa
                LockControl(true);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }

            // PHÂN QUYỀN (Code cũ của bạn)
            if (Const.LoaiTaiKhoan == 2)
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLamMoi.Enabled = false;
                btnLuu.Enabled = false;
                dgvTaiKhoan.ReadOnly = true;
            }
        }

        // --- HÀM KHÓA/MỞ GIAO DIỆN (QUAN TRỌNG) ---
        void LockControl(bool lockState)
        {
            // true = Khóa (chỉ xem), false = Mở (được nhập)
            txtTaiKhoan.Enabled = !lockState;
            txtMatKhau.Enabled = !lockState;
            cboChonNhanVien.Enabled = !lockState;
            cboPhanQuyen.Enabled = !lockState;

            // Nút chức năng
            btnThem.Enabled = lockState;
            btnSua.Enabled = lockState;
            btnXoa.Enabled = lockState;

            // Nút hành động
            btnLuu.Enabled = !lockState; // Chỉ sáng khi đang nhập
            btnLamMoi.Enabled = true;    // Nút Hủy/Làm mới luôn sáng
        }

        // --- CÁC HÀM TẢI DỮ LIỆU (GIỮ NGUYÊN) ---
        private void TaiDuLieuLenCombobox()
        {
            SqlDataAdapter daNV = new SqlDataAdapter("SELECT MANV, HOTEN FROM tb_NHANVIEN", conn);
            DataTable dtNV = new DataTable();
            daNV.Fill(dtNV);
            cboChonNhanVien.DataSource = dtNV;
            cboChonNhanVien.DisplayMember = "HOTEN";
            cboChonNhanVien.ValueMember = "MANV";
            cboChonNhanVien.SelectedIndex = -1;

            SqlDataAdapter daQuyen = new SqlDataAdapter("SELECT IDQuyen, TenQuyen FROM tb_QUYEN", conn);
            DataTable dtQuyen = new DataTable();
            daQuyen.Fill(dtQuyen);
            cboPhanQuyen.DataSource = dtQuyen;
            cboPhanQuyen.DisplayMember = "TenQuyen";
            cboPhanQuyen.ValueMember = "IDQuyen";
            cboPhanQuyen.SelectedIndex = -1;
        }

        private void TaiDanhSachTaiKhoan()
        {
            string sql = @"SELECT tk.TenDangNhap, tk.MatKhau, nv.HOTEN, q.TenQuyen, tk.MANV, tk.IDQuyen
                           FROM tb_TAIKHOAN tk
                           JOIN tb_NHANVIEN nv ON tk.MANV = nv.MANV
                           JOIN tb_QUYEN q ON tk.IDQuyen = q.IDQuyen";

            daTK = new SqlDataAdapter(sql, conn);
            dtTK = new DataTable();
            daTK.Fill(dtTK);
            dgvTaiKhoan.DataSource = dtTK;

            if (dgvTaiKhoan.Columns["MANV"] != null) dgvTaiKhoan.Columns["MANV"].Visible = false;
            if (dgvTaiKhoan.Columns["IDQuyen"] != null) dgvTaiKhoan.Columns["IDQuyen"].Visible = false;
            if (dgvTaiKhoan.Columns["HOTEN"] != null) dgvTaiKhoan.Columns["HOTEN"].HeaderText = "Chủ tài khoản";
            if (dgvTaiKhoan.Columns["TenQuyen"] != null) dgvTaiKhoan.Columns["TenQuyen"].HeaderText = "Quyền hạn";
        }

        // --- SỰ KIỆN CLICK LƯỚI ---
        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnThem.Enabled == true) // Chỉ cho chọn khi không ở chế độ thêm/sửa
            {
                DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];
                txtTaiKhoan.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value.ToString();

                if (row.Cells["MANV"].Value != DBNull.Value)
                    cboChonNhanVien.SelectedValue = row.Cells["MANV"].Value;

                if (row.Cells["IDQuyen"].Value != DBNull.Value)
                    cboPhanQuyen.SelectedValue = row.Cells["IDQuyen"].Value;
            }
        }

        // ==============================================================
        // CODE LOGIC CHÍNH: THÊM - SỬA - LƯU
        // ==============================================================

        // 1. NÚT THÊM (Chỉ mở khóa giao diện)
        private void btnThem_Click(object sender, EventArgs e)
        {
            flagThem = true;     // Đánh dấu là đang THÊM
            LockControl(false);  // Mở khóa nhập liệu

            // Xóa trắng để nhập mới
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            cboChonNhanVien.SelectedIndex = -1;
            cboPhanQuyen.SelectedIndex = -1;
            txtTaiKhoan.Focus();
        }

        // 2. NÚT SỬA (Chỉ mở khóa giao diện)
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text == "")
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa!");
                return;
            }

            flagThem = false;   // Đánh dấu là đang SỬA
            LockControl(false); // Mở khóa nhập liệu

            // Khi sửa thì KHÔNG cho sửa Tên đăng nhập và Người sở hữu (để tránh lỗi data)
            txtTaiKhoan.Enabled = false;
            cboChonNhanVien.Enabled = false;
        }

        // 3. NÚT LƯU (Thực hiện lệnh SQL)
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra nhập liệu
            if (string.IsNullOrEmpty(txtTaiKhoan.Text) || string.IsNullOrEmpty(txtMatKhau.Text) ||
                cboChonNhanVien.SelectedIndex == -1 || cboPhanQuyen.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (flagThem) // --- ĐANG THÊM MỚI ---
                {
                    // Kiểm tra trùng tên
                    SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM tb_TAIKHOAN WHERE TenDangNhap=@user", conn);
                    check.Parameters.AddWithValue("@user", txtTaiKhoan.Text);
                    if ((int)check.ExecuteScalar() > 0) { MessageBox.Show("Tên đăng nhập đã tồn tại!"); return; }

                    // Insert
                    cmd.CommandText = "INSERT INTO tb_TAIKHOAN (TenDangNhap, MatKhau, MANV, IDQuyen) VALUES (@User, @Pass, @MaNV, @Quyen)";
                }
                else // --- ĐANG SỬA ---
                {
                    // Update (chỉ sửa mật khẩu và quyền)
                    cmd.CommandText = "UPDATE tb_TAIKHOAN SET MatKhau = @Pass, IDQuyen = @Quyen WHERE TenDangNhap = @User";
                }

                // Truyền tham số
                cmd.Parameters.AddWithValue("@User", txtTaiKhoan.Text);
                cmd.Parameters.AddWithValue("@Pass", txtMatKhau.Text);
                cmd.Parameters.AddWithValue("@MaNV", cboChonNhanVien.SelectedValue);
                cmd.Parameters.AddWithValue("@Quyen", cboPhanQuyen.SelectedValue);

                cmd.ExecuteNonQuery();

                MessageBox.Show(flagThem ? "Thêm mới thành công!" : "Cập nhật thành công!");

                TaiDanhSachTaiKhoan(); // Refresh lưới
                LamMoi();              // Reset giao diện về ban đầu
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // 4. NÚT XÓA (Xóa ngay)
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text == "") { MessageBox.Show("Chọn tài khoản cần xóa!"); return; }

            if (MessageBox.Show("Xóa tài khoản " + txtTaiKhoan.Text + "?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tb_TAIKHOAN WHERE TenDangNhap = @User", conn);
                    cmd.Parameters.AddWithValue("@User", txtTaiKhoan.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Đã xóa!");
                    TaiDanhSachTaiKhoan();
                    LamMoi();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        // 5. NÚT HỦY / LÀM MỚI
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void LamMoi()
        {
            // Reset về trạng thái khóa
            LockControl(true);

            // Xóa trắng ô nhập
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            cboChonNhanVien.SelectedIndex = -1;
            cboPhanQuyen.SelectedIndex = -1;
            dgvTaiKhoan.ClearSelection();
        }
    }
}