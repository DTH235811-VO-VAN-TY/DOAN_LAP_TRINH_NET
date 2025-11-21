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
            uC_ChucNangNS2.AddPhuCapClicked += Uc_PhuCap_AddPhuCapClicked;
            uC_NhanVien2.DataUpdated += UC_NhanVien_DataUpdated;
            this.btnNhanSu.Click += new System.EventHandler(this.btnNhanSu_Click);

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
        private void Uc_PhuCap_AddPhuCapClicked(object sender, EventArgs e)
        {
            uc_PhuCap1.BringToFront();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TrangChu_Load(object sender, EventArgs e)
        {
            uC_ChucNangNS2.Visible = true;
            uC_ChucNangNS2.BringToFront();
        }
        private void UC_NhanVien_DataUpdated(object sender, EventArgs e)
        {
            if(uc_HopDong1 != null)
            {
                uc_HopDong1.ReloadNhanVien();
            }
            if(uC_KhenThuong_KyLuat1 != null)
            {
                uC_KhenThuong_KyLuat1.ReloadNhanVien();
            }
            if (uC_BaoHiemNV1 != null)
            {
                uC_BaoHiemNV1.ReloadNhanVien();
            }
            if(uc_PhuCap1 != null)
            {
                uc_PhuCap1.ReloadNhanVienPhuCap();
            }
            MessageBox.Show("Dữ liệu nhân viên đã được cập nhật. Các phần liên quan đã được làm mới.");
        }

        private void btnThemUC_Luong_Click(object sender, EventArgs e)
        {
            uC_BangLuong1.BringToFront();
        }
    }
}
