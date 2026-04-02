using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MilkTeaPOS.Models;

namespace MilkTeaPOS
{
    public partial class frmCategories : Form
    {
        private readonly PostgresContext _context;
        private Category? _selectedCategory;

        public frmCategories()
        {
            InitializeComponent();
            _context = new PostgresContext();
            InitializeForm();
        }

        private void InitializeForm()
        {
            LoadCategories();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.BorderStyle = BorderStyle.None;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.MultiSelect = false;
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategories.RowTemplate.Height = 50;
            dgvCategories.CellClick += dgvCategories_CellClick;

            // Styling
            dgvCategories.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvCategories.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 55, 72);
            dgvCategories.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCategories.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCategories.ColumnHeadersHeight = 45;

            dgvCategories.DefaultCellStyle.Font = new Font("Segoe UI", 11F);
            dgvCategories.DefaultCellStyle.BackColor = Color.White;
            dgvCategories.DefaultCellStyle.ForeColor = Color.FromArgb(45, 55, 72);
            dgvCategories.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 252);

            dgvCategories.EnableHeadersVisualStyles = false;
            dgvCategories.GridColor = Color.FromArgb(226, 232, 240);
            dgvCategories.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCategories.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 107, 107);
            dgvCategories.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private async void LoadCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => c.CreatedAt)
                    .ToListAsync();

                dgvCategories.DataSource = categories.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    c.DisplayOrder,
                    c.IsActive,
                    c.CreatedAt
                }).ToList();

                CustomizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi tải dữ liệu:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CustomizeColumns()
        {
            if (dgvCategories.Columns.Count == 0) return;

            var columns = dgvCategories.Columns;

            if (columns["Id"] != null)
            {
                columns["Id"].HeaderText = "ID";
                columns["Id"].Width = 250;
            }

            if (columns["Name"] != null)
            {
                columns["Name"].HeaderText = "Tên danh mục";
                columns["Name"].Width = 200;
            }

            if (columns["Description"] != null)
            {
                columns["Description"].HeaderText = "Mô tả";
                columns["Description"].Width = 300;
            }

            if (columns["DisplayOrder"] != null)
            {
                columns["DisplayOrder"].HeaderText = "Thứ tự";
                columns["DisplayOrder"].Width = 80;
                columns["DisplayOrder"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (columns["IsActive"] != null)
            {
                columns["IsActive"].HeaderText = "Hoạt động";
                columns["IsActive"].Width = 100;
                columns["IsActive"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (columns["CreatedAt"] != null)
            {
                columns["CreatedAt"].HeaderText = "Ngày tạo";
                columns["CreatedAt"].Width = 150;
                columns["CreatedAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }

            if (columns["UpdatedAt"] != null)
            {
                columns["UpdatedAt"].HeaderText = "Ngày cập nhật";
                columns["UpdatedAt"].Width = 150;
                columns["UpdatedAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                columns["UpdatedAt"].Visible = true;
            }

            // Custom formatting for IsActive column
            dgvCategories.CellFormatting += (s, e) =>
            {
                if (columns["IsActive"] == null || e.ColumnIndex != columns["IsActive"].Index || e.Value == null)
                    return;

                // Get the actual boolean value from the data source
                var row = dgvCategories.Rows[e.RowIndex];
                var cellValue = row.Cells["IsActive"].Value;
                
                if (cellValue is bool isActive)
                {
                    if (isActive)
                    {
                        e.Value = "✓ Đúng";
                        e.CellStyle.ForeColor = Color.FromArgb(72, 187, 120);
                        e.CellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                    }
                    else
                    {
                        e.Value = "✗ Sai";
                        e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69);
                        e.CellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                    }
                }
            };
        }

        private void dgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvCategories.Rows[e.RowIndex];

            if (row.Cells["Id"].Value == null) return;

            _selectedCategory = new Category
            {
                Id = Guid.Parse(row.Cells["Id"].Value.ToString()),
                Name = row.Cells["Name"].Value?.ToString() ?? string.Empty,
                Description = row.Cells["Description"].Value?.ToString(),
                DisplayOrder = Convert.ToInt32(row.Cells["DisplayOrder"].Value ?? 0),
                IsActive = Convert.ToBoolean(row.Cells["IsActive"].Value ?? true)
            };

            FillFormData();
        }

        private void FillFormData()
        {
            if (_selectedCategory == null) return;

            txtName.Text = _selectedCategory.Name;
            txtDescription.Text = _selectedCategory.Description ?? string.Empty;
            numDisplayOrder.Value = _selectedCategory.DisplayOrder ?? 0;
            dtpCreatedAt.Value = _selectedCategory.CreatedAt ?? DateTime.UtcNow;
            txtImageUrl.Text = _selectedCategory.ImageUrl ?? string.Empty;
            chkIsActive.Checked = _selectedCategory.IsActive ?? true;

            // Load preview with full path
            if (!string.IsNullOrEmpty(_selectedCategory.ImageUrl))
            {
                string fullPath = _selectedCategory.ImageUrl;
                if (!Path.IsPathRooted(_selectedCategory.ImageUrl))
                {
                    string projectPath = AppDomain.CurrentDomain.BaseDirectory;
                    fullPath = Path.Combine(projectPath, _selectedCategory.ImageUrl);
                }
                LoadImagePreview(fullPath);
            }
            else
            {
                picPreview.Image = null;
            }
        }

        private void LoadImagePreview(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                picPreview.Image = null;
                return;
            }

            try
            {
                // Build full path from relative path
                string fullPath = imageUrl;
                if (!Path.IsPathRooted(imageUrl))
                {
                    string projectPath = AppDomain.CurrentDomain.BaseDirectory;
                    fullPath = Path.Combine(projectPath, imageUrl);
                }
                
                if (File.Exists(fullPath))
                {
                    picPreview.Image = Image.FromFile(fullPath);
                }
                else
                {
                    picPreview.ImageLocation = imageUrl;
                }
            }
            catch
            {
                picPreview.Image = null;
            }
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                PerformSearch();
            }
        }

        private async void PerformSearch()
        {
            var searchText = txtSearch.Text.Trim().ToLower();

            try
            {
                var categories = await _context.Categories
                    .AsNoTracking()
                    .Where(c => string.IsNullOrEmpty(searchText) ||
                               c.Name.ToLower().Contains(searchText) ||
                               (c.Description != null && c.Description.ToLower().Contains(searchText)))
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => c.CreatedAt)
                    .ToListAsync();

                dgvCategories.DataSource = categories.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    c.DisplayOrder,
                    c.IsActive,
                    c.CreatedAt
                }).ToList();

                CustomizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi tìm kiếm:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            // Lưu ngay khi click Thêm
            await SaveCategory();
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedCategory == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn danh mục cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật ngay khi click Sửa
            await UpdateCategory();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCategory == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn danh mục cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"🗑️ Bạn có chắc muốn xóa danh mục '{_selectedCategory.Name}'?\n\n" +
                $"⚠️ Hành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                var category = await _context.Categories.FindAsync(_selectedCategory.Id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();

                    MessageBox.Show("✅ Xóa danh mục thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadCategories();
                    ClearForm();
                }
            }
            catch (DbUpdateException dbEx)
            {
                string errorMsg = $"❌ Lỗi khi xóa:\n\n{dbEx.Message}";
                if (dbEx.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{dbEx.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ Lỗi khi xóa:\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{ex.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategories();
            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtDescription.Clear();
            numDisplayOrder.Value = 0;
            dtpCreatedAt.Value = DateTime.UtcNow;
            txtImageUrl.Clear();
            chkIsActive.Checked = true;
            picPreview.Image = null;
            _selectedCategory = null;
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.webp|All Files|*.*";
                ofd.Title = "Chọn hình ảnh";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Copy image to project Images folder
                    string fileName = Path.GetFileName(ofd.FileName);
                    string projectPath = AppDomain.CurrentDomain.BaseDirectory;
                    string imagesFolder = Path.Combine(projectPath, "Images");
                    
                    // Create Images folder if not exists
                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }
                    
                    // Generate unique filename with timestamp
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                    string extension = Path.GetExtension(fileName);
                    string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{extension}";
                    string destPath = Path.Combine(imagesFolder, newFileName);
                    
                    // Copy file
                    File.Copy(ofd.FileName, destPath, true);
                    
                    // Save relative path to textbox
                    string relativePath = Path.Combine("Images", newFileName);
                    txtImageUrl.Text = relativePath;
                    
                    // Load preview
                    LoadImagePreview(destPath);
                }
            }
        }

        private async Task SaveCategory()
        {
            var categoryName = txtName.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("⚠️ Vui lòng nhập tên danh mục!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            try
            {
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = categoryName,
                    Description = txtDescription.Text,
                    DisplayOrder = (int)numDisplayOrder.Value,
                    CreatedAt = dtpCreatedAt.Value.Kind == DateTimeKind.Utc ? dtpCreatedAt.Value : dtpCreatedAt.Value.ToUniversalTime(),
                    ImageUrl = txtImageUrl.Text,
                    IsActive = chkIsActive.Checked,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                MessageBox.Show("✅ Thêm danh mục thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadCategories();
                ClearForm();
            }
            catch (DbUpdateException dbEx)
            {
                string errorMsg = $"❌ Lỗi khi lưu vào database:\n\n{dbEx.Message}";
                if (dbEx.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{dbEx.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ Lỗi khi lưu:\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{ex.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateCategory()
        {
            var categoryName = txtName.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("⚠️ Vui lòng nhập tên danh mục!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            try
            {
                var category = await _context.Categories.FindAsync(_selectedCategory!.Id);
                if (category != null)
                {
                    category.Name = categoryName;
                    category.Description = txtDescription.Text;
                    category.DisplayOrder = (int)numDisplayOrder.Value;
                    category.CreatedAt = dtpCreatedAt.Value.Kind == DateTimeKind.Utc ? dtpCreatedAt.Value : dtpCreatedAt.Value.ToUniversalTime();
                    category.ImageUrl = txtImageUrl.Text;
                    category.IsActive = chkIsActive.Checked;
                    category.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    MessageBox.Show("✅ Cập nhật danh mục thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadCategories();
                    ClearForm();
                }
            }
            catch (DbUpdateException dbEx)
            {
                string errorMsg = $"❌ Lỗi khi cập nhật vào database:\n\n{dbEx.Message}";
                if (dbEx.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{dbEx.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ Lỗi khi cập nhật:\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{ex.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
