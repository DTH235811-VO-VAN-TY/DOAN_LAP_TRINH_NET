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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace QuanLyNhanSU
{
    public partial class UC_BaoHiemNV : UserControl
    {
        //(Kết nối bằng tên Server SQL)
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daNhanVien;
        SqlDataAdapter daBaoHiem;

        public UC_BaoHiemNV()
        {
            InitializeComponent();
        }

        private void UC_BaoHiemNV_Load(object sender, EventArgs e)
        {
            LoadSearchComboBox();
            if (this.DesignMode) return;
            try
            {
                dgvBaoHiem.AutoGenerateColumns = false;
                dgvBaoHiem.RowHeadersVisible = false;
                dgvBaoHiem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // (Nếu bạn có TextBox txtIdBH, hãy đặt nó ở chế độ chỉ đọc)
                // txtIdBH.ReadOnly = true; 

                conn = new SqlConnection(connString);
                conn.Open();

                //Dữ liệu Combobox Nhân Viên (Giữ nguyên, dùng để Thêm/Sửa)
                string sQueryNhanVien = @"SELECT * FROM TB_NHANVIEN";
                daNhanVien = new SqlDataAdapter(sQueryNhanVien, conn);
                daNhanVien.Fill(ds, "tblNHANVIEN");
                cboChonNhanVien.DataSource = ds.Tables["tblNHANVIEN"];
                cboChonNhanVien.DisplayMember = "HOTEN";
                cboChonNhanVien.ValueMember = "MANV";
                cboChonNhanVien.SelectedIndex = -1; // Xóa chọn mặc định

                // --- SỬA 1: Bỏ 'WHERE bh.MANV = @MANV' để tải TẤT CẢ ---
                string sQueryBaoHiem = @"SELECT bh.*, nv.HOTEN 
                                         FROM TB_BAOHIEM bh
                                         LEFT JOIN TB_NHANVIEN nv ON bh.MANV = nv.MANV";
                daBaoHiem = new SqlDataAdapter(sQueryBaoHiem, conn);

                // --- SỬA 2: Xóa 2 dòng này, không cần lọc nữa ---
                // daBaoHiem.SelectCommand.Parameters.Add("@MANV", SqlDbType.NVarChar, 10);
                // cboChonNhanVien.SelectedIndexChanged += ... (Xóa dòng này trong code cũ của bạn)

                // --- SỬA 3: Tải DGV ngay lập tức ---
                daBaoHiem.Fill(ds, "tblBAOHIEM"); // Thay vì FillSchema
                dgvBaoHiem.DataSource = ds.Tables["tblBAOHIEM"];


                // Gán DataPropertyName (Kiểm tra lại tên cột (Name) của bạn trong Designer)
                dgvBaoHiem.Columns["IdBH"].DataPropertyName = "IDBH";
                dgvBaoHiem.Columns["TenNhanVien"].DataPropertyName = "HOTEN";
                dgvBaoHiem.Columns["SoBaoHiem"].DataPropertyName = "SOBH";
                dgvBaoHiem.Columns["ngayCapBH"].DataPropertyName = "NGAYCAP";
                dgvBaoHiem.Columns["NoiCapBH"].DataPropertyName = "NOICAP";
                dgvBaoHiem.Columns["NoiKhamBenh"].DataPropertyName = "NOIKHAMBENH";


                // --- Code InsertCommand (Đã sửa không chèn IDBH) ---
                string sThemBH = @"INSERT INTO TB_BAOHIEM (SOBH, NGAYCAP, NOICAP, NOIKHAMBENH, MANV) " +
                                 "VALUES (@SOBH, @NGAYCAP, @NOICAP, @NOIKHAMBENH, @MANV)";
                SqlCommand cmdThemBH = new SqlCommand(sThemBH, conn);
                cmdThemBH.Parameters.Add("@SOBH", SqlDbType.NVarChar, 50, "SOBH");
                cmdThemBH.Parameters.Add("@NGAYCAP", SqlDbType.Date, 8, "NGAYCAP");
                cmdThemBH.Parameters.Add("@NOICAP", SqlDbType.NVarChar, 100, "NOICAP");
                cmdThemBH.Parameters.Add("@NOIKHAMBENH", SqlDbType.NVarChar, 100, "NOIKHAMBENH");
                cmdThemBH.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
                daBaoHiem.InsertCommand = cmdThemBH;

                //---Comand sửa bảo hiểm (Code này đã đúng)
                string sSuaBH = @"UPDATE TB_BAOHIEM SET SOBH=@SOBH, NGAYCAP=@NGAYCAP, NOICAP=@NOICAP, NOIKHAMBENH=@NOIKHAMBENH, MANV=@MANV WHERE IDBH=@IDBH";
                SqlCommand cmdSuaBH = new SqlCommand(sSuaBH, conn);
                cmdSuaBH.Parameters.Add("@SOBH", SqlDbType.NVarChar, 50, "SOBH");
                cmdSuaBH.Parameters.Add("@NGAYCAP", SqlDbType.Date, 8, "NGAYCAP");
                cmdSuaBH.Parameters.Add("@NOICAP", SqlDbType.NVarChar, 100, "NOICAP");
                cmdSuaBH.Parameters.Add("@NOIKHAMBENH", SqlDbType.NVarChar, 100, "NOIKHAMBENH");
                cmdSuaBH.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
                cmdSuaBH.Parameters.Add("@IDBH", SqlDbType.Int, 4, "IDBH");
                daBaoHiem.UpdateCommand = cmdSuaBH;

                //---Comand xóa bảo hiểm (Code này đã đúng)
                string sXoaBH = @"DELETE FROM TB_BAOHIEM WHERE IDBH=@IDBH";
                SqlCommand cmdXoaBH = new SqlCommand(sXoaBH, conn);
                cmdXoaBH.Parameters.Add("@IDBH", SqlDbType.Int, 4, "IDBH");
                daBaoHiem.DeleteCommand = cmdXoaBH;

                DataColumn pk = ds.Tables["tblBAOHIEM"].Columns["IDBH"];
                pk.AutoIncrement = true;
                pk.AutoIncrementSeed = -1;
                pk.AutoIncrementStep = -1;

                ds.Tables["tblBAOHIEM"].PrimaryKey = new DataColumn[] { ds.Tables["tblBAOHIEM"].Columns["IDBH"] };

                // --- SỬA 4: Xóa khối logic tải DGV lần đầu, vì đã tải ở trên ---
                // if (cboChonNhanVien.Items.Count > 0) ... (Xóa khối này)
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI GỐC KHI TẢI FORM: " + ex.Message, "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- SỬA 5: XÓA HOÀN TOÀN hàm 'cboChonNhanVien_SelectedIndexChanged' ---
        // (Vì chúng ta không lọc DGV nữa)


        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtSoBH.Text == "" || txtNoiCap.Text == "" || txtNoiKham.Text == "" || cboChonNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên và nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataRow newRow = ds.Tables["tblBAOHIEM"].NewRow();

            newRow["SOBH"] = txtSoBH.Text;
            newRow["NGAYCAP"] = dtpNgayCap.Value;
            newRow["NOICAP"] = txtNoiCap.Text;
            newRow["NOIKHAMBENH"] = txtNoiKham.Text;
            newRow["MANV"] = cboChonNhanVien.SelectedValue;
            newRow["HOTEN"] = cboChonNhanVien.Text;
            ds.Tables["tblBAOHIEM"].Rows.Add(newRow);
            MessageBox.Show("Đã thêm vào bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");

            LamMoiControls();
        }

        private void dgvBaoHiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBaoHiem.SelectedRows.Count > 0)
                {
                    DataGridViewRow dr = dgvBaoHiem.SelectedRows[0];

                    // Lấy DataRowView từ hàng được chọn
                    DataRowView drv = dr.DataBoundItem as DataRowView;
                    if (drv == null) return;

                    // --- SỬA 6: Cập nhật ComboBox khi chọn DGV ---
                    // Lấy MANV từ dữ liệu (ngay cả khi nó bị ẩn)
                    cboChonNhanVien.SelectedValue = drv["MANV"];

                    // (Nếu bạn có TextBox txtIdBH)
                    // txtIdBH.Text = dr.Cells["IdBH"].Value.ToString();
                    txtIdBH.Enabled = false; // Chỉ đọc
                    txtSoBH.Text = dr.Cells["SoBaoHiem"].Value.ToString();
                    dtpNgayCap.Value = Convert.ToDateTime(dr.Cells["ngayCapBH"].Value);
                    txtNoiCap.Text = dr.Cells["NoiCapBH"].Value.ToString();
                    txtNoiKham.Text = dr.Cells["NoiKhamBenh"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn hàng: " + ex.Message);
            }
        }

        private void LamMoiControls()
        {
            // (Nếu bạn có TextBox txtIdBH)
            // txtIdBH.Text = "";
            txtSoBH.Text = "";
            txtNoiCap.Text = "";
            txtNoiKham.Text = "";
            dtpNgayCap.Value = DateTime.Now;
            cboChonNhanVien.SelectedIndex = -1; // Xóa chọn ComboBox
            dgvBaoHiem.ClearSelection();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvBaoHiem.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa.");
                return;
            }

            if (cboChonNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên.");
                return;
            }

            int idbh = Convert.ToInt32(dgvBaoHiem.SelectedRows[0].Cells["IdBH"].Value);
            DataRow rowToUpdate = ds.Tables["tblBAOHIEM"].Rows.Find(idbh);

            if (rowToUpdate != null)
            {
                rowToUpdate.BeginEdit();
                rowToUpdate["SOBH"] = txtSoBH.Text;
                rowToUpdate["NGAYCAP"] = dtpNgayCap.Value;
                rowToUpdate["NOICAP"] = txtNoiCap.Text;
                rowToUpdate["NOIKHAMBENH"] = txtNoiKham.Text;
                rowToUpdate["MANV"] = cboChonNhanVien.SelectedValue;
                rowToUpdate["HOTEN"] = cboChonNhanVien.Text;
                rowToUpdate.EndEdit();
                MessageBox.Show("Đã sửa trong bộ nhớ. Nhấn 'Lưu' để cập nhật CSDL.");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvBaoHiem.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi bảo hiểm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            int idbh = Convert.ToInt32(dgvBaoHiem.SelectedRows[0].Cells["IdBH"].Value);
            DataRow rowToDelete = ds.Tables["tblBAOHIEM"].Rows.Find(idbh);
            if (rowToDelete != null)
            {
                rowToDelete.Delete();
                MessageBox.Show("Đã xóa trong bộ nhớ. Nhấn 'Lưu' để cập nhật CSDL.");
                LamMoiControls();
            }
        }

        // Nút 'Lưu'
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = daBaoHiem.Update(ds, "tblBAOHIEM");
                MessageBox.Show($"Đã lưu {rowsAffected} thay đổi vào CSDL thành công!");

                // --- SỬA 7: Tải lại DGV bằng cách Fill lại ---
                ds.Tables["tblBAOHIEM"].Clear();
                daBaoHiem.Fill(ds, "tblBAOHIEM");

                LamMoiControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ds.Tables["tblBAOHIEM"].RejectChanges();
            }
        }

        // Nút 'Làm mới' (Nếu bạn có nút này riêng)
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiControls();

            // Hủy các thay đổi chưa lưu
            ds.Tables["tblBAOHIEM"].RejectChanges();
        }
        private void LoadSearchComboBox()
        {
            var searchOptions = new[] {
                new { Text = "Tìm theo Mã BH", Value = "IDBH" },
                new { Text = "Tìm theo Họ Tên", Value = "HOTEN" }
            };
            // (Sửa lại tên 'cboTimKiemTheo' nếu tên của bạn khác)
            cboTKtenBH.DataSource = searchOptions;
            cboTKtenBH.DisplayMember = "Text";
            cboTKtenBH.ValueMember = "Value";
            cboTKtenBH.SelectedIndex = 0;
        }
        private void btnTkBH_Click(object sender, EventArgs e)
        {
            string searchTerm = txtTimKiemBH.Text.Trim().Replace("'", "''");
            string searchColumn = cboTKtenBH.SelectedValue.ToString();

            DataView dv = ds.Tables["tblBAOHIEM"].DefaultView;

            if (string.IsNullOrEmpty(searchTerm))
            {
                dv.RowFilter = string.Empty;
                return;
            }

            // Kiểm tra kiểu dữ liệu của cột
            Type columnType = ds.Tables["tblBAOHIEM"].Columns[searchColumn].DataType;

            try
            {
                if (columnType == typeof(string))
                {
                    // Nếu là chuỗi → dùng LIKE
                    dv.RowFilter = $"{searchColumn} LIKE '%{searchTerm}%'";
                }
                else if (columnType == typeof(int) || columnType == typeof(double) || columnType == typeof(float))
                {
                    // Nếu là số → dùng so sánh chính xác
                    dv.RowFilter = $"{searchColumn} = {searchTerm}";
                }
                else
                {
                    // Nếu kiểu khác → xử lý tùy trường hợp
                    MessageBox.Show("Không hỗ trợ tìm kiếm với kiểu dữ liệu này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Giá trị tìm kiếm không hợp lệ với kiểu dữ liệu của cột", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnHienAllBH_Click(object sender, EventArgs e)
        {
            txtTimKiemBH.Text = "";
            ds.Tables["tblBAOHIEM"].DefaultView.RowFilter = string.Empty;
        }
    }
}