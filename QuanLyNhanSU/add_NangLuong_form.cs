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
    public partial class add_NangLuong_form : Form
    {
        // --- 1. CẤU HÌNH KẾT NỐI ---
        // Lưu ý: Thay đổi chuỗi kết nối phù hợp với máy của bạn
        string strKetNoi = @"Data Source=LAPTOP-S5P1Q2HR\SQLEXPRESS;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        SqlConnection conn = null;

        // --- 2. BIẾN TRẠNG THÁI ---
        bool isThem = false; // True = Đang thêm, False = Đang sửa
        int idHienTai = -1;  // Lưu ID dòng đang chọn trên lưới
        public add_NangLuong_form()
        {
            InitializeComponent();
            conn = new SqlConnection(strKetNoi);
        }

        private void add_NangLuong_form_Load(object sender, EventArgs e)
        {
            LoadData();
            LockControl(true); // Khóa các ô nhập liệu, chỉ cho xem
        }
        // --- HÀM DÙNG CHUNG ---

        // Tải dữ liệu lên Grid
        private void LoadData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = "SELECT * FROM tb_NANGLUONG ORDER BY NGAYLENLUONG DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvNangLuong.DataSource = dt;

                // Format cột hiển thị (Tùy chọn)
                dgvNangLuong.Columns["MANV"].HeaderText = "Mã NV";
                dgvNangLuong.Columns["HOTEN"].HeaderText = "Họ Tên";
                // Các cột khác...
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
            finally { conn.Close(); }
        }
        private void LockControl(bool lockState)
        {
            // Nếu lockState = true (Khóa) -> Enabled = false
            txtSoHopDong.Enabled = !lockState;
            txtMaNV.Enabled = !lockState;
            txtTenNV.Enabled = !lockState;
            txtGhichu.Enabled = !lockState;

            cboHopDong.Enabled = !lockState;
            dtpNgayKy.Enabled = !lockState;
            dtpNgayLenLuong.Enabled = !lockState;
            nudHeSLC.Enabled = !lockState;
            nudHSLMoi.Enabled = !lockState;

            // Xử lý các nút
            btnThem.Enabled = lockState;  // Khi khóa nhập -> Cho phép bấm Thêm
            btnSua.Enabled = lockState;   // Khi khóa nhập -> Cho phép bấm Sửa
            btnXoa.Enabled = lockState;   // Khi khóa nhập -> Cho phép bấm Xóa
            btnLuu.Enabled = !lockState;  // Chỉ sáng lên khi đang nhập liệu
            btnLamMoi.Enabled = true;
        }

        // Hàm xóa trắng form
        private void ResetInput()
        {
            txtSoHopDong.Clear();
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtGhichu.Clear();
            cboHopDong.SelectedIndex = -1;
            nudHeSLC.Value = 0;
            nudHSLMoi.Value = 0;
            dtpNgayKy.Value = DateTime.Now;
            dtpNgayLenLuong.Value = DateTime.Now;
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNangLuong.Rows[e.RowIndex];
                try
                {
                    // Lưu ID để xử lý Sửa/Xóa
                    idHienTai = Convert.ToInt32(row.Cells["ID"].Value);

                    // Đổ dữ liệu ngược lại các ô text
                    txtSoHopDong.Text = row.Cells["SOHOPDONG"].Value?.ToString();
                    txtMaNV.Text = row.Cells["MANV"].Value?.ToString();
                    txtTenNV.Text = row.Cells["HOTEN"].Value?.ToString();
                    txtGhichu.Text = row.Cells["GHICHU"].Value?.ToString();

                    // Xử lý ngày tháng
                    if (row.Cells["NGAYKY"].Value != DBNull.Value)
                        dtpNgayKy.Value = Convert.ToDateTime(row.Cells["NGAYKY"].Value);

                    if (row.Cells["NGAYLENLUONG"].Value != DBNull.Value)
                        dtpNgayLenLuong.Value = Convert.ToDateTime(row.Cells["NGAYLENLUONG"].Value);

                    // Xử lý số
                    if (row.Cells["HESOLUONGHIENTAI"].Value != DBNull.Value)
                        nudHeSLC.Value = Convert.ToDecimal(row.Cells["HESOLUONGHIENTAI"].Value);

                    if (row.Cells["HESOLUONGMOI"].Value != DBNull.Value)
                        nudHSLMoi.Value = Convert.ToDecimal(row.Cells["HESOLUONGMOI"].Value);
                }
                catch (Exception) { }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            isThem = true;       // Bật cờ Thêm
            ResetInput();        // Xóa trắng form
            LockControl(false);  // Mở khóa ô nhập
            txtSoHopDong.Focus(); // Đặt con trỏ chuột vào ô đầu tiên
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (idHienTai == -1)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa trên lưới!", "Thông báo");
                return;
            }
            isThem = false;      // Bật cờ Sửa (Update)
            LockControl(false);  // Mở khóa ô nhập

            // Có thể chặn không cho sửa Mã NV nếu muốn
            // txtMaNV.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (idHienTai == -1)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tb_NANGLUONG WHERE ID = @ID", conn);
                    cmd.Parameters.AddWithValue("@ID", idHienTai);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    ResetInput();
                    idHienTai = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
                finally { conn.Close(); }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // -- Validate dữ liệu --
            if (string.IsNullOrWhiteSpace(txtSoHopDong.Text) ||
                string.IsNullOrWhiteSpace(txtMaNV.Text) ||
                string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ Số HĐ, Mã NV và Tên NV!", "Cảnh báo");
                return;
            }

            // Kiểm tra Mã NV phải là số
            int maNV;
            if (!int.TryParse(txtMaNV.Text, out maNV))
            {
                MessageBox.Show("Mã Nhân Viên phải là số!", "Lỗi định dạng");
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (isThem) // --- TRƯỜNG HỢP THÊM ---
                {
                    cmd.CommandText = @"INSERT INTO tb_NANGLUONG (SOHOPDONG, MANV, HOTEN, NGAYKY, NGAYLENLUONG, HESOLUONGHIENTAI, HESOLUONGMOI, GHICHU) 
                                        VALUES (@SoHD, @MaNV, @HoTen, @NgayKy, @NgayLen, @HSL_Cu, @HSL_Moi, @GhiChu)";
                }
                else // --- TRƯỜNG HỢP SỬA ---
                {
                    cmd.CommandText = @"UPDATE tb_NANGLUONG 
                                        SET SOHOPDONG=@SoHD, MANV=@MaNV, HOTEN=@HoTen, NGAYKY=@NgayKy, 
                                            NGAYLENLUONG=@NgayLen, HESOLUONGHIENTAI=@HSL_Cu, 
                                            HESOLUONGMOI=@HSL_Moi, GHICHU=@GhiChu 
                                        WHERE ID=@ID";
                    cmd.Parameters.AddWithValue("@ID", idHienTai);
                }

                // Truyền tham số
                cmd.Parameters.AddWithValue("@SoHD", txtSoHopDong.Text);
                cmd.Parameters.AddWithValue("@MaNV", maNV); // Dùng biến int đã parse
                cmd.Parameters.AddWithValue("@HoTen", txtTenNV.Text);
                cmd.Parameters.AddWithValue("@NgayKy", dtpNgayKy.Value);
                cmd.Parameters.AddWithValue("@NgayLen", dtpNgayLenLuong.Value);

                // NumericUpDown trả về Decimal -> Cần ép về float/double cho SQL
                cmd.Parameters.AddWithValue("@HSL_Cu", Convert.ToDouble(nudHeSLC.Value));
                cmd.Parameters.AddWithValue("@HSL_Moi", Convert.ToDouble(nudHSLMoi.Value));

                cmd.Parameters.AddWithValue("@GhiChu", txtGhichu.Text);

                // Thực thi
                cmd.ExecuteNonQuery();

                MessageBox.Show(isThem ? "Thêm thành công!" : "Cập nhật thành công!");
                LoadData();      // Tải lại lưới
                LockControl(true); // Khóa form lại
                idHienTai = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
            LockControl(true);
            idHienTai = -1;
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
