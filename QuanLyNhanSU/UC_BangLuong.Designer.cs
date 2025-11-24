namespace QuanLyNhanSU
{
    partial class UC_BangLuong
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnHienNVungluong = new System.Windows.Forms.Button();
            this.btnHienNangLuong = new System.Windows.Forms.Button();
            this.btnQuayLai = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnInPhieuLuong = new System.Windows.Forms.Button();
            this.btnTinhLuong = new System.Windows.Forms.Button();
            this.dtpKyLuong = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvBangLuong = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MANV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HOTEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LUONGCB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NGAYCONG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PHUCAP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.THUONG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KYLUAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UNGLUONG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.THUCLINH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnHienAll = new System.Windows.Forms.Button();
            this.btnTiemKiemLuong = new System.Windows.Forms.Button();
            this.txtTimKiemLuong = new System.Windows.Forms.TextBox();
            this.cboTimKiemLuong = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBangLuong)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnHienNVungluong);
            this.groupBox1.Controls.Add(this.btnHienNangLuong);
            this.groupBox1.Controls.Add(this.btnQuayLai);
            this.groupBox1.Controls.Add(this.btnXoa);
            this.groupBox1.Controls.Add(this.btnInPhieuLuong);
            this.groupBox1.Controls.Add(this.btnTinhLuong);
            this.groupBox1.Controls.Add(this.dtpKyLuong);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(67, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1460, 181);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin kỳ lương";
            // 
            // btnHienNVungluong
            // 
            this.btnHienNVungluong.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnHienNVungluong.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHienNVungluong.ForeColor = System.Drawing.Color.White;
            this.btnHienNVungluong.Location = new System.Drawing.Point(911, 104);
            this.btnHienNVungluong.Name = "btnHienNVungluong";
            this.btnHienNVungluong.Size = new System.Drawing.Size(413, 50);
            this.btnHienNVungluong.TabIndex = 3;
            this.btnHienNVungluong.Text = "Xem Quá Trình Ứng Lương";
            this.btnHienNVungluong.UseVisualStyleBackColor = false;
            this.btnHienNVungluong.Click += new System.EventHandler(this.btnHienNVungluong_Click);
            // 
            // btnHienNangLuong
            // 
            this.btnHienNangLuong.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnHienNangLuong.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHienNangLuong.ForeColor = System.Drawing.Color.White;
            this.btnHienNangLuong.Location = new System.Drawing.Point(388, 104);
            this.btnHienNangLuong.Name = "btnHienNangLuong";
            this.btnHienNangLuong.Size = new System.Drawing.Size(383, 50);
            this.btnHienNangLuong.TabIndex = 3;
            this.btnHienNangLuong.Text = "Xem Quá Trình Nâng Lương";
            this.btnHienNangLuong.UseVisualStyleBackColor = false;
            this.btnHienNangLuong.Click += new System.EventHandler(this.btnHienNangLuong_Click);
            // 
            // btnQuayLai
            // 
            this.btnQuayLai.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnQuayLai.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuayLai.ForeColor = System.Drawing.Color.White;
            this.btnQuayLai.Location = new System.Drawing.Point(1247, 28);
            this.btnQuayLai.Name = "btnQuayLai";
            this.btnQuayLai.Size = new System.Drawing.Size(174, 50);
            this.btnQuayLai.TabIndex = 2;
            this.btnQuayLai.Text = "QUAY LẠI";
            this.btnQuayLai.UseVisualStyleBackColor = false;
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnXoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(911, 28);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(233, 50);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "XÓA KỲ LƯƠNG";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnInPhieuLuong
            // 
            this.btnInPhieuLuong.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnInPhieuLuong.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInPhieuLuong.ForeColor = System.Drawing.Color.White;
            this.btnInPhieuLuong.Location = new System.Drawing.Point(661, 28);
            this.btnInPhieuLuong.Name = "btnInPhieuLuong";
            this.btnInPhieuLuong.Size = new System.Drawing.Size(230, 50);
            this.btnInPhieuLuong.TabIndex = 2;
            this.btnInPhieuLuong.Text = "IN PHIẾU LƯƠNG";
            this.btnInPhieuLuong.UseVisualStyleBackColor = false;
            this.btnInPhieuLuong.Click += new System.EventHandler(this.btnInPhieuLuong_Click);
            // 
            // btnTinhLuong
            // 
            this.btnTinhLuong.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnTinhLuong.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTinhLuong.ForeColor = System.Drawing.Color.White;
            this.btnTinhLuong.Location = new System.Drawing.Point(388, 28);
            this.btnTinhLuong.Name = "btnTinhLuong";
            this.btnTinhLuong.Size = new System.Drawing.Size(267, 50);
            this.btnTinhLuong.TabIndex = 2;
            this.btnTinhLuong.Text = "TÍNH LƯƠNG";
            this.btnTinhLuong.UseVisualStyleBackColor = false;
            this.btnTinhLuong.Click += new System.EventHandler(this.btnTinhLuong_Click);
            // 
            // dtpKyLuong
            // 
            this.dtpKyLuong.CustomFormat = "MM/yyyy";
            this.dtpKyLuong.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpKyLuong.Location = new System.Drawing.Point(131, 41);
            this.dtpKyLuong.Name = "dtpKyLuong";
            this.dtpKyLuong.ShowUpDown = true;
            this.dtpKyLuong.Size = new System.Drawing.Size(246, 22);
            this.dtpKyLuong.TabIndex = 1;
            this.dtpKyLuong.ValueChanged += new System.EventHandler(this.dtpKyLuong_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Kỳ lương";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MediumTurquoise;
            this.label1.Location = new System.Drawing.Point(547, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(493, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "BẢNG LƯƠNG NHÂN VIÊN";
            // 
            // dgvBangLuong
            // 
            this.dgvBangLuong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBangLuong.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.MANV,
            this.HOTEN,
            this.LUONGCB,
            this.NGAYCONG,
            this.PHUCAP,
            this.THUONG,
            this.KYLUAT,
            this.UNGLUONG,
            this.THUCLINH});
            this.dgvBangLuong.Location = new System.Drawing.Point(67, 360);
            this.dgvBangLuong.Name = "dgvBangLuong";
            this.dgvBangLuong.ReadOnly = true;
            this.dgvBangLuong.RowHeadersWidth = 51;
            this.dgvBangLuong.RowTemplate.Height = 24;
            this.dgvBangLuong.Size = new System.Drawing.Size(1479, 324);
            this.dgvBangLuong.TabIndex = 2;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 60;
            // 
            // MANV
            // 
            this.MANV.HeaderText = "Mã NV";
            this.MANV.MinimumWidth = 6;
            this.MANV.Name = "MANV";
            this.MANV.ReadOnly = true;
            this.MANV.Width = 125;
            // 
            // HOTEN
            // 
            this.HOTEN.HeaderText = "Họ Tên";
            this.HOTEN.MinimumWidth = 6;
            this.HOTEN.Name = "HOTEN";
            this.HOTEN.ReadOnly = true;
            this.HOTEN.Width = 175;
            // 
            // LUONGCB
            // 
            this.LUONGCB.HeaderText = "Lương Hợp Đồng";
            this.LUONGCB.MinimumWidth = 6;
            this.LUONGCB.Name = "LUONGCB";
            this.LUONGCB.ReadOnly = true;
            this.LUONGCB.Width = 125;
            // 
            // NGAYCONG
            // 
            this.NGAYCONG.HeaderText = "Ngày Công";
            this.NGAYCONG.MinimumWidth = 6;
            this.NGAYCONG.Name = "NGAYCONG";
            this.NGAYCONG.ReadOnly = true;
            this.NGAYCONG.Width = 70;
            // 
            // PHUCAP
            // 
            this.PHUCAP.HeaderText = "Phụ Cấp";
            this.PHUCAP.MinimumWidth = 6;
            this.PHUCAP.Name = "PHUCAP";
            this.PHUCAP.ReadOnly = true;
            this.PHUCAP.Width = 125;
            // 
            // THUONG
            // 
            this.THUONG.HeaderText = "Thưởng";
            this.THUONG.MinimumWidth = 6;
            this.THUONG.Name = "THUONG";
            this.THUONG.ReadOnly = true;
            this.THUONG.Width = 125;
            // 
            // KYLUAT
            // 
            this.KYLUAT.HeaderText = "Kỷ luật";
            this.KYLUAT.MinimumWidth = 6;
            this.KYLUAT.Name = "KYLUAT";
            this.KYLUAT.ReadOnly = true;
            this.KYLUAT.Width = 125;
            // 
            // UNGLUONG
            // 
            this.UNGLUONG.HeaderText = "Tiền Đã Ứng";
            this.UNGLUONG.MinimumWidth = 6;
            this.UNGLUONG.Name = "UNGLUONG";
            this.UNGLUONG.ReadOnly = true;
            this.UNGLUONG.Width = 125;
            // 
            // THUCLINH
            // 
            this.THUCLINH.HeaderText = "Thực Lĩnh";
            this.THUCLINH.MinimumWidth = 6;
            this.THUCLINH.Name = "THUCLINH";
            this.THUCLINH.ReadOnly = true;
            this.THUCLINH.Width = 150;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnHienAll);
            this.groupBox2.Controls.Add(this.btnTiemKiemLuong);
            this.groupBox2.Controls.Add(this.txtTimKiemLuong);
            this.groupBox2.Controls.Add(this.cboTimKiemLuong);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(67, 242);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1479, 80);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tìm Kiếm Lương Nhân Viên";
            // 
            // btnHienAll
            // 
            this.btnHienAll.BackColor = System.Drawing.Color.Cyan;
            this.btnHienAll.Location = new System.Drawing.Point(1247, 27);
            this.btnHienAll.Name = "btnHienAll";
            this.btnHienAll.Size = new System.Drawing.Size(147, 39);
            this.btnHienAll.TabIndex = 2;
            this.btnHienAll.Text = "Hiện tất cả";
            this.btnHienAll.UseVisualStyleBackColor = false;
            // 
            // btnTiemKiemLuong
            // 
            this.btnTiemKiemLuong.BackColor = System.Drawing.Color.Cyan;
            this.btnTiemKiemLuong.Location = new System.Drawing.Point(1039, 28);
            this.btnTiemKiemLuong.Name = "btnTiemKiemLuong";
            this.btnTiemKiemLuong.Size = new System.Drawing.Size(147, 39);
            this.btnTiemKiemLuong.TabIndex = 3;
            this.btnTiemKiemLuong.Text = "Tim kiếm";
            this.btnTiemKiemLuong.UseVisualStyleBackColor = false;
            // 
            // txtTimKiemLuong
            // 
            this.txtTimKiemLuong.Location = new System.Drawing.Point(248, 35);
            this.txtTimKiemLuong.Name = "txtTimKiemLuong";
            this.txtTimKiemLuong.Size = new System.Drawing.Size(746, 28);
            this.txtTimKiemLuong.TabIndex = 1;
            // 
            // cboTimKiemLuong
            // 
            this.cboTimKiemLuong.FormattingEnabled = true;
            this.cboTimKiemLuong.Location = new System.Drawing.Point(21, 33);
            this.cboTimKiemLuong.Name = "cboTimKiemLuong";
            this.cboTimKiemLuong.Size = new System.Drawing.Size(199, 30);
            this.cboTimKiemLuong.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label3.Location = new System.Drawing.Point(82, 709);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(700, 32);
            this.label3.TabIndex = 5;
            this.label3.Text = "*Ghi chú: Lương hợp đồng = (Lương Cơ Bản * HSL)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label4.Location = new System.Drawing.Point(82, 750);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1215, 32);
            this.label4.TabIndex = 5;
            this.label4.Text = "*Ghi chú: Thực lĩnh = ((Lương Hợp Đồng */26) * Ngày Công) + Phụ cấp + Thưởng - Kỹ" +
    " luật";
            // 
            // UC_BangLuong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dgvBangLuong);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "UC_BangLuong";
            this.Size = new System.Drawing.Size(1560, 970);
            this.Load += new System.EventHandler(this.UC_BangLuong_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBangLuong)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnQuayLai;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnInPhieuLuong;
        private System.Windows.Forms.Button btnTinhLuong;
        private System.Windows.Forms.DateTimePicker dtpKyLuong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvBangLuong;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnHienAll;
        private System.Windows.Forms.Button btnTiemKiemLuong;
        private System.Windows.Forms.TextBox txtTimKiemLuong;
        private System.Windows.Forms.ComboBox cboTimKiemLuong;
        private System.Windows.Forms.Button btnHienNVungluong;
        private System.Windows.Forms.Button btnHienNangLuong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn MANV;
        private System.Windows.Forms.DataGridViewTextBoxColumn HOTEN;
        private System.Windows.Forms.DataGridViewTextBoxColumn LUONGCB;
        private System.Windows.Forms.DataGridViewTextBoxColumn NGAYCONG;
        private System.Windows.Forms.DataGridViewTextBoxColumn PHUCAP;
        private System.Windows.Forms.DataGridViewTextBoxColumn THUONG;
        private System.Windows.Forms.DataGridViewTextBoxColumn KYLUAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn UNGLUONG;
        private System.Windows.Forms.DataGridViewTextBoxColumn THUCLINH;
    }
}
