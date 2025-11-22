using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyNhanSU
{
    public partial class add_UngLuong_form : Form
    {
        // 1. Cấu hình chuỗi kết nối (Thay đổi tên Server/Database của bạn vào đây)
        string strKetNoi = @"Data Source=LAPTOP-S5P1Q2HR\SQLEXPRESS;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        SqlConnection conn = null;

        // 2. Biến cờ hiệu
        bool isThem = false;
        int idHienTai = -1;
        public add_UngLuong_form()
        {
            InitializeComponent();
            conn = new SqlConnection(strKetNoi);
        }
        private void LoadData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // Lấy dữ liệu, giả sử tên bảng là tb_UNGLUONG
                string sql = "SELECT * FROM tb_UNGLUONG ORDER BY NGAY DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUngluong.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
            finally { conn.Close(); }
        }

        private void LockControl(bool lockState)
        {
            // Khóa/Mở khóa các ô nhập
            txtMaNV.Enabled = !lockState;
            txtTenNV.Enabled = !lockState;
            txtSoTienung.Enabled = !lockState;
            txtGhichu.Enabled = !lockState;
            dtpNgayungluong.Enabled = !lockState;

            // Khóa/Mở khóa nút
            btnThem.Enabled = lockState;
            btnSua.Enabled = lockState;
            btnXoa.Enabled = lockState;
            btnLuu.Enabled = !lockState; // Nút Lưu chỉ sáng khi đang nhập
            btnLamMoi.Enabled = true;
        }

        private void ResetInput()
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtSoTienung.Clear();
            txtGhichu.Clear();
            dtpNgayungluong.Value = DateTime.Now;
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUngluong.Rows[e.RowIndex];
                try
                {
                    idHienTai = Convert.ToInt32(row.Cells["ID"].Value);

                    // Đổ dữ liệu lên ô nhập
                    txtMaNV.Text = row.Cells["MANV"].Value?.ToString();

                    // Lưu ý: Database hiện tại không có cột HOTEN, nên ô Tên NV sẽ trống hoặc bạn phải tự join bảng
                    // txtTenNV.Text = ...; 

                    // Xử lý ngày tháng
                    if (row.Cells["NGAY"].Value != DBNull.Value)
                        dtpNgayungluong.Value = Convert.ToDateTime(row.Cells["NGAY"].Value);

                    // Xử lý số tiền (Format có dấu phẩy cho dễ nhìn)
                    if (row.Cells["SOTIEN"].Value != DBNull.Value)
                    {
                        decimal soTien = Convert.ToDecimal(row.Cells["SOTIEN"].Value);
                        txtSoTienung.Text = soTien.ToString("0.##");
                    }
                }
                catch (Exception) { }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            isThem = true;
            ResetInput();
            LockControl(false);
            txtMaNV.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (idHienTai == -1)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa!");
                return;
            }
            isThem = false;
            LockControl(false);
            // txtMaNV.Enabled = false; // Nếu muốn cấm sửa mã nhân viên thì bỏ comment dòng này
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (idHienTai == -1) { MessageBox.Show("Chưa chọn dòng để xóa!"); return; }

            if (MessageBox.Show("Bạn có chắc muốn xóa phiếu ứng lương này?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tb_UNGLUONG WHERE ID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", idHienTai);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã xóa!");
                    LoadData();
                    ResetInput();
                    idHienTai = -1;
                }
                catch (Exception ex) { MessageBox.Show("Lỗi xóa: " + ex.Message); }
                finally { conn.Close(); }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
            LockControl(true);
            idHienTai = -1;
            LoadData();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập liệu
            if (string.IsNullOrWhiteSpace(txtMaNV.Text) || string.IsNullOrWhiteSpace(txtSoTienung.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã NV và Số tiền ứng!");
                return;
            }

            decimal soTienUng;
            if (!decimal.TryParse(txtSoTienung.Text, out soTienUng))
            {
                MessageBox.Show("Số tiền phải là số!");
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // Tách Ngày, Tháng, Năm từ DateTimePicker
                int nam = dtpNgayungluong.Value.Year;
                int thang = dtpNgayungluong.Value.Month;
                DateTime ngayDayDu = dtpNgayungluong.Value;

                if (isThem)
                {
                    // INSERT: Tự động tách ngày tháng năm để lưu vào 3 cột
                    // Mặc định TRANGTHAI = 1
                    cmd.CommandText = @"INSERT INTO tb_UNGLUONG (NAM, THANG, NGAY, SOTIEN, MANV, TRANGTHAI) 
                                        VALUES (@Nam, @Thang, @Ngay, @SoTien, @MaNV, 1)";
                }
                else
                {
                    // UPDATE
                    cmd.CommandText = @"UPDATE tb_UNGLUONG 
                                        SET NAM=@Nam, THANG=@Thang, NGAY=@Ngay, 
                                            SOTIEN=@SoTien, MANV=@MaNV 
                                        WHERE ID=@ID";
                    cmd.Parameters.AddWithValue("@ID", idHienTai);
                }

                // Truyền tham số
                cmd.Parameters.AddWithValue("@Nam", nam);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Ngay", ngayDayDu);
                cmd.Parameters.AddWithValue("@SoTien", soTienUng);
                cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show(isThem ? "Thêm mới thành công!" : "Cập nhật thành công!");
                LoadData();
                LockControl(true);
                idHienTai = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu: " + ex.Message);
            }
            finally { conn.Close(); }
        }
    }
}
