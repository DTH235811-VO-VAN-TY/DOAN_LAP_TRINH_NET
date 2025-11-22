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

            // Cấu hình hiển thị DateTimePicker chỉ hiện Tháng/Năm
            dtpKyLuong.Format = DateTimePickerFormat.Custom;
            dtpKyLuong.CustomFormat = "MM/yyyy";
            dtpKyLuong.ShowUpDown = true;
            dgvBangLuong.AutoGenerateColumns = false;

            // --- MAP DỮ LIỆU (Đảm bảo tên cột bên phải KHỚP 100% với SQL) ---
            dgvBangLuong.Columns["ID"].DataPropertyName = "ID";
            dgvBangLuong.Columns["MANV"].DataPropertyName = "MANV";
            dgvBangLuong.Columns["HOTEN"].DataPropertyName = "HOTEN";

            // Cột này quan trọng, phải khớp với tên cột trong bảng tb_BANGLUONG
            dgvBangLuong.Columns["LUONGCB"].DataPropertyName = "LUONGCOBAN";

            dgvBangLuong.Columns["NGAYCONG"].DataPropertyName = "NGAYCONGTHUCTE";
            dgvBangLuong.Columns["PHUCAP"].DataPropertyName = "TIENPHUCAP";
            dgvBangLuong.Columns["THUONG"].DataPropertyName = "TIENKHENTHUONG";
            dgvBangLuong.Columns["KYLUAT"].DataPropertyName = "TIENKYLUAT";
            dgvBangLuong.Columns["THUCLINH"].DataPropertyName = "THUCLINH";

            // Định dạng hiển thị tiền
            dgvBangLuong.Columns["LUONGCB"].DefaultCellStyle.Format = "N0";
            dgvBangLuong.Columns["PHUCAP"].DefaultCellStyle.Format = "N0";
            dgvBangLuong.Columns["THUONG"].DefaultCellStyle.Format = "N0";
            dgvBangLuong.Columns["KYLUAT"].DefaultCellStyle.Format = "N0";
            dgvBangLuong.Columns["THUCLINH"].DefaultCellStyle.Format = "N0";

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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bảng lương: " + ex.Message);
            }
        }

        private void dtpKyLuong_ValueChanged(object sender, EventArgs e)
        {
            LoadBangLuong();
        }

        private void btnTinhLuong_Click(object sender, EventArgs e)
        {
            int maKyCong = int.Parse(dtpKyLuong.Value.ToString("yyyyMM"));

            if (ds.Tables["tblBANGLUONG"].Rows.Count > 0)
            {
                if (MessageBox.Show("Kỳ lương này đã được tính. Bạn có muốn tính lại không? (Dữ liệu cũ sẽ bị xóa)", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
                DeleteBangLuongThang(maKyCong);
            }

            TinhToanLuong(maKyCong);
            LoadBangLuong();
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
            catch (Exception ex) { MessageBox.Show("Lỗi xóa kỳ lương cũ: " + ex.Message); }
        }

        private void TinhToanLuong(int maKyCong)
        {
            double LUONG_CO_SO = 1800000;
            int NGAY_CONG_CHUAN = 26;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sqlNV = "SELECT MANV, HOTEN FROM tb_NHANVIEN";
                SqlDataAdapter daNV = new SqlDataAdapter(sqlNV, conn);
                DataTable dtNV = new DataTable();
                daNV.Fill(dtNV);

                string sqlHeSo = @"SELECT TOP 1 HESOLUONG FROM tb_HOPDONG WHERE MANV = @MANV ORDER BY NGAYKY DESC";
                string sqlPhuCap = @"SELECT SUM(SOTIEN) FROM tb_NHANVIEN_PHUCAP WHERE MANV = @MANV";
                string sqlThuong = @"SELECT SUM(SOTIEN) FROM TB_KHENTHUONG WHERE MANV = @MANV AND MONTH(NGAYKY) = @THANG AND YEAR(NGAYKY) = @NAM";
                string sqlKyLuat = @"SELECT SUM(SOTIEN) FROM TB_KYLUAT WHERE MANV = @MANV AND MONTH(NGAYKY) = @THANG AND YEAR(NGAYKY) = @NAM";

                int thang = maKyCong % 100;
                int nam = maKyCong / 100;

                foreach (DataRow nv in dtNV.Rows)
                {
                    string maNV = nv["MANV"].ToString();
                    string hoTen = nv["HOTEN"].ToString();

                    double heSo = GetValueFromDB(sqlHeSo, maNV, 0, 0);
                    if (heSo == 0) heSo = 1;

                    double phuCap = GetValueFromDB(sqlPhuCap, maNV, 0, 0);
                    double tienThuong = GetValueFromDB(sqlThuong, maNV, thang, nam);
                    double tienPhat = GetValueFromDB(sqlKyLuat, maNV, thang, nam);

                    double ngayCongThucTe = 26;
                    double tienTangCa = 0;
                    double tienUng = 0;

                    // Lương cơ bản theo Hợp đồng
                    double luongHopDong = LUONG_CO_SO * heSo;

                    // Lương thực tế theo ngày công
                    double luongTheoNgay = (luongHopDong / NGAY_CONG_CHUAN) * ngayCongThucTe;

                    double thucLinh = luongTheoNgay + phuCap + tienThuong + tienTangCa - tienPhat - tienUng;

                    // Truyền luongHopDong vào tham số luongCB
                    InsertBangLuong(maKyCong, maNV, hoTen, luongHopDong, ngayCongThucTe, phuCap, tienThuong, tienPhat, thucLinh);
                }

                MessageBox.Show("Đã tính lương xong! (Lưu ý: Ngày công đang mặc định là 26)");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính lương: " + ex.Message);
            }
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
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDouble(result);
                }
            }
            catch { }
            return 0;
        }

        private void InsertBangLuong(int maKyCong, string maNV, string hoTen, double luongCB, double ngayCong, double phuCap, double thuong, double kyLuat, double thucLinh)
        {
            string sqlInsert = @"INSERT INTO tb_BANGLUONG 
            (MAKYCONG, MANV, HOTEN, LUONGCOBAN, NGAYCONGTHUCTE, TIENPHUCAP, TIENKHENTHUONG, TIENKYLUAT, THUCLINH, NGAYTINH)
            VALUES (@KC, @MNV, @TEN, @LCB, @NC, @PC, @KT, @KL, @TL, GETDATE())";

            SqlCommand cmd = new SqlCommand(sqlInsert, conn);
            cmd.Parameters.AddWithValue("@KC", maKyCong);
            cmd.Parameters.AddWithValue("@MNV", maNV);
            cmd.Parameters.AddWithValue("@TEN", hoTen);
            cmd.Parameters.AddWithValue("@LCB", luongCB); // Đây là tham số Lương Cơ Bản
            cmd.Parameters.AddWithValue("@NC", ngayCong);
            cmd.Parameters.AddWithValue("@PC", phuCap);
            cmd.Parameters.Add("@KT", thuong);
            cmd.Parameters.Add("@KL", kyLuat);
            cmd.Parameters.Add("@TL", thucLinh);

            cmd.ExecuteNonQuery();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int maKyCong = int.Parse(dtpKyLuong.Value.ToString("yyyyMM"));
            if (MessageBox.Show($"Bạn chắc chắn muốn xóa bảng lương tháng {dtpKyLuong.Value:MM/yyyy}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DeleteBangLuongThang(maKyCong);
                LoadBangLuong();
            }
        }

        private void btnHienNangLuong_Click(object sender, EventArgs e)
        {
            add_NangLuong_form nangLuongForm = new add_NangLuong_form();
            nangLuongForm.ShowDialog();
        }

        private void btnHienNVungluong_Click(object sender, EventArgs e)
        {
            add_UngLuong_form ungLuongForm = new add_UngLuong_form();
            ungLuongForm.ShowDialog();
        }
    }
}