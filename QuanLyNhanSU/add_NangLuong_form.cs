using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class add_NangLuong_form : Form
    {
        // 1. CẤU HÌNH
        // Lưu ý: Kiểm tra lại chuỗi kết nối của bạn
        string strKetNoi = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        SqlConnection conn = null;

        // Biến cờ
        string maQDCu = ""; // Lưu SOQD của bản ghi hiện tại khi chọn trên lưới
        bool isThem = false;
        int idHienTai = -1; // Dùng để lưu ID bản ghi khi Sửa/Xóa

        public add_NangLuong_form()
        {
            InitializeComponent();
            conn = new SqlConnection(strKetNoi);
        }

        private void add_NangLuong_form_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
            LoadComboBoxHopDong(); // Tải danh sách Hợp đồng vào ComboBox
            LockControl(true);     // Khóa các ô nhập
        }

        // --- PHẦN 1: CÁC HÀM HỖ TRỢ (LOAD DATA) ---

        private void LoadDataGrid()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Dùng JOIN để lấy luôn Tên Nhân Viên từ bảng Nhân Viên (đảm bảo luôn có tên)
                string sql = @"SELECT 
                        T.SOQD, 
                        T.SOHOPDONG,
                        T.MANV, 
                        NV.HOTEN, 
                        T.NGAYKY, 
                        T.HESOLUONG_HIENTAI, 
                        T.NGAYLENLUONG, 
                        T.HESOLUONG_MOI, 
                        T.NOIDUNG 
                      FROM tb_NANGLUONG T
                      JOIN tb_NHANVIEN NV ON T.MANV = NV.MANV
                      ORDER BY T.NGAYLENLUONG DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNangLuong.AutoGenerateColumns = false;
                dgvNangLuong.DataSource = dt;

               

                // --- QUAN TRỌNG: GÁN DATAPROPERTYNAME BẰNG CODE ---
                // Bạn không cần chỉnh trong Designer nữa, code này sẽ tự gán
                // Đảm bảo tên cột bên phải khớp chính xác với tên cột trong câu SQL trên
                dgvNangLuong.Columns["SoQD"].DataPropertyName = "SOQD"; // Cột ẩn nếu có
                dgvNangLuong.Columns["MaNhanVien"].DataPropertyName = "MANV";
                dgvNangLuong.Columns["Tennhanvien"].DataPropertyName = "HOTEN";
                dgvNangLuong.Columns["Ngayky"].DataPropertyName = "NGAYKY";
                dgvNangLuong.Columns["Hesoluongcu"].DataPropertyName = "HESOLUONG_HIENTAI";
                dgvNangLuong.Columns["Ngaynangluong"].DataPropertyName = "NGAYLENLUONG";
                dgvNangLuong.Columns["Hesoluongmoi"].DataPropertyName = "HESOLUONG_MOI";
                dgvNangLuong.Columns["Ghichu"].DataPropertyName = "NOIDUNG";
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
            finally { conn.Close(); }
        }

        private void LoadComboBoxHopDong()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // Chỉ lấy các hợp đồng đang hiệu lực hoặc tất cả tùy nghiệp vụ
                string sql = "SELECT SOHD FROM tb_HOPDONG";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboHopDong.DataSource = dt;
                cboHopDong.DisplayMember = "SOHD";
                cboHopDong.ValueMember = "SOHD";
                cboHopDong.SelectedIndex = -1; // Chưa chọn gì cả
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải ComboBox: " + ex.Message); }
            finally { conn.Close(); }
        }

        // SỰ KIỆN QUAN TRỌNG: Khi chọn Số Hợp Đồng -> Tự điền thông tin NV và Lương cũ
        private void cboHopDong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboHopDong.SelectedIndex == -1 || cboHopDong.SelectedValue == null) return;

            string soHD = cboHopDong.SelectedValue.ToString();

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Join bảng Hợp Đồng và Nhân Viên để lấy thông tin
                string sql = @"SELECT HD.MANV, NV.HOTEN, HD.HESOLUONG 
                               FROM tb_HOPDONG HD 
                               JOIN tb_NHANVIEN NV ON HD.MANV = NV.MANV 
                               WHERE HD.SOHD = @SOHD";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SOHD", soHD);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtMaNV.Text = dr["MANV"].ToString();
                    txtTenNV.Text = dr["HOTEN"].ToString();

                    // Tự động điền lương hiện tại vào ô Lương Cũ
                    if (dr["HESOLUONG"] != DBNull.Value)
                    {
                        nudHeSLC.Value = Convert.ToDecimal(dr["HESOLUONG"]);
                    }
                }
                dr.Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi lấy thông tin HĐ: " + ex.Message); }
            finally { conn.Close(); }
        }

        // --- PHẦN 2: XỬ LÝ GIAO DIỆN (LOCK/UNLOCK) ---

        private void LockControl(bool lockState)
        {
            // True = Khóa (Chỉ xem), False = Mở (Được nhập)
            cboHopDong.Enabled = !lockState;
            txtGhichu.Enabled = !lockState;
            dtpNgayKy.Enabled = !lockState;
            dtpNgayLenLuong.Enabled = !lockState;
            nudHSLMoi.Enabled = !lockState;

            // Các ô này luôn khóa vì tự động load, không cho sửa tay
            txtMaNV.Enabled = false;
            txtTenNV.Enabled = false;
            nudHeSLC.Enabled = false;
            txtSoHopDong.Enabled = false; // Dùng ComboBox thay thế textbox này

            btnThem.Enabled = lockState;
            btnSua.Enabled = lockState;
            btnXoa.Enabled = lockState;
            btnLuu.Enabled = !lockState;
            btnLamMoi.Enabled = true;
        }

        private void ResetInput()
        {
            cboHopDong.SelectedIndex = -1;
            txtSoHopDong.Clear();
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtGhichu.Clear();
            nudHeSLC.Value = 0;
            nudHSLMoi.Value = 0;
            dtpNgayKy.Value = DateTime.Now;
            dtpNgayLenLuong.Value = DateTime.Now;
        }

        // --- PHẦN 3: CHỨC NĂNG CRUD (THÊM, SỬA, XÓA) ---

        private void btnThem_Click(object sender, EventArgs e)
        {
            isThem = true;
            ResetInput();
            LockControl(false); // Mở khóa để nhập
            cboHopDong.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maQDCu)) // Kiểm tra chuỗi rỗng
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa trên lưới!", "Thông báo");
                return;
            }
            isThem = false;
            LockControl(false);

            // Khi sửa quyết định nâng lương, thường cấm sửa Nhân viên (vì sẽ sai lệch lịch sử)
            cboHopDong.Enabled = false;
            txtMaNV.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maQDCu))
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa Quyết định: " + maQDCu + "?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    // Sửa câu lệnh SQL dùng SOQD
                    SqlCommand cmd = new SqlCommand("DELETE FROM tb_NANGLUONG WHERE SOQD = @SOQD", conn);
                    cmd.Parameters.AddWithValue("@SOQD", maQDCu);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công!");
                    LoadDataGrid();
                    ResetInput();
                    maQDCu = ""; // Reset biến
                }
                catch (Exception ex) { MessageBox.Show("Lỗi xóa: " + ex.Message); }
                finally { conn.Close(); }
            }
        }

        // --- PHẦN 4: NÚT LƯU (QUAN TRỌNG NHẤT) ---
        private void btnLuu_Click(object sender, EventArgs e)
        {// 1. Validate dữ liệu cơ bản
            if (cboHopDong.SelectedIndex == -1 || string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn Số Hợp Đồng (dữ liệu nhân viên chưa được tải đủ)!", "Cảnh báo");
                return;
            }

            // 2. Lấy giá trị Số HĐ
            string soHD = string.Empty;
            if (cboHopDong.SelectedValue != null)
            {
                soHD = cboHopDong.SelectedValue.ToString();
            }
            else
            {
                MessageBox.Show("Số hợp đồng không hợp lệ.");
                return;
            }

            // 3. Mở kết nối
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = transaction;

                // --- CÁC THAM SỐ DÙNG CHUNG (Thêm hay Sửa đều cần) ---
                // Khai báo trước để dùng cho cả 2 trường hợp
                cmd.Parameters.AddWithValue("@NgayKy", dtpNgayKy.Value);
                cmd.Parameters.AddWithValue("@NgayLen", dtpNgayLenLuong.Value);
                cmd.Parameters.AddWithValue("@HSL_Moi", Convert.ToDouble(nudHSLMoi.Value));
                cmd.Parameters.AddWithValue("@GhiChu", txtGhichu.Text);

                if (isThem)
                {
                    // ================= TRƯỜNG HỢP THÊM MỚI =================

                    // 1. Insert vào bảng NANGLUONG
                    // (Tôi đã bổ sung cột SOHOPDONG vào đây)
                    cmd.CommandText = @"INSERT INTO tb_NANGLUONG 
                      (SOQD, SOHOPDONG, MANV, NGAYKY, NGAYLENLUONG, HESOLUONG_HIENTAI, HESOLUONG_MOI, NOIDUNG) 
                      VALUES (@SoQD, @SoHD, @MaNV, @NgayKy, @NgayLen, @HSL_Cu, @HSL_Moi, @GhiChu)";

                    // Tạo mã tự động
                    string maQD = "QDNL" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    cmd.Parameters.AddWithValue("@SoQD", maQD);
                    cmd.Parameters.AddWithValue("@SoHD", soHD); // <-- QUAN TRỌNG: Lưu số HĐ
                    cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                    cmd.Parameters.AddWithValue("@HSL_Cu", Convert.ToDouble(nudHeSLC.Value));

                    cmd.ExecuteNonQuery();

                    // 2. Update ngược lại vào bảng HOPDONG
                    // (Cần clear tham số cũ để tránh xung đột tên biến)
                    SqlCommand cmdUpdateHD = conn.CreateCommand();
                    cmdUpdateHD.Transaction = transaction;
                    cmdUpdateHD.CommandText = "UPDATE tb_HOPDONG SET HESOLUONG = @NewSalary WHERE SOHD = @SoHD_Key";
                    cmdUpdateHD.Parameters.AddWithValue("@NewSalary", Convert.ToDouble(nudHSLMoi.Value));
                    cmdUpdateHD.Parameters.AddWithValue("@SoHD_Key", soHD);

                    cmdUpdateHD.ExecuteNonQuery();
                }
                else
                {
                    // ================= TRƯỜNG HỢP SỬA (UPDATE) =================

                    // Lúc nãy bạn thiếu toàn bộ tham số ở đây
                    cmd.CommandText = @"UPDATE tb_NANGLUONG 
                SET NGAYKY=@NgayKy, 
                    NGAYLENLUONG=@NgayLen, 
                    HESOLUONG_MOI=@HSL_Moi, 
                    NOIDUNG=@GhiChu 
                WHERE SOQD=@SOQD";

                    cmd.Parameters.AddWithValue("@SOQD", maQDCu);

                    // Lưu ý: Các tham số @NgayKy, @NgayLen... đã được Add ở phần "DÙNG CHUNG" bên trên rồi
                    // nên không cần Add lại, lệnh ExecuteNonQuery sẽ tự nhận.

                    cmd.ExecuteNonQuery();

                    // (Tùy chọn) Nếu bạn muốn khi sửa QĐ nâng lương cũng cập nhật lại Hợp Đồng 
                    // thì viết thêm lệnh Update HOPDONG ở đây tương tự phần Thêm.
                }

                transaction.Commit();
                MessageBox.Show("Lưu thành công!");
                LoadDataGrid();
                LockControl(true);
                idHienTai = -1;
                maQDCu = ""; // Reset mã
            }
            catch (FormatException)
            {
                transaction.Rollback();
                MessageBox.Show("Lỗi định dạng số! Kiểm tra lại các ô nhập lương.", "Lỗi Format");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Sự kiện Click vào Grid để đưa dữ liệu lên form
        private void dgvNangLuong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Kiểm tra click hợp lệ
            if (e.RowIndex < 0 || e.RowIndex >= dgvNangLuong.Rows.Count) return;

            try
            {
                // 2. Lấy dữ liệu gốc (DataRowView)
                DataGridViewRow row = dgvNangLuong.Rows[e.RowIndex];
                DataRowView drv = row.DataBoundItem as DataRowView;

                if (drv == null) return;

                // --- A. ĐIỀN CÁC TEXTBOX TRỰC TIẾP (Không phụ thuộc ComboBox) ---

                // 1. Số Quyết Định (Bạn kiểm tra lại tên TextBox này trên form nhé, tôi đoán là txtSoQD)
                if (drv.Row.Table.Columns.Contains("SOQD"))
                {
                    maQDCu = drv["SOQD"].ToString();
                    // Nếu bạn có textbox tên là txtSoQD thì bỏ comment dòng dưới:
                    // txtSoQD.Text = maQDCu; 
                }

                // 2. Mã và Tên Nhân Viên (Lấy thẳng từ Grid luôn cho chắc)
                if (drv.Row.Table.Columns.Contains("MANV"))
                    txtMaNV.Text = drv["MANV"].ToString();

                if (drv.Row.Table.Columns.Contains("HOTEN"))
                    txtTenNV.Text = drv["HOTEN"].ToString();

                // 3. Hệ số lương cũ
                if (drv.Row.Table.Columns.Contains("HESOLUONG_HIENTAI") && drv["HESOLUONG_HIENTAI"] != DBNull.Value)
                    nudHeSLC.Value = Convert.ToDecimal(drv["HESOLUONG_HIENTAI"]);

                // --- B. XỬ LÝ COMBOBOX (HỢP ĐỒNG) ---
                // Chúng ta ngắt sự kiện SelectedIndexChanged tạm thời để tránh nó reset dữ liệu
                cboHopDong.SelectedIndexChanged -= cboHopDong_SelectedIndexChanged;

                if (drv.Row.Table.Columns.Contains("SOHOPDONG") && drv["SOHOPDONG"] != DBNull.Value)
                {
                    cboHopDong.SelectedValue = drv["SOHOPDONG"].ToString();
                }
                else
                {
                    cboHopDong.SelectedIndex = -1; // Nếu không có HĐ thì để trống
                }

                // Gắn lại sự kiện
                cboHopDong.SelectedIndexChanged += cboHopDong_SelectedIndexChanged;

                // --- C. CÁC TRƯỜNG CÒN LẠI ---

                // Ngày Ký
                if (drv.Row.Table.Columns.Contains("NGAYKY") && drv["NGAYKY"] != DBNull.Value)
                    dtpNgayKy.Value = Convert.ToDateTime(drv["NGAYKY"]);

                // Ngày Lên Lương
                if (drv.Row.Table.Columns.Contains("NGAYLENLUONG") && drv["NGAYLENLUONG"] != DBNull.Value)
                    dtpNgayLenLuong.Value = Convert.ToDateTime(drv["NGAYLENLUONG"]);

                // Lương Mới
                if (drv.Row.Table.Columns.Contains("HESOLUONG_MOI") && drv["HESOLUONG_MOI"] != DBNull.Value)
                    nudHSLMoi.Value = Convert.ToDecimal(drv["HESOLUONG_MOI"]);

                // Ghi chú
                if (drv.Row.Table.Columns.Contains("NOIDUNG"))
                    txtGhichu.Text = drv["NOIDUNG"].ToString();

                // Mở khóa nút
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = false;
                LockControl(false); // Mở khóa GroupBox để xem nhưng chặn nhập
                txtMaNV.Enabled = false;
                txtTenNV.Enabled = false;

                // Vì đây là xem lại lịch sử, nên khóa luôn ComboBox Hợp đồng lại để tránh người dùng sửa nhầm
                // Muốn sửa thì bấm nút Sửa mới mở ra
                cboHopDong.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
            LoadDataGrid();
            idHienTai = -1;
            LockControl(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}