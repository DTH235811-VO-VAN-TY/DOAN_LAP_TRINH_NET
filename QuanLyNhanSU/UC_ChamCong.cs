using System;
using System.Data;
using System.Data.SqlClient; // Thư viện kết nối SQL
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class UC_ChamCong : UserControl
    {
        // 1. CHUỖI KẾT NỐI (Đã giữ lại chuỗi kết nối chuẩn theo máy REDMI-11 của bạn)
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

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

            LoadDataComboBox();
            PhanQuyenGiaoDien();

            // Nếu là NHÂN VIÊN -> Tự động tải dữ liệu của họ luôn
            if (Const.LoaiTaiKhoan == 2 && cboChonNhanVien.SelectedValue != null)
            {
                string maNV = cboChonNhanVien.SelectedValue.ToString();
                TaiLichSuHomNay();
                KiemTraTrangThai(maNV);
                TinhTongNgayCong(maNV); // Tính năng đếm ngày công
            }
        }

        // --- 1. PHÂN QUYỀN (Logic mới) ---
        private void PhanQuyenGiaoDien()
        {
            // Giả sử Const.LoaiTaiKhoan = 2 là Nhân Viên, 1 là Admin
            if (Const.LoaiTaiKhoan == 2) // NHÂN VIÊN
            {
                // Tự động chọn đúng mã nhân viên đang đăng nhập
                cboChonNhanVien.SelectedValue = Const.MaNV;

                // Khóa không cho chọn nhân viên khác (hoặc để Enable nhưng chặn ở sự kiện Click)
                // Ở đây ta vẫn để Enable=true để họ thấy tên mình, nhưng chặn logic lúc bấm nút
                cboChonNhanVien.Enabled = false;

                // Ẩn nút "Thêm chấm công" (Chức năng sửa công của Admin)
                btnThemChamCong.Visible = false;
            }
            else // ADMIN
            {
                cboChonNhanVien.Enabled = true;
                btnThemChamCong.Visible = true;
            }
        }

        // --- 2. CÁC HÀM TẢI DỮ LIỆU ---
        public void LoadDataComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MANV, HOTEN FROM tb_NHANVIEN", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboChonNhanVien.DataSource = dt;
                    cboChonNhanVien.DisplayMember = "HOTEN";
                    cboChonNhanVien.ValueMember = "MANV";
                    cboChonNhanVien.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    // Chỉ hiện lỗi khi chạy thật
                    if (System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv")
                        MessageBox.Show("Lỗi tải nhân viên: " + ex.Message);
                }
            }
        }

        private void TaiLichSuHomNay()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Lấy dữ liệu NGÀY HÔM NAY
                    string sql = @"SELECT bc.ID, nv.HOTEN, bc.ThoiGianVao, bc.ThoiGianRa 
                                   FROM tb_BANGCONG bc
                                   JOIN tb_NHANVIEN nv ON bc.MANV = nv.MANV
                                   WHERE bc.NGAY = @Ngay 
                                   ORDER BY bc.ThoiGianVao DESC";

                    SqlCommand cmd = new SqlCommand(sql, conn);
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
                catch { }
            }
        }

        // --- 3. TÍNH TỔNG NGÀY CÔNG (TÍNH NĂNG MỚI) ---
        private void TinhTongNgayCong(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT COUNT(DISTINCT NGAY) 
                                   FROM tb_BANGCONG 
                                   WHERE MANV = @MaNV AND THANG = @Thang AND NAM = @Nam";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@Thang", DateTime.Now.Month);
                    cmd.Parameters.AddWithValue("@Nam", DateTime.Now.Year);

                    int tongCong = 0;
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        tongCong = Convert.ToInt32(result);

                    if (lblNgayCong != null)
                    {
                        lblNgayCong.Text = "Tổng công tháng " + DateTime.Now.Month + ": " + tongCong + " ngày";
                        lblNgayCong.ForeColor = Color.Blue;
                    }
                }
                catch { }
            }
        }

        // --- 4. SỰ KIỆN CHỌN NHÂN VIÊN ---
        private void cboChonNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1 || cboChonNhanVien.SelectedValue == null) return;

            string maNV = cboChonNhanVien.SelectedValue.ToString();

            KiemTraTrangThai(maNV);
            TinhTongNgayCong(maNV);
        }

        // --- 5. LOGIC KIỂM TRA TRẠNG THÁI ---
        private void KiemTraTrangThai(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sql = @"SELECT TOP 1 ThoiGianVao, ThoiGianRa FROM tb_BANGCONG 
                               WHERE MANV = @MaNV AND NGAY = @Ngay
                               ORDER BY ThoiGianVao DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
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
                    txtTrangThai.Text = "Đã hoàn thành ca";
                    txtTrangThai.ForeColor = Color.Blue;
                    btnVaoCa.Enabled = false;
                    btnTanCa.Enabled = false;
                }
            }
        }

        // --- 6. NÚT VÀO CA (CHECK-IN) ---
        private void btnVaoCa_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1 || cboChonNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Chưa chọn nhân viên!");
                return;
            }

            // BẢO MẬT: Nếu là Nhân viên -> Bắt buộc phải là chính mình
            if (Const.LoaiTaiKhoan == 2 && cboChonNhanVien.SelectedValue.ToString() != Const.MaNV)
            {
                MessageBox.Show("Bạn chỉ được phép chấm công cho chính mình!");
                cboChonNhanVien.SelectedValue = Const.MaNV;
                return;
            }

            string maNV = cboChonNhanVien.SelectedValue.ToString();
            DateTime now = DateTime.Now;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Kiểm tra xem hôm nay đã bấm nút này chưa
                    SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM tb_BANGCONG WHERE MANV=@mnv AND NGAY=@ngay", conn);
                    cmdCheck.Parameters.AddWithValue("@mnv", maNV);
                    cmdCheck.Parameters.Add("@ngay", SqlDbType.Date).Value = now;

                    if ((int)cmdCheck.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Hôm nay bạn đã bấm Vào Ca rồi!");
                        return;
                    }

                    // SQL INSERT (Có thêm phần tự lấy IDLC để tránh lỗi)
                    string sql = @"INSERT INTO tb_BANGCONG (MANV, NAM, THANG, NGAY, ThoiGianVao, IDLC) 
                                   VALUES (@MaNV, @Nam, @Thang, @Ngay, @GioVao, (SELECT TOP 1 IDLC FROM tb_LOAICONG))";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@Nam", now.Year);
                    cmd.Parameters.AddWithValue("@Thang", now.Month);
                    cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = now;
                    cmd.Parameters.AddWithValue("@GioVao", now);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Vào ca thành công lúc: " + now.ToString("HH:mm"));

                    TaiLichSuHomNay();
                    KiemTraTrangThai(maNV);
                    TinhTongNgayCong(maNV);
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        // --- 7. NÚT TAN CA (CHECK-OUT) ---
        private void btnTanCa_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1 || cboChonNhanVien.SelectedValue == null) return;

            string maNV = cboChonNhanVien.SelectedValue.ToString();
            DateTime now = DateTime.Now;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string sql = @"UPDATE tb_BANGCONG 
                                   SET ThoiGianRa = @GioRa 
                                   WHERE MANV = @MaNV AND NGAY = @Ngay";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@GioRa", now);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
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
        }

        // --- 8. CÁC NÚT PHỤ ---
        private void btnThemChamCong_Click(object sender, EventArgs e)
        {
            // Chỉ dành cho Admin (Đã ẩn nút này với nhân viên ở hàm PhanQuyenGiaoDien)
            frmChamCongThuCong frm = new frmChamCongThuCong();
            frm.ShowDialog();
            TaiLichSuHomNay();
        }

        private void btnNghiPhep_Click(object sender, EventArgs e)
        {
            // Mở trực tiếp Form/UserControl Nghỉ phép (Theo logic Code mới)
            Form formPopup = new Form();
            formPopup.Text = "Quản lý Nghỉ Phép";
            formPopup.Size = new Size(1100, 700);
            formPopup.StartPosition = FormStartPosition.CenterScreen;

            // Đảm bảo bạn đã có UC_NghiPhepNV trong dự án
            UC_NghiPhepNV ucNghiPhep = new UC_NghiPhepNV();
            ucNghiPhep.Dock = DockStyle.Fill;
            formPopup.Controls.Add(ucNghiPhep);

            formPopup.ShowDialog();
        }
    }
}