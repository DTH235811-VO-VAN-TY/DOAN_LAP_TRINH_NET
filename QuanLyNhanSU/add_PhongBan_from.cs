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
    public partial class add_PhongBan_from : Form
    {
        public add_PhongBan_from()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daPhongBan;
        public event EventHandler DataSaved;

        private void add_PhongBan_from_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
            try
            {
                conn.Open();

                // === SỬA LỖI 1 & 2: Sửa lại tên bảng và cột ===
                string sqlSelect = "SELECT IDPB, TENPB FROM TB_PHONGBAN"; // Dùng TB_TRINHDO
                daPhongBan = new SqlDataAdapter(sqlSelect, conn);
                daPhongBan.Fill(ds, "PhongBan");

                // === SỬA LỖI 2 (DGV TRẮNG): Tắt Tự Động Tạo Cột ===
                dgvPhongBan.AutoGenerateColumns = false;

                // Gán DGV
                dgvPhongBan.DataSource = ds.Tables["PhongBan"];

                // === SỬA LỖI 1 (CRASH KHI THÊM): Dạy DataSet cột IDTD là AutoIncrement ===
                // (Phải làm sau khi .Fill())
                DataColumn pk = ds.Tables["PhongBan"].Columns["IDPB"];
                pk.AutoIncrement = true;            // Bật tự tăng (trong DataSet)
                pk.AutoIncrementSeed = -1;        // Bắt đầu đếm từ -1 (giảm dần)
                pk.AutoIncrementStep = -1;        // Mỗi lần thêm 1 hàng, giảm 1 (tránh trùng ID thật)
                                                  // (Hành động này tự động set pk.AllowDBNull = true, sửa lỗi crash)

                // === SỬA LỖI 2 (DGV TRẮNG): Gán DataPropertyName ===
                // (Giả sử 2 cột bạn tạo trong [Design] có tên là "ID" và "TrinhDo")
                // (Nếu tên khác, hãy sửa lại tên cột, ví dụ dgvTrinhDo.Columns["Column1"])
                dgvPhongBan.Columns["id"].DataPropertyName = "IDPB";
                dgvPhongBan.Columns["PhongBan"].DataPropertyName = "TENPB";

                // (Bỏ 2 dòng ForeColor đi, chúng không phải là lỗi)
                // dgvTrinhDo.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black; 
                // dgvTrinhDo.DefaultCellStyle.ForeColor = Color.Black; 

                dgvPhongBan.Columns["PhongBan"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Thêm Khóa Chính cho DataSet (để .Find() hoạt động)
                ds.Tables["PhongBan"].PrimaryKey = new DataColumn[] { ds.Tables["PhongBan"].Columns["IDPB"] };

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu. " + ex.Message);
            }
            ///---Command them pb----\
            string themTD = @"INSERT INTO TB_PHONGBAN(TENPB) VALUES(@TENPB)";
            SqlCommand cmdThemTD = new SqlCommand(themTD, conn);

            cmdThemTD.Parameters.Add("@TENPB", SqlDbType.NVarChar, 100, "TENPB"); // (Cho tên dài hơn)
            daPhongBan.InsertCommand = cmdThemTD;

            //--- Command Sửa ---
            // === SỬA LỖI 1 & 3: Sửa tên bảng và kiểu dữ liệu ===
            string suaTD = @"UPDATE TB_PHONGBAN SET TENPB=@TENPB WHERE IDPB=@IDPB";
            SqlCommand cmdSuaTD = new SqlCommand(suaTD, conn);
            cmdSuaTD.Parameters.Add("@TENPB", SqlDbType.NVarChar, 100, "TENPB");
            cmdSuaTD.Parameters.Add("@IDPB", SqlDbType.Int, 4, "IDPB"); // << Sửa: Phải là INT
            daPhongBan.UpdateCommand = cmdSuaTD;

            //--- Command Xóa ---
            // === SỬA LỖI 1 & 3: Sửa tên bảng và kiểu dữ liệu ===
            string xoaTD = @"DELETE FROM TB_PHONGBAN WHERE IDPB=@IDPB";
            SqlCommand cmdXoaTD = new SqlCommand(xoaTD, conn);
            cmdXoaTD.Parameters.Add("@IDPB", SqlDbType.Int, 4, "IDPB"); // << Sửa: Phải là INT
            daPhongBan.DeleteCommand = cmdXoaTD;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuuPB_Click(object sender, EventArgs e)
        {
            try
            {
                // Đẩy tất cả thay đổi (Thêm, Sửa, Xóa) trong DataSet xuống CSDL
                int rowsAffected = daPhongBan.Update(ds, "PhongBan");
                MessageBox.Show($"Đã lưu {rowsAffected} thay đổi vào CSDL thành công.", "Thành công");

                // Tải lại dữ liệu "sạch" từ CSDL
                ds.Tables["PhongBan"].Clear();
                daPhongBan.Fill(ds, "PhongBan");

                // Làm sạch text box
                DataSaved?.Invoke(this, EventArgs.Empty);
                //ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi");
                // Nếu lỗi, khôi phục lại DataSet
                ds.Tables["PhongBan"].RejectChanges();
            }
        }

        private void btnThemPB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenPB.Text))
            {
                MessageBox.Show("Tên trình độ không được rỗng!");
                return;
            }

            // 1. Thêm vào DataSet (bộ nhớ RAM)
            DataRow row = ds.Tables["PhongBan"].NewRow();
            row["TENPB"] = txtTenPB.Text;
            ds.Tables["PhongBan"].Rows.Add(row);

            MessageBox.Show("Đã thêm vào bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            DataSaved?.Invoke(this, EventArgs.Empty);
        }

        private void btnSuuPB_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn hàng nào chưa (bằng cách xem txtIdPB có bị khóa không)
            if (txtIdPB.Enabled == true || string.IsNullOrEmpty(txtIdPB.Text))
            {
                MessageBox.Show("Bạn chưa chọn phòng ban để sửa. Hãy nhấn vào một dòng trong lưới.", "Lỗi");
                return;
            }

            try
            {
                // 1. Tìm dòng (Row) trong DataSet bằng Khóa Chính
                DataRow rowToUpdate = ds.Tables["PhongBan"].Rows.Find(Convert.ToInt32(txtIdPB.Text));

                if (rowToUpdate != null)
                {
                    // 2. Cập nhật dữ liệu cho dòng đó (trong RAM)
                    rowToUpdate["TENPB"] = txtTenPB.Text;
                    DataSaved?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Đã sửa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm phòng ban để sửa: " + ex.Message, "Lỗi");
            }
        }

        private void btnXoaPB_Click(object sender, EventArgs e)
        {
            if (txtIdPB.Enabled == true || string.IsNullOrEmpty(txtIdPB.Text))
            {
                MessageBox.Show("Bạn chưa chọn phòng ban để xóa. Hãy nhấn vào một dòng trong lưới.", "Lỗi");
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn xóa phòng ban '{txtTenPB.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            try
            {
                // 1. Tìm dòng trong DataSet
                DataRow rowToDelete = ds.Tables["PhongBan"].Rows.Find(Convert.ToInt32(txtIdPB.Text));

                if (rowToDelete != null)
                {
                    // 2. Xóa dòng (trong RAM)
                    rowToDelete.Delete();
                    DataSaved?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Đã xóa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");

                    // Xóa xong thì làm sạch form
                    //  ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm phòng ban để xóa: " + ex.Message, "Lỗi");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ds.Tables["PhongBan"].RejectChanges();
            MessageBox.Show("Đã hủy các thay đổi chưa lưu.");
        }

        private void dgvPhongBan_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPhongBan.SelectedRows.Count > 0)
                {
                    DataGridViewRow dr = dgvPhongBan.SelectedRows[0];
                    txtIdPB.Text = dr.Cells["id"].Value.ToString();
                    txtTenPB.Text = dr.Cells["PhongBan"].Value.ToString();

                    // Khóa txtIdPB lại vì đây là Khóa Chính, không cho sửa
                    txtIdPB.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn hàng: " + ex.Message);
            }
        }
    }
}
