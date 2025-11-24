using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class UC_ThoiViec : UserControl
    {
        // 1. Cấu hình chuỗi kết nối (Kiểm tra lại tên Server của bạn nếu cần)
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        SqlConnection conn;
        SqlDataAdapter da;
        DataTable dt;
        public event EventHandler DataUpdated;

        public UC_ThoiViec()
        {
            InitializeComponent();
        }

        // --- SỰ KIỆN LOAD FORM ---
        private void UC_ThoiViec_Load(object sender, EventArgs e)
        {
            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng thêm/sửa/xóa
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnLamMoi.Enabled = false;
             //   btnInHD.Enabled = false;
                // (Nếu có nút Hủy hay Làm mới thì tắt nốt nếu muốn)

                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox) ((TextBox)c).ReadOnly = true;
                }

                // 3. GridView chỉ cho xem
                dgvThoiViec.ReadOnly = true;
            }
            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng thêm/sửa/xóa
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnLamMoi.Enabled = false;
                // (Nếu có nút Hủy hay Làm mới thì tắt nốt nếu muốn)

                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox) ((TextBox)c).ReadOnly = true;
                }

                // 3. GridView chỉ cho xem
                dgvThoiViec.ReadOnly = true;
            }
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }// Tránh lỗi giao diện khi thiết kế

            conn = new SqlConnection(connString);

            // Cấu hình hiển thị ngày tháng
            dtpNgayNop.Format = DateTimePickerFormat.Short;
            dtpNgayNghi.Format = DateTimePickerFormat.Short;

            // Load dữ liệu
            LoadComboBoxNhanVien();
            LoadDataGrid();
            ResetInput();
        }

        // HÀM 1: Load danh sách nhân viên ĐANG LÀM VIỆC vào ComboBox
        private void LoadComboBoxNhanVien()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Chỉ lấy nhân viên chưa nghỉ (DATHOIVIEC = 0 hoặc NULL)
                string sql = "SELECT MANV, HOTEN FROM tb_NHANVIEN WHERE DATHOIVIEC = 0 OR DATHOIVIEC IS NULL";

                SqlDataAdapter daNV = new SqlDataAdapter(sql, conn);
                DataTable dtNV = new DataTable();
                daNV.Fill(dtNV);

                cboMaNV.DataSource = dtNV;
                cboMaNV.DisplayMember = "MANV";
                cboMaNV.ValueMember = "MANV";
                cboMaNV.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load nhân viên: " + ex.Message); }
        }

        // HÀM 2: Load danh sách Thôi Việc lên lưới
        private void LoadDataGrid()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = @"SELECT tv.SOQD, tv.MANV, nv.HOTEN, tv.NGAYNOPDON, tv.NGAYNGHI, tv.LYDO 
                               FROM tb_THOIVIEC tv
                               JOIN tb_NHANVIEN nv ON tv.MANV = nv.MANV";

                da = new SqlDataAdapter(sql, conn);
                dt = new DataTable();
                da.Fill(dt);

                dgvThoiViec.DataSource = dt;

                // Đặt tên cột hiển thị
                dgvThoiViec.Columns["SOQD"].HeaderText = "Số Quyết Định";
                dgvThoiViec.Columns["MANV"].HeaderText = "Mã NV";
                dgvThoiViec.Columns["HOTEN"].HeaderText = "Họ Tên";
                dgvThoiViec.Columns["NGAYNOPDON"].HeaderText = "Ngày Nộp Đơn";
                dgvThoiViec.Columns["NGAYNGHI"].HeaderText = "Ngày Nghỉ";
                dgvThoiViec.Columns["LYDO"].HeaderText = "Lý Do";

                dgvThoiViec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        // Sự kiện khi chọn Mã NV -> Tự điền Tên NV
        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedValue != null && cboMaNV.SelectedIndex != -1)
            {
                DataRowView drv = cboMaNV.SelectedItem as DataRowView;
                if (drv != null) txtTenNV.Text = drv["HOTEN"].ToString();
            }
            else
            {
                txtTenNV.Clear();
            }
        }

        // --- NÚT LƯU: XỬ LÝ THÔI VIỆC ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Validate
            if (cboMaNV.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tự sinh Số Quyết Định nếu để trống
            string soQD = txtSoQD.Text.Trim();
            if (string.IsNullOrEmpty(soQD))
            {
                soQD = "QDTV" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            string maNV = cboMaNV.SelectedValue.ToString();

            // Xác nhận
            if (MessageBox.Show($"Bạn chắc chắn muốn cho nhân viên {maNV} thôi việc?\nNhân viên này sẽ chuyển sang trạng thái ĐÃ NGHỈ.",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            // 2. Thực hiện Transaction (Lưu + Update trạng thái)
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = transaction;

                // Bước A: Insert vào bảng THÔI VIỆC
                cmd.CommandText = @"INSERT INTO tb_THOIVIEC (SOQD, MANV, NGAYNOPDON, NGAYNGHI, LYDO) 
                                    VALUES (@SOQD, @MANV, @NGAYNOP, @NGAYNGHI, @LYDO)";

                cmd.Parameters.AddWithValue("@SOQD", soQD);
                cmd.Parameters.AddWithValue("@MANV", maNV);
                cmd.Parameters.AddWithValue("@NGAYNOP", dtpNgayNop.Value);
                cmd.Parameters.AddWithValue("@NGAYNGHI", dtpNgayNghi.Value);
                cmd.Parameters.AddWithValue("@LYDO", txtLyDo.Text);

                cmd.ExecuteNonQuery();

                // Bước B: Cập nhật trạng thái nhân viên (DATHOIVIEC = 1)
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tb_NHANVIEN SET DATHOIVIEC = 1 WHERE MANV = @MANV";
                cmd.Parameters.AddWithValue("@MANV", maNV);

                cmd.ExecuteNonQuery();

                // Hoàn tất
                transaction.Commit();
                DataUpdated?.Invoke(this, EventArgs.Empty); // Thông báo dữ liệu đã được cập nhật
                MessageBox.Show("Lưu quyết định thôi việc thành công!", "Thông báo");

                // Reset giao diện
                LoadDataGrid();
                LoadComboBoxNhanVien(); // Load lại để ẩn nhân viên vừa nghỉ
                ResetInput();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Lỗi xử lý: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        // --- NÚT XÓA: HỦY QUYẾT ĐỊNH (KHÔI PHỤC NHÂN VIÊN) ---
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvThoiViec.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn quyết định cần xóa trên lưới!");
                return;
            }

            string soQD = dgvThoiViec.SelectedRows[0].Cells["SOQD"].Value.ToString();
            string maNV = dgvThoiViec.SelectedRows[0].Cells["MANV"].Value.ToString();

            if (MessageBox.Show("Hủy quyết định thôi việc sẽ khôi phục nhân viên về trạng thái ĐANG LÀM VIỆC.\nBạn có chắc chắn không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = transaction;

                // Bước A: Xóa khỏi bảng THÔI VIỆC
                cmd.CommandText = "DELETE FROM tb_THOIVIEC WHERE SOQD = @SOQD";
                cmd.Parameters.AddWithValue("@SOQD", soQD);
                cmd.ExecuteNonQuery();

                // Bước B: Khôi phục trạng thái nhân viên (DATHOIVIEC = 0)
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tb_NHANVIEN SET DATHOIVIEC = 0 WHERE MANV = @MANV";
                cmd.Parameters.AddWithValue("@MANV", maNV);
                cmd.ExecuteNonQuery();

                transaction.Commit();
                DataUpdated?.Invoke(this, EventArgs.Empty); // Thông báo dữ liệu đã được cập nhật
                MessageBox.Show("Đã xóa quyết định. Nhân viên đã được khôi phục.");

                LoadDataGrid();
                LoadComboBoxNhanVien(); // Load lại để hiện nhân viên đó lên
                ResetInput();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        // --- SỰ KIỆN CLICK VÀO LƯỚI ---
        private void dgvThoiViec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvThoiViec.Rows[e.RowIndex];

                txtSoQD.Text = row.Cells["SOQD"].Value.ToString();

                // Lưu ý: Vì nhân viên đã nghỉ nên có thể không hiển thị được trong ComboBox (do ta lọc)
                // Ta gán thủ công Text để người dùng thấy
                cboMaNV.Text = row.Cells["MANV"].Value.ToString();
                txtTenNV.Text = row.Cells["HOTEN"].Value.ToString();

                if (row.Cells["NGAYNOPDON"].Value != DBNull.Value)
                    dtpNgayNop.Value = Convert.ToDateTime(row.Cells["NGAYNOPDON"].Value);

                if (row.Cells["NGAYNGHI"].Value != DBNull.Value)
                    dtpNgayNghi.Value = Convert.ToDateTime(row.Cells["NGAYNGHI"].Value);

                txtLyDo.Text = row.Cells["LYDO"].Value.ToString();
            }
        }

        // --- CÁC NÚT PHỤ ---
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
            LoadDataGrid();
            LoadComboBoxNhanVien();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetInput();
            cboMaNV.Focus();
        }

        // Nút Sửa (Thường thì chỉ cần xóa đi làm lại, nhưng nếu muốn sửa Lý do/Ngày nghỉ thì dùng code này)
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Logic sửa tương tự Lưu, nhưng dùng câu lệnh UPDATE tb_THOIVIEC
            // Bạn có thể bổ sung nếu cần thiết.
            MessageBox.Show("Để đảm bảo toàn vẹn dữ liệu, vui lòng Xóa quyết định cũ và tạo mới nếu cần thay đổi lớn.");
        }

        private void ResetInput()
        {
            txtSoQD.Clear();
            cboMaNV.SelectedIndex = -1;
            txtTenNV.Clear();
            txtLyDo.Clear();
            dtpNgayNop.Value = DateTime.Now;
            dtpNgayNghi.Value = DateTime.Now;
        }
    }
}