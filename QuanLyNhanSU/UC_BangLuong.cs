using System;
using System.Data;
using System.Data.SqlClient;
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

            dtpKyLuong.Format = DateTimePickerFormat.Custom;
            dtpKyLuong.CustomFormat = "MM/yyyy";
            dtpKyLuong.ShowUpDown = true;
            dgvBangLuong.AutoGenerateColumns = false;

            // Map dữ liệu
            dgvBangLuong.Columns["ID"].DataPropertyName = "ID";
            dgvBangLuong.Columns["MANV"].DataPropertyName = "MANV";
            dgvBangLuong.Columns["HOTEN"].DataPropertyName = "HOTEN";
            dgvBangLuong.Columns["LUONGCB"].DataPropertyName = "LUONGCOBAN";
            dgvBangLuong.Columns["NGAYCONG"].DataPropertyName = "NGAYCONGTHUCTE";
            dgvBangLuong.Columns["PHUCAP"].DataPropertyName = "TIENPHUCAP";
            dgvBangLuong.Columns["THUONG"].DataPropertyName = "TIENKHENTHUONG";
            dgvBangLuong.Columns["KYLUAT"].DataPropertyName = "TIENKYLUAT";

            // --- SỬA 1: Thêm hiển thị cột Ứng Lương (Bạn cần vào Design thêm cột tên là UNGLUONG nhé) ---
            if (dgvBangLuong.Columns.Contains("UNGLUONG"))
            {
                dgvBangLuong.Columns["UNGLUONG"].DataPropertyName = "TIENUNGLUONG";
                dgvBangLuong.Columns["UNGLUONG"].DefaultCellStyle.Format = "N0";
            }
            // ------------------------------------------------------------------------------------------

            dgvBangLuong.Columns["THUCLINH"].DataPropertyName = "THUCLINH";

            // Format tiền tệ
            foreach (DataGridViewColumn col in dgvBangLuong.Columns)
            {
                if (col.Name != "ID" && col.Name != "MANV" && col.Name != "HOTEN" && col.Name != "NGAYCONG")
                {
                    col.DefaultCellStyle.Format = "N0";
                }
            }

            LoadBangLuong();
        }

        private void LoadBangLuong()
        {
            try
            {
                if (conn == null) conn = new SqlConnection(connString);
                if (conn.State == ConnectionState.Closed) conn.Open();

                int maKyCong = int.Parse(dtpKyLuong.Value.ToString("yyyyMM"));
                string sql = @"SELECT * FROM tb_BANGLUONG WHERE MAKYCONG = @MAKYCONG";

                daBangLuong = new SqlDataAdapter(sql, conn);
                daBangLuong.SelectCommand.Parameters.AddWithValue("@MAKYCONG", maKyCong);

                if (ds.Tables.Contains("tblBANGLUONG")) ds.Tables["tblBANGLUONG"].Clear();

                daBangLuong.Fill(ds, "tblBANGLUONG");
                dgvBangLuong.DataSource = ds.Tables["tblBANGLUONG"];
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải bảng lương: " + ex.Message); }
        }

        private void dtpKyLuong_ValueChanged(object sender, EventArgs e)
        {
            LoadBangLuong();
        }

        private void btnTinhLuong_Click(object sender, EventArgs e)
        {
            int maKyCong = int.Parse(dtpKyLuong.Value.ToString("yyyyMM"));

            // 1. Xóa lương cũ nếu đã tính
            if (ds.Tables["tblBANGLUONG"].Rows.Count > 0)
            {
                if (MessageBox.Show("Kỳ lương này đã được tính. Bạn có muốn tính lại không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteBangLuongThang(maKyCong);
                }
                else return;
            }

            // 2. Bắt đầu tính
            TinhToanLuongDonGian(maKyCong);
            LoadBangLuong();
        }

        private void TinhToanLuongDonGian(int maKyCong)
        {
            double LUONG_CO_SO = 1800000;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                int thang = maKyCong % 100;
                int nam = maKyCong / 100;

                // Lấy số công chuẩn
                double NGAY_CONG_CHUAN = GetNgayCongChuan(thang, nam);

                // Lấy danh sách nhân viên
                string sqlNV = "SELECT MANV, HOTEN FROM tb_NHANVIEN";
                SqlDataAdapter daNV = new SqlDataAdapter(sqlNV, conn);
                DataTable dtNV = new DataTable();
                daNV.Fill(dtNV);

                // Các câu query lấy dữ liệu phụ
                string sqlHeSo = @"SELECT TOP 1 HESOLUONG FROM tb_HOPDONG WHERE MANV = @MANV ORDER BY NGAYKY DESC";
                string sqlPhuCap = @"SELECT SUM(SOTIEN) FROM tb_NHANVIEN_PHUCAP WHERE MANV = @MANV";
                string sqlThuong = @"SELECT SUM(SOTIEN) FROM TB_KHENTHUONG WHERE MANV = @MANV AND MONTH(NGAYKY) = @THANG AND YEAR(NGAYKY) = @NAM";
                string sqlKyLuat = @"SELECT SUM(SOTIEN) FROM TB_KYLUAT WHERE MANV = @MANV AND MONTH(NGAYKY) = @THANG AND YEAR(NGAYKY) = @NAM";
                string sqlUngLuong = @"SELECT SUM(SOTIEN) FROM TB_UNGLUONG WHERE MANV = @MANV AND MONTH(NGAY) = @THANG AND YEAR(NGAY) = @NAM";

                foreach (DataRow nv in dtNV.Rows)
                {
                    string maNV = nv["MANV"].ToString();
                    string hoTen = nv["HOTEN"].ToString();

                    // 1. Lấy các thông số tiền
                    double heSo = GetValueFromDB(sqlHeSo, maNV, 0, 0);
                    if (heSo == 0) heSo = 1;

                    double phuCap = GetValueFromDB(sqlPhuCap, maNV, 0, 0);
                    double tienThuong = GetValueFromDB(sqlThuong, maNV, thang, nam);
                    double tienKyLuat = GetValueFromDB(sqlKyLuat, maNV, thang, nam);
                    double tienUng = GetValueFromDB(sqlUngLuong, maNV, thang, nam);

                    // 2. ĐẾM SỐ NGÀY CÔNG
                    double soNgayDiLam = CountNgayCong(maNV, thang, nam);

                    // 3. TÍNH TOÁN
                    double luongHopDong = LUONG_CO_SO * heSo;
                    double luongThucTe = (luongHopDong / NGAY_CONG_CHUAN) * soNgayDiLam;

                    // --- ĐÃ CÓ TRỪ TIỀN ỨNG Ở ĐÂY LÀ ĐÚNG RỒI ---
                    double thucLinh = luongThucTe + phuCap + tienThuong - tienKyLuat - tienUng;

                    // 4. LƯU VÀO DB
                    InsertBangLuong(maKyCong, maNV, hoTen, luongHopDong, soNgayDiLam, phuCap, tienThuong, tienKyLuat, tienUng, thucLinh);
                }

                MessageBox.Show("Đã tính lương xong!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính lương: " + ex.Message);
            }
        }

        private double CountNgayCong(string maNV, int thang, int nam)
        {
            try
            {
                string sql = "SELECT COUNT(DISTINCT NGAY) FROM tb_BANGCONG WHERE MANV=@MANV AND THANG=@THANG AND NAM=@NAM";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MANV", maNV);
                cmd.Parameters.AddWithValue("@THANG", thang);
                cmd.Parameters.AddWithValue("@NAM", nam);
                return Convert.ToDouble(cmd.ExecuteScalar());
            }
            catch { return 0; }
        }

        private double GetValueFromDB(string sql, string maNV, int thang, int nam)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MANV", maNV);
                if (thang > 0) cmd.Parameters.AddWithValue("@THANG", thang);
                if (nam > 0) cmd.Parameters.AddWithValue("@NAM", nam);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value) return Convert.ToDouble(result);
            }
            catch { }
            return 0;
        }

        private double GetNgayCongChuan(int thang, int nam)
        {
            try
            {
                int maKyCong = nam * 100 + thang;
                string sql = "SELECT NGAYCONGCHUAN FROM TB_KYCONG WHERE MAKYCONG = @MAKYCONG";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MAKYCONG", maKyCong);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value) return Convert.ToDouble(result);
            }
            catch { }
            return 26;
        }

        // --- SỬA 2: CẬP NHẬT HÀM INSERT ---
        private void InsertBangLuong(int maKyCong, string maNV, string hoTen, double luongCB, double ngayCong, double phuCap, double thuong, double kyLuat, double ungLuong, double thucLinh)
        {
            // Thêm cột TIENUNGLUONG vào câu lệnh INSERT
            string sql = @"INSERT INTO tb_BANGLUONG 
            (MAKYCONG, MANV, HOTEN, LUONGCOBAN, NGAYCONGTHUCTE, TIENPHUCAP, TIENKHENTHUONG, TIENKYLUAT, TIENUNGLUONG, THUCLINH, NGAYTINH)
            VALUES (@KC, @MNV, @TEN, @LCB, @NC, @PC, @KT, @KL, @UL, @TL, GETDATE())";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@KC", maKyCong);
            cmd.Parameters.AddWithValue("@MNV", maNV);
            cmd.Parameters.AddWithValue("@TEN", hoTen);
            cmd.Parameters.AddWithValue("@LCB", luongCB);
            cmd.Parameters.AddWithValue("@NC", ngayCong);
            cmd.Parameters.AddWithValue("@PC", phuCap);
            cmd.Parameters.AddWithValue("@KT", thuong);
            cmd.Parameters.AddWithValue("@KL", kyLuat);
            cmd.Parameters.AddWithValue("@UL", ungLuong); // Tham số Ứng lương
            cmd.Parameters.AddWithValue("@TL", thucLinh);
            cmd.ExecuteNonQuery();
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
            catch { }
        }

        private void btnHienNangLuong_Click(object sender, EventArgs e)
        {
            add_NangLuong_form f = new add_NangLuong_form();
            f.ShowDialog();
        }

        private void btnHienNVungluong_Click(object sender, EventArgs e)
        {
            add_UngLuong_form f = new add_UngLuong_form();
            f.ShowDialog();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int maKyCong = int.Parse(dtpKyLuong.Value.ToString("yyyyMM"));
            DeleteBangLuongThang(maKyCong);
            LoadBangLuong();
        }
    }
}