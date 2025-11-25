using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;

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
            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng thêm/sửa/xóa
                btnTinhLuong.Enabled = false;
                btnHienNangLuong.Enabled = false;
                btnHienNVungluong.Enabled = false;
                btnInPhieuLuong.Enabled = false;
                btnQuayLai.Enabled = false;
                btnXoa.Enabled = false;
                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox) ((TextBox)c).ReadOnly = true;
                }

                // 3. GridView chỉ cho xem
                dgvBangLuong.ReadOnly = true;
            }
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
            //---Load combobox tìm kiếm ---
            cboTimKiemLuong.Items.Clear();
            // Thêm các lựa chọn
            cboTimKiemLuong.Items.Add("Mã Nhân Viên");
            cboTimKiemLuong.Items.Add("Họ Tên");
            // Mặc định chọn dòng đầu tiên (Mã Nhân Viên) để không bị trắng
            cboTimKiemLuong.SelectedIndex = 0;
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

        private void btnInPhieuLuong_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem đã chọn nhân viên nào chưa
            if (dgvBangLuong.SelectedRows.Count == 0 && dgvBangLuong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để in phiếu lương!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dòng đang chọn
            if (dgvBangLuong.SelectedRows.Count > 0)
                rowToPrint = dgvBangLuong.SelectedRows[0];
            else
                rowToPrint = dgvBangLuong.CurrentRow;

            // 2. Cấu hình in ấn
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(InPhieuLuong_NoiDung);

            // 3. Hiện khung xem trước (Preview)
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.Width = 600;
            ppd.Height = 800;
            ppd.ShowDialog();

        }
        // Hàm này dùng để "Vẽ" nội dung lên giấy
        DataGridViewRow rowToPrint;
        private void InPhieuLuong_NoiDung(object sender, PrintPageEventArgs e)
        {
            // Lấy thông tin từ dòng đã chọn (Đảm bảo tên cột đúng với DataPropertyName bạn đã đặt)
            string maNV = rowToPrint.Cells["MANV"].FormattedValue.ToString();
            string hoTen = rowToPrint.Cells["HOTEN"].FormattedValue.ToString();
            string kyLuong = dtpKyLuong.Value.ToString("MM/yyyy");

            string luongHD = rowToPrint.Cells["LUONGCB"].FormattedValue.ToString(); // Hoặc LUONGHOPDONG tùy tên cột
            string ngayCong = rowToPrint.Cells["NGAYCONG"].FormattedValue.ToString();
            string phuCap = rowToPrint.Cells["PHUCAP"].FormattedValue.ToString();
            string thuong = rowToPrint.Cells["THUONG"].FormattedValue.ToString();
            string kyLuat = rowToPrint.Cells["KYLUAT"].FormattedValue.ToString();

            // Kiểm tra xem cột Ứng Lương có tồn tại không
            string ungLuong = "0";
            if (dgvBangLuong.Columns.Contains("UNGLUONG"))
                ungLuong = rowToPrint.Cells["UNGLUONG"].FormattedValue.ToString();

            string thucLinh = rowToPrint.Cells["THUCLINH"].FormattedValue.ToString();

            // --- BẮT ĐẦU VẼ ---
            Graphics g = e.Graphics;
            Font fontTieuDe = new Font("Arial", 18, FontStyle.Bold);
            Font fontDam = new Font("Arial", 12, FontStyle.Bold);
            Font fontThuong = new Font("Arial", 12, FontStyle.Regular);

            float y = 50; // Tọa độ Y (độ cao), cứ viết xong 1 dòng thì cộng thêm vào
            float x_label = 100; // Tọa độ X của nhãn (bên trái)
            float x_value = 400; // Tọa độ X của giá trị (bên phải)

            // 1. Tiêu đề
            g.DrawString("PHIẾU LƯƠNG NHÂN VIÊN", fontTieuDe, Brushes.Blue, new PointF(220, y));
            y += 40;
            g.DrawString("Kỳ lương: " + kyLuong, fontThuong, Brushes.Black, new PointF(300, y));
            y += 50;

            // Kẻ đường gạch ngang
            g.DrawLine(Pens.Black, 50, y, 750, y);
            y += 30;

            // 2. Thông tin chung
            g.DrawString("Mã Nhân Viên:", fontDam, Brushes.Black, new PointF(x_label, y));
            g.DrawString(maNV, fontThuong, Brushes.Black, new PointF(x_value, y));
            y += 30;

            g.DrawString("Họ và Tên:", fontDam, Brushes.Black, new PointF(x_label, y));
            g.DrawString(hoTen, fontThuong, Brushes.Black, new PointF(x_value, y));
            y += 50; // Cách xa một chút

            // 3. Chi tiết lương
            g.DrawString("Lương Hợp Đồng:", fontThuong, Brushes.Black, new PointF(x_label, y));
            g.DrawString(luongHD + " VNĐ", fontThuong, Brushes.Black, new PointF(x_value, y));
            y += 30;

            g.DrawString("Ngày công thực tế:", fontThuong, Brushes.Black, new PointF(x_label, y));
            g.DrawString(ngayCong + " ngày", fontThuong, Brushes.Black, new PointF(x_value, y));
            y += 30;

            g.DrawString("Phụ cấp:", fontThuong, Brushes.Green, new PointF(x_label, y));
            g.DrawString("+" + phuCap, fontThuong, Brushes.Green, new PointF(x_value, y));
            y += 30;

            g.DrawString("Thưởng:", fontThuong, Brushes.Green, new PointF(x_label, y));
            g.DrawString("+" + thuong, fontThuong, Brushes.Green, new PointF(x_value, y));
            y += 30;

            g.DrawString("Kỷ luật/Phạt:", fontThuong, Brushes.Red, new PointF(x_label, y));
            g.DrawString("-" + kyLuat, fontThuong, Brushes.Red, new PointF(x_value, y));
            y += 30;

            g.DrawString("Đã ứng:", fontThuong, Brushes.Red, new PointF(x_label, y));
            g.DrawString("-" + ungLuong, fontThuong, Brushes.Red, new PointF(x_value, y));
            y += 40;

            // Kẻ đường gạch ngang kết thúc
            g.DrawLine(Pens.Black, 100, y, 700, y);
            y += 20;

            // 4. Tổng kết
            g.DrawString("THỰC LĨNH:", new Font("Arial", 16, FontStyle.Bold), Brushes.Blue, new PointF(x_label, y));
            g.DrawString(thucLinh + " VNĐ", new Font("Arial", 16, FontStyle.Bold), Brushes.Red, new PointF(x_value, y));

            y += 100;
            g.DrawString("Người lập phiếu", fontThuong, Brushes.Black, new PointF(500, y));
            y += 30;
            g.DrawString("(Ký tên)", fontThuong, Brushes.Gray, new PointF(530, y));
        }

        private void btnHienAll_Click(object sender, EventArgs e)
        {
            txtTimKiemLuong.Text = "";
            ds.Tables["tblBANGLUONG"].DefaultView.RowFilter = string.Empty;
        }

        private void btnTiemKiemLuong_Click(object sender, EventArgs e)
        {
            string searchValue = txtTimKiemLuong.Text.Trim().Replace("'", "''");

            if (ds.Tables.Contains("tblBANGLUONG")) // Nhớ sửa tên bảng thành tblBANGLUONG
            {
                if (string.IsNullOrEmpty(searchValue))
                {
                    ds.Tables["tblBANGLUONG"].DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    // Kiểm tra xem người dùng đang chọn tìm theo cái gì
                    string kieuTimKiem = cboTimKiemLuong.Text;

                    if (kieuTimKiem == "Mã Nhân Viên")
                    {
                        ds.Tables["tblBANGLUONG"].DefaultView.RowFilter = $"MANV LIKE '%{searchValue}%'";
                    }
                    else if (kieuTimKiem == "Họ Tên")
                    {
                        ds.Tables["tblBANGLUONG"].DefaultView.RowFilter = $"HOTEN LIKE '%{searchValue}%'";
                    }
                }
            }
        }
    }
    }