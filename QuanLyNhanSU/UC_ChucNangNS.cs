using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class UC_ChucNangNS : UserControl
    {
        public event EventHandler AddNhanVienClicked;
        public event EventHandler AddHopDongClicked;
        public event EventHandler AddThoiViecClicked;
        public event EventHandler AddBaoHiemClicked;
        public event EventHandler AddKhenThuongLyLuatClicked;
        public event EventHandler AddPhuCapClicked;
        public UC_ChucNangNS()
        {
            InitializeComponent();
        }

        private void btnAdd_NhanVien_Click(object sender, EventArgs e)
        {
            AddNhanVienClicked.Invoke(this, EventArgs.Empty);
        }

        private void btnAdd_HopDong_Click(object sender, EventArgs e)
        {
            AddHopDongClicked.Invoke(this, EventArgs.Empty);
        }

        private void btnAdd_NghiPhep_Click(object sender, EventArgs e)
        {
            AddThoiViecClicked.Invoke(this, EventArgs.Empty);
        }

        private void btnAdd_BaoHiem_Click(object sender, EventArgs e)
        {
            AddBaoHiemClicked.Invoke(this, EventArgs.Empty);
        }

        private void btnAdd_KhenThuong_KL_Click(object sender, EventArgs e)
        {
            AddKhenThuongLyLuatClicked.Invoke(this, EventArgs.Empty);
        }

        private void btnAdd_PhuCap_Click(object sender, EventArgs e)
        {
            AddPhuCapClicked.Invoke(this, EventArgs.Empty);
        }
    }
}
