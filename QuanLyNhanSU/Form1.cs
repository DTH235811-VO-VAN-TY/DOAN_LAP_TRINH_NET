using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; // Bắt buộc phải có thư viện này
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 1. Chuỗi kết nối chuẩn (Khớp với máy TUNG-IT của bạn)
        string connString = @"Data Source=TUNG-IT\MSSQL_EXP_2008R2;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        private void Form1_Load(object sender, EventArgs e)
        {
            // Có thể để trống hoặc đặt focus vào ô tài khoản
            // txtTaiKhoan.Focus();
        }

        // 2. SỰ KIỆN NÚT ĐĂNG NHẬP
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // A. Kiểm tra nhập liệu (Tránh ô trống)
            if (string.IsNullOrEmpty(txtTaiKhoan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTaiKhoan.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            // B. Kết nối SQL để kiểm tra
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Câu lệnh: Tìm IDQuyen dựa vào TenDangNhap và MatKhau
                    string sql = "SELECT IDQuyen FROM tb_TAIKHOAN WHERE TenDangNhap = @user AND MatKhau = @pass";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    // Dùng tham số @ để bảo mật, chống hack SQL Injection
                    cmd.Parameters.AddWithValue("@user", txtTaiKhoan.Text);
                    cmd.Parameters.AddWithValue("@pass", txtMatKhau.Text);

                    // Thực thi và lấy kết quả ô đầu tiên (Chính là IDQuyen)
                    object ketQua = cmd.ExecuteScalar();

                    if (ketQua != null) // TÌM THẤY (Đăng nhập thành công)
                    {
                        int quyen = Convert.ToInt32(ketQua);

                        // Hiển thị thông báo
                        string tenQuyen = (quyen == 1) ? "Admin (Quản trị)" : "User (Nhân viên)";
                        MessageBox.Show("Đăng nhập thành công!\nXin chào: " + tenQuyen, "Thông báo");

                        // C. MỞ TRANG CHỦ VÀ TRUYỀN QUYỀN
                        TrangChu tc = new TrangChu();

                        // Gọi hàm phân quyền bên TrangChu (Hàm này chúng ta đã viết ở bước trước)
                        tc.PhanQuyen(quyen);

                        this.Hide(); // Ẩn Form đăng nhập
                        tc.ShowDialog(); // Hiện Form Trang chủ

                        // Khi Trang chủ đóng thì thoát luôn chương trình
                        this.Close();
                    }
                    else // KHÔNG TÌM THẤY (Sai tài khoản hoặc mật khẩu)
                    {
                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMatKhau.Focus();
                        txtMatKhau.SelectAll();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 3. SỰ KIỆN NÚT THOÁT
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult traloi = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        // (Tùy chọn) Sự kiện nhấn Enter ở ô Mật khẩu thì tự Đăng nhập luôn
        /*
        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap.PerformClick();
            }
        }
        */
    }
}