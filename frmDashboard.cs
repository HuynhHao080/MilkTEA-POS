using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MilkTeaPOS.Models;

namespace MilkTeaPOS
{
    public partial class frmDashboard : Form
    {
        private System.Windows.Forms.Timer _clockTimer;
        private System.Windows.Forms.Timer _refreshTimer;

        public frmDashboard()
        {
            InitializeComponent();
            InitializeColumnHeaders();
            StartClock();
            StartAutoRefresh();
            LoadDashboard();
        }

        private void InitializeColumnHeaders()
        {
            try
            {
                // Recent Orders columns
                if (dgvRecentOrders.Columns.Count > 0)
                {
                    dgvRecentOrders.Columns[0].HeaderText = "Mã đơn";
                    dgvRecentOrders.Columns[1].HeaderText = "Khách hàng";
                    dgvRecentOrders.Columns[2].HeaderText = "Bàn";
                    dgvRecentOrders.Columns[3].HeaderText = "Tổng tiền";
                    dgvRecentOrders.Columns[4].HeaderText = "Trạng thái";
                    dgvRecentOrders.Columns[5].HeaderText = "Giờ";
                }

                // Top Products columns
                if (dgvTopProducts.Columns.Count > 0)
                {
                    dgvTopProducts.Columns[0].HeaderText = "";
                    dgvTopProducts.Columns[1].HeaderText = "Sản phẩm";
                    dgvTopProducts.Columns[2].HeaderText = "SL";
                    dgvTopProducts.Columns[3].HeaderText = "Doanh thu";
                }

                // Payment Breakdown columns
                if (dgvPaymentBreakdown.Columns.Count > 0)
                {
                    dgvPaymentBreakdown.Columns[0].HeaderText = "Phương thức";
                    dgvPaymentBreakdown.Columns[1].HeaderText = "Số GD";
                    dgvPaymentBreakdown.Columns[2].HeaderText = "Tổng tiền";
                }

                // Tables columns
                if (dgvTables.Columns.Count > 0)
                {
                    dgvTables.Columns[0].HeaderText = "Tên bàn";
                    dgvTables.Columns[1].HeaderText = "Vị trí";
                    dgvTables.Columns[2].HeaderText = "Sức chứa";
                    dgvTables.Columns[3].HeaderText = "Trạng thái";
                }
            }
            catch
            {
                // Ignore errors - columns might not be created yet
            }
        }

        private void StartClock()
        {
            _clockTimer = new System.Windows.Forms.Timer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += (s, e) =>
            {
                lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
                lblDate.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            };
            _clockTimer.Start();
        }

        private void StartAutoRefresh()
        {
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 30000;
            _refreshTimer.Tick += async (s, e) => await LoadDashboard();
            _refreshTimer.Start();
        }

        private async Task LoadDashboard()
        {
            try
            {
                await Task.WhenAll(
                    LoadKpiCards(),
                    LoadRecentOrders(),
                    LoadTopProducts(),
                    LoadTableStatus(),
                    LoadPaymentBreakdown(),
                    LoadRevenueChart(),
                    LoadHourlyOrders(),
                    LoadMembershipStats()
                );
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                    Invoke(() => MessageBox.Show($"❌ Lỗi tải dashboard:\n{ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
        }

        private async Task LoadKpiCards()
        {
            try
            {
                using var context = new PostgresContext();
                var today = DateTime.UtcNow.Date;
                var todayStart = today;
                var todayEnd = today.AddDays(1);

                // Revenue today - filter by served orders
                var revenueToday = await context.Orders
                    .Where(o => o.Status == "served" && o.CreatedAt >= todayStart && o.CreatedAt < todayEnd)
                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;

                // Orders today
                var ordersToday = await context.Orders
                    .CountAsync(o => o.CreatedAt >= todayStart && o.CreatedAt < todayEnd);

                // Orders yesterday for trend
                var yesterday = today.AddDays(-1);
                var ordersYesterday = await context.Orders
                    .CountAsync(o => o.CreatedAt >= yesterday && o.CreatedAt < todayStart);
                var orderTrend = ordersToday > ordersYesterday ? "▲" : ordersToday < ordersYesterday ? "▼" : "●";
                var orderDiff = ordersToday - ordersYesterday;

                // Active tables
                var activeTables = await context.Tables.CountAsync(t => t.Status == "occupied");
                var totalTables = await context.Tables.CountAsync();

                // Total customers
                var totalCustomers = await context.Customers.CountAsync();

                // Weekly revenue
                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                var revenueWeek = await context.Orders
                    .Where(o => o.Status == "served" && o.CreatedAt >= weekStart)
                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;

                // Average order value
                var avgOrderValue = ordersToday > 0 ? revenueToday / ordersToday : 0m;

                // VIP members
                var vipCount = await context.Memberships.CountAsync(m => 
                    m.Tier == "gold" || m.Tier == "platinum" || m.Tier == "diamond");

                // Pending & preparing orders
                var pendingOrders = await context.Orders.CountAsync(o => o.Status == "pending");
                var preparingOrders = await context.Orders.CountAsync(o => o.Status == "preparing");

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        lblCard1Value.Text = FormatCurrency(revenueToday);
                        lblCard1Value.Font = _fontCardValue;
                        lblCard1Trend.Text = $"{orderTrend} {orderDiff:+0;-0;0} so với hôm qua";
                        lblCard1Trend.Font = _fontCardTrend;

                        lblCard2Value.Text = ordersToday.ToString();
                        lblCard2Value.Font = _fontCardValue;
                        lblCard2Trend.Text = $"⏳ Chờ: {pendingOrders} | 🔥 Pha: {preparingOrders}";
                        lblCard2Trend.Font = _fontCardTrend;

                        lblCard3Value.Text = $"{activeTables}/{totalTables}";
                        lblCard3Value.Font = _fontCardValue;
                        lblCard3Trend.Text = $"Trống: {totalTables - activeTables} bàn";
                        lblCard3Trend.Font = _fontCardTrend;

                        lblCard4Value.Text = totalCustomers.ToString();
                        lblCard4Value.Font = _fontCardValue;
                        lblCard4Trend.Text = $"VIP: {vipCount} | TB: {FormatCurrency(avgOrderValue)}/đơn";
                        lblCard4Trend.Font = _fontCardTrend;

                        lblCard5Value.Text = FormatCurrency(revenueWeek);
                        lblCard5Value.Font = _fontCardValue;
                        lblCard5Trend.Text = $"TB ngày: {FormatCurrency(revenueWeek / 7)}";
                        lblCard5Trend.Font = _fontCardTrend;
                    });
                }
            }
            catch
            {
                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        lblCard1Value.Text = "0đ"; lblCard1Trend.Text = "";
                        lblCard2Value.Text = "0"; lblCard2Trend.Text = "";
                        lblCard3Value.Text = "0/0"; lblCard3Trend.Text = "";
                        lblCard4Value.Text = "0"; lblCard4Trend.Text = "";
                        lblCard5Value.Text = "0đ"; lblCard5Trend.Text = "";
                    });
                }
            }
        }

        private async Task LoadRecentOrders()
        {
            try
            {
                using var context = new PostgresContext();
                var recentOrders = await context.Orders
                    .Include(o => o.Table)
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                dgvRecentOrders.Rows.Clear();
                foreach (var order in recentOrders)
                {
                    var statusText = order.Status switch
                    {
                        "pending" => "⏳ Chờ",
                        "preparing" => "🔥 Đang pha",
                        "ready" => "✅ Sẵn sàng",
                        "served" => "🍹 Đã phục vụ",
                        "cancelled" => "❌ Hủy",
                        _ => order.Status ?? "❓ Không rõ"
                    };

                    var statusColor = order.Status switch
                    {
                        "pending" => Color.FromArgb(255, 193, 7),
                        "preparing" => Color.FromArgb(255, 107, 107),
                        "ready" => Color.FromArgb(72, 187, 120),
                        "served" => Color.FromArgb(23, 162, 184),
                        "cancelled" => Color.FromArgb(220, 53, 69),
                        _ => Color.Gray
                    };

                    var customerName = order.Customer?.Name ?? order.CustomerName ?? "Khách lẻ";
                    var tableName = order.Table?.Name ?? (order.IsDelivery == true ? "Mang đi" : "Tại quán");
                    var createTime = order.CreatedAt?.ToLocalTime().ToString("HH:mm") ?? "--:--";

                    dgvRecentOrders.Rows.Add(
                        order.OrderNumber ?? "",
                        customerName,
                        tableName,
                        FormatCurrency(order.TotalAmount ?? 0),
                        statusText,
                        createTime
                    );

                    // Set status color
                    var lastRow = dgvRecentOrders.Rows[dgvRecentOrders.Rows.Count - 1];
                    lastRow.Cells["Status"].Style.ForeColor = statusColor;
                    lastRow.Cells["Status"].Style.Font = _fontRowBold;
                }

                if (recentOrders.Count == 0)
                {
                    dgvRecentOrders.Rows.Add("", "", "", "💤 Chưa có đơn hàng nào", "", "");
                    dgvRecentOrders.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
                    dgvRecentOrders.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
                }
            }
            catch
            {
                // Silently fail - will retry on next refresh
            }
        }

        private async Task LoadTopProducts()
        {
            try
            {
                using var context = new PostgresContext();
                
                // Query using ProductName snapshot (more efficient than joining Product table)
                var topProducts = await context.OrderDetails
                    .GroupBy(od => new { od.ProductId, od.ProductName })
                    .Select(g => new 
                    { 
                        ProductId = g.Key.ProductId,
                        ProductName = g.Key.ProductName ?? "Unknown", 
                        TotalQty = g.Sum(x => x.Quantity), 
                        TotalRevenue = g.Sum(x => x.Subtotal) 
                    })
                    .OrderByDescending(x => x.TotalQty)
                    .Take(7)
                    .ToListAsync();

                dgvTopProducts.Rows.Clear();
                int rank = 1;
                foreach (var item in topProducts)
                {
                    var medal = rank == 1 ? "🥇" : rank == 2 ? "🥈" : rank == 3 ? "🥉" : $"{rank}.";
                    dgvTopProducts.Rows.Add(
                        medal, 
                        item.ProductName, 
                        item.TotalQty.ToString(), 
                        FormatCurrency(item.TotalRevenue)
                    );
                    
                    // Highlight top 3
                    if (rank <= 3)
                    {
                        dgvTopProducts.Rows[dgvTopProducts.Rows.Count - 1].DefaultCellStyle.Font = _fontRowBold;
                        dgvTopProducts.Rows[dgvTopProducts.Rows.Count - 1].DefaultCellStyle.ForeColor = 
                            rank == 1 ? Color.FromArgb(255, 193, 7) : 
                            rank == 2 ? Color.FromArgb(108, 117, 125) : 
                            Color.FromArgb(205, 127, 50);
                    }
                    rank++;
                }

                if (topProducts.Count == 0)
                {
                    dgvTopProducts.Rows.Add("", "💤 Chưa có sản phẩm nào", "", "");
                    dgvTopProducts.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
                    dgvTopProducts.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
                }
            }
            catch
            {
                // Silently fail - will retry on next refresh
            }
        }

        private async Task LoadTableStatus()
        {
            try
            {
                using var context = new PostgresContext();
                var tables = await context.Tables
                    .OrderBy(t => t.Name)
                    .ToListAsync();

                dgvTables.Rows.Clear();
                foreach (var table in tables)
                {
                    var statusEmoji = table.Status switch
                    {
                        "available" => "🟢",
                        "occupied" => "🔴",
                        "reserved" => "🟡",
                        "maintenance" => "⚫",
                        _ => "⚪"
                    };

                    var statusText = table.Status switch
                    {
                        "available" => "Trống",
                        "occupied" => "Đang dùng",
                        "reserved" => "Đã đặt",
                        "maintenance" => "Bảo trì",
                        _ => table.Status ?? "Không rõ"
                    };

                    var statusColor = table.Status switch
                    {
                        "available" => Color.FromArgb(72, 187, 120),
                        "occupied" => Color.FromArgb(220, 53, 69),
                        "reserved" => Color.FromArgb(255, 193, 7),
                        "maintenance" => Color.FromArgb(108, 117, 125),
                        _ => Color.Gray
                    };

                    dgvTables.Rows.Add(
                        table.Name, 
                        table.Location ?? "Khu vực chính", 
                        table.Capacity?.ToString() ?? "?",
                        statusEmoji + " " + statusText
                    );

                    // Set status color
                    var lastRow = dgvTables.Rows[dgvTables.Rows.Count - 1];
                    lastRow.Cells["TableStatus"].Style.ForeColor = statusColor;
                    lastRow.Cells["TableStatus"].Style.Font = _fontRowBold;
                }

                // Add summary row
                var totalTables = tables.Count;
                var availableCount = tables.Count(t => t.Status == "available");
                var occupiedCount = tables.Count(t => t.Status == "occupied");
                var reservedCount = tables.Count(t => t.Status == "reserved");
                var maintenanceCount = tables.Count(t => t.Status == "maintenance");

                dgvTables.Rows.Add(
                    $"📊 Tổng cộng ({totalTables} bàn)",
                    $"🟢 {availableCount} | 🔴 {occupiedCount} | 🟡 {reservedCount} | ⚫ {maintenanceCount}",
                    "",
                    ""
                );
                dgvTables.Rows[dgvTables.Rows.Count - 1].DefaultCellStyle.Font = _fontRowBold;
                dgvTables.Rows[dgvTables.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(240, 242, 245);
            }
            catch
            {
                // Silently fail - will retry on next refresh
            }
        }

        private async Task LoadPaymentBreakdown()
        {
            try
            {
                using var context = new PostgresContext();
                var today = DateTime.UtcNow.Date;

                var paymentStats = await context.Payments
                    .Where(p => p.Status == "completed" && p.CreatedAt >= today)
                    .GroupBy(p => p.Method)
                    .Select(g => new 
                    { 
                        Method = g.Key, 
                        Count = g.Count(), 
                        Total = g.Sum(x => (decimal?)x.PaidAmount) ?? 0m 
                    })
                    .ToListAsync();

                dgvPaymentBreakdown.Rows.Clear();
                foreach (var stat in paymentStats)
                {
                    var methodText = stat.Method switch
                    {
                        "cash" => "💵 Tiền mặt",
                        "card" => "💳 Thẻ",
                        "qr_code" => "📱 QR Code",
                        "bank_transfer" => "🏦 Chuyển khoản",
                        "e_wallet" => "📲 Ví điện tử",
                        _ => stat.Method ?? "❓ Khác"
                    };

                    var methodIcon = stat.Method switch
                    {
                        "cash" => "💵",
                        "card" => "💳",
                        "qr_code" => "📱",
                        "bank_transfer" => "🏦",
                        "e_wallet" => "📲",
                        _ => "❓"
                    };

                    dgvPaymentBreakdown.Rows.Add(
                        methodText, 
                        stat.Count.ToString(), 
                        FormatCurrency(stat.Total)
                    );
                }

                // Add totals row
                var totalTx = paymentStats.Sum(p => p.Count);
                var totalAmount = paymentStats.Sum(p => p.Total);
                dgvPaymentBreakdown.Rows.Add(
                    "📊 Tổng cộng", 
                    totalTx.ToString(), 
                    FormatCurrency(totalAmount)
                );
                dgvPaymentBreakdown.Rows[dgvPaymentBreakdown.Rows.Count - 1].DefaultCellStyle.Font = _fontRowBold;
                dgvPaymentBreakdown.Rows[dgvPaymentBreakdown.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(240, 242, 245);

                if (paymentStats.Count == 0)
                {
                    dgvPaymentBreakdown.Rows.Insert(0, "💤 Chưa có giao dịch nào", "", "");
                    dgvPaymentBreakdown.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
                    dgvPaymentBreakdown.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
                }
            }
            catch
            {
                // Silently fail - will retry on next refresh
            }
        }

        private async Task LoadRevenueChart()
        {
            try
            {
                using var context = new PostgresContext();
                var today = DateTime.UtcNow.Date;

                var orders = await context.Orders
                    .Where(o => o.Status == "served" && o.CreatedAt >= today)
                    .ToListAsync();

                var totalRevenue = orders.Sum(o => o.TotalAmount ?? 0m);
                var totalOrders = orders.Count;
                var avgOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0m;

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        lblChartInfo.Text = $"📈 Hôm nay: {totalOrders} đơn | {FormatCurrency(totalRevenue)} | TB: {FormatCurrency(avgOrderValue)}/đơn";
                        lblChartInfo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                    });
                }
            }
            catch
            {
                // Silently fail
            }
        }

        private async Task LoadHourlyOrders()
        {
            try
            {
                using var context = new PostgresContext();
                var today = DateTime.UtcNow.Date;
                var hourCounts = new int[14]; // 6:00 - 20:00

                var orders = await context.Orders
                    .Where(o => o.CreatedAt >= today)
                    .ToListAsync();

                foreach (var order in orders)
                {
                    var hour = order.CreatedAt?.Hour ?? 0;
                    if (hour >= 6 && hour <= 20)
                    {
                        hourCounts[hour - 6]++;
                    }
                }

                var peakHour = GetPeakHour(hourCounts);
                var avgPerHour = hourCounts.Average();

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        lblHourlyInfo.Text = $"⏰ Cao điểm: {peakHour} | TB/giờ: {avgPerHour:F1} đơn";
                        lblHourlyInfo.Font = new Font("Segoe UI", 11F);
                    });
                }
            }
            catch
            {
                // Silently fail
            }
        }

        private string GetPeakHour(int[] counts)
        {
            if (counts.Length == 0) return "N/A";
            
            int maxIndex = 0;
            for (int i = 1; i < counts.Length; i++)
            {
                if (counts[i] > counts[maxIndex]) maxIndex = i;
            }
            return $"{maxIndex + 6}h: {counts[maxIndex]} đơn";
        }

        private string FormatCurrency(decimal amount) => amount.ToString("#,##0") + "đ";

        private async Task LoadMembershipStats()
        {
            try
            {
                using var context = new PostgresContext();
                
                var tierStats = await context.Memberships
                    .GroupBy(m => m.Tier)
                    .Select(g => new 
                    { 
                        Tier = g.Key, 
                        Count = g.Count(),
                        AvgPoints = g.Average(x => (double?)x.Points) ?? 0,
                        TotalRevenue = g.Sum(x => (decimal?)x.TotalSpent) ?? 0m
                    })
                    .ToListAsync();

                var totalVouchers = await context.Vouchers.CountAsync();
                var totalVoucherUsage = await context.Vouchers
                    .SumAsync(v => (int?)v.UsageCount) ?? 0;
                var totalCustomers = await context.Customers.CountAsync();

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        var statsText = $"👥 Thành viên: {tierStats.Sum(t => t.Count)} | 🎫 Voucher: {totalVouchers} (Đã dùng: {totalVoucherUsage}) | 👤 Khách hàng: {totalCustomers}";
                        lblStatsInfo.Text = statsText;
                        lblStatsInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                    });
                }
            }
            catch
            {
                // Silently fail
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _clockTimer?.Stop();
            _refreshTimer?.Stop();
            _clockTimer?.Dispose();
            _refreshTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
