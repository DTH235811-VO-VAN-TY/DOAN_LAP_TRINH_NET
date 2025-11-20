namespace QuanLyNhanSU
{
    partial class add_ThemPhuCap_form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(add_ThemPhuCap_form));
            this.btnQuayLai = new System.Windows.Forms.Button();
            this.txtSoTien = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvPhuCapMoi = new System.Windows.Forms.DataGridView();
            this.Idphucap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TenPhuCap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sotienphucap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHuyPC = new System.Windows.Forms.Button();
            this.btnXoaPC = new System.Windows.Forms.Button();
            this.btnSuuPC = new System.Windows.Forms.Button();
            this.btnThemPC = new System.Windows.Forms.Button();
            this.btnLuuPC = new System.Windows.Forms.Button();
            this.txtTenPCMoi = new System.Windows.Forms.TextBox();
            this.txtIdPC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhuCapMoi)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuayLai
            // 
            this.btnQuayLai.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnQuayLai.Location = new System.Drawing.Point(679, 541);
            this.btnQuayLai.Name = "btnQuayLai";
            this.btnQuayLai.Size = new System.Drawing.Size(177, 50);
            this.btnQuayLai.TabIndex = 68;
            this.btnQuayLai.Text = "Quay Lại";
            this.btnQuayLai.UseVisualStyleBackColor = true;
            // 
            // txtSoTien
            // 
            this.txtSoTien.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtSoTien.Location = new System.Drawing.Point(305, 260);
            this.txtSoTien.Name = "txtSoTien";
            this.txtSoTien.Size = new System.Drawing.Size(376, 30);
            this.txtSoTien.TabIndex = 67;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.Location = new System.Drawing.Point(115, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 25);
            this.label4.TabIndex = 66;
            this.label4.Text = "Số Tiền Phụ Cấp";
            // 
            // dgvPhuCapMoi
            // 
            this.dgvPhuCapMoi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhuCapMoi.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Idphucap,
            this.TenPhuCap,
            this.Sotienphucap});
            this.dgvPhuCapMoi.Location = new System.Drawing.Point(76, 370);
            this.dgvPhuCapMoi.Name = "dgvPhuCapMoi";
            this.dgvPhuCapMoi.RowHeadersWidth = 51;
            this.dgvPhuCapMoi.RowTemplate.Height = 24;
            this.dgvPhuCapMoi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhuCapMoi.Size = new System.Drawing.Size(780, 150);
            this.dgvPhuCapMoi.TabIndex = 65;
            // 
            // Idphucap
            // 
            this.Idphucap.HeaderText = "ID Phụ Cấp";
            this.Idphucap.MinimumWidth = 6;
            this.Idphucap.Name = "Idphucap";
            this.Idphucap.Width = 125;
            // 
            // TenPhuCap
            // 
            this.TenPhuCap.HeaderText = "Tên Phụ Cấp";
            this.TenPhuCap.MinimumWidth = 6;
            this.TenPhuCap.Name = "TenPhuCap";
            this.TenPhuCap.Width = 300;
            // 
            // Sotienphucap
            // 
            this.Sotienphucap.HeaderText = "Số Tiền Phụ Cấp";
            this.Sotienphucap.MinimumWidth = 6;
            this.Sotienphucap.Name = "Sotienphucap";
            this.Sotienphucap.Width = 300;
            // 
            // btnHuyPC
            // 
            this.btnHuyPC.BackColor = System.Drawing.Color.Tomato;
            this.btnHuyPC.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnHuyPC.ForeColor = System.Drawing.Color.White;
            this.btnHuyPC.Location = new System.Drawing.Point(688, 306);
            this.btnHuyPC.Name = "btnHuyPC";
            this.btnHuyPC.Size = new System.Drawing.Size(147, 42);
            this.btnHuyPC.TabIndex = 64;
            this.btnHuyPC.Text = "HỦY";
            this.btnHuyPC.UseVisualStyleBackColor = false;
            // 
            // btnXoaPC
            // 
            this.btnXoaPC.BackColor = System.Drawing.Color.Red;
            this.btnXoaPC.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXoaPC.ForeColor = System.Drawing.Color.White;
            this.btnXoaPC.Location = new System.Drawing.Point(535, 306);
            this.btnXoaPC.Name = "btnXoaPC";
            this.btnXoaPC.Size = new System.Drawing.Size(147, 42);
            this.btnXoaPC.TabIndex = 63;
            this.btnXoaPC.Text = "XÓA ";
            this.btnXoaPC.UseVisualStyleBackColor = false;
            // 
            // btnSuuPC
            // 
            this.btnSuuPC.BackColor = System.Drawing.Color.Yellow;
            this.btnSuuPC.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSuuPC.ForeColor = System.Drawing.Color.White;
            this.btnSuuPC.Location = new System.Drawing.Point(382, 306);
            this.btnSuuPC.Name = "btnSuuPC";
            this.btnSuuPC.Size = new System.Drawing.Size(147, 42);
            this.btnSuuPC.TabIndex = 62;
            this.btnSuuPC.Text = "SỬA ";
            this.btnSuuPC.UseVisualStyleBackColor = false;
            // 
            // btnThemPC
            // 
            this.btnThemPC.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnThemPC.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemPC.ForeColor = System.Drawing.Color.White;
            this.btnThemPC.Location = new System.Drawing.Point(229, 306);
            this.btnThemPC.Name = "btnThemPC";
            this.btnThemPC.Size = new System.Drawing.Size(147, 42);
            this.btnThemPC.TabIndex = 61;
            this.btnThemPC.Text = "THÊM";
            this.btnThemPC.UseVisualStyleBackColor = false;
            // 
            // btnLuuPC
            // 
            this.btnLuuPC.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnLuuPC.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLuuPC.ForeColor = System.Drawing.Color.White;
            this.btnLuuPC.Location = new System.Drawing.Point(76, 306);
            this.btnLuuPC.Name = "btnLuuPC";
            this.btnLuuPC.Size = new System.Drawing.Size(147, 42);
            this.btnLuuPC.TabIndex = 60;
            this.btnLuuPC.Text = "Lưu";
            this.btnLuuPC.UseVisualStyleBackColor = false;
            // 
            // txtTenPCMoi
            // 
            this.txtTenPCMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTenPCMoi.Location = new System.Drawing.Point(305, 195);
            this.txtTenPCMoi.Name = "txtTenPCMoi";
            this.txtTenPCMoi.Size = new System.Drawing.Size(376, 30);
            this.txtTenPCMoi.TabIndex = 59;
            // 
            // txtIdPC
            // 
            this.txtIdPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtIdPC.Location = new System.Drawing.Point(305, 146);
            this.txtIdPC.Name = "txtIdPC";
            this.txtIdPC.Size = new System.Drawing.Size(207, 30);
            this.txtIdPC.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(115, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 25);
            this.label3.TabIndex = 57;
            this.label3.Text = "Tên Phụ Cấp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(115, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 25);
            this.label2.TabIndex = 56;
            this.label2.Text = "ID Phụ Cấp";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.label1.Location = new System.Drawing.Point(209, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(519, 37);
            this.label1.TabIndex = 55;
            this.label1.Text = "THÊM PHỤ LOẠI PHỤ CẤP MỚI";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(17, 628);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(924, 20);
            this.panel2.TabIndex = 53;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(941, 21);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(18, 627);
            this.panel3.TabIndex = 52;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 21);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(17, 627);
            this.panel4.TabIndex = 54;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(959, 21);
            this.panel1.TabIndex = 51;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(866, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 37);
            this.button2.TabIndex = 69;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // add_ThemPhuCap_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 648);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnQuayLai);
            this.Controls.Add(this.txtSoTien);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvPhuCapMoi);
            this.Controls.Add(this.btnHuyPC);
            this.Controls.Add(this.btnXoaPC);
            this.Controls.Add(this.btnSuuPC);
            this.Controls.Add(this.btnThemPC);
            this.Controls.Add(this.btnLuuPC);
            this.Controls.Add(this.txtTenPCMoi);
            this.Controls.Add(this.txtIdPC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "add_ThemPhuCap_form";
            this.Text = "add_ThemPhuCap_form";
            this.Load += new System.EventHandler(this.add_ThemPhuCap_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhuCapMoi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQuayLai;
        private System.Windows.Forms.TextBox txtSoTien;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvPhuCapMoi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Idphucap;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenPhuCap;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sotienphucap;
        private System.Windows.Forms.Button btnHuyPC;
        private System.Windows.Forms.Button btnXoaPC;
        private System.Windows.Forms.Button btnSuuPC;
        private System.Windows.Forms.Button btnThemPC;
        private System.Windows.Forms.Button btnLuuPC;
        private System.Windows.Forms.TextBox txtTenPCMoi;
        private System.Windows.Forms.TextBox txtIdPC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
    }
}