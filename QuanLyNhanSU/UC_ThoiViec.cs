using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class UC_ThoiViec : UserControl
    {
        // 1. Cấu hình chuỗi kết nối
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        SqlConnection conn;
        SqlDataAdapter da;

        // SỬA ĐỔI: Dùng DataSet thay vì DataTable rời rạc
        DataSet ds = new DataSet();

        public event EventHandler DataUpdated;

        public UC_ThoiViec()
        {
            InitializeComponent();
        }

        // --- SỰ KIỆN LOAD FORM ---
        private void UC_ThoiViec_Load(object sender, EventArgs e)
        {
            // --- PHÂN QUYỀN (Giữ nguyên) ---
            if (Const.LoaiTaiKhoan == 2)
            {
                btnThem.Enabled = false; btnSua.Enabled = false; btnXoa.Enabled = false;
                btnLuu.Enabled = false; btnLamMoi.Enabled = false;
                foreach (Control c in this.Controls) { if (c is TextBox) ((TextBox)c).ReadOnly = true; }
                dgvThoiViec.ReadOnly = true;
            }

            if (this.DesignMode) return;

            conn = new SqlConnection(connString);

            // Cấu hình hiển thị ngày tháng
            dtpNgayNop.Format = DateTimePickerFormat.Short;
            dtpNgayNghi.Format = DateTimePickerFormat.Short;

            // Load ComboBox tìm kiếm (Thêm mới để tránh lỗi nếu chưa có dữ liệu)
            LoadComboBoxTimKiem();

            // Load dữ liệu
            LoadComboBoxNhanVien();
            LoadDataGrid();
            ResetInput();
        }

        // Tạo dữ liệu cho ComboBox Tìm kiếm (Tránh lỗi null)
        private void LoadComboBoxTimKiem()
        {
            // Tạo danh sách tùy chọn tìm kiếm thủ công
            var items = new[] {
                new { Text = "Tìm theo Mã NV", Value = "MANV" },
                new { Text = "Tìm theo Tên NV", Value = "HOTEN" }
            };

            // Giả sử tên ComboBox tìm kiếm của bạn là cboTimKiemNV
            // Nếu tên khác, hãy sửa lại dòng dưới
            cboTimKiemNV.DataSource = items;
            cboTimKiemNV.DisplayMember = "Text";
            cboTimKiemNV.ValueMember = "Value";
            cboTimKiemNV.SelectedIndex = 0;
        }

        // HÀM 1: Load danh sách nhân viên vào ComboBox
        public void LoadComboBoxNhanVien()
        {
            try
            {
                // Dùng DataAdapter riêng cho ComboBox để không ảnh hưởng dữ liệu chính
                string sql = "SELECT MANV, HOTEN FROM tb_NHANVIEN WHERE DATHOIVIEC = 0 OR DATHOIVIEC IS NULL";
                SqlDataAdapter daNV = new SqlDataAdapter(sql, conn);

                // Đổ vào một bảng riêng trong DataSet tên là "tblNHANVIEN_COMBO"
                if (ds.Tables.Contains("tblNHANVIEN_COMBO")) ds.Tables["tblNHANVIEN_COMBO"].Clear();
                daNV.Fill(ds, "tblNHANVIEN_COMBO");

                cboMaNV.DataSource = ds.Tables["tblNHANVIEN_COMBO"];
                cboMaNV.DisplayMember = "MANV";
                cboMaNV.ValueMember = "MANV";
                cboMaNV.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load nhân viên: " + ex.Message); }
        }

        // HÀM 2: Load danh sách Thôi Việc lên lưới (DÙNG DATASET)
        private void LoadDataGrid()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = @"SELECT tv.SOQD, tv.MANV, nv.HOTEN, tv.NGAYNOPDON, tv.NGAYNGHI, tv.LYDO 
                               FROM tb_THOIVIEC tv
                               JOIN tb_NHANVIEN nv ON tv.MANV = nv.MANV";

                da = new SqlDataAdapter(sql, conn);

                // --- SỬA ĐỔI QUAN TRỌNG: Dùng DataSet ---
                // Xóa dữ liệu cũ nếu có để tránh nạp chồng
                if (ds.Tables.Contains("tblTHOIVIEC")) ds.Tables["tblTHOIVIEC"].Clear();

                // Đổ dữ liệu vào bảng đặt tên là "tblTHOIVIEC"
                da.Fill(ds, "tblTHOIVIEC");

                // Gán nguồn dữ liệu cho GridView
                dgvThoiViec.DataSource = ds.Tables["tblTHOIVIEC"];

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

        // --- HÀM TÌM KIẾM (ĐÃ SỬA CHUẨN) ---
        private void btnTimNV_Click(object sender, EventArgs e)
        {
            // 1. Lấy từ khóa người dùng nhập
            string key = txtTimKiemNV.Text.Trim();

            // Xử lý ký tự đặc biệt để tránh lỗi SQL (dấu nháy đơn)
            key = key.Replace("'", "''");

            // 2. Nếu ô nhập trống thì hiện tất cả
            if (string.IsNullOrEmpty(key))
            {
                ds.Tables["tblTHOIVIEC"].DefaultView.RowFilter = string.Empty;
                return;
            }

            // 3. Xác định cột cần tìm (Dựa vào ValueMember đã set ở hàm LoadComboBoxTimKiem)
            // Nếu bạn chưa set ValueMember, hãy dùng: string colName = cboTimKiemNV.SelectedIndex == 0 ? "MANV" : "HOTEN";
            string colName = cboTimKiemNV.SelectedValue.ToString();

            // 4. Lọc dữ liệu trên DataSet (Không cần query lại SQL)
            try
            {
                // Cú pháp: TenCot LIKE '%TuKhoa%'
                ds.Tables["tblTHOIVIEC"].DefaultView.RowFilter = $"{colName} LIKE '%{key}%'";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        // --- HÀM HIỆN TẤT CẢ ---
        private void btnHienAll_Click(object sender, EventArgs e)
        {
            txtTimKiemNV.Text = "";
            // Xóa bộ lọc để hiện lại toàn bộ
            if (ds.Tables.Contains("tblTHOIVIEC"))
            {
                ds.Tables["tblTHOIVIEC"].DefaultView.RowFilter = string.Empty;
            }
        }

        // Sự kiện khi chọn Mã NV -> Tự điền Tên NV
        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedValue != null && cboMaNV.SelectedIndex != -1)
            {
                DataRowView drv = cboMaNV.SelectedItem as DataRowView;
                if (drv != null) txtTenNV.Text = drv["HOTEN"].ToString();
            }
            else txtTenNV.Clear();
        }

        // --- NÚT LƯU ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtLyDo.Text.Trim()))
            {
                MessageBox.Show("Chưa nhập lý do thôi việc!"); return;
            }
            if (cboMaNV.SelectedIndex == -1) { MessageBox.Show("Chưa chọn nhân viên!"); return; }

            string soQD = txtSoQD.Text.Trim();
            if (string.IsNullOrEmpty(soQD)) soQD = "QDTV" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string maNV = cboMaNV.SelectedValue.ToString();

            if (MessageBox.Show($"Cho nhân viên {maNV} thôi việc?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = transaction;

                // Insert
                cmd.CommandText = @"INSERT INTO tb_THOIVIEC (SOQD, MANV, NGAYNOPDON, NGAYNGHI, LYDO) 
                                    VALUES (@SOQD, @MANV, @NGAYNOP, @NGAYNGHI, @LYDO)";
                cmd.Parameters.AddWithValue("@SOQD", soQD);
                cmd.Parameters.AddWithValue("@MANV", maNV);
                cmd.Parameters.AddWithValue("@NGAYNOP", dtpNgayNop.Value);
                cmd.Parameters.AddWithValue("@NGAYNGHI", dtpNgayNghi.Value);
                cmd.Parameters.AddWithValue("@LYDO", txtLyDo.Text);
                cmd.ExecuteNonQuery();

                // Update trạng thái NV
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tb_NHANVIEN SET DATHOIVIEC = 1 WHERE MANV = @MANV";
                cmd.Parameters.AddWithValue("@MANV", maNV);
                cmd.ExecuteNonQuery();

                transaction.Commit();
                DataUpdated?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Thành công!");

                LoadDataGrid(); // Load lại Grid (DataSet sẽ được cập nhật)
                LoadComboBoxNhanVien();
                ResetInput();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        // --- NÚT XÓA ---
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvThoiViec.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }

            string soQD = dgvThoiViec.SelectedRows[0].Cells["SOQD"].Value.ToString();
            string maNV = dgvThoiViec.SelectedRows[0].Cells["MANV"].Value.ToString();

            if (MessageBox.Show("Hủy quyết định này sẽ khôi phục nhân viên. Tiếp tục?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = transaction;

                // Xóa
                cmd.CommandText = "DELETE FROM tb_THOIVIEC WHERE SOQD = @SOQD";
                cmd.Parameters.AddWithValue("@SOQD", soQD);
                cmd.ExecuteNonQuery();

                // Khôi phục NV
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tb_NHANVIEN SET DATHOIVIEC = 0 WHERE MANV = @MANV";
                cmd.Parameters.AddWithValue("@MANV", maNV);
                cmd.ExecuteNonQuery();

                transaction.Commit();
                DataUpdated?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Đã xóa và khôi phục nhân viên.");

                LoadDataGrid();
                LoadComboBoxNhanVien();
                ResetInput();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void dgvThoiViec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvThoiViec.Rows[e.RowIndex];
                txtSoQD.Text = row.Cells["SOQD"].Value.ToString();
                cboMaNV.Text = row.Cells["MANV"].Value.ToString();
                txtTenNV.Text = row.Cells["HOTEN"].Value.ToString();
                if (row.Cells["NGAYNOPDON"].Value != DBNull.Value) dtpNgayNop.Value = Convert.ToDateTime(row.Cells["NGAYNOPDON"].Value);
                if (row.Cells["NGAYNGHI"].Value != DBNull.Value) dtpNgayNghi.Value = Convert.ToDateTime(row.Cells["NGAYNGHI"].Value);
                txtLyDo.Text = row.Cells["LYDO"].Value.ToString();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e) { ResetInput(); LoadDataGrid(); }
        private void btnThem_Click(object sender, EventArgs e) { ResetInput(); cboMaNV.Focus(); }
        private void btnSua_Click(object sender, EventArgs e) { MessageBox.Show("Vui lòng Xóa và tạo mới nếu cần thay đổi lớn."); }

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