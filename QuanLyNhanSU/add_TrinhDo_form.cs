using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class add_TrinhDo_form : Form
    {
        public add_TrinhDo_form()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daTrinhDo;
        public event EventHandler DataSaved;


        private void button2_Click(object sender, EventArgs e)
        {
            if (ds.Tables["TrinhDo"].GetChanges() != null)
            {
                if (MessageBox.Show("Bạn có thay đổi chưa lưu. Bạn có muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return; // Không đóng
                }
            }
            this.Close();
        }

        private void add_TrinhDo_form_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";
            try
            {
                conn.Open();

                // === SỬA LỖI 1 & 2: Sửa lại tên bảng và cột ===
                string sqlSelect = "SELECT IDTD, TENTD FROM TB_TRINHDO"; // Dùng TB_TRINHDO
                daTrinhDo = new SqlDataAdapter(sqlSelect, conn);
                daTrinhDo.Fill(ds, "TrinhDo");

                // === SỬA LỖI 2 (DGV TRẮNG): Tắt Tự Động Tạo Cột ===
                dgvTrinhDo.AutoGenerateColumns = false;

                // Gán DGV
                dgvTrinhDo.DataSource = ds.Tables["TrinhDo"];

                // === SỬA LỖI 1 (CRASH KHI THÊM): Dạy DataSet cột IDTD là AutoIncrement ===
                // (Phải làm sau khi .Fill())
                DataColumn pk = ds.Tables["TrinhDo"].Columns["IDTD"];
                pk.AutoIncrement = true;            // Bật tự tăng (trong DataSet)
                pk.AutoIncrementSeed = -1;        // Bắt đầu đếm từ -1 (giảm dần)
                pk.AutoIncrementStep = -1;        // Mỗi lần thêm 1 hàng, giảm 1 (tránh trùng ID thật)
                                                  // (Hành động này tự động set pk.AllowDBNull = true, sửa lỗi crash)

                // === SỬA LỖI 2 (DGV TRẮNG): Gán DataPropertyName ===
                // (Giả sử 2 cột bạn tạo trong [Design] có tên là "ID" và "TrinhDo")
                // (Nếu tên khác, hãy sửa lại tên cột, ví dụ dgvTrinhDo.Columns["Column1"])
                dgvTrinhDo.Columns["ID"].DataPropertyName = "IDTD";
                dgvTrinhDo.Columns["TrinhDo"].DataPropertyName = "TENTD";

                // (Bỏ 2 dòng ForeColor đi, chúng không phải là lỗi)
                // dgvTrinhDo.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black; 
                // dgvTrinhDo.DefaultCellStyle.ForeColor = Color.Black; 

                dgvTrinhDo.Columns["TrinhDo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Thêm Khóa Chính cho DataSet (để .Find() hoạt động)
                ds.Tables["TrinhDo"].PrimaryKey = new DataColumn[] { ds.Tables["TrinhDo"].Columns["IDTD"] };

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu. " + ex.Message);
            }
            ///---Command them pb----\
            string themTD = @"INSERT INTO TB_TRINHDO(TENTD) VALUES(@TENTD)";
            SqlCommand cmdThemTD = new SqlCommand(themTD, conn);
            
            cmdThemTD.Parameters.Add("@TENTD", SqlDbType.NVarChar, 100, "TENTD"); // (Cho tên dài hơn)
            daTrinhDo.InsertCommand = cmdThemTD;

            //--- Command Sửa ---
            // === SỬA LỖI 1 & 3: Sửa tên bảng và kiểu dữ liệu ===
            string suaTD = @"UPDATE TB_TRINHDO SET TENTD=@TENTD WHERE IDTD=@IDTD";
            SqlCommand cmdSuaTD = new SqlCommand(suaTD, conn);
            cmdSuaTD.Parameters.Add("@TENTD", SqlDbType.NVarChar, 100, "TENTD");
            cmdSuaTD.Parameters.Add("@IDTD", SqlDbType.Int, 4, "IDTD"); // << Sửa: Phải là INT
            daTrinhDo.UpdateCommand = cmdSuaTD;

            //--- Command Xóa ---
            // === SỬA LỖI 1 & 3: Sửa tên bảng và kiểu dữ liệu ===
            string xoaTD = @"DELETE FROM TB_TRINHDO WHERE IDTD=@IDTD";
            SqlCommand cmdXoaTD = new SqlCommand(xoaTD, conn);
            cmdXoaTD.Parameters.Add("@IDTD", SqlDbType.Int, 4, "IDTD"); // << Sửa: Phải là INT
            daTrinhDo.DeleteCommand = cmdXoaTD;
        }

        private void btnThemTD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenTD.Text))
            {
                MessageBox.Show("Tên trình độ không được rỗng!");
                return;
            }

            // 1. Thêm vào DataSet (bộ nhớ RAM)
            DataRow row = ds.Tables["TrinhDo"].NewRow();
            row["TENTD"] = txtTenTD.Text;
            ds.Tables["TrinhDo"].Rows.Add(row);
            DataSaved?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Đã thêm vào bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
        }

        private void btnSuuTD_Click(object sender, EventArgs e)
        {
            if (txtIdTD.Enabled == true || string.IsNullOrEmpty(txtIdTD.Text))
            {
                MessageBox.Show("Bạn chưa chọn trình độ để sửa.");
                return;
            }

            // 1. Tìm dòng trong DataSet bằng Khóa Chính
            DataRow row = ds.Tables["TrinhDo"].Rows.Find(int.Parse(txtIdTD.Text));

            if (row != null)
            {
                // 2. Sửa trong RAM
                row.BeginEdit();
                row["TENTD"] = txtTenTD.Text;
                row.EndEdit();
                DataSaved?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Đã sửa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
            }
        }

        private void btnXoaTD_Click(object sender, EventArgs e)
        {
            if (txtIdTD.Enabled == true || string.IsNullOrEmpty(txtIdTD.Text))
            {
                MessageBox.Show("Bạn chưa chọn trình độ để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa Trình độ ID: " + txtIdTD.Text + "?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // 1. Tìm dòng trong DataSet
                DataRow row = ds.Tables["TrinhDo"].Rows.Find(int.Parse(txtIdTD.Text));
                if (row != null)
                {
                    // 2. Xóa trong RAM
                    row.Delete();
                    DataSaved?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Đã xóa trong bộ nhớ đệm. Nhấn 'Lưu' để cập nhật CSDL.");
                }
            }
        }

        private void btnHuyTD_Click(object sender, EventArgs e)
        {
            txtIdTD.Text = "";
            txtTenTD.Text = "";
           // txtIdTD.Enabled = true; // Mở khóa để nhập ID mới
        }

        private void dgvTrinhDo_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnLuuTD_Click(object sender, EventArgs e)
        {
            try
            {
                // 3. Đẩy tất cả thay đổi (Thêm, Sửa, Xóa) từ RAM xuống CSDL
                daTrinhDo.Update(ds, "TrinhDo");
                DataSaved?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Lưu thành công!");

                // === BẮN TÍN HIỆU CHO UC_NhanVien BIẾT ===
                // OnDataChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                ds.Tables["TrinhDo"].RejectChanges(); // Hủy thay đổi nếu lỗi
            }
        }

        private void dgvTrinhDo_Click(object sender, EventArgs e)
        {
            if(dgvTrinhDo.SelectedRows.Count >0)
            {
                DataGridViewRow dr = dgvTrinhDo.SelectedRows[0];
                txtIdTD.Text = dr.Cells["id"].Value.ToString();
                txtTenTD.Text = dr.Cells["TrinhDo"].Value.ToString();
                txtIdTD.Enabled = false;
            }
        }
    }
}
