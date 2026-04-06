namespace MilkTeaPOS
{
    partial class frmVouchers
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
            pbx1 = new PictureBox();
            lbl1 = new Label();
            lbl2 = new Label();
            lbl3 = new Label();
            lbl4_hieuung = new Label();
            lbl4 = new Label();
            groupBox1 = new GroupBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            button2 = new Button();
            txtSearch = new TextBox();
            groupBox2 = new GroupBox();
            cbx1_tatcama = new ComboBox();
            groupBox3 = new GroupBox();
            btt_xoa = new Button();
            btt_sua = new Button();
            btt_lammoi = new Button();
            btt_them = new Button();
            grpDanhSach = new GroupBox();
            dgvVouchers = new DataGridView();
            grpNhapLieu = new GroupBox();
            textBox3 = new TextBox();
            label16 = new Label();
            txtMaxDiscount = new TextBox();
            label15 = new Label();
            label14 = new Label();
            cbxVoucherType = new ComboBox();
            label13 = new Label();
            dtp_batdau = new DateTimePicker();
            dtp_handung = new DateTimePicker();
            txtMinOrder = new TextBox();
            textBox5 = new TextBox();
            textBox4 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label12 = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)pbx1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            grpDanhSach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVouchers).BeginInit();
            grpNhapLieu.SuspendLayout();
            SuspendLayout();
            // 
            // pbx1
            // 
            pbx1.Dock = DockStyle.Top;
            pbx1.Image = Properties.Resources.la_ca_tra_sua_30k_1200x628;
            pbx1.Location = new Point(0, 0);
            pbx1.Name = "pbx1";
            pbx1.Size = new Size(1491, 276);
            pbx1.SizeMode = PictureBoxSizeMode.StretchImage;
            pbx1.TabIndex = 0;
            pbx1.TabStop = false;
            // 
            // lbl1
            // 
            lbl1.BackColor = SystemColors.ControlLight;
            lbl1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl1.Location = new Point(97, 38);
            lbl1.Name = "lbl1";
            lbl1.Size = new Size(189, 107);
            lbl1.TabIndex = 1;
            lbl1.Text = "Tong Vouchers";
            lbl1.TextAlign = ContentAlignment.TopCenter;
            lbl1.Click += lbl1_Click;
            // 
            // lbl2
            // 
            lbl2.BackColor = SystemColors.ControlLight;
            lbl2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl2.Location = new Point(440, 38);
            lbl2.Name = "lbl2";
            lbl2.Size = new Size(189, 107);
            lbl2.TabIndex = 3;
            lbl2.Text = "Đang Hoạt Động";
            lbl2.TextAlign = ContentAlignment.TopCenter;
            lbl2.Click += label2_Click;
            // 
            // lbl3
            // 
            lbl3.BackColor = SystemColors.ControlLight;
            lbl3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl3.Location = new Point(794, 38);
            lbl3.Name = "lbl3";
            lbl3.Size = new Size(189, 107);
            lbl3.TabIndex = 5;
            lbl3.Text = "Hết Hạn";
            lbl3.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbl4_hieuung
            // 
            lbl4_hieuung.BackColor = SystemColors.ActiveCaption;
            lbl4_hieuung.Location = new Point(1117, 38);
            lbl4_hieuung.Name = "lbl4_hieuung";
            lbl4_hieuung.Size = new Size(15, 107);
            lbl4_hieuung.TabIndex = 8;
            // 
            // lbl4
            // 
            lbl4.BackColor = SystemColors.ControlLight;
            lbl4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl4.Location = new Point(1127, 38);
            lbl4.Name = "lbl4";
            lbl4.Size = new Size(189, 107);
            lbl4.TabIndex = 7;
            lbl4.Text = "Đã Dùng Hết Hạn";
            lbl4.TextAlign = ContentAlignment.TopCenter;
            lbl4.Click += lbl4_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(lbl2);
            groupBox1.Controls.Add(lbl1);
            groupBox1.Controls.Add(lbl4_hieuung);
            groupBox1.Controls.Add(lbl3);
            groupBox1.Controls.Add(lbl4);
            groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(18, 282);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1358, 185);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thống Kê Voucher";
            // 
            // label4
            // 
            label4.BackColor = SystemColors.ActiveCaption;
            label4.Location = new Point(97, 38);
            label4.Name = "label4";
            label4.Size = new Size(15, 107);
            label4.TabIndex = 11;
            // 
            // label3
            // 
            label3.BackColor = SystemColors.ActiveCaption;
            label3.Location = new Point(426, 38);
            label3.Name = "label3";
            label3.Size = new Size(15, 107);
            label3.TabIndex = 10;
            // 
            // label2
            // 
            label2.BackColor = SystemColors.ActiveCaption;
            label2.Location = new Point(790, 38);
            label2.Name = "label2";
            label2.Size = new Size(15, 107);
            label2.TabIndex = 9;
            // 
            // button2
            // 
            button2.BackColor = Color.DarkSalmon;
            button2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.Location = new Point(483, 42);
            button2.Name = "button2";
            button2.Size = new Size(138, 51);
            button2.TabIndex = 17;
            button2.Text = "Tìm";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Location = new Point(6, 42);
            txtSearch.Multiline = true;
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(441, 51);
            txtSearch.TabIndex = 16;
            txtSearch.Text = "Tìm theo mã...";
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyPress += txtSearch_KeyPress;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtSearch);
            groupBox2.Controls.Add(button2);
            groupBox2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(18, 473);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1358, 125);
            groupBox2.TabIndex = 19;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tìm Kiếm";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // cbx1_tatcama
            // 
            cbx1_tatcama.FormattingEnabled = true;
            cbx1_tatcama.Items.AddRange(new object[] { "active", "", "inactive", "", "expired", "", "used_up" });
            cbx1_tatcama.Location = new Point(549, 172);
            cbx1_tatcama.Name = "cbx1_tatcama";
            cbx1_tatcama.Size = new Size(112, 36);
            cbx1_tatcama.TabIndex = 20;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btt_xoa);
            groupBox3.Controls.Add(btt_sua);
            groupBox3.Controls.Add(btt_lammoi);
            groupBox3.Controls.Add(btt_them);
            groupBox3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox3.Location = new Point(24, 604);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1365, 87);
            groupBox3.TabIndex = 20;
            groupBox3.TabStop = false;
            groupBox3.Text = "Thao Tác";
            // 
            // btt_xoa
            // 
            btt_xoa.BackColor = Color.Red;
            btt_xoa.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btt_xoa.ForeColor = Color.White;
            btt_xoa.Location = new Point(420, 32);
            btt_xoa.Name = "btt_xoa";
            btt_xoa.Size = new Size(94, 41);
            btt_xoa.TabIndex = 4;
            btt_xoa.Text = "Xóa";
            btt_xoa.UseVisualStyleBackColor = false;
            btt_xoa.Click += btt_xoa_Click;
            // 
            // btt_sua
            // 
            btt_sua.BackColor = Color.Gold;
            btt_sua.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btt_sua.ForeColor = Color.White;
            btt_sua.Location = new Point(280, 32);
            btt_sua.Name = "btt_sua";
            btt_sua.Size = new Size(94, 42);
            btt_sua.TabIndex = 2;
            btt_sua.Text = "Sửa";
            btt_sua.UseVisualStyleBackColor = false;
            btt_sua.Click += btt_sua_Click;
            // 
            // btt_lammoi
            // 
            btt_lammoi.BackColor = Color.FromArgb(0, 0, 192);
            btt_lammoi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btt_lammoi.ForeColor = Color.White;
            btt_lammoi.Location = new Point(561, 33);
            btt_lammoi.Name = "btt_lammoi";
            btt_lammoi.Size = new Size(105, 41);
            btt_lammoi.TabIndex = 3;
            btt_lammoi.Text = "Làm Mới";
            btt_lammoi.UseVisualStyleBackColor = false;
            btt_lammoi.Click += btt_lammoi_Click;
            // 
            // btt_them
            // 
            btt_them.BackColor = Color.FromArgb(128, 255, 128);
            btt_them.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btt_them.ForeColor = Color.White;
            btt_them.Location = new Point(145, 33);
            btt_them.Name = "btt_them";
            btt_them.Size = new Size(94, 41);
            btt_them.TabIndex = 1;
            btt_them.Text = "Thêm";
            btt_them.UseVisualStyleBackColor = false;
            btt_them.Click += btt_them_Click;
            // 
            // grpDanhSach
            // 
            grpDanhSach.Controls.Add(dgvVouchers);
            grpDanhSach.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpDanhSach.Location = new Point(24, 707);
            grpDanhSach.Name = "grpDanhSach";
            grpDanhSach.Size = new Size(1365, 309);
            grpDanhSach.TabIndex = 21;
            grpDanhSach.TabStop = false;
            grpDanhSach.Text = "Danh Sách Vouchers";
            // 
            // dgvVouchers
            // 
            dgvVouchers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVouchers.Location = new Point(6, 33);
            dgvVouchers.Name = "dgvVouchers";
            dgvVouchers.RowHeadersWidth = 51;
            dgvVouchers.Size = new Size(1384, 266);
            dgvVouchers.TabIndex = 0;
            dgvVouchers.CellClick += dgvVouchers_CellClick;
            dgvVouchers.CellContentClick += dgv_vouchers_CellContentClick;
            dgvVouchers.CellDoubleClick += dgvVouchers_CellDoubleClick;
            // 
            // grpNhapLieu
            // 
            grpNhapLieu.Controls.Add(textBox3);
            grpNhapLieu.Controls.Add(label16);
            grpNhapLieu.Controls.Add(txtMaxDiscount);
            grpNhapLieu.Controls.Add(label15);
            grpNhapLieu.Controls.Add(label14);
            grpNhapLieu.Controls.Add(cbxVoucherType);
            grpNhapLieu.Controls.Add(label13);
            grpNhapLieu.Controls.Add(dtp_batdau);
            grpNhapLieu.Controls.Add(cbx1_tatcama);
            grpNhapLieu.Controls.Add(dtp_handung);
            grpNhapLieu.Controls.Add(txtMinOrder);
            grpNhapLieu.Controls.Add(textBox5);
            grpNhapLieu.Controls.Add(textBox4);
            grpNhapLieu.Controls.Add(textBox2);
            grpNhapLieu.Controls.Add(textBox1);
            grpNhapLieu.Controls.Add(label12);
            grpNhapLieu.Controls.Add(label11);
            grpNhapLieu.Controls.Add(label10);
            grpNhapLieu.Controls.Add(label9);
            grpNhapLieu.Controls.Add(label8);
            grpNhapLieu.Controls.Add(label7);
            grpNhapLieu.Controls.Add(label6);
            grpNhapLieu.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpNhapLieu.Location = new Point(30, 1022);
            grpNhapLieu.Name = "grpNhapLieu";
            grpNhapLieu.Size = new Size(1423, 270);
            grpNhapLieu.TabIndex = 22;
            grpNhapLieu.TabStop = false;
            grpNhapLieu.Text = "Form Nhập Liệu";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(185, 230);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(323, 34);
            textBox3.TabIndex = 28;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(6, 233);
            label16.Name = "label16";
            label16.Size = new Size(72, 28);
            label16.TabIndex = 27;
            label16.Text = "Mô Tả";
            label16.Click += label16_Click;
            // 
            // txtMaxDiscount
            // 
            txtMaxDiscount.Location = new Point(549, 127);
            txtMaxDiscount.Name = "txtMaxDiscount";
            txtMaxDiscount.Size = new Size(171, 34);
            txtMaxDiscount.TabIndex = 26;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(396, 127);
            label15.Name = "label15";
            label15.Size = new Size(129, 28);
            label15.TabIndex = 25;
            label15.Text = "Giảm Tối Đa";
            label15.Click += label15_Click;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(396, 70);
            label14.Name = "label14";
            label14.Size = new Size(147, 28);
            label14.TabIndex = 24;
            label14.Text = "Đơn Tối Thiếu";
            // 
            // cbxVoucherType
            // 
            cbxVoucherType.FormattingEnabled = true;
            cbxVoucherType.Items.AddRange(new object[] { "percentage", "", "fixed_amount", "", "free_item", "", "buy_one_get_one" });
            cbxVoucherType.Location = new Point(185, 172);
            cbxVoucherType.Name = "cbxVoucherType";
            cbxVoucherType.Size = new Size(183, 36);
            cbxVoucherType.TabIndex = 23;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(1105, 148);
            label13.Name = "label13";
            label13.Size = new Size(144, 28);
            label13.TabIndex = 22;
            label13.Text = "Ngày Bắt Đầu";
            label13.Click += label13_Click;
            // 
            // dtp_batdau
            // 
            dtp_batdau.CalendarFont = new Font("Segoe UI", 11F);
            dtp_batdau.CustomFormat = "dd/MM/yyyy";
            dtp_batdau.Format = DateTimePickerFormat.Custom;
            dtp_batdau.Location = new Point(1105, 190);
            dtp_batdau.Margin = new Padding(3, 4, 3, 4);
            dtp_batdau.Name = "dtp_batdau";
            dtp_batdau.ShowCheckBox = true;
            dtp_batdau.Size = new Size(183, 34);
            dtp_batdau.TabIndex = 21;
            dtp_batdau.Value = new DateTime(2026, 4, 4, 0, 0, 0, 0);
            // 
            // dtp_handung
            // 
            dtp_handung.CalendarFont = new Font("Segoe UI", 11F);
            dtp_handung.CustomFormat = "dd/MM/yyyy";
            dtp_handung.Format = DateTimePickerFormat.Custom;
            dtp_handung.Location = new Point(1105, 96);
            dtp_handung.Margin = new Padding(3, 4, 3, 4);
            dtp_handung.Name = "dtp_handung";
            dtp_handung.ShowCheckBox = true;
            dtp_handung.Size = new Size(183, 34);
            dtp_handung.TabIndex = 17;
            dtp_handung.Value = new DateTime(2026, 4, 4, 0, 0, 0, 0);
            dtp_handung.ValueChanged += dtp_handung_ValueChanged;
            // 
            // txtMinOrder
            // 
            txtMinOrder.Location = new Point(549, 67);
            txtMinOrder.Name = "txtMinOrder";
            txtMinOrder.Size = new Size(171, 34);
            txtMinOrder.TabIndex = 13;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(894, 127);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(183, 34);
            textBox5.TabIndex = 11;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(894, 67);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(183, 34);
            textBox4.TabIndex = 10;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(185, 118);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(183, 34);
            textBox2.TabIndex = 8;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(185, 64);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(183, 34);
            textBox1.TabIndex = 7;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(396, 180);
            label12.Name = "label12";
            label12.Size = new Size(112, 28);
            label12.TabIndex = 6;
            label12.Text = "Trạng Thái";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(1105, 64);
            label11.Name = "label11";
            label11.Size = new Size(107, 28);
            label11.TabIndex = 5;
            label11.Text = "Hạn Dùng";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(760, 130);
            label10.Name = "label10";
            label10.Size = new Size(131, 28);
            label10.TabIndex = 4;
            label10.Text = "Lượt Còn Lại";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(760, 70);
            label9.Name = "label9";
            label9.Size = new Size(128, 28);
            label9.TabIndex = 3;
            label9.Text = "Giá Trị Giảm";
            label9.Click += label9_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 180);
            label8.Name = "label8";
            label8.Size = new Size(51, 28);
            label8.TabIndex = 2;
            label8.Text = "Loại";
            label8.Click += label8_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 127);
            label7.Name = "label7";
            label7.Size = new Size(179, 28);
            label7.TabIndex = 1;
            label7.Text = "Tên Chương Trình";
            label7.Click += label7_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 70);
            label6.Name = "label6";
            label6.Size = new Size(134, 28);
            label6.TabIndex = 0;
            label6.Text = "Mã Vouchers";
            label6.Click += label6_Click;
            // 
            // frmVouchers
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1491, 1325);
            Controls.Add(grpNhapLieu);
            Controls.Add(grpDanhSach);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(pbx1);
            Name = "frmVouchers";
            Text = "frmVouchers";
            Load += frmVouchers_Load;
            ((System.ComponentModel.ISupportInitialize)pbx1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            grpDanhSach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVouchers).EndInit();
            grpNhapLieu.ResumeLayout(false);
            grpNhapLieu.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbx1;
        private Label lbl1;
        private Label lbl2;
        private Label lbl3;
        private Label lbl4_hieuung;
        private Label lbl4;
        private GroupBox groupBox1;
        private Button button2;
        private TextBox txtSearch;
        private GroupBox groupBox2;
        private Label label4;
        private Label label3;
        private Label label2;
        private ComboBox cbx1_tatcama;
        private GroupBox groupBox3;
        private Button btt_xoa;
        private Button btt_lammoi;
        private Button btt_sua;
        private Button btt_them;
        private GroupBox grpDanhSach;
        private DataGridView dgvVouchers;
        private GroupBox grpNhapLieu;
        private Label label6;
        private Label label7;
        private Label label10;
        private Label label9;
        private Label label8;
        private TextBox txtMinOrder;
        private TextBox textBox5;
        private TextBox textBox4;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label12;
        private Label label11;
        private DateTimePicker dtp_handung;
        private Label label13;
        private DateTimePicker dtp_batdau;
        private ComboBox cbxVoucherType;
        private Label label15;
        private Label label14;
        private TextBox txtMaxDiscount;
        private TextBox textBox3;
        private Label label16;
    }
}