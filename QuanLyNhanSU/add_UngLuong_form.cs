using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class add_UngLuong_form : Form
    {
        // 1. Cấu hình chuỗi kết nối
        string strKetNoi = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        SqlConnection conn = null;
        DataSet ds = new DataSet();

        SqlDataAdapter daNhanVien; // Để load combobox
        SqlDataAdapter daUngLuong; // Để xử lý chính

        public add_UngLuong_form()
        {
            InitializeComponent();
        }

        // Sự kiện Load Form
        private void add_UngLuong_form_Load(object sender, EventArgs e)
        {
            LoadData();
            LockControl(true); // Khóa các ô nhập khi mới mở
        }

        private void LoadData()
        {
            try
            {
                conn = new SqlConnection(strKetNoi);
                conn.Open();

                // --- A. LOAD COMBOBOX NHÂN VIÊN ---
                string sqlNV = "SELECT * FROM TB_NHANVIEN";
                daNhanVien = new SqlDataAdapter(sqlNV, conn);
                daNhanVien.Fill(ds, "tblNHANVIEN");

                cboMaNV.DataSource = ds.Tables["tblNHANVIEN"];
                cboMaNV.DisplayMember = "MANV";
                cboMaNV.ValueMember = "MANV";
                cboMaNV.SelectedIndex = -1;

                // --- B. CẤU HÌNH ADAPTER ỨNG LƯƠNG (Quan trọng nhất) ---

                // 1. Select Command (Lấy cả Tên NV để hiện lên lưới)
                // Giả sử bảng TB_UNGLUONG có các cột: ID, MANV, NAM, THANG, NGAY, SOTIEN, GHICHU (nếu có), TRANGTHAI
                string sqlSelect = @"SELECT ul.ID, ul.MANV, nv.HOTEN, ul.NGAY, ul.SOTIEN, ul.GHICHU 
                                     FROM TB_UNGLUONG ul 
                                     LEFT JOIN TB_NHANVIEN nv ON ul.MANV = nv.MANV 
                                     ORDER BY ul.NGAY DESC";

                daUngLuong = new SqlDataAdapter(sqlSelect, conn);

                // 2. Insert Command
                // Lưu ý: Tự động tách YEAR và MONTH từ tham số @NGAY
                string sqlInsert = @"INSERT INTO TB_UNGLUONG (MANV, NGAY, NAM, THANG, SOTIEN, GHICHU, TRANGTHAI) 
                                     VALUES (@MANV, @NGAY, YEAR(@NGAY), MONTH(@NGAY), @SOTIEN, @GHICHU, 1)";

                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.Add("@MANV", SqlDbType.NVarChar, 50, "MANV");
                cmdInsert.Parameters.Add("@NGAY", SqlDbType.DateTime, 8, "NGAY");
                cmdInsert.Parameters.Add("@SOTIEN", SqlDbType.Float, 8, "SOTIEN"); // Hoặc Decimal tùy DB
                cmdInsert.Parameters.Add("@GHICHU", SqlDbType.NVarChar, 200, "GHICHU");
                daUngLuong.InsertCommand = cmdInsert;

                // 3. Update Command
                string sqlUpdate = @"UPDATE TB_UNGLUONG 
                                     SET MANV=@MANV, NGAY=@NGAY, NAM=YEAR(@NGAY), THANG=MONTH(@NGAY), 
                                         SOTIEN=@SOTIEN, GHICHU=@GHICHU 
                                     WHERE ID=@ID";

                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.Add("@MANV", SqlDbType.NVarChar, 50, "MANV");
                cmdUpdate.Parameters.Add("@NGAY", SqlDbType.DateTime, 8, "NGAY");
                cmdUpdate.Parameters.Add("@SOTIEN", SqlDbType.Float, 8, "SOTIEN");
                cmdUpdate.Parameters.Add("@GHICHU", SqlDbType.NVarChar, 200, "GHICHU");
                cmdUpdate.Parameters.Add("@ID", SqlDbType.Int, 4, "ID"); // Khóa chính để tìm dòng update
                daUngLuong.UpdateCommand = cmdUpdate;

                // 4. Delete Command
                string sqlDelete = @"DELETE FROM TB_UNGLUONG WHERE ID=@ID";
                SqlCommand cmdDelete = new SqlCommand(sqlDelete, conn);
                cmdDelete.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
                daUngLuong.DeleteCommand = cmdDelete;

                // --- C. TẢI DỮ LIỆU VÀO DATASET ---
                daUngLuong.Fill(ds, "tblUNGLUONG");

                // Thiết lập Khóa chính cho bảng trong bộ nhớ (để tìm kiếm Sửa/Xóa nhanh hơn)
                DataColumn pk = ds.Tables["tblUNGLUONG"].Columns["ID"];
                pk.AutoIncrement = true;
                pk.AutoIncrementSeed = -1;
                pk.AutoIncrementStep = -1;
                ds.Tables["tblUNGLUONG"].PrimaryKey = new DataColumn[] { pk };

                // --- D. GÁN DỮ LIỆU LÊN LƯỚI ---
                dgvUngluong.AutoGenerateColumns = false; // Tắt tự động tạo cột để dùng cột bạn đã thiết kế
                dgvUngluong.DataSource = ds.Tables["tblUNGLUONG"];

                // Mapping: Tên cột trong Grid (Name) -> Tên cột trong SQL
                // Bạn cần kiểm tra kỹ (Name) trong file Designer của bạn có khớp không nhé
                dgvUngluong.Columns["Manhanvien"].DataPropertyName = "MANV";
                dgvUngluong.Columns["Tennhanvien"].DataPropertyName = "HOTEN";
                dgvUngluong.Columns["Ngayungluong"].DataPropertyName = "NGAY";
                dgvUngluong.Columns["Sotienung"].DataPropertyName = "SOTIEN";
                dgvUngluong.Columns["Ghichu"].DataPropertyName = "GHICHU";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // Hàm điều khiển trạng thái nút (giống UC_BaoHiem)
        private void LockControl(bool lockState)
        {
            // True: Đang xem (Khóa nhập), False: Đang sửa (Mở nhập)
            cboMaNV.Enabled = !lockState;
            txtSoTienung.Enabled = !lockState;
            txtGhichu.Enabled = !lockState;
            dtpNgayungluong.Enabled = !lockState;

            // Khóa text Tên NV luôn (vì tự động load)
            txtTenNV.Enabled = false;

            btnThem.Enabled = lockState;
            btnSua.Enabled = lockState;
            btnXoa.Enabled = lockState;

            btnLuu.Enabled = !lockState; // Chỉ sáng khi đang nhập liệu
            btnLamMoi.Enabled = true;
        }

        private void ResetInput()
        {
            cboMaNV.SelectedIndex = -1;
            txtTenNV.Clear();
            txtSoTienung.Clear();
            txtGhichu.Clear();
            dtpNgayungluong.Value = DateTime.Now;
            dgvUngluong.ClearSelection();
        }

        // Xử lý khi chọn 1 dòng trên lưới
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvUngluong.Rows[e.RowIndex];

                    // Lấy DataRowView từ dòng được chọn (Dữ liệu gốc)
                    DataRowView drv = row.DataBoundItem as DataRowView;
                    if (drv == null) return;

                    // Đổ dữ liệu lên form
                    cboMaNV.SelectedValue = drv["MANV"].ToString();
                    // TxtTenNV sẽ tự nhảy nhờ sự kiện cbo_SelectedIndexChanged

                    if (drv["NGAY"] != DBNull.Value)
                        dtpNgayungluong.Value = Convert.ToDateTime(drv["NGAY"]);

                    if (drv["SOTIEN"] != DBNull.Value)
                    {
                        // Format số tiền cho đẹp nếu cần, nhưng khi gán vào textbox để sửa thì nên để số thô
                        txtSoTienung.Text = drv["SOTIEN"].ToString();
                    }

                    if (ds.Tables["tblUNGLUONG"].Columns.Contains("GHICHU") && drv["GHICHU"] != DBNull.Value)
                        txtGhichu.Text = drv["GHICHU"].ToString();
                    else
                        txtGhichu.Text = "";

                    // Mở nút Sửa/Xóa
                    LockControl(true);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi chọn dòng: " + ex.Message); }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetInput();
            LockControl(false); // Mở khóa để nhập
            cboMaNV.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvUngluong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa!");
                return;
            }
            LockControl(false); // Mở khóa để sửa
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvUngluong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chưa chọn dòng để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa phiếu ứng lương này?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Xóa trong DataTable (Bộ nhớ đệm)
                    int id = Convert.ToInt32(dgvUngluong.SelectedRows[0].Cells["ID"].Value); // Đảm bảo Grid có cột ID (ẩn cũng được) hoặc lấy từ DataRowView

                    // Cách lấy ID an toàn hơn từ DataRowView
                    DataRowView drv = dgvUngluong.SelectedRows[0].DataBoundItem as DataRowView;
                    if (drv != null)
                    {
                        drv.Row.Delete(); // Đánh dấu là đã xóa
                        MessageBox.Show("Đã xóa trong bộ nhớ tạm. Bấm LƯU để cập nhật CSDL.");
                        ResetInput();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Lỗi xóa: " + ex.Message); }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Validate
            if (cboMaNV.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtSoTienung.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã NV và Số tiền ứng!");
                return;
            }

            // 2. Xử lý dữ liệu vào DataTable
            try
            {
                // A. Trường hợp THÊM MỚI
                // Kiểm tra xem người dùng đang bấm Thêm (không có row nào chọn) hay Sửa
                // Tuy nhiên logic đơn giản nhất: Nếu txtSoTienung đang mở, ta kiểm tra xem có đang chọn row nào để sửa không.

                // Logic chuẩn: Dùng DataRow
                DataRow row;

                // Nếu đang Sửa (có dòng được chọn và không phải là thêm mới)
                // Ở đây tôi dùng logic đơn giản: Nếu grid đang focus dòng nào thì coi như sửa dòng đó, 
                // nhưng nếu vừa bấm nút Thêm (ResetInput) thì grid mất focus.

                // Cách tốt nhất: Kiểm tra ID. Nhưng ở đây dùng DataSet, ta có thể làm như sau:

                if (dgvUngluong.SelectedRows.Count > 0 && dgvUngluong.CurrentRow.DataBoundItem != null)
                {
                    // -- UPDATE --
                    DataRowView drv = dgvUngluong.CurrentRow.DataBoundItem as DataRowView;

                    // Kiểm tra xem dòng này có phải là dòng mới thêm chưa lưu không, hay dòng cũ
                    // Nếu đang ở chế độ thêm mới thì ta đã ClearSelection ở nút Thêm rồi.
                    // Nên nếu có SelectedRows thì là SỬA.

                    // Tuy nhiên để chắc chắn, bạn nên dùng 1 biến cờ isThem như code cũ, hoặc kiểm tra trạng thái
                    // Nhưng theo phong cách UC_BaoHiem, ta thao tác trực tiếp Row.

                    row = drv.Row;
                    row.BeginEdit();
                    row["MANV"] = cboMaNV.SelectedValue;
                    row["HOTEN"] = txtTenNV.Text; // Cập nhật hiển thị ngay
                    row["NGAY"] = dtpNgayungluong.Value;
                    row["SOTIEN"] = Convert.ToDouble(txtSoTienung.Text);
                    row["GHICHU"] = txtGhichu.Text;
                    row.EndEdit();
                }
                else
                {
                    // -- INSERT --
                    row = ds.Tables["tblUNGLUONG"].NewRow();
                    row["MANV"] = cboMaNV.SelectedValue;
                    row["HOTEN"] = txtTenNV.Text; // Cập nhật hiển thị ngay
                    row["NGAY"] = dtpNgayungluong.Value;
                    row["SOTIEN"] = Convert.ToDouble(txtSoTienung.Text);
                    row["GHICHU"] = txtGhichu.Text;
                    ds.Tables["tblUNGLUONG"].Rows.Add(row);
                }

                // 3. ĐẨY DỮ LIỆU XUỐNG SQL (Batch Update)
                int result = daUngLuong.Update(ds, "tblUNGLUONG");

                MessageBox.Show("Đã lưu thành công vào CSDL!");

                // Tải lại dữ liệu để lấy ID mới và đảm bảo đồng bộ
                ds.Tables["tblUNGLUONG"].Clear();
                daUngLuong.Fill(ds, "tblUNGLUONG");

                LockControl(true);
                ResetInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Hủy các thay đổi chưa lưu và tải lại
            ds.Tables["tblUNGLUONG"].RejectChanges();
            ResetInput();
            LockControl(true);
        }

        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedItem != null)
            {
                DataRowView drv = cboMaNV.SelectedItem as DataRowView;
                txtTenNV.Text = drv["HOTEN"].ToString();
            }
            else
            {
                txtTenNV.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Bạn cần sự kiện Load cho Form, hãy vào file Designer hoặc Properties để gán sự kiện này
        // this.Load += new System.EventHandler(this.add_UngLuong_form_Load);
    }
}