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
    public partial class UC_NghiPhepNV : UserControl
    {
        // --- 1. KHAI BÁO BIẾN ---
        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daNhanVien;
        SqlDataAdapter daNghiPhep;

        // Chuỗi kết nối chuẩn của bạn
        string connectionString = @"Data Source=TUNG-IT\MSSQL_EXP_2008R2;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        public UC_NghiPhepNV()
        {
            InitializeComponent();
        }
        public event EventHandler DataSaved;
        // --- 2. HÀM LOAD FORM (QUAN TRỌNG NHẤT) ---
        private void UC_NghiPhepNV_Load(object sender, EventArgs e)
        {
            // Chặn lỗi Designer (Bắt buộc)
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                // A. Tải ComboBox Nhân Viên
                string sqlNV = "SELECT MANV, HOTEN FROM TB_NHANVIEN";
                daNhanVien = new SqlDataAdapter(sqlNV, conn);
                daNhanVien.Fill(ds, "tblNhanVien");

                cboChonNhanVien.DataSource = ds.Tables["tblNhanVien"];
                cboChonNhanVien.DisplayMember = "HOTEN";
                cboChonNhanVien.ValueMember = "MANV";
                cboChonNhanVien.SelectedIndex = -1;

                // B. Tải Dữ liệu Nghỉ Phép và Cấu hình Lệnh (Commands)
                // Lưu ý: TrangThai lấy về là số (0,1,2)
                string sqlNP = @"SELECT np.ID, np.MANV, np.NgayBatDau, np.SoNgayNghi, np.LyDo, np.TrangThai, nv.HOTEN 
                                 FROM tb_NGHIPHEP np 
                                 LEFT JOIN TB_NHANVIEN nv ON np.MANV = nv.MANV";

                daNghiPhep = new SqlDataAdapter(sqlNP, conn);

                // --- CẤU HÌNH LỆNH INSERT (THÊM) ---
                string sThem = @"INSERT INTO tb_NGHIPHEP (MANV, NgayBatDau, SoNgayNghi, LyDo, TrangThai) 
                                 VALUES (@MANV, @NgayBatDau, @SoNgayNghi, @LyDo, @TrangThai)";
                SqlCommand cmdThem = new SqlCommand(sThem, conn);
                cmdThem.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
                cmdThem.Parameters.Add("@NgayBatDau", SqlDbType.Date, 8, "NgayBatDau");
                cmdThem.Parameters.Add("@SoNgayNghi", SqlDbType.Int, 4, "SoNgayNghi");
                cmdThem.Parameters.Add("@LyDo", SqlDbType.NVarChar, 200, "LyDo");
                cmdThem.Parameters.Add("@TrangThai", SqlDbType.Int, 4, "TrangThai");
                daNghiPhep.InsertCommand = cmdThem;

                // --- CẤU HÌNH LỆNH UPDATE (SỬA) ---
                // Quan trọng: Phải có WHERE ID = @ID
                string sSua = @"UPDATE tb_NGHIPHEP 
                                SET MANV=@MANV, NgayBatDau=@NgayBatDau, SoNgayNghi=@SoNgayNghi, LyDo=@LyDo, TrangThai=@TrangThai 
                                WHERE ID=@ID";
                SqlCommand cmdSua = new SqlCommand(sSua, conn);
                cmdSua.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
                cmdSua.Parameters.Add("@NgayBatDau", SqlDbType.Date, 8, "NgayBatDau");
                cmdSua.Parameters.Add("@SoNgayNghi", SqlDbType.Int, 4, "SoNgayNghi");
                cmdSua.Parameters.Add("@LyDo", SqlDbType.NVarChar, 200, "LyDo");
                cmdSua.Parameters.Add("@TrangThai", SqlDbType.Int, 4, "TrangThai");

                // Tham số ID dùng để tìm dòng cũ
                SqlParameter paramID = cmdSua.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
                paramID.SourceVersion = DataRowVersion.Original; // Quan trọng: Lấy ID gốc trước khi sửa

                daNghiPhep.UpdateCommand = cmdSua;

                // --- CẤU HÌNH LỆNH DELETE (XÓA) ---
                string sXoa = @"DELETE FROM tb_NGHIPHEP WHERE ID=@ID";
                SqlCommand cmdXoa = new SqlCommand(sXoa, conn);
                SqlParameter paramIDXoa = cmdXoa.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
                paramIDXoa.SourceVersion = DataRowVersion.Original; // Quan trọng: Lấy ID gốc
                daNghiPhep.DeleteCommand = cmdXoa;

                // C. Đổ dữ liệu vào DataSet
                daNghiPhep.Fill(ds, "tblNghiPhep");

                // Thiết lập Khóa Chính (Cực kỳ quan trọng để hàm Find hoạt động)
                ds.Tables["tblNghiPhep"].PrimaryKey = new DataColumn[] { ds.Tables["tblNghiPhep"].Columns["ID"] };

                // Cấu hình ID tự tăng trong RAM (Để khi thêm mới không bị trùng ID tạm)
                ds.Tables["tblNghiPhep"].Columns["ID"].AutoIncrement = true;
                ds.Tables["tblNghiPhep"].Columns["ID"].AutoIncrementSeed = -1;
                ds.Tables["tblNghiPhep"].Columns["ID"].AutoIncrementStep = -1;

                // Thêm cột hiển thị trạng thái bằng chữ (Cột ảo, không lưu xuống SQL)
                if (!ds.Tables["tblNghiPhep"].Columns.Contains("TenTrangThai"))
                {
                    ds.Tables["tblNghiPhep"].Columns.Add("TenTrangThai", typeof(string));
                }

                // Cập nhật text cho cột ảo
                RefreshTrangThaiText();

                dgvNghiPhep.DataSource = ds.Tables["tblNghiPhep"];
                FormatLuoi();
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv")
                    MessageBox.Show("Lỗi khởi động: " + ex.Message);
            }
        }

        // --- 3. CÁC HÀM XỬ LÝ LOGIC ---

        // Hàm chuyển đổi số 0,1,2 thành chữ "Chờ duyệt" để hiển thị
        private void RefreshTrangThaiText()
        {
            foreach (DataRow dr in ds.Tables["tblNghiPhep"].Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    int stt = (dr["TrangThai"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["TrangThai"]);
                    if (stt == 0) dr["TenTrangThai"] = "Chờ duyệt";
                    else if (stt == 1) dr["TenTrangThai"] = "Đã duyệt";
                    else if (stt == 2) dr["TenTrangThai"] = "Từ chối";
                }
            }
        }

        private void FormatLuoi()
        {
            // Ẩn cột ID và cột số TrangThai, chỉ hiện cột chữ TenTrangThai
            if (dgvNghiPhep.Columns["ID"] != null) dgvNghiPhep.Columns["ID"].Visible = false;
            if (dgvNghiPhep.Columns["TrangThai"] != null) dgvNghiPhep.Columns["TrangThai"].Visible = false;
            if (dgvNghiPhep.Columns["TenTrangThai"] != null)
            {
                dgvNghiPhep.Columns["TenTrangThai"].HeaderText = "Trạng Thái";
                dgvNghiPhep.Columns["TenTrangThai"].DisplayIndex = dgvNghiPhep.Columns.Count - 1;
            }
        }

        private void LamMoi()
        {
            cboChonNhanVien.SelectedIndex = -1;
            txtLyDo.Text = "";
            txtSoNgayNghi.Text = "";
            dtpNgayBatDau.Value = DateTime.Now;
            dgvNghiPhep.ClearSelection();
            this.Tag = null; // Xóa ID đang nhớ
        }

        // --- 4. CÁC SỰ KIỆN NÚT BẤM ---

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1) { MessageBox.Show("Chưa chọn nhân viên"); return; }
            if (txtSoNgayNghi.Text == "") { MessageBox.Show("Chưa nhập số ngày"); return; }

            // Thêm dòng mới vào RAM
            DataRow row = ds.Tables["tblNghiPhep"].NewRow();
            row["MANV"] = cboChonNhanVien.SelectedValue;
            row["HOTEN"] = cboChonNhanVien.Text;
            row["NgayBatDau"] = dtpNgayBatDau.Value;

            int soNgay = 0;
            int.TryParse(txtSoNgayNghi.Text, out soNgay);
            row["SoNgayNghi"] = soNgay;

            row["LyDo"] = txtLyDo.Text;
            row["TrangThai"] = 0; // 0 = Chờ duyệt
            row["TenTrangThai"] = "Chờ duyệt"; // Hiển thị ngay

            ds.Tables["tblNghiPhep"].Rows.Add(row);

            MessageBox.Show("Đã thêm vào danh sách. Bấm LƯU để ghi vào CSDL.");
            LamMoi();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (this.Tag == null) { MessageBox.Show("Vui lòng chọn dòng cần sửa!"); return; }

            int id = Convert.ToInt32(this.Tag);
            DataRow row = ds.Tables["tblNghiPhep"].Rows.Find(id);

            if (row != null)
            {
                row.BeginEdit();
                row["MANV"] = cboChonNhanVien.SelectedValue;
                row["HOTEN"] = cboChonNhanVien.Text;
                row["NgayBatDau"] = dtpNgayBatDau.Value;

                int soNgay = 0;
                int.TryParse(txtSoNgayNghi.Text, out soNgay);
                row["SoNgayNghi"] = soNgay;

                row["LyDo"] = txtLyDo.Text;
                // Khi sửa thông tin, có thể giữ nguyên trạng thái cũ hoặc reset về chờ duyệt tùy bạn
                // Ở đây mình giữ nguyên trạng thái cũ
                row.EndEdit();

                RefreshTrangThaiText(); // Cập nhật lại hiển thị
                MessageBox.Show("Đã sửa thông tin. Bấm LƯU để ghi vào CSDL.");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (this.Tag == null) { MessageBox.Show("Vui lòng chọn dòng cần xóa!"); return; }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(this.Tag);
                DataRow row = ds.Tables["tblNghiPhep"].Rows.Find(id);

                if (row != null)
                {
                    row.Delete(); // Đánh dấu dòng này là đã xóa (chưa mất hẳn, chờ Lưu)
                    MessageBox.Show("Đã xóa khỏi danh sách. Bấm LƯU để hoàn tất.");
                    LamMoi();
                }
            }
        }

        // --- NÚT LƯU (QUAN TRỌNG NHẤT) ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Đẩy toàn bộ thay đổi (Thêm/Sửa/Xóa) xuống SQL
                daNghiPhep.Update(ds, "tblNghiPhep");

                MessageBox.Show("Đã lưu thành công vào CSDL!");

                // Tải lại dữ liệu để cập nhật ID thật từ SQL (thay thế ID tạm -1, -2...)
                ds.Tables["tblNghiPhep"].Clear();
                daNghiPhep.Fill(ds, "tblNghiPhep");
                RefreshTrangThaiText(); // Cập nhật lại chữ hiển thị
                DataSaved?.Invoke(this, EventArgs.Empty);
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                // Nếu lỗi thì hoàn tác để dữ liệu trên lưới khớp với database
                ds.Tables["tblNghiPhep"].RejectChanges();
            }
        }

        // --- CÁC NÚT DUYỆT / TỪ CHỐI ---
        private void CapNhatTrangThai(int trangThaiMoi, string tenTrangThai)
        {
            if (this.Tag == null) { MessageBox.Show("Chọn đơn cần xử lý!"); return; }

            int id = Convert.ToInt32(this.Tag);
            DataRow row = ds.Tables["tblNghiPhep"].Rows.Find(id);

            if (row != null)
            {
                row.BeginEdit();
                row["TrangThai"] = trangThaiMoi; // 1 hoặc 2
                row["TenTrangThai"] = tenTrangThai;
                row.EndEdit();

                // Tự động lưu luôn khi Duyệt (cho tiện)
                try
                {
                    daNghiPhep.Update(ds, "tblNghiPhep");
                    MessageBox.Show("Đã cập nhật trạng thái: " + tenTrangThai);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            CapNhatTrangThai(1, "Đã duyệt");
        }

        private void btnTuChoi_Click(object sender, EventArgs e)
        {
            CapNhatTrangThai(2, "Từ chối");
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void dgvNghiPhep_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvNghiPhep.Rows.Count - 1)
            {
                try
                {
                    DataGridViewRow row = dgvNghiPhep.Rows[e.RowIndex];

                    if (row.Cells["MANV"].Value != DBNull.Value)
                        cboChonNhanVien.SelectedValue = row.Cells["MANV"].Value;

                    txtLyDo.Text = row.Cells["LyDo"].Value?.ToString() ?? "";
                    txtSoNgayNghi.Text = row.Cells["SoNgayNghi"].Value?.ToString() ?? "";

                    if (row.Cells["NgayBatDau"].Value != DBNull.Value)
                        dtpNgayBatDau.Value = Convert.ToDateTime(row.Cells["NgayBatDau"].Value);

                    // Lưu ID vào Tag
                    this.Tag = row.Cells["ID"].Value;
                }
                catch { }
            }
        }
    }
}