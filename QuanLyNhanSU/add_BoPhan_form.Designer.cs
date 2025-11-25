namespace QuanLyNhanSU
{
    partial class add_BoPhan_form
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(add_BoPhan_form));
            this.button2 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvBoPhan = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TenPhongBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BoPhan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHuyBP = new System.Windows.Forms.Button();
            this.btnXoaBP = new System.Windows.Forms.Button();
            this.btnSuuBP = new System.Windows.Forms.Button();
            this.btnThemBP = new System.Windows.Forms.Button();
            this.btnLuuBP = new System.Windows.Forms.Button();
            this.txtTenBP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtIdBP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboPhongBan = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBoPhan)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(907, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 37);
            this.button2.TabIndex = 55;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(15, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(967, 10);
            this.panel5.TabIndex = 54;
            // 
            // dgvBoPhan
            // 
            this.dgvBoPhan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBoPhan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.TenPhongBan,
            this.BoPhan});
            this.dgvBoPhan.Location = new System.Drawing.Point(62, 379);
            this.dgvBoPhan.Name = "dgvBoPhan";
            this.dgvBoPhan.RowHeadersWidth = 51;
            this.dgvBoPhan.RowTemplate.Height = 24;
            this.dgvBoPhan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBoPhan.Size = new System.Drawing.Size(895, 150);
            this.dgvBoPhan.TabIndex = 53;
            this.dgvBoPhan.Click += new System.EventHandler(this.dgvBoPhan_Click);
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.Width = 125;
            // 
            // TenPhongBan
            // 
            this.TenPhongBan.HeaderText = "Phòng Ban";
            this.TenPhongBan.MinimumWidth = 6;
            this.TenPhongBan.Name = "TenPhongBan";
            this.TenPhongBan.Width = 350;
            // 
            // BoPhan
            // 
            this.BoPhan.HeaderText = "Bộ Phận";
            this.BoPhan.MinimumWidth = 6;
            this.BoPhan.Name = "BoPhan";
            this.BoPhan.Width = 400;
            // 
            // btnHuyBP
            // 
            this.btnHuyBP.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnHuyBP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnHuyBP.ForeColor = System.Drawing.Color.White;
            this.btnHuyBP.Location = new System.Drawing.Point(732, 317);
            this.btnHuyBP.Name = "btnHuyBP";
            this.btnHuyBP.Size = new System.Drawing.Size(147, 42);
            this.btnHuyBP.TabIndex = 52;
            this.btnHuyBP.Text = "HỦY";
            this.btnHuyBP.UseVisualStyleBackColor = false;
            this.btnHuyBP.Click += new System.EventHandler(this.btnHuyBP_Click);
            // 
            // btnXoaBP
            // 
            this.btnXoaBP.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnXoaBP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXoaBP.ForeColor = System.Drawing.Color.White;
            this.btnXoaBP.Location = new System.Drawing.Point(579, 317);
            this.btnXoaBP.Name = "btnXoaBP";
            this.btnXoaBP.Size = new System.Drawing.Size(147, 42);
            this.btnXoaBP.TabIndex = 51;
            this.btnXoaBP.Text = "XÓA ";
            this.btnXoaBP.UseVisualStyleBackColor = false;
            this.btnXoaBP.Click += new System.EventHandler(this.btnXoaBP_Click);
            // 
            // btnSuuBP
            // 
            this.btnSuuBP.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnSuuBP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSuuBP.ForeColor = System.Drawing.Color.White;
            this.btnSuuBP.Location = new System.Drawing.Point(426, 317);
            this.btnSuuBP.Name = "btnSuuBP";
            this.btnSuuBP.Size = new System.Drawing.Size(147, 42);
            this.btnSuuBP.TabIndex = 50;
            this.btnSuuBP.Text = "SỬA ";
            this.btnSuuBP.UseVisualStyleBackColor = false;
            this.btnSuuBP.Click += new System.EventHandler(this.btnSuuBP_Click);
            // 
            // btnThemBP
            // 
            this.btnThemBP.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnThemBP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemBP.ForeColor = System.Drawing.Color.White;
            this.btnThemBP.Location = new System.Drawing.Point(273, 317);
            this.btnThemBP.Name = "btnThemBP";
            this.btnThemBP.Size = new System.Drawing.Size(147, 42);
            this.btnThemBP.TabIndex = 49;
            this.btnThemBP.Text = "THÊM";
            this.btnThemBP.UseVisualStyleBackColor = false;
            this.btnThemBP.Click += new System.EventHandler(this.btnThemBP_Click);
            // 
            // btnLuuBP
            // 
            this.btnLuuBP.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLuuBP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLuuBP.ForeColor = System.Drawing.Color.White;
            this.btnLuuBP.Location = new System.Drawing.Point(120, 317);
            this.btnLuuBP.Name = "btnLuuBP";
            this.btnLuuBP.Size = new System.Drawing.Size(147, 42);
            this.btnLuuBP.TabIndex = 48;
            this.btnLuuBP.Text = "Lưu";
            this.btnLuuBP.UseVisualStyleBackColor = false;
            this.btnLuuBP.Click += new System.EventHandler(this.btnLuuBP_Click);
            // 
            // txtTenBP
            // 
            this.txtTenBP.Location = new System.Drawing.Point(306, 257);
            this.txtTenBP.Name = "txtTenBP";
            this.txtTenBP.Size = new System.Drawing.Size(376, 28);
            this.txtTenBP.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(116, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 22);
            this.label3.TabIndex = 45;
            this.label3.Text = "TÊN BỘ PHẬN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 22);
            this.label2.TabIndex = 44;
            this.label2.Text = "ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.label1.Location = new System.Drawing.Point(180, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(557, 37);
            this.label1.TabIndex = 43;
            this.label1.Text = "THÔNG TIN BỘ PHÂN NHÂN VIÊN";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(982, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(18, 625);
            this.panel4.TabIndex = 39;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(15, 625);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(985, 16);
            this.panel3.TabIndex = 40;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(15, 641);
            this.panel2.TabIndex = 41;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel1.Location = new System.Drawing.Point(-6, -26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 16);
            this.panel1.TabIndex = 42;
            // 
            // txtIdBP
            // 
            this.txtIdBP.Location = new System.Drawing.Point(303, 163);
            this.txtIdBP.Name = "txtIdBP";
            this.txtIdBP.Size = new System.Drawing.Size(207, 28);
            this.txtIdBP.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(116, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 22);
            this.label4.TabIndex = 45;
            this.label4.Text = "TÊN PHÒNG BAN";
            // 
            // cboPhongBan
            // 
            this.cboPhongBan.FormattingEnabled = true;
            this.cboPhongBan.Location = new System.Drawing.Point(306, 217);
            this.cboPhongBan.Name = "cboPhongBan";
            this.cboPhongBan.Size = new System.Drawing.Size(376, 30);
            this.cboPhongBan.TabIndex = 56;
            // 
            // add_BoPhan_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 641);
            this.Controls.Add(this.cboPhongBan);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.dgvBoPhan);
            this.Controls.Add(this.btnHuyBP);
            this.Controls.Add(this.btnXoaBP);
            this.Controls.Add(this.btnSuuBP);
            this.Controls.Add(this.btnThemBP);
            this.Controls.Add(this.btnLuuBP);
            this.Controls.Add(this.txtTenBP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtIdBP);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "add_BoPhan_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "add_BoPhan_form";
            this.Load += new System.EventHandler(this.add_BoPhan_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBoPhan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dgvBoPhan;
        private System.Windows.Forms.Button btnHuyBP;
        private System.Windows.Forms.Button btnXoaBP;
        private System.Windows.Forms.Button btnSuuBP;
        private System.Windows.Forms.Button btnThemBP;
        private System.Windows.Forms.Button btnLuuBP;
        private System.Windows.Forms.TextBox txtTenBP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtIdBP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboPhongBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenPhongBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn BoPhan;
    }
}