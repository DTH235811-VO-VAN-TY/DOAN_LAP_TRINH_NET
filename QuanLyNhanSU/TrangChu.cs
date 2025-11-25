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
        // Thêm vào bên trong class TrangChu
        public void PhanQuyen(int quyen)
        {
            // TRƯỜNG HỢP 1: ADMIN (Quyền = 1)
            if (quyen == 1)
            {
                this.Text = "Hệ thống Quản lý Nhân sự - QUẢN TRỊ VIÊN";

                // Mở khóa tất cả chức năng (đề phòng trường hợp đăng xuất rồi đăng nhập lại)
                if (uC_ChucNangNS2 != null) uC_ChucNangNS2.Enabled = true;
            }
            // TRƯỜNG HỢP 2: NHÂN VIÊN (Quyền = 2)
            else if (quyen == 2)
            {
                this.Text = "Hệ thống Quản lý Nhân sự - NHÂN VIÊN (Chỉ xem)";

                // Khóa toàn bộ thanh chức năng bên trái
                if (uC_ChucNangNS2 != null)
                {
                    uC_ChucNangNS2.Enabled = false;
                }

                MessageBox.Show("Chào bạn! Bạn đang đăng nhập với quyền Nhân Viên.\nChức năng chỉnh sửa hệ thống đã bị khóa.", "Phân quyền hệ thống");
            }
        }
        public TrangChu()
        {
            InitializeComponent();
            uC_ChucNangNS2.AddNhanVienClicked += Uc_ChucNangNS2_AddNhanVienClicked;
            uC_ChucNangNS2.AddBaoHiemClicked += Uc_BaoHiemNV1_AddBaoHiemClicked;
            uC_ChucNangNS2.AddHopDongClicked += Uc_HopDong_AddHopDongClicked;
            uC_ChucNangNS2.AddKhenThuongLyLuatClicked += Uc_KhenThuong_KyLuat_AddKhenThuongLyLuatClicked;
            uC_ChucNangNS2.AddThoiViecClicked += Uc_ThoiViec_AddThoiViecClicked;
            uC_ChucNangNS2.AddPhuCapClicked += Uc_PhuCap_AddPhuCapClicked;
          //  uC_ChamCong2.AddNghiPhepClicked += Uc_NghiPhep_AddNghiPhepClicked;
            uC_NhanVien2.DataUpdated += UC_NhanVien_DataUpdated;
            uC_ThoiViec1.DataUpdated += UC_ThoiViec_DataUpdated;
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
        private void Uc_ThoiViec_AddThoiViecClicked(object sender, EventArgs e)
        {
          // uC_NghiPhepNV1.BringToFront();
            uC_ThoiViec1.BringToFront();
        }
      /*  private void Uc_NghiPhep_AddNghiPhepClicked(object sender, EventArgs e)
        {
            uC_NghiPhepNV1.BringToFront();
        }*/


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

      /*  private void button6_Click(object sender, EventArgs e)
        {
           // Form_QuanLyTaiKhoan f = new Form_QuanLyTaiKhoan();
          //  f.ShowDialog();
        }*/
        private void TrangChu_Load(object sender, EventArgs e)
        {
          //  uC_ChucNangNS2.Visible = true;
           // uC_ChucNangNS2.BringToFront();
          // lblTaiKhoan.Text = ; // Hiển thị tên đăng nhập lên label
          uC_HomePage1.Visible = true;
            uC_HomePage1.BringToFront();
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
            if(uC_NghiPhepNV1 != null)
            {
                uC_NghiPhepNV1.ReloadData();
            }
           if(uC_ThoiViec1 != null)
            {
                uC_ThoiViec1.LoadComboBoxNhanVien();
            }
           if(uC_ChamCong2 != null)
            {
                uC_ChamCong2.LoadDataComboBox();
            }
            MessageBox.Show("Dữ liệu nhân viên đã được cập nhật. Các phần liên quan đã được làm mới.");
        }
        private void UC_ThoiViec_DataUpdated(object sender, EventArgs e)
        {
            if (uC_BangLuong1 != null)
            {
                if (uc_HopDong1 != null)
                {
                    uc_HopDong1.ReloadNhanVien();
                }
                if (uC_KhenThuong_KyLuat1 != null)
                {
                    uC_KhenThuong_KyLuat1.ReloadNhanVien();
                }
                if (uC_BaoHiemNV1 != null)
                {
                    uC_BaoHiemNV1.ReloadNhanVien();
                }
                if (uc_PhuCap1 != null)
                {
                    uc_PhuCap1.ReloadNhanVienPhuCap();
                }
                if(uC_NghiPhepNV1 !=null)
                {
                    uC_NghiPhepNV1.ReloadData();
                }
                if(uC_ChamCong2 != null)
                {
                    uC_ChamCong2.LoadDataComboBox();
                }
                MessageBox.Show("Dữ liệu nhân viên đã được cập nhật. Các phần liên quan đã được làm mới.");
            }
            if(uC_NhanVien2 != null)
            {
                uC_NhanVien2.ReloadData();
            }
        }
        /* private void UC_PhuCap_DataUpdated(Object sender, EventArgs e)
         {
             if( uC_BangLuong1 != null)
             {

             }
         }*/
        private void btnThemUC_Luong_Click(object sender, EventArgs e)
        {
            uC_BangLuong1.BringToFront();
        }

        private void btnHomePage_Click(object sender, EventArgs e)
        {
            uC_HomePage1.BringToFront();
        }

        private void btnAddUc_ChamCong_Click(object sender, EventArgs e)
        {
            uC_ChamCong2.BringToFront();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            uC_QuanLyTaiKhoan1.BringToFront();
        }
        
        public string contentGioiThieu = @"TÊN CÔNG TY: TECHVISION GLOBAL CORP

VỀ CHÚNG TÔI:
Được thành lập năm 2010, TechVision là đơn vị tiên phong trong lĩnh vực cung cấp giải pháp chuyển đổi số toàn diện. Với đội ngũ hơn 500 kỹ sư chất lượng cao, chúng tôi cam kết mang lại giá trị bền vững cho khách hàng.

TẦM NHÌN:
Trở thành tập đoàn công nghệ hàng đầu khu vực Đông Nam Á vào năm 2030.

SỨ MỆNH:
Kiến tạo giải pháp thông minh - Nâng tầm cuộc sống Việt.

TRỤ SỞ CHÍNH:
Tòa nhà Tech Tower, Khu Công nghệ cao Hòa Lạc, Hà Nội.";

        // 2. Dữ liệu Quy định - Biểu mẫu
        public string contentQuyDinh = @"QUY ĐỊNH LÀM VIỆC & BIỂU MẪU NHÂN SỰ

I. THỜI GIAN LÀM VIỆC:
- Sáng: 08:00 - 12:00
- Chiều: 13:30 - 17:30
- Làm việc từ Thứ 2 đến Thứ 6 (Nghỉ T7, CN)

II. TRANG PHỤC (DRESS CODE):
- Thứ 2: Đồng phục công ty.
- Các ngày còn lại: Tự do, lịch sự (Smart Casual).

III. CÁC BIỂU MẪU CẦN BIẾT:
1. Mẫu xin nghỉ phép (Form NP-01) - Gửi trước 2 ngày.
2. Mẫu đề nghị tăng lương (Form TL-02) - Định kỳ tháng 6 hàng năm.
3. Mẫu thanh toán công tác phí (Form CP-03).

(Vui lòng liên hệ phòng HR để nhận file cứng hoặc tải trên Portal nội bộ)";

        // 3. Dữ liệu Tuyển dụng
        public string contentTuyenDung = @"TIN TUYỂN DỤNG THÁNG 11/2025

TechVision đang tìm kiếm những mảnh ghép tài năng cho các vị trí sau:

1. SENIOR .NET DEVELOPER (03 người)
- Yêu cầu: 3+ năm kinh nghiệm C#, .NET Core, SQL Server.
- Mức lương: 1.500$ - 2.500$
- Mô tả: Tham gia phát triển hệ thống ERP cốt lõi.

2. FRONTEND DEVELOPER (ReactJS) (05 người)
- Yêu cầu: Thành thạo HTML/CSS, JS, React Hook.
- Mức lương: 1.000$ - 1.800$

3. SOFTWARE TESTER / QC (02 người)
- Yêu cầu: Tư duy logic tốt, biết viết Test Case, ưu tiên biết Automation.
- Mức lương: 10 - 20 triệu VNĐ.

>> Ứng tuyển gửi CV về: hr@techvision.com.vn";

        // Sự kiện Click của Label GIỚI THIỆU
        private void lblGioiThieu_Click(object sender, EventArgs e)
        {
            FormThongTin frm = new FormThongTin("Giới Thiệu Công Ty", contentGioiThieu);
            frm.ShowDialog(); // ShowDialog để bắt buộc người dùng đóng form này mới làm việc tiếp được
        }

        // Sự kiện Click của Label QUY ĐỊNH
        private void lblQuyDinh_Click(object sender, EventArgs e)
        {
            FormThongTin frm = new FormThongTin("Quy Định & Biểu Mẫu", contentQuyDinh);
            frm.ShowDialog();
        }

        // Sự kiện Click của Label TUYỂN DỤNG
        private void lblTuyenDung_Click(object sender, EventArgs e)
        {
            FormThongTin frm = new FormThongTin("Thông Tin Tuyển Dụng", contentTuyenDung);
            frm.ShowDialog();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            // 1. Hỏi người dùng có chắc chắn muốn thoát không
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // 2. Nếu chọn Yes thì xử lý
            if (result == DialogResult.Yes)
            {
                // Ẩn form chính đi (để tránh giật màn hình)
                this.Hide();

                // 3. Mở lại Form Đăng Nhập
                // LƯU Ý: Thay 'FormDangNhap' bằng tên thực tế của Form đăng nhập trong dự án của bạn (ví dụ: FrmLogin, Form1...)
                Form1 loginForm = new Form1();
                loginForm.ShowDialog(); // Hiển thị dưới dạng hộp thoại

                // 4. Sau khi Form Đăng nhập đóng lại (hoặc đăng nhập lại thành công), 
                // ta đóng hoàn toàn Form chính hiện tại để giải phóng tài nguyên
                this.Close();
            }
        }
        // Sửa lại hàm này trong TrangChu.cs

    }
}
