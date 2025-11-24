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
    public partial class Uc_PhuCap : UserControl
    {
        // Chuỗi kết nối
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daNhanVien;
        SqlDataAdapter daLoaiPhuCap; // Dùng để lấy danh sách các loại phụ cấp
        SqlDataAdapter daNV_PhuCap;  // Dùng để thao tác bảng TB_NHANVIEN_PHUCAP
        public event EventHandler DataUpdated;

        public Uc_PhuCap()
        {
            InitializeComponent();
        }

        private void Uc_PhuCap_Load(object sender, EventArgs e)
        {
            if (Const.LoaiTaiKhoan == 2) // Nếu là NHÂN VIÊN
            {
                // 1. Tắt hết các nút chức năng thêm/sửa/xóa
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnLamMoi.Enabled = false;
               // btnInHD.Enabled = false;
                // (Nếu có nút Hủy hay Làm mới thì tắt nốt nếu muốn)

                // 2. Các ô nhập liệu chỉ cho đọc
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox) ((TextBox)c).ReadOnly = true;
                }

                // 3. GridView chỉ cho xem
                dgvPhuCap.ReadOnly = true;
            }
            if (this.DesignMode) return;

            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                // 1. Tải ComboBox Nhân Viên
                string sqlNV = "SELECT * FROM tb_NHANVIEN WHERE DATHOIVIEC = 0 OR DATHOIVIEC IS NULL"; 
                daNhanVien = new SqlDataAdapter(sqlNV, conn);
                daNhanVien.Fill(ds, "tblNHANVIEN");

                cboMaNVPC.DataSource = ds.Tables["tblNHANVIEN"];
                cboMaNVPC.DisplayMember = "MANV"; // Hiển thị Mã
                cboMaNVPC.ValueMember = "MANV";   // Giá trị là Mã
                cboMaNVPC.SelectedIndex = -1;

                // 2. Tải ComboBox Loại Phụ Cấp (Lấy từ bảng TB_PHUCAP)
                string sqlLoaiPC = "SELECT IDPC, TENPC, SOTIEN FROM TB_PHUCAP";
                daLoaiPhuCap = new SqlDataAdapter(sqlLoaiPC, conn);
                daLoaiPhuCap.Fill(ds, "tblLOAIPHUCAP");

                cboTenPC.DataSource = ds.Tables["tblLOAIPHUCAP"];
                cboTenPC.DisplayMember = "TENPC"; // Hiển thị Tên PC
                cboTenPC.ValueMember = "IDPC";    // Giá trị là IDPC
                cboTenPC.SelectedIndex = -1;

                // Nối sự kiện để tự điền số tiền khi chọn loại PC
            //    cboTenPC.SelectedIndexChanged += CboTenPC_SelectedIndexChanged;


                // 3. Tải DataGridView (Bảng TB_NHANVIEN_PHUCAP)
                // JOIN để lấy HOTEN nhân viên và TENPC phụ cấp
                string sqlLoad = @"SELECT nvpc.*, nv.HOTEN, pc.TENPC 
                                   FROM TB_NHANVIEN_PHUCAP nvpc
                                   LEFT JOIN TB_NHANVIEN nv ON nvpc.MANV = nv.MANV
                                   LEFT JOIN TB_PHUCAP pc ON nvpc.IDPC = pc.IDPC";

                daNV_PhuCap = new SqlDataAdapter(sqlLoad, conn);
                daNV_PhuCap.Fill(ds, "tblNV_PHUCAP");
                dgvPhuCap.AutoGenerateColumns = false;
                dgvPhuCap.DataSource = ds.Tables["tblNV_PHUCAP"];

                // Gán DataPropertyName (Khớp với tên cột trong Designer)
                dgvPhuCap.Columns["Idphucap"].DataPropertyName = "ID";        // ID của bảng NHANVIEN_PHUCAP
                dgvPhuCap.Columns["MaNhanVien"].DataPropertyName = "MANV";
                dgvPhuCap.Columns["Tennhanvien"].DataPropertyName = "HOTEN";
                dgvPhuCap.Columns["Tenphucap"].DataPropertyName = "TENPC";    // Hiển thị tên thay vì IDPC
                dgvPhuCap.Columns["Ngayphucap"].DataPropertyName = "NGAY";
                dgvPhuCap.Columns["Sotienphucap"].DataPropertyName = "SOTIEN";

                DataColumn pk = ds.Tables["tblNV_PHUCAP"].Columns["ID"];
                pk.AutoIncrement = true;
                pk.AutoIncrementSeed = -1;
                pk.AutoIncrementStep = -1;

                // Cấu hình DataSet
                ds.Tables["tblNV_PHUCAP"].PrimaryKey = new DataColumn[] { ds.Tables["tblNV_PHUCAP"].Columns["ID"] };

                // --- Cấu hình INSERT/UPDATE/DELETE ---
                BuildCommands();

                // Cài đặt tìm kiếm
                LoadSearchComboBox();

                // Khóa các ô không cho nhập tay ID
                txtIDPC.Enabled = false; // ID tự tăng
                txtTenNVPC.ReadOnly = true; // Tự động điền
                // txtSotienPC.ReadOnly = true; // Có thể cho sửa hoặc không tùy bạn

                LamMoiControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải form: " + ex.Message);
            }
        }
        public void ReloadNhanVienPhuCap()
        {
            try
            {
                if (ds.Tables.Contains("tblNHANVIEN"))
                            {
                                ds.Tables["tblNHANVIEN"].Clear();
                            }
                if(daNV_PhuCap != null)
                {
                  string sqlNV = "SELECT MANV, HOTEN FROM TB_NHANVIEN WHERE DATHOIVIEC = 0 OR DATHOIVIEC IS NULL"; ;
                   daNhanVien = new SqlDataAdapter(sqlNV, conn);
                
                            }
             daNhanVien.Fill(ds, "tblNHANVIEN");
             cboMaNVPC.DataSource = ds.Tables["tblNHANVIEN"];
             cboMaNVPC.DisplayMember = "MANV"; // Hiển thị Mã
             cboMaNVPC.ValueMember = "MANV";   // Giá trị là Mã
             cboMaNVPC.SelectedIndex = -1;
            }catch(Exception ex)
            {
                MessageBox.Show("Lỗi tải lại nhân viên: " + ex.Message);
            }

        }

        private void BuildCommands()
        {
            // INSERT
            string sqlInsert = @"INSERT INTO TB_NHANVIEN_PHUCAP (IDPC, MANV, NGAY, NOIDUNG, SOTIEN) 
                                 VALUES (@IDPC, @MANV, @NGAY, @NOIDUNG, @SOTIEN)";
            SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
            cmdInsert.Parameters.Add("@IDPC", SqlDbType.Int, 4, "IDPC");
            cmdInsert.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            cmdInsert.Parameters.Add("@NGAY", SqlDbType.Date, 0, "NGAY");
            cmdInsert.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG"); // Giả sử bạn có cột NOIDUNG (nếu ko có thì xóa dòng này)
            cmdInsert.Parameters.Add("@SOTIEN", SqlDbType.Float, 8, "SOTIEN");
            daNV_PhuCap.InsertCommand = cmdInsert;

            // UPDATE
            string sqlUpdate = @"UPDATE TB_NHANVIEN_PHUCAP 
                                 SET IDPC=@IDPC, MANV=@MANV, NGAY=@NGAY, NOIDUNG=@NOIDUNG, SOTIEN=@SOTIEN 
                                 WHERE ID=@ID";
            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn);
            cmdUpdate.Parameters.Add("@IDPC", SqlDbType.Int, 4, "IDPC");
            cmdUpdate.Parameters.Add("@MANV", SqlDbType.NVarChar, 10, "MANV");
            cmdUpdate.Parameters.Add("@NGAY", SqlDbType.Date, 0, "NGAY");
            cmdUpdate.Parameters.Add("@NOIDUNG", SqlDbType.NVarChar, 255, "NOIDUNG");
            cmdUpdate.Parameters.Add("@SOTIEN", SqlDbType.Float, 8, "SOTIEN");
            cmdUpdate.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
            daNV_PhuCap.UpdateCommand = cmdUpdate;

            // DELETE
            string sqlDelete = @"DELETE FROM TB_NHANVIEN_PHUCAP WHERE ID=@ID";
            SqlCommand cmdDelete = new SqlCommand(sqlDelete, conn);
            cmdDelete.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
            daNV_PhuCap.DeleteCommand = cmdDelete;
        }

        // Sự kiện: Khi chọn Mã NV -> Tự điền Tên NV
        private void cboMaNVPC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNVPC.SelectedItem != null)
            {
                DataRowView drv = cboMaNVPC.SelectedItem as DataRowView;
                txtTenNVPC.Text = drv["HOTEN"].ToString();
            }
            else
            {
                txtTenNVPC.Text = "";
            }
        }

        // Sự kiện: Khi chọn Loại Phụ Cấp -> Tự điền Số Tiền
       

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboMaNVPC.SelectedValue == null || cboTenPC.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Nhân viên và Loại phụ cấp.");
                return;
            }

            DataRow row = ds.Tables["tblNV_PHUCAP"].NewRow();
            row["MANV"] = cboMaNVPC.SelectedValue;
            row["HOTEN"] = txtTenNVPC.Text; // Hiển thị ngay trên lưới
            row["IDPC"] = cboTenPC.SelectedValue;
            row["TENPC"] = cboTenPC.Text;   // Hiển thị ngay trên lưới
            row["NGAY"] = dtpNgayBatDau.Value;
            row["SOTIEN"] = double.Parse(txtSotienPC.Text.Trim());
            row["NOIDUNG"] = ""; // Nếu có textbox nội dung thì gán vào

            ds.Tables["tblNV_PHUCAP"].Rows.Add(row);
            MessageBox.Show("Đã thêm vào bộ nhớ. Nhấn LƯU để cập nhật CSDL.");
            LamMoiControls();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDPC.Text))
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa.");
                return;
            }

            int id = int.Parse(txtIDPC.Text);
            DataRow row = ds.Tables["tblNV_PHUCAP"].Rows.Find(id);

            if (row != null)
            {
                row.BeginEdit();
                row["MANV"] = cboMaNVPC.SelectedValue;
                row["HOTEN"] = txtTenNVPC.Text;
                row["IDPC"] = cboTenPC.SelectedValue;
                row["TENPC"] = cboTenPC.Text;
                row["NGAY"] = dtpNgayBatDau.Value;
                row["SOTIEN"] = double.Parse(txtSotienPC.Text.Trim());
                // row["NOIDUNG"] = ...
                row.EndEdit();
                MessageBox.Show("Đã sửa trong bộ nhớ. Nhấn LƯU để cập nhật CSDL.");
                LamMoiControls();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDPC.Text))
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = int.Parse(txtIDPC.Text);
                DataRow row = ds.Tables["tblNV_PHUCAP"].Rows.Find(id);
                if (row != null)
                {
                    row.Delete();
                    MessageBox.Show("Đã xóa trong bộ nhớ. Nhấn LƯU để cập nhật CSDL.");
                    LamMoiControls();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                int result = daNV_PhuCap.Update(ds, "tblNV_PHUCAP");
                MessageBox.Show($"Đã lưu thành công {result} bản ghi.");

                // Refresh lại dữ liệu để có ID mới (nếu vừa thêm)
                ds.Tables["tblNV_PHUCAP"].Clear();
                daNV_PhuCap.Fill(ds, "tblNV_PHUCAP");
                LamMoiControls();
                DataUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message);
                ds.Tables["tblNV_PHUCAP"].RejectChanges();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiControls();
            ds.Tables["tblNV_PHUCAP"].RejectChanges();
        }

        private void LamMoiControls()
        {
            txtIDPC.Text = "";
            cboMaNVPC.SelectedIndex = -1;
            txtTenNVPC.Text = "";
            cboTenPC.SelectedIndex = -1;
            txtSotienPC.Text = "";
            dtpNgayBatDau.Value = DateTime.Now;
            dgvPhuCap.ClearSelection();
        }

        // Sự kiện Click DataGridView (Quan trọng: Phải nối dây trong Designer)
        

        private void btnThemPCMoi_Click(object sender, EventArgs e)
        {
            add_ThemPhuCap_form themPC = new add_ThemPhuCap_form();
            // Nếu bạn muốn khi form con đóng lại, form cha tự reload loại phụ cấp:
            themPC.FormClosed += (s, args) =>
            {
                // Reload ComboBox Loại Phụ Cấp
                ds.Tables["tblLOAIPHUCAP"].Clear();
                daLoaiPhuCap.Fill(ds, "tblLOAIPHUCAP");
            };
            themPC.ShowDialog();
        }

        // --- Phần Tìm Kiếm ---
        private void LoadSearchComboBox()
        {
            cboTimKiemPC.Items.Add("Theo Mã NV");
            cboTimKiemPC.Items.Add("Theo Tên NV");
            cboTimKiemPC.SelectedIndex = 0;
        }

        

        

        private void cboTenPC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTenPC.SelectedItem != null)
            {
                DataRowView drv = cboTenPC.SelectedItem as DataRowView;
                txtSotienPC.Text = drv["SOTIEN"].ToString();
            }
            else
            {
                txtSotienPC.Text = "";
            }
        }

        private void dgvPhuCap_Click(object sender, EventArgs e)
        {
            if (dgvPhuCap.SelectedRows.Count > 0)
            {
                DataRowView drv = dgvPhuCap.SelectedRows[0].DataBoundItem as DataRowView;
                if (drv != null)
                {
                    txtIDPC.Text = drv["ID"].ToString();
                    cboMaNVPC.SelectedValue = drv["MANV"];
                    cboTenPC.SelectedValue = drv["IDPC"];
                    dtpNgayBatDau.Value = Convert.ToDateTime(drv["NGAY"]);
                    txtSotienPC.Text = drv["SOTIEN"].ToString();
                }
            }
        }

        private void btnTimPC_Click(object sender, EventArgs e)
        {
            string key = txtTimKiemNVPC.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                ds.Tables["tblNV_PHUCAP"].DefaultView.RowFilter = "";
                return;
            }

            if (cboTimKiemPC.SelectedIndex == 0) // Mã NV
            {
                ds.Tables["tblNV_PHUCAP"].DefaultView.RowFilter = $"MANV LIKE '%{key}%'";
            }
            else // Tên NV
            {
                ds.Tables["tblNV_PHUCAP"].DefaultView.RowFilter = $"HOTEN LIKE '%{key}%'";
            }
        }

        private void btnHienAll_Click(object sender, EventArgs e)
        {
            txtTimKiemNVPC.Text = "";
            ds.Tables["tblNV_PHUCAP"].DefaultView.RowFilter = "";
        }
    }
}