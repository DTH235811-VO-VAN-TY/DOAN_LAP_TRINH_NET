using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyNhanSU
{
    public partial class Uc_PhuCap : UserControl
    {
        public Uc_PhuCap()
        {
            InitializeComponent();
        }
        // Khai báo cấp độ Class
        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daPhuCap;
        SqlDataAdapter daNhanVien;
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cboMaNVPC.Text) || string.IsNullOrEmpty(cboTenPC.Text) || string.IsNullOrEmpty(txtSotienPC.Text))
                {
                    MessageBox.Show("Vui lòng nhập đủ: Mã NV, Tên PC, Số Tiền.", "Thông báo");
                    return;
                }

                DataRow row = ds.Tables["PhuCap"].NewRow();
                row["MANV"] = cboMaNVPC.SelectedValue;
                row["TenNhanVien"] = txtTenNVPC.Text;
                row["TENPC"] = cboTenPC.Text;
                row["SOTIEN"] = decimal.Parse(txtSotienPC.Text);
                row["NGAYBATDAU"] = dtpNgayBatDau.Value;
                row["GHICHU"] = ""; // Gán tạm rỗng nếu chưa có control nhập Ghi chú

                ds.Tables["PhuCap"].Rows.Add(row);
                MessageBox.Show("Đã thêm vào danh sách chờ. Nhấn LƯU để cập nhật CSDL.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        private void Uc_PhuCap_Load(object sender, EventArgs e)
        {
            string cnStr = @"Data Source=LAPTOP-S5P1Q2HR\SQLEXPRESS;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

            // 2. Khởi tạo đối tượng kết nối với chuỗi đó
            conn = new SqlConnection(cnStr);


            try
            {
                conn.Open();

                // --- A. Tải danh sách Nhân Viên vào ComboBox (cboMaNVPC) ---
                string sqlNV = "SELECT MANV, HOTEN FROM TB_NHANVIEN";
                daNhanVien = new SqlDataAdapter(sqlNV, conn);
                daNhanVien.Fill(ds, "NhanVien");

                cboMaNVPC.DataSource = ds.Tables["NhanVien"];
                cboMaNVPC.DisplayMember = "MANV";
                cboMaNVPC.ValueMember = "MANV";
                cboMaNVPC.SelectedIndex = -1; // Không chọn ai lúc đầu

                // --- B. Tải danh sách Tên Phụ Cấp (Nếu có bảng danh mục riêng thì tải, ko thì add cứng hoặc lấy distinct) ---
                // Ví dụ add cứng vài loại cho cboTenPC nếu chưa có bảng danh mục
                cboTenPC.Items.Add("Phụ cấp ăn trưa");
                cboTenPC.Items.Add("Phụ cấp xăng xe");
                cboTenPC.Items.Add("Phụ cấp trách nhiệm");

                // --- C. Gọi hàm tải dữ liệu chính ---
                LoadDataPhuCap();

                // --- D. Tải ComboBox Tìm kiếm ---
                LoadSearchComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadSearchComboBox()
        {
            var searchOptions = new[] {
                new { Text = "Tìm theo Mã NV", Value = "MANV" },
                new { Text = "Tìm theo Tên NV", Value = "TenNhanVien" },
                new { Text = "Tìm theo Tên Phụ Cấp", Value = "TENPC" }
            };
            cboTimKiemPC.DataSource = searchOptions;
            cboTimKiemPC.DisplayMember = "Text";
            cboTimKiemPC.ValueMember = "Value";
            cboTimKiemPC.SelectedIndex = 0;
        }

        private void dvgPhuCap_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPhuCap.Rows[e.RowIndex].Cells["IDPC"].Value != DBNull.Value)
            {
                DataGridViewRow row = dgvPhuCap.Rows[e.RowIndex];
                try
                {
                    // Đổ dữ liệu từ dòng chọn lên các controls
                    txtIDPC.Text = row.Cells["IDPC"].Value.ToString();
                    cboMaNVPC.SelectedValue = row.Cells["MANV"].Value.ToString();
                    // Tên NV tự nhảy theo cboMaNVPC_SelectedIndexChanged

                    cboTenPC.Text = row.Cells["TENPC"].Value.ToString();
                    txtSotienPC.Text = row.Cells["SOTIEN"].Value.ToString();

                    if (row.Cells["NGAYBATDAU"].Value != DBNull.Value)
                        dtpNgayBatDau.Value = Convert.ToDateTime(row.Cells["NGAYBATDAU"].Value);

                    // Nếu có TextBox Ghi chú thì gán vào:
                    // txtGhiChu.Text = row.Cells["GHICHU"].Value.ToString();
                }
                catch (Exception ex) { /* Bỏ qua lỗi nhỏ khi click */ }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật toàn bộ thay đổi (Thêm/Sửa/Xóa) từ DataSet xuống DB
                int result = daPhuCap.Update(ds, "PhuCap");
                MessageBox.Show($"Đã cập nhật thành công {result} dòng dữ liệu.", "Thông báo");

                // Load lại để làm tươi dữ liệu và lấy ID mới
                LoadDataPhuCap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu xuống CSDL: " + ex.Message + "\n(Hãy kiểm tra lại cấu trúc bảng trong SQL)", "Lỗi Nghiêm Trọng");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDPC.Text))
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa từ lưới.", "Thông báo");
                return;
            }

            try
            {
                // Tìm dòng trong DataSet dựa vào IDPC (Khóa chính)
                int idpc = int.Parse(txtIDPC.Text);
                DataRow row = ds.Tables["PhuCap"].Rows.Find(idpc);

                if (row != null)
                {
                    row.BeginEdit();
                    row["MANV"] = cboMaNVPC.SelectedValue;
                    row["TenNhanVien"] = txtTenNVPC.Text;
                    row["TENPC"] = cboTenPC.Text;
                    row["SOTIEN"] = decimal.Parse(txtSotienPC.Text);
                    row["NGAYBATDAU"] = dtpNgayBatDau.Value;
                    row["GHICHU"] = "";
                    row.EndEdit();
                    MessageBox.Show("Đã sửa xong. Nhấn LƯU để cập nhật CSDL.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDPC.Text))
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    int idpc = int.Parse(txtIDPC.Text);
                    DataRow row = ds.Tables["PhuCap"].Rows.Find(idpc);
                    if (row != null)
                    {
                        row.Delete(); // Đánh dấu xóa
                        MessageBox.Show("Đã xóa. Nhấn LƯU để cập nhật CSDL.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtIDPC.Clear();
            cboMaNVPC.SelectedIndex = -1;
            txtTenNVPC.Clear();
            cboTenPC.SelectedIndex = -1;
            cboTenPC.Text = "";
            txtSotienPC.Clear();
            dtpNgayBatDau.Value = DateTime.Now;
            LoadDataPhuCap(); // Tải lại dữ liệu gốc
        }

        private void cboMaNVPC_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboMaNVPC.SelectedValue != null)
            {
                // Tìm dòng dữ liệu tương ứng trong bảng NhanVien của DataSet
                string maSelected = cboMaNVPC.SelectedValue.ToString();
                DataRow[] rows = ds.Tables["NhanVien"].Select("MANV = '" + maSelected + "'");
                if (rows.Length > 0)
                {
                    txtTenNVPC.Text = rows[0]["HOTEN"].ToString();
                }
            }
            else
            {
                txtTenNVPC.Clear();
            }
        }
        private void LoadDataPhuCap()
        {
            try
            {
                // Câu lệnh SELECT kết hợp JOIN để lấy Tên Nhân Viên hiển thị ra Grid
                // Lưu ý: Bảng phải có cột MANV, NGAYBATDAU, GHICHU mới chạy được lệnh này
                string sqlSelect = @"
                    SELECT 
                        pc.IDPC, 
                        pc.MANV, 
                        nv.HOTEN as TenNhanVien, 
                        pc.TENPC, 
                        pc.SOTIEN, 
                        pc.NGAYBATDAU, 
                        pc.GHICHU 
                    FROM TB_PHUCAP pc
                    LEFT JOIN TB_NHANVIEN nv ON pc.MANV = nv.MANV";

                daPhuCap = new SqlDataAdapter(sqlSelect, conn);

                // Xóa dữ liệu cũ trong DataSet để nạp mới
                if (ds.Tables.Contains("PhuCap")) ds.Tables["PhuCap"].Clear();

                daPhuCap.Fill(ds, "PhuCap");

                // Thiết lập Khóa chính để tìm kiếm/sửa/xóa chính xác
                ds.Tables["PhuCap"].PrimaryKey = new DataColumn[] { ds.Tables["PhuCap"].Columns["IDPC"] };

                // Đổ dữ liệu ra DataGridView
                dgvPhuCap.DataSource = ds.Tables["PhuCap"];

                // --- CẤU HÌNH CÁC CỘT TRONG GRID (Ẩn ID, hiện tên tiếng Việt) ---
                // Bạn cần đảm bảo DataGridView đã xóa hết các cột cũ hoặc đặt AutoGenerateColumns = true
                // Code này sẽ định dạng lại tiêu đề cột:
                if (dgvPhuCap.Columns.Contains("IDPC")) dgvPhuCap.Columns["IDPC"].Visible = false;
                if (dgvPhuCap.Columns.Contains("MANV")) dgvPhuCap.Columns["MANV"].HeaderText = "Mã NV";
                if (dgvPhuCap.Columns.Contains("TenNhanVien")) dgvPhuCap.Columns["TenNhanVien"].HeaderText = "Tên Nhân Viên";
                if (dgvPhuCap.Columns.Contains("TENPC")) dgvPhuCap.Columns["TENPC"].HeaderText = "Tên Phụ Cấp";
                if (dgvPhuCap.Columns.Contains("SOTIEN")) dgvPhuCap.Columns["SOTIEN"].HeaderText = "Số Tiền";
                if (dgvPhuCap.Columns.Contains("NGAYBATDAU")) dgvPhuCap.Columns["NGAYBATDAU"].HeaderText = "Ngày Bắt Đầu";
                if (dgvPhuCap.Columns.Contains("GHICHU")) dgvPhuCap.Columns["GHICHU"].HeaderText = "Ghi Chú";


                // --- CẤU HÌNH LỆNH INSERT ---
                string sqlInsert = @"INSERT INTO TB_PHUCAP (MANV, TENPC, SOTIEN, NGAYBATDAU, GHICHU) 
                                     VALUES (@MANV, @TENPC, @SOTIEN, @NGAYBATDAU, @GHICHU);
                                     SELECT SCOPE_IDENTITY();"; // Lấy ID vừa sinh ra
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.Add("@MANV", SqlDbType.VarChar, 10, "MANV");
                cmdInsert.Parameters.Add("@TENPC", SqlDbType.NVarChar, 100, "TENPC");
                cmdInsert.Parameters.Add("@SOTIEN", SqlDbType.Decimal, 18, "SOTIEN");
                cmdInsert.Parameters.Add("@NGAYBATDAU", SqlDbType.Date, 0, "NGAYBATDAU");
                cmdInsert.Parameters.Add("@GHICHU", SqlDbType.NVarChar, 500, "GHICHU");
                daPhuCap.InsertCommand = cmdInsert;
                // Dòng này quan trọng để DataAdapter cập nhật lại IDPC vào DataSet sau khi Insert
                daPhuCap.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;


                // --- CẤU HÌNH LỆNH UPDATE ---
                string sqlUpdate = @"UPDATE TB_PHUCAP SET 
                                        MANV = @MANV, 
                                        TENPC = @TENPC, 
                                        SOTIEN = @SOTIEN, 
                                        NGAYBATDAU = @NGAYBATDAU, 
                                        GHICHU = @GHICHU 
                                     WHERE IDPC = @IDPC";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.Add("@MANV", SqlDbType.VarChar, 10, "MANV");
                cmdUpdate.Parameters.Add("@TENPC", SqlDbType.NVarChar, 100, "TENPC");
                cmdUpdate.Parameters.Add("@SOTIEN", SqlDbType.Decimal, 18, "SOTIEN");
                cmdUpdate.Parameters.Add("@NGAYBATDAU", SqlDbType.Date, 0, "NGAYBATDAU");
                cmdUpdate.Parameters.Add("@GHICHU", SqlDbType.NVarChar, 500, "GHICHU");
                // Tham số IDPC dùng cho WHERE (dùng phiên bản gốc Original)
                cmdUpdate.Parameters.Add("@IDPC", SqlDbType.Int, 0, "IDPC").SourceVersion = DataRowVersion.Original;
                daPhuCap.UpdateCommand = cmdUpdate;


                // --- CẤU HÌNH LỆNH DELETE ---
                string sqlDelete = "DELETE FROM TB_PHUCAP WHERE IDPC = @IDPC";
                SqlCommand cmdDelete = new SqlCommand(sqlDelete, conn);
                cmdDelete.Parameters.Add("@IDPC", SqlDbType.Int, 0, "IDPC").SourceVersion = DataRowVersion.Original;
                daPhuCap.DeleteCommand = cmdDelete;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
    }
}
