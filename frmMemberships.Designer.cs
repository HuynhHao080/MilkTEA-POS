namespace MilkTeaPOS
{
    partial class frmMemberships
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
            label1 = new Label();
            label2 = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            dgvMemberships = new DataGridView();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            cbCustomer = new ComboBox();
            txtPoints = new TextBox();
            txtTotalSpent = new TextBox();
            txtTotalOrders = new TextBox();
            npgsqlDataAdapter1 = new Npgsql.NpgsqlDataAdapter();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnCancel = new Button();
            dtpJoinedAt = new DateTimePicker();
            dtpExpiresAt = new DateTimePicker();
            cbTier = new ComboBox();
            test = new Label();
            txtPhone = new TextBox();
            label9 = new Label();
            dtpLastOrder = new DateTimePicker();
            label10 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvMemberships).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(424, 9);
            label1.Name = "label1";
            label1.Size = new Size(418, 46);
            label1.TabIndex = 0;
            label1.Text = "QUẢN LÝ MEMBERSHIPS";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(79, 85);
            label2.Name = "label2";
            label2.Size = new Size(78, 20);
            label2.TabIndex = 1;
            label2.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(163, 80);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(270, 27);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.RoyalBlue;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(455, 76);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(94, 29);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // dgvMemberships
            // 
            dgvMemberships.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvMemberships.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvMemberships.BackgroundColor = Color.DarkGray;
            dgvMemberships.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMemberships.Location = new Point(79, 113);
            dgvMemberships.Name = "dgvMemberships";
            dgvMemberships.RowHeadersWidth = 51;
            dgvMemberships.Size = new Size(1103, 231);
            dgvMemberships.TabIndex = 4;
            dgvMemberships.CellClick += dgvMemberships_CellClick;
            dgvMemberships.CellContentClick += dgvMemberships_CellContentClick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(79, 367);
            label3.Name = "label3";
            label3.Size = new Size(103, 23);
            label3.TabIndex = 5;
            label3.Text = "Khách hàng";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(79, 504);
            label4.Name = "label4";
            label4.Size = new Size(53, 23);
            label4.TabIndex = 6;
            label4.Text = "Điểm";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(488, 367);
            label5.Name = "label5";
            label5.Size = new Size(72, 23);
            label5.TabIndex = 7;
            label5.Text = "Chi tiêu";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(802, 367);
            label6.Name = "label6";
            label6.Size = new Size(172, 23);
            label6.TabIndex = 8;
            label6.Text = "Tổng đơn đã Orders";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(488, 500);
            label7.Name = "label7";
            label7.Size = new Size(127, 23);
            label7.TabIndex = 9;
            label7.Text = "Ngày gia nhập";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(802, 507);
            label8.Name = "label8";
            label8.Size = new Size(117, 23);
            label8.TabIndex = 10;
            label8.Text = "Ngày hết hạn";
            // 
            // cbCustomer
            // 
            cbCustomer.FormattingEnabled = true;
            cbCustomer.Location = new Point(189, 362);
            cbCustomer.Name = "cbCustomer";
            cbCustomer.Size = new Size(222, 28);
            cbCustomer.TabIndex = 11;
            // 
            // txtPoints
            // 
            txtPoints.Location = new Point(189, 500);
            txtPoints.Name = "txtPoints";
            txtPoints.Size = new Size(222, 27);
            txtPoints.TabIndex = 12;
            // 
            // txtTotalSpent
            // 
            txtTotalSpent.Location = new Point(573, 362);
            txtTotalSpent.Name = "txtTotalSpent";
            txtTotalSpent.Size = new Size(187, 27);
            txtTotalSpent.TabIndex = 13;
            // 
            // txtTotalOrders
            // 
            txtTotalOrders.Location = new Point(980, 366);
            txtTotalOrders.Name = "txtTotalOrders";
            txtTotalOrders.Size = new Size(202, 27);
            txtTotalOrders.TabIndex = 14;
            txtTotalOrders.TextChanged += txtTotalOrders_TextChanged;
            // 
            // npgsqlDataAdapter1
            // 
            npgsqlDataAdapter1.DeleteCommand = null;
            npgsqlDataAdapter1.InsertCommand = null;
            npgsqlDataAdapter1.SelectCommand = null;
            npgsqlDataAdapter1.UpdateCommand = null;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(128, 255, 128);
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(79, 607);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(96, 50);
            btnAdd.TabIndex = 17;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click_1;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.Gold;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(217, 607);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(96, 50);
            btnEdit.TabIndex = 18;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click_1;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Red;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(360, 607);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(96, 50);
            btnDelete.TabIndex = 19;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Turquoise;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(502, 607);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(96, 50);
            btnCancel.TabIndex = 21;
            btnCancel.Text = "Thoát";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // dtpJoinedAt
            // 
            dtpJoinedAt.CalendarFont = new Font("Segoe UI", 11F);
            dtpJoinedAt.CustomFormat = "dd/MM/yyyy";
            dtpJoinedAt.Format = DateTimePickerFormat.Custom;
            dtpJoinedAt.Location = new Point(621, 504);
            dtpJoinedAt.Margin = new Padding(3, 4, 3, 4);
            dtpJoinedAt.Name = "dtpJoinedAt";
            dtpJoinedAt.ShowCheckBox = true;
            dtpJoinedAt.Size = new Size(139, 27);
            dtpJoinedAt.TabIndex = 22;
            dtpJoinedAt.Value = new DateTime(2026, 4, 4, 0, 0, 0, 0);
            // 
            // dtpExpiresAt
            // 
            dtpExpiresAt.CalendarFont = new Font("Segoe UI", 11F);
            dtpExpiresAt.CustomFormat = "dd/MM/yyyy";
            dtpExpiresAt.Format = DateTimePickerFormat.Custom;
            dtpExpiresAt.Location = new Point(980, 501);
            dtpExpiresAt.Margin = new Padding(3, 4, 3, 4);
            dtpExpiresAt.Name = "dtpExpiresAt";
            dtpExpiresAt.ShowCheckBox = true;
            dtpExpiresAt.Size = new Size(202, 27);
            dtpExpiresAt.TabIndex = 23;
            dtpExpiresAt.Value = new DateTime(2026, 4, 4, 0, 0, 0, 0);
            // 
            // cbTier
            // 
            cbTier.FormattingEnabled = true;
            cbTier.Items.AddRange(new object[] { "none", "silver", "gold", "platinum", "diamond" });
            cbTier.Location = new Point(189, 432);
            cbTier.Name = "cbTier";
            cbTier.Size = new Size(222, 28);
            cbTier.TabIndex = 24;
            // 
            // test
            // 
            test.AutoSize = true;
            test.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            test.Location = new Point(79, 433);
            test.Name = "test";
            test.Size = new Size(58, 23);
            test.TabIndex = 25;
            test.Text = "Hạng ";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(573, 430);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(187, 27);
            txtPhone.TabIndex = 27;
            txtPhone.TextChanged += txtPhone_TextChanged;
            txtPhone.KeyDown += txtPhone_KeyDown;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(488, 433);
            label9.Name = "label9";
            label9.Size = new Size(43, 23);
            label9.TabIndex = 26;
            label9.Text = "SĐT";
            // 
            // dtpLastOrder
            // 
            dtpLastOrder.CalendarFont = new Font("Segoe UI", 11F);
            dtpLastOrder.CustomFormat = "dd/MM/yyyy";
            dtpLastOrder.Format = DateTimePickerFormat.Custom;
            dtpLastOrder.Location = new Point(980, 429);
            dtpLastOrder.Margin = new Padding(3, 4, 3, 4);
            dtpLastOrder.Name = "dtpLastOrder";
            dtpLastOrder.ShowCheckBox = true;
            dtpLastOrder.Size = new Size(202, 27);
            dtpLastOrder.TabIndex = 28;
            dtpLastOrder.Value = new DateTime(2026, 4, 4, 0, 0, 0, 0);
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(802, 434);
            label10.Name = "label10";
            label10.Size = new Size(128, 23);
            label10.TabIndex = 29;
            label10.Text = "Lần cuối Order";
            // 
            // frmMemberships
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1224, 672);
            Controls.Add(label10);
            Controls.Add(dtpLastOrder);
            Controls.Add(txtPhone);
            Controls.Add(label9);
            Controls.Add(test);
            Controls.Add(cbTier);
            Controls.Add(dtpExpiresAt);
            Controls.Add(dtpJoinedAt);
            Controls.Add(btnCancel);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(txtTotalOrders);
            Controls.Add(txtTotalSpent);
            Controls.Add(txtPoints);
            Controls.Add(cbCustomer);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(dgvMemberships);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "frmMemberships";
            Text = "Memberships";
            Load += Memberships_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMemberships).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtSearch;
        private Button btnSearch;
        private DataGridView dgvMemberships;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private ComboBox cbCustomer;
        private TextBox txtPoints;
        private TextBox txtTotalSpent;
        private TextBox txtTotalOrders;
        private Npgsql.NpgsqlDataAdapter npgsqlDataAdapter1;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnCancel;
        private DateTimePicker dtpJoinedAt;
        private DateTimePicker dtpExpiresAt;
        private ComboBox cbTier;
        private Label test;
        private TextBox txtPhone;
        private Label label9;
        private DateTimePicker dtpLastOrder;
        private Label label10;
    }
}