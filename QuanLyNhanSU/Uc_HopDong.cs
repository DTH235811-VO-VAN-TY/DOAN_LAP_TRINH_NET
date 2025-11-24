using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class Uc_HopDong : UserControl
    {
        public Uc_HopDong()
        {
            InitializeComponent();
        }
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daNhanVien;
        SqlDataAdapter daHopDong;

        private void Uc_HopDong_Load(object sender, EventArgs e)
        {
            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng thêm/sửa/xóa
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnLamMoi.Enabled = false;
                btnInHD.Enabled = false;
                // (Nếu có nút Hủy hay Làm mới thì tắt nốt nếu muốn)

                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox) ((TextBox)c).ReadOnly = true;
                }

                // 3. GridView chỉ cho xem
                dgvHopDong.ReadOnly = true;
            }
            // Thoát nếu đang ở chế độ Design
            txtTenNV.Enabled = false;
            if (this.DesignMode) return;

            try
            {
                // Cấu hình DGV
                dgvHopDong.AutoGenerateColumns = false;
                dgvHopDong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                conn = new SqlConnection(connString);
                conn.Open();

                // 1. Tải ComboBox Nhân Viên (dùng để Thêm/Sửa)
                string sQueryNhanVien = @"SELECT * FROM tb_NHANVIEN WHERE DATHOIVIEC = 0 OR DATHOIVIEC IS NULL";
                daNhanVien = new SqlDataAdapter(sQueryNhanVien, conn);
                daNhanVien.Fill(ds, "tblNHANVIEN");
                cboMaNV.DataSource = ds.Tables["tblNHANVIEN"];
                cboMaNV.DisplayMember = "MANV";
                cboMaNV.ValueMember = "MANV";
                cboMaNV.SelectedIndex = -1; // Mặc định không chọn ai

                // 2. Tải ComboBox Thời Hạn (dữ liệu tĩnh)
                cboThoiHan.Items.Add("3 tháng");
                cboThoiHan.Items.Add("6 tháng");
                cboThoiHan.Items.Add("1 năm");
                cboThoiHan.Items.Add("2 năm");
                cboThoiHan.Items.Add("Vô thời hạn");

                // 3. Tải DataGridView Hợp Đồng (hiển thị tất cả)
                string sQueryHopDong = @"SELECT hd.*, nv.HOTEN 
                                        FROM TB_HOPDONG hd
                                        LEFT JOIN TB_NHANVIEN nv ON hd.MANV = nv.MANV";
                daHopDong = new SqlDataAdapter(sQueryHopDong, conn);
                daHopDong.Fill(ds, "tblHOPDONG");
                dgvHopDong.DataSource = ds.Tables["tblHOPDONG"];

                // 4. Gán DataPropertyName (QUAN TRỌNG: Ghi đè thuộc tính trong Designer)
                dgvHopDong.Columns["SoHD"].DataPropertyName = "SOHD";
                dgvHopDong.Columns["Tennhanvien"].DataPropertyName = "HOTEN"; // Lấy từ JOIN
                dgvHopDong.Columns["Lanky"].DataPropertyName = "LANKY";
                dgvHopDong.Columns["Ngayky"].DataPropertyName = "NGAYKY";
                dgvHopDong.Columns["Ngaybatdau"].DataPropertyName = "NGAYBATDAU";
                dgvHopDong.Columns["Ngayketthuc"].DataPropertyName = "NGAYKETTHUC";
                dgvHopDong.Columns["Hesoluong"].DataPropertyName = "HESOLUONG";
                dgvHopDong.Columns["Thoihan"].DataPropertyName = "THOIHAN";
                dgvHopDong.Columns["Noidung"].DataPropertyName = "NOIDUNG";
                dgvHopDong.Columns["Manv"].DataPropertyName = "MANV"; // Ẩn, dùng để lưu
                dgvHopDong.Columns["Manv"].Visible = false;

                // 5. Định nghĩa InsertCommand
                string sThem = @"INSERT INTO TB_HOPDONG (SOHD, MANV, LANKY, NGAYKY, NGAYBATDAU, NGAYKETTHUC, HESOLUONG, THOIHAN, NOIDUNG) 
                                 VALUES (@SOHD, @MANV, @LANKY, @NGAYKY, @NGAYBATDAU, @NGAYKETTHUC, @HESOLUONG, @THOIHAN, @NOIDUNG)";
                SqlCommand cmdThem = new SqlCommand(sThem, conn);
                cmdThem.Parameters.Add("@SOHD", SqlDbType.NVarChar, 10, "SOHD");
                cmdThem.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
                cmdThem.Parameters.Add("@LANKY", SqlDbType.Int, 4, "LANKY");
                cmdThem.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
                cmdThem.Parameters.Add("@NGAYBATDAU", SqlDbType.Date, 0, "NGAYBATDAU");
                cmdThem.Parameters.Add("@NGAYKETTHUC", SqlDbType.Date, 0, "NGAYKETTHUC");
                cmdThem.Parameters.Add("@HESOLUONG", SqlDbType.Float, 8, "HESOLUONG");
                cmdThem.Parameters.Add("@THOIHAN", SqlDbType.NVarChar, 50, "THOIHAN");
                cmdThem.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
                daHopDong.InsertCommand = cmdThem;

                // 6. Định nghĩa UpdateCommand
                string sSua = @"UPDATE TB_HOPDONG SET 
                                    MANV=@MANV, LANKY=@LANKY, NGAYKY=@NGAYKY, 
                                    NGAYBATDAU=@NGAYBATDAU, NGAYKETTHUC=@NGAYKETTHUC, 
                                    HESOLUONG=@HESOLUONG, THOIHAN=@THOIHAN, NOIDUNG=@NOIDUNG 
                                WHERE SOHD=@SOHD";
                SqlCommand cmdSua = new SqlCommand(sSua, conn);
                cmdSua.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
                cmdSua.Parameters.Add("@LANKY", SqlDbType.Int, 4, "LANKY");
                cmdSua.Parameters.Add("@NGAYKY", SqlDbType.Date, 0, "NGAYKY");
                cmdSua.Parameters.Add("@NGAYBATDAU", SqlDbType.Date, 0, "NGAYBATDAU");
                cmdSua.Parameters.Add("@NGAYKETTHUC", SqlDbType.Date, 0, "NGAYKETTHUC");
                cmdSua.Parameters.Add("@HESOLUONG", SqlDbType.Float, 8, "HESOLUONG");
                cmdSua.Parameters.Add("@THOIHAN", SqlDbType.NVarChar, 50, "THOIHAN");
                cmdSua.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
                cmdSua.Parameters.Add("@SOHD", SqlDbType.NVarChar, 10, "SOHD"); // Khóa chính
                daHopDong.UpdateCommand = cmdSua;

                // 7. Định nghĩa DeleteCommand
                string sXoa = @"DELETE FROM TB_HOPDONG WHERE SOHD=@SOHD";
                SqlCommand cmdXoa = new SqlCommand(sXoa, conn);
                cmdXoa.Parameters.Add("@SOHD", SqlDbType.NVarChar, 10, "SOHD");
                daHopDong.DeleteCommand = cmdXoa;

                // 8. Đặt Primary Key cho DataSet (để .Find() hoạt động)
                ds.Tables["tblHOPDONG"].PrimaryKey = new DataColumn[] { ds.Tables["tblHOPDONG"].Columns["SOHD"] };
            }
            catch (Exception ex)
            {
                MessageBox.Show("LỖI GỐC KHI TẢI FORM: " + ex.Message, "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadSearchComboBox();
        }

        /// <summary>
        /// Hàm tiện ích để làm mới các ô nhập liệu
        /// </summary>
        /// 
        public void ReloadNhanVien()
        {
            try
            {
                // Xóa dữ liệu cũ trong DataSet (bảng nhân viên)
                if (ds.Tables.Contains("tblNHANVIEN"))
                    ds.Tables["tblNHANVIEN"].Clear();

                // Tải lại từ CSDL
                // (Đảm bảo daNhanVien đã được khởi tạo trong Load, nếu chưa thì phải khởi tạo lại)
                if (daNhanVien == null)
                {
                    string sql = "SELECT MANV, HOTEN FROM TB_NHANVIEN";
                    daNhanVien = new SqlDataAdapter(sql, conn);
                }

                daNhanVien.Fill(ds, "tblNHANVIEN");

                // Gán lại DataSource (để làm mới ComboBox)
                cboMaNV.DataSource = ds.Tables["tblNHANVIEN"];
                cboMaNV.DisplayMember = "MANV";
                cboMaNV.ValueMember = "MANV";
            }
            catch (Exception ex) { /* Xử lý lỗi nếu cần */ }
        }
        private void LamMoiControls()
        {
            txtSoHD.Text = "";
            txtSoHD.Enabled = true; // Bật lại để cho phép Thêm mới
            cboMaNV.SelectedIndex = -1;
            numericUpDown1.Value = 1; // (numericUpDown1 là tên của Lần Ký)
            dtpNgayKy.Value = DateTime.Now;
            dtpNgayBatDau.Value = DateTime.Now;
            dtpNgayKetThuc.Value = DateTime.Now;
            txtHSL.Text = "0.0";
            numericUpDown1.Value = 0;
            //nudHSL.Value = 0;
            cboThoiHan.SelectedIndex = -1;
            txtNoiDung.Text = "";
            dgvHopDong.ClearSelection();
        }

        // --- CODE CHO CÁC NÚT BẤM ---
        // (Bạn cần "nối dây" các sự kiện Click này trong Designer)

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = daHopDong.Update(ds, "tblHOPDONG");
                MessageBox.Show($"Đã lưu {rowsAffected} thay đổi vào CSDL thành công!");

                // Tải lại DGV
                ds.Tables["tblHOPDONG"].Clear();
                daHopDong.Fill(ds, "tblHOPDONG");

                LamMoiControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ds.Tables["tblHOPDONG"].RejectChanges();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra rỗng
            if (string.IsNullOrEmpty(txtSoHD.Text) || cboMaNV.SelectedValue == null)
            {
                MessageBox.Show("Số Hợp Đồng và Tên Nhân Viên là bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoHD.Text, @"^HDLD/\d+$"))
            {
                MessageBox.Show("Định dạng không hợp lệ!\nVui lòng nhập theo mẫu: HDLD/xxxx (Ví dụ: HDLD/2025)", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoHD.Focus();
                return; // <--- Thêm lệnh này để dừng việc thêm mới
            }

            // 3. Kiểm tra trùng mã trong bộ nhớ (DataSet)
            if (ds.Tables["tblHOPDONG"].Rows.Find(txtSoHD.Text) != null)
            {
                MessageBox.Show("Số Hợp Đồng này đã tồn tại trong danh sách!", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Kiểm tra ngày tháng
            if (dtpNgayKetThuc.Value <= dtpNgayBatDau.Value)
            {
                MessageBox.Show("Ngày Kết Thúc phải sau Ngày Bắt Đầu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(txtHSL.Text=="")
            {
                MessageBox.Show("Hệ Số Lương không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            double heSoLuong;
            if (!double.TryParse(txtHSL.Text, out heSoLuong) || heSoLuong <= 0)
            {
                MessageBox.Show("Hệ số lương phải là số và lớn hơn 0 (Ví dụ: 2.34)!", "Lỗi nhập liệu");
                txtHSL.Focus();
                return;
            }
            if (cboThoiHan.Text == "")
            {
                MessageBox.Show("Vui lòng chọn Thời Hạn hợp đồng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // --- Nếu qua hết các ải trên thì mới thực hiện Thêm ---
            try
            {
                DataRow newRow = ds.Tables["tblHOPDONG"].NewRow();
                newRow["SOHD"] = txtSoHD.Text.Trim(); // Trim() để xóa khoảng trắng thừa nếu có
                newRow["MANV"] = cboMaNV.SelectedValue;
                newRow["HOTEN"] = cboMaNV.Text;
                newRow["LANKY"] = numericUpDown1.Value;
                newRow["NGAYKY"] = dtpNgayKy.Value;
                newRow["NGAYBATDAU"] = dtpNgayBatDau.Value;
                newRow["NGAYKETTHUC"] = dtpNgayKetThuc.Value;

                // Kiểm tra parse số liệu an toàn hơn
                
                newRow["HESOLUONG"] = heSoLuong;

                newRow["THOIHAN"] = cboThoiHan.SelectedItem ?? "Vô thời hạn"; // Xử lý null
                newRow["NOIDUNG"] = txtNoiDung.Text;

                ds.Tables["tblHOPDONG"].Rows.Add(newRow);

                MessageBox.Show("Đã thêm vào bộ nhớ tạm. Hãy nhấn nút 'Lưu' để ghi vào CSDL.");
                LamMoiControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dòng: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtSoHD.Enabled == true || string.IsNullOrEmpty(txtSoHD.Text))
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng từ lưới để sửa.");
                return;
            }
            if (cboMaNV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Tên Nhân Viên.");
                return;
            }

            // Tìm dòng bằng Khóa Chính (SOHD)
            DataRow rowToUpdate = ds.Tables["tblHOPDONG"].Rows.Find(txtSoHD.Text);

            if (rowToUpdate != null)
            {
                rowToUpdate.BeginEdit();
                rowToUpdate["MANV"] = cboMaNV.SelectedValue;
                rowToUpdate["HOTEN"] = cboMaNV.Text;
                rowToUpdate["LANKY"] = numericUpDown1.Value;
                rowToUpdate["NGAYKY"] = dtpNgayKy.Value;
                rowToUpdate["NGAYBATDAU"] = dtpNgayBatDau.Value;
                rowToUpdate["NGAYKETTHUC"] = dtpNgayKetThuc.Value;
                rowToUpdate["HESOLUONG"] = txtHSL.Text;
                rowToUpdate["THOIHAN"] = cboThoiHan.SelectedItem;
                rowToUpdate["NOIDUNG"] = txtNoiDung.Text;
                rowToUpdate.EndEdit();

                MessageBox.Show("Đã sửa trong bộ nhớ. Nhấn 'Lưu' để cập nhật CSDL.");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHopDong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng từ lưới để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa Hợp đồng: " + txtSoHD.Text + "?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            DataRow rowToDelete = ds.Tables["tblHOPDONG"].Rows.Find(txtSoHD.Text);
            if (rowToDelete != null)
            {
                rowToDelete.Delete();
                MessageBox.Show("Đã xóa trong bộ nhớ. Nhấn 'Lưu' để cập nhật CSDL.");
                LamMoiControls();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiControls();
            // Hủy các thay đổi chưa được lưu
            ds.Tables["tblHOPDONG"].RejectChanges();
        }

        private void dgvHopDong_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHopDong.SelectedRows.Count > 0)
                {
                    DataGridViewRow dr = dgvHopDong.SelectedRows[0];
                    // Lấy DataRowView để lấy dữ liệu gốc an toàn
                    DataRowView drv = dr.DataBoundItem as DataRowView;
                    if (drv == null) return;

                    // Gán dữ liệu lên controls
                    txtSoHD.Text = drv["SOHD"].ToString();
                    cboMaNV.SelectedValue = drv["MANV"];
                    numericUpDown1.Value = Convert.ToDecimal(drv["LANKY"]);
                    dtpNgayKy.Value = Convert.ToDateTime(drv["NGAYKY"]);
                    dtpNgayBatDau.Value = Convert.ToDateTime(drv["NGAYBATDAU"]);
                    dtpNgayKetThuc.Value = Convert.ToDateTime(drv["NGAYKETTHUC"]);
                    txtHSL.Text = drv["HESOLUONG"].ToString();
                    //nudHSL.Value = Convert.ToDecimal(drv["HESOLUONG"]);
                    cboThoiHan.SelectedItem = drv["THOIHAN"].ToString();
                    txtNoiDung.Text = drv["NOIDUNG"].ToString();

                    // Khóa khóa chính lại, không cho sửa
                    txtSoHD.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn hàng: " + ex.Message);
            }
        }

        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboMaNV.SelectedItem != null)
            {
                DataRowView drv = cboMaNV.SelectedItem as DataRowView;
                txtTenNV.Text = drv["HOTEN"].ToString();
            }
            else
            {
                txtTenNV.Text = "";
            }
        }
        private string _in_SoHD = "";
        private string _in_HoTen = "";
        private string _in_SinhNgay = "";
        private string _in_CCCD = "";
        private string _in_DiaChi = "";
        private string _in_ChucVu = "";
        private string _in_PhongBan = "";
        private string _in_BoPhan = "";
        private string _in_TrinhDo = "";
        private string _in_LoaiHD = ""; // Thời hạn
        private string _in_TuNgay = "";
        private string _in_DenNgay = "";
        private string _in_HeSoLuong = "";
        private string _in_NgayKy = "";
        private void btnInHD_Click(object sender, EventArgs e)
        {
            if (dgvHopDong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng để in!");
                return;
            }

            // 1. Lấy Mã NV và Số HĐ từ dòng đang chọn
            string maNV = dgvHopDong.SelectedRows[0].Cells["MANV"].Value.ToString(); // Đảm bảo cột MANV có tồn tại (có thể ẩn)
            string soHD = dgvHopDong.SelectedRows[0].Cells["SoHD"].Value.ToString();

            // 2. Truy vấn CSDL để lấy đầy đủ thông tin chi tiết (Join nhiều bảng)
            if (LayThongTinChiTietHopDong(maNV, soHD))
            {
                // 3. Tiến hành in
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // Khổ A4 đứng
                pd.PrintPage += new PrintPageEventHandler(this.pd_InHopDong_PrintPage);

                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ppd.Document = pd;
                ppd.Width = 900;
                ppd.Height = 700;
                ppd.ShowDialog();
            }
        }

        // Hàm lấy dữ liệu chi tiết
        private bool LayThongTinChiTietHopDong(string maNV, string soHD)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Câu truy vấn lấy TẤT CẢ thông tin cần thiết
                string sql = @"SELECT 
                                nv.HOTEN, nv.NGAYSINH, nv.CCCD, nv.DIACHI,
                                pb.TENPB, cv.TENCV, bp.TENBP, td.TENTD,
                                hd.SOHD, hd.THOIHAN, hd.NGAYBATDAU, hd.NGAYKETTHUC, hd.HESOLUONG, hd.NGAYKY
                               FROM TB_HOPDONG hd
                               JOIN TB_NHANVIEN nv ON hd.MANV = nv.MANV
                               LEFT JOIN TB_PHONGBAN pb ON nv.IDPB = pb.IDPB
                               LEFT JOIN TB_CHUCVU cv ON nv.IDCV = cv.IDCV
                               LEFT JOIN TB_BOPHAN bp ON nv.IDBP = bp.IDBP
                               LEFT JOIN TB_TRINHDO td ON nv.IDTD = td.IDTD
                               WHERE hd.SOHD = @SOHD";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SOHD", soHD);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    _in_SoHD = dr["SOHD"].ToString();
                    _in_HoTen = dr["HOTEN"].ToString();
                    _in_SinhNgay = Convert.ToDateTime(dr["NGAYSINH"]).ToString("dd/MM/yyyy");
                    _in_CCCD = dr["CCCD"].ToString();
                    _in_DiaChi = dr["DIACHI"].ToString();
                    _in_PhongBan = dr["TENPB"].ToString();
                    _in_ChucVu = dr["TENCV"].ToString();
                    _in_BoPhan = dr["TENBP"].ToString();
                    _in_TrinhDo = dr["TENTD"].ToString();

                    _in_LoaiHD = dr["THOIHAN"].ToString();
                    _in_TuNgay = Convert.ToDateTime(dr["NGAYBATDAU"]).ToString("dd/MM/yyyy");

                    // Xử lý ngày kết thúc (nếu vô thời hạn thì có thể null)
                    if (dr["NGAYKETTHUC"] != DBNull.Value)
                        _in_DenNgay = Convert.ToDateTime(dr["NGAYKETTHUC"]).ToString("dd/MM/yyyy");
                    else
                        _in_DenNgay = "...";

                    _in_HeSoLuong = dr["HESOLUONG"].ToString();
                    _in_NgayKy = Convert.ToDateTime(dr["NGAYKY"]).ToString("dd/MM/yyyy");

                    dr.Close();
                    return true;
                }
                dr.Close();
                MessageBox.Show("Không tìm thấy dữ liệu chi tiết!");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu in: " + ex.Message);
                return false;
            }
        }

        private void pd_InHopDong_PrintPage(object sender, PrintPageEventArgs e)
        {
            // --- CẤU HÌNH FONT ---
            Font fontQuocHieu = new Font("Times New Roman", 12, FontStyle.Bold);
            Font fontTieuDe = new Font("Times New Roman", 18, FontStyle.Bold);
            Font fontDam = new Font("Times New Roman", 11, FontStyle.Bold); // Font nội dung đậm
            Font fontThuong = new Font("Times New Roman", 11, FontStyle.Regular); // Font nội dung thường
            Font fontNghieng = new Font("Times New Roman", 11, FontStyle.Italic);

            float y = e.MarginBounds.Top;
            float leftMargin = e.MarginBounds.Left;
            float rightMargin = e.MarginBounds.Right;
            float pageCenter = e.PageBounds.Width / 2;
            float w = e.MarginBounds.Width;

            StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };

            // --- 1. VẼ QUỐC HIỆU TIÊU NGỮ ---
            e.Graphics.DrawString("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM", fontQuocHieu, Brushes.Black, pageCenter, y, centerFormat);
            y += 20;
            e.Graphics.DrawString("Độc lập - Tự do - Hạnh phúc", fontDam, Brushes.Black, pageCenter, y, centerFormat);
            y += 20;
            e.Graphics.DrawString("----------------", fontDam, Brushes.Black, pageCenter, y, centerFormat);
            y += 40;

            // --- 2. TIÊU ĐỀ HỢP ĐỒNG ---
            e.Graphics.DrawString("HỢP ĐỒNG LAO ĐỘNG", fontTieuDe, Brushes.Black, pageCenter, y, centerFormat);
            y += 30;
            e.Graphics.DrawString($"Số: {_in_SoHD}", fontNghieng, Brushes.Black, pageCenter, y, centerFormat);
            y += 40;

            // --- 3. NỘI DUNG ---
            // Hàm vẽ dòng text kết hợp dữ liệu (Helper nhỏ)
            void DrawLine(string label, string value, Font fLabel, Font fValue, float yPos)
            {
                e.Graphics.DrawString(label, fLabel, Brushes.Black, leftMargin, yPos);
                SizeF sizeLabel = e.Graphics.MeasureString(label, fLabel);
                e.Graphics.DrawString(value, fValue, Brushes.Black, leftMargin + sizeLabel.Width, yPos);
            }

            e.Graphics.DrawString("Hôm nay, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + ", tại Công ty ABC, chúng tôi gồm:", fontNghieng, Brushes.Black, leftMargin, y);
            y += 30;

            // === BÊN A ===
            e.Graphics.DrawString("BÊN A (Người sử dụng lao động): CÔNG TY CÔNG NGHỆ PHẦN MỀM VĂN TỶ", fontDam, Brushes.Black, leftMargin, y);
            y += 25;
            e.Graphics.DrawString("Đại diện: Ông Võ Văn Tỷ", fontThuong, Brushes.Black, leftMargin, y);
            y += 25;
            e.Graphics.DrawString("Chức vụ: Tổng Giám Đốc", fontThuong, Brushes.Black, leftMargin, y);
            y += 25;
            e.Graphics.DrawString("Địa chỉ: Số 123, Đường  Nguyễn Văn Linh, Phường Long Xuyên, An Giang", fontThuong, Brushes.Black, leftMargin, y);
            y += 40;

            // === BÊN B ===
            e.Graphics.DrawString("BÊN B (Người lao động):", fontDam, Brushes.Black, leftMargin, y);
            y += 25;
            DrawLine("Ông/Bà: ", _in_HoTen.ToUpper(), fontThuong, fontDam, y);
            y += 25;
            DrawLine("Sinh ngày: ", _in_SinhNgay, fontThuong, fontThuong, y);
            y += 25;
            DrawLine("Số CCCD/CMND: ", _in_CCCD, fontThuong, fontDam, y);
            y += 25;
            DrawLine("Địa chỉ thường trú: ", _in_DiaChi, fontThuong, fontThuong, y);
            y += 40;

            e.Graphics.DrawString("Thỏa thuận ký kết hợp đồng lao động và cam kết làm đúng những điều khoản sau đây:", fontNghieng, Brushes.Black, leftMargin, y);
            y += 30;

            // === ĐIỀU KHOẢN ===
            // Điều 1
            e.Graphics.DrawString("Điều 1: Thời hạn và công việc hợp đồng", fontDam, Brushes.Black, leftMargin, y);
            y += 25;
            DrawLine("- Loại hợp đồng: ", _in_LoaiHD, fontThuong, fontDam, y);
            y += 25;
            DrawLine("- Từ ngày: ", _in_TuNgay + "   đến ngày: " + _in_DenNgay, fontThuong, fontThuong, y);
            y += 25;
            DrawLine("- Địa điểm làm việc: ", _in_PhongBan + " - " + _in_BoPhan, fontThuong, fontDam, y);
            y += 25;
            DrawLine("- Chức danh chuyên môn: ", _in_ChucVu, fontThuong, fontDam, y);
            y += 25;
            DrawLine("- Trình độ chuyên môn: ", _in_TrinhDo, fontThuong, fontThuong, y);
            y += 40;

            // Điều 2
            e.Graphics.DrawString("Điều 2: Chế độ làm việc và lương thưởng", fontDam, Brushes.Black, leftMargin, y);
            y += 25;
            e.Graphics.DrawString("- Thời gian làm việc: 8 giờ/ngày, từ thứ 2 đến thứ 6.", fontThuong, Brushes.Black, leftMargin, y);
            y += 25;
            DrawLine("- Hệ số lương cơ bản: ", _in_HeSoLuong, fontThuong, fontDam, y);
            y += 25;
            e.Graphics.DrawString("- Được hưởng các chế độ BHYT, BHXH theo quy định của nhà nước.", fontThuong, Brushes.Black, leftMargin, y);
            y += 25;
            e.Graphics.DrawString("- Được thưởng lễ, tết theo tình hình kinh doanh của công ty.", fontThuong, Brushes.Black, leftMargin, y);
            y += 50;

            // === CHỮ KÝ ===
            float xBenA = leftMargin + 50;
            float xBenB = e.MarginBounds.Right - 250;

            e.Graphics.DrawString("NGƯỜI LAO ĐỘNG", fontDam, Brushes.Black, xBenA, y);
            e.Graphics.DrawString("NGƯỜI SỬ DỤNG LAO ĐỘNG", fontDam, Brushes.Black, xBenB, y);
            y += 20;
            e.Graphics.DrawString("(Ký, ghi rõ họ tên)", fontNghieng, Brushes.Black, xBenA + 10, y);
            e.Graphics.DrawString("(Ký, đóng dấu)", fontNghieng, Brushes.Black, xBenB + 30, y);

            // Chừa chỗ ký tên
            y += 100;
            e.Graphics.DrawString(_in_HoTen, fontDam, Brushes.Black, xBenA, y);
            e.Graphics.DrawString("Võ Văn Tỷ", fontDam, Brushes.Black, xBenB + 20, y);
        }
        private void LoadSearchComboBox()
        {
            var searchOptions = new[] {
                new { Text = "Tìm theo Số HĐ", Value = "SOHD" },
                new { Text = "Tìm theo Họ Tên", Value = "HOTEN" }
            };
            // (Sửa lại tên 'cboTimKiemTheo' nếu tên của bạn khác)
            cboTimKiemNV.DataSource = searchOptions;
            cboTimKiemNV.DisplayMember = "Text";
            cboTimKiemNV.ValueMember = "Value";
            cboTimKiemNV.SelectedIndex = 0;
        }
        private void btnTiemKiemNV_Click(object sender, EventArgs e)
        {
            // 1. Lấy từ khóa từ ô TEXTBOX (nơi người dùng nhập), không phải ComboBox
            string key = txtTimKiemNV.Text.Trim();

            // Xử lý ký tự đặc biệt để tránh lỗi (ví dụ dấu nháy đơn)
            key = key.Replace("'", "''");

            // 2. Nếu ô nhập trống thì hiện tất cả
            if (string.IsNullOrEmpty(key))
            {
                ds.Tables["tblHOPDONG"].DefaultView.RowFilter = string.Empty;
                return;
            }

            // 3. Xác định cột cần tìm dựa trên ValueMember đã cài đặt ở hàm LoadSearchComboBox
            // ValueMember bạn đã đặt là "SOHD" và "HOTEN" nên ta lấy trực tiếp luôn
            string colName = cboTimKiemNV.SelectedValue.ToString();

            // 4. Thiết lập bộ lọc (RowFilter)
            try
            {
                // Câu lệnh này tương đương: WHERE SOHD LIKE '%...%' hoặc WHERE HOTEN LIKE '%...%'
                ds.Tables["tblHOPDONG"].DefaultView.RowFilter = $"{colName} LIKE '%{key}%'";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void btnHienAllNV_Click(object sender, EventArgs e)
        {
            txtTimKiemNV.Text = "";
            ds.Tables["tblHOPDONG"].DefaultView.RowFilter = string.Empty;
        }
    }
}