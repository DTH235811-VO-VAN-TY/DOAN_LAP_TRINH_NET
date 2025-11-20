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
    public partial class Uc_PhuCap : UserControl
    {
        public Uc_PhuCap()
        {
            InitializeComponent();
        }
        // Khai báo cấp độ Class
        SqlConnection conn;
        DataSet ds = new DataSet();
        SqlDataAdapter daPhuCap;
        SqlDataAdapter daNhanVien;
        private void btnThem_Click(object sender, EventArgs e)
        {
          
        }

        private void Uc_PhuCap_Load(object sender, EventArgs e)
        {
           
        }
        private void LoadSearchComboBox()
        {
        }

        private void dvgPhuCap_Click(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
           
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
            
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
           
        }

        private void cboMaNVPC_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void LoadDataPhuCap()
        {
           
        }
    }
}
