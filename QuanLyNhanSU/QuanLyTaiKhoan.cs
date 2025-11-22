using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class Form_QuanLyTaiKhoan : Form
    {
        // Chuỗi kết nối chuẩn
        string connString = @"Data Source=TUNG-IT\MSSQL_EXP_2008R2;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        // Khai báo global (chỉ dùng cho Adapter và tham chiếu)
        SqlConnection conn;
        SqlDataAdapter daTK;
        DataTable dtTK;

        public Form_QuanLyTaiKhoan()
        {
            InitializeComponent();
        }

        // --- 1. HÀM LOAD (Khởi động Form) ---
        private void Form_QuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString); // Khởi tạo kết nối, KHÔNG MỞ Ở ĐÂY

            try
            {
                // Mở kết nối tạm thời để tải ComboBox và Lưới
                if (conn.State == ConnectionState.Closed) conn.Open();

                TaiDuLieuLenCombobox();
                TaiDanhSachTaiKhoan();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi nghiêm trọng");
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close(); // Đóng ngay sau khi tải xong
            }
        }

        // --- A. TẢI DỮ LIỆU LÊN COMBOBOX ---
        private void TaiDuLieuLenCombobox()
        {
            // 1. Load Nhân viên
            SqlDataAdapter daNV = new SqlDataAdapter("SELECT MANV, HOTEN FROM tb_NHANVIEN", conn);
            DataTable dtNV = new DataTable();
            daNV.Fill(dtNV);

            cboChonNhanVien.DataSource = dtNV;
            cboChonNhanVien.DisplayMember = "HOTEN";
            cboChonNhanVien.ValueMember = "MANV";
            cboChonNhanVien.SelectedIndex = -1;

            // 2. Load Quyền
            SqlDataAdapter daQuyen = new SqlDataAdapter("SELECT IDQuyen, TenQuyen FROM tb_QUYEN", conn);
            DataTable dtQuyen = new DataTable();
            daQuyen.Fill(dtQuyen);

            cboPhanQuyen.DataSource = dtQuyen;
            cboPhanQuyen.DisplayMember = "TenQuyen";
            cboPhanQuyen.ValueMember = "IDQuyen";
            cboPhanQuyen.SelectedIndex = -1;
        }

        // --- B. TẢI DANH SÁCH TÀI KHOẢN LÊN LƯỚI ---
        private void TaiDanhSachTaiKhoan()
        {
            string sql = @"SELECT tk.TenDangNhap, tk.MatKhau, nv.HOTEN, q.TenQuyen, tk.MANV, tk.IDQuyen
                           FROM tb_TAIKHOAN tk
                           JOIN tb_NHANVIEN nv ON tk.MANV = nv.MANV
                           JOIN tb_QUYEN q ON tk.IDQuyen = q.IDQuyen";

            daTK = new SqlDataAdapter(sql, conn);
            dtTK = new DataTable();

            if (conn.State == ConnectionState.Closed) conn.Open(); // Mở kết nối để FILL
            daTK.Fill(dtTK);
            if (conn.State == ConnectionState.Open) conn.Close(); // Đóng ngay sau khi FILL

            dtTK.PrimaryKey = new DataColumn[] { dtTK.Columns["TenDangNhap"] };
            dgvTaiKhoan.DataSource = dtTK;

            // Thiết lập cột hiển thị (giữ nguyên)
            if (dgvTaiKhoan.Columns["MANV"] != null) dgvTaiKhoan.Columns["MANV"].Visible = false;
            if (dgvTaiKhoan.Columns["IDQuyen"] != null) dgvTaiKhoan.Columns["IDQuyen"].Visible = false;
            if (dgvTaiKhoan.Columns["HOTEN"] != null) dgvTaiKhoan.Columns["HOTEN"].HeaderText = "Chủ tài khoản";
            if (dgvTaiKhoan.Columns["TenQuyen"] != null) dgvTaiKhoan.Columns["TenQuyen"].HeaderText = "Quyền hạn";
        }

        // --- C. SỰ KIỆN CLICK VÀO LƯỚI ---
        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];

                txtTaiKhoan.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value.ToString();
                cboChonNhanVien.SelectedValue = row.Cells["MANV"].Value;
                cboPhanQuyen.SelectedValue = row.Cells["IDQuyen"].Value;

                // Khóa tên đăng nhập và nhân viên khi sửa
                txtTaiKhoan.Enabled = false;
                cboChonNhanVien.Enabled = false;
            }
        }

        // --- 1. NÚT THÊM (Ghi trực tiếp vào SQL) ---
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTaiKhoan.Text) || string.IsNullOrEmpty(txtMatKhau.Text) ||
                cboChonNhanVien.SelectedIndex == -1 || cboPhanQuyen.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            using (SqlConnection localConn = new SqlConnection(connString))
            {
                try
                {
                    localConn.Open();
                    string sql = "INSERT INTO tb_TAIKHOAN (TenDangNhap, MatKhau, MANV, IDQuyen) VALUES (@User, @Pass, @MaNV, @Quyen)";
                    SqlCommand cmd = new SqlCommand(sql, localConn);

                    cmd.Parameters.AddWithValue("@User", txtTaiKhoan.Text);
                    cmd.Parameters.AddWithValue("@Pass", txtMatKhau.Text);
                    cmd.Parameters.AddWithValue("@MaNV", cboChonNhanVien.SelectedValue);
                    cmd.Parameters.AddWithValue("@Quyen", cboPhanQuyen.SelectedValue);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm tài khoản thành công!");
                    LamMoi();
                    TaiDanhSachTaiKhoan(); // Tải lại lưới
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Lỗi Unique Key Violation
                        MessageBox.Show("Lỗi: Tài khoản đã tồn tại hoặc Nhân viên này đã có tài khoản.");
                    else
                        MessageBox.Show("Lỗi SQL: " + ex.Message);
                }
            }
        }

        // --- 2. NÚT SỬA (ĐỔI PASS / ĐỔI QUYỀN) ---
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Enabled) // Nếu đang ở chế độ Thêm, không cho sửa
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa trên lưới!");
                return;
            }
            if (string.IsNullOrEmpty(txtMatKhau.Text))
            {
                MessageBox.Show("Mật khẩu không được để trống!");
                return;
            }

            using (SqlConnection localConn = new SqlConnection(connString))
            {
                try
                {
                    string sql = "UPDATE tb_TAIKHOAN SET MatKhau = @Pass, IDQuyen = @Quyen WHERE TenDangNhap = @User";
                    SqlCommand cmd = new SqlCommand(sql, localConn);

                    cmd.Parameters.AddWithValue("@Pass", txtMatKhau.Text);
                    cmd.Parameters.AddWithValue("@Quyen", cboPhanQuyen.SelectedValue);
                    cmd.Parameters.AddWithValue("@User", txtTaiKhoan.Text);

                    localConn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật tài khoản thành công!");
                    LamMoi();
                    TaiDanhSachTaiKhoan();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        // --- 3. NÚT XÓA ---
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text == "") { MessageBox.Show("Chọn tài khoản cần xóa!"); return; }

            if (MessageBox.Show("Bạn chắc chắn muốn xóa tài khoản: " + txtTaiKhoan.Text + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (SqlConnection localConn = new SqlConnection(connString))
                {
                    try
                    {
                        string sql = "DELETE FROM tb_TAIKHOAN WHERE TenDangNhap = @User";
                        SqlCommand cmd = new SqlCommand(sql, localConn);
                        cmd.Parameters.AddWithValue("@User", txtTaiKhoan.Text);

                        localConn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã xóa tài khoản!");
                        LamMoi();
                        TaiDanhSachTaiKhoan();
                    }
                    catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
                }
            }
        }

        // --- 4. NÚT LÀM MỚI ---
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LamMoi()
        {
            txtTaiKhoan.Enabled = true;
            cboChonNhanVien.Enabled = true;
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            cboChonNhanVien.SelectedIndex = -1;
            cboPhanQuyen.SelectedIndex = -1;
            dgvTaiKhoan.ClearSelection();
        }
    }
}