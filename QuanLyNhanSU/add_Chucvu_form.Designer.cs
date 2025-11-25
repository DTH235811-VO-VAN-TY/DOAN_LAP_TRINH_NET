namespace QuanLyNhanSU
{
    partial class add_Chucvu_form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(add_Chucvu_form));
            this.button2 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvChucVu = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChucVu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHuyCV = new System.Windows.Forms.Button();
            this.btnXoaCV = new System.Windows.Forms.Button();
            this.btnSuuCV = new System.Windows.Forms.Button();
            this.btnThemCV = new System.Windows.Forms.Button();
            this.btnLuuCV = new System.Windows.Forms.Button();
            this.txtTenCV = new System.Windows.Forms.TextBox();
            this.txtIdCV = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChucVu)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(872, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 37);
            this.button2.TabIndex = 72;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(15, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(938, 10);
            this.panel5.TabIndex = 71;
            // 
            // dgvChucVu
            // 
            this.dgvChucVu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChucVu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.ChucVu});
            this.dgvChucVu.Location = new System.Drawing.Point(141, 356);
            this.dgvChucVu.Name = "dgvChucVu";
            this.dgvChucVu.RowHeadersWidth = 51;
            this.dgvChucVu.RowTemplate.Height = 24;
            this.dgvChucVu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChucVu.Size = new System.Drawing.Size(588, 150);
            this.dgvChucVu.TabIndex = 70;
            this.dgvChucVu.Click += new System.EventHandler(this.dgvChucVu_Click);
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.Width = 125;
            // 
            // ChucVu
            // 
            this.ChucVu.HeaderText = "Chức Vụ";
            this.ChucVu.MinimumWidth = 6;
            this.ChucVu.Name = "ChucVu";
            this.ChucVu.Width = 400;
            // 
            // btnHuyCV
            // 
            this.btnHuyCV.BackColor = System.Drawing.Color.Tomato;
            this.btnHuyCV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnHuyCV.ForeColor = System.Drawing.Color.White;
            this.btnHuyCV.Location = new System.Drawing.Point(704, 285);
            this.btnHuyCV.Name = "btnHuyCV";
            this.btnHuyCV.Size = new System.Drawing.Size(147, 42);
            this.btnHuyCV.TabIndex = 69;
            this.btnHuyCV.Text = "HỦY";
            this.btnHuyCV.UseVisualStyleBackColor = false;
            this.btnHuyCV.Click += new System.EventHandler(this.btnHuyCV_Click);
            // 
            // btnXoaCV
            // 
            this.btnXoaCV.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnXoaCV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXoaCV.ForeColor = System.Drawing.Color.White;
            this.btnXoaCV.Location = new System.Drawing.Point(551, 285);
            this.btnXoaCV.Name = "btnXoaCV";
            this.btnXoaCV.Size = new System.Drawing.Size(147, 42);
            this.btnXoaCV.TabIndex = 68;
            this.btnXoaCV.Text = "XÓA ";
            this.btnXoaCV.UseVisualStyleBackColor = false;
            this.btnXoaCV.Click += new System.EventHandler(this.btnXoaCV_Click);
            // 
            // btnSuuCV
            // 
            this.btnSuuCV.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnSuuCV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSuuCV.ForeColor = System.Drawing.Color.White;
            this.btnSuuCV.Location = new System.Drawing.Point(398, 285);
            this.btnSuuCV.Name = "btnSuuCV";
            this.btnSuuCV.Size = new System.Drawing.Size(147, 42);
            this.btnSuuCV.TabIndex = 67;
            this.btnSuuCV.Text = "SỬA ";
            this.btnSuuCV.UseVisualStyleBackColor = false;
            this.btnSuuCV.Click += new System.EventHandler(this.btnSuuCV_Click);
            // 
            // btnThemCV
            // 
            this.btnThemCV.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnThemCV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemCV.ForeColor = System.Drawing.Color.White;
            this.btnThemCV.Location = new System.Drawing.Point(245, 285);
            this.btnThemCV.Name = "btnThemCV";
            this.btnThemCV.Size = new System.Drawing.Size(147, 42);
            this.btnThemCV.TabIndex = 66;
            this.btnThemCV.Text = "THÊM";
            this.btnThemCV.UseVisualStyleBackColor = false;
            this.btnThemCV.Click += new System.EventHandler(this.btnThemCV_Click);
            // 
            // btnLuuCV
            // 
            this.btnLuuCV.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLuuCV.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLuuCV.ForeColor = System.Drawing.Color.White;
            this.btnLuuCV.Location = new System.Drawing.Point(92, 285);
            this.btnLuuCV.Name = "btnLuuCV";
            this.btnLuuCV.Size = new System.Drawing.Size(147, 42);
            this.btnLuuCV.TabIndex = 65;
            this.btnLuuCV.Text = "Lưu";
            this.btnLuuCV.UseVisualStyleBackColor = false;
            this.btnLuuCV.Click += new System.EventHandler(this.btnLuuCV_Click);
            // 
            // txtTenCV
            // 
            this.txtTenCV.Location = new System.Drawing.Point(278, 225);
            this.txtTenCV.Name = "txtTenCV";
            this.txtTenCV.Size = new System.Drawing.Size(376, 22);
            this.txtTenCV.TabIndex = 64;
            // 
            // txtIdCV
            // 
            this.txtIdCV.Location = new System.Drawing.Point(278, 176);
            this.txtIdCV.Name = "txtIdCV";
            this.txtIdCV.Size = new System.Drawing.Size(207, 22);
            this.txtIdCV.TabIndex = 63;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 62;
            this.label3.Text = "TÊN CHỨC VỤ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 16);
            this.label2.TabIndex = 61;
            this.label2.Text = "ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.label1.Location = new System.Drawing.Point(155, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(562, 37);
            this.label1.TabIndex = 60;
            this.label1.Text = "THÔNG TIN CHỨC VỤ NHÂN VIÊN";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(953, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(18, 561);
            this.panel4.TabIndex = 56;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(15, 561);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(956, 16);
            this.panel3.TabIndex = 57;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(15, 577);
            this.panel2.TabIndex = 58;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel1.Location = new System.Drawing.Point(-31, -13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 16);
            this.panel1.TabIndex = 59;
            // 
            // add_Chucvu_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 577);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.dgvChucVu);
            this.Controls.Add(this.btnHuyCV);
            this.Controls.Add(this.btnXoaCV);
            this.Controls.Add(this.btnSuuCV);
            this.Controls.Add(this.btnThemCV);
            this.Controls.Add(this.btnLuuCV);
            this.Controls.Add(this.txtTenCV);
            this.Controls.Add(this.txtIdCV);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "add_Chucvu_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "add_Chucvu_form";
            this.Load += new System.EventHandler(this.add_Chucvu_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChucVu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dgvChucVu;
        private System.Windows.Forms.Button btnHuyCV;
        private System.Windows.Forms.Button btnXoaCV;
        private System.Windows.Forms.Button btnSuuCV;
        private System.Windows.Forms.Button btnThemCV;
        private System.Windows.Forms.Button btnLuuCV;
        private System.Windows.Forms.TextBox txtTenCV;
        private System.Windows.Forms.TextBox txtIdCV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChucVu;
    }
}