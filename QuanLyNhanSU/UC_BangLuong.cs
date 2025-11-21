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
    public partial class UC_BangLuong : UserControl
    {
        public UC_BangLuong()
        {
            InitializeComponent();
        }
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daBangLuong;

        private void UC_BangLuong_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            // Cấu hình hiển thị DateTimePicker chỉ hiện Tháng/Năm
            dtpKyLuong.Format = DateTimePickerFormat.Custom;
            dtpKyLuong.CustomFormat = "MM/yyyy";
            dtpKyLuong.ShowUpDown = true;
            dgvBangLuong.AutoGenerateColumns = false;

            LoadBangLuong();
        }
        private void LoadBangLuong()
        {
            try
            {
                if (conn == null) conn = new SqlConnection(connString);
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Lấy ra Kỳ Công (Dạng số nguyên YYYYMM, ví dụ: 202511)
                int maKyCong = int.Parse(dtpKyLuong.Value.ToString("yyyyMM"));

                // Query lấy dữ liệu theo tháng đã chọn
                string sql = @"SELECT * FROM tb_BANGLUONG WHERE MAKYCONG = @MAKYCONG";

                daBangLuong = new SqlDataAdapter(sql, conn);
                daBangLuong.SelectCommand.Parameters.AddWithValue("@MAKYCONG", maKyCong);

                if (ds.Tables.Contains("tblBANGLUONG")) ds.Tables["tblBANGLUONG"].Clear();

                daBangLuong.Fill(ds, "tblBANGLUONG");
                dgvBangLuong.DataSource = ds.Tables["tblBANGLUONG"];

                // Setup hiển thị cột (Bạn cần map đúng tên cột trong DataGridView Designer)
                // Ví dụ:
                // dgvBangLuong.Columns["ThucLinh"].DefaultCellStyle.Format = "N0"; // Định dạng tiền tệ
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bảng lương: " + ex.Message);
            }
        }

        private void dtpKyLuong_ValueChanged(object sender, EventArgs e)
        {
            LoadBangLuong();
        }

        private void btnTinhLuong_Click(object sender, EventArgs e)
        {
            int maKyCong = int.Parse(dtpKyLuong.Value.ToString("yyyyMM"));

            // Kiểm tra xem tháng này đã tính lương chưa?
            if (ds.Tables["tblBANGLUONG"].Rows.Count > 0)
            {
                if (MessageBox.Show("Kỳ lương này đã được tính. Bạn có muốn tính lại không? (Dữ liệu cũ sẽ bị xóa)", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
                // Nếu đồng ý tính lại thì xóa dữ liệu cũ đi
                DeleteBangLuongThang(maKyCong);
            }

            // Gọi hàm tính toán (Sẽ viết ở phần tiếp theo)
            TinhToanLuong(maKyCong);

            // Tải lại lưới sau khi tính
            LoadBangLuong();
        }
        private void DeleteBangLuongThang(int maKyCong)
        {
            try
            {
                string sql = "DELETE FROM tb_BANGLUONG WHERE MAKYCONG = @MAKYCONG";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MAKYCONG", maKyCong);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi xóa kỳ lương cũ: " + ex.Message); }
        }

        // --- HÀM TÍNH TOÁN (PLACEHOLDER) ---
        private void TinhToanLuong(int maKyCong)
        {
            // Đây là nơi chúng ta sẽ viết logic phức tạp nhất
            MessageBox.Show("Đang phát triển tính năng tính lương cho kỳ: " + maKyCong);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int maKyCong = int.Parse(dtpKyLuong.Value.ToString("yyyyMM"));
            if (MessageBox.Show($"Bạn chắc chắn muốn xóa bảng lương tháng {dtpKyLuong.Value:MM/yyyy}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DeleteBangLuongThang(maKyCong);
                LoadBangLuong();
            }
        }
    }
}
