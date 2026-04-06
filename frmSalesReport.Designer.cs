namespace MilkTeaPOS
{
    partial class frmSalesReport
    {
        private System.ComponentModel.IContainer components = null;

        // Panels
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;

        // Header
        private System.Windows.Forms.Label lblTitle;

        // Toolbar
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnWeek;
        private System.Windows.Forms.Button btnMonth;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;

        // Filters
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;

        // Summary KPIs
        private System.Windows.Forms.Label lblTotalRevenueTitle;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.Label lblTotalOrdersTitle;
        private System.Windows.Forms.Label lblTotalOrders;
        private System.Windows.Forms.Label lblAvgOrderValueTitle;
        private System.Windows.Forms.Label lblAvgOrderValue;
        private System.Windows.Forms.Label lblTotalDiscountTitle;
        private System.Windows.Forms.Label lblTotalDiscount;
        private System.Windows.Forms.Label lblPendingOrdersTitle;
        private System.Windows.Forms.Label lblPendingOrders;
        private System.Windows.Forms.Label lblCancelledOrdersTitle;
        private System.Windows.Forms.Label lblCancelledOrders;
        private System.Windows.Forms.Label lblUniqueCustomersTitle;
        private System.Windows.Forms.Label lblUniqueCustomers;
        private System.Windows.Forms.Label lblAvgSpentPerCustomerTitle;
        private System.Windows.Forms.Label lblAvgSpentPerCustomer;

        // DataGridViews
        private System.Windows.Forms.DataGridView dgvDailyRevenue;
        private System.Windows.Forms.DataGridView dgvPaymentBreakdown;
        private System.Windows.Forms.DataGridView dgvProductPerformance;
        private System.Windows.Forms.DataGridView dgvOrderStats;
        private System.Windows.Forms.DataGridView dgvHourlyDistribution;
        private System.Windows.Forms.DataGridView dgvTopCustomers;

        // Loading
        private System.Windows.Forms.Label lblLoading;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();

            pnlHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            pnlToolbar = new System.Windows.Forms.Panel();
            btnToday = new System.Windows.Forms.Button();
            btnWeek = new System.Windows.Forms.Button();
            btnMonth = new System.Windows.Forms.Button();
            btnFilter = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnExport = new System.Windows.Forms.Button();
            pnlFilters = new System.Windows.Forms.Panel();
            lblStartDate = new System.Windows.Forms.Label();
            dtpStartDate = new System.Windows.Forms.DateTimePicker();
            lblEndDate = new System.Windows.Forms.Label();
            dtpEndDate = new System.Windows.Forms.DateTimePicker();
            pnlSummary = new System.Windows.Forms.Panel();
            lblTotalRevenueTitle = new System.Windows.Forms.Label();
            lblTotalRevenue = new System.Windows.Forms.Label();
            lblTotalOrdersTitle = new System.Windows.Forms.Label();
            lblTotalOrders = new System.Windows.Forms.Label();
            lblAvgOrderValueTitle = new System.Windows.Forms.Label();
            lblAvgOrderValue = new System.Windows.Forms.Label();
            lblTotalDiscountTitle = new System.Windows.Forms.Label();
            lblTotalDiscount = new System.Windows.Forms.Label();
            lblPendingOrdersTitle = new System.Windows.Forms.Label();
            lblPendingOrders = new System.Windows.Forms.Label();
            lblCancelledOrdersTitle = new System.Windows.Forms.Label();
            lblCancelledOrders = new System.Windows.Forms.Label();
            lblUniqueCustomersTitle = new System.Windows.Forms.Label();
            lblUniqueCustomers = new System.Windows.Forms.Label();
            lblAvgSpentPerCustomerTitle = new System.Windows.Forms.Label();
            lblAvgSpentPerCustomer = new System.Windows.Forms.Label();
            pnlMain = new System.Windows.Forms.Panel();
            pnlLeft = new System.Windows.Forms.Panel();
            dgvDailyRevenue = new System.Windows.Forms.DataGridView();
            dgvPaymentBreakdown = new System.Windows.Forms.DataGridView();
            dgvProductPerformance = new System.Windows.Forms.DataGridView();
            pnlRight = new System.Windows.Forms.Panel();
            dgvOrderStats = new System.Windows.Forms.DataGridView();
            dgvHourlyDistribution = new System.Windows.Forms.DataGridView();
            dgvTopCustomers = new System.Windows.Forms.DataGridView();
            lblLoading = new System.Windows.Forms.Label();

            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlSummary.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlLeft.SuspendLayout();
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDailyRevenue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPaymentBreakdown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductPerformance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderStats).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHourlyDistribution).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvTopCustomers).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.White;
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1920, 80);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(45, 55, 72);
            lblTitle.Location = new System.Drawing.Point(25, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(500, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📊 Báo cáo doanh thu";
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = System.Drawing.Color.White;
            pnlToolbar.Controls.Add(btnToday);
            pnlToolbar.Controls.Add(btnWeek);
            pnlToolbar.Controls.Add(btnMonth);
            pnlToolbar.Controls.Add(btnFilter);
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnExport);
            pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolbar.Location = new System.Drawing.Point(0, 80);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new System.Drawing.Size(1920, 60);
            pnlToolbar.TabIndex = 1;
            // 
            // btnToday
            // 
            btnToday.BackColor = System.Drawing.Color.FromArgb(72, 187, 120);
            btnToday.Cursor = System.Windows.Forms.Cursors.Hand;
            btnToday.FlatAppearance.BorderSize = 0;
            btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnToday.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnToday.ForeColor = System.Drawing.Color.White;
            btnToday.Location = new System.Drawing.Point(25, 10);
            btnToday.Name = "btnToday";
            btnToday.Size = new System.Drawing.Size(110, 40);
            btnToday.TabIndex = 0;
            btnToday.Text = "📅 Hôm nay";
            btnToday.UseVisualStyleBackColor = false;
            btnToday.Click += new System.EventHandler(btnToday_Click);
            // 
            // btnWeek
            // 
            btnWeek.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            btnWeek.Cursor = System.Windows.Forms.Cursors.Hand;
            btnWeek.FlatAppearance.BorderSize = 0;
            btnWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnWeek.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnWeek.ForeColor = System.Drawing.Color.White;
            btnWeek.Location = new System.Drawing.Point(145, 10);
            btnWeek.Name = "btnWeek";
            btnWeek.Size = new System.Drawing.Size(110, 40);
            btnWeek.TabIndex = 1;
            btnWeek.Text = "📆 7 ngày";
            btnWeek.UseVisualStyleBackColor = false;
            btnWeek.Click += new System.EventHandler(btnWeek_Click);
            // 
            // btnMonth
            // 
            btnMonth.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            btnMonth.FlatAppearance.BorderSize = 0;
            btnMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMonth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnMonth.ForeColor = System.Drawing.Color.White;
            btnMonth.Location = new System.Drawing.Point(265, 10);
            btnMonth.Name = "btnMonth";
            btnMonth.Size = new System.Drawing.Size(110, 40);
            btnMonth.TabIndex = 2;
            btnMonth.Text = "🗓️ 30 ngày";
            btnMonth.UseVisualStyleBackColor = false;
            btnMonth.Click += new System.EventHandler(btnMonth_Click);
            // 
            // btnFilter
            // 
            btnFilter.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            btnFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnFilter.ForeColor = System.Drawing.Color.White;
            btnFilter.Location = new System.Drawing.Point(800, 10);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new System.Drawing.Size(110, 40);
            btnFilter.TabIndex = 3;
            btnFilter.Text = "🔍 Lọc";
            btnFilter.UseVisualStyleBackColor = false;
            btnFilter.Click += new System.EventHandler(btnFilter_Click);
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Location = new System.Drawing.Point(920, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(120, 40);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
            // 
            // btnExport
            // 
            btnExport.BackColor = System.Drawing.Color.FromArgb(230, 126, 34);
            btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnExport.ForeColor = System.Drawing.Color.White;
            btnExport.Location = new System.Drawing.Point(1050, 10);
            btnExport.Name = "btnExport";
            btnExport.Size = new System.Drawing.Size(130, 40);
            btnExport.TabIndex = 5;
            btnExport.Text = "📥 Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += new System.EventHandler(btnExport_Click);
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = System.Drawing.Color.White;
            pnlFilters.Controls.Add(lblStartDate);
            pnlFilters.Controls.Add(dtpStartDate);
            pnlFilters.Controls.Add(lblEndDate);
            pnlFilters.Controls.Add(dtpEndDate);
            pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            pnlFilters.Location = new System.Drawing.Point(0, 140);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Size = new System.Drawing.Size(1920, 50);
            pnlFilters.TabIndex = 2;
            // 
            // lblStartDate
            // 
            lblStartDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblStartDate.Location = new System.Drawing.Point(25, 10);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Size = new System.Drawing.Size(80, 30);
            lblStartDate.TabIndex = 0;
            lblStartDate.Text = "Từ ngày:";
            lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpStartDate
            // 
            dtpStartDate.CustomFormat = "dd/MM/yyyy";
            dtpStartDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpStartDate.Location = new System.Drawing.Point(105, 12);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new System.Drawing.Size(130, 23);
            dtpStartDate.TabIndex = 1;
            dtpStartDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(dtpStartDate_KeyPress);
            // 
            // lblEndDate
            // 
            lblEndDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEndDate.Location = new System.Drawing.Point(245, 10);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.Size = new System.Drawing.Size(80, 30);
            lblEndDate.TabIndex = 2;
            lblEndDate.Text = "Đến ngày:";
            lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpEndDate
            // 
            dtpEndDate.CustomFormat = "dd/MM/yyyy";
            dtpEndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpEndDate.Location = new System.Drawing.Point(325, 12);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new System.Drawing.Size(130, 23);
            dtpEndDate.TabIndex = 3;
            dtpEndDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(dtpEndDate_KeyPress);
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = System.Drawing.Color.White;
            pnlSummary.Controls.Add(lblTotalRevenueTitle);
            pnlSummary.Controls.Add(lblTotalRevenue);
            pnlSummary.Controls.Add(lblTotalOrdersTitle);
            pnlSummary.Controls.Add(lblTotalOrders);
            pnlSummary.Controls.Add(lblAvgOrderValueTitle);
            pnlSummary.Controls.Add(lblAvgOrderValue);
            pnlSummary.Controls.Add(lblTotalDiscountTitle);
            pnlSummary.Controls.Add(lblTotalDiscount);
            pnlSummary.Controls.Add(lblPendingOrdersTitle);
            pnlSummary.Controls.Add(lblPendingOrders);
            pnlSummary.Controls.Add(lblCancelledOrdersTitle);
            pnlSummary.Controls.Add(lblCancelledOrders);
            pnlSummary.Controls.Add(lblUniqueCustomersTitle);
            pnlSummary.Controls.Add(lblUniqueCustomers);
            pnlSummary.Controls.Add(lblAvgSpentPerCustomerTitle);
            pnlSummary.Controls.Add(lblAvgSpentPerCustomer);
            pnlSummary.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSummary.Location = new System.Drawing.Point(0, 190);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new System.Drawing.Size(1920, 120);
            pnlSummary.TabIndex = 3;
            // 
            // lblTotalRevenueTitle
            // 
            lblTotalRevenueTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblTotalRevenueTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            lblTotalRevenueTitle.Location = new System.Drawing.Point(25, 15);
            lblTotalRevenueTitle.Name = "lblTotalRevenueTitle";
            lblTotalRevenueTitle.Size = new System.Drawing.Size(180, 20);
            lblTotalRevenueTitle.TabIndex = 0;
            lblTotalRevenueTitle.Text = "💰 Tổng doanh thu";
            lblTotalRevenueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalRevenue
            // 
            lblTotalRevenue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblTotalRevenue.ForeColor = System.Drawing.Color.FromArgb(72, 187, 120);
            lblTotalRevenue.Location = new System.Drawing.Point(25, 35);
            lblTotalRevenue.Name = "lblTotalRevenue";
            lblTotalRevenue.Size = new System.Drawing.Size(280, 40);
            lblTotalRevenue.TabIndex = 1;
            lblTotalRevenue.Text = "0đ";
            lblTotalRevenue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalOrdersTitle
            // 
            lblTotalOrdersTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblTotalOrdersTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            lblTotalOrdersTitle.Location = new System.Drawing.Point(250, 15);
            lblTotalOrdersTitle.Name = "lblTotalOrdersTitle";
            lblTotalOrdersTitle.Size = new System.Drawing.Size(150, 20);
            lblTotalOrdersTitle.TabIndex = 2;
            lblTotalOrdersTitle.Text = "📦 Tổng đơn hàng";
            lblTotalOrdersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalOrders
            // 
            lblTotalOrders.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblTotalOrders.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            lblTotalOrders.Location = new System.Drawing.Point(250, 35);
            lblTotalOrders.Name = "lblTotalOrders";
            lblTotalOrders.Size = new System.Drawing.Size(150, 40);
            lblTotalOrders.TabIndex = 3;
            lblTotalOrders.Text = "0";
            lblTotalOrders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAvgOrderValueTitle
            // 
            lblAvgOrderValueTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblAvgOrderValueTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            lblAvgOrderValueTitle.Location = new System.Drawing.Point(410, 15);
            lblAvgOrderValueTitle.Name = "lblAvgOrderValueTitle";
            lblAvgOrderValueTitle.Size = new System.Drawing.Size(150, 20);
            lblAvgOrderValueTitle.TabIndex = 4;
            lblAvgOrderValueTitle.Text = "📊 TB/đơn";
            lblAvgOrderValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAvgOrderValue
            // 
            lblAvgOrderValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblAvgOrderValue.ForeColor = System.Drawing.Color.FromArgb(155, 89, 182);
            lblAvgOrderValue.Location = new System.Drawing.Point(410, 35);
            lblAvgOrderValue.Name = "lblAvgOrderValue";
            lblAvgOrderValue.Size = new System.Drawing.Size(200, 40);
            lblAvgOrderValue.TabIndex = 5;
            lblAvgOrderValue.Text = "0đ";
            lblAvgOrderValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalDiscountTitle
            // 
            lblTotalDiscountTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblTotalDiscountTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            lblTotalDiscountTitle.Location = new System.Drawing.Point(620, 15);
            lblTotalDiscountTitle.Name = "lblTotalDiscountTitle";
            lblTotalDiscountTitle.Size = new System.Drawing.Size(150, 20);
            lblTotalDiscountTitle.TabIndex = 6;
            lblTotalDiscountTitle.Text = "🎫 Giảm giá";
            lblTotalDiscountTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalDiscount
            // 
            lblTotalDiscount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblTotalDiscount.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
            lblTotalDiscount.Location = new System.Drawing.Point(620, 35);
            lblTotalDiscount.Name = "lblTotalDiscount";
            lblTotalDiscount.Size = new System.Drawing.Size(200, 40);
            lblTotalDiscount.TabIndex = 7;
            lblTotalDiscount.Text = "0đ";
            lblTotalDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPendingOrdersTitle
            // 
            lblPendingOrdersTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblPendingOrdersTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            lblPendingOrdersTitle.Location = new System.Drawing.Point(25, 75);
            lblPendingOrdersTitle.Name = "lblPendingOrdersTitle";
            lblPendingOrdersTitle.Size = new System.Drawing.Size(120, 20);
            lblPendingOrdersTitle.TabIndex = 8;
            lblPendingOrdersTitle.Text = "⏳ Chờ xử lý";
            lblPendingOrdersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPendingOrders
            // 
            lblPendingOrders.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblPendingOrders.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            lblPendingOrders.Location = new System.Drawing.Point(25, 95);
            lblPendingOrders.Name = "lblPendingOrders";
            lblPendingOrders.Size = new System.Drawing.Size(100, 30);
            lblPendingOrders.TabIndex = 9;
            lblPendingOrders.Text = "0";
            lblPendingOrders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCancelledOrdersTitle
            // 
            lblCancelledOrdersTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblCancelledOrdersTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            lblCancelledOrdersTitle.Location = new System.Drawing.Point(135, 75);
            lblCancelledOrdersTitle.Name = "lblCancelledOrdersTitle";
            lblCancelledOrdersTitle.Size = new System.Drawing.Size(100, 20);
            lblCancelledOrdersTitle.TabIndex = 10;
            lblCancelledOrdersTitle.Text = "❌ Đã hủy";
            lblCancelledOrdersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCancelledOrders
            // 
            lblCancelledOrders.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblCancelledOrders.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
            lblCancelledOrders.Location = new System.Drawing.Point(135, 95);
            lblCancelledOrders.Name = "lblCancelledOrders";
            lblCancelledOrders.Size = new System.Drawing.Size(100, 30);
            lblCancelledOrders.TabIndex = 11;
            lblCancelledOrders.Text = "0";
            lblCancelledOrders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUniqueCustomersTitle
            // 
            lblUniqueCustomersTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblUniqueCustomersTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            lblUniqueCustomersTitle.Location = new System.Drawing.Point(250, 75);
            lblUniqueCustomersTitle.Name = "lblUniqueCustomersTitle";
            lblUniqueCustomersTitle.Size = new System.Drawing.Size(130, 20);
            lblUniqueCustomersTitle.TabIndex = 12;
            lblUniqueCustomersTitle.Text = "👥 Khách hàng";
            lblUniqueCustomersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUniqueCustomers
            // 
            lblUniqueCustomers.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblUniqueCustomers.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblUniqueCustomers.Location = new System.Drawing.Point(250, 95);
            lblUniqueCustomers.Name = "lblUniqueCustomers";
            lblUniqueCustomers.Size = new System.Drawing.Size(100, 30);
            lblUniqueCustomers.TabIndex = 13;
            lblUniqueCustomers.Text = "0";
            lblUniqueCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAvgSpentPerCustomerTitle
            // 
            lblAvgSpentPerCustomerTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblAvgSpentPerCustomerTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            lblAvgSpentPerCustomerTitle.Location = new System.Drawing.Point(360, 75);
            lblAvgSpentPerCustomerTitle.Name = "lblAvgSpentPerCustomerTitle";
            lblAvgSpentPerCustomerTitle.Size = new System.Drawing.Size(120, 20);
            lblAvgSpentPerCustomerTitle.TabIndex = 14;
            lblAvgSpentPerCustomerTitle.Text = "💵 TB/KH";
            lblAvgSpentPerCustomerTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAvgSpentPerCustomer
            // 
            lblAvgSpentPerCustomer.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblAvgSpentPerCustomer.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblAvgSpentPerCustomer.Location = new System.Drawing.Point(360, 95);
            lblAvgSpentPerCustomer.Name = "lblAvgSpentPerCustomer";
            lblAvgSpentPerCustomer.Size = new System.Drawing.Size(180, 30);
            lblAvgSpentPerCustomer.TabIndex = 15;
            lblAvgSpentPerCustomer.Text = "0đ";
            lblAvgSpentPerCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(pnlLeft);
            pnlMain.Controls.Add(pnlRight);
            pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMain.Location = new System.Drawing.Point(0, 310);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new System.Windows.Forms.Padding(10);
            pnlMain.Size = new System.Drawing.Size(1920, 750);
            pnlMain.TabIndex = 4;
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(dgvDailyRevenue);
            pnlLeft.Controls.Add(dgvPaymentBreakdown);
            pnlLeft.Controls.Add(dgvProductPerformance);
            pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLeft.Location = new System.Drawing.Point(10, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            pnlLeft.Size = new System.Drawing.Size(1060, 740);
            pnlLeft.TabIndex = 0;
            // 
            // dgvDailyRevenue
            // 
            dgvDailyRevenue.AllowUserToAddRows = false;
            dgvDailyRevenue.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            dgvDailyRevenue.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvDailyRevenue.BackgroundColor = System.Drawing.Color.FromArgb(247, 249, 252);
            dgvDailyRevenue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvDailyRevenue.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvDailyRevenue.ColumnHeadersHeight = 40;
            dgvDailyRevenue.Dock = System.Windows.Forms.DockStyle.Top;
            dgvDailyRevenue.EnableHeadersVisualStyles = false;
            dgvDailyRevenue.Location = new System.Drawing.Point(0, 0);
            dgvDailyRevenue.Name = "dgvDailyRevenue";
            dgvDailyRevenue.ReadOnly = true;
            dgvDailyRevenue.RowHeadersVisible = false;
            dgvDailyRevenue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvDailyRevenue.Size = new System.Drawing.Size(1060, 220);
            dgvDailyRevenue.TabIndex = 0;
            // 
            // dgvPaymentBreakdown
            // 
            dgvPaymentBreakdown.AllowUserToAddRows = false;
            dgvPaymentBreakdown.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            dgvPaymentBreakdown.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dgvPaymentBreakdown.BackgroundColor = System.Drawing.Color.FromArgb(247, 249, 252);
            dgvPaymentBreakdown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvPaymentBreakdown.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvPaymentBreakdown.ColumnHeadersHeight = 40;
            dgvPaymentBreakdown.Dock = System.Windows.Forms.DockStyle.Top;
            dgvPaymentBreakdown.EnableHeadersVisualStyles = false;
            dgvPaymentBreakdown.Location = new System.Drawing.Point(0, 220);
            dgvPaymentBreakdown.Name = "dgvPaymentBreakdown";
            dgvPaymentBreakdown.ReadOnly = true;
            dgvPaymentBreakdown.RowHeadersVisible = false;
            dgvPaymentBreakdown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvPaymentBreakdown.Size = new System.Drawing.Size(1060, 180);
            dgvPaymentBreakdown.TabIndex = 1;
            // 
            // dgvProductPerformance
            // 
            dgvProductPerformance.AllowUserToAddRows = false;
            dgvProductPerformance.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            dgvProductPerformance.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvProductPerformance.BackgroundColor = System.Drawing.Color.FromArgb(247, 249, 252);
            dgvProductPerformance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(230, 126, 34);
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(211, 84, 0);
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvProductPerformance.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvProductPerformance.ColumnHeadersHeight = 40;
            dgvProductPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvProductPerformance.EnableHeadersVisualStyles = false;
            dgvProductPerformance.Location = new System.Drawing.Point(0, 400);
            dgvProductPerformance.Name = "dgvProductPerformance";
            dgvProductPerformance.ReadOnly = true;
            dgvProductPerformance.RowHeadersVisible = false;
            dgvProductPerformance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvProductPerformance.Size = new System.Drawing.Size(1060, 340);
            dgvProductPerformance.TabIndex = 2;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(dgvOrderStats);
            pnlRight.Controls.Add(dgvHourlyDistribution);
            pnlRight.Controls.Add(dgvTopCustomers);
            pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            pnlRight.Location = new System.Drawing.Point(1070, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            pnlRight.Size = new System.Drawing.Size(840, 740);
            pnlRight.TabIndex = 1;
            // 
            // dgvOrderStats
            // 
            dgvOrderStats.AllowUserToAddRows = false;
            dgvOrderStats.AllowUserToDeleteRows = false;
            dgvOrderStats.BackgroundColor = System.Drawing.Color.FromArgb(247, 249, 252);
            dgvOrderStats.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvOrderStats.ColumnHeadersHeight = 40;
            dgvOrderStats.Dock = System.Windows.Forms.DockStyle.Top;
            dgvOrderStats.EnableHeadersVisualStyles = false;
            dgvOrderStats.Location = new System.Drawing.Point(0, 0);
            dgvOrderStats.Name = "dgvOrderStats";
            dgvOrderStats.ReadOnly = true;
            dgvOrderStats.RowHeadersVisible = false;
            dgvOrderStats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvOrderStats.Size = new System.Drawing.Size(830, 220);
            dgvOrderStats.TabIndex = 0;
            // 
            // dgvHourlyDistribution
            // 
            dgvHourlyDistribution.AllowUserToAddRows = false;
            dgvHourlyDistribution.AllowUserToDeleteRows = false;
            dgvHourlyDistribution.BackgroundColor = System.Drawing.Color.FromArgb(247, 249, 252);
            dgvHourlyDistribution.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvHourlyDistribution.ColumnHeadersHeight = 40;
            dgvHourlyDistribution.Dock = System.Windows.Forms.DockStyle.Top;
            dgvHourlyDistribution.EnableHeadersVisualStyles = false;
            dgvHourlyDistribution.Location = new System.Drawing.Point(0, 220);
            dgvHourlyDistribution.Name = "dgvHourlyDistribution";
            dgvHourlyDistribution.ReadOnly = true;
            dgvHourlyDistribution.RowHeadersVisible = false;
            dgvHourlyDistribution.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvHourlyDistribution.Size = new System.Drawing.Size(830, 250);
            dgvHourlyDistribution.TabIndex = 1;
            // 
            // dgvTopCustomers
            // 
            dgvTopCustomers.AllowUserToAddRows = false;
            dgvTopCustomers.AllowUserToDeleteRows = false;
            dgvTopCustomers.BackgroundColor = System.Drawing.Color.FromArgb(247, 249, 252);
            dgvTopCustomers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvTopCustomers.ColumnHeadersHeight = 40;
            dgvTopCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvTopCustomers.EnableHeadersVisualStyles = false;
            dgvTopCustomers.Location = new System.Drawing.Point(0, 470);
            dgvTopCustomers.Name = "dgvTopCustomers";
            dgvTopCustomers.ReadOnly = true;
            dgvTopCustomers.RowHeadersVisible = false;
            dgvTopCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvTopCustomers.Size = new System.Drawing.Size(830, 260);
            dgvTopCustomers.TabIndex = 2;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblLoading.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            lblLoading.Location = new System.Drawing.Point(850, 500);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new System.Drawing.Size(200, 21);
            lblLoading.TabIndex = 99;
            lblLoading.Text = "⏳ Đang tải dữ liệu...";
            lblLoading.Visible = false;
            // 
            // frmSalesReport
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            ClientSize = new System.Drawing.Size(1920, 1060);
            Controls.Add(pnlMain);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Controls.Add(lblLoading);
            Name = "frmSalesReport";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "📊 Báo cáo doanh thu - MilkTeaPOS";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            pnlSummary.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            pnlLeft.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDailyRevenue).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPaymentBreakdown).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductPerformance).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderStats).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHourlyDistribution).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvTopCustomers).EndInit();
            ResumeLayout(false);
            PerformLayout();

            InitializeDataGridViewColumns();
        }

        private void InitializeDataGridViewColumns()
        {
            // dgvDailyRevenue columns
            dgvDailyRevenue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Date", HeaderText = "📅 Ngày", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "DayOfWeek", HeaderText = "Thứ", Width = 70, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Orders", HeaderText = "Đơn hàng", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Revenue", HeaderText = "💰 Doanh thu", Width = 180, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } }
            });

            // dgvPaymentBreakdown columns
            dgvPaymentBreakdown.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Method", HeaderText = "💳 Phương thức", Width = 200, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Count", HeaderText = "Số GD", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Total", HeaderText = "💰 Tổng tiền", Width = 180, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "AvgAmount", HeaderText = "📊 Trung bình", Width = 140, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Percentage", HeaderText = "Tỷ lệ", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } }
            });

            // dgvProductPerformance columns
            dgvProductPerformance.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Rank", HeaderText = "XH", Width = 60, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ProductName", HeaderText = "🧋 Sản phẩm", Width = 300, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "SL", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Revenue", HeaderText = "💰 Doanh thu", Width = 180, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "AvgPrice", HeaderText = "Giá TB", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } }
            });

            // dgvOrderStats columns
            dgvOrderStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "OrderStatLabel", HeaderText = "Trạng thái", Width = 250, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "OrderStatValue", HeaderText = "Số lượng", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } }
            });

            // dgvHourlyDistribution columns
            dgvHourlyDistribution.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Hour", HeaderText = "⏰ Giờ", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Orders", HeaderText = "Đơn", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Revenue", HeaderText = "💰 Doanh thu", Width = 160, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Bar", HeaderText = "📊 Biểu đồ", Width = 400, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Consolas", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } }
            });

            // dgvTopCustomers columns
            dgvTopCustomers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "Rank", HeaderText = "XH", Width = 60, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "CustomerName", HeaderText = "👤 Khách hàng", Width = 300, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "OrderCount", HeaderText = "Số đơn", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter } },
                new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "TotalSpent", HeaderText = "💰 Tổng chi", Width = 180, DefaultCellStyle = new DataGridViewCellStyle { Font = new System.Drawing.Font("Segoe UI", 10F), Padding = new System.Windows.Forms.Padding(8, 5, 8, 5) } }
            });

            // Set AutoSizeColumnsMode
            dgvDailyRevenue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvPaymentBreakdown.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductPerformance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrderStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvHourlyDistribution.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopCustomers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // Set RowTemplate heights
            dgvDailyRevenue.RowTemplate.Height = 35;
            dgvPaymentBreakdown.RowTemplate.Height = 35;
            dgvProductPerformance.RowTemplate.Height = 35;
            dgvOrderStats.RowTemplate.Height = 35;
            dgvHourlyDistribution.RowTemplate.Height = 35;
            dgvTopCustomers.RowTemplate.Height = 35;
        }
    }
}
