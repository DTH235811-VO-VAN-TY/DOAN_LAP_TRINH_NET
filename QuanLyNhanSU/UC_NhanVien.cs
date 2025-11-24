using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing; // Thư viện để in ấn
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyNhanSU
{
    public partial class UC_NhanVien : UserControl
    {
        public UC_NhanVien()
        {
            InitializeComponent();
        }

        // === SỬA LỖI 1: Chuyển 'conn' thành biến toàn cục ===
        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daNhanVien;
        SqlDataAdapter daPhongBan;
        SqlDataAdapter daTrinhDo;
        SqlDataAdapter daBoPhan;
        SqlDataAdapter daChucVu;
        public event EventHandler DataUpdated;
        private void label3_Click(object sender, EventArgs e)
        {

        }

        // --- PHƯƠNG THỨC MỚI: ReloadData ---
        // Phương thức này để các Form khác gọi khi cần cập nhật lại danh sách nhân viên
        public void ReloadData()
        {
            try
            {
                if (conn == null)
                {
                    conn = new SqlConnection(@"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True");
                }

                if (conn.State == ConnectionState.Closed) conn.Open();

                // Xóa dữ liệu cũ trong bảng NhanVien của DataSet
                if (ds.Tables.Contains("NhanVien"))
                {
                    ds.Tables["NhanVien"].Clear();
                }

                // Tải lại dữ liệu mới từ CSDL
                // Lưu ý: daNhanVien đã được cấu hình câu lệnh SELECT ở LoadDataConfiguration
                if (daNhanVien != null)
                {
                    daNhanVien.Fill(ds, "NhanVien");
                }
                else
                {
                    // Trường hợp daNhanVien chưa được khởi tạo (hiếm gặp nếu Load đã chạy)
                    // Gọi lại cấu hình
                    LoadDataConfiguration();
                    daNhanVien.Fill(ds, "NhanVien");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
            }
        }

        // Tách logic cấu hình DataAdapter ra riêng để gọn gàng và tái sử dụng
        private void LoadDataConfiguration()
        {
            //--------------------//
            // === SỬA LỖI 2: Xóa 'DIENTHOAI' khỏi câu SELECT ===
            // (Câu SELECT của bạn đã xóa rồi, rất tốt! Tôi giữ nguyên)
            // Cập nhật câu lệnh SELECT để chỉ lấy nhân viên ĐANG LÀM VIỆC (DATHOIVIEC = 0 hoặc NULL)
            string sqlNhanVien = @"SELECT 
                                            n.MANV, n.HOTEN, n.GIOITINH, n.NGAYSINH, n.CCCD, n.DIACHI, 
                                            n.IDPB, p.TENPB, 
                                            n.IDBP, b.TENBP,
                                            n.IDCV, c.TENCV,
                                            n.IDTD, t.TENTD
                                          FROM TB_NHANVIEN n
                                          LEFT JOIN TB_PHONGBAN p ON n.IDPB = p.IDPB
                                          LEFT JOIN TB_BOPHAN b ON n.IDBP = b.IDBP
                                          LEFT JOIN TB_CHUCVU c ON n.IDCV = c.IDCV
                                          LEFT JOIN TB_TRINHDO t ON n.IDTD = t.IDTD
                                          WHERE n.DATHOIVIEC = 0 OR n.DATHOIVIEC IS NULL"; // <-- THÊM ĐIỀU KIỆN LỌC

            daNhanVien = new SqlDataAdapter(sqlNhanVien, conn);

            //-----Comand them nhan vien
            // === SỬA LỖI 2: Xóa 'DIENTHOAI' khỏi câu INSERT ===
            string sThemNV = @"INSERT INTO TB_NHANVIEN (MANV, HOTEN, GIOITINH, NGAYSINH, CCCD, DIACHI, IDPB, IDBP, IDCV, IDTD) 
                                       VALUES (@MANV, @HOTEN, @GIOITINH, @NGAYSINH, @CCCD, @DIACHI, @IDPB, @IDBP, @IDCV, @IDTD)";

            SqlCommand cmdThemNV = new SqlCommand(sThemNV, conn);
            cmdThemNV.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            cmdThemNV.Parameters.Add("@HOTEN", SqlDbType.NVarChar, 100, "HOTEN");
            cmdThemNV.Parameters.Add("@GIOITINH", SqlDbType.NVarChar, 10, "GIOITINH");
            cmdThemNV.Parameters.Add("@NGAYSINH", SqlDbType.Date, 0, "NGAYSINH");
            cmdThemNV.Parameters.Add("@CCCD", SqlDbType.VarChar, 15, "CCCD");
            // (Đã xóa dòng DIENTHOAI)
            cmdThemNV.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 255, "DIACHI");
            cmdThemNV.Parameters.Add("@IDPB", SqlDbType.Int, 4, "IDPB");
            cmdThemNV.Parameters.Add("@IDBP", SqlDbType.Int, 4, "IDBP");
            cmdThemNV.Parameters.Add("@IDCV", SqlDbType.Int, 4, "IDCV");
            cmdThemNV.Parameters.Add("@IDTD", SqlDbType.Int, 4, "IDTD");
            daNhanVien.InsertCommand = cmdThemNV;

            // (Câu UPDATE của bạn đã đúng, không có DIENTHOAI)
            string sSuaNV = @"UPDATE TB_NHANVIEN SET 
                                    HOTEN=@HOTEN, GIOITINH=@GIOITINH, NGAYSINH=@NGAYSINH, CCCD=@CCCD, 
                                    DIACHI=@DIACHI, IDPB=@IDPB, IDBP=@IDBP, IDCV=@IDCV, IDTD=@IDTD 
                                  WHERE MANV=@MANV";

            SqlCommand cmdSuaNV = new SqlCommand(sSuaNV, conn);
            cmdSuaNV.Parameters.Add("@HOTEN", SqlDbType.NVarChar, 100, "HOTEN");
            cmdSuaNV.Parameters.Add("@GIOITINH", SqlDbType.NVarChar, 10, "GIOITINH");
            cmdSuaNV.Parameters.Add("@NGAYSINH", SqlDbType.Date, 0, "NGAYSINH");
            cmdSuaNV.Parameters.Add("@CCCD", SqlDbType.VarChar, 15, "CCCD");
            // (Đã xóa dòng DIENTHOAI)
            cmdSuaNV.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 255, "DIACHI");
            cmdSuaNV.Parameters.Add("@IDPB", SqlDbType.Int, 4, "IDPB");
            cmdSuaNV.Parameters.Add("@IDBP", SqlDbType.Int, 4, "IDBP");
            cmdSuaNV.Parameters.Add("@IDCV", SqlDbType.Int, 4, "IDCV");
            cmdSuaNV.Parameters.Add("@IDTD", SqlDbType.Int, 4, "IDTD");
            cmdSuaNV.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daNhanVien.UpdateCommand = cmdSuaNV;

            // (Câu DELETE của bạn đã đúng)
            string sXoaNV = @"DELETE FROM TB_NHANVIEN WHERE MANV=@MANV";
            SqlCommand cmdXoaNV = new SqlCommand(sXoaNV, conn);
            cmdXoaNV.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            daNhanVien.DeleteCommand = cmdXoaNV;
        }


        private void UC_NhanVien_Load(object sender, EventArgs e)
        {

            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng thêm/sửa/xóa
                btnThemNv.Enabled = false;
                btnSuaNV.Enabled = false;
                btnXoaNV.Enabled = false;
                btnLuuNv.Enabled = false;
                btnHuyNV.Enabled = false;
                btnInNV.Enabled = false;
                // (Nếu có nút Hủy hay Làm mới thì tắt nốt nếu muốn)

                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox) ((TextBox)c).ReadOnly = true;
                }

                // 3. GridView chỉ cho xem
                dgvNhanVien.ReadOnly = true;
            }
            // Chặn lỗi design mode
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }

            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

            try
            {
                conn.Open();

                // Tải ComboBoxes (Code của bạn đã đúng)
                string sqlPhongBan = @"SELECT * FROM TB_PHONGBAN";
                daPhongBan = new SqlDataAdapter(sqlPhongBan, conn);
                daPhongBan.Fill(ds, "PhongBan");
                cboPhongBan.DataSource = ds.Tables["PhongBan"];
                cboPhongBan.DisplayMember = "TENPB";
                cboPhongBan.ValueMember = "IDPB";

                string sqlTrinhDo = @"SELECT * FROM TB_TRINHDO";
                daTrinhDo = new SqlDataAdapter(sqlTrinhDo, conn);
                daTrinhDo.Fill(ds, "TrinhDo");
                cboTrinhDo.DataSource = ds.Tables["TrinhDo"];
                cboTrinhDo.DisplayMember = "TENTD";
                cboTrinhDo.ValueMember = "IDTD";

                string sqlBoPhan = @"SELECT * FROM TB_BOPHAN";
                daBoPhan = new SqlDataAdapter(sqlBoPhan, conn);
                // Sửa: Tải vào bảng tên "AllBoPhan" để giữ làm master list
                daBoPhan.Fill(ds, "AllBoPhan");

                // 3. THÊM: "Nối dây" sự kiện. Khi cboPhongBan thay đổi, gọi hàm lọc
                this.cboPhongBan.SelectedIndexChanged += new System.EventHandler(this.cboPhongBan_SelectedIndexChanged);

                string sqlChucVu = @"SELECT * FROM TB_CHUCVU";
                daChucVu = new SqlDataAdapter(sqlChucVu, conn);
                daChucVu.Fill(ds, "ChucVu");
                cboChucVu.DataSource = ds.Tables["ChucVu"];
                cboChucVu.DisplayMember = "TENCV";
                cboChucVu.ValueMember = "IDCV";

                // Cấu hình Adapter cho Nhân viên
                LoadDataConfiguration();

                // Tải dữ liệu nhân viên
                daNhanVien.Fill(ds, "NhanVien");

                // === SỬA LỖI 3: Thêm Khóa Chính cho DataSet (QUAN TRỌNG!) ===
                // Giúp DataSet tìm, sửa, xóa nhanh và chính xác
                ds.Tables["NhanVien"].PrimaryKey = new DataColumn[] { ds.Tables["NhanVien"].Columns["MANV"] };

                dgvNhanVien.DataSource = ds.Tables["NhanVien"];

                // (Code cấu hình cột của bạn đã đúng)
                dgvNhanVien.Columns["MANV"].HeaderText = "Mã NV";
                dgvNhanVien.Columns["MANV"].Width = 60;
                dgvNhanVien.Columns["HOTEN"].HeaderText = "Họ tên nhân viên";
                dgvNhanVien.Columns["HOTEN"].Width = 200;
                dgvNhanVien.Columns["GIOITINH"].HeaderText = "Gioi tinh";
                dgvNhanVien.Columns["GIOITINH"].Width = 50;
                dgvNhanVien.Columns["NGAYSINH"].HeaderText = "Ngay Sinh";
                dgvNhanVien.Columns["NGAYSINH"].Width = 100;
                dgvNhanVien.Columns["CCCD"].HeaderText = "CCCD";
                dgvNhanVien.Columns["CCCD"].Width = 125;
                dgvNhanVien.Columns["DIACHI"].HeaderText = "Địa chỉ";
                dgvNhanVien.Columns["DIACHI"].Width = 150;
                dgvNhanVien.Columns["TENPB"].HeaderText = "Tên Phòng Ban";
                dgvNhanVien.Columns["TENPB"].Width = 120;
                dgvNhanVien.Columns["TENBP"].HeaderText = "Tên Bộ Phận";
                dgvNhanVien.Columns["TENBP"].Width = 120;
                dgvNhanVien.Columns["TENCV"].HeaderText = "Tên Chức Vụ";
                dgvNhanVien.Columns["TENCV"].Width = 120;
                dgvNhanVien.Columns["TENTD"].HeaderText = "Tên Trình Độ";
                dgvNhanVien.Columns["TENTD"].Width = 120;
                dgvNhanVien.Columns["IDPB"].Visible = false;
                dgvNhanVien.Columns["IDBP"].Visible = false;
                dgvNhanVien.Columns["IDCV"].Visible = false;
                dgvNhanVien.Columns["IDTD"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nghiêm trọng khi tải Form: " + ex.Message);
            }
            finally
            {
                // KHÔNG đóng 'conn' ở đây, vì 'daNhanVien' cần nó "sống"
                // (Chúng ta sẽ đóng nó khi Form/UC bị đóng)
            }
            LoadSearchComboBox();
        }
        private void LoadSearchComboBox()
        {
            var searchOptions = new[] {
                new { Text = "Tìm theo Mã NV", Value = "MANV" },
                new { Text = "Tìm theo Họ Tên", Value = "HOTEN" }
            };
            // (Sửa lại tên 'cboTimKiemTheo' nếu tên của bạn khác)
            cboTimKiemNV.DataSource = searchOptions;
            cboTimKiemNV.DisplayMember = "Text";
            cboTimKiemNV.ValueMember = "Value";
            cboTimKiemNV.SelectedIndex = 0;
        }

        private void dgvNhanVien_Click(object sender, EventArgs e)
        {
            // (Hàm này bạn đã "cắm điện" ⚡ cho DGV chưa? 
            // Nếu chưa, vào [Design] -> Events ⚡ -> Click -> chọn dgvNhanVien_Click)

            // === SỬA LỖI 5: Thêm try-catch và kiểm tra DBNull ===
            try
            {
                if (dgvNhanVien.SelectedRows.Count > 0)
                {
                    DataGridViewRow dr = dgvNhanVien.SelectedRows[0];
                    txtMaNV.Text = dr.Cells["MANV"].Value.ToString();
                    txtHoTen.Text = dr.Cells["HOTEN"].Value.ToString();
                    if (dr.Cells["GIOITINH"].Value.ToString() == "Nam")
                        rdNam.Checked = true;
                    else
                        rdNu.Checked = true;

                    // Kiểm tra NULL trước khi Convert
                    if (dr.Cells["NGAYSINH"].Value != DBNull.Value)
                        dtpNgaySinh.Value = Convert.ToDateTime(dr.Cells["NGAYSINH"].Value);

                    txtCCCD.Text = dr.Cells["CCCD"].Value.ToString();
                    txtDcNV.Text = dr.Cells["DIACHI"].Value.ToString();

                    // Gán ComboBox bằng Value 
                    cboPhongBan.SelectedValue = dr.Cells["IDPB"].Value != DBNull.Value ? dr.Cells["IDPB"].Value : -1;
                    cboBoPhan.SelectedValue = dr.Cells["IDBP"].Value != DBNull.Value ? dr.Cells["IDBP"].Value : -1;
                    cboChucVu.SelectedValue = dr.Cells["IDCV"].Value != DBNull.Value ? dr.Cells["IDCV"].Value : -1;
                    cboTrinhDo.SelectedValue = dr.Cells["IDTD"].Value != DBNull.Value ? dr.Cells["IDTD"].Value : -1;

                    txtMaNV.Enabled = false; // Khóa khóa chính
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn hàng: " + ex.Message);
            }
        }

        private void btnThemNv_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text.Length > 6)
            {
                MessageBox.Show("Mã nhân viên tối đa 6 ký tự");
            }
            else if ((!System.Text.RegularExpressions.Regex.IsMatch(txtMaNV.Text, @"^NV\d+$")))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng Mã Nhân Viên, Ví dụ: NV001");
            }
            else if (txtMaNV.Text == "")
            {
                MessageBox.Show("Mã nhân viên không được rỗng!");
            }
            else if (txtCCCD.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Căn Cước công dân");
            }
            else if (txtDcNV.Text == "")
            {
                MessageBox.Show("Vui lòng nhập địa chỉ nhân viên");
            }
            else if(dtpNgaySinh.Value >= DateTime.Now)
            {
                MessageBox.Show("Ngày sinh không hợp lệ!");
            }
            else
            {
                DataRow row = ds.Tables["NhanVien"].NewRow();
                row["MANV"] = txtMaNV.Text;
                row["HOTEN"] = txtHoTen.Text;
                if (rdNam.Checked == true)
                    row["GIOITINH"] = "Nam";
                else
                    row["GIOITINH"] = "Nữ";
                row["NGAYSINH"] = dtpNgaySinh.Value;
                row["CCCD"] = txtCCCD.Text;
                row["DIACHI"] = txtDcNV.Text;
                row["IDPB"] = cboPhongBan.SelectedValue;
                row["IDBP"] = cboBoPhan.SelectedValue;
                row["IDCV"] = cboChucVu.SelectedValue;
                row["IDTD"] = cboTrinhDo.SelectedValue;
                row["TENPB"] = cboPhongBan.Text;
                row["TENBP"] = cboBoPhan.Text;
                row["TENCV"] = cboChucVu.Text;
                row["TENTD"] = cboTrinhDo.Text;
                ds.Tables["NhanVien"].Rows.Add(row);

                MessageBox.Show("Đã thêm vào bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            }
        }

        // === SỬA LỖI 4: Sửa lại hàm Sửa cho đúng ===
        private void btnSuaNV_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Enabled == true || string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Bạn chưa chọn nhân viên để sửa.", "Lỗi");
                return;
            }

            // 1. Tìm dòng (Row) trong DataSet bằng Khóa Chính
            DataRow rowToUpdate = ds.Tables["NhanVien"].Rows.Find(txtMaNV.Text);

            if (rowToUpdate != null)
            {
                // 2. Cập nhật dữ liệu cho dòng đó (trong RAM)
                rowToUpdate.BeginEdit();
                // (Không sửa MANV - Khóa chính)
                rowToUpdate["HOTEN"] = txtHoTen.Text;
                if (rdNam.Checked == true)
                    rowToUpdate["GIOITINH"] = "Nam";
                else
                    rowToUpdate["GIOITINH"] = "Nữ";
                rowToUpdate["NGAYSINH"] = dtpNgaySinh.Value;
                rowToUpdate["CCCD"] = txtCCCD.Text;
                rowToUpdate["DIACHI"] = txtDcNV.Text;
                rowToUpdate["IDPB"] = cboPhongBan.SelectedValue;
                rowToUpdate["IDBP"] = cboBoPhan.SelectedValue;
                rowToUpdate["IDCV"] = cboChucVu.SelectedValue;
                rowToUpdate["IDTD"] = cboTrinhDo.SelectedValue;

                // Cập nhật tên (để DGV hiển thị ngay)
                rowToUpdate["TENPB"] = cboPhongBan.Text;
                rowToUpdate["TENBP"] = cboBoPhan.Text;
                rowToUpdate["TENCV"] = cboChucVu.Text;
                rowToUpdate["TENTD"] = cboTrinhDo.Text;
                rowToUpdate.EndEdit();

                MessageBox.Show("Đã sửa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên để sửa.", "Lỗi");
            }
        }

        private void btnXoaNV_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Enabled == true || string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Bạn chưa chọn nhân viên để xóa.", "Lỗi");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa NV: " + txtMaNV.Text + "?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            // 1. Tìm dòng trong DataSet
            DataRow rowToDelete = ds.Tables["NhanVien"].Rows.Find(txtMaNV.Text);

            if (rowToDelete != null)
            {
                // 2. Xóa dòng (trong RAM)
                rowToDelete.Delete();
                MessageBox.Show("Đã xóa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            }
        }

        private void btnLuuNv_Click(object sender, EventArgs e)
        {
            try
            {
                // Đây là lúc "đẩy" tất cả thay đổi (Thêm, Sửa, Xóa) xuống CSDL
                int rowsAffected = daNhanVien.Update(ds, "NhanVien");
                MessageBox.Show($"Đã Lưu {rowsAffected} thay đổi vào cơ sở dữ liệu thành công.");
                DataUpdated?.Invoke(this, EventArgs.Empty);
                // (Tùy chọn) Tải lại dữ liệu "sạch" từ CSDL sau khi lưu
                ReloadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message);
                // Nếu lỗi, khôi phục lại DataSet
                ds.Tables["NhanVien"].RejectChanges();
            }
        }

        private void btnThemPB_Click(object sender, EventArgs e)
        {
            add_PhongBan_from add_PhongBan_From = new add_PhongBan_from();
            add_PhongBan_From.DataSaved += Add_PhongBan_From_DataSaved;
            add_PhongBan_From.ShowDialog();
        }
        private void Add_PhongBan_From_DataSaved(object sender, EventArgs e)
        {
            try
            {
                // Xóa dữ liệu cũ trong bảng "PhongBan" của DataSet
                ds.Tables["PhongBan"].Clear();
                // Tải lại dữ liệu mới từ CSDL vào bảng đó
                daPhongBan.Fill(ds, "PhongBan");
                // ComboBox cboPhongBan sẽ tự động cập nhật
                // vì nó đã được gán DataSource = ds.Tables["PhongBan"]
                // trong hàm UC_NhanVien_Load
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lại danh sách phòng ban: " + ex.Message);
            }
        }

        private void btnThemTrinhDo_Click(object sender, EventArgs e)
        {
            add_TrinhDo_form add_TrinhDo_Form = new add_TrinhDo_form();
            add_TrinhDo_Form.DataSaved += Add_TrinhDo_Form_DataSaved;
            add_TrinhDo_Form.ShowDialog();
        }
        private void Add_TrinhDo_Form_DataSaved(object sender, EventArgs e)
        {
            // Tải lại ComboBox Trình Độ
            try
            {
                ds.Tables["TrinhDo"].Clear();
                daTrinhDo.Fill(ds, "TrinhDo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lại danh sách trình độ: " + ex.Message);
            }
        }
        private void cboPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem cboPhongBan đã chọn giá trị hợp lệ chưa
            if (cboPhongBan.SelectedValue == null || cboPhongBan.SelectedValue is DBNull)
            {
                cboBoPhan.DataSource = null; // Nếu chưa, làm trắng cboBoPhan
                return;
            }

            try
            {
                // Lấy ID Phòng ban đã chọn
                int selectedIDPB = Convert.ToInt32(cboPhongBan.SelectedValue);

                // Tạo một DataView từ bảng "AllBoPhan" (bảng master list)
                DataView dvBoPhan = new DataView(ds.Tables["AllBoPhan"]);

                // Áp dụng bộ lọc (RowFilter)
                // "Chỉ hiển thị các dòng có IDPB bằng với ID ta đã chọn"
                dvBoPhan.RowFilter = $"IDPB = {selectedIDPB}";

                // Gán DataView đã lọc làm nguồn cho cboBoPhan
                cboBoPhan.DataSource = dvBoPhan;
                cboBoPhan.DisplayMember = "TENBP";
                cboBoPhan.ValueMember = "IDBP";
            }
            catch (Exception)
            {
                // Xảy ra khi form đang tải, an toàn để bỏ qua
                cboBoPhan.DataSource = null;
            }
        }

        private void btnThemBP_Click(object sender, EventArgs e)
        {
            add_BoPhan_form add_BoPhan_Form = new add_BoPhan_form();
            add_BoPhan_Form.DataSaved += Add_BoPhan_Form_DataSaved;
            add_BoPhan_Form.ShowDialog();
        }

        private void Add_BoPhan_Form_DataSaved(object sender, EventArgs e)
        {
            try
            {
                // Tải lại bảng master "AllBoPhan"
                ds.Tables["AllBoPhan"].Clear();
                daBoPhan.Fill(ds, "AllBoPhan");

                MessageBox.Show("Đã cập nhật danh sách bộ phận!");

                // Sau khi tải lại, chúng ta cần "lọc" lại cboBoPhan
                // bằng cách giả vờ như người dùng vừa nhấn vào cboPhongBan
                cboPhongBan_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lại danh sách bộ phận: " + ex.Message);
            }
        }

        private void btnTiemKiemNV_Click(object sender, EventArgs e)
        {
            string searchTerm = txtTimKiemNV.Text.Trim().Replace("'", "''"); // Lấy nội dung, và thoát ký tự '
            string searchColumn = cboTimKiemNV.SelectedValue.ToString();

            // Lấy DataView mặc định của bảng NhanVien
            DataView dv = ds.Tables["NhanVien"].DefaultView;

            if (string.IsNullOrEmpty(searchTerm))
            {
                // Nếu ô tìm kiếm rỗng, hiển thị lại tất cả
                dv.RowFilter = string.Empty;
                return;
            }

            // Áp dụng bộ lọc (RowFilter)
            // Chúng ta dùng LIKE '%' để tìm gần đúng (chứa chuỗi)
            // Ví dụ: HOTEN LIKE '%An%' (Tìm bất kỳ ai có tên chứa 'An')
            dv.RowFilter = $"{searchColumn} LIKE '%{searchTerm}%'";
        }

        private void btnHienAllNV_Click(object sender, EventArgs e)
        {
            // Chỉ cần xóa bộ lọc và xóa chữ trong ô tìm kiếm
            txtTimKiemNV.Text = "";
            ds.Tables["NhanVien"].DefaultView.RowFilter = string.Empty;
        }

        private void btnThemChucVu_Click(object sender, EventArgs e)
        {
            add_Chucvu_form add_Chucvu_Form = new add_Chucvu_form();
            add_Chucvu_Form.DataSaved += Add_ChucVu_Form_DataSaved;
            add_Chucvu_Form.ShowDialog();
        }
        private void Add_ChucVu_Form_DataSaved(object sender, EventArgs e)
        {
            // Đây là nơi chúng ta tải lại ComboBox Phòng Ban
            try
            {
                // Xóa dữ liệu cũ trong bảng "PhongBan" của DataSet
                ds.Tables["ChucVu"].Clear();

                // Tải lại dữ liệu mới từ CSDL vào bảng đó
                daChucVu.Fill(ds, "ChucVu");

                // ComboBox cboPhongBan sẽ tự động cập nhật
                // vì nó đã được gán DataSource = ds.Tables["PhongBan"]
                // trong hàm UC_NhanVien_Load
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lại danh sách chuc vu: " + ex.Message);
            }
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void txtHoTen_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtHoTen.Text == "")
            {
                e.Cancel = true;
                errorProvider.SetError(txtHoTen, "Không được để trống họ tên");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtHoTen, null);

            }
        }
        private void txtMaNV_Validating(object sender, CancelEventArgs e)
        {
            if (txtMaNV.Text.Length > 6)
            {
                e.Cancel = true;
                errorProvider.SetError(txtMaNV,"Mã nhân viên tối đa 6 ký tự");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtMaNV,null);
            }
        }
        private void btnHuyNV_Click(object sender, EventArgs e)
        {
            txtMaNV.Text = "";
            txtHoTen.Text = "";
            txtDcNV.Text = "";
            txtCCCD.Text = "";
            txtTimKiemNV.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            txtMaNV.Enabled = true;
            rdNam.Checked = true;
            cboPhongBan.SelectedIndex = -1;
            cboBoPhan.SelectedIndex = -1;
            cboChucVu.SelectedIndex = -1;
            cboTrinhDo.SelectedIndex = -1;
            dgvNhanVien.ClearSelection();
        }

        private void txtCCCD_Validating(object sender, CancelEventArgs e)
        {
            if (txtCCCD.Text.Length != 12 || !txtCCCD.Text.All(char.IsDigit))
            {
                e.Cancel = true;
                errorProvider.SetError(txtCCCD, "CCCD phải đủ 12 chữ số và chỉ chứa số");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtCCCD, null);
            }
        }

        private void dtpNgaySinh_Validating(object sender, CancelEventArgs e)
        {
            if (dtpNgaySinh.Value.Date >= DateTime.Now.Date)
            {
                e.Cancel = true;
                errorProvider.SetError(dtpNgaySinh, "Ngày sinh không hợp lệ");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(dtpNgaySinh, null);
            }

        }
        private void pd_BeginPrint(object sender, PrintEventArgs e)
        {
            // Đây là nơi quan trọng nhất: RESET lại mọi thứ về 0 trước khi in
            iRow = 0;
            arrColumnLefts.Clear();
            arrColumnWidths.Clear();
            bFirstPage = true;
            bNewPage = true;
        }
        private void btnInNV_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Landscape = true;

            // Thêm sự kiện BeginPrint để reset biến đếm trước khi in
            pd.BeginPrint += new PrintEventHandler(this.pd_BeginPrint);
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.Width = 1000;
            ppd.Height = 700;

            ppd.ShowDialog();
        }

        // Các biến toàn cục dùng để điều khiển trang in
        StringFormat strFormat; // Căn chỉnh text
        List<int> arrColumnLefts = new List<int>(); // Vị trí lề trái các cột
        List<int> arrColumnWidths = new List<int>(); // Độ rộng các cột
        int iCellHeight = 0; // Độ cao dòng
        int iTotalWidth = 0; // Tổng độ rộng bảng
        int iRow = 0; // Dòng hiện tại đang in
        bool bFirstPage = false; // Kiểm tra trang đầu
        bool bNewPage = false; // Kiểm tra trang mới
        int iHeaderHeight = 0; // Chiều cao tiêu đề cột

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                // 1. THIẾT LẬP CƠ BẢN
                int iLeftMargin = e.MarginBounds.Left;
                int iTopMargin = e.MarginBounds.Top;
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;

                // --- TÙY CHỈNH FONT & MÀU SẮC ---
                Font titleFont = new Font("Arial", 20, FontStyle.Bold);         // Font Tiêu đề lớn
                Font headerFont = new Font("Arial", 10, FontStyle.Bold);        // Font Tiêu đề cột
                Font contentFont = new Font("Arial", 10, FontStyle.Regular);    // Font Nội dung

                Brush titleBrush = Brushes.DarkBlue;                            // Màu chữ tiêu đề lớn
                Brush headerBrush = Brushes.Black;                              // Màu chữ tiêu đề cột
                Brush contentBrush = Brushes.Black;                             // Màu chữ nội dung

                // Màu nền cho tiêu đề cột (Xanh nhạt cho chuyên nghiệp)
                Brush headerBackBrush = new SolidBrush(Color.FromArgb(220, 230, 241));
                Pen borderPen = new Pen(Color.Black, 1);                        // Viền bảng

                // Chiều cao dòng cơ bản (tối thiểu)
                int rowHeightBase = 35;

                // 2. TÍNH TOÁN ĐỘ RỘNG CỘT (Chỉ làm ở trang đầu)
                if (bFirstPage)
                {
                    arrColumnLefts.Clear();
                    arrColumnWidths.Clear();
                    int totalVisibleWidth = 0;

                    // Tính tổng độ rộng thực tế của các cột đang hiện
                    foreach (DataGridViewColumn GridCol in dgvNhanVien.Columns)
                    {
                        if (GridCol.Visible) totalVisibleWidth += GridCol.Width;
                    }

                    // Tính lại độ rộng từng cột theo tỷ lệ trang giấy in
                    int iLeft = iLeftMargin;
                    foreach (DataGridViewColumn GridCol in dgvNhanVien.Columns)
                    {
                        if (GridCol.Visible)
                        {
                            // Công thức co giãn độ rộng cột theo khổ giấy
                            int newWidth = (int)(Math.Floor((double)((double)GridCol.Width / (double)totalVisibleWidth * (double)e.MarginBounds.Width)));

                            arrColumnWidths.Add(newWidth);
                            arrColumnLefts.Add(iLeft);
                            iLeft += newWidth;
                        }
                    }
                }

                // 3. VẼ TIÊU ĐỀ BÁO CÁO (Chỉ trang đầu)
                if (bFirstPage)
                {
                    string strTitle = "DANH SÁCH NHÂN VIÊN";
                    SizeF titleSize = e.Graphics.MeasureString(strTitle, titleFont);

                    // Căn giữa tiêu đề trang giấy
                    float titleX = e.MarginBounds.Left + (e.MarginBounds.Width - titleSize.Width) / 2;
                    e.Graphics.DrawString(strTitle, titleFont, titleBrush, titleX, e.MarginBounds.Top - 60);

                    string strDate = "Ngày in: " + DateTime.Now.ToString("dd/MM/yyyy");
                    e.Graphics.DrawString(strDate, new Font("Arial", 10, FontStyle.Italic), Brushes.Black, e.MarginBounds.Right - 150, e.MarginBounds.Top - 30);
                }

                // 4. VẼ TIÊU ĐỀ CỘT (Header) - Vẽ lại ở mỗi trang mới
                if (bNewPage)
                {
                    int iColumnIndex = 0;
                    iTopMargin = e.MarginBounds.Top; // Bắt đầu vẽ từ lề trên

                    foreach (DataGridViewColumn GridCol in dgvNhanVien.Columns)
                    {
                        if (GridCol.Visible)
                        {
                            Rectangle rectHeader = new Rectangle(arrColumnLefts[iColumnIndex], iTopMargin, arrColumnWidths[iColumnIndex], rowHeightBase);

                            // Tô màu nền tiêu đề
                            e.Graphics.FillRectangle(headerBackBrush, rectHeader);
                            e.Graphics.DrawRectangle(borderPen, rectHeader);

                            // Căn giữa chữ tiêu đề
                            StringFormat sfHeader = new StringFormat();
                            sfHeader.LineAlignment = StringAlignment.Center;
                            sfHeader.Alignment = StringAlignment.Center;

                            e.Graphics.DrawString(GridCol.HeaderText, headerFont, headerBrush, rectHeader, sfHeader);

                            iColumnIndex++;
                        }
                    }
                    iTopMargin += rowHeightBase; // Dịch xuống để vẽ dữ liệu
                }

                // 5. VẼ DỮ LIỆU (Nội dung)
                while (iRow < dgvNhanVien.Rows.Count)
                {
                    DataGridViewRow GridRow = dgvNhanVien.Rows[iRow];
                    if (GridRow.IsNewRow) { iRow++; continue; } // Bỏ qua dòng trống cuối cùng

                    // Tính chiều cao thực tế của dòng (phòng trường hợp chữ dài bị xuống dòng)
                    // Ở đây mình để mặc định, nếu muốn xịn hơn thì phải đo chuỗi
                    int currentRowHeight = rowHeightBase;

                    // Kiểm tra xem còn giấy để in dòng này không
                    if (iTopMargin + currentRowHeight >= e.MarginBounds.Bottom)
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break; // Ngắt trang
                    }
                    else
                    {
                        bNewPage = false;
                        bFirstPage = false; // Đã qua trang đầu

                        int iColumnIndex = 0;
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            if (Cel.OwningColumn.Visible)
                            {
                                Rectangle rectCell = new Rectangle(arrColumnLefts[iColumnIndex], iTopMargin, arrColumnWidths[iColumnIndex], currentRowHeight);

                                // Lấy dữ liệu và xử lý FORMAT
                                string cellValue = Cel.Value != null ? Cel.Value.ToString() : "";

                                // Xử lý Format NGÀY SINH (Quan trọng!)
                                if (Cel.OwningColumn.Name == "NGAYSINH" && Cel.Value != DBNull.Value)
                                {
                                    cellValue = Convert.ToDateTime(Cel.Value).ToString("dd/MM/yyyy");
                                }

                                // Thiết lập căn lề (Padding)
                                StringFormat sfCell = new StringFormat();
                                sfCell.LineAlignment = StringAlignment.Center; // Căn giữa chiều dọc

                                // Căn lề theo loại dữ liệu
                                // Mã NV, Ngày sinh, Giới tính, CCCD -> Căn Giữa
                                if (Cel.OwningColumn.Name == "MANV" || Cel.OwningColumn.Name == "NGAYSINH"
                                    || Cel.OwningColumn.Name == "GIOITINH" || Cel.OwningColumn.Name == "CCCD")
                                {
                                    sfCell.Alignment = StringAlignment.Center;
                                }
                                else // Tên, Địa chỉ, Phòng ban... -> Căn Trái (có thụt lề một chút)
                                {
                                    sfCell.Alignment = StringAlignment.Near;
                                    // Tạo khoảng hở bên trái để chữ không dính vạch
                                    Rectangle rectText = rectCell;
                                    rectText.X += 5;
                                    rectText.Width -= 5;
                                }

                                // Vẽ khung và chữ
                                e.Graphics.DrawRectangle(borderPen, rectCell);
                                e.Graphics.DrawString(cellValue, contentFont, contentBrush, rectCell, sfCell);

                                iColumnIndex++;
                            }
                        }
                        iTopMargin += currentRowHeight;
                        iRow++;
                    }
                }

                // Kiểm tra xem cần in trang tiếp theo không
                if (bMorePagesToPrint)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in ấn: " + ex.Message);
            }
        }

        
    }
}