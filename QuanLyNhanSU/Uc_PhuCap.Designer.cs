namespace QuanLyNhanSU
{
    partial class Uc_PhuCap
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
            this.btnSua = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnThem = new System.Windows.Forms.Button();
            this.dtpNgayBatDau = new System.Windows.Forms.DateTimePicker();
            this.cboMaNVPC = new System.Windows.Forms.ComboBox();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.txtIDPC = new System.Windows.Forms.TextBox();
            this.btnXoa = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnThemPCMoi = new System.Windows.Forms.Button();
            this.txtSotienPC = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTenNVPC = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboTenPC = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnHienAll = new System.Windows.Forms.Button();
            this.btnTimPC = new System.Windows.Forms.Button();
            this.txtTimKiemNVPC = new System.Windows.Forms.TextBox();
            this.cboTimKiemPC = new System.Windows.Forms.ComboBox();
            this.dgvPhuCap = new System.Windows.Forms.DataGridView();
            this.Idphucap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaNhanVien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tennhanvien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tenphucap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ngayphucap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sotienphucap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhuCap)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(683, 674);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(162, 36);
            this.btnSua.TabIndex = 21;
            this.btnSua.Text = "SỬA";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "ID";
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
            this.label1.Location = new System.Drawing.Point(534, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(410, 41);
            this.label1.TabIndex = 16;
            this.label1.Text = "Quản Lý Phụ Cấp Nhân Viên";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(474, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 20);
            this.label9.TabIndex = 10;
            this.label9.Text = "Ngày ký";
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(486, 674);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(162, 36);
            this.btnThem.TabIndex = 17;
            this.btnThem.Text = "THÊM";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // dtpNgayBatDau
            // 
            this.dtpNgayBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayBatDau.Location = new System.Drawing.Point(606, 38);
            this.dtpNgayBatDau.Name = "dtpNgayBatDau";
            this.dtpNgayBatDau.Size = new System.Drawing.Size(146, 27);
            this.dtpNgayBatDau.TabIndex = 5;
            // 
            // cboMaNVPC
            // 
            this.cboMaNVPC.FormattingEnabled = true;
            this.cboMaNVPC.Location = new System.Drawing.Point(160, 102);
            this.cboMaNVPC.Name = "cboMaNVPC";
            this.cboMaNVPC.Size = new System.Drawing.Size(282, 28);
            this.cboMaNVPC.TabIndex = 3;
            this.cboMaNVPC.SelectedIndexChanged += new System.EventHandler(this.cboMaNVPC_SelectedIndexChanged);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(279, 674);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(162, 36);
            this.btnLuu.TabIndex = 18;
            this.btnLuu.Text = "LƯU";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(1105, 674);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(162, 36);
            this.btnLamMoi.TabIndex = 19;
            this.btnLamMoi.Text = "LÀM MỚI";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // txtIDPC
            // 
            this.txtIDPC.Location = new System.Drawing.Point(160, 36);
            this.txtIDPC.Name = "txtIDPC";
            this.txtIDPC.Size = new System.Drawing.Size(282, 27);
            this.txtIDPC.TabIndex = 1;
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(889, 674);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(162, 36);
            this.btnXoa.TabIndex = 20;
            this.btnXoa.Text = "XÓA";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnThemPCMoi);
            this.groupBox1.Controls.Add(this.txtSotienPC);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtTenNVPC);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboTenPC);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.dtpNgayBatDau);
            this.groupBox1.Controls.Add(this.cboMaNVPC);
            this.groupBox1.Controls.Add(this.txtIDPC);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(162, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1167, 226);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin phụ cấp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(474, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Số tiền phụ cấp";
            // 
            // btnThemPCMoi
            // 
            this.btnThemPCMoi.BackColor = System.Drawing.Color.Cyan;
            this.btnThemPCMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemPCMoi.Location = new System.Drawing.Point(907, 153);
            this.btnThemPCMoi.Name = "btnThemPCMoi";
            this.btnThemPCMoi.Size = new System.Drawing.Size(198, 39);
            this.btnThemPCMoi.TabIndex = 2;
            this.btnThemPCMoi.Text = "Thêm Phụ Cấp";
            this.btnThemPCMoi.UseVisualStyleBackColor = false;
            this.btnThemPCMoi.Click += new System.EventHandler(this.btnThemPCMoi_Click);
            // 
            // txtSotienPC
            // 
            this.txtSotienPC.Location = new System.Drawing.Point(606, 161);
            this.txtSotienPC.Name = "txtSotienPC";
            this.txtSotienPC.Size = new System.Drawing.Size(282, 27);
            this.txtSotienPC.TabIndex = 16;
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
            // txtTenNVPC
            // 
            this.txtTenNVPC.Location = new System.Drawing.Point(606, 105);
            this.txtTenNVPC.Name = "txtTenNVPC";
            this.txtTenNVPC.Size = new System.Drawing.Size(282, 27);
            this.txtTenNVPC.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Tên Phụ Cấp";
            // 
            // cboTenPC
            // 
            this.cboTenPC.FormattingEnabled = true;
            this.cboTenPC.Location = new System.Drawing.Point(160, 158);
            this.cboTenPC.Name = "cboTenPC";
            this.cboTenPC.Size = new System.Drawing.Size(282, 28);
            this.cboTenPC.TabIndex = 12;
            this.cboTenPC.SelectedIndexChanged += new System.EventHandler(this.cboTenPC_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnHienAll);
            this.groupBox2.Controls.Add(this.btnTimPC);
            this.groupBox2.Controls.Add(this.txtTimKiemNVPC);
            this.groupBox2.Controls.Add(this.cboTimKiemPC);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(162, 306);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1167, 80);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tìm Kiếm Nhân Viên";
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
            // btnTimPC
            // 
            this.btnTimPC.BackColor = System.Drawing.Color.Aqua;
            this.btnTimPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnTimPC.Location = new System.Drawing.Point(836, 29);
            this.btnTimPC.Name = "btnTimPC";
            this.btnTimPC.Size = new System.Drawing.Size(118, 37);
            this.btnTimPC.TabIndex = 3;
            this.btnTimPC.Text = "Tìm";
            this.btnTimPC.UseVisualStyleBackColor = false;
            this.btnTimPC.Click += new System.EventHandler(this.btnTimPC_Click);
            // 
            // txtTimKiemNVPC
            // 
            this.txtTimKiemNVPC.Location = new System.Drawing.Point(244, 38);
            this.txtTimKiemNVPC.Name = "txtTimKiemNVPC";
            this.txtTimKiemNVPC.Size = new System.Drawing.Size(566, 28);
            this.txtTimKiemNVPC.TabIndex = 1;
            // 
            // cboTimKiemPC
            // 
            this.cboTimKiemPC.FormattingEnabled = true;
            this.cboTimKiemPC.Location = new System.Drawing.Point(17, 36);
            this.cboTimKiemPC.Name = "cboTimKiemPC";
            this.cboTimKiemPC.Size = new System.Drawing.Size(199, 30);
            this.cboTimKiemPC.TabIndex = 0;
            // 
            // dgvPhuCap
            // 
            this.dgvPhuCap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhuCap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Idphucap,
            this.MaNhanVien,
            this.Tennhanvien,
            this.Tenphucap,
            this.Ngayphucap,
            this.Sotienphucap});
            this.dgvPhuCap.Location = new System.Drawing.Point(162, 411);
            this.dgvPhuCap.Name = "dgvPhuCap";
            this.dgvPhuCap.ReadOnly = true;
            this.dgvPhuCap.RowHeadersWidth = 51;
            this.dgvPhuCap.RowTemplate.Height = 24;
            this.dgvPhuCap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhuCap.Size = new System.Drawing.Size(1167, 257);
            this.dgvPhuCap.TabIndex = 23;
            this.dgvPhuCap.Click += new System.EventHandler(this.dgvPhuCap_Click);
            // 
            // Idphucap
            // 
            this.Idphucap.HeaderText = "ID Phụ Cấp";
            this.Idphucap.MinimumWidth = 6;
            this.Idphucap.Name = "Idphucap";
            this.Idphucap.ReadOnly = true;
            this.Idphucap.Width = 125;
            // 
            // MaNhanVien
            // 
            this.MaNhanVien.HeaderText = "Mã Nhân Viên";
            this.MaNhanVien.MinimumWidth = 6;
            this.MaNhanVien.Name = "MaNhanVien";
            this.MaNhanVien.ReadOnly = true;
            this.MaNhanVien.Width = 125;
            // 
            // Tennhanvien
            // 
            this.Tennhanvien.HeaderText = "Tên Nhân Viên";
            this.Tennhanvien.MinimumWidth = 6;
            this.Tennhanvien.Name = "Tennhanvien";
            this.Tennhanvien.ReadOnly = true;
            this.Tennhanvien.Width = 200;
            // 
            // Tenphucap
            // 
            this.Tenphucap.HeaderText = "Tên Phụ Cấp";
            this.Tenphucap.MinimumWidth = 6;
            this.Tenphucap.Name = "Tenphucap";
            this.Tenphucap.ReadOnly = true;
            this.Tenphucap.Width = 200;
            // 
            // Ngayphucap
            // 
            this.Ngayphucap.HeaderText = "Ngày Phụ Cấp";
            this.Ngayphucap.MinimumWidth = 6;
            this.Ngayphucap.Name = "Ngayphucap";
            this.Ngayphucap.ReadOnly = true;
            this.Ngayphucap.Width = 200;
            // 
            // Sotienphucap
            // 
            this.Sotienphucap.HeaderText = "Số Tiền Phụ Cấp";
            this.Sotienphucap.MinimumWidth = 6;
            this.Sotienphucap.Name = "Sotienphucap";
            this.Sotienphucap.ReadOnly = true;
            this.Sotienphucap.Width = 200;
            // 
            // Uc_PhuCap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvPhuCap);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.groupBox1);
            this.Name = "Uc_PhuCap";
            this.Size = new System.Drawing.Size(1493, 742);
            this.Load += new System.EventHandler(this.Uc_PhuCap_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhuCap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.DateTimePicker dtpNgayBatDau;
        private System.Windows.Forms.ComboBox cboMaNVPC;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.TextBox txtIDPC;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboTenPC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSotienPC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTenNVPC;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtTimKiemNVPC;
        private System.Windows.Forms.ComboBox cboTimKiemPC;
        private System.Windows.Forms.Button btnTimPC;
        private System.Windows.Forms.Button btnThemPCMoi;
        private System.Windows.Forms.DataGridView dgvPhuCap;
        private System.Windows.Forms.DataGridViewTextBoxColumn Idphucap;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaNhanVien;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tennhanvien;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tenphucap;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ngayphucap;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sotienphucap;
        private System.Windows.Forms.Button btnHienAll;
    }
}
