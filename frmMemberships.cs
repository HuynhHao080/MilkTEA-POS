using Microsoft.EntityFrameworkCore;
using MilkTeaPOS.Models;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilkTeaPOS
{
    public partial class frmMemberships : Form
    {
        private Guid selectedId = Guid.Empty;

        #region Constructor & Initialization

        public frmMemberships()
        {
            InitializeComponent();
        }

        private async void Memberships_Load(object sender, EventArgs e)
        {
            try
            {

                await LoadMembershipsAsync();
                dtpJoinedAt.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi load form: " + ex.Message);
            }
        }

        #endregion

        private async void PerformMembershipSearch()
        {
            var searchText = txtSearch.Text.Trim().ToLower();

            try
            {
                using (var context = new PostgresContext())
                {
                    // 1. Truy vấn LINQ với Join để lấy dữ liệu thô
                    var query = from m in context.Memberships
                                join c in context.Customers on m.CustomerId equals c.Id
                                where string.IsNullOrEmpty(searchText) ||
                                      c.Name.ToLower().Contains(searchText) ||
                                      c.Phone.Contains(searchText) ||
                                      ((string)(object)m.Tier).ToLower().Contains(searchText)
                                orderby m.JoinedAt descending
                                select new { m, c };

                    var results = await query.AsNoTracking().ToListAsync();

                    // 2. Format dữ liệu ĐỒNG BỘ với hàm LoadMembershipsAsync
                    dgvMemberships.DataSource = results.Select(x => new
                    {
                        x.m.Id,
                        CustomerName = x.c.Name, // Giữ nguyên tên để CustomersMemberships_Column nhận ra
                        Tier = x.m.Tier,
                        Points = x.m.Points,
                        TotalSpent = x.m.TotalSpent, // Để số nguyên để định dạng N0 ở hàm Column
                        TotalOrders = x.m.TotalOrders,
                        JoinedAt = x.m.JoinedAt,
                        // Tính ngày hết hạn nếu null giống hàm Load
                        ExpiresAt = x.m.ExpiresAt ?? (x.m.JoinedAt.HasValue ? x.m.JoinedAt.Value.AddMonths(6) : (DateTime?)null),
                        LastOrder = x.m.LastOrderAt,
                        UpdatedAt = x.m.UpdatedAt
                    }).ToList();
                }

                // 3. Gọi hàm định dạng lại tiêu đề và ẩn ID
                CustomersMemberships_Column();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Data Loading & Display customers bỏ



        public void CustomersMemberships_Column()
        {
            if (dgvMemberships.Columns.Count == 0) return;
            if (dgvMemberships.Columns.Contains("Id")) dgvMemberships.Columns["Id"].Visible = false;
            if (dgvMemberships.Columns.Contains("CustomerId")) dgvMemberships.Columns["CustomerId"].Visible = false;
            dgvMemberships.Columns["CustomerName"].HeaderText = "Tên Khách Hàng";
            dgvMemberships.Columns["Tier"].HeaderText = "Hạng";
            dgvMemberships.Columns["Points"].HeaderText = "Điểm";
            dgvMemberships.Columns["TotalSpent"].HeaderText = "Tổng Chi Tiêu";
            dgvMemberships.Columns["TotalOrders"].HeaderText = "Tổng Đơn";
            dgvMemberships.Columns["JoinedAt"].HeaderText = "Ngày Tham Gia";
            dgvMemberships.Columns["ExpiresAt"].HeaderText = "Ngày Hết Hạn";
            if (dgvMemberships.Columns.Contains("JoinedAt"))
                dgvMemberships.Columns["JoinedAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (dgvMemberships.Columns.Contains("ExpiresAt"))
                dgvMemberships.Columns["ExpiresAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (dgvMemberships.Columns.Contains("TotalSpent"))
            {
                dgvMemberships.Columns["TotalSpent"].DefaultCellStyle.Format = "N0";
                dgvMemberships.Columns["TotalSpent"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        private async Task LoadMembershipsAsync()
        {
            using (var context = new PostgresContext())
            {
                var data = await context.Memberships
                    .AsNoTracking()
                    .Include(m => m.Customer)
                    .Select(m => new
                    {
                        m.Id,
                        CustomerName = m.Customer != null ? m.Customer.Name : "N/A",
                        m.Tier,
                        m.Points,
                        m.TotalSpent,
                        m.TotalOrders,
                        m.JoinedAt,
                        // TÍNH TOÁN TẠI ĐÂY: Nếu DB có ExpiresAt thì lấy lấy ở đây phải có ngàythang, 
                        // nếu không thì tự tính bằng cách lấy JoinedAt + 6 tháng
                        ExpiresAt = m.ExpiresAt ?? (m.JoinedAt.HasValue ? m.JoinedAt.Value.AddMonths(6) : (DateTime?)null),
                        LastOrder = m.LastOrderAt,
                        m.UpdatedAt
                    })
                    .ToListAsync();

                dgvMemberships.DataSource = data;
            }
            CustomersMemberships_Column();
        }

        #endregion

        #region Event Handlers

        private void dgvMemberships_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvMemberships.Rows[e.RowIndex];
            if (row.Cells["Id"].Value != null)
                selectedId = (Guid)row.Cells["Id"].Value;
            cbCustomer.Text = row.Cells["CustomerName"].Value?.ToString() ?? "";
            if (dgvMemberships.Columns.Contains("Tier"))
                cbTier.Text = row.Cells["Tier"].Value?.ToString() ?? "none";
            txtPoints.Text = row.Cells["Points"].Value?.ToString() ?? "0";
            txtTotalSpent.Text = row.Cells["TotalSpent"].Value?.ToString() ?? "0";
            txtTotalOrders.Text = row.Cells["TotalOrders"].Value?.ToString() ?? "0";
            if (row.Cells["JoinedAt"].Value != null && row.Cells["JoinedAt"].Value != DBNull.Value)
                dtpJoinedAt.Value = Convert.ToDateTime(row.Cells["JoinedAt"].Value);
            else
                dtpJoinedAt.Value = DateTime.Now;
            if (row.Cells["ExpiresAt"].Value != null && row.Cells["ExpiresAt"].Value != DBNull.Value)
                dtpExpiresAt.Value = Convert.ToDateTime(row.Cells["ExpiresAt"].Value);
            else
                dtpExpiresAt.Value = DateTime.Now;
            if (dgvMemberships.Columns.Contains("LastOrder") &&
                row.Cells["LastOrder"].Value != null && row.Cells["LastOrder"].Value != DBNull.Value)
            {
                dtpLastOrder.Value = Convert.ToDateTime(row.Cells["LastOrder"].Value);
            }
        }

        #endregion
        private void ResetMembershipForm()
        {
            txtPoints.Clear();

        }

        #region Form Data Management

        private bool ValidateForm()
        {
            if (cbCustomer.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn khách hàng");
                return false;
            }

            if (!int.TryParse(txtPoints.Text, out _))
            {
                MessageBox.Show("⚠️ Điểm không hợp lệ");
                return false;
            }

            if (!decimal.TryParse(txtTotalSpent.Text, out _))
            {
                MessageBox.Show("⚠️ Tổng chi tiêu không hợp lệ");
                return false;
            }

            if (!int.TryParse(txtTotalOrders.Text, out _))
            {
                MessageBox.Show("⚠️ Số đơn không hợp lệ");
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            selectedId = Guid.Empty;
            cbCustomer.SelectedIndex = -1;
            txtPoints.Clear();
            txtTotalSpent.Clear();
            txtTotalOrders.Clear();
            dtpJoinedAt.Value = DateTime.Now;
            dtpExpiresAt.Value = DateTime.Now;
        }

        #endregion

        #region Database Operations

        private async Task AddMembershipAsync()
        {
            if (!ValidateForm()) return;

            try
            {
                using (var context = new PostgresContext())
                {
                    var membership = new Membership
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = (Guid)cbCustomer.SelectedValue,
                        Points = int.Parse(txtPoints.Text),
                        TotalSpent = decimal.Parse(txtTotalSpent.Text),
                        TotalOrders = int.Parse(txtTotalOrders.Text),
                        JoinedAt = DateTime.Now,
                        ExpiresAt = dtpExpiresAt.Value,
                        UpdatedAt = DateTime.Now
                    };

                    context.Memberships.Add(membership);
                    await context.SaveChangesAsync();
                }

                MessageBox.Show("✅ Thêm thành công");
                await LoadMembershipsAsync();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi thêm: " + ex.Message);
            }
        }

        private async Task UpdateMembershipAsync()
        {
            if (!ValidateForm()) return;

            try
            {
                using (var context = new PostgresContext())
                {
                    var membership = await context.Memberships.FindAsync(selectedId);
                    if (membership == null) return;

                    membership.Points = int.Parse(txtPoints.Text);
                    membership.TotalSpent = decimal.Parse(txtTotalSpent.Text);
                    membership.TotalOrders = int.Parse(txtTotalOrders.Text);
                    membership.ExpiresAt = dtpExpiresAt.Value;
                    membership.UpdatedAt = DateTime.Now;

                    await context.SaveChangesAsync();
                }

                MessageBox.Show("✅ Cập nhật thành công");
                await LoadMembershipsAsync();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi cập nhật: " + ex.Message);
            }
        }

        private async Task DeleteMembershipAsync()
        {
            if (selectedId == Guid.Empty) return;

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var context = new PostgresContext())
                {
                    var membership = await context.Memberships.FindAsync(selectedId);
                    if (membership != null)
                    {
                        context.Memberships.Remove(membership);
                        await context.SaveChangesAsync();
                    }
                }

                MessageBox.Show("✅ Xóa thành công");
                await LoadMembershipsAsync();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi xóa: " + ex.Message);
            }
        }

        #endregion

        #region Search Functionality

        private async Task SearchAsync()
        {
            var keyword = txtSearch.Text.Trim();

            using (var context = new PostgresContext())
            {
                var data = await context.Memberships
                    .Where(m => m.Customer.Name.Contains(keyword))
                    .Select(m => new
                    {
                        m.Id,
                        CustomerName = m.Customer.Name,
                        m.Points,
                        m.TotalSpent,
                        m.TotalOrders,
                        m.JoinedAt,
                        m.ExpiresAt
                    })
                    .ToListAsync();

                dgvMemberships.DataSource = data;
            }
        }

        #endregion

        #region Toolbar Actions

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            await AddMembershipAsync();
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            await UpdateMembershipAsync();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            await DeleteMembershipAsync();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await SearchAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadMembershipsAsync();
            ClearForm();
        }

        #endregion

        private async void dgvMemberships_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (selectedId == Guid.Empty)
            {
                MessageBox.Show("Vui lòng chọn thành viên cần sửa từ danh sách!");
                return;
            }

            try
            {
                using (var context = new PostgresContext())
                {
                    var membership = await context.Memberships.FindAsync(selectedId);

                    if (membership == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin thành viên trong hệ thống!");
                        return;
                    }
                    membership.Tier = cbTier.Text;
                    if (int.TryParse(txtPoints.Text, out int points))
                        membership.Points = points;
                    else
                        membership.Points = 0;
                    if (decimal.TryParse(txtTotalSpent.Text, out decimal totalSpent))
                        membership.TotalSpent = totalSpent;
                    if (int.TryParse(txtTotalOrders.Text, out int totalOrders))
                        membership.TotalOrders = totalOrders;

                    membership.JoinedAt = dtpJoinedAt.Value.ToUniversalTime();
                    membership.ExpiresAt = dtpExpiresAt.Value.ToUniversalTime();
                    await context.SaveChangesAsync();

                    MessageBox.Show("Cập nhật thông tin thành viên thành công!");
                    LoadMembershipsAsync();
                    ResetMembershipForm();
                }
            }
            catch (Exception ex)
            {
                string msg = $"Lỗi khi cập nhật:\n{ex.Message}";
                if (ex.InnerException != null)
                    msg += $"\nChi tiết: {ex.InnerException.Message}";

                MessageBox.Show(msg);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetMembershipForm();
        }

        private async void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvMemberships.CurrentRow == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn Hội viên cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 2. Lấy ID và Tên khách hàng từ dòng đang chọn để hiển thị thông báo xác nhận
            var row = dgvMemberships.CurrentRow;
            Guid idCanXoa = (Guid)row.Cells["Id"].Value;
            string tenKhachHang = row.Cells["CustomerName"].Value?.ToString() ?? "Không rõ tên";

            var result = MessageBox.Show(
                $" Bạn có chắc muốn xóa thẻ Hội viên của khách hàng '{tenKhachHang}'?\n\n⚠️ Hành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;
            try
            {
                using (var context = new PostgresContext())
                {
                    var membership = await context.Memberships.FindAsync(idCanXoa);
                    if (membership != null)
                    {
                        context.Memberships.Remove(membership);
                        await context.SaveChangesAsync();

                        MessageBox.Show("Xóa thẻ Hội viên thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);


                        LoadMembershipsAsync();
                        ResetMembershipForm();
                    }
                    else
                    {
                        MessageBox.Show("❌ Không tìm thấy dữ liệu hội viên này trong hệ thống!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (DbUpdateException dbEx)
            {

                string errorMsg = $"Lỗi khi xóa (Ràng buộc dữ liệu):\n\n{dbEx.Message}";
                if (dbEx.InnerException != null)
                {
                    errorMsg += $"\n\nChi tiết lỗi:\n{dbEx.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Lỗi hệ thống khi xóa:\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\n\nChi tiết lỗi:\n{ex.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task SaveMembership()
        {
            var phoneInput = txtPhone.Text.Trim(); // Lấy từ ô SĐT
            var customerName = cbCustomer.Text.Trim(); // Tên đã được tự điền
            var tier = cbTier.Text.Trim();

            // Kiểm tra xem đã có tên khách hàng (đã tìm thấy) chưa
            if (string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Vui lòng nhập SĐT hợp lệ để xác định khách hàng!", "Thông báo");
                return;
            }

            // Các phần parse số và ngày giữ nguyên như cũ...
            int.TryParse(txtPoints.Text.Trim(), out int points);
            decimal.TryParse(txtTotalSpent.Text.Trim(), out decimal totalSpent);
            int.TryParse(txtTotalOrders.Text.Trim(), out int totalOrders);
            DateTime joinedAt = dtpJoinedAt.Value.ToUniversalTime();
            DateTime expiresAt = dtpExpiresAt.Value.ToUniversalTime();
            DateTime? lastOrder = dtpLastOrder.Checked ? dtpLastOrder.Value.ToUniversalTime() : (DateTime?)null;

            try
            {
                using var context = new PostgresContext();

                var parameters = new[]
                {
            new NpgsqlParameter("phone",        phoneInput),
            new NpgsqlParameter("tier",         tier),
            new NpgsqlParameter("points",       points),
            new NpgsqlParameter("total_spent",  totalSpent),
            new NpgsqlParameter("total_orders", totalOrders),
            new NpgsqlParameter("joined_at",    joinedAt),
            new NpgsqlParameter("expires_at",   expiresAt),
            new NpgsqlParameter("last_order_at",   (object)lastOrder ?? DBNull.Value),
            new NpgsqlParameter("updated_at",   DateTime.UtcNow)
        };

                // Sử dụng INSERT với Subquery dựa trên SĐT để lấy ID
                await context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO memberships 
                        (id, customer_id, tier, points, total_spent, total_orders, 
                         joined_at, expires_at, last_order_at, updated_at)
                    VALUES
                        (gen_random_uuid(), 
                         (SELECT id FROM customers WHERE phone = @phone LIMIT 1), 
                         @tier::membership_tier, -- THÊM ÉP KIỂU Ở ĐÂY
                         @points, @total_spent, @total_orders, 
                         @joined_at, @expires_at, @last_order_at, @updated_at)
                ", parameters);

                MessageBox.Show($"Thêm Hội viên cho khách hàng '{customerName}' thành công!", "Thành công");

                LoadMembershipsAsync();
                ResetMembershipForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
        //        private async Task SaveMembership()
        //        {      
        //            var customerName = cbCustomer.Text.Trim(); 
        //            var tier = cbTier.Text.Trim();             
        //            if (string.IsNullOrEmpty(customerName))
        //            {
        //                MessageBox.Show("Vui lòng chọn hoặc nhập tên khách hàng!", "Thông báo",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
        //            int.TryParse(txtPoints.Text.Trim(), out int points);
        //            decimal.TryParse(txtTotalSpent.Text.Trim(), out decimal totalSpent);
        //            int.TryParse(txtTotalOrders.Text.Trim(), out int totalOrders);
        //            DateTime joinedAt = dtpJoinedAt.Value.ToUniversalTime();
        //            DateTime expiresAt = dtpExpiresAt.Value.ToUniversalTime();
        ////cái này lỗi cẩn thận
        //            // Xử lý LastOrder (Order gần nhất) - Có thể nhập hoặc không
        //            // Nếu checkbox bên cạnh dtpLastOrder được tích thì lấy giá trị, ngược lại để null
        //            DateTime? lastOrder = dtpLastOrder.Checked ? dtpLastOrder.Value.ToUniversalTime() : (DateTime?)null;
        //            if (expiresAt <= joinedAt)
        //            {
        //                MessageBox.Show("Ngày hết hạn phải sau ngày tham gia!", "Thông báo",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
        //            try
        //            {
        //                using var context = new PostgresContext();
        //                var parameters = new[]
        //                {
        //            new NpgsqlParameter("id",           Guid.NewGuid()),
        //            new NpgsqlParameter("customer_name", customerName),
        //            new NpgsqlParameter("tier",          tier),
        //            new NpgsqlParameter("points",        points),
        //            new NpgsqlParameter("total_spent",   totalSpent),
        //            new NpgsqlParameter("total_orders",  totalOrders),
        //            new NpgsqlParameter("joined_at",     joinedAt),
        //            new NpgsqlParameter("expires_at",    expiresAt),
        //            new NpgsqlParameter("last_order",    (object)lastOrder ?? DBNull.Value), // CÓ THỂ NULL
        //            new NpgsqlParameter("updated_at",    DateTime.UtcNow)
        //        };
        //                await context.Database.ExecuteSqlRawAsync(@"
        //            INSERT INTO memberships 
        //                (id, customer_name, tier, points, total_spent, total_orders, 
        //                 joined_at, expires_at, last_order, updated_at)
        //            VALUES
        //                (@id, @customer_name, @tier, @points, @total_spent, @total_orders, 
        //                 @joined_at, @expires_at, @last_order, @updated_at)
        //        ", parameters);

        //                MessageBox.Show("Thêm Hội viên thành công!", "Thành công",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                LoadMembershipsAsync();
        //                ResetMembershipForm();
        //            }
        //            catch (Exception ex)
        //            {
        //                var inner = ex.InnerException?.Message ?? ex.Message;
        //                MessageBox.Show($"Lỗi khi thêm hội viên:\n{inner}", "Lỗi",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            SaveMembership();
        }

        private async void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private async void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                string phone = txtPhone.Text.Trim();
                if (string.IsNullOrEmpty(phone)) return;

                try
                {
                    using var context = new PostgresContext();
                    var customer = await context.Customers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Phone == phone);

                    if (customer != null)
                    {
                        cbCustomer.Text = customer.Name;
                        cbCustomer.ForeColor = Color.Blue;
                        txtPoints.Focus();
                    }
                    else
                    {
                        cbCustomer.Text = "";
                        MessageBox.Show($"Không tìm thấy khách hàng với SĐT: {phone}\nVui lòng kiểm tra lại hoặc thêm mới khách hàng!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi truy vấn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            PerformMembershipSearch();
        }

        private void txtTotalOrders_TextChanged(object sender, EventArgs e)
        {

        }
    }
}