namespace QuanLyNhanSU
{
    partial class UC_BaoHiemNV
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
            this.cboChonNhanVien = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNoiKham = new System.Windows.Forms.TextBox();
            this.txtTenNV = new System.Windows.Forms.TextBox();
            this.txtNoiCap = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpNgayCap = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIdBH = new System.Windows.Forms.TextBox();
            this.txtSoBH = new System.Windows.Forms.TextBox();
            this.dgvBaoHiem = new System.Windows.Forms.DataGridView();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboTKtenBH = new System.Windows.Forms.ComboBox();
            this.btnTkBH = new System.Windows.Forms.Button();
            this.btnHienAllBH = new System.Windows.Forms.Button();
            this.txtTimKiemBH = new System.Windows.Forms.TextBox();
            this.IdBH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TenNhanVien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SoBaoHiem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ngayCapBH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoiCapBH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoiKhamBenh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoHiem)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboChonNhanVien
            // 
            this.cboChonNhanVien.FormattingEnabled = true;
            this.cboChonNhanVien.Location = new System.Drawing.Point(183, 109);
            this.cboChonNhanVien.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboChonNhanVien.Name = "cboChonNhanVien";
            this.cboChonNhanVien.Size = new System.Drawing.Size(215, 30);
            this.cboChonNhanVien.TabIndex = 0;
            this.cboChonNhanVien.SelectedIndexChanged += new System.EventHandler(this.cboChonNhanVien_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MediumTurquoise;
            this.label1.Location = new System.Drawing.Point(570, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(436, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "Quản Lí Bảo Hiểm Nhân Viên";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtNoiKham);
            this.groupBox1.Controls.Add(this.txtTenNV);
            this.groupBox1.Controls.Add(this.txtNoiCap);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpNgayCap);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtIdBH);
            this.groupBox1.Controls.Add(this.txtSoBH);
            this.groupBox1.Controls.Add(this.cboChonNhanVien);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(238, 51);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1243, 308);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin bảo hiểm";
            // 
            // txtNoiKham
            // 
            this.txtNoiKham.Location = new System.Drawing.Point(679, 177);
            this.txtNoiKham.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNoiKham.Name = "txtNoiKham";
            this.txtNoiKham.Size = new System.Drawing.Size(234, 30);
            this.txtNoiKham.TabIndex = 9;
            // 
            // txtTenNV
            // 
            this.txtTenNV.Location = new System.Drawing.Point(186, 175);
            this.txtTenNV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTenNV.Name = "txtTenNV";
            this.txtTenNV.Size = new System.Drawing.Size(234, 30);
            this.txtTenNV.TabIndex = 8;
            // 
            // txtNoiCap
            // 
            this.txtNoiCap.Location = new System.Drawing.Point(679, 114);
            this.txtNoiCap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNoiCap.Name = "txtNoiCap";
            this.txtNoiCap.Size = new System.Drawing.Size(234, 30);
            this.txtNoiCap.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 175);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 22);
            this.label8.TabIndex = 6;
            this.label8.Text = "Tên nhân viên";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(500, 185);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 22);
            this.label6.TabIndex = 7;
            this.label6.Text = "Nơi khám bệnh";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(500, 114);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 22);
            this.label5.TabIndex = 6;
            this.label5.Text = "Nơi cấp";
            // 
            // dtpNgayCap
            // 
            this.dtpNgayCap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayCap.Location = new System.Drawing.Point(679, 255);
            this.dtpNgayCap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpNgayCap.Name = "dtpNgayCap";
            this.dtpNgayCap.Size = new System.Drawing.Size(287, 30);
            this.dtpNgayCap.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(503, 248);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "Ngày cấp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 242);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(108, 22);
            this.label3.TabIndex = 3;
            this.label3.Text = "Số bảo hiểm";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 48);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 22);
            this.label7.TabIndex = 2;
            this.label7.Text = "IDBH";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 115);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "Chọn nhân viên";
            // 
            // txtIdBH
            // 
            this.txtIdBH.Location = new System.Drawing.Point(183, 48);
            this.txtIdBH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIdBH.Name = "txtIdBH";
            this.txtIdBH.Size = new System.Drawing.Size(133, 30);
            this.txtIdBH.TabIndex = 1;
            // 
            // txtSoBH
            // 
            this.txtSoBH.Location = new System.Drawing.Point(186, 232);
            this.txtSoBH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSoBH.Name = "txtSoBH";
            this.txtSoBH.Size = new System.Drawing.Size(234, 30);
            this.txtSoBH.TabIndex = 1;
            // 
            // dgvBaoHiem
            // 
            this.dgvBaoHiem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBaoHiem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdBH,
            this.TenNhanVien,
            this.SoBaoHiem,
            this.ngayCapBH,
            this.NoiCapBH,
            this.NoiKhamBenh});
            this.dgvBaoHiem.Location = new System.Drawing.Point(238, 438);
            this.dgvBaoHiem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvBaoHiem.Name = "dgvBaoHiem";
            this.dgvBaoHiem.ReadOnly = true;
            this.dgvBaoHiem.RowHeadersWidth = 51;
            this.dgvBaoHiem.RowTemplate.Height = 24;
            this.dgvBaoHiem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBaoHiem.Size = new System.Drawing.Size(1243, 290);
            this.dgvBaoHiem.TabIndex = 3;
            this.dgvBaoHiem.Click += new System.EventHandler(this.dgvBaoHiem_Click);
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(486, 736);
            this.btnThem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(235, 58);
            this.btnThem.TabIndex = 4;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(743, 736);
            this.btnSua.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(235, 58);
            this.btnSua.TabIndex = 5;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(1229, 736);
            this.btnLamMoi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(235, 58);
            this.btnLamMoi.TabIndex = 6;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(986, 736);
            this.btnXoa.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(235, 58);
            this.btnXoa.TabIndex = 7;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(243, 736);
            this.btnLuu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(235, 58);
            this.btnLuu.TabIndex = 4;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboTKtenBH);
            this.groupBox2.Controls.Add(this.btnTkBH);
            this.groupBox2.Controls.Add(this.btnHienAllBH);
            this.groupBox2.Controls.Add(this.txtTimKiemBH);
            this.groupBox2.Location = new System.Drawing.Point(238, 366);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(1243, 65);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tìm kiếm bảo hiểm";
            // 
            // cboTKtenBH
            // 
            this.cboTKtenBH.FormattingEnabled = true;
            this.cboTKtenBH.Location = new System.Drawing.Point(17, 27);
            this.cboTKtenBH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboTKtenBH.Name = "cboTKtenBH";
            this.cboTKtenBH.Size = new System.Drawing.Size(187, 27);
            this.cboTKtenBH.TabIndex = 0;
            // 
            // btnTkBH
            // 
            this.btnTkBH.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnTkBH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTkBH.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTkBH.ForeColor = System.Drawing.Color.White;
            this.btnTkBH.Location = new System.Drawing.Point(782, 13);
            this.btnTkBH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTkBH.Name = "btnTkBH";
            this.btnTkBH.Size = new System.Drawing.Size(161, 48);
            this.btnTkBH.TabIndex = 7;
            this.btnTkBH.Text = "Tìm kiếm";
            this.btnTkBH.UseVisualStyleBackColor = false;
            this.btnTkBH.Click += new System.EventHandler(this.btnTkBH_Click);
            // 
            // btnHienAllBH
            // 
            this.btnHienAllBH.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnHienAllBH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHienAllBH.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHienAllBH.ForeColor = System.Drawing.Color.White;
            this.btnHienAllBH.Location = new System.Drawing.Point(966, 13);
            this.btnHienAllBH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHienAllBH.Name = "btnHienAllBH";
            this.btnHienAllBH.Size = new System.Drawing.Size(161, 48);
            this.btnHienAllBH.TabIndex = 6;
            this.btnHienAllBH.Text = "Hiện tất cả";
            this.btnHienAllBH.UseVisualStyleBackColor = false;
            this.btnHienAllBH.Click += new System.EventHandler(this.btnHienAllBH_Click);
            // 
            // txtTimKiemBH
            // 
            this.txtTimKiemBH.Location = new System.Drawing.Point(249, 27);
            this.txtTimKiemBH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTimKiemBH.Name = "txtTimKiemBH";
            this.txtTimKiemBH.Size = new System.Drawing.Size(454, 27);
            this.txtTimKiemBH.TabIndex = 1;
            // 
            // IdBH
            // 
            this.IdBH.HeaderText = "ID";
            this.IdBH.MinimumWidth = 6;
            this.IdBH.Name = "IdBH";
            this.IdBH.ReadOnly = true;
            this.IdBH.Width = 125;
            // 
            // TenNhanVien
            // 
            this.TenNhanVien.HeaderText = "Tên Nhân Viên";
            this.TenNhanVien.MinimumWidth = 6;
            this.TenNhanVien.Name = "TenNhanVien";
            this.TenNhanVien.Width = 300;
            // 
            // SoBaoHiem
            // 
            this.SoBaoHiem.HeaderText = "Số Bảo Hiểm";
            this.SoBaoHiem.MinimumWidth = 6;
            this.SoBaoHiem.Name = "SoBaoHiem";
            this.SoBaoHiem.Width = 125;
            // 
            // ngayCapBH
            // 
            this.ngayCapBH.HeaderText = "Ngày Cấp";
            this.ngayCapBH.MinimumWidth = 6;
            this.ngayCapBH.Name = "ngayCapBH";
            this.ngayCapBH.Width = 125;
            // 
            // NoiCapBH
            // 
            this.NoiCapBH.HeaderText = "Nơi Cấp";
            this.NoiCapBH.MinimumWidth = 6;
            this.NoiCapBH.Name = "NoiCapBH";
            this.NoiCapBH.Width = 125;
            // 
            // NoiKhamBenh
            // 
            this.NoiKhamBenh.HeaderText = "Nơi Khám Bệnh";
            this.NoiKhamBenh.MinimumWidth = 6;
            this.NoiKhamBenh.Name = "NoiKhamBenh";
            this.NoiKhamBenh.Width = 300;
            // 
            // UC_BaoHiemNV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.dgvBaoHiem);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UC_BaoHiemNV";
            this.Size = new System.Drawing.Size(1782, 812);
            this.Load += new System.EventHandler(this.UC_BaoHiemNV_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoHiem)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboChonNhanVien;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSoBH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpNgayCap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNoiKham;
        private System.Windows.Forms.TextBox txtNoiCap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvBaoHiem;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtIdBH;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboTKtenBH;
        private System.Windows.Forms.Button btnTkBH;
        private System.Windows.Forms.Button btnHienAllBH;
        private System.Windows.Forms.TextBox txtTimKiemBH;
        private System.Windows.Forms.TextBox txtTenNV;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdBH;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenNhanVien;
        private System.Windows.Forms.DataGridViewTextBoxColumn SoBaoHiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ngayCapBH;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoiCapBH;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoiKhamBenh;
    }
}
