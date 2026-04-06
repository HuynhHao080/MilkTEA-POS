using Dapper;
using Microsoft.EntityFrameworkCore;
using MilkTeaPOS.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace MilkTeaPOS
{
    public partial class frmVouchers : Form
    {
        private Voucher _selectedVoucher;
        public frmVouchers()
        {
            InitializeComponent();
        }
        #region THIẾT KẾ GIAO DIỆN CONTROL BO GÓC
        private void BoGoc(Control ctrl, int radius)
        {
            if (ctrl.Width <= 0 || ctrl.Height <= 0) return;

            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(ctrl.Width - d, 0, d, d, 270, 90);
            path.AddArc(ctrl.Width - d, ctrl.Height - d, d, d, 0, 90);
            path.AddArc(0, ctrl.Height - d, d, d, 90, 90);

            path.CloseFigure();
            ctrl.Region = new Region(path);
        }
        #endregion

        //Load form, hiển thị ...
        private void frmVouchers_Load(object sender, EventArgs e)
        {


        }

        #region Đếm Vouchers
        private async void LoadVoucherData()
        {
            try
            {
                // Sử dụng Context có sẵn của dự án
                using (var context = new PostgresContext())
                {
                    // Lấy toàn bộ danh sách Voucher từ Database bằng EF Core
                    var vouchers = await context.Vouchers.ToListAsync<Voucher>();

                    if (vouchers != null)
                    {
                        UpdateVoucherStatistics(vouchers);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi tải dữ liệu Voucher:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateVoucherStatistics(List<Voucher> vouchers)
        {
            DateTime now = DateTime.Now;

            // Tính toán các con số trước
            int total = vouchers.Count;

            int active = vouchers.Count(v =>
                v.status == "active" &&
                (v.ValidUntil == null || v.ValidUntil >= now));

            int expired = vouchers.Count(v =>
                v.status == "expired" ||
                (v.ValidUntil != null && v.ValidUntil < now));

            int usedUp = vouchers.Count(v =>
                v.UsageLimit > 0 && v.UsageCount >= v.UsageLimit);

            // Hiển thị: Tên nhãn + Xuống hàng (\n) + Con số
            // Sử dụng ký tự $ trước chuỗi để dùng biến trực tiếp trong ngoặc nhọn { }
            lbl1.Text = $"Tổng Vouchers\n{total}";
            lbl2.Text = $"Đang hoạt động\n{active}";
            lbl3.Text = $"Hết hạn\n{expired}";
            lbl4.Text = $"Đã dùng hết\n{usedUp}";
        }
        #endregion


        #region HÀM TÌM KIẾM VOUCHER
        private async void PerformVoucherSearch()
        {
            var searchText = txtSearch.Text.Trim().ToLower();

            try
            {
                using (var context = new PostgresContext())
                {
                    var vouchers = await context.Vouchers
                        .AsNoTracking()
                        .Where(v => string.IsNullOrEmpty(searchText) ||
                                   v.Code.ToLower().Contains(searchText) ||
                                   v.Name.ToLower().Contains(searchText))
                        .OrderByDescending(v => v.CreatedAt)
                        .ToListAsync();
                    dgvVouchers.DataSource = vouchers.Select(v => new
                    {
                        v.Id,
                        Code = v.Code,
                        Name = v.Name,

                        Description = string.IsNullOrEmpty(v.Description) ? "" : v.Description,

                        Loai = v.VoucherType,

                        GiamGia = v.VoucherType == "percentage"
         ? $"{v.DiscountValue}%"
         : $"{v.DiscountValue:N0}",

                        MinOrder = v.MinOrderAmount.HasValue
         ? $"{v.MinOrderAmount:N0}"
         : "0",

                        MaxDiscount = v.MaxDiscountAmount.HasValue
         ? $"{v.MaxDiscountAmount:N0}"
         : "Không giới hạn",

                        ConLai = v.UsageLimit.HasValue
         ? (v.UsageLimit.Value - v.UsageCount).ToString()
         : "∞",

                        BatDau = v.ValidFrom?.ToString("dd/MM/yyyy") ?? "",

                        HanDung = v.ValidUntil.HasValue
         ? v.ValidUntil.Value.ToString("dd/MM/yyyy")
         : "Vô thời hạn",

                        TrangThai = v.status
                    }).ToList();
                }
                CustomizeVoucherColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm Voucher:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        #region lấy dữ liệu từ postgreSQL
        private void CustomizeVoucherColumns()
        {
            if (dgvVouchers.Columns.Count == 0) return;

            dgvVouchers.Columns["Id"].Visible = false;

            dgvVouchers.Columns["Code"].HeaderText = "Mã Voucher";
            dgvVouchers.Columns["Name"].HeaderText = "Tên chương trình";
            dgvVouchers.Columns["Description"].HeaderText = "Mô tả";
            dgvVouchers.Columns["Loai"].HeaderText = "Loại";
            dgvVouchers.Columns["GiamGia"].HeaderText = "Giá trị giảm";
            dgvVouchers.Columns["MinOrder"].HeaderText = "Đơn tối thiểu";
            dgvVouchers.Columns["MaxDiscount"].HeaderText = "Giảm tối đa";
            dgvVouchers.Columns["ConLai"].HeaderText = "Lượt còn lại";
            dgvVouchers.Columns["BatDau"].HeaderText = "Ngày Bắt Đầu";
            dgvVouchers.Columns["HanDung"].HeaderText = "Hạn dùng";
            dgvVouchers.Columns["TrangThai"].HeaderText = "Trạng thái";

            dgvVouchers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private async void LoadVouchersToGrid()
        {
            try
            {
                using (var context = new PostgresContext())
                {
                    var vouchers = await context.Vouchers
                        .OrderByDescending(v => v.CreatedAt)
                        .ToListAsync();

                    var data = vouchers.Select(v => new
                    {
                        v.Id,
                        Code = v.Code,
                        Name = v.Name,
                        Description = string.IsNullOrEmpty(v.Description) ? "" : v.Description,
                        Loai = v.VoucherType,
                        GiamGia = v.VoucherType == "percentage"
                            ? $"{v.DiscountValue}%"
                            : $"{v.DiscountValue:N0}",
                        MinOrder = v.MinOrderAmount.HasValue
                            ? $"{v.MinOrderAmount:N0}"
                            : "0",
                        MaxDiscount = v.MaxDiscountAmount.HasValue
                            ? $"{v.MaxDiscountAmount:N0}"
                            : "Không giới hạn",

                        ConLai = v.UsageLimit.HasValue
                            ? (v.UsageLimit.Value - v.UsageCount).ToString()
                            : "∞",
                        //BatDau = v.ValidFrom.ToString("dd/MM/yyyy"),
                        BatDau = v.ValidFrom?.ToString("dd/MM/yyyy") ?? "",
                        HanDung = v.ValidUntil.HasValue
                                ? v.ValidUntil.Value.ToString("dd/MM/yyyy")
                                : "Vô thời hạn",
                        TrangThai = v.status
                    }).ToList();

                    dgvVouchers.DataSource = data;

                    UpdateVoucherStatistics(vouchers);
                }

                CustomizeVoucherColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}");
            }
        }

        #endregion

        #region sự kiện hàm làm mới

        private void ResetForm()
        {
            txtSearch.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            dtp_handung.Value = DateTime.Now;
            dtp_batdau.Value = DateTime.Now;
            txtMinOrder.Clear();
            txtMaxDiscount.Clear();
            _selectedVoucher = null;

            LoadVouchersToGrid();
        }
        #endregion

        private void lbl1_Click(object sender, EventArgs e)
        {

        }

        private void lbl1_hieuung_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbl4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Lọc_Click(object sender, EventArgs e)
        {

        }

        private void dgv_vouchers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btt_lammoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            MessageBox.Show("Đã làm mới dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvVouchers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Xóa voucher đã chọn
        private async void btt_xoa_Click(object sender, EventArgs e)
        {

            if (dgvVouchers.CurrentRow == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn Voucher cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Lấy ID và Code từ dòng đang chọn
            var row = dgvVouchers.CurrentRow;
            Guid idCanXoa = (Guid)row.Cells["Id"].Value;
            string maVoucher = row.Cells["Code"].Value?.ToString() ?? "";
            var result = MessageBox.Show(
            $"🗑️ Bạn có chắc muốn xóa Voucher '{maVoucher}'?\n\n⚠️ Hành động này không thể hoàn tác!",
            "Xác nhận xóa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            try
            {
                using (var context = new PostgresContext())
                {

                    var voucher = await context.Vouchers.FindAsync(idCanXoa);
                    if (voucher != null)
                    {
                        context.Vouchers.Remove(voucher);
                        await context.SaveChangesAsync();
                    }
                }

                MessageBox.Show("✅ Xóa Voucher thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);


                LoadVouchersToGrid();
                ResetForm();
            }
            catch (DbUpdateException dbEx)
            {
                string errorMsg = $"❌ Lỗi khi xóa (Lỗi DB):\n\n{dbEx.Message}";
                if (dbEx.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{dbEx.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ Lỗi hệ thống khi xóa:\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{ex.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        // Khi người dùng click vào một ô trong DataGridView, hiển thị chi tiết voucher lên form
        private async void dgvVouchers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvVouchers.Rows[e.RowIndex];

            textBox1.Text = row.Cells["Code"].Value?.ToString();
            textBox2.Text = row.Cells["Name"].Value?.ToString();
            textBox3.Text = row.Cells["Description"].Value?.ToString();
            cbxVoucherType.Text = row.Cells["Loai"].Value?.ToString();
            var giamGiaStr = row.Cells["GiamGia"].Value?.ToString()
                ?.Replace("%", "")
                ?.Replace(",", "")
                ?.Trim();
            txtMinOrder.Text = row.Cells["MinOrder"].Value?.ToString();
            txtMaxDiscount.Text = row.Cells["MaxDiscount"].Value?.ToString() == "Không giới hạn"
                ? ""
                : row.Cells["MaxDiscount"].Value?.ToString();

            textBox4.Text = giamGiaStr;
            textBox5.Text = row.Cells["ConLai"].Value?.ToString();
            if (row.Cells["BatDau"].Value != null)
            {
                if (DateTime.TryParseExact(row.Cells["BatDau"].Value.ToString(),
                    "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime bd))
                {
                    dtp_batdau.Value = bd;
                }
            }
            if (row.Cells["HanDung"].Value != null &&
                row.Cells["HanDung"].Value.ToString() != "Vô thời hạn")
            {
                if (DateTime.TryParseExact(row.Cells["HanDung"].Value.ToString(),
                    "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime hd))
                {
                    dtp_handung.Value = hd;
                }
            }
            cbx1_tatcama.Text = row.Cells["TrangThai"].Value?.ToString();
            _selectedVoucher = new Voucher
            {
                Id = (Guid)row.Cells["Id"].Value
            };
        }

        // Sửa voucher đã chọn
        private async void btt_sua_Click(object sender, EventArgs e)
        {
            if (_selectedVoucher == null)
            {
                MessageBox.Show("Chọn voucher cần sửa!");
                return;
            }

            try
            {
                using (var context = new PostgresContext())
                {
                    var voucher = await context.Vouchers.FindAsync(_selectedVoucher.Id);

                    if (voucher == null)
                    {
                        MessageBox.Show("Không tìm thấy voucher!");
                        return;
                    }

                    // 🔹 Code + Name
                    voucher.Code = textBox1.Text.Trim().ToUpper();
                    voucher.Name = textBox2.Text.Trim();

                    // 🔹 Description (NULL OK)
                    voucher.Description = string.IsNullOrWhiteSpace(textBox3.Text)
                        ? null
                        : textBox3.Text.Trim();

                    // 🔹 Type
                    voucher.VoucherType = cbxVoucherType.Text;

                    // 🔹 Discount
                    if (!decimal.TryParse(textBox4.Text, out decimal discountVal) || discountVal < 0)
                    {
                        MessageBox.Show("Giá trị giảm không hợp lệ!");
                        return;
                    }
                    voucher.DiscountValue = discountVal;

                    // 🔹 Min Order
                    if (decimal.TryParse(txtMinOrder.Text, out decimal min))
                        voucher.MinOrderAmount = min;
                    else
                        voucher.MinOrderAmount = null;

                    // 🔹 Max Discount
                    if (decimal.TryParse(txtMaxDiscount.Text, out decimal max))
                        voucher.MaxDiscountAmount = max;
                    else
                        voucher.MaxDiscountAmount = null;

                    // 🔹 UsageLimit (không dùng ConLai)
                    if (int.TryParse(textBox5.Text, out int limit) && limit > 0)
                        voucher.UsageLimit = limit;
                    else
                        voucher.UsageLimit = null;

                    // 🔹 Date
                    if (dtp_handung.Value <= dtp_batdau.Value)
                    {
                        MessageBox.Show("Ngày hết hạn phải sau ngày bắt đầu!");
                        return;
                    }

                    voucher.ValidFrom = dtp_batdau.Value.ToUniversalTime();
                    voucher.ValidUntil = dtp_handung.Value.ToUniversalTime();

                    // 🔹 Status
                    voucher.status = cbx1_tatcama.Text;

                    // 🔹 Update time
                    voucher.UpdatedAt = DateTime.UtcNow;

                    await context.SaveChangesAsync();

                    MessageBox.Show("Cập nhật thành công!");

                    LoadVouchersToGrid();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                string msg = $"Lỗi:\n{ex.Message}";
                if (ex.InnerException != null)
                    msg += $"\nChi tiết: {ex.InnerException.Message}";

                MessageBox.Show(msg);
            }
        }

        #region Hàm thêm mới voucher updata
        private async Task SaveVoucher()
        {
            var code = textBox1.Text.Trim().ToUpper();
            var name = textBox2.Text.Trim();
            var description = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên Voucher!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(textBox4.Text.Trim(), out decimal discountVal) || discountVal < 0)
            {
                MessageBox.Show("Giá trị giảm giá không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal? minOrderAmount = decimal.TryParse(txtMinOrder.Text.Trim(), out decimal mo) ? mo : null;
            decimal? maxDiscount = decimal.TryParse(txtMaxDiscount.Text.Trim(), out decimal md) ? md : null;
            int? usageLimit = int.TryParse(textBox5.Text.Trim(), out int ul) && ul > 0 ? ul : (int?)null;

            var voucherType = cbxVoucherType.SelectedItem?.ToString() ?? "percentage";
            var status = cbx1_tatcama.SelectedItem?.ToString() ?? "active";

            DateTime validFrom = dtp_batdau.Value.ToUniversalTime();
            DateTime validUntil = dtp_handung.Value.ToUniversalTime();

            if (validUntil <= validFrom)
            {
                MessageBox.Show("Ngày hết hạn phải sau ngày bắt đầu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new PostgresContext();

                bool isExist = await context.Vouchers.AsNoTracking().AnyAsync(v => v.Code == code);
                if (isExist)
                {
                    MessageBox.Show($"Mã Voucher '{code}' đã tồn tại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var parameters = new[]
                {
            new NpgsqlParameter("id",           Guid.NewGuid()),
            new NpgsqlParameter("code",          code),
            new NpgsqlParameter("name",          name),
            new NpgsqlParameter("description",   string.IsNullOrEmpty(description) ? DBNull.Value : description),
            new NpgsqlParameter("voucher_type",  voucherType)  { DataTypeName = "voucher_type" },
            new NpgsqlParameter("discount_value",discountVal),
            new NpgsqlParameter("min_order",     minOrderAmount.HasValue ? minOrderAmount.Value : DBNull.Value),
            new NpgsqlParameter("max_discount",  maxDiscount.HasValue    ? maxDiscount.Value    : DBNull.Value),
            new NpgsqlParameter("usage_limit",   usageLimit.HasValue     ? usageLimit.Value     : DBNull.Value),
            new NpgsqlParameter("status",        status)       { DataTypeName = "voucher_status" },
            new NpgsqlParameter("valid_from",    validFrom),
            new NpgsqlParameter("valid_until",   validUntil),
            new NpgsqlParameter("created_at",    DateTime.UtcNow),
            new NpgsqlParameter("updated_at",    DateTime.UtcNow),
        };

                await context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO vouchers 
                (id, code, name, description, voucher_type, discount_value,
                 min_order_amount, max_discount_amount, usage_limit, usage_count,
                 status, valid_from, valid_until, created_at, updated_at)
            VALUES
                (@id, @code, @name, @description, @voucher_type::voucher_type, @discount_value,
                 @min_order, @max_discount, @usage_limit, 0,
                 @status::voucher_status, @valid_from, @valid_until, @created_at, @updated_at)
        ", parameters);

                MessageBox.Show("Thêm Voucher thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadVouchersToGrid();
                ResetForm();
            }
            catch (DbUpdateException dbEx)
            {
                var inner = dbEx.InnerException?.Message ?? dbEx.Message;
                MessageBox.Show($"Lỗi database:\n{inner}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Lỗi:\n{inner}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        private async void btt_them_Click(object sender, EventArgs e)
        {
            await SaveVoucher();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PerformVoucherSearch();

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                PerformVoucherSearch();
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void dtp_handung_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
