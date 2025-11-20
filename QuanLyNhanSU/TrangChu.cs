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
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
            uC_ChucNangNS2.AddNhanVienClicked += Uc_ChucNangNS2_AddNhanVienClicked;
            uC_ChucNangNS2.AddBaoHiemClicked += Uc_BaoHiemNV1_AddBaoHiemClicked;
            uC_ChucNangNS2.AddHopDongClicked += Uc_HopDong_AddHopDongClicked;
            uC_ChucNangNS2.AddKhenThuongLyLuatClicked += Uc_KhenThuong_KyLuat_AddKhenThuongLyLuatClicked;
            uC_ChucNangNS2.AddNghiPhepClicked += Uc_NghiPhep_AddNghiPhepClicked;
            this.btnNhanSu.Click += new System.EventHandler(this.btnNhanSu_Click);

        }

        private void UC_ChucNangNS2_AddNghiPhepClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void uC_NhanVien1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult traloi = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                Application.Exit();
            }
        }
        private void Uc_ChucNangNS2_AddNhanVienClicked(object sender, EventArgs e)
        {
            // A-ha! "Con 1" (ChucNang) vừa bắn tín hiệu.
            // Bây giờ "Cha" (Dashboard) sẽ ra lệnh cho "Con 2" (NhanVien) hiện ra.
            uC_NhanVien2.BringToFront();
        }
        private void Uc_BaoHiemNV1_AddBaoHiemClicked(object sender, EventArgs e)
        {
            uC_BaoHiemNV1.BringToFront();
        }
        private void Uc_HopDong_AddHopDongClicked(object sender, EventArgs e)
        {
            uc_HopDong1.BringToFront();
        }
        public void Uc_KhenThuong_KyLuat_AddKhenThuongLyLuatClicked(object sender, EventArgs e)
        {
            uC_KhenThuong_KyLuat1.BringToFront();
        }
        private void btnNhanSu_Click(object sender, EventArgs e)
        {
            uC_ChucNangNS2.BringToFront();
        }

        public void Uc_NghiPhep_AddNghiPhepClicked(object sender, EventArgs e)
        {
             //uC_NghiPhepNV.BringToFront();
             uC_NghiPhepNV1.BringToFront();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}