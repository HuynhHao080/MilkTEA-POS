using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MilkTeaPOS.Models;
using Npgsql;

namespace MilkTeaPOS
{
    public partial class frmOrders : Form
    {
        private readonly List<Product> _allProducts = new();
        private readonly List<CartItem> _cartItems = new();
        private readonly List<Topping> _allToppings = new();
        private readonly Dictionary<string, List<string>> _categoryImageFiles = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Bitmap> _imageCache = new(StringComparer.OrdinalIgnoreCase);
        private FlowLayoutPanel? _productsContainer;
        private FlowLayoutPanel? _cartContainer;
        private string? _selectedCategoryKey;
        private Bitmap? _placeholderImage;
        private Customer? _selectedCustomer;
        private Membership? _selectedMembership;

        private Panel? _popupOverlay;
        private Panel? _popupSheet;
        private VoucherSnapshot? _appliedVoucher;
        private decimal _voucherDiscount;
        private readonly string? _initialOrderSelection;
        private readonly string? _initialTableName;

        public frmOrders() : this(null, null)
        {
        }

        public frmOrders(string? initialOrderSelection, string? initialTableName)
        {
            InitializeComponent();
            _initialOrderSelection = initialOrderSelection;
            _initialTableName = initialTableName;
        }

        private async void frmOrders_Load(object sender, EventArgs e)
        {
            InitializeOrdersScreen();
            await LoadOrderTypeOptionsAsync();
            await LoadProductsAsync();
            await LoadToppingsAsync();
            RenderProducts();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private async System.Threading.Tasks.Task LoadToppingsAsync()
        {
            try
            {
                using var context = new PostgresContext();
                var toppings = await context.Toppings
                    .AsNoTracking()
                    .Where(t => t.IsAvailable != false)
                    .OrderBy(t => t.Name)
                    .ToListAsync();

                _allToppings.Clear();
                _allToppings.AddRange(toppings);
            }
            catch
            {
            }
        }

        private void pn_content_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InitializeOrdersScreen()
        {
            lblsohoadon.Text = $"HD-{DateTime.Now:yyMMddHHmm}";
            lbltrangthai.Text = "Đang tải";

            cbotrangthai.Items.Clear();
            cbotrangthai.DropDownStyle = ComboBoxStyle.DropDownList;

            SetupProductContainer();
            SetupCartContainer();
            SetupFilterEvents();
            SelectCategory(btnall, null);
            UpdateTotals();
        }

        private async Task LoadOrderTypeOptionsAsync()
        {
            var items = new List<string> { "Mang về", "Giao hàng" };
            var dbConnInfo = string.Empty;

            try
            {
                using var context = new PostgresContext();
                var conn = context.Database.GetDbConnection();
                dbConnInfo = $"Host: {conn.DataSource} | DB: {conn.Database}";
                var tableNames = await context.Tables
                    .AsNoTracking()
                    .OrderBy(t => t.Name)
                    .Select(t => t.Name)
                    .ToListAsync();

                items.AddRange(tableNames.Where(x => !string.IsNullOrWhiteSpace(x)));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được danh sách trạng thái/bàn.\n{ex.Message}\n{dbConnInfo}", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            cbotrangthai.BeginUpdate();
            cbotrangthai.Items.Clear();
            foreach (var item in items.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                cbotrangthai.Items.Add(item);
            }
            cbotrangthai.EndUpdate();

            var preferred = !string.IsNullOrWhiteSpace(_initialTableName)
                ? _initialTableName
                : _initialOrderSelection;

            if (!string.IsNullOrWhiteSpace(preferred))
            {
                var matched = cbotrangthai.Items
                    .Cast<object>()
                    .Select(x => x.ToString())
                    .FirstOrDefault(x => string.Equals(x, preferred, StringComparison.OrdinalIgnoreCase));

                if (matched != null)
                {
                    cbotrangthai.SelectedItem = matched;
                    return;
                }
            }

            if (cbotrangthai.Items.Count > 0)
            {
                cbotrangthai.SelectedIndex = 0;
            }
        }

        private static bool IsDeliverySelection(string? selection)
        {
            return string.Equals(selection, "Giao hàng", StringComparison.OrdinalIgnoreCase)
                || string.Equals(selection, "Mang về", StringComparison.OrdinalIgnoreCase);
        }

        private static async Task<Guid?> ResolveSelectedTableIdAsync(PostgresContext context, string? selection)
        {
            if (string.IsNullOrWhiteSpace(selection) || IsDeliverySelection(selection))
            {
                return null;
            }

            return await context.Tables
                .AsNoTracking()
                .Where(t => t.Name == selection)
                .Select(t => (Guid?)t.Id)
                .FirstOrDefaultAsync();
        }

        private void SetupProductContainer()
        {
            pn_content.Controls.Clear();

            _productsContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(8),
                BackColor = Color.White
            };

            pn_content.Controls.Add(_productsContainer);
        }

        private void SetupFilterEvents()
        {
            txtfind.TextChanged += (_, _) => RenderProducts();

            btnall.Click += (_, _) => SelectCategory(btnall, null);
            btncoffee.Click += (_, _) => SelectCategory(btncoffee, "coffee");
            btnmilktea.Click += (_, _) => SelectCategory(btnmilktea, "milktea");
            btnfruittea.Click += (_, _) => SelectCategory(btnfruittea, "fruittea");
            btnmilk.Click += (_, _) => SelectCategory(btnmilk, "milk");
            btnfrappe.Click += (_, _) => SelectCategory(btnfrappe, "frappe");
            btnpuretea.Click += (_, _) => SelectCategory(btnpuretea, "puretea");
        }

        private void SetupCartContainer()
        {
            pnGiohang.Controls.Clear();

            _cartContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(8),
                BackColor = Color.White
            };

            pnGiohang.Controls.Add(_cartContainer);
        }

        private void SelectCategory(Button selectedButton, string? categoryKey)
        {
            _selectedCategoryKey = categoryKey;

            var buttons = new[] { btnall, btncoffee, btnmilktea, btnfruittea, btnmilk, btnfrappe, btnpuretea };
            foreach (var button in buttons)
            {
                button.BackColor = Color.White;
                button.ForeColor = Color.Black;
                button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            }

            selectedButton.BackColor = Color.FromArgb(255, 238, 238);
            selectedButton.ForeColor = Color.FromArgb(220, 53, 69);
            selectedButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            RenderProducts();
        }

        private async System.Threading.Tasks.Task LoadProductsAsync()
        {
            try
            {
                using var context = new PostgresContext();
                var products = await context.Products
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Where(p => p.IsAvailable != false)
                    .OrderBy(p => p.Name)
                    .ToListAsync();

                _allProducts.Clear();
                _allProducts.AddRange(products);
                lbltrangthai.Text = $"{_allProducts.Count} món";
            }
            catch (Exception ex)
            {
                lbltrangthai.Text = "Lỗi tải";
                MessageBox.Show($"Không tải được sản phẩm.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderProducts()
        {
            if (_productsContainer == null) return;

            _productsContainer.SuspendLayout();
            _productsContainer.Controls.Clear();

            var filteredProducts = _allProducts
                .Where(MatchesSearch)
                .Where(MatchesCategory)
                .ToList();

            foreach (var product in filteredProducts)
            {
                _productsContainer.Controls.Add(CreateProductCard(product));
            }

            if (filteredProducts.Count == 0)
            {
                _productsContainer.Controls.Add(new Label
                {
                    Width = pn_content.Width - 40,
                    Height = 60,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = Color.DimGray,
                    Text = "Không có sản phẩm phù hợp"
                });
            }

            _productsContainer.ResumeLayout();
        }

        private bool MatchesSearch(Product product)
        {
            var keyword = NormalizeText(txtfind.Text.Trim());
            if (string.IsNullOrWhiteSpace(keyword)) return true;

            return NormalizeText(product.Name).Contains(keyword);
        }

        private bool MatchesCategory(Product product)
        {
            if (_selectedCategoryKey == null) return true;
            var categoryName = NormalizeText(product.Category?.Name ?? string.Empty);

            return _selectedCategoryKey switch
            {
                "coffee" => categoryName.Contains("ca phe") || categoryName.Contains("coffee"),
                "milktea" => categoryName.Contains("tra sua") || categoryName.Contains("milk tea"),
                "fruittea" => categoryName.Contains("tra trai cay") || categoryName.Contains("fruit tea"),
                "milk" => categoryName.Contains("sua tuoi") || categoryName.Contains("sua chua") || categoryName.Contains("yogurt"),
                "frappe" => categoryName.Contains("da xay") || categoryName.Contains("frappe"),
                "puretea" => categoryName.Contains("tra pure") || categoryName.Contains("pure tea"),
                _ => true
            };
        }

        private Panel CreateProductCard(Product product)
        {
            var card = new Panel
            {
                Width = 192,
                Height = 152,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Margin = new Padding(8)
            };

            var image = new PictureBox
            {
                Left = 10,
                Top = 10,
                Width = 170,
                Height = 62,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = LoadProductImage(product)
            };

            var name = new Label
            {
                Left = 10,
                Top = 78,
                Width = 170,
                Height = 40,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = product.Name
            };

            var price = new Label
            {
                Left = 10,
                Top = 120,
                Width = 170,
                Height = 22,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69),
                Text = $"{product.BasePrice:N0}đ"
            };

            card.Controls.Add(image);
            card.Controls.Add(name);
            card.Controls.Add(price);

            card.Cursor = Cursors.Hand;
            image.Cursor = Cursors.Hand;
            name.Cursor = Cursors.Hand;
            price.Cursor = Cursors.Hand;

            card.Click += async (_, _) => await OpenCustomizePopupAsync(product);
            image.Click += async (_, _) => await OpenCustomizePopupAsync(product);
            name.Click += async (_, _) => await OpenCustomizePopupAsync(product);
            price.Click += async (_, _) => await OpenCustomizePopupAsync(product);

            return card;
        }

        private async System.Threading.Tasks.Task OpenCustomizePopupAsync(Product product)
        {
            CloseCustomizePopup();

            var sizeOptions = await GetSizeOptionsAsync(product);
            var selectedSize = sizeOptions.FirstOrDefault();
            var selectedSugar = "100";
            var selectedIce = "100";
            var selectedToppings = new HashSet<Guid>();

            _popupOverlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(80, 0, 0, 0)
            };

            _popupSheet = new Panel
            {
                Width = this.ClientSize.Width - 24,
                Height = Math.Min(560, Math.Max(470, this.ClientSize.Height - 120)),
                Left = 12,
                Top = this.ClientSize.Height - Math.Min(560, Math.Max(470, this.ClientSize.Height - 120)) - 12,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            SetTopRoundedRegion(_popupSheet, 18);

            var lblTitle = new Label
            {
                Left = 16,
                Top = 14,
                Width = 300,
                Height = 28,
                Text = "Tùy chọn món",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            var lblProductName = new Label
            {
                Left = 16,
                Top = 44,
                Width = 420,
                Height = 26,
                Text = product.Name,
                Font = new Font("Segoe UI", 11F)
            };

            var y = 78;
            var lblSize = new Label { Left = 16, Top = y, Width = 200, Height = 24, Text = "Kích cỡ" };
            lblSize.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            y += 28;

            var flSize = new FlowLayoutPanel
            {
                Left = 16,
                Top = y,
                Width = _popupSheet.Width - 32,
                Height = 42,
                WrapContents = true,
                BackColor = Color.White
            };

            y += 48;
            var lblSugar = new Label { Left = 16, Top = y, Width = 200, Height = 24, Text = "Đường" };
            lblSugar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            y += 28;

            var flSugar = new FlowLayoutPanel
            {
                Left = 16,
                Top = y,
                Width = _popupSheet.Width - 32,
                Height = 42,
                WrapContents = true,
                BackColor = Color.White
            };

            y += 48;
            var lblIce = new Label { Left = 16, Top = y, Width = 200, Height = 24, Text = "Đá" };
            lblIce.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            y += 28;

            var flIce = new FlowLayoutPanel
            {
                Left = 16,
                Top = y,
                Width = _popupSheet.Width - 32,
                Height = 42,
                WrapContents = true,
                BackColor = Color.White
            };

            y += 46;
            var lblTop = new Label { Left = 16, Top = y, Width = 320, Height = 24, Text = "Topping thêm (+5.000đ / loại)" };
            lblTop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            y += 28;

            var buttonsTop = _popupSheet.Height - 52;
            var toppingHeight = Math.Max(120, buttonsTop - y - 10);

            var flTop = new FlowLayoutPanel
            {
                Left = 16,
                Top = y,
                Width = _popupSheet.Width - 32,
                Height = toppingHeight,
                WrapContents = true,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.White
            };

            var btnCancel = new Button
            {
                Left = 16,
                Top = buttonsTop,
                Width = 90,
                Height = 34,
                Text = "Hủy",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };

            var btnAdd = new Button
            {
                Left = 116,
                Top = buttonsTop,
                Width = _popupSheet.Width - 132,
                Height = 34,
                Text = "+ Thêm vào đơn",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };

            btnCancel.FlatAppearance.BorderColor = Color.Silver;
            btnAdd.FlatAppearance.BorderColor = Color.Silver;
            SetRoundedRegion(btnCancel, 8);
            SetRoundedRegion(btnAdd, 8);

            void SelectSingle(FlowLayoutPanel panel, Button selected)
            {
                foreach (Control c in panel.Controls)
                {
                    if (c is Button b)
                    {
                        SetChipStyle(b, b == selected);
                    }
                }
            }

            foreach (var s in sizeOptions)
            {
                var b = CreateChipButton($"{s.Label} - {FormatCurrency(s.Price)}");
                if (selectedSize?.Label == s.Label) SetChipStyle(b, true);
                b.Click += (_, _) =>
                {
                    selectedSize = s;
                    SelectSingle(flSize, b);
                };
                flSize.Controls.Add(b);
            }

            var sugars = new[] { "0", "30", "70", "100" };
            foreach (var sugar in sugars)
            {
                var b = CreateChipButton($"{sugar}%");
                if (sugar == selectedSugar) SetChipStyle(b, true);
                b.Click += (_, _) =>
                {
                    selectedSugar = sugar;
                    SelectSingle(flSugar, b);
                };
                flSugar.Controls.Add(b);
            }

            var iceOptions = new[]
            {
                ("0", "Không đá"),
                ("30", "Ít đá"),
                ("70", "Bình thường"),
                ("100", "Nhiều đá")
            };

            foreach (var ice in iceOptions)
            {
                var b = CreateChipButton(ice.Item2);
                if (ice.Item1 == selectedIce) SetChipStyle(b, true);
                b.Click += (_, _) =>
                {
                    selectedIce = ice.Item1;
                    SelectSingle(flIce, b);
                };
                flIce.Controls.Add(b);
            }

            foreach (var topping in _allToppings)
            {
                var b = CreateToppingChipButton(topping.Name);
                b.Click += (_, _) =>
                {
                    if (selectedToppings.Contains(topping.Id))
                    {
                        selectedToppings.Remove(topping.Id);
                        SetChipStyle(b, false);
                    }
                    else
                    {
                        selectedToppings.Add(topping.Id);
                        SetChipStyle(b, true);
                    }
                };
                flTop.Controls.Add(b);
            }

            btnCancel.Click += (_, _) => CloseCustomizePopup();
            _popupOverlay.Click += (_, _) => CloseCustomizePopup();
            _popupSheet.Click += (_, _) => { };
            btnAdd.Click += (_, _) =>
            {
                var selectedToppingModels = _allToppings
                    .Where(t => selectedToppings.Contains(t.Id))
                    .Select(t => new CartTopping { ToppingId = t.Id, Name = t.Name, Price = t.Price })
                    .ToList();

                AddConfiguredProductToCart(product, selectedSize, selectedSugar, selectedIce, selectedToppingModels);
                CloseCustomizePopup();
            };

            _popupSheet.Controls.Add(lblTitle);
            _popupSheet.Controls.Add(lblProductName);
            _popupSheet.Controls.Add(lblSize);
            _popupSheet.Controls.Add(flSize);
            _popupSheet.Controls.Add(lblSugar);
            _popupSheet.Controls.Add(flSugar);
            _popupSheet.Controls.Add(lblIce);
            _popupSheet.Controls.Add(flIce);
            _popupSheet.Controls.Add(lblTop);
            _popupSheet.Controls.Add(flTop);
            _popupSheet.Controls.Add(btnCancel);
            _popupSheet.Controls.Add(btnAdd);

            _popupOverlay.Controls.Add(_popupSheet);
            this.Controls.Add(_popupOverlay);
            _popupOverlay.BringToFront();
        }

        private void CloseCustomizePopup()
        {
            if (_popupOverlay != null)
            {
                this.Controls.Remove(_popupOverlay);
                _popupOverlay.Dispose();
                _popupOverlay = null;
                _popupSheet = null;
            }
        }

        private async System.Threading.Tasks.Task<List<SizeOption>> GetSizeOptionsAsync(Product product)
        {
            try
            {
                using var context = new PostgresContext();
                var prices = await context.ProductSizes
                    .AsNoTracking()
                    .Where(ps => ps.ProductId == product.Id)
                    .OrderBy(ps => ps.Price)
                    .Select(ps => ps.Price)
                    .ToListAsync();

                if (prices.Count > 0)
                {
                    var labels = new[] { "S", "M", "L" };
                    var result = new List<SizeOption>();
                    for (var i = 0; i < prices.Count && i < labels.Length; i++)
                    {
                        result.Add(new SizeOption { Label = labels[i], Price = prices[i] });
                    }
                    return result;
                }
            }
            catch
            {
            }

            return new List<SizeOption>
            {
                new SizeOption { Label = "M", Price = product.BasePrice }
            };
        }

        private static Button CreateChipButton(string text)
        {
            var measured = TextRenderer.MeasureText(text, new Font("Segoe UI", 9.5F, FontStyle.Regular));
            var width = Math.Max(90, measured.Width + 26);

            var button = new Button
            {
                AutoSize = false,
                Width = width,
                Height = 34,
                Padding = new Padding(12, 0, 12, 0),
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(242, 242, 238),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 8, 8)
            };

            button.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            button.FlatAppearance.BorderSize = 1;
            return button;
        }

        private static Button CreateToppingChipButton(string text)
        {
            var measured = TextRenderer.MeasureText(text, new Font("Segoe UI", 10F, FontStyle.Regular));
            var width = Math.Max(120, measured.Width + 34);

            return new Button
            {
                AutoSize = false,
                Width = width,
                Height = 36,
                Padding = new Padding(14, 0, 14, 0),
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(242, 242, 238),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 8, 8)
            };
        }

        private static void SetChipStyle(Button button, bool selected)
        {
            button.BackColor = selected ? Color.FromArgb(255, 249, 249) : Color.FromArgb(242, 242, 238);
            button.ForeColor = selected ? Color.FromArgb(220, 53, 69) : Color.Black;
            button.FlatAppearance.BorderColor = selected ? Color.FromArgb(220, 53, 69) : Color.Silver;
        }

        private static void SetTopRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;

            var path = new GraphicsPath();
            var diameter = radius * 2;
            var rect = new Rectangle(0, 0, control.Width - 1, control.Height - 1);

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom);
            path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
            path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + radius);
            path.CloseFigure();

            control.Region = new Region(path);
            control.Resize += (_, _) => SetTopRoundedRegion(control, radius);
        }

        private static void SetRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;

            var path = new GraphicsPath();
            var diameter = radius * 2;
            var rect = new Rectangle(0, 0, control.Width - 1, control.Height - 1);

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
            control.Resize += (_, _) => SetRoundedRegion(control, radius);
        }

        private void AddConfiguredProductToCart(Product product, SizeOption? selectedSize, string sugar, string ice, List<CartTopping> toppings)
        {
            var sizeLabel = selectedSize?.Label ?? "M";
            var unitPrice = selectedSize?.Price ?? product.BasePrice;

            var existing = _cartItems.FirstOrDefault(x =>
                x.ProductId == product.Id &&
                x.Size == sizeLabel &&
                x.Sugar == sugar &&
                x.Ice == ice &&
                x.Toppings.Select(t => t.ToppingId).OrderBy(id => id).SequenceEqual(toppings.Select(t => t.ToppingId).OrderBy(id => id)));

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    ItemId = Guid.NewGuid(),
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Size = sizeLabel,
                    Sugar = sugar,
                    Ice = ice,
                    UnitPrice = unitPrice,
                    Toppings = toppings,
                    Quantity = 1
                });
            }

            RenderCart();
            UpdateTotals();
        }

        private void RenderCart()
        {
            if (_cartContainer == null) return;

            _cartContainer.SuspendLayout();
            _cartContainer.Controls.Clear();

            foreach (var item in _cartItems.OrderBy(i => i.ProductName))
            {
                _cartContainer.Controls.Add(CreateCartRow(item));
            }

            _cartContainer.ResumeLayout();
        }

        private Control CreateCartRow(CartItem item)
        {
            var row = new Panel
            {
                Width = Math.Max(340, pnGiohang.ClientSize.Width - 30),
                Height = 56,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 8),
                BackColor = Color.White
            };

            var lblName = new Label
            {
                Left = 8,
                Top = 4,
                Width = 130,
                Height = 24,
                Text = item.ProductName,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            var lblOptions = new Label
            {
                Left = 8,
                Top = 28,
                Width = 190,
                Height = 24,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.DimGray,
                Text = item.OptionSummary
            };

            var btnMinus = new Button
            {
                Left = 145,
                Top = 13,
                Width = 28,
                Height = 28,
                Text = "-"
            };

            var lblQty = new Label
            {
                Left = 178,
                Top = 13,
                Width = 30,
                Height = 28,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = item.Quantity.ToString()
            };

            var btnPlus = new Button
            {
                Left = 213,
                Top = 13,
                Width = 28,
                Height = 28,
                Text = "+"
            };

            var lblLineTotal = new Label
            {
                Left = 248,
                Top = 13,
                Width = 70,
                Height = 28,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69),
                Text = FormatCurrency(item.LineTotal)
            };

            var btnRemove = new Button
            {
                Left = 322,
                Top = 13,
                Width = 28,
                Height = 28,
                Text = "x"
            };

            btnMinus.Click += (_, _) => ChangeItemQuantity(item.ItemId, -1);
            btnPlus.Click += (_, _) => ChangeItemQuantity(item.ItemId, 1);
            btnRemove.Click += (_, _) => RemoveItem(item.ItemId);

            row.Controls.Add(lblName);
            row.Controls.Add(lblOptions);
            row.Controls.Add(btnMinus);
            row.Controls.Add(lblQty);
            row.Controls.Add(btnPlus);
            row.Controls.Add(lblLineTotal);
            row.Controls.Add(btnRemove);

            return row;
        }

        private void ChangeItemQuantity(Guid itemId, int delta)
        {
            var item = _cartItems.FirstOrDefault(x => x.ItemId == itemId);
            if (item == null) return;

            item.Quantity += delta;
            if (item.Quantity <= 0)
            {
                _cartItems.Remove(item);
            }

            RenderCart();
            UpdateTotals();
        }

        private void RemoveItem(Guid itemId)
        {
            var item = _cartItems.FirstOrDefault(x => x.ItemId == itemId);
            if (item != null)
            {
                _cartItems.Remove(item);
                RenderCart();
                UpdateTotals();
            }
        }

        private void UpdateTotals()
        {
            var totals = CalculateCurrentTotals();
            _voucherDiscount = totals.VoucherDiscount;

            lblTamtinh.Text = FormatCurrency(totals.Subtotal);
            lblGiamGia.Text = $"-{FormatCurrency(totals.Discount + totals.VoucherDiscount)}";
            lblDiemTichLuy.Text = _selectedMembership?.Points?.ToString("N0") ?? "0";
            lblTongThanhToan.Text = FormatCurrency(totals.Total);
        }

        private (decimal Subtotal, decimal Discount, decimal PointsDiscount, decimal VoucherDiscount, decimal Total) CalculateCurrentTotals()
        {
            var subtotal = _cartItems.Sum(x => x.LineTotal);
            var discount = 0m;
            var pointsDiscount = 0m;
            var voucherDiscount = CalculateVoucherDiscount(subtotal, _appliedVoucher);
            var total = Math.Max(0m, subtotal - discount - voucherDiscount - pointsDiscount);

            return (subtotal, discount, pointsDiscount, voucherDiscount, total);
        }

        private static string FormatCurrency(decimal value) => $"{value:N0}đ";

        private Bitmap LoadProductImage(Product product)
        {
            try
            {
                var categoryKey = GetCategoryKey(product.Category?.Name);
                var files = GetCategoryImageFiles(categoryKey);
                if (files.Count > 0)
                {
                    var index = Math.Abs(product.Id.GetHashCode()) % files.Count;
                    return LoadImageFromFile(files[index]);
                }

                if (!string.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    var normalizedPath = product.ImageUrl.Trim().TrimStart('/', '\\');
                    var projectPath = GetProjectPath();
                    var fullPath = Path.IsPathRooted(normalizedPath)
                        ? normalizedPath
                        : Path.Combine(projectPath, normalizedPath);

                    if (File.Exists(fullPath))
                    {
                        return LoadImageFromFile(fullPath);
                    }
                }
            }
            catch
            {
            }

            _placeholderImage ??= CreatePlaceholderImage();
            return _placeholderImage;
        }

        private Bitmap LoadImageFromFile(string fullPath)
        {
            if (_imageCache.TryGetValue(fullPath, out var cached))
            {
                return cached;
            }

            using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            using var image = Image.FromStream(stream);
            var bmp = new Bitmap(image);
            _imageCache[fullPath] = bmp;
            return bmp;
        }

        private List<string> GetCategoryImageFiles(string categoryKey)
        {
            if (_categoryImageFiles.TryGetValue(categoryKey, out var existing))
            {
                return existing;
            }

            var results = new List<string>();
            var root = Path.Combine(GetProjectPath(), "images", "categories");
            var categoryFolder = Path.Combine(root, categoryKey);

            if (Directory.Exists(categoryFolder))
            {
                results = Directory.GetFiles(categoryFolder)
                    .Where(IsImageFile)
                    .OrderBy(x => x)
                    .ToList();
            }
            else if (Directory.Exists(root))
            {
                results = Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                    .Where(IsImageFile)
                    .Where(x => NormalizeText(Path.GetFileNameWithoutExtension(x)).Contains(categoryKey))
                    .OrderBy(x => x)
                    .ToList();
            }

            _categoryImageFiles[categoryKey] = results;
            return results;
        }

        private static bool IsImageFile(string file)
        {
            var ext = Path.GetExtension(file).ToLowerInvariant();
            return ext is ".jpg" or ".jpeg" or ".png" or ".webp" or ".bmp";
        }

        private static string GetCategoryKey(string? categoryName)
        {
            var normalized = NormalizeText(categoryName ?? string.Empty);

            if (normalized.Contains("ca phe") || normalized.Contains("coffee")) return "coffee";
            if (normalized.Contains("tra sua") || normalized.Contains("milk tea")) return "milktea";
            if (normalized.Contains("tra trai cay") || normalized.Contains("fruit tea")) return "fruittea";
            if (normalized.Contains("sua tuoi") || normalized.Contains("sua chua") || normalized.Contains("yogurt")) return "milk";
            if (normalized.Contains("da xay") || normalized.Contains("frappe")) return "frappe";
            if (normalized.Contains("tra pure") || normalized.Contains("pure tea")) return "puretea";

            return "default";
        }

        private static string GetProjectPath()
        {
            var current = AppDomain.CurrentDomain.BaseDirectory;
            while (!string.IsNullOrEmpty(current) && !File.Exists(Path.Combine(current, "MilkTeaPOS.csproj")))
            {
                current = Directory.GetParent(current)?.FullName;
            }

            return current ?? AppDomain.CurrentDomain.BaseDirectory;
        }

        private static Bitmap CreatePlaceholderImage()
        {
            var bmp = new Bitmap(170, 62);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(245, 245, 245));

            using var border = new Pen(Color.FromArgb(220, 220, 220));
            g.DrawRectangle(border, 0, 0, 169, 61);

            using var brush = new SolidBrush(Color.Gray);
            using var font = new Font("Segoe UI", 9F);
            var text = "No Image";
            var size = g.MeasureString(text, font);
            g.DrawString(text, font, brush, (170 - size.Width) / 2, (62 - size.Height) / 2);

            return bmp;
        }

        private static string NormalizeText(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            var normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch == 'đ' ? 'd' : ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private sealed class CartItem
        {
            public Guid ItemId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public string Size { get; set; } = "M";
            public string Sugar { get; set; } = "100";
            public string Ice { get; set; } = "100";
            public List<CartTopping> Toppings { get; set; } = new();
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal ToppingTotal => Toppings.Sum(t => t.Price);
            public decimal LineTotal => (UnitPrice + ToppingTotal) * Quantity;
            public string OptionSummary => $"{Size} - đường {Sugar}% - đá {Ice}%";
        }

        private sealed class CartTopping
        {
            public Guid ToppingId { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
        }

        private sealed class SizeOption
        {
            public string Label { get; set; } = "M";
            public decimal Price { get; set; }
        }

        private sealed class CheckoutSnapshotItem
        {
            public Guid ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal ToppingTotal { get; set; }
            public decimal LineTotal { get; set; }
            public List<CheckoutSnapshotTopping> Toppings { get; set; } = new();
        }

        private sealed class CheckoutSnapshotTopping
        {
            public Guid ToppingId { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
        }

        private sealed class VoucherSnapshot
        {
            public Guid Id { get; set; }
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string VoucherType { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public decimal DiscountValue { get; set; }
            public decimal MinOrderAmount { get; set; }
            public decimal? MaxDiscountAmount { get; set; }
            public int? UsageLimit { get; set; }
            public int UsageCount { get; set; }
            public DateTime? ValidFrom { get; set; }
            public DateTime? ValidUntil { get; set; }
            public HashSet<string> ApplicableTiers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        }

        private decimal CalculateVoucherDiscount(decimal subtotal, VoucherSnapshot? voucher)
        {
            if (voucher == null || subtotal <= 0) return 0m;

            var nowUtc = DateTime.UtcNow;
            if (!string.Equals(voucher.Status, "active", StringComparison.OrdinalIgnoreCase)) return 0m;
            if (voucher.ValidFrom.HasValue && voucher.ValidFrom.Value > nowUtc) return 0m;
            if (voucher.ValidUntil.HasValue && voucher.ValidUntil.Value < nowUtc) return 0m;
            if (subtotal < voucher.MinOrderAmount) return 0m;
            if (voucher.UsageLimit.HasValue && voucher.UsageCount >= voucher.UsageLimit.Value) return 0m;

            var tier = ResolveCurrentTier();
            if (voucher.ApplicableTiers.Count > 0 && !voucher.ApplicableTiers.Contains(tier)) return 0m;

            return voucher.VoucherType switch
            {
                "percentage" => Math.Min(subtotal * (voucher.DiscountValue / 100m), voucher.MaxDiscountAmount ?? subtotal),
                "fixed_amount" => Math.Min(voucher.DiscountValue, subtotal),
                "free_item" => CalculateFreeItemDiscount(subtotal),
                "buy_one_get_one" => CalculateBogoDiscount(subtotal),
                _ => 0m
            };
        }

        private decimal CalculateFreeItemDiscount(decimal subtotal)
        {
            var minToppingPrice = _cartItems
                .SelectMany(i => i.Toppings)
                .Select(t => t.Price)
                .DefaultIfEmpty(0m)
                .Min();

            return Math.Min(minToppingPrice, subtotal);
        }

        private decimal CalculateBogoDiscount(decimal subtotal)
        {
            var discount = _cartItems
                .Where(i => NormalizeText(i.ProductName).Contains("tra sua truyen thong"))
                .Sum(i => Math.Floor(i.Quantity / 2m) * i.UnitPrice);

            return Math.Min(discount, subtotal);
        }

        private string ResolveCurrentTier()
        {
            var spent = _selectedMembership?.TotalSpent ?? 0m;

            if (spent >= 5000000m) return "diamond";
            if (spent >= 3000000m) return "platinum";
            if (spent >= 1500000m) return "gold";
            if (spent >= 500000m) return "silver";
            return "none";
        }

        private static HashSet<string> ParseTierArray(string? value)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(value)) return result;

            var trimmed = value.Trim().Trim('{', '}');
            if (string.IsNullOrWhiteSpace(trimmed)) return result;

            foreach (var part in trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                result.Add(part.Trim('"'));
            }

            return result;
        }

        private static T? GetNullableValue<T>(DbDataReader reader, string name) where T : struct
        {
            var ordinal = reader.GetOrdinal(name);
            if (reader.IsDBNull(ordinal)) return null;
            return reader.GetFieldValue<T>(ordinal);
        }

        private static string GetStringValue(DbDataReader reader, string name)
        {
            var ordinal = reader.GetOrdinal(name);
            if (reader.IsDBNull(ordinal)) return string.Empty;
            return reader.GetString(ordinal);
        }

        private bool TryLoadVoucher(string voucherCode, out VoucherSnapshot? voucher, out string errorMessage)
        {
            voucher = null;
            errorMessage = string.Empty;

            try
            {
                using var context = new PostgresContext();
                var connection = context.Database.GetDbConnection();
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                using var command = connection.CreateCommand();
                command.CommandText = @"
SELECT
    id,
    code,
    name,
    voucher_type::text AS voucher_type,
    status::text AS status,
    discount_value,
    min_order_amount,
    max_discount_amount,
    usage_limit,
    usage_count,
    valid_from,
    valid_until,
    applicable_tiers::text AS applicable_tiers
FROM vouchers
WHERE UPPER(code) = @code
LIMIT 1;";

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@code";
                parameter.Value = voucherCode.ToUpperInvariant();
                command.Parameters.Add(parameter);

                using var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    errorMessage = "Không tìm thấy voucher.";
                    return false;
                }

                voucher = new VoucherSnapshot
                {
                    Id = reader.GetFieldValue<Guid>(reader.GetOrdinal("id")),
                    Code = GetStringValue(reader, "code"),
                    Name = GetStringValue(reader, "name"),
                    VoucherType = GetStringValue(reader, "voucher_type"),
                    Status = GetStringValue(reader, "status"),
                    DiscountValue = reader.GetFieldValue<decimal>(reader.GetOrdinal("discount_value")),
                    MinOrderAmount = reader.GetFieldValue<decimal>(reader.GetOrdinal("min_order_amount")),
                    MaxDiscountAmount = GetNullableValue<decimal>(reader, "max_discount_amount"),
                    UsageLimit = GetNullableValue<int>(reader, "usage_limit"),
                    UsageCount = reader.GetFieldValue<int>(reader.GetOrdinal("usage_count")),
                    ValidFrom = GetNullableValue<DateTime>(reader, "valid_from"),
                    ValidUntil = GetNullableValue<DateTime>(reader, "valid_until"),
                    ApplicableTiers = ParseTierArray(GetStringValue(reader, "applicable_tiers"))
                };

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Không thể tải voucher. {ex.Message}";
                return false;
            }
        }

        private void btnApdungvoucher_Click(object sender, EventArgs e)
        {
            // Khi thử áp mã mới, luôn bỏ mã cũ trước để tránh dính giảm giá trước đó
            _appliedVoucher = null;
            _voucherDiscount = 0m;
            UpdateTotals();

            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm trước khi áp dụng voucher.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var code = textBox2.Text.Trim();
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Vui lòng nhập mã voucher.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!TryLoadVoucher(code, out var voucher, out var error))
            {
                MessageBox.Show(error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var subtotal = _cartItems.Sum(x => x.LineTotal);
            var discount = CalculateVoucherDiscount(subtotal, voucher);
            if (discount <= 0m)
            {
                MessageBox.Show("Voucher không áp dụng được cho đơn hiện tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _appliedVoucher = voucher;
            UpdateTotals();

            MessageBox.Show($"Áp dụng voucher {_appliedVoucher.Code} thành công. Giảm {FormatCurrency(_voucherDiscount)}.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnThemMember_Click(object sender, EventArgs e)
        {
            var phone = txtSDT.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using var context = new PostgresContext();
                using var transaction = context.Database.BeginTransaction();

                var normalizedPhone = NormalizePhoneNumber(phone);
                if (string.IsNullOrWhiteSpace(normalizedPhone))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var customer = context.Customers
                    .FirstOrDefault(c => c.Phone == normalizedPhone);

                var isNewCustomer = false;
                var isNewMembership = false;

                if (customer == null)
                {
                    isNewCustomer = true;
                    customer = new Customer
                    {
                        Id = Guid.NewGuid(),
                        Name = $"KH {normalizedPhone}",
                        Phone = normalizedPhone,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.Customers.Add(customer);
                    context.SaveChanges();
                }

                var membership = context.Memberships
                    .FirstOrDefault(m => m.CustomerId == customer.Id);

                if (membership == null)
                {
                    isNewMembership = true;
                    membership = new Membership
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customer.Id,
                        Points = 0,
                        TotalSpent = 0,
                        TotalOrders = 0,
                        JoinedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.Memberships.Add(membership);
                }

                context.SaveChanges();
                transaction.Commit();

                _selectedCustomer = customer;
                _selectedMembership = membership;
                UpdateTotals();

                var message = isNewCustomer || isNewMembership
                    ? "Đã tạo thẻ thành viên cho khách hàng."
                    : "Khách hàng đã có thẻ thành viên.";

                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Không thể thêm thành viên.\n{details}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimMember_Click(object sender, EventArgs e)
        {
            var phone = txtSDT.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại để tìm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using var context = new PostgresContext();

                var normalizedPhone = NormalizePhoneNumber(phone);

                var query = from c in context.Customers
                            join m in context.Memberships on c.Id equals m.CustomerId
                            where c.Phone == normalizedPhone
                            select new { Customer = c, Membership = m };

                var result = query.FirstOrDefault();

                if (result == null)
                {
                    _selectedCustomer = null;
                    _selectedMembership = null;
                    UpdateTotals();
                    MessageBox.Show("Không tìm thấy thành viên với số điện thoại này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _selectedCustomer = result.Customer;
                _selectedMembership = result.Membership;
                UpdateTotals();

                MessageBox.Show($"Đã tải thành viên: {_selectedCustomer.Name} - Điểm: {_selectedMembership.Points:N0}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tìm thành viên.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnGiuhoadon_Click(object sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống, không có đơn để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var totals = CalculateCurrentTotals();
            var cartSnapshot = _cartItems
                .Select(i => new CheckoutSnapshotItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    ToppingTotal = i.ToppingTotal,
                    LineTotal = i.LineTotal,
                    Toppings = i.Toppings.Select(t => new CheckoutSnapshotTopping
                    {
                        ToppingId = t.ToppingId,
                        Name = t.Name,
                        Price = t.Price
                    }).ToList()
                })
                .ToList();

            try
            {
                var saveResult = await PersistOrderAsync(
                    isHoldOrder: true,
                    cartSnapshot: cartSnapshot,
                    totals: totals,
                    receivedAmount: 0m,
                    paymentMethod: null);

                if (!saveResult.Success)
                {
                    MessageBox.Show($"Không thể lưu đơn giữ.\n{saveResult.ErrorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _cartItems.Clear();
                _appliedVoucher = null;
                _voucherDiscount = 0m;
                textBox2.Clear();
                lblsohoadon.Text = $"HD-{DateTime.Now:yyMMddHHmm}";
                RenderCart();
                UpdateTotals();

                MessageBox.Show("Đã lưu đơn giữ thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                try
                {
                    await OpenOrderHistoryFormAsync();
                }
                catch (Exception exOpen)
                {
                    MessageBox.Show($"Đã lưu đơn giữ nhưng không mở được lịch sử đơn.\n{exOpen.Message}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Không thể lưu đơn giữ.\n{details}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThanhtoan_Click(object sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var totals = CalculateCurrentTotals();
            if (totals.Total <= 0m)
            {
                MessageBox.Show("Tổng thanh toán không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var paymentLines = _cartItems.Select(i => new frmThanhtoan.PaymentLineItem
            {
                ProductName = i.ProductName,
                Options = i.OptionSummary,
                Toppings = i.Toppings.Select(t => t.Name).ToList(),
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice + i.ToppingTotal,
                LineTotal = i.LineTotal
            }).ToList();

            using var paymentForm = new frmThanhtoan(
                totals.Total,
                paymentLines,
                _selectedCustomer?.Phone,
                _selectedCustomer?.Name,
                _selectedMembership?.Points ?? 0,
                _appliedVoucher?.Code);

            if (paymentForm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (paymentForm.ReceivedAmount < totals.Total)
            {
                MessageBox.Show("Số tiền nhận chưa đủ để thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cartSnapshot = _cartItems
                .Select(i => new CheckoutSnapshotItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    ToppingTotal = i.ToppingTotal,
                    LineTotal = i.LineTotal,
                    Toppings = i.Toppings.Select(t => new CheckoutSnapshotTopping
                    {
                        ToppingId = t.ToppingId,
                        Name = t.Name,
                        Price = t.Price
                    }).ToList()
                })
                .ToList();

            try
            {
                var saveResult = await PersistOrderAsync(
                    isHoldOrder: false,
                    cartSnapshot: cartSnapshot,
                    totals: totals,
                    receivedAmount: paymentForm.ReceivedAmount,
                    paymentMethod: paymentForm.SelectedPaymentMethod);

                if (!saveResult.Success)
                {
                    MessageBox.Show($"Thanh toán thất bại.\n{saveResult.ErrorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var completedOrderNumber = saveResult.OrderNumber ?? string.Empty;
                var changeAmount = saveResult.ChangeAmount;

                if (paymentForm.ShouldPrintReceipt)
                {
                    PrintCashReceipt(completedOrderNumber, cartSnapshot, totals, paymentForm.ReceivedAmount, changeAmount);
                }

                _cartItems.Clear();
                _appliedVoucher = null;
                _voucherDiscount = 0m;
                textBox2.Clear();
                if (_selectedCustomer != null)
                {
                    using var reloadContext = new PostgresContext();
                    _selectedMembership = await reloadContext.Memberships
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.CustomerId == _selectedCustomer.Id);
                }
                lblsohoadon.Text = $"HD-{DateTime.Now:yyMMddHHmm}";
                RenderCart();
                UpdateTotals();

                MessageBox.Show($"Thanh toán thành công. Mã đơn: {completedOrderNumber}.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                try
                {
                    await OpenOrderHistoryFormAsync();
                }
                catch (Exception exOpen)
                {
                    MessageBox.Show($"Đã thanh toán thành công nhưng không mở được lịch sử đơn.\n{exOpen.Message}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Thanh toán thất bại.\n{details}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Task<(bool Success, string? OrderNumber, decimal ChangeAmount, string? ErrorMessage)> PersistOrderAsync(
            bool isHoldOrder,
            List<CheckoutSnapshotItem> cartSnapshot,
            (decimal Subtotal, decimal Discount, decimal PointsDiscount, decimal VoucherDiscount, decimal Total) totals,
            decimal receivedAmount,
            string? paymentMethod)
        {
            var session = Guid.NewGuid().ToString("N")[..8];

            try
            {
                using var context = new PostgresContext();
                LogOrderStep(session, "START", "open context");
                context.Database.AutoSavepointsEnabled = false;

                var now = DateTime.UtcNow;
                var orderSelection = cbotrangthai.SelectedItem?.ToString();
                var isDelivery = IsDeliverySelection(orderSelection);
                Guid? tableId = null;
                if (!string.IsNullOrWhiteSpace(orderSelection) && !IsDeliverySelection(orderSelection))
                {
                    tableId = context.Tables
                        .AsNoTracking()
                        .Where(t => t.Name == orderSelection)
                        .Select(t => (Guid?)t.Id)
                        .FirstOrDefault();
                }
                LogOrderStep(session, "TX", "skip explicit transaction");

                LogOrderStep(session, "BEGIN", $"isHold={isHoldOrder}, items={cartSnapshot.Count}, total={totals.Total:N0}");

                var orderId = Guid.NewGuid();
                context.Database.ExecuteSqlInterpolated($@"
INSERT INTO orders (
    id, table_id, customer_id, customer_name, customer_phone,
    subtotal, discount, voucher_discount, membership_discount, total_amount,
    is_delivery, notes, created_at, updated_at
)
VALUES (
    {orderId}, {tableId}, {_selectedCustomer?.Id}, {_selectedCustomer?.Name}, {_selectedCustomer?.Phone},
    {totals.Subtotal}, {totals.Discount}, {totals.VoucherDiscount}, {totals.PointsDiscount}, {totals.Total},
    {isDelivery}, { (isHoldOrder ? "Đơn giữ" : null) }, {now}, {now}
);
");
                LogOrderStep(session, "ORDER", $"saved orderId={orderId}");

                var detailPairs = new List<(Guid DetailId, CheckoutSnapshotItem CartItem)>();
                foreach (var cartItem in cartSnapshot)
                {
                    var detailId = Guid.NewGuid();
                    detailPairs.Add((detailId, cartItem));

                    context.Database.ExecuteSqlInterpolated($@"
INSERT INTO order_details (
    id, order_id, product_id, product_name, quantity,
    unit_price, topping_total, subtotal, created_at, updated_at
)
VALUES (
    {detailId}, {orderId}, {cartItem.ProductId}, {cartItem.ProductName}, {cartItem.Quantity},
    {cartItem.UnitPrice}, {cartItem.ToppingTotal}, {cartItem.LineTotal}, {now}, {now}
);
");
                }

                LogOrderStep(session, "DETAIL", $"saved details={detailPairs.Count}");

                foreach (var pair in detailPairs)
                {
                    foreach (var topping in pair.CartItem.Toppings)
                    {
                        context.Database.ExecuteSqlInterpolated($@"
INSERT INTO order_toppings (
    id, order_detail_id, topping_id, topping_name, quantity, price, created_at
)
VALUES (
    {Guid.NewGuid()}, {pair.DetailId}, {topping.ToppingId}, {topping.Name}, {1}, {topping.Price}, {now}
);
");
                    }
                }

                LogOrderStep(session, "TOPPING", "saved toppings");

                if (_selectedCustomer != null)
                {
                    context.Database.ExecuteSqlInterpolated($@"
INSERT INTO memberships (
    id, customer_id, points, total_spent, total_orders, joined_at, updated_at
)
SELECT {Guid.NewGuid()}, {_selectedCustomer.Id}, 0, 0, 0, {now}, {now}
WHERE NOT EXISTS (
    SELECT 1 FROM memberships WHERE customer_id = {_selectedCustomer.Id}
);
");
                    LogOrderStep(session, "MEMBER", "membership upsert checked");
                }

                var changeAmount = 0m;
                if (!isHoldOrder)
                {
                    var effectivePaymentMethod = string.IsNullOrWhiteSpace(paymentMethod) ? "cash" : paymentMethod;
                    changeAmount = Math.Max(0m, receivedAmount - totals.Total);
                    context.Database.ExecuteSqlInterpolated($@"
INSERT INTO payments (
    id, order_id, method, received_amount, paid_amount, change_amount,
    status, transaction_id, created_at, paid_at, updated_at, payment_info
)
VALUES (
    {Guid.NewGuid()}, {orderId}, {effectivePaymentMethod}::payment_method, {receivedAmount}, {totals.Total}, {changeAmount},
    {"completed"}::payment_status, {Guid.NewGuid().ToString("N")}, {now}, {now}, {now}, {"POS checkout"}
);
");

                    context.Database.ExecuteSqlInterpolated($@"
UPDATE orders
SET status = {"served"}::order_status,
    served_at = {now},
    updated_at = {now}
WHERE id = {orderId};
");
                    LogOrderStep(session, "PAYMENT", $"saved payment method={paymentMethod}");

                    if (_appliedVoucher != null && totals.VoucherDiscount > 0m)
                    {
                        try
                        {
                            context.Database.ExecuteSqlInterpolated($@"
INSERT INTO voucher_usages (id, voucher_id, customer_id, order_id, discount_amount, used_at)
VALUES ({Guid.NewGuid()}, {_appliedVoucher.Id}, {_selectedCustomer?.Id}, {orderId}, {totals.VoucherDiscount}, {now});
");
                        }
                        catch (PostgresException ex) when (ex.SqlState == "42P01")
                        {
                            LogOrderStep(session, "VOUCHER-WARN", "voucher_usages table not found, skip usage history");
                        }

                        context.Database.ExecuteSqlInterpolated($@"
UPDATE vouchers
SET usage_count = usage_count + 1,
    updated_at = {now}
WHERE id = {_appliedVoucher.Id};
");
                        LogOrderStep(session, "VOUCHER", "voucher usage saved");
                    }
                }

                var completedOrderNumber = orderId.ToString()[..8].ToUpperInvariant();
                LogOrderStep(session, "COMMIT", $"success orderNo={completedOrderNumber}");

                return Task.FromResult((true, completedOrderNumber, changeAmount, (string?)null));
            }
            catch (PostgresException pgEx)
            {
                var error = $"[{pgEx.SqlState}] {pgEx.MessageText} | Detail: {pgEx.Detail}";
                LogOrderStep(session, "ERROR-POSTGRES", error);
                return Task.FromResult((false, (string?)null, 0m, error));
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.Message ?? ex.Message;
                LogOrderStep(session, "ERROR", ex.ToString());
                return Task.FromResult((false, (string?)null, 0m, error));
            }
        }

        private static void LogOrderStep(string session, string step, string message)
        {
            try
            {
                var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(logDir);
                var file = Path.Combine(logDir, "order-save.log");
                var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{session}] [{step}] {message}{Environment.NewLine}";
                File.AppendAllText(file, line);
            }
            catch
            {
            }
        }

        private async Task OpenOrderHistoryFormAsync()
        {
            using var history = new frmOrderHistory();
            var result = history.ShowDialog(this);
            if (result == DialogResult.OK && history.ResumeOrderId.HasValue)
            {
                await LoadHeldOrderToCurrentCartAsync(history.ResumeOrderId.Value);
            }
        }

        private async Task LoadHeldOrderToCurrentCartAsync(Guid orderId)
        {
            try
            {
                using var context = new PostgresContext();
                var holdOrder = await context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderId);
                if (holdOrder == null)
                {
                    MessageBox.Show("Không tìm thấy đơn giữ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var connection = context.Database.GetDbConnection();
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await using (var statusCommand = connection.CreateCommand())
                {
                    statusCommand.CommandText = "SELECT status::text FROM orders WHERE id = @orderId LIMIT 1;";
                    var pStatusOrderId = statusCommand.CreateParameter();
                    pStatusOrderId.ParameterName = "@orderId";
                    pStatusOrderId.Value = orderId;
                    statusCommand.Parameters.Add(pStatusOrderId);

                    var status = (await statusCommand.ExecuteScalarAsync())?.ToString() ?? string.Empty;
                    if (status is not ("pending" or "preparing" or "ready"))
                    {
                        MessageBox.Show("Đơn này không còn ở trạng thái đơn giữ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                var loadedItems = new List<CartItem>();
                var details = new List<(Guid DetailId, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, string Size, string Sugar, string Ice)>();

                await using (var detailCommand = connection.CreateCommand())
                {
                    detailCommand.CommandText = @"
SELECT
    od.id,
    od.product_id,
    od.product_name,
    COALESCE(od.quantity, 1),
    COALESCE(od.unit_price, 0),
    COALESCE(od.size::text, 'M'),
    COALESCE(od.sugar::text, '100'),
    COALESCE(od.ice::text, '100')
FROM order_details od
WHERE od.order_id = @orderId
ORDER BY od.created_at;";

                    var pOrderId = detailCommand.CreateParameter();
                    pOrderId.ParameterName = "@orderId";
                    pOrderId.Value = orderId;
                    detailCommand.Parameters.Add(pOrderId);

                    await using var detailReader = await detailCommand.ExecuteReaderAsync();
                    while (await detailReader.ReadAsync())
                    {
                        details.Add((
                            detailReader.GetGuid(0),
                            detailReader.GetGuid(1),
                            detailReader.GetString(2),
                            detailReader.GetInt32(3),
                            detailReader.GetDecimal(4),
                            detailReader.GetString(5),
                            detailReader.GetString(6),
                            detailReader.GetString(7)));
                    }
                }

                foreach (var detail in details)
                {
                    var toppings = new List<CartTopping>();
                    await using var toppingCommand = connection.CreateCommand();
                    toppingCommand.CommandText = @"
SELECT topping_id, topping_name, COALESCE(price, 0), COALESCE(quantity, 1)
FROM order_toppings
WHERE order_detail_id = @detailId
ORDER BY created_at;";

                    var pDetailId = toppingCommand.CreateParameter();
                    pDetailId.ParameterName = "@detailId";
                    pDetailId.Value = detail.DetailId;
                    toppingCommand.Parameters.Add(pDetailId);

                    await using var toppingReader = await toppingCommand.ExecuteReaderAsync();
                    while (await toppingReader.ReadAsync())
                    {
                        var toppingId = toppingReader.GetGuid(0);
                        var toppingName = toppingReader.GetString(1);
                        var toppingPrice = toppingReader.GetDecimal(2);
                        var toppingQuantity = toppingReader.GetInt32(3);

                        for (var i = 0; i < Math.Max(1, toppingQuantity); i++)
                        {
                            toppings.Add(new CartTopping
                            {
                                ToppingId = toppingId,
                                Name = toppingName,
                                Price = toppingPrice
                            });
                        }
                    }

                    loadedItems.Add(new CartItem
                    {
                        ItemId = Guid.NewGuid(),
                        ProductId = detail.ProductId,
                        ProductName = detail.ProductName,
                        Size = string.IsNullOrWhiteSpace(detail.Size) ? "M" : detail.Size,
                        Sugar = string.IsNullOrWhiteSpace(detail.Sugar) ? "100" : detail.Sugar,
                        Ice = string.IsNullOrWhiteSpace(detail.Ice) ? "100" : detail.Ice,
                        UnitPrice = detail.UnitPrice,
                        Quantity = detail.Quantity,
                        Toppings = toppings
                    });
                }

                _cartItems.Clear();
                _cartItems.AddRange(loadedItems);

                _selectedCustomer = null;
                _selectedMembership = null;
                if (holdOrder.CustomerId.HasValue)
                {
                    _selectedCustomer = await context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == holdOrder.CustomerId.Value);
                    _selectedMembership = await context.Memberships.AsNoTracking().FirstOrDefaultAsync(m => m.CustomerId == holdOrder.CustomerId.Value);
                }

                txtSDT.Text = _selectedCustomer?.Phone ?? string.Empty;
                _appliedVoucher = null;
                _voucherDiscount = 0m;
                textBox2.Clear();

                if (holdOrder.IsDelivery == true)
                {
                    cbotrangthai.SelectedItem = "Mang về";
                }
                else if (holdOrder.TableId.HasValue)
                {
                    var tableName = await context.Tables
                        .AsNoTracking()
                        .Where(t => t.Id == holdOrder.TableId.Value)
                        .Select(t => t.Name)
                        .FirstOrDefaultAsync();

                    if (!string.IsNullOrWhiteSpace(tableName))
                    {
                        cbotrangthai.SelectedItem = tableName;
                    }
                }
                if (!string.IsNullOrWhiteSpace(holdOrder.OrderNumber))
                {
                    lblsohoadon.Text = holdOrder.OrderNumber;
                }

                await context.Database.ExecuteSqlInterpolatedAsync($@"
DELETE FROM orders
WHERE id = {orderId}
  AND status IN ('pending', 'preparing', 'ready');
");

                RenderCart();
                UpdateTotals();
                MessageBox.Show("Đã nạp đơn giữ để tiếp tục gọi món và xóa đơn giữ cũ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể nạp đơn giữ.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string NormalizePhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return string.Empty;

            var sb = new StringBuilder();
            foreach (var ch in phone)
            {
                if (char.IsDigit(ch)) sb.Append(ch);
            }

            return sb.ToString();
        }

        private void PrintCashReceipt(string orderNumber, List<CheckoutSnapshotItem> cartSnapshot, (decimal Subtotal, decimal Discount, decimal PointsDiscount, decimal VoucherDiscount, decimal Total) totals, decimal receivedAmount, decimal changeAmount)
        {
            try
            {
                var lines = new List<string>
                {
                    "MILK TEA POS",
                    $"Đơn: {orderNumber}",
                    $"Giờ: {DateTime.Now:dd/MM/yyyy HH:mm}",
                    "-----------------------------"
                };

                foreach (var item in cartSnapshot)
                {
                    lines.Add($"{item.ProductName} x{item.Quantity}");
                    lines.Add($"  {FormatCurrency(item.LineTotal)}");
                }

                lines.Add("-----------------------------");
                lines.Add($"Tạm tính: {FormatCurrency(totals.Subtotal)}");
                lines.Add($"Voucher: -{FormatCurrency(totals.VoucherDiscount)}");
                lines.Add($"Tổng: {FormatCurrency(totals.Total)}");
                lines.Add($"Khách đưa: {FormatCurrency(receivedAmount)}");
                lines.Add($"Tiền thối: {FormatCurrency(changeAmount)}");

                var document = new PrintDocument();
                document.PrintPage += (_, args) =>
                {
                    using var font = new Font("Consolas", 10F);
                    float y = 10;
                    foreach (var line in lines)
                    {
                        args.Graphics.DrawString(line, font, Brushes.Black, 10, y);
                        y += 18;
                    }
                };

                document.Print();
            }
            catch
            {
                MessageBox.Show("Không thể in hóa đơn. Vui lòng kiểm tra máy in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
