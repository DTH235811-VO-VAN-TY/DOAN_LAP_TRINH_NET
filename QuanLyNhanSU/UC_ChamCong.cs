using System;
using System.Data;
using System.Data.SqlClient; // Thư viện kết nối SQL
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class UC_ChamCong : UserControl
    {
        // 1. Khai báo biến kết nối
        SqlConnection conn;
        // Chuỗi kết nối chuẩn theo máy TUNG-IT của bạn
       // string connString = @"Data Source=TUNG-IT\MSSQL_EXP_2008R2;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        public event EventHandler AddNghiPhepClicked;
        public UC_ChamCong()
        {
            InitializeComponent();

            // 2. Tạo đồng hồ chạy giờ (Cập nhật mỗi giây)
            Timer t = new Timer();
            t.Interval = 1000;
            t.Tick += (s, e) => {
                if (lblGioHienTai != null)
                    lblGioHienTai.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            };
            t.Start();
        }

        // --- HÀM LOAD (KHỞI ĐỘNG) ---
        private void UC_ChamCong_Load(object sender, EventArgs e)
        {
            // Chặn lỗi khi đang ở chế độ thiết kế (Design Mode)
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

                // A. Tải danh sách nhân viên vào ComboBox
                SqlDataAdapter da = new SqlDataAdapter("SELECT MANV, HOTEN FROM tb_NHANVIEN", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboChonNhanVien.DataSource = dt;
                cboChonNhanVien.DisplayMember = "HOTEN";
                cboChonNhanVien.ValueMember = "MANV";
                cboChonNhanVien.SelectedIndex = -1;

                // B. Tải lịch sử chấm công hôm nay
                TaiLichSuHomNay();
            }
            catch (Exception ex)
            {
                // Chỉ hiện lỗi khi chạy thật
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv")
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private void TaiLichSuHomNay()
        {
            // Lấy dữ liệu NGÀY HÔM NAY
            string sql = @"SELECT bc.ID, nv.HOTEN, bc.ThoiGianVao, bc.ThoiGianRa 
                           FROM tb_BANGCONG bc
                           JOIN tb_NHANVIEN nv ON bc.MANV = nv.MANV
                           WHERE bc.NGAY = @Ngay 
                           ORDER BY bc.ThoiGianVao DESC";

            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);

            // Quan trọng: Truyền kiểu DateTime vào cột NGAY (kiểu DATE trong SQL)
            cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = DateTime.Now;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvLichSu.DataSource = dt;

            // Ẩn cột ID và Format giờ hiển thị
            if (dgvLichSu.Columns["ID"] != null) dgvLichSu.Columns["ID"].Visible = false;
            if (dgvLichSu.Columns["ThoiGianVao"] != null) dgvLichSu.Columns["ThoiGianVao"].DefaultCellStyle.Format = "HH:mm:ss";
            if (dgvLichSu.Columns["ThoiGianRa"] != null) dgvLichSu.Columns["ThoiGianRa"].DefaultCellStyle.Format = "HH:mm:ss";
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
            // Kiểm tra xem nhân viên này hôm nay đã chấm công chưa
            string sql = @"SELECT TOP 1 ThoiGianVao, ThoiGianRa FROM tb_BANGCONG 
                           WHERE MANV = @MaNV AND NGAY = @Ngay";

            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@MaNV", maNV);
            // Sửa lỗi type clash: Truyền DateTime vào
            cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = DateTime.Now;

            SqlDataReader dr = cmd.ExecuteReader();
            bool daVao = false;
            bool daRa = false;

            if (dr.Read())
            {
                if (dr["ThoiGianVao"] != DBNull.Value) daVao = true;
                if (dr["ThoiGianRa"] != DBNull.Value) daRa = true;
            }
            dr.Close();

            // Cập nhật trạng thái nút bấm và TextBox
            if (!daVao)
            {
                txtTrangThai.Text = "Chưa vào ca";
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
                txtTrangThai.Text = "Đã kết thúc ca";
                txtTrangThai.ForeColor = Color.Blue;
                btnVaoCa.Enabled = false;
                btnTanCa.Enabled = false;
            }
        }

        // --- NÚT VÀO CA (CHECK-IN) ---
        private void btnVaoCa_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1) { MessageBox.Show("Chưa chọn nhân viên!"); return; }

            string maNV = cboChonNhanVien.SelectedValue.ToString();
            DateTime now = DateTime.Now;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = @"INSERT INTO tb_BANGCONG (MANV, NAM, THANG, NGAY, ThoiGianVao) 
                               VALUES (@MaNV, @Nam, @Thang, @Ngay, @GioVao)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Nam", now.Year);
                cmd.Parameters.AddWithValue("@Thang", now.Month);
                // Truyền đúng kiểu Date
                cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = now;
                cmd.Parameters.AddWithValue("@GioVao", now);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Vào ca thành công lúc: " + now.ToString("HH:mm"));
                TaiLichSuHomNay();
                KiemTraTrangThai(maNV);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        // --- NÚT TAN CA (CHECK-OUT) ---
        private void btnTanCa_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1) return;
            string maNV = cboChonNhanVien.SelectedValue.ToString();
            DateTime now = DateTime.Now;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = @"UPDATE tb_BANGCONG 
                               SET ThoiGianRa = @GioRa 
                               WHERE MANV = @MaNV AND NGAY = @Ngay";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@GioRa", now);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                // Truyền đúng kiểu Date để tìm dòng cần Update
                cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = now;

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Tan ca thành công lúc: " + now.ToString("HH:mm"));
                    TaiLichSuHomNay();
                    KiemTraTrangThai(maNV);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu Vào Ca của hôm nay!");
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnThemChamCong_Click(object sender, EventArgs e)
        {
            frmChamCongThuCong frmChamCongThuCong = new frmChamCongThuCong();
            frmChamCongThuCong.ShowDialog();
        }

        private void btnNghiPhep_Click(object sender, EventArgs e)
        {
            AddNghiPhepClicked.Invoke(this, EventArgs.Empty);
        }
    }
}