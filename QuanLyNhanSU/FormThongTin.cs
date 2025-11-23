using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhanSU
{
    public partial class FormThongTin : Form
    {
        public FormThongTin(string tieuDe, string noiDung)
        {
            InitializeComponent();

            // 1. Cài đặt tiêu đề
            // Đảm bảo bạn đã đặt tên Label tiêu đề là 'lblTitle' trong Designer
            if (lblTitle != null)
            {
                lblTitle.Text = tieuDe.ToUpper();
            }

            // 2. Xử lý nội dung đẹp
            HienThiNoiDungDep(noiDung);

            // 3. Xử lý nút đóng cho đẹp (Bo tròn nhẹ nếu thích)
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.BackColor = Color.Crimson;
            btnDong.ForeColor = Color.White;
            btnDong.Cursor = Cursors.Hand;
        }

       

        private void HienThiNoiDungDep(string rawText)
        {
            rtbNoiDung.Text = "";
            rtbNoiDung.BackColor = Color.White; // Đảm bảo nền trắng

            string[] lines = rawText.Split(new[] { '\n' }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                // Logic tô màu: Dòng nào có dấu hai chấm : ở cuối hoặc viết HOA HẾT -> Tiêu đề mục
                if (IsHeader(line))
                {
                    // Màu xanh đậm cho tiêu đề mục
                    AppendText(line + "\n", new Font("Segoe UI", 12, FontStyle.Bold), Color.FromArgb(0, 122, 204));
                    AppendText("\n", new Font("Segoe UI", 4), Color.Black);
                }
                else if (line.Contains("TÊN CÔNG TY"))
                {
                    // Tên công ty màu đỏ cam nổi bật
                    AppendText(line + "\n\n", new Font("Segoe UI", 15, FontStyle.Bold), Color.OrangeRed);
                }
                else
                {
                    // Nội dung thường màu xám đen
                    AppendText(line + "\n", new Font("Segoe UI", 11, FontStyle.Regular), Color.FromArgb(50, 50, 50));
                }
            }
        }

        private void AppendText(string text, Font font, Color color)
        {
            rtbNoiDung.SelectionStart = rtbNoiDung.TextLength;
            rtbNoiDung.SelectionLength = 0;
            rtbNoiDung.SelectionColor = color;
            rtbNoiDung.SelectionFont = font;
            rtbNoiDung.AppendText(text);
            rtbNoiDung.SelectionColor = rtbNoiDung.ForeColor;
        }

        private bool IsHeader(string line)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line)) return false;
            return line.EndsWith(":") || (line == line.ToUpper() && line.Length > 3 && line.Length < 60);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- XÓA HÀM OnPaint CŨ ĐI NHÉ ---
        // Vì chúng ta đã dùng Padding 3px của Form để làm viền rồi, không cần vẽ đè lên nữa.
    }
}