namespace QuanLyNhanSU
{
    partial class add_TrinhDo_form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(add_TrinhDo_form));
            this.button2 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvTrinhDo = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrinhDo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHuyTD = new System.Windows.Forms.Button();
            this.btnXoaTD = new System.Windows.Forms.Button();
            this.btnSuuTD = new System.Windows.Forms.Button();
            this.btnThemTD = new System.Windows.Forms.Button();
            this.btnLuuTD = new System.Windows.Forms.Button();
            this.txtTenTD = new System.Windows.Forms.TextBox();
            this.txtIdTD = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrinhDo)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(889, 12);
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
            this.panel5.Size = new System.Drawing.Size(949, 10);
            this.panel5.TabIndex = 54;
            // 
            // dgvTrinhDo
            // 
            this.dgvTrinhDo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrinhDo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.TrinhDo});
            this.dgvTrinhDo.Location = new System.Drawing.Point(119, 344);
            this.dgvTrinhDo.Name = "dgvTrinhDo";
            this.dgvTrinhDo.RowHeadersWidth = 51;
            this.dgvTrinhDo.RowTemplate.Height = 24;
            this.dgvTrinhDo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrinhDo.Size = new System.Drawing.Size(588, 150);
            this.dgvTrinhDo.TabIndex = 53;
            this.dgvTrinhDo.Click += new System.EventHandler(this.dgvTrinhDo_Click);
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.Width = 125;
            // 
            // TrinhDo
            // 
            this.TrinhDo.HeaderText = "Trình Độ";
            this.TrinhDo.MinimumWidth = 6;
            this.TrinhDo.Name = "TrinhDo";
            this.TrinhDo.Width = 400;
            // 
            // btnHuyTD
            // 
            this.btnHuyTD.BackColor = System.Drawing.Color.Tomato;
            this.btnHuyTD.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnHuyTD.ForeColor = System.Drawing.Color.White;
            this.btnHuyTD.Location = new System.Drawing.Point(713, 272);
            this.btnHuyTD.Name = "btnHuyTD";
            this.btnHuyTD.Size = new System.Drawing.Size(147, 42);
            this.btnHuyTD.TabIndex = 52;
            this.btnHuyTD.Text = "HỦY";
            this.btnHuyTD.UseVisualStyleBackColor = false;
            this.btnHuyTD.Click += new System.EventHandler(this.btnHuyTD_Click);
            // 
            // btnXoaTD
            // 
            this.btnXoaTD.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnXoaTD.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXoaTD.ForeColor = System.Drawing.Color.White;
            this.btnXoaTD.Location = new System.Drawing.Point(560, 272);
            this.btnXoaTD.Name = "btnXoaTD";
            this.btnXoaTD.Size = new System.Drawing.Size(147, 42);
            this.btnXoaTD.TabIndex = 51;
            this.btnXoaTD.Text = "XÓA ";
            this.btnXoaTD.UseVisualStyleBackColor = false;
            this.btnXoaTD.Click += new System.EventHandler(this.btnXoaTD_Click);
            // 
            // btnSuuTD
            // 
            this.btnSuuTD.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnSuuTD.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSuuTD.ForeColor = System.Drawing.Color.White;
            this.btnSuuTD.Location = new System.Drawing.Point(407, 272);
            this.btnSuuTD.Name = "btnSuuTD";
            this.btnSuuTD.Size = new System.Drawing.Size(147, 42);
            this.btnSuuTD.TabIndex = 50;
            this.btnSuuTD.Text = "SỬA ";
            this.btnSuuTD.UseVisualStyleBackColor = false;
            this.btnSuuTD.Click += new System.EventHandler(this.btnSuuTD_Click);
            // 
            // btnThemTD
            // 
            this.btnThemTD.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnThemTD.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemTD.ForeColor = System.Drawing.Color.White;
            this.btnThemTD.Location = new System.Drawing.Point(254, 272);
            this.btnThemTD.Name = "btnThemTD";
            this.btnThemTD.Size = new System.Drawing.Size(147, 42);
            this.btnThemTD.TabIndex = 49;
            this.btnThemTD.Text = "THÊM";
            this.btnThemTD.UseVisualStyleBackColor = false;
            this.btnThemTD.Click += new System.EventHandler(this.btnThemTD_Click);
            // 
            // btnLuuTD
            // 
            this.btnLuuTD.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnLuuTD.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnLuuTD.ForeColor = System.Drawing.Color.White;
            this.btnLuuTD.Location = new System.Drawing.Point(101, 272);
            this.btnLuuTD.Name = "btnLuuTD";
            this.btnLuuTD.Size = new System.Drawing.Size(147, 42);
            this.btnLuuTD.TabIndex = 48;
            this.btnLuuTD.Text = "Lưu";
            this.btnLuuTD.UseVisualStyleBackColor = false;
            this.btnLuuTD.Click += new System.EventHandler(this.btnLuuTD_Click);
            // 
            // txtTenTD
            // 
            this.txtTenTD.Location = new System.Drawing.Point(287, 212);
            this.txtTenTD.Name = "txtTenTD";
            this.txtTenTD.Size = new System.Drawing.Size(376, 22);
            this.txtTenTD.TabIndex = 47;
            // 
            // txtIdTD
            // 
            this.txtIdTD.Location = new System.Drawing.Point(287, 163);
            this.txtIdTD.Name = "txtIdTD";
            this.txtIdTD.Size = new System.Drawing.Size(207, 22);
            this.txtIdTD.TabIndex = 46;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 16);
            this.label3.TabIndex = 45;
            this.label3.Text = "TÊN TRÌNH ĐỘ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 16);
            this.label2.TabIndex = 44;
            this.label2.Text = "ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.label1.Location = new System.Drawing.Point(164, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(574, 37);
            this.label1.TabIndex = 43;
            this.label1.Text = "THÔNG TIN TRÌNH ĐỘ NHÂN VIÊN";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(964, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(18, 578);
            this.panel4.TabIndex = 39;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(15, 578);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(967, 16);
            this.panel3.TabIndex = 40;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(15, 594);
            this.panel2.TabIndex = 41;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel1.Location = new System.Drawing.Point(-22, -26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 16);
            this.panel1.TabIndex = 42;
            // 
            // add_TrinhDo_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 594);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.dgvTrinhDo);
            this.Controls.Add(this.btnHuyTD);
            this.Controls.Add(this.btnXoaTD);
            this.Controls.Add(this.btnSuuTD);
            this.Controls.Add(this.btnThemTD);
            this.Controls.Add(this.btnLuuTD);
            this.Controls.Add(this.txtTenTD);
            this.Controls.Add(this.txtIdTD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "add_TrinhDo_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "add_TrinhDo_form";
            this.Load += new System.EventHandler(this.add_TrinhDo_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrinhDo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dgvTrinhDo;
        private System.Windows.Forms.Button btnHuyTD;
        private System.Windows.Forms.Button btnXoaTD;
        private System.Windows.Forms.Button btnSuuTD;
        private System.Windows.Forms.Button btnThemTD;
        private System.Windows.Forms.Button btnLuuTD;
        private System.Windows.Forms.TextBox txtTenTD;
        private System.Windows.Forms.TextBox txtIdTD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrinhDo;
    }
}