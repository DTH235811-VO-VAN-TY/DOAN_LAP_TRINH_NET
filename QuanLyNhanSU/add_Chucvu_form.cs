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
    public partial class add_Chucvu_form : Form
    {
        public add_Chucvu_form()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daChucVu;
        public event EventHandler DataSaved;

        private void button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có thay đổi chưa lưu không
            if (ds.Tables["TbChucVu"] != null && ds.Tables["TbChucVu"].GetChanges() != null)
            {
                if (MessageBox.Show("Bạn có thay đổi chưa lưu. Bạn có muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return; // Không đóng
                }
            }
            this.Close();
        }

        private void add_Chucvu_form_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
            try
            {
                conn.Open();

                string sqlSelect = "SELECT IDCV, TENCV FROM TB_CHUCVU";
                daChucVu = new SqlDataAdapter(sqlSelect, conn);
                daChucVu.Fill(ds, "TbChucVu");

                dgvChucVu.AutoGenerateColumns = false;
                dgvChucVu.DataSource = ds.Tables["TbChucVu"];

                // Cấu hình cột AutoIncrement cho IDCV
                DataColumn pk = ds.Tables["TbChucVu"].Columns["IDCV"];
                pk.AutoIncrement = true;
                pk.AutoIncrementSeed = -1;
                pk.AutoIncrementStep = -1;

                // Gán DataPropertyName cho các cột DataGridView
                // Lưu ý: Tên cột "Id" và "ChucVu" phải khớp với Designer của bạn
                dgvChucVu.Columns["Id"].DataPropertyName = "IDCV";
                dgvChucVu.Columns["ChucVu"].DataPropertyName = "TENCV";

                // Cấu hình AutoSizeMode (nếu cột tên là ChucVu)
                dgvChucVu.Columns["ChucVu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Đặt khóa chính cho DataTable
                ds.Tables["TbChucVu"].PrimaryKey = new DataColumn[] { ds.Tables["TbChucVu"].Columns["IDCV"] };

                // --- THIẾT LẬP CÁC COMMAND CHO DACHUCVU ---

                // INSERT Command
                string sqlInsert = @"INSERT INTO TB_CHUCVU(TENCV) VALUES(@TENCV)";
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.Add("@TENCV", SqlDbType.NVarChar, 100, "TENCV");
                daChucVu.InsertCommand = cmdInsert;

                // UPDATE Command
                string sqlUpdate = @"UPDATE TB_CHUCVU SET TENCV=@TENCV WHERE IDCV=@IDCV";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.Add("@TENCV", SqlDbType.NVarChar, 100, "TENCV");
                cmdUpdate.Parameters.Add("@IDCV", SqlDbType.Int, 4, "IDCV");
                daChucVu.UpdateCommand = cmdUpdate;

                // DELETE Command
                string sqlDelete = @"DELETE FROM TB_CHUCVU WHERE IDCV=@IDCV";
                SqlCommand cmdDelete = new SqlCommand(sqlDelete, conn);
                cmdDelete.Parameters.Add("@IDCV", SqlDbType.Int, 4, "IDCV");
                daChucVu.DeleteCommand = cmdDelete;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu. " + ex.Message);
            }
        }

        private void btnThemCV_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenCV.Text))
            {
                MessageBox.Show("Tên chức vụ không được rỗng!");
                return;
            }

            // Thêm vào DataSet (bộ nhớ RAM)
            DataRow row = ds.Tables["TbChucVu"].NewRow();
            row["TENCV"] = txtTenCV.Text;
            ds.Tables["TbChucVu"].Rows.Add(row);

            // Thông báo và reset form
            MessageBox.Show("Đã thêm vào bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            ResetForm();
        }

       

        private void btnXoaCV_Click(object sender, EventArgs e)
        {
            if (txtIdCV.Enabled == true || string.IsNullOrEmpty(txtIdCV.Text))
            {
                MessageBox.Show("Bạn chưa chọn chức vụ để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa Chức vụ ID: " + txtIdCV.Text + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int idToDelete;
                if (int.TryParse(txtIdCV.Text, out idToDelete))
                {
                    // Tìm dòng trong DataSet
                    DataRow row = ds.Tables["TbChucVu"].Rows.Find(idToDelete);
                    if (row != null)
                    {
                        // Xóa trong RAM
                        row.Delete();
                        MessageBox.Show("Đã xóa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
                        ResetForm();
                    }
                }
            }
        }

        private void btnLuuCV_Click(object sender, EventArgs e)
        {
            try
            {
                // Đẩy tất cả thay đổi (Thêm, Sửa, Xóa) từ RAM xuống CSDL
                daChucVu.Update(ds, "TbChucVu");

                // Bắn sự kiện để form cha cập nhật lại ComboBox (nếu cần)
                DataSaved?.Invoke(this, EventArgs.Empty);

                MessageBox.Show("Lưu thành công vào cơ sở dữ liệu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                ds.Tables["TbChucVu"].RejectChanges(); // Hủy thay đổi nếu lỗi để đồng bộ lại
            }
        }

        private void btnHuyCV_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void dgvChucVu_Click(object sender, EventArgs e)
        {
            if (dgvChucVu.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgvChucVu.SelectedRows[0];

                // Kiểm tra giá trị null trước khi gán
                if (dr.Cells["Id"].Value != null)
                    txtIdCV.Text = dr.Cells["Id"].Value.ToString();

                if (dr.Cells["ChucVu"].Value != null)
                    txtTenCV.Text = dr.Cells["ChucVu"].Value.ToString();

                // Khóa ô ID lại khi đang chọn để sửa/xóa
                txtIdCV.Enabled = false;
            }
        }

        // Hàm phụ trợ để reset các control nhập liệu
        private void ResetForm()
        {
            txtIdCV.Text = "";
            txtTenCV.Text = "";
            txtIdCV.Enabled = true; // Mở khóa để nhập mới (mặc dù ID tự tăng nhưng để trạng thái ban đầu)
            dgvChucVu.ClearSelection(); // Bỏ chọn trên grid
        }

        private void btnSuuCV_Click(object sender, EventArgs e)
        {
            if (txtIdCV.Enabled == true || string.IsNullOrEmpty(txtIdCV.Text))
            {
                MessageBox.Show("Bạn chưa chọn trình độ để sửa.");
                return;
            }

            // 1. Tìm dòng trong DataSet bằng Khóa Chính
            DataRow row = ds.Tables["TbChucVu"].Rows.Find(int.Parse(txtIdCV.Text));

            if (row != null)
            {
                // 2. Sửa trong RAM
                row.BeginEdit();
                row["TENCV"] = txtTenCV.Text;
                row.EndEdit();
                DataSaved?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Đã sửa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            }
        }
    }
}