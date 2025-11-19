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
            // Thoát nếu đang ở chế độ Design
            if (this.DesignMode) return;

            try
            {
                // Cấu hình DGV
                dgvHopDong.AutoGenerateColumns = false;
                dgvHopDong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                conn = new SqlConnection(connString);
                conn.Open();

                // 1. Tải ComboBox Nhân Viên (dùng để Thêm/Sửa)
                string sQueryNhanVien = @"SELECT * FROM TB_NHANVIEN";
                daNhanVien = new SqlDataAdapter(sQueryNhanVien, conn);
                daNhanVien.Fill(ds, "tblNHANVIEN");
                cboTenNV.DataSource = ds.Tables["tblNHANVIEN"];
                cboTenNV.DisplayMember = "HOTEN";
                cboTenNV.ValueMember = "MANV";
                cboTenNV.SelectedIndex = -1; // Mặc định không chọn ai

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
        }

        /// <summary>
        /// Hàm tiện ích để làm mới các ô nhập liệu
        /// </summary>
        private void LamMoiControls()
        {
            txtSoHD.Text = "";
            txtSoHD.Enabled = true; // Bật lại để cho phép Thêm mới
            cboTenNV.SelectedIndex = -1;
            numericUpDown1.Value = 1; // (numericUpDown1 là tên của Lần Ký)
            dtpNgayKy.Value = DateTime.Now;
            dtpNgayBatDau.Value = DateTime.Now;
            dtpNgayKetThuc.Value = DateTime.Now;
            nudHSL.Value = 0;
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
            if (string.IsNullOrEmpty(txtSoHD.Text) || cboTenNV.SelectedValue == null)
            {
                MessageBox.Show("Số Hợp Đồng và Tên Nhân Viên là bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem SOHD đã tồn tại trong DataSet chưa
            if (ds.Tables["tblHOPDONG"].Rows.Find(txtSoHD.Text) != null)
            {
                MessageBox.Show("Số Hợp Đồng này đã tồn tại!");
                return;
            }

            DataRow newRow = ds.Tables["tblHOPDONG"].NewRow();
            newRow["SOHD"] = txtSoHD.Text;
            newRow["MANV"] = cboTenNV.SelectedValue;
            newRow["HOTEN"] = cboTenNV.Text; // Để hiển thị DGV ngay
            newRow["LANKY"] = numericUpDown1.Value; // (numericUpDown1 là Lần Ký)
            newRow["NGAYKY"] = dtpNgayKy.Value;
            newRow["NGAYBATDAU"] = dtpNgayBatDau.Value;
            newRow["NGAYKETTHUC"] = dtpNgayKetThuc.Value;
            newRow["HESOLUONG"] = nudHSL.Value;
            newRow["THOIHAN"] = cboThoiHan.SelectedItem;
            newRow["NOIDUNG"] = txtNoiDung.Text;

            ds.Tables["tblHOPDONG"].Rows.Add(newRow);
            MessageBox.Show("Đã thêm vào bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            LamMoiControls();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtSoHD.Enabled == true || string.IsNullOrEmpty(txtSoHD.Text))
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng từ lưới để sửa.");
                return;
            }
            if (cboTenNV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Tên Nhân Viên.");
                return;
            }

            // Tìm dòng bằng Khóa Chính (SOHD)
            DataRow rowToUpdate = ds.Tables["tblHOPDONG"].Rows.Find(txtSoHD.Text);

            if (rowToUpdate != null)
            {
                rowToUpdate.BeginEdit();
                rowToUpdate["MANV"] = cboTenNV.SelectedValue;
                rowToUpdate["HOTEN"] = cboTenNV.Text;
                rowToUpdate["LANKY"] = numericUpDown1.Value;
                rowToUpdate["NGAYKY"] = dtpNgayKy.Value;
                rowToUpdate["NGAYBATDAU"] = dtpNgayBatDau.Value;
                rowToUpdate["NGAYKETTHUC"] = dtpNgayKetThuc.Value;
                rowToUpdate["HESOLUONG"] = nudHSL.Value;
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
                    cboTenNV.SelectedValue = drv["MANV"];
                    numericUpDown1.Value = Convert.ToDecimal(drv["LANKY"]);
                    dtpNgayKy.Value = Convert.ToDateTime(drv["NGAYKY"]);
                    dtpNgayBatDau.Value = Convert.ToDateTime(drv["NGAYBATDAU"]);
                    dtpNgayKetThuc.Value = Convert.ToDateTime(drv["NGAYKETTHUC"]);
                    nudHSL.Value = Convert.ToDecimal(drv["HESOLUONG"]);
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
    }
}