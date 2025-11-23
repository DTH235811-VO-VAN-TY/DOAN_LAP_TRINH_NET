using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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

        // Chuỗi kết nối chuẩn máy TUNG-IT
        string connString =  @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        public UC_NghiPhepNV()
        {
            InitializeComponent();
        }

        // --- 2. HÀM LOAD FORM ---
        private void UC_NghiPhepNV_Load(object sender, EventArgs e)
        {
            // Chặn lỗi khi kéo thả trong Designer
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                // A. Tải ComboBox Nhân Viên
                string sqlNV = "SELECT * FROM tb_NHANVIEN WHERE DATHOIVIEC = 0 OR DATHOIVIEC IS NULL";
                daNhanVien = new SqlDataAdapter(sqlNV, conn);
                daNhanVien.Fill(ds, "tblNhanVien");

                cboChonNhanVien.DataSource = ds.Tables["tblNhanVien"];
                cboChonNhanVien.DisplayMember = "HOTEN";
                cboChonNhanVien.ValueMember = "MANV";
                cboChonNhanVien.SelectedIndex = -1;

                // B. Tải Dữ liệu Nghỉ Phép
                // Cột TrangThai trong SQL là số (0,1,2)
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
                string sSua = @"UPDATE tb_NGHIPHEP 
                                SET MANV=@MANV, NgayBatDau=@NgayBatDau, SoNgayNghi=@SoNgayNghi, LyDo=@LyDo, TrangThai=@TrangThai 
                                WHERE ID=@ID";
                SqlCommand cmdSua = new SqlCommand(sSua, conn);
                cmdSua.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
                cmdSua.Parameters.Add("@NgayBatDau", SqlDbType.Date, 8, "NgayBatDau");
                cmdSua.Parameters.Add("@SoNgayNghi", SqlDbType.Int, 4, "SoNgayNghi");
                cmdSua.Parameters.Add("@LyDo", SqlDbType.NVarChar, 200, "LyDo");
                cmdSua.Parameters.Add("@TrangThai", SqlDbType.Int, 4, "TrangThai");
                SqlParameter paramID = cmdSua.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
                paramID.SourceVersion = DataRowVersion.Original; // Quan trọng: Dùng ID gốc để tìm
                daNghiPhep.UpdateCommand = cmdSua;

                // --- CẤU HÌNH LỆNH DELETE (XÓA) ---
                string sXoa = @"DELETE FROM tb_NGHIPHEP WHERE ID=@ID";
                SqlCommand cmdXoa = new SqlCommand(sXoa, conn);
                SqlParameter paramIDXoa = cmdXoa.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
                paramIDXoa.SourceVersion = DataRowVersion.Original;
                daNghiPhep.DeleteCommand = cmdXoa;

                // C. Đổ dữ liệu vào DataSet
                daNghiPhep.Fill(ds, "tblNghiPhep");

                // Thiết lập Khóa Chính (Bắt buộc để Sửa/Xóa được)
                ds.Tables["tblNghiPhep"].PrimaryKey = new DataColumn[] { ds.Tables["tblNghiPhep"].Columns["ID"] };

                // Cấu hình ID tự tăng (RAM)
                ds.Tables["tblNghiPhep"].Columns["ID"].AutoIncrement = true;
                ds.Tables["tblNghiPhep"].Columns["ID"].AutoIncrementSeed = -1;
                ds.Tables["tblNghiPhep"].Columns["ID"].AutoIncrementStep = -1;

                // Thêm cột ảo "TenTrangThai" để hiện chữ thay vì số
                if (!ds.Tables["tblNghiPhep"].Columns.Contains("TenTrangThai"))
                {
                    ds.Tables["tblNghiPhep"].Columns.Add("TenTrangThai", typeof(string));
                }
                RefreshTrangThaiText(); // Cập nhật chữ cho cột ảo

                dgvNghiPhep.DataSource = ds.Tables["tblNghiPhep"];
                FormatLuoi(); // Ẩn cột ID, hiện tên cột đẹp
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv")
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
        public void ReloadData()
        {
            ds.Tables["tblNghiPhep"].Clear();
            daNghiPhep.Fill(ds, "tblNghiPhep");
            RefreshTrangThaiText();
            LamMoi();
        }

        // --- CÁC HÀM HỖ TRỢ ---
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
            cboChonNhanVien.Enabled = true; // Mở khóa chọn nhân viên
            cboChonNhanVien.SelectedIndex = -1;
            txtLyDo.Text = "";
            txtSoNgayNghi.Text = "";
            dtpNgayBatDau.Value = DateTime.Now;
            dgvNghiPhep.ClearSelection();
            this.Tag = null;
        }

        // --- 3. SỰ KIỆN CLICK LƯỚI ---
        private void dgvNghiPhep_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvNghiPhep.Rows.Count - 1)
            {
                try
                {
                    DataGridViewRow row = dgvNghiPhep.Rows[e.RowIndex];

                    // Đổ dữ liệu nhân viên
                    if (row.Cells["MANV"].Value != DBNull.Value)
                    {
                        cboChonNhanVien.SelectedValue = row.Cells["MANV"].Value;
                        cboChonNhanVien.Enabled = false; // KHÓA LẠI KHÔNG CHO ĐỔI NGƯỜI KHI SỬA
                    }

                    // Đổ các thông tin khác (Xử lý null an toàn)
                    txtLyDo.Text = row.Cells["LyDo"].Value?.ToString() ?? "";
                    txtSoNgayNghi.Text = row.Cells["SoNgayNghi"].Value?.ToString() ?? "";

                    if (row.Cells["NgayBatDau"].Value != DBNull.Value)
                        dtpNgayBatDau.Value = Convert.ToDateTime(row.Cells["NgayBatDau"].Value);

                    // Lưu ID vào Tag để dùng cho nút Sửa/Xóa
                    this.Tag = row.Cells["ID"].Value;
                }
                catch { }
            }
        }

        // --- 4. NÚT THÊM ---
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboChonNhanVien.SelectedIndex == -1) { MessageBox.Show("Chưa chọn nhân viên"); return; }
            if (string.IsNullOrWhiteSpace(txtLyDo.Text)) { MessageBox.Show("Chưa nhập lý do"); return; }

            // KIỂM TRA SỐ NGÀY HỢP LỆ
            int soNgay = 0;
            bool isNumber = int.TryParse(txtSoNgayNghi.Text, out soNgay);
            if (!isNumber || soNgay <= 0)
            {
                MessageBox.Show("Số ngày nghỉ phải là số lớn hơn 0!", "Lỗi");
                txtSoNgayNghi.Focus();
                return;
            }

            // Thêm vào RAM
            DataRow row = ds.Tables["tblNghiPhep"].NewRow();
            row["MANV"] = cboChonNhanVien.SelectedValue;
            row["HOTEN"] = cboChonNhanVien.Text;
            row["NgayBatDau"] = dtpNgayBatDau.Value;
            row["SoNgayNghi"] = soNgay;
            row["LyDo"] = txtLyDo.Text.Trim();
            row["TrangThai"] = 0; // 0 = Chờ duyệt
            row["TenTrangThai"] = "Chờ duyệt";

            ds.Tables["tblNghiPhep"].Rows.Add(row);
            MessageBox.Show("Đã thêm vào danh sách. Bấm LƯU để ghi vào CSDL.");
            LamMoi();
        }

        // --- 5. NÚT SỬA ---
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (this.Tag == null) { MessageBox.Show("Chọn dòng cần sửa!"); return; }
            if (string.IsNullOrWhiteSpace(txtLyDo.Text)) { MessageBox.Show("Chưa nhập lý do"); return; }

            // KIỂM TRA SỐ NGÀY
            int soNgay = 0;
            bool isNumber = int.TryParse(txtSoNgayNghi.Text, out soNgay);
            if (!isNumber || soNgay <= 0) { MessageBox.Show("Số ngày nghỉ sai!", "Lỗi"); return; }

            int id = Convert.ToInt32(this.Tag);
            DataRow row = ds.Tables["tblNghiPhep"].Rows.Find(id);

            if (row != null)
            {
                // KIỂM TRA: CHỈ CHO SỬA KHI CHƯA DUYỆT
                int trangThai = (row["TrangThai"] == DBNull.Value) ? 0 : Convert.ToInt32(row["TrangThai"]);
                if (trangThai != 0)
                {
                    MessageBox.Show("Đơn này đã được Duyệt/Từ chối. Không thể sửa!", "Cảnh báo");
                    return;
                }

                row.BeginEdit();
                // Không cho sửa MANV (vì đã khóa combobox)
                row["LyDo"] = txtLyDo.Text.Trim();
                row["NgayBatDau"] = dtpNgayBatDau.Value;
                row["SoNgayNghi"] = soNgay;
                row.EndEdit();

                MessageBox.Show("Đã sửa. Bấm LƯU để ghi vào CSDL.");
            }
        }

        // --- 6. NÚT XÓA ---
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (this.Tag == null) { MessageBox.Show("Chọn dòng cần xóa!"); return; }

            int id = Convert.ToInt32(this.Tag);
            DataRow row = ds.Tables["tblNghiPhep"].Rows.Find(id);

            if (row != null)
            {
                // KIỂM TRA: CHỈ CHO XÓA KHI CHƯA DUYỆT
                int trangThai = (row["TrangThai"] == DBNull.Value) ? 0 : Convert.ToInt32(row["TrangThai"]);
                if (trangThai != 0)
                {
                    MessageBox.Show("Đơn này đã được Duyệt/Từ chối. Không thể xóa!", "Cảnh báo");
                    return;
                }

                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    row.Delete();
                    MessageBox.Show("Đã xóa khỏi danh sách. Bấm LƯU để hoàn tất.");
                    LamMoi();
                }
            }
        }

        // --- 7. NÚT LƯU (Đẩy xuống SQL) ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                daNghiPhep.Update(ds, "tblNghiPhep");
                MessageBox.Show("Đã lưu thành công vào CSDL!");

                // Tải lại để lấy ID thật và cập nhật lại mọi thứ
                ds.Tables["tblNghiPhep"].Clear();
                daNghiPhep.Fill(ds, "tblNghiPhep");
                RefreshTrangThaiText();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                ds.Tables["tblNghiPhep"].RejectChanges();
            }
        }

        // --- 8. CÁC NÚT CHỨC NĂNG KHÁC ---
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            CapNhatTrangThai(1, "Đã duyệt");
        }

        private void btnTuChoi_Click(object sender, EventArgs e)
        {
            CapNhatTrangThai(2, "Từ chối");
        }

        private void CapNhatTrangThai(int trangThaiMoi, string tenTrangThai)
        {
            if (this.Tag == null) { MessageBox.Show("Chọn đơn cần duyệt!"); return; }
            int id = Convert.ToInt32(this.Tag);
            DataRow row = ds.Tables["tblNghiPhep"].Rows.Find(id);

            if (row != null)
            {
                row.BeginEdit();
                row["TrangThai"] = trangThaiMoi;
                row["TenTrangThai"] = tenTrangThai;
                row.EndEdit();

                // Duyệt thì lưu luôn cho tiện
                try
                {
                    daNghiPhep.Update(ds, "tblNghiPhep");
                    MessageBox.Show("Đã cập nhật: " + tenTrangThai);
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }
    }
}