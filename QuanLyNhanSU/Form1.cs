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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        private void Form1_Load(object sender, EventArgs e)
        {
            txtTaiKhoan.Focus();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {

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
                        //  DashBoards tc = new DashBoards();
                        TrangChu tc = new TrangChu();
                        tc.PhanQuyen(quyen); // Gọi hàm phân quyền
                        this.Hide();

                        tc.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        // KHÔNG TÌM THẤY (Đăng nhập thất bại)
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMatKhau.Clear();
                        txtTaiKhoan.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }   

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult traloi = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap.PerformClick();
            }
        }
    }
}
