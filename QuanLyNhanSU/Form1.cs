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

                    // --- CÂU LỆNH SQL CẢI TIẾN ---
                    // Ta cần lấy: IDQuyen (để phân quyền), MANV (để chấm công), HOTEN (để hiển thị xin chào)
                    // Nên phải JOIN bảng TAIKHOAN và NHANVIEN
                    string sql = @"SELECT T.IDQuyen, T.MANV, N.HOTEN 
                           FROM tb_TAIKHOAN T 
                           INNER JOIN tb_NHANVIEN N ON T.MANV = N.MANV 
                           WHERE T.TenDangNhap = @user AND T.MatKhau = @pass";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user", txtTaiKhoan.Text);
                    cmd.Parameters.AddWithValue("@pass", txtMatKhau.Text);

                    // Dùng DataAdapter để lấy dữ liệu về (An toàn và lấy được nhiều cột)
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // 3. Kiểm tra kết quả
                    if (dt.Rows.Count > 0) // Tìm thấy tài khoản
                    {
                        // --- LƯU THÔNG TIN VÀO CONST (QUAN TRỌNG) ---
                        Const.LoaiTaiKhoan = int.Parse(dt.Rows[0]["IDQuyen"].ToString());
                        Const.MaNV = dt.Rows[0]["MANV"].ToString();      // Lưu mã để dùng bên Chấm công
                        Const.TenHienThi = dt.Rows[0]["HOTEN"].ToString(); // Lưu tên để hiện Xin chào

                        // Hiển thị thông báo
                        string vaiTro = (Const.LoaiTaiKhoan == 1) ? "Quản trị viên (Admin)" : "Nhân viên";
                        MessageBox.Show("Đăng nhập thành công!\nXin chào: " + Const.TenHienThi + "\nVai trò: " + vaiTro, "Thông báo");

                        // 4. Mở Form Trang Chủ (Dashboard)
                        // Lưu ý: Nếu form chính của bạn tên là 'DashBoards' thì sửa 'TrangChu' thành 'DashBoards'
                        TrangChu tc = new TrangChu();

                        this.Hide(); // Ẩn form đăng nhập đi
                        tc.ShowDialog(); // Hiện form chính lên

                        // Khi form chính đóng lại thì đóng luôn ứng dụng (hoặc hiện lại form login tùy logic)
                        this.Close();
                    }
                    else
                    {
                        // Đăng nhập thất bại
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
