using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class UC_ChamCong : UserControl
    {
        // 1. Khai báo kết nối
        SqlConnection conn;
        // Chuỗi kết nối chuẩn máy TUNG-IT
        string connString = @"Data Source=TUNG-IT\MSSQL_EXP_2008R2;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        public UC_ChamCong()
        {
            InitializeComponent();

            // 2. Tạo đồng hồ thời gian thực
            Timer t = new Timer();
            t.Interval = 1000;
            t.Tick += (s, e) => {
                if (lblGioHienTai != null)
                    lblGioHienTai.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            };
            t.Start();
        }

        private void UC_ChamCong_Load(object sender, EventArgs e)
        {
            // Chặn lỗi Designer
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            conn = new SqlConnection(connString);
            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // A. Tải Nhân Viên
                SqlDataAdapter da = new SqlDataAdapter("SELECT MANV, HOTEN FROM tb_NHANVIEN", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboChonNhanVien.DataSource = dt;
                cboChonNhanVien.DisplayMember = "HOTEN";
                cboChonNhanVien.ValueMember = "MANV";
                cboChonNhanVien.SelectedIndex = -1;

                // B. Thiết lập ngày mặc định là hôm nay
                if (dtpNgayCong != null) dtpNgayCong.Value = DateTime.Now;

                // C. Tải dữ liệu
                TaiLichSuTheoNgay();
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv")
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        // --- HÀM TẢI DỮ LIỆU THEO NGÀY TRÊN LỊCH ---
       private void TaiLichSuTheoNgay()
{
    DateTime ngayChon = (dtpNgayCong != null) ? dtpNgayCong.Value : DateTime.Now;

    // --- SỬA SQL: DÙNG WINDOW FUNCTION ĐỂ TÍNH TỔNG ---
    // COUNT(...) OVER(...) sẽ đếm tổng số dòng của nhân viên đó trong tháng
    string sql = @"SELECT 
                      nv.HOTEN, 
                      bc.ThoiGianVao, 
                      bc.ThoiGianRa,
                      COUNT(bc.ID) OVER(PARTITION BY bc.MANV) AS TongNgayCong -- Cột này sẽ hiện tổng
                   FROM tb_BANGCONG bc
                   JOIN tb_NHANVIEN nv ON bc.MANV = nv.MANV
                   WHERE bc.THANG = @Thang AND bc.NAM = @Nam
                   ORDER BY nv.HOTEN, bc.NGAY DESC";

    if (conn.State == ConnectionState.Closed) conn.Open();
    
    SqlCommand cmd = new SqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@Thang", ngayChon.Month);
    cmd.Parameters.AddWithValue("@Nam", ngayChon.Year);

    SqlDataAdapter da = new SqlDataAdapter(cmd);
    DataTable dt = new DataTable();
    da.Fill(dt);
    dgvLichSu.DataSource = dt;
    
    // --- CẤU HÌNH CỘT ---
    if (dgvLichSu.Columns["HOTEN"] != null) 
    {
        dgvLichSu.Columns["HOTEN"].HeaderText = "Họ Tên";
        dgvLichSu.Columns["HOTEN"].Width = 150;
    }

    if (dgvLichSu.Columns["ThoiGianVao"] != null) 
    {
        dgvLichSu.Columns["ThoiGianVao"].HeaderText = "Giờ Vào";
        dgvLichSu.Columns["ThoiGianVao"].DefaultCellStyle.Format = "dd/MM HH:mm"; // Hiện cả ngày cho dễ nhìn
        dgvLichSu.Columns["ThoiGianVao"].Width = 120;
    }

    if (dgvLichSu.Columns["ThoiGianRa"] != null) 
    {
        dgvLichSu.Columns["ThoiGianRa"].HeaderText = "Giờ Ra";
        dgvLichSu.Columns["ThoiGianRa"].DefaultCellStyle.Format = "dd/MM HH:mm";
        dgvLichSu.Columns["ThoiGianRa"].Width = 120;
    }

    if (dgvLichSu.Columns["TongNgayCong"] != null) 
    {
        dgvLichSu.Columns["TongNgayCong"].HeaderText = "Tổng Công Tháng";
        dgvLichSu.Columns["TongNgayCong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvLichSu.Columns["TongNgayCong"].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
        dgvLichSu.Columns["TongNgayCong"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }
}

        // --- SỰ KIỆN KHI ĐỔI NGÀY TRÊN LỊCH ---
        private void dtpNgayXem_ValueChanged(object sender, EventArgs e)
        {
            TaiLichSuTheoNgay();
            if (cboChonNhanVien.SelectedIndex != -1)
            {
                string maNV = cboChonNhanVien.SelectedValue.ToString();
                KiemTraTrangThai(maNV);
            }
        }

        // --- SỰ KIỆN CHỌN NHÂN VIÊN ---
        private void cboChonNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1) return;
            string maNV = cboChonNhanVien.SelectedValue.ToString();
            KiemTraTrangThai(maNV);
        }

        private void KiemTraTrangThai(string maNV)
        {
            DateTime ngayChon = (dtpNgayCong != null) ? dtpNgayCong.Value : DateTime.Now;

            string sql = @"SELECT TOP 1 ThoiGianVao, ThoiGianRa FROM tb_BANGCONG 
                           WHERE MANV = @MaNV AND NGAY = @Ngay";

            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaNV", maNV);
            cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = ngayChon;

            SqlDataReader dr = cmd.ExecuteReader();
            bool daVao = false;
            bool daRa = false;

            if (dr.Read())
            {
                if (dr["ThoiGianVao"] != DBNull.Value) daVao = true;
                if (dr["ThoiGianRa"] != DBNull.Value) daRa = true;
            }
            dr.Close();

            // Cập nhật giao diện
            if (!daVao)
            {
                txtTrangThai.Text = "Chưa chấm công";
                txtTrangThai.ForeColor = Color.Red;
                btnVaoCa.Enabled = true;
                btnTanCa.Enabled = false;
            }
            else if (daVao && !daRa)
            {
                txtTrangThai.Text = "Đang làm việc...";
                txtTrangThai.ForeColor = Color.Green;
                btnVaoCa.Enabled = false;
                btnTanCa.Enabled = true;
            }
            else
            {
                txtTrangThai.Text = "Đã xong công";
                txtTrangThai.ForeColor = Color.Blue;
                btnVaoCa.Enabled = false;
                btnTanCa.Enabled = false;
            }
        }

        // --- NÚT VÀO CA ---
        private void btnVaoCa_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1) { MessageBox.Show("Chưa chọn nhân viên!"); return; }

            string maNV = cboChonNhanVien.SelectedValue.ToString();
            DateTime ngayChon = dtpNgayCong.Value;
            // Lấy ngày trên lịch + giờ hiện tại
            DateTime gioLuu = new DateTime(ngayChon.Year, ngayChon.Month, ngayChon.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = @"INSERT INTO tb_BANGCONG (MANV, NAM, THANG, NGAY, ThoiGianVao) 
                               VALUES (@MaNV, @Nam, @Thang, @Ngay, @GioVao)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Nam", ngayChon.Year);
                cmd.Parameters.AddWithValue("@Thang", ngayChon.Month);
                cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = ngayChon;
                cmd.Parameters.AddWithValue("@GioVao", gioLuu);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Vào ca thành công!");
                TaiLichSuTheoNgay();
                KiemTraTrangThai(maNV);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        // --- NÚT TAN CA ---
        private void btnTanCa_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1) return;
            string maNV = cboChonNhanVien.SelectedValue.ToString();
            DateTime ngayChon = dtpNgayCong.Value;
            DateTime gioLuu = new DateTime(ngayChon.Year, ngayChon.Month, ngayChon.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = @"UPDATE tb_BANGCONG SET ThoiGianRa = @GioRa 
                               WHERE MANV = @MaNV AND NGAY = @Ngay";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@GioRa", gioLuu);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = ngayChon;

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Tan ca thành công!");
                    TaiLichSuTheoNgay();
                    KiemTraTrangThai(maNV);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu Vào Ca của ngày này!");
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        // --- NÚT MỞ FORM GHI CÔNG CHI TIẾT ---
        private void btnMoGhiCong_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1) { MessageBox.Show("Chọn nhân viên trước!"); return; }

            string maNV = cboChonNhanVien.SelectedValue.ToString();
            string tenNV = cboChonNhanVien.Text;
            int thang = dtpNgayCong.Value.Month;
            int nam =   dtpNgayCong.Value.Year;

            Form_GhiCong f = new Form_GhiCong(maNV, tenNV, thang, nam);
            f.ShowDialog();

            TaiLichSuTheoNgay();
            KiemTraTrangThai(maNV);
        }
    }
}