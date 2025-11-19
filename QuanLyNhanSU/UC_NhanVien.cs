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

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void UC_NhanVien_Load(object sender, EventArgs e)
        {
            // === SỬA LỖI 1 (tiếp): Khởi tạo 'conn' toàn cục ===
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

                // 2. XÓA các dòng gán DataSource trực tiếp này
                // cboBoPhan.DataSource = ds.Tables["BoPhan"]; // <-- XÓA DÒNG NÀY
                // cboBoPhan.DisplayMember = "TENBP"; // <-- XÓA DÒNG NÀY
                // cboBoPhan.ValueMember = "IDBP"; // <-- XÓA DÒNG NÀY

                // 3. THÊM: "Nối dây" sự kiện. Khi cboPhongBan thay đổi, gọi hàm lọc
                this.cboPhongBan.SelectedIndexChanged += new System.EventHandler(this.cboPhongBan_SelectedIndexChanged);    

                string sqlChucVu = @"SELECT * FROM TB_CHUCVU";
                daChucVu = new SqlDataAdapter(sqlChucVu, conn);
                daChucVu.Fill(ds, "ChucVu");
                cboChucVu.DataSource = ds.Tables["ChucVu"];
                cboChucVu.DisplayMember = "TENCV";
                cboChucVu.ValueMember = "IDCV";

                //--------------------//
                // === SỬA LỖI 2: Xóa 'DIENTHOAI' khỏi câu SELECT ===
                // (Câu SELECT của bạn đã xóa rồi, rất tốt! Tôi giữ nguyên)
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
                                          LEFT JOIN TB_TRINHDO t ON n.IDTD = t.IDTD";

                daNhanVien = new SqlDataAdapter(sqlNhanVien, conn);
                daNhanVien.Fill(ds, "NhanVien");

                // === SỬA LỖI 3: Thêm Khóa Chính cho DataSet (QUAN TRỌNG!) ===
                // Giúp DataSet tìm, sửa, xóa nhanh và chính xác
                ds.Tables["NhanVien"].PrimaryKey = new DataColumn[] { ds.Tables["NhanVien"].Columns["MANV"] };

                dgvNhanVien.DataSource = ds.Tables["NhanVien"];

                // (Code cấu hình cột của bạn đã đúng)
                dgvNhanVien.Columns["MANV"].HeaderText = "Mã NV";
                dgvNhanVien.Columns["MANV"].Width = 70;
                dgvNhanVien.Columns["HOTEN"].HeaderText = "Họ tên nhân viên";
                dgvNhanVien.Columns["HOTEN"].Width = 200;
                dgvNhanVien.Columns["GIOITINH"].HeaderText = "Gioi tinh";
                dgvNhanVien.Columns["GIOITINH"].Width = 60;
                dgvNhanVien.Columns["NGAYSINH"].HeaderText = "Ngay Sinh";
                dgvNhanVien.Columns["NGAYSINH"].Width = 100;
                dgvNhanVien.Columns["CCCD"].HeaderText = "CCCD";
                dgvNhanVien.Columns["CCCD"].Width = 125;
                dgvNhanVien.Columns["DIACHI"].HeaderText = "Địa chỉ";
                dgvNhanVien.Columns["DIACHI"].Width = 150;
                dgvNhanVien.Columns["TENPB"].HeaderText = "Tên Phòng Ban";
                dgvNhanVien.Columns["TENPB"].Width = 150;
                dgvNhanVien.Columns["TENBP"].HeaderText = "Tên Bộ Phận";
                dgvNhanVien.Columns["TENBP"].Width = 150;
                dgvNhanVien.Columns["TENCV"].HeaderText = "Tên Chức Vụ";
                dgvNhanVien.Columns["TENCV"].Width = 150;
                dgvNhanVien.Columns["TENTD"].HeaderText = "Tên Trình Độ";
                dgvNhanVien.Columns["TENTD"].Width = 150;
                dgvNhanVien.Columns["IDPB"].Visible = false;
                dgvNhanVien.Columns["IDBP"].Visible = false ;
                dgvNhanVien.Columns["IDCV"].Visible = false;
                dgvNhanVien.Columns["IDTD"].Visible = false;

                // ... (các cột khác) ...

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
            else if((!System.Text.RegularExpressions.Regex.IsMatch(txtMaNV.Text, @"^NV\d+$")))
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

                // (Tùy chọn) Tải lại dữ liệu "sạch" từ CSDL sau khi lưu
                // LoadDataGridViewNhanVien(); 
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
            // Đây là nơi chúng ta tải lại ComboBox Phòng Ban
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
            if(this.txtHoTen.Text =="")
            {
                e.Cancel = true;
                errorProvider.SetError(txtHoTen, "Không được để trống họ tên");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtHoTen, "");
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
    }
}