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
    public partial class add_BoPhan_form : Form
    {
        public add_BoPhan_form()
        {
            InitializeComponent();
        }
        public event EventHandler DataSaved;

        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daPhongBan; // Để tải cboPhongBan
        SqlDataAdapter daBoPhan;

        private void add_BoPhan_form_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

            try
            {
                conn.Open();

                // 1. Tải ComboBox Phòng Ban (để chọn)
                string sqlPhongBan = @"SELECT IDPB, TENPB FROM TB_PHONGBAN";
                daPhongBan = new SqlDataAdapter(sqlPhongBan, conn);
                daPhongBan.Fill(ds, "PhongBan");

                cboPhongBan.DataSource = ds.Tables["PhongBan"];
                cboPhongBan.DisplayMember = "TENPB";
                cboPhongBan.ValueMember = "IDPB";

                // 2. Tải DataGridView Bộ Phận (hiển thị những gì đã có)
                string sqlBoPhan = @"SELECT b.IDBP, b.TENBP, b.IDPB, p.TENPB 
                                     FROM TB_BOPHAN b
                                     LEFT JOIN TB_PHONGBAN p ON b.IDPB = p.IDPB";
                daBoPhan = new SqlDataAdapter(sqlBoPhan, conn);
                daBoPhan.Fill(ds, "BoPhan");

                dgvBoPhan.AutoGenerateColumns = false; // Tắt tự tạo cột
                dgvBoPhan.DataSource = ds.Tables["BoPhan"];

                // Gán DataPropertyName
                dgvBoPhan.Columns["Id"].DataPropertyName = "IDBP";
                dgvBoPhan.Columns["BoPhan"].DataPropertyName = "TENBP";
                dgvBoPhan.Columns["TenPhongBan"].DataPropertyName = "TENPB";

                // 3. Cấu hình AutoIncrement cho IDBP (giống form Phòng Ban)
                DataColumn pk = ds.Tables["BoPhan"].Columns["IDBP"];
                pk.AutoIncrement = true;
                pk.AutoIncrementSeed = -1;
                pk.AutoIncrementStep = -1;

                // 4. Thêm Khóa Chính
                ds.Tables["BoPhan"].PrimaryKey = new DataColumn[] { ds.Tables["BoPhan"].Columns["IDBP"] };

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải form: " + ex.Message);
            }

            // --- COMMANDS ---

            // 5. InsertCommand (Thêm)
            string sThem = @"INSERT INTO TB_BOPHAN(TENBP, IDPB) VALUES(@TENBP, @IDPB)";
            SqlCommand cmdThem = new SqlCommand(sThem, conn);
            cmdThem.Parameters.Add("@TENBP", SqlDbType.NVarChar, 100, "TENBP");
            cmdThem.Parameters.Add("@IDPB", SqlDbType.Int, 4, "IDPB");
            daBoPhan.InsertCommand = cmdThem;

            // 6. UpdateCommand (Sửa)
            string sSua = @"UPDATE TB_BOPHAN SET TENBP=@TENBP, IDPB=@IDPB WHERE IDBP=@IDBP";
            SqlCommand cmdSua = new SqlCommand(sSua, conn);
            cmdSua.Parameters.Add("@TENBP", SqlDbType.NVarChar, 100, "TENBP");
            cmdSua.Parameters.Add("@IDPB", SqlDbType.Int, 4, "IDPB");
            cmdSua.Parameters.Add("@IDBP", SqlDbType.Int, 4, "IDBP");
            daBoPhan.UpdateCommand = cmdSua;

            // 7. DeleteCommand (Xóa)
            string sXoa = @"DELETE FROM TB_BOPHAN WHERE IDBP=@IDBP";
            SqlCommand cmdXoa = new SqlCommand(sXoa, conn);
            cmdXoa.Parameters.Add("@IDBP", SqlDbType.Int, 4, "IDBP");
            daBoPhan.DeleteCommand = cmdXoa;
        }

        private void btnThemBP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenBP.Text))
            {
                MessageBox.Show("Tên bộ phận không được rỗng!");
                return;
            }
            if (cboPhongBan.SelectedValue == null)
            {
                MessageBox.Show("Bạn phải chọn một phòng ban!");
                return;
            }

            DataRow row = ds.Tables["BoPhan"].NewRow();
            row["TENBP"] = txtTenBP.Text;
            row["IDPB"] = cboPhongBan.SelectedValue;
            row["TENPB"] = cboPhongBan.Text; // Để DGV hiển thị
            ds.Tables["BoPhan"].Rows.Add(row);

            // BỎ DÒNG NÀY ĐI, CHỈ NÊN BẮN TÍN HIỆU KHI LƯU
            // DataSaved?.Invoke(this, EventArgs.Empty); 

            MessageBox.Show("Đã thêm vào bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            ResetForm();
        }

        private void btnLuuBP_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = daBoPhan.Update(ds, "BoPhan");
                MessageBox.Show($"Đã lưu {rowsAffected} thay đổi vào CSDL.", "Thành công");

                // Tải lại dữ liệu "sạch"
                ds.Tables["BoPhan"].Clear();
                daBoPhan.Fill(ds, "BoPhan");

                // BẮN TÍN HIỆU CHO UC_NHANVIEN (Chỉ bắn khi LƯU thành công)
                DataSaved?.Invoke(this, EventArgs.Empty);

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                ds.Tables["BoPhan"].RejectChanges();
            }
        }
        private void ResetForm()
        {
            txtIdBP.Text = "";
            txtTenBP.Text = "";
            txtIdBP.Enabled = true; // Mở khóa ID để chuẩn bị Thêm
            cboPhongBan.SelectedIndex = -1;
            dgvBoPhan.ClearSelection();
        }

        private void btnSuuBP_Click(object sender, EventArgs e)
        {
            // Logic kiểm tra của bạn đã đúng
            if (txtIdBP.Enabled == true || string.IsNullOrEmpty(txtIdBP.Text))
            {
                MessageBox.Show("Vui long chọn 1 bộ phận để sửa ");
                return;
            }

            try
            {
                // --- SỬA LỖI: Phải Convert ID sang INT khi tìm ---
                int idbp = Convert.ToInt32(txtIdBP.Text);
                DataRow rowToUpdate = ds.Tables["BoPhan"].Rows.Find(idbp);

                if (rowToUpdate != null)
                {
                    rowToUpdate.BeginEdit();
                    rowToUpdate["IDPB"] = cboPhongBan.SelectedValue;
                    rowToUpdate["TENBP"] = txtTenBP.Text;
                    rowToUpdate["TENPB"] = cboPhongBan.Text;
                    rowToUpdate.EndEdit();

                    MessageBox.Show("Đã sửa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bộ phận để sửa.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi");
            }
        }

        private void btnXoaBP_Click(object sender, EventArgs e)
        {
            // Bổ sung code Xóa
            if (txtIdBP.Enabled == true || string.IsNullOrEmpty(txtIdBP.Text))
            {
                MessageBox.Show("Bạn chưa chọn bộ phận để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa bộ phận: " + txtTenBP.Text + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            try
            {
                int idbp = Convert.ToInt32(txtIdBP.Text);
                DataRow rowToDelete = ds.Tables["BoPhan"].Rows.Find(idbp);

                if (rowToDelete != null)
                {
                    rowToDelete.Delete();
                    MessageBox.Show("Đã xóa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
                    ResetForm(); // Xóa xong thì làm sạch form
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi");
            }
        }

        private void btnHuyBP_Click(object sender, EventArgs e)
        {
            // Bổ sung code Hủy
            if (ds.Tables["BoPhan"].GetChanges() != null)
            {
                ds.Tables["BoPhan"].RejectChanges();
                MessageBox.Show("Đã hủy các thay đổi chưa lưu.");
            }
            ResetForm();
        }

        // --- HÀM BỊ LỖI CỦA BẠN ĐÂY ---
        private void dgvBoPhan_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBoPhan.SelectedRows.Count > 0)
                {
                    DataGridViewRow dr = dgvBoPhan.SelectedRows[0];

                    // --- SỬA LỖI: Lấy DataRowView để truy cập an toàn ---
                    DataRowView drv = dr.DataBoundItem as DataRowView;
                    if (drv == null) return; // Thoát nếu có lỗi

                    // Giờ chúng ta truy cập bằng tên cột DỮ LIỆU (từ DataSet)
                    // thay vì tên cột DGV

                    txtIdBP.Text = drv["IDBP"].ToString();
                    txtTenBP.Text = drv["TENBP"].ToString();
                    cboPhongBan.SelectedValue = drv["IDPB"] != DBNull.Value ? drv["IDPB"] : -1;

                    // --- SỬA LOGIC: Phải KHÓA txtIdBP lại ---
                    txtIdBP.Enabled = false; // <-- Sửa từ true thành false
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi khi chon hang " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Thêm kiểm tra thay đổi chưa lưu
            if (ds.Tables["BoPhan"].GetChanges() != null)
            {
                if (MessageBox.Show("Bạn có thay đổi chưa lưu, bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return; // Hủy việc đóng form
                }
            }
            this.Close();
        }
    }
}