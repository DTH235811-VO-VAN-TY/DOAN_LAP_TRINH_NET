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
    public partial class add_ThemPhuCap_form : Form
    {
        // Khai báo sự kiện để báo cho form cha biết đã có thay đổi
        public event EventHandler DataSaved;

        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daLoaiPhuCap;

        public add_ThemPhuCap_form()
        {
            InitializeComponent();
        }

        private void add_ThemPhuCap_form_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                // 1. Tải dữ liệu
                string sql = "SELECT * FROM TB_PHUCAP";
                daLoaiPhuCap = new SqlDataAdapter(sql, conn);
                daLoaiPhuCap.Fill(ds, "tblLOAIPHUCAP");

                // 2. Gán DGV
                dgvPhuCapMoi.AutoGenerateColumns = false;
                dgvPhuCapMoi.DataSource = ds.Tables["tblLOAIPHUCAP"];
                dgvPhuCapMoi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // 3. Map DataPropertyName (Khớp với tên cột trong SQL)
                // Lưu ý: Kiểm tra kỹ tên cột (Name) trong Designer của bạn
                dgvPhuCapMoi.Columns["Idphucap"].DataPropertyName = "IDPC";
                dgvPhuCapMoi.Columns["TenPhuCap"].DataPropertyName = "TENPC";
                dgvPhuCapMoi.Columns["Sotienphucap"].DataPropertyName = "SOTIEN";

                // 4. Cấu hình ID tự tăng trong DataSet (Tránh lỗi khi thêm)
                DataColumn pk = ds.Tables["tblLOAIPHUCAP"].Columns["IDPC"];
                pk.AutoIncrement = true;
                pk.AutoIncrementSeed = -1;
                pk.AutoIncrementStep = -1;
                ds.Tables["tblLOAIPHUCAP"].PrimaryKey = new DataColumn[] { pk };

                // 5. Xây dựng các lệnh Insert/Update/Delete
                BuildCommands();

                // 6. Khóa ô ID
                txtIdPC.Enabled = false;

                LamMoiControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải form: " + ex.Message);
            }
        }

        private void BuildCommands()
        {
            // INSERT (Không chèn IDPC vì là Identity)
            string sqlInsert = "INSERT INTO TB_PHUCAP (TENPC, SOTIEN) VALUES (@TENPC, @SOTIEN)";
            SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
            cmdInsert.Parameters.Add("@TENPC", SqlDbType.NVarChar, 100, "TENPC");
            cmdInsert.Parameters.Add("@SOTIEN", SqlDbType.Float, 8, "SOTIEN");
            daLoaiPhuCap.InsertCommand = cmdInsert;

            // UPDATE
            string sqlUpdate = "UPDATE TB_PHUCAP SET TENPC=@TENPC, SOTIEN=@SOTIEN WHERE IDPC=@IDPC";
            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn);
            cmdUpdate.Parameters.Add("@TENPC", SqlDbType.NVarChar, 100, "TENPC");
            cmdUpdate.Parameters.Add("@SOTIEN", SqlDbType.Float, 8, "SOTIEN");
            cmdUpdate.Parameters.Add("@IDPC", SqlDbType.Int, 4, "IDPC");
            daLoaiPhuCap.UpdateCommand = cmdUpdate;

            // DELETE
            string sqlDelete = "DELETE FROM TB_PHUCAP WHERE IDPC=@IDPC";
            SqlCommand cmdDelete = new SqlCommand(sqlDelete, conn);
            cmdDelete.Parameters.Add("@IDPC", SqlDbType.Int, 4, "IDPC");
            daLoaiPhuCap.DeleteCommand = cmdDelete;
        }

        private void btnThemPC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenPCMoi.Text) || string.IsNullOrEmpty(txtSoTien.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên và Số tiền.");
                return;
            }

            DataRow row = ds.Tables["tblLOAIPHUCAP"].NewRow();
            row["TENPC"] = txtTenPCMoi.Text;

            // Xử lý số tiền an toàn
            double soTien = 0;
            if (double.TryParse(txtSoTien.Text, out soTien))
            {
                row["SOTIEN"] = soTien;
            }
            else
            {
                MessageBox.Show("Số tiền không hợp lệ.");
                return;
            }

            ds.Tables["tblLOAIPHUCAP"].Rows.Add(row);
            MessageBox.Show("Đã thêm vào bộ nhớ. Nhấn LƯU để cập nhật.");
            LamMoiControls();
        }

        private void btnSuuPC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdPC.Text))
            {
                MessageBox.Show("Vui lòng chọn dòng để sửa.");
                return;
            }

            int id = int.Parse(txtIdPC.Text);
            DataRow row = ds.Tables["tblLOAIPHUCAP"].Rows.Find(id);

            if (row != null)
            {
                row.BeginEdit();
                row["TENPC"] = txtTenPCMoi.Text;
                double soTien = 0;
                double.TryParse(txtSoTien.Text, out soTien);
                row["SOTIEN"] = soTien;
                row.EndEdit();

                MessageBox.Show("Đã sửa trong bộ nhớ. Nhấn LƯU để cập nhật.");
                LamMoiControls();
            }
        }

        private void btnXoaPC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdPC.Text)) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa loại phụ cấp này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = int.Parse(txtIdPC.Text);
                DataRow row = ds.Tables["tblLOAIPHUCAP"].Rows.Find(id);
                if (row != null)
                {
                    row.Delete();
                    MessageBox.Show("Đã xóa trong bộ nhớ. Nhấn LƯU để cập nhật.");
                    LamMoiControls();
                }
            }
        }

        private void btnLuuPC_Click(object sender, EventArgs e)
        {
            try
            {
                int result = daLoaiPhuCap.Update(ds, "tblLOAIPHUCAP");
                MessageBox.Show($"Lưu thành công {result} loại phụ cấp.");

                ds.Tables["tblLOAIPHUCAP"].Clear();
                daLoaiPhuCap.Fill(ds, "tblLOAIPHUCAP");

                // *** QUAN TRỌNG: BẮN TÍN HIỆU VỀ FORM CHA ***
                DataSaved?.Invoke(this, EventArgs.Empty);

                LamMoiControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                ds.Tables["tblLOAIPHUCAP"].RejectChanges();
            }
        }

        private void btnHuyPC_Click(object sender, EventArgs e)
        {
            ds.Tables["tblLOAIPHUCAP"].RejectChanges();
            LamMoiControls();
        }

        private void LamMoiControls()
        {
            txtIdPC.Text = "";
            txtTenPCMoi.Text = "";
            txtSoTien.Text = "";
            dgvPhuCapMoi.ClearSelection();
        }

        // Sự kiện Click vào DataGridView
        // Bạn cần nối dây sự kiện này trong Designer: Chọn DGV -> Events -> Click
        private void dgvPhuCapMoi_Click(object sender, EventArgs e)
        {
            if (dgvPhuCapMoi.SelectedRows.Count > 0)
            {
                DataRowView drv = dgvPhuCapMoi.SelectedRows[0].DataBoundItem as DataRowView;
                if (drv != null)
                {
                    txtIdPC.Text = drv["IDPC"].ToString();
                    txtTenPCMoi.Text = drv["TENPC"].ToString();
                    txtSoTien.Text = drv["SOTIEN"].ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Nút quay lại (nếu cần giống nút thoát)
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}