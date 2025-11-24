using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhanSU
{
    public static class Const
    {
        // 1. Biến lưu loại tài khoản (1 = Admin, 2 = User)
        // Mặc định là -1 (nghĩa là chưa đăng nhập)
        public static int LoaiTaiKhoan = -1;

        // 2. Biến lưu Mã nhân viên (Ví dụ: "NV001")
        // Dùng để tự động chọn tên trong form Chấm công
        public static string MaNV = "";

        // 3. Biến lưu Họ tên (Ví dụ: "Nguyễn Văn A")
        // Dùng để hiển thị "Xin chào..." trên màn hình chính
        public static string TenHienThi = "";

        // ==============================================
        // Hàm reset: Dùng khi bấm nút Đăng Xuất
        // Giúp xóa sạch dữ liệu cũ, tránh người sau vào bị dính quyền người trước
        // ==============================================
        public static void XoaThongTin()
        {
            LoaiTaiKhoan = -1;
            MaNV = "";
            TenHienThi = "";
        }
    }
}
