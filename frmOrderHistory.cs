using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MilkTeaPOS.Models;

namespace MilkTeaPOS
{
    public partial class frmOrderHistory : Form
    {
        private readonly List<OrderHistoryItem> _orders = new();
        private Guid? _selectedOrderId;
        private OrderListMode _mode = OrderListMode.Paid;

        public Guid? ResumeOrderId { get; private set; }

        public frmOrderHistory()
        {
            InitializeComponent();
            btnLoc.Click += btnLoc_Click;
        }

        private async void frmOrderHistory_Load_1(object sender, EventArgs e)
        {
            dtpTungay.Format = DateTimePickerFormat.Custom;
            dtpTungay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            dtpTungay.Value = DateTime.Today;
            dtpDenNgay.Value = DateTime.Today;

            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += async (_, _) =>
            {
                _selectedOrderId = GetFilteredOrders().FirstOrDefault()?.Id;
                RenderOrderList();
                await RenderOrderDetailsAsync(_selectedOrderId);
            };

            await LoadDashboardAsync();
            await LoadOrdersAsync();
            RenderOrderList();
            await RenderOrderDetailsAsync(_selectedOrderId);
        }

        private async void btnLoc_Click(object? sender, EventArgs e)
        {
            await LoadDashboardAsync();
            await LoadOrdersAsync();
            RenderOrderList();
            await RenderOrderDetailsAsync(_selectedOrderId);
        }

        private async Task LoadDashboardAsync()
        {
            try
            {
                await using var context = new PostgresContext();
                var connection = context.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await using var command = connection.CreateCommand();
                command.CommandText = @"
SELECT
    COUNT(*) FILTER (WHERE DATE(created_at) = CURRENT_DATE) AS orders_today,
    COALESCE(SUM(total_amount) FILTER (WHERE DATE(created_at) = CURRENT_DATE AND status = 'served'), 0) AS revenue_today,
    COUNT(*) FILTER (WHERE DATE(created_at) = CURRENT_DATE AND status IN ('pending', 'preparing', 'ready')) AS holding_today,
    COUNT(*) FILTER (WHERE DATE(created_at) = CURRENT_DATE AND status = 'cancelled') AS refunded_today
FROM orders;";

                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var ordersToday = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var revenue = reader.IsDBNull(1) ? 0m : reader.GetDecimal(1);
                    var holding = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    var refunded = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);

                    lblDonhomnay.Text = $"{ordersToday}\nĐơn hôm nay";
                    lblDoanhthu.Text = $"{FormatCurrency(revenue)}\nDoanh thu";
                    lblDondangiu.Text = $"{holding}\nĐơn đang giữ";
                    lblDonHoantra.Text = $"{refunded}\nĐơn hoàn trả";
                }
            }
            catch
            {
            }
        }

        private async Task LoadOrdersAsync()
        {
            _orders.Clear();

            try
            {
                var fromDate = dtpTungay.Value.Date;
                var toDate = dtpDenNgay.Value.Date.AddDays(1);

                await using var context = new PostgresContext();
                var connection = context.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await using var command = connection.CreateCommand();
                command.CommandText = @"
SELECT
    o.id,
    COALESCE(o.order_number, '') AS order_number,
    COALESCE(o.created_at, NOW()) AS created_at,
    COALESCE(o.total_amount, 0) AS total_amount,
    COALESCE(o.customer_name, '') AS customer_name,
    COALESCE(o.customer_phone, '') AS customer_phone,
    COALESCE(o.is_delivery, FALSE) AS is_delivery,
    COALESCE(t.name, '') AS table_name,
    o.status::text AS order_status,
    COALESCE((
        SELECT p.method::text
        FROM payments p
        WHERE p.order_id = o.id
        ORDER BY p.created_at DESC
        LIMIT 1
    ), '') AS payment_method,
    COALESCE((
        SELECT p.status::text
        FROM payments p
        WHERE p.order_id = o.id
        ORDER BY p.created_at DESC
        LIMIT 1
    ), '') AS payment_status
FROM orders o
LEFT JOIN tables t ON t.id = o.table_id
WHERE o.created_at >= @fromDate
  AND o.created_at < @toDate
ORDER BY o.created_at DESC;";

                var pFrom = command.CreateParameter();
                pFrom.ParameterName = "@fromDate";
                pFrom.Value = fromDate;
                command.Parameters.Add(pFrom);

                var pTo = command.CreateParameter();
                pTo.ParameterName = "@toDate";
                pTo.Value = toDate;
                command.Parameters.Add(pTo);

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    _orders.Add(new OrderHistoryItem
                    {
                        Id = reader.GetGuid(0),
                        OrderNumber = reader.GetString(1),
                        CreatedAt = reader.GetDateTime(2),
                        TotalAmount = reader.GetDecimal(3),
                        CustomerName = reader.GetString(4),
                        CustomerPhone = reader.GetString(5),
                        IsDelivery = reader.GetBoolean(6),
                        TableName = reader.GetString(7),
                        OrderStatus = reader.GetString(8),
                        PaymentMethod = reader.GetString(9),
                        PaymentStatus = reader.GetString(10)
                    });
                }

                UpdateFilterOptions();
                _selectedOrderId = GetFilteredOrders().FirstOrDefault()?.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được lịch sử đơn.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateFilterOptions()
        {
            var current = cboTrangThai.SelectedItem?.ToString() ?? "Tất cả";
            var options = new List<string> { "Tất cả", "Mang về", "Giao hàng", "Dùng tại quán" };
            options.AddRange(_orders
                .Where(x => !x.IsDelivery && !string.IsNullOrWhiteSpace(x.TableName))
                .Select(x => x.TableName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x));

            cboTrangThai.Items.Clear();
            foreach (var option in options)
            {
                cboTrangThai.Items.Add(option);
            }

            cboTrangThai.SelectedItem = options.Any(x => string.Equals(x, current, StringComparison.OrdinalIgnoreCase)) ? current : "Tất cả";
        }

        private List<OrderHistoryItem> GetFilteredOrders()
        {
            var filter = cboTrangThai.SelectedItem?.ToString() ?? "Tất cả";
            return _orders
                .Where(x => MatchFilter(x, filter))
                .Where(x => _mode == OrderListMode.Paid ? IsPaid(x) : IsHold(x))
                .ToList();
        }

        private static bool MatchFilter(OrderHistoryItem item, string filter)
        {
            if (filter == "Tất cả") return true;
            if (filter == "Mang về") return item.IsDelivery && string.IsNullOrWhiteSpace(item.TableName);
            if (filter == "Giao hàng") return item.IsDelivery;
            if (filter == "Dùng tại quán") return !item.IsDelivery;
            return string.Equals(item.TableName, filter, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPaid(OrderHistoryItem item)
            => item.OrderStatus == "served"
               || item.PaymentStatus == "completed"
               || item.PaymentStatus == "refunded"
               || item.OrderStatus == "cancelled";

        private static bool IsHold(OrderHistoryItem item)
            => !IsPaid(item) && item.OrderStatus is "pending" or "preparing" or "ready";

        private void RenderOrderList()
        {
            pnContent.Controls.Clear();

            var root = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var topTabs = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = Color.FromArgb(247, 247, 247),
                ColumnCount = 2,
                RowCount = 1
            };
            topTabs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            topTabs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            topTabs.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var btnPaid = new Button
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Text = $"Đã thanh toán  {_orders.Count(IsPaid)}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = _mode == OrderListMode.Paid ? Color.White : Color.FromArgb(247, 247, 247),
                ForeColor = _mode == OrderListMode.Paid ? Color.FromArgb(18, 125, 82) : Color.FromArgb(70, 70, 70)
            };
            var btnHold = new Button
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Text = $"Đơn giữ  {_orders.Count(IsHold)}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = _mode == OrderListMode.Hold ? Color.White : Color.FromArgb(247, 247, 247),
                ForeColor = _mode == OrderListMode.Hold ? Color.FromArgb(167, 111, 25) : Color.FromArgb(70, 70, 70)
            };

            btnPaid.FlatAppearance.BorderColor = Color.Gainsboro;
            btnHold.FlatAppearance.BorderColor = Color.Gainsboro;
            btnPaid.FlatAppearance.BorderSize = 0;
            btnHold.FlatAppearance.BorderSize = 0;

            btnPaid.Click += async (_, _) =>
            {
                _mode = OrderListMode.Paid;
                _selectedOrderId = GetFilteredOrders().FirstOrDefault()?.Id;
                RenderOrderList();
                await RenderOrderDetailsAsync(_selectedOrderId);
            };

            btnHold.Click += async (_, _) =>
            {
                _mode = OrderListMode.Hold;
                _selectedOrderId = GetFilteredOrders().FirstOrDefault()?.Id;
                RenderOrderList();
                await RenderOrderDetailsAsync(_selectedOrderId);
            };

            topTabs.Controls.Add(btnPaid, 0, 0);
            topTabs.Controls.Add(btnHold, 1, 0);

            var listPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(0)
            };

            var orders = GetFilteredOrders();
            foreach (var order in orders)
            {
                listPanel.Controls.Add(CreateOrderCard(order));
            }

            if (orders.Count == 0)
            {
                listPanel.Controls.Add(new Label
                {
                    Width = Math.Max(200, pnContent.Width - 40),
                    Height = 44,
                    Text = "Không có dữ liệu đơn hàng",
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.DimGray
                });
            }

            root.Controls.Add(listPanel);
            root.Controls.Add(topTabs);
            pnContent.Controls.Add(root);
        }

        private Control CreateOrderCard(OrderHistoryItem order)
        {
            var isSelected = _selectedOrderId == order.Id;
            var card = new Panel
            {
                Width = Math.Max(300, pnContent.ClientSize.Width - 20),
                Height = 96,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = isSelected ? Color.FromArgb(248, 238, 238) : Color.White,
                Cursor = Cursors.Hand
            };

            var code = string.IsNullOrWhiteSpace(order.OrderNumber) ? order.Id.ToString()[..8].ToUpperInvariant() : order.OrderNumber;
            var place = order.IsDelivery ? "Mang về" : order.TableName;
            var customer = string.IsNullOrWhiteSpace(order.CustomerName) ? order.CustomerPhone : order.CustomerName;

            card.Controls.Add(new Label { Left = 12, Top = 9, Width = 220, Height = 22, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(33, 37, 41), Text = code });
            card.Controls.Add(new Label { Left = 12, Top = 31, Width = 260, Height = 18, ForeColor = Color.FromArgb(95, 95, 95), Font = new Font("Segoe UI", 9F), Text = $"{order.CreatedAt:HH:mm} — {place}" });
            card.Controls.Add(new Label { Left = 12, Top = 52, Width = 300, Height = 36, ForeColor = Color.FromArgb(70, 70, 70), Font = new Font("Segoe UI", 9F), Text = customer });

            var lblAmount = new Label
            {
                Left = card.Width - 132,
                Top = 12,
                Width = 116,
                Height = 22,
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.FromArgb(220, 53, 69),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = FormatCurrency(order.TotalAmount)
            };

            var lblStatus = new Label
            {
                Left = card.Width - 76,
                Top = 39,
                Width = 60,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Text = ToStatus(order.OrderStatus, order.PaymentStatus),
                ForeColor = IsHold(order) ? Color.FromArgb(120, 82, 14) : Color.FromArgb(27, 103, 62),
                BackColor = IsHold(order) ? Color.FromArgb(252, 239, 213) : Color.FromArgb(225, 245, 233)
            };

            var lblMethod = new Label
            {
                Left = card.Width - 88,
                Top = 63,
                Width = 72,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 90, 170),
                BackColor = Color.FromArgb(232, 242, 255),
                Text = ToMethod(order.PaymentMethod)
            };

            card.Controls.Add(lblAmount);
            card.Controls.Add(lblStatus);
            card.Controls.Add(lblMethod);
            card.Controls.Add(new Panel
            {
                Left = 0,
                Top = card.Height - 1,
                Width = card.Width,
                Height = 1,
                BackColor = Color.FromArgb(229, 229, 229)
            });

            async void SelectOrder(object? _, EventArgs __)
            {
                _selectedOrderId = order.Id;
                RenderOrderList();
                await RenderOrderDetailsAsync(order.Id);
            }

            card.Click += SelectOrder;
            foreach (Control child in card.Controls)
            {
                child.Click += SelectOrder;
            }

            return card;
        }

        private async Task RenderOrderDetailsAsync(Guid? orderId)
        {
            pnChiTiet.Controls.Clear();
            if (orderId == null)
            {
                pnChiTiet.Controls.Add(new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.DimGray,
                    Text = "Chọn đơn để xem chi tiết"
                });
                return;
            }

            try
            {
                var order = _orders.FirstOrDefault(x => x.Id == orderId.Value);
                if (order == null) return;

                var details = await LoadOrderDetailsAsync(orderId.Value);
                var isHold = IsHold(order);

                var container = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
                var code = string.IsNullOrWhiteSpace(order.OrderNumber) ? order.Id.ToString()[..8].ToUpperInvariant() : order.OrderNumber;

                var lblHeader = new Label
                {
                    Left = 8,
                    Top = 8,
                    Width = pnChiTiet.Width - 16,
                    Height = 30,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(33, 37, 41),
                    Text = isHold ? $"{code} — Đơn giữ" : $"{code} — Chi tiết đơn"
                };

                var btnLeft = new Button
                {
                    Left = pnChiTiet.Width - 172,
                    Top = 42,
                    Width = 78,
                    Height = 34,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(248, 248, 248),
                    ForeColor = Color.FromArgb(45, 45, 45),
                    Text = isHold ? "Hủy đơn" : "In lại"
                };

                var btnRight = new Button
                {
                    Left = pnChiTiet.Width - 90,
                    Top = 42,
                    Width = 78,
                    Height = 34,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(248, 248, 248),
                    ForeColor = Color.FromArgb(45, 45, 45),
                    Text = isHold ? "Tiếp tục" : "Hoàn trả",
                    Enabled = isHold
                };

                btnLeft.FlatAppearance.BorderColor = Color.FromArgb(212, 212, 212);
                btnRight.FlatAppearance.BorderColor = Color.FromArgb(212, 212, 212);

                if (isHold)
                {
                    btnLeft.Click += async (_, _) => await CancelHoldOrderAsync(order.Id);
                    btnRight.Click += (_, _) =>
                    {
                        ResumeOrderId = order.Id;
                        DialogResult = DialogResult.OK;
                        Close();
                    };
                }
                else
                {
                    btnLeft.Click += async (_, _) =>
                    {
                        try
                        {
                            var printDetails = await LoadOrderDetailsAsync(order.Id);
                            PrintOrderReceipt(order, printDetails);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Không thể in hóa đơn.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                }

                var lblMeta = new Label
                {
                    Left = 8,
                    Top = 84,
                    Width = pnChiTiet.Width - 16,
                    Height = 50,
                    Font = new Font("Segoe UI", 9.5F),
                    Text = $"Bàn: {(order.IsDelivery ? "Mang về" : order.TableName)}   Thời gian: {order.CreatedAt:HH:mm dd/MM/yyyy}\nKhách: {(string.IsNullOrWhiteSpace(order.CustomerName) ? order.CustomerPhone : order.CustomerName)}"
                };

                var itemsPanel = new FlowLayoutPanel
                {
                    Left = 8,
                    Top = 142,
                    Width = pnChiTiet.Width - 24,
                    Height = pnChiTiet.Height - 236,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false
                };

                var index = 1;
                foreach (var d in details)
                {
                    var row = new Panel
                    {
                        Width = itemsPanel.Width - 24,
                        Height = 82,
                        BorderStyle = BorderStyle.None,
                        Margin = new Padding(0, 0, 0, 6)
                    };

                    row.Controls.Add(new Label { Left = 8, Top = 8, Width = 20, Height = 24, Font = new Font("Segoe UI", 9F), Text = index.ToString() });
                    row.Controls.Add(new Label { Left = 34, Top = 8, Width = 220, Height = 24, Text = d.ProductName, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(33, 37, 41) });
                    row.Controls.Add(new Label { Left = row.Width - 138, Top = 8, Width = 40, Height = 24, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Text = $"×{d.Quantity}" });
                    row.Controls.Add(new Label { Left = row.Width - 100, Top = 8, Width = 96, Height = 24, TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(220, 53, 69), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Text = FormatCurrency(d.Subtotal) });
                    row.Controls.Add(new Label { Left = 34, Top = 34, Width = row.Width - 42, Height = 40, ForeColor = Color.FromArgb(95, 95, 95), Font = new Font("Segoe UI", 9F), Text = d.Options });
                    row.Controls.Add(new Panel
                    {
                        Left = 34,
                        Top = row.Height - 1,
                        Width = row.Width - 42,
                        Height = 1,
                        BackColor = Color.FromArgb(230, 230, 230)
                    });

                    itemsPanel.Controls.Add(row);
                    index++;
                }

                var lblTotal = new Label
                {
                    Left = 8,
                    Top = pnChiTiet.Height - 52,
                    Width = pnChiTiet.Width - 16,
                    Height = 30,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight,
                    Text = isHold ? $"Tạm tính (chưa TT)   {FormatCurrency(order.TotalAmount)}" : $"Tổng   {FormatCurrency(order.TotalAmount)}"
                };

                container.Controls.Add(lblHeader);
                container.Controls.Add(btnLeft);
                container.Controls.Add(btnRight);
                container.Controls.Add(lblMeta);
                container.Controls.Add(itemsPanel);
                container.Controls.Add(lblTotal);
                pnChiTiet.Controls.Add(container);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được chi tiết đơn.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintOrderReceipt(OrderHistoryItem order, List<OrderDetailViewItem> details)
        {
            using var document = new PrintDocument();
            document.DocumentName = string.IsNullOrWhiteSpace(order.OrderNumber)
                ? $"ORDER-{order.Id.ToString()[..8].ToUpperInvariant()}"
                : order.OrderNumber;

            document.PrintPage += (_, e) =>
            {
                var g = e.Graphics;
                using var titleFont = new Font("Segoe UI", 11F, FontStyle.Bold);
                using var normalFont = new Font("Segoe UI", 9F, FontStyle.Regular);
                using var boldFont = new Font("Segoe UI", 9F, FontStyle.Bold);

                int y = 12;
                int left = 12;
                int right = e.MarginBounds.Right;

                var code = string.IsNullOrWhiteSpace(order.OrderNumber)
                    ? order.Id.ToString()[..8].ToUpperInvariant()
                    : order.OrderNumber;

                g.DrawString("MILKTEA POS", titleFont, Brushes.Black, left, y);
                y += 24;
                g.DrawString($"Hóa đơn: {code}", boldFont, Brushes.Black, left, y);
                y += 18;
                g.DrawString($"Thời gian: {order.CreatedAt:HH:mm dd/MM/yyyy}", normalFont, Brushes.Black, left, y);
                y += 18;
                g.DrawString($"Khách: {(string.IsNullOrWhiteSpace(order.CustomerName) ? order.CustomerPhone : order.CustomerName)}", normalFont, Brushes.Black, left, y);
                y += 18;
                g.DrawString($"Bàn: {(order.IsDelivery ? "Mang về" : order.TableName)}", normalFont, Brushes.Black, left, y);
                y += 14;
                g.DrawLine(Pens.Gray, left, y, right, y);
                y += 8;

                foreach (var item in details)
                {
                    g.DrawString($"{item.Quantity} x {item.ProductName}", normalFont, Brushes.Black, left, y);
                    g.DrawString(FormatCurrency(item.Subtotal), boldFont, Brushes.Black, right - 110, y);
                    y += 16;

                    if (!string.IsNullOrWhiteSpace(item.Options))
                    {
                        g.DrawString(item.Options.Replace('\n', ' '), normalFont, Brushes.DimGray, left + 8, y);
                        y += 16;
                    }
                }

                y += 4;
                g.DrawLine(Pens.Gray, left, y, right, y);
                y += 8;
                g.DrawString("TỔNG", boldFont, Brushes.Black, left, y);
                g.DrawString(FormatCurrency(order.TotalAmount), boldFont, Brushes.Black, right - 110, y);
                y += 22;
                g.DrawString("Cảm ơn quý khách!", normalFont, Brushes.Black, left, y);
                e.HasMorePages = false;
            };

            using var printDialog = new PrintDialog
            {
                Document = document,
                UseEXDialog = true
            };

            if (printDialog.ShowDialog(this) == DialogResult.OK)
            {
                document.Print();
            }
        }

        private async Task<List<OrderDetailViewItem>> LoadOrderDetailsAsync(Guid orderId)
        {
            var result = new List<OrderDetailViewItem>();
            await using var context = new PostgresContext();
            var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            await using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT
                    od.product_name,
                    COALESCE(od.quantity, 1),
                    COALESCE(od.subtotal, 0),
                    COALESCE(od.size::text, ''),
                    COALESCE(od.sugar::text, ''),
                    COALESCE(od.ice::text, ''),
                    COALESCE(string_agg(ot.topping_name, ', ' ORDER BY ot.topping_name), '') AS toppings
                FROM order_details od
                LEFT JOIN order_toppings ot ON ot.order_detail_id = od.id
                WHERE od.order_id = @orderId
                GROUP BY od.id, od.product_name, od.quantity, od.subtotal, od.size, od.sugar, od.ice
                ORDER BY od.created_at;";

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@orderId";
            parameter.Value = orderId;
            command.Parameters.Add(parameter);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var size = reader.GetString(3);
                var sugar = reader.GetString(4);
                var ice = reader.GetString(5);
                var toppings = reader.GetString(6);

                var options = $"{size} • đường {sugar}% • đá {ice}%";
                if (!string.IsNullOrWhiteSpace(toppings))
                {
                    options += $"\n+ {toppings}";
                }

                result.Add(new OrderDetailViewItem
                {
                    ProductName = reader.GetString(0),
                    Quantity = reader.GetInt32(1),
                    Subtotal = reader.GetDecimal(2),
                    Options = options
                });
            }

            return result;
        }

        private async Task CancelHoldOrderAsync(Guid orderId)
        {
            if (MessageBox.Show("Xác nhận hủy đơn giữ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                await using var context = new PostgresContext();
                var now = DateTime.UtcNow;
                await context.Database.ExecuteSqlInterpolatedAsync($@"
UPDATE orders
SET status = 'cancelled'::order_status,
    cancelled_at = {now},
    updated_at = {now}
WHERE id = {orderId}
  AND status IN ('pending', 'preparing', 'ready');
");

                await LoadDashboardAsync();
                await LoadOrdersAsync();
                RenderOrderList();
                await RenderOrderDetailsAsync(_selectedOrderId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể hủy đơn giữ.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string ToMethod(string method)
        {
            return method switch
            {
                "cash" => "Tiền mặt",
                "qr_code" => "CK/QR",
                "card" => "Thẻ",
                "bank_transfer" => "Chuyển khoản",
                "e_wallet" => "Ví điện tử",
                _ => string.IsNullOrWhiteSpace(method) ? string.Empty : method
            };
        }

        private static string ToStatus(string orderStatus, string paymentStatus)
        {
            if (orderStatus == "cancelled" || paymentStatus == "refunded") return "Hoàn trả";
            if (orderStatus is "pending" or "preparing" or "ready") return "Đơn giữ";
            if (orderStatus == "served" || paymentStatus == "completed") return "Đã TT";
            return orderStatus;
        }

        private static string FormatCurrency(decimal value) => $"{value:N0}đ";

        private sealed class OrderHistoryItem
        {
            public Guid Id { get; set; }
            public string OrderNumber { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public decimal TotalAmount { get; set; }
            public string CustomerName { get; set; } = string.Empty;
            public string CustomerPhone { get; set; } = string.Empty;
            public bool IsDelivery { get; set; }
            public string TableName { get; set; } = string.Empty;
            public string OrderStatus { get; set; } = string.Empty;
            public string PaymentMethod { get; set; } = string.Empty;
            public string PaymentStatus { get; set; } = string.Empty;
        }

        private sealed class OrderDetailViewItem
        {
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal Subtotal { get; set; }
            public string Options { get; set; } = string.Empty;
        }

        private enum OrderListMode
        {
            Paid,
            Hold
        }
    }
}
