namespace QuanLyNhanSU
{
    partial class add_PhongBan_from
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(add_PhongBan_from));
            this.dgvPhongBan = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhongBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnXoaPB = new System.Windows.Forms.Button();
            this.btnSuuPB = new System.Windows.Forms.Button();
            this.btnThemPB = new System.Windows.Forms.Button();
            this.btnLuuPB = new System.Windows.Forms.Button();
            this.txtTenPB = new System.Windows.Forms.TextBox();
            this.txtIdPB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhongBan)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPhongBan
            // 
            this.dgvPhongBan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhongBan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.PhongBan});
            this.dgvPhongBan.Location = new System.Drawing.Point(128, 310);
            this.dgvPhongBan.Name = "dgvPhongBan";
            this.dgvPhongBan.RowHeadersWidth = 51;
            this.dgvPhongBan.RowTemplate.Height = 24;
            this.dgvPhongBan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhongBan.Size = new System.Drawing.Size(588, 150);
            this.dgvPhongBan.TabIndex = 36;
            this.dgvPhongBan.Click += new System.EventHandler(this.dgvPhongBan_Click);
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.Width = 125;
            // 
            // PhongBan
            // 
            this.PhongBan.HeaderText = "Phòng Ban";
            this.PhongBan.MinimumWidth = 6;
            this.PhongBan.Name = "PhongBan";
            this.PhongBan.Width = 400;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.Tomato;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(722, 246);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(147, 42);
            this.btnHuy.TabIndex = 35;
            this.btnHuy.Text = "HỦY";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnXoaPB
            // 
            this.btnXoaPB.BackColor = System.Drawing.Color.Red;
            this.btnXoaPB.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXoaPB.ForeColor = System.Drawing.Color.White;
            this.btnXoaPB.Location = new System.Drawing.Point(569, 246);
            this.btnXoaPB.Name = "btnXoaPB";
            this.btnXoaPB.Size = new System.Drawing.Size(147, 42);
            this.btnXoaPB.TabIndex = 34;
            this.btnXoaPB.Text = "XÓA ";
            this.btnXoaPB.UseVisualStyleBackColor = false;
            this.btnXoaPB.Click += new System.EventHandler(this.btnXoaPB_Click);
            // 
            // btnSuuPB
            // 
            this.btnSuuPB.BackColor = System.Drawing.Color.Yellow;
            this.btnSuuPB.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSuuPB.ForeColor = System.Drawing.Color.White;
            this.btnSuuPB.Location = new System.Drawing.Point(416, 246);
            this.btnSuuPB.Name = "btnSuuPB";
            this.btnSuuPB.Size = new System.Drawing.Size(147, 42);
            this.btnSuuPB.TabIndex = 33;
            this.btnSuuPB.Text = "SỬA ";
            this.btnSuuPB.UseVisualStyleBackColor = false;
            this.btnSuuPB.Click += new System.EventHandler(this.btnSuuPB_Click);
            // 
            // btnThemPB
            // 
            this.btnThemPB.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnThemPB.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemPB.ForeColor = System.Drawing.Color.White;
            this.btnThemPB.Location = new System.Drawing.Point(263, 246);
            this.btnThemPB.Name = "btnThemPB";
            this.btnThemPB.Size = new System.Drawing.Size(147, 42);
            this.btnThemPB.TabIndex = 32;
            this.btnThemPB.Text = "THÊM";
            this.btnThemPB.UseVisualStyleBackColor = false;
            this.btnThemPB.Click += new System.EventHandler(this.btnThemPB_Click);
            // 
            // btnLuuPB
            // 
            this.btnLuuPB.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnLuuPB.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLuuPB.ForeColor = System.Drawing.Color.White;
            this.btnLuuPB.Location = new System.Drawing.Point(110, 246);
            this.btnLuuPB.Name = "btnLuuPB";
            this.btnLuuPB.Size = new System.Drawing.Size(147, 42);
            this.btnLuuPB.TabIndex = 31;
            this.btnLuuPB.Text = "Lưu";
            this.btnLuuPB.UseVisualStyleBackColor = false;
            this.btnLuuPB.Click += new System.EventHandler(this.btnLuuPB_Click);
            // 
            // txtTenPB
            // 
            this.txtTenPB.Location = new System.Drawing.Point(296, 186);
            this.txtTenPB.Name = "txtTenPB";
            this.txtTenPB.Size = new System.Drawing.Size(376, 28);
            this.txtTenPB.TabIndex = 30;
            // 
            // txtIdPB
            // 
            this.txtIdPB.Location = new System.Drawing.Point(296, 137);
            this.txtIdPB.Name = "txtIdPB";
            this.txtIdPB.Size = new System.Drawing.Size(207, 28);
            this.txtIdPB.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 20);
            this.label3.TabIndex = 28;
            this.label3.Text = "TÊN PHÒNG BAN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 27;
            this.label2.Text = "ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.label1.Location = new System.Drawing.Point(173, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(606, 37);
            this.label1.TabIndex = 26;
            this.label1.Text = "THÔNG TIN PHÒNG BAN NHÂN VIÊN";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(982, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(18, 625);
            this.panel4.TabIndex = 22;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(15, 625);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(985, 16);
            this.panel3.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(15, 641);
            this.panel2.TabIndex = 24;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel1.Location = new System.Drawing.Point(-13, -52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 16);
            this.panel1.TabIndex = 25;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(15, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(967, 10);
            this.panel5.TabIndex = 37;
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
            this.button2.TabIndex = 38;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // add_PhongBan_from
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 641);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.dgvPhongBan);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnXoaPB);
            this.Controls.Add(this.btnSuuPB);
            this.Controls.Add(this.btnThemPB);
            this.Controls.Add(this.btnLuuPB);
            this.Controls.Add(this.txtTenPB);
            this.Controls.Add(this.txtIdPB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "add_PhongBan_from";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "add_PhongBan_from";
            this.Load += new System.EventHandler(this.add_PhongBan_from_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhongBan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPhongBan;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnXoaPB;
        private System.Windows.Forms.Button btnSuuPB;
        private System.Windows.Forms.Button btnThemPB;
        private System.Windows.Forms.Button btnLuuPB;
        private System.Windows.Forms.TextBox txtTenPB;
        private System.Windows.Forms.TextBox txtIdPB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhongBan;
    }
}