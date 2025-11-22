using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class Form_GhiCong : Form
    {
        // 1. Kết nối SQL
        string connString = @"Data Source=TUNG-IT\MSSQL_EXP_2008R2;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        SqlConnection conn;

        // Biến lưu thông tin nhận được
        string _maNV;
        int _thang;
        int _nam;

        // Constructor nhận tham số (QUAN TRỌNG)
        public Form_GhiCong(string maNV, string tenNV, int thang, int nam)
        {
            InitializeComponent();

            // Lưu lại để dùng
            _maNV = maNV;
            _thang = thang;
            _nam = nam;

            // Hiển thị tên nhân viên lên TextBox (như bạn yêu cầu)
            if (txtChamCongNV != null)
            {
                txtChamCongNV.Text = tenNV;
                txtChamCongNV.ReadOnly = true; // Khóa không cho sửa tên
            }

            // Cập nhật tiêu đề Form
            this.Text = $"Chi tiết chấm công - Tháng {_thang}/{_nam}";
            conn = new SqlConnection(connString);
            TaoLichLamViec(); // Vẽ lịch ngay khi tạo form
        }

        private void Form_GhiCong_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);
            TaoLichLamViec(); // Vẽ lịch ngay khi mở
        }

        // --- HÀM TỰ ĐỘNG VẼ CÁC Ô NGÀY ---
        private void TaoLichLamViec()
        {
            if (flpLich == null) return; // Kiểm tra an toàn

            flpLich.Controls.Clear(); // Xóa các nút cũ
            int soNgayTrongThang = DateTime.DaysInMonth(_nam, _thang);

            // Lấy danh sách các ngày ĐÃ CÓ trong Database
            List<int> ngayDaCham = LayDuLieuDaCham();

            // Vòng lặp tạo nút (Button)
            for (int i = 1; i <= soNgayTrongThang; i++)
            {
                Button btn = new Button();
                btn.Name = "btnNgay_" + i;
                btn.Text = i.ToString();
                btn.Width = 60;
                btn.Height = 50;
                btn.Margin = new Padding(5);
                btn.Font = new Font("Arial", 10, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;

                // Logic tô màu: Có trong SQL thì Xanh, không thì Trắng
                if (ngayDaCham.Contains(i))
                {
                    btn.BackColor = Color.MediumSeaGreen;
                    btn.ForeColor = Color.White;
                    btn.Tag = "Co"; // Đánh dấu là CÓ đi làm
                }
                else
                {
                    btn.BackColor = Color.WhiteSmoke;
                    btn.Tag = "Khong";
                }

                // GẮN SỰ KIỆN CLICK
                btn.Click += BtnNgay_Click;

                flpLich.Controls.Add(btn);
            }
        }

        // Hàm lấy danh sách ngày đã đi làm từ SQL
        private List<int> LayDuLieuDaCham()
        {
            List<int> listNgay = new List<int>();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // Chỉ lấy cột NGAY (Day)
                string sql = "SELECT DAY(NGAY) FROM tb_BANGCONG WHERE MANV=@MaNV AND THANG=@Thang AND NAM=@Nam";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", _maNV);
                cmd.Parameters.AddWithValue("@Thang", _thang);
                cmd.Parameters.AddWithValue("@Nam", _nam);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listNgay.Add(dr.GetInt32(0)); // Thêm ngày vào list
                }
                dr.Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
            return listNgay;
        }

        // --- SỰ KIỆN KHI BẤM VÀO Ô NGÀY ---
        private void BtnNgay_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Tag.ToString() == "Co")
            {
                // Đang có -> Chuyển thành Không
                btn.BackColor = Color.WhiteSmoke;
                btn.ForeColor = Color.Black;
                btn.Tag = "Khong";
            }
            else
            {
                // Đang không -> Chuyển thành Có
                btn.BackColor = Color.MediumSeaGreen;
                btn.ForeColor = Color.White;
                btn.Tag = "Co";
            }
        }

        // --- NÚT LƯU (btnLuu) ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn lưu bảng công này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No) return;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // BƯỚC 1: Xóa sạch dữ liệu cũ của tháng này (Reset)
                string sqlDelete = "DELETE FROM tb_BANGCONG WHERE MANV=@MaNV AND THANG=@Thang AND NAM=@Nam";
                SqlCommand cmdDel = new SqlCommand(sqlDelete, conn);
                cmdDel.Parameters.AddWithValue("@MaNV", _maNV);
                cmdDel.Parameters.AddWithValue("@Thang", _thang);
                cmdDel.Parameters.AddWithValue("@Nam", _nam);
                cmdDel.ExecuteNonQuery();

                // BƯỚC 2: Duyệt qua các nút, nút nào MÀU XANH thì INSERT lại
                int dem = 0;
                foreach (Control c in flpLich.Controls)
                {
                    if (c is Button btn && btn.Tag.ToString() == "Co")
                    {
                        int ngay = int.Parse(btn.Text);
                        DateTime ngayCheck = new DateTime(_nam, _thang, ngay);

                        // Mặc định giờ hành chính (8h00 - 17h00)
                        DateTime gioVao = new DateTime(_nam, _thang, ngay, 8, 0, 0);
                        DateTime gioRa = new DateTime(_nam, _thang, ngay, 17, 0, 0);

                        string sqlInsert = @"INSERT INTO tb_BANGCONG (MANV, NAM, THANG, NGAY, ThoiGianVao, ThoiGianRa) 
                                             VALUES (@MaNV, @Nam, @Thang, @Ngay, @GioVao, @GioRa)";

                        SqlCommand cmdIn = new SqlCommand(sqlInsert, conn);
                        cmdIn.Parameters.AddWithValue("@MaNV", _maNV);
                        cmdIn.Parameters.AddWithValue("@Nam", _nam);
                        cmdIn.Parameters.AddWithValue("@Thang", _thang);
                        cmdIn.Parameters.Add("@Ngay", SqlDbType.Date).Value = ngayCheck;
                        cmdIn.Parameters.AddWithValue("@GioVao", gioVao);
                        cmdIn.Parameters.AddWithValue("@GioRa", gioRa);

                        cmdIn.ExecuteNonQuery();
                        dem++;
                    }
                }

                MessageBox.Show($"Đã lưu thành công {dem} ngày công!", "Thông báo");
                this.Close(); // Đóng form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        // --- NÚT XÓA (btnXoa) ---
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn xóa trắng bảng công tháng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Chỉ xóa trên giao diện (chưa xóa DB, bấm Lưu mới xóa DB)
                foreach (Control c in flpLich.Controls)
                {
                    if (c is Button btn)
                    {
                        btn.BackColor = Color.WhiteSmoke;
                        btn.Tag = "Khong";
                    }
                }
            }
        }

        // --- NÚT SỬA / TẢI LẠI (btnSua) ---
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Nút này dùng để tải lại dữ liệu gốc từ Database (Reset những gì vừa bấm bậy)
            TaoLichLamViec();
        }

        // --- NÚT THOÁT (btnThoat) ---
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}