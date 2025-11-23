using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class frmChamCongThuCong : Form
    {
        string strKetNoi = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        SqlConnection conn = null;

        public frmChamCongThuCong()
        {
            InitializeComponent();
        }

        private void frmChamCongThuCong_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(strKetNoi);
            LoadComboBoxNhanVien();

            // Cấu hình DateTimePicker chỉ hiện Tháng/Năm
            dtpThangNam.Format = DateTimePickerFormat.Custom;
            dtpThangNam.CustomFormat = "MM/yyyy";

            // Mặc định load ngày của tháng hiện tại
            TaoGiaoDienLich(DateTime.Now.Month, DateTime.Now.Year);
        }

        private void LoadComboBoxNhanVien()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MANV, HOTEN FROM tb_NHANVIEN", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboNhanVien.DataSource = dt;
                cboNhanVien.DisplayMember = "HOTEN";
                cboNhanVien.ValueMember = "MANV";
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load nhân viên: " + ex.Message); }
        }

        private void dtpThangNam_ValueChanged(object sender, EventArgs e)
        {
            // Khi đổi tháng thì vẽ lại lịch
            TaoGiaoDienLich(dtpThangNam.Value.Month, dtpThangNam.Value.Year);
            LoadDuLieuDaCham(); // Nếu tháng đó đã chấm rồi thì hiện lại màu xanh
        }

        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDuLieuDaCham();
        }

        // --- HÀM 1: TẠO CÁC Ô VUÔNG (QUAN TRỌNG) ---
        private void TaoGiaoDienLich(int thang, int nam)
        {
            flpNgayCong.Controls.Clear();
            int soNgayTrongThang = DateTime.DaysInMonth(nam, thang);

            for (int i = 1; i <= soNgayTrongThang; i++)
            {
                Button btn = new Button();
                btn.Name = "btnDay_" + i;
                btn.Text = i.ToString();
                btn.Size = new Size(50, 50); // Kích thước ô vuông
                btn.BackColor = Color.White; // Mặc định là chưa chọn
                btn.FlatStyle = FlatStyle.Flat;
                btn.Click += BtnNgay_Click; // Gán sự kiện click

                // Tô màu ngày chủ nhật cho dễ nhìn (nếu muốn)
                DateTime ngay = new DateTime(nam, thang, i);
                if (ngay.DayOfWeek == DayOfWeek.Sunday)
                {
                    btn.ForeColor = Color.Red;
                }

                flpNgayCong.Controls.Add(btn);
            }
            CapNhatTongCong();
        }

        // --- HÀM 2: SỰ KIỆN KHI BẤM VÀO Ô NGÀY ---
        private void BtnNgay_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackColor == Color.White)
            {
                btn.BackColor = Color.Blue;
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;

                // Trả lại màu đỏ nếu là chủ nhật
                int ngay = int.Parse(btn.Text);
                DateTime d = new DateTime(dtpThangNam.Value.Year, dtpThangNam.Value.Month, ngay);
                if (d.DayOfWeek == DayOfWeek.Sunday) btn.ForeColor = Color.Red;
            }
            CapNhatTongCong();
        }

        private void CapNhatTongCong()
        {
            int count = 0;
            foreach (Control c in flpNgayCong.Controls)
            {
                if (c is Button && c.BackColor == Color.Blue) count++;
            }
            lblTongCong.Text = "Số ngày công chọn: " + count;
        }

        // --- HÀM 3: LƯU DỮ LIỆU GIẢ LẬP ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboNhanVien.SelectedValue == null) return;

            string maNV = cboNhanVien.SelectedValue.ToString();
            int thang = dtpThangNam.Value.Month;
            int nam = dtpThangNam.Value.Year;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // BƯỚC 1: XÓA CÔNG CŨ CỦA THÁNG NÀY (Để cập nhật lại từ đầu)
                string sqlDel = "DELETE FROM tb_BANGCONG WHERE MANV=@MANV AND THANG=@THANG AND NAM=@NAM";
                SqlCommand cmdDel = new SqlCommand(sqlDel, conn);
                cmdDel.Parameters.AddWithValue("@MANV", maNV);
                cmdDel.Parameters.AddWithValue("@THANG", thang);
                cmdDel.Parameters.AddWithValue("@NAM", nam);
                cmdDel.ExecuteNonQuery();

                // BƯỚC 2: DUYỆT CÁC Ô MÀU XANH ĐỂ INSERT
                foreach (Control c in flpNgayCong.Controls)
                {
                    Button btn = c as Button;
                    if (btn.BackColor == Color.Blue)
                    {
                        int ngay = int.Parse(btn.Text);

                        // Tạo giờ VÀO/RA chuẩn (08:00 - 17:00) để ăn trọn 1 ngày công
                        DateTime ngayLam = new DateTime(nam, thang, ngay);
                        DateTime gioVao = new DateTime(nam, thang, ngay, 8, 0, 0);
                        DateTime gioRa = new DateTime(nam, thang, ngay, 17, 0, 0);

                        string sqlIns = @"INSERT INTO tb_BANGCONG (MANV, NGAY, THANG, NAM, ThoiGianVao, ThoiGianRa) 
                                          VALUES (@MANV, @NGAY, @THANG, @NAM, @GIOVAO, @GIORA)";

                        SqlCommand cmdIns = new SqlCommand(sqlIns, conn);
                        cmdIns.Parameters.AddWithValue("@MANV", maNV);
                        cmdIns.Parameters.AddWithValue("@NGAY", ngayLam);
                        cmdIns.Parameters.AddWithValue("@THANG", thang);
                        cmdIns.Parameters.AddWithValue("@NAM", nam);
                        cmdIns.Parameters.AddWithValue("@GIOVAO", gioVao);
                        cmdIns.Parameters.AddWithValue("@GIORA", gioRa);

                        cmdIns.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Đã chấm công thủ công thành công! Bạn có thể qua tính lương ngay.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu: " + ex.Message);
            }
        }

        // --- HÀM 4: LOAD LẠI DỮ LIỆU ĐÃ CHẤM (Nếu mở lại form) ---
        private void LoadDuLieuDaCham()
        {
            if (cboNhanVien.SelectedValue == null) return;

            // Reset màu trắng hết trước
            foreach (Control c in flpNgayCong.Controls)
            {
                Button btn = c as Button;
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                // (Xử lý lại màu chủ nhật nếu cần...)
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string maNV = cboNhanVien.SelectedValue.ToString();
                int thang = dtpThangNam.Value.Month;
                int nam = dtpThangNam.Value.Year;

                // Lấy danh sách các ngày đã chấm công
                string sql = "SELECT DAY(NGAY) as Ngay FROM tb_BANGCONG WHERE MANV=@MANV AND THANG=@THANG AND NAM=@NAM";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MANV", maNV);
                cmd.Parameters.AddWithValue("@THANG", thang);
                cmd.Parameters.AddWithValue("@NAM", nam);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int day = int.Parse(dr["Ngay"].ToString());
                    // Tìm cái nút tương ứng để tô màu xanh
                    Control[] ctrls = flpNgayCong.Controls.Find("btnDay_" + day, false);
                    if (ctrls.Length > 0)
                    {
                        ctrls[0].BackColor = Color.Blue;
                        ctrls[0].ForeColor = Color.White;
                    }
                }
                dr.Close();
                CapNhatTongCong();
            }
            catch { }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}