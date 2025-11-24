namespace QuanLyNhanSU
{
    partial class UC_ThoiViec
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
            this.btnHienAll = new System.Windows.Forms.Button();
            this.btnTimNV = new System.Windows.Forms.Button();
            this.txtTimKiemNV = new System.Windows.Forms.TextBox();
            this.cboTimKiemNV = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSua = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTenNV = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboMaNV = new System.Windows.Forms.ComboBox();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLyDo = new System.Windows.Forms.TextBox();
            this.dtpNgayNghi = new System.Windows.Forms.DateTimePicker();
            this.dtpNgayNop = new System.Windows.Forms.DateTimePicker();
            this.txtSoQD = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.dgvThoiViec = new System.Windows.Forms.DataGridView();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThoiViec)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHienAll
            // 
            this.btnHienAll.BackColor = System.Drawing.Color.Aqua;
            this.btnHienAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnHienAll.Location = new System.Drawing.Point(987, 31);
            this.btnHienAll.Name = "btnHienAll";
            this.btnHienAll.Size = new System.Drawing.Size(174, 37);
            this.btnHienAll.TabIndex = 3;
            this.btnHienAll.Text = "Hiện tất cả";
            this.btnHienAll.UseVisualStyleBackColor = false;
            this.btnHienAll.Click += new System.EventHandler(this.btnHienAll_Click);
            // 
            // btnTimNV
            // 
            this.btnTimNV.BackColor = System.Drawing.Color.Aqua;
            this.btnTimNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnTimNV.Location = new System.Drawing.Point(836, 29);
            this.btnTimNV.Name = "btnTimNV";
            this.btnTimNV.Size = new System.Drawing.Size(118, 37);
            this.btnTimNV.TabIndex = 3;
            this.btnTimNV.Text = "Tìm";
            this.btnTimNV.UseVisualStyleBackColor = false;
            this.btnTimNV.Click += new System.EventHandler(this.btnTimNV_Click);
            // 
            // txtTimKiemNV
            // 
            this.txtTimKiemNV.Location = new System.Drawing.Point(244, 38);
            this.txtTimKiemNV.Name = "txtTimKiemNV";
            this.txtTimKiemNV.Size = new System.Drawing.Size(566, 28);
            this.txtTimKiemNV.TabIndex = 1;
            // 
            // cboTimKiemNV
            // 
            this.cboTimKiemNV.FormattingEnabled = true;
            this.cboTimKiemNV.Location = new System.Drawing.Point(17, 36);
            this.cboTimKiemNV.Name = "cboTimKiemNV";
            this.cboTimKiemNV.Size = new System.Drawing.Size(199, 30);
            this.cboTimKiemNV.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnHienAll);
            this.groupBox2.Controls.Add(this.btnTimNV);
            this.groupBox2.Controls.Add(this.txtTimKiemNV);
            this.groupBox2.Controls.Add(this.cboTimKiemNV);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(214, 306);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1167, 80);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tìm Kiếm Nhân Viên";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(789, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Ngày Nghĩ";
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(735, 674);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(162, 36);
            this.btnSua.TabIndex = 30;
            this.btnSua.Text = "SỬA";
            this.btnSua.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(474, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "Tên nhân viên";
            // 
            // txtTenNV
            // 
            this.txtTenNV.Location = new System.Drawing.Point(606, 105);
            this.txtTenNV.Name = "txtTenNV";
            this.txtTenNV.Size = new System.Drawing.Size(282, 27);
            this.txtTenNV.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(467, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Ngày Nộp Đơn";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Số Quyết Định";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Mã Nhân viên ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.MediumTurquoise;
            this.label1.Location = new System.Drawing.Point(586, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 41);
            this.label1.TabIndex = 25;
            this.label1.Text = "Quản Lý Thôi Việc Nhân Viên";
            // 
            // cboMaNV
            // 
            this.cboMaNV.FormattingEnabled = true;
            this.cboMaNV.Location = new System.Drawing.Point(160, 102);
            this.cboMaNV.Name = "cboMaNV";
            this.cboMaNV.Size = new System.Drawing.Size(282, 28);
            this.cboMaNV.TabIndex = 3;
            this.cboMaNV.SelectedIndexChanged += new System.EventHandler(this.cboMaNV_SelectedIndexChanged);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(331, 674);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(162, 36);
            this.btnLuu.TabIndex = 27;
            this.btnLuu.Text = "LƯU";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(1157, 674);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(162, 36);
            this.btnLamMoi.TabIndex = 28;
            this.btnLamMoi.Text = "LÀM MỚI";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtLyDo);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtTenNV);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpNgayNghi);
            this.groupBox1.Controls.Add(this.dtpNgayNop);
            this.groupBox1.Controls.Add(this.cboMaNV);
            this.groupBox1.Controls.Add(this.txtSoQD);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(214, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1167, 226);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 20);
            this.label7.TabIndex = 19;
            this.label7.Text = "Lý Do";
            // 
            // txtLyDo
            // 
            this.txtLyDo.Location = new System.Drawing.Point(160, 148);
            this.txtLyDo.Multiline = true;
            this.txtLyDo.Name = "txtLyDo";
            this.txtLyDo.Size = new System.Drawing.Size(728, 61);
            this.txtLyDo.TabIndex = 18;
            // 
            // dtpNgayNghi
            // 
            this.dtpNgayNghi.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayNghi.Location = new System.Drawing.Point(907, 34);
            this.dtpNgayNghi.Name = "dtpNgayNghi";
            this.dtpNgayNghi.Size = new System.Drawing.Size(146, 27);
            this.dtpNgayNghi.TabIndex = 5;
            // 
            // dtpNgayNop
            // 
            this.dtpNgayNop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayNop.Location = new System.Drawing.Point(606, 34);
            this.dtpNgayNop.Name = "dtpNgayNop";
            this.dtpNgayNop.Size = new System.Drawing.Size(146, 27);
            this.dtpNgayNop.TabIndex = 5;
            // 
            // txtSoQD
            // 
            this.txtSoQD.Location = new System.Drawing.Point(160, 36);
            this.txtSoQD.Name = "txtSoQD";
            this.txtSoQD.Size = new System.Drawing.Size(282, 27);
            this.txtSoQD.TabIndex = 1;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(538, 674);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(162, 36);
            this.btnThem.TabIndex = 26;
            this.btnThem.Text = "THÊM";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(941, 674);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(162, 36);
            this.btnXoa.TabIndex = 29;
            this.btnXoa.Text = "XÓA";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // dgvThoiViec
            // 
            this.dgvThoiViec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThoiViec.Location = new System.Drawing.Point(214, 405);
            this.dgvThoiViec.Name = "dgvThoiViec";
            this.dgvThoiViec.ReadOnly = true;
            this.dgvThoiViec.RowHeadersWidth = 51;
            this.dgvThoiViec.RowTemplate.Height = 24;
            this.dgvThoiViec.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThoiViec.Size = new System.Drawing.Size(1167, 263);
            this.dgvThoiViec.TabIndex = 32;
            this.dgvThoiViec.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThoiViec_CellClick);
            // 
            // UC_ThoiViec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvThoiViec);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.btnXoa);
            this.Name = "UC_ThoiViec";
            this.Size = new System.Drawing.Size(1493, 742);
            this.Load += new System.EventHandler(this.UC_ThoiViec_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThoiViec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnHienAll;
        private System.Windows.Forms.Button btnTimNV;
        private System.Windows.Forms.TextBox txtTimKiemNV;
        private System.Windows.Forms.ComboBox cboTimKiemNV;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTenNV;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboMaNV;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSoQD;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLyDo;
        private System.Windows.Forms.DateTimePicker dtpNgayNghi;
        private System.Windows.Forms.DateTimePicker dtpNgayNop;
        private System.Windows.Forms.DataGridView dgvThoiViec;
    }
}
