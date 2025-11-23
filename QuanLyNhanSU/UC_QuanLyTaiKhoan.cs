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

        public UC_QuanLyTaiKhoan()
        {
            InitializeComponent();
        }

        // --- SỰ KIỆN LOAD ---
        private void UC_QuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            // Chặn lỗi Designer
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            conn = new SqlConnection(connString);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                TaiDuLieuLenCombobox();
                TaiDanhSachTaiKhoan();
                LamMoi();
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv")
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

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

        private void TaiDanhSachTaiKhoan()
        {
            string sql = @"SELECT tk.TenDangNhap, tk.MatKhau, nv.HOTEN, q.TenQuyen, tk.MANV, tk.IDQuyen
                           FROM tb_TAIKHOAN tk
                           JOIN tb_NHANVIEN nv ON tk.MANV = nv.MANV
                           JOIN tb_QUYEN q ON tk.IDQuyen = q.IDQuyen";

            daTK = new SqlDataAdapter(sql, conn);
            dtTK = new DataTable();
            daTK.Fill(dtTK);

            dtTK.PrimaryKey = new DataColumn[] { dtTK.Columns["TenDangNhap"] };
            dgvTaiKhoan.DataSource = dtTK;

            if (dgvTaiKhoan.Columns["MANV"] != null) dgvTaiKhoan.Columns["MANV"].Visible = false;
            if (dgvTaiKhoan.Columns["IDQuyen"] != null) dgvTaiKhoan.Columns["IDQuyen"].Visible = false;
            if (dgvTaiKhoan.Columns["HOTEN"] != null) dgvTaiKhoan.Columns["HOTEN"].HeaderText = "Chủ tài khoản";
            if (dgvTaiKhoan.Columns["TenQuyen"] != null) dgvTaiKhoan.Columns["TenQuyen"].HeaderText = "Quyền hạn";
        }

        // --- SỰ KIỆN CLICK LƯỚI ---
        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];
                txtTaiKhoan.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value.ToString();

                if (row.Cells["MANV"].Value != DBNull.Value)
                    cboChonNhanVien.SelectedValue = row.Cells["MANV"].Value;

                if (row.Cells["IDQuyen"].Value != DBNull.Value)
                    cboPhanQuyen.SelectedValue = row.Cells["IDQuyen"].Value;

                txtTaiKhoan.Enabled = false;
                cboChonNhanVien.Enabled = false;
            }
        }

        // --- CÁC NÚT BẤM ---
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTaiKhoan.Text) || string.IsNullOrEmpty(txtMatKhau.Text) ||
                cboChonNhanVien.SelectedIndex == -1 || cboPhanQuyen.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sql = "INSERT INTO tb_TAIKHOAN (TenDangNhap, MatKhau, MANV, IDQuyen) VALUES (@User, @Pass, @MaNV, @Quyen)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@User", txtTaiKhoan.Text);
                cmd.Parameters.AddWithValue("@Pass", txtMatKhau.Text);
                cmd.Parameters.AddWithValue("@MaNV", cboChonNhanVien.SelectedValue);
                cmd.Parameters.AddWithValue("@Quyen", cboPhanQuyen.SelectedValue);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm tài khoản thành công!");
                LamMoi();
                TaiDanhSachTaiKhoan();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi (Trùng tên hoặc nhân viên đã có nick): " + ex.Message); }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Enabled) { MessageBox.Show("Chọn tài khoản cần sửa!"); return; }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sql = "UPDATE tb_TAIKHOAN SET MatKhau = @Pass, IDQuyen = @Quyen WHERE TenDangNhap = @User";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Pass", txtMatKhau.Text);
                cmd.Parameters.AddWithValue("@Quyen", cboPhanQuyen.SelectedValue);
                cmd.Parameters.AddWithValue("@User", txtTaiKhoan.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công!");
                LamMoi();
                TaiDanhSachTaiKhoan();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text == "") { MessageBox.Show("Chọn tài khoản cần xóa!"); return; }

            if (MessageBox.Show("Xóa tài khoản " + txtTaiKhoan.Text + "?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string sql = "DELETE FROM tb_TAIKHOAN WHERE TenDangNhap = @User";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@User", txtTaiKhoan.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Đã xóa!");
                    LamMoi();
                    TaiDanhSachTaiKhoan();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
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