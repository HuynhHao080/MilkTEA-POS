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

        #region Constants

        private const long MAX_FILE_SIZE = 10 * 1024 * 1024; // 10MB
        private static readonly string[] ALLOWED_EXTENSIONS = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        #endregion

        #region Constructor & Initialization

        public frmCategories()
        {
            InitializeComponent();
            _context = new PostgresContext();
            InitializeForm();
        }

        private void InitializeForm()
        {
            LoadCategories();
        }

        #endregion

        #region Data Loading & Display

        private async void LoadCategories()
        {
            try
            {
                ShowLoading(true);
                Application.DoEvents(); // Process UI events immediately

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
                    c.ImageUrl,
                    c.CreatedAt
                }).ToList();

                CustomizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi tải dữ liệu:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
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

            if (columns["ImageUrl"] != null)
            {
                columns["ImageUrl"].HeaderText = "Hình ảnh";
                columns["ImageUrl"].Width = 200;
                columns["ImageUrl"].Visible = false;
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

            dgvCategories.CellFormatting += (s, e) =>
            {
                if (columns["IsActive"] == null || e.ColumnIndex != columns["IsActive"].Index || e.Value == null)
                    return;

                var row = dgvCategories.Rows[e.RowIndex];
                var cellValue = row.Cells["IsActive"].Value;

                if (cellValue is bool isActive)
                {
                    e.Value = isActive ? "✓ Đúng" : "✗ Sai";
                    e.CellStyle.ForeColor = isActive ? Color.FromArgb(72, 187, 120) : Color.FromArgb(220, 53, 69);
                    e.CellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                }
            };
        }

        #endregion

        #region Event Handlers - DataGridView

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
                ImageUrl = row.Cells["ImageUrl"]?.Value?.ToString(),
                DisplayOrder = Convert.ToInt32(row.Cells["DisplayOrder"].Value ?? 0),
                IsActive = Convert.ToBoolean(row.Cells["IsActive"].Value ?? true)
            };

            FillFormData();
        }

        #endregion

        #region Form Data Management

        private void FillFormData()
        {
            if (_selectedCategory == null) return;

            txtName.Text = _selectedCategory.Name;
            txtDescription.Text = _selectedCategory.Description ?? string.Empty;
            numDisplayOrder.Value = _selectedCategory.DisplayOrder ?? 0;
            dtpCreatedAt.Value = _selectedCategory.CreatedAt ?? DateTime.UtcNow;
            txtImageUrl.Text = _selectedCategory.ImageUrl ?? string.Empty;
            chkIsActive.Checked = _selectedCategory.IsActive ?? true;

            if (!string.IsNullOrEmpty(_selectedCategory.ImageUrl))
            {
                string fullPath = GetFullImagePath(_selectedCategory.ImageUrl);
                LoadImagePreview(fullPath);
            }
            else
            {
                picPreview.Image = null;
            }
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

        #endregion

        #region Image Handling

        private void LoadImagePreview(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                picPreview.Image = null;
                return;
            }

            try
            {
                if (File.Exists(imageUrl))
                {
                    using var fs = new FileStream(imageUrl, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    ms.Position = 0;
                    picPreview.Image = Image.FromStream(ms);
                }
                else
                {
                    picPreview.Image = null;
                }
            }
            catch
            {
                picPreview.Image = null;
            }
        }

        private string GetProjectPath()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            while (!string.IsNullOrEmpty(projectPath) &&
                   !File.Exists(Path.Combine(projectPath, "MilkTeaPOS.csproj")))
            {
                projectPath = Directory.GetParent(projectPath)?.FullName;
            }
            return projectPath ?? string.Empty;
        }

        private string GetFullImagePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return string.Empty;

            // Normalize path - remove leading slashes/backslashes
            string normalizedPath = relativePath.TrimStart('/', '\\');

            // Prevent path traversal attacks (check original and normalized)
            if (relativePath.Contains("..") || normalizedPath.Contains("..") ||
                Path.IsPathRooted(normalizedPath))
            {
                throw new ArgumentException("Invalid path format", nameof(relativePath));
            }

            string fullPath = Path.Combine(GetProjectPath(), normalizedPath);
            
            // Ensure the resolved path is within project directory
            string projectPath = GetProjectPath();
            if (!fullPath.StartsWith(projectPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Path traversal detected", nameof(relativePath));
            }

            return fullPath;
        }

        private string GetProjectImagesPath()
        {
            return Path.Combine(GetProjectPath(), "Images");
        }

        private async Task UpdateCategoryImageOnly(string newImageUrl, string newImagePath)
        {
            if (_selectedCategory == null) return;

            try
            {
                var category = await _context.Categories.FindAsync(_selectedCategory.Id);
                if (category != null)
                {
                    string oldImageUrl = category.ImageUrl;
                    if (!string.IsNullOrEmpty(oldImageUrl) && oldImageUrl != newImageUrl)
                    {
                        DeleteOldImage(oldImageUrl);
                    }

                    category.ImageUrl = newImageUrl;
                    category.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    _selectedCategory.ImageUrl = newImageUrl;
                    LoadCategories();
                    ReselectCurrentRow();
                }
            }
            catch
            {
                // Silent fail - image path remains in textbox
            }
        }

        private void DeleteOldImage(string oldImageUrl)
        {
            if (string.IsNullOrEmpty(oldImageUrl)) return;

            try
            {
                string fullPath = GetFullImagePath(oldImageUrl);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch
            {
                // Ignore delete errors
            }
        }

        private void ReselectCurrentRow()
        {
            if (_selectedCategory == null || dgvCategories.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dgvCategories.Rows)
            {
                if (row.Cells["Id"].Value?.ToString() == _selectedCategory.Id.ToString())
                {
                    dgvCategories.ClearSelection();
                    row.Selected = true;
                    dgvCategories.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        #endregion

        #region Search Functionality

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
                ShowLoading(true);
                Application.DoEvents();

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
                    c.ImageUrl,
                    c.CreatedAt
                }).ToList();

                CustomizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi tìm kiếm:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        #endregion

        #region Toolbar Actions

        private async void btnAdd_Click(object sender, EventArgs e)
        {
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
                $"🗑️ Bạn có chắc muốn xóa danh mục '{_selectedCategory.Name}'?\n\n⚠️ Hành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                var category = await _context.Categories.FindAsync(_selectedCategory.Id);
                if (category != null)
                {
                    string oldImageUrl = category.ImageUrl;
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(oldImageUrl))
                    {
                        DeleteOldImage(oldImageUrl);
                    }

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
                MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ Lỗi khi xóa:\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\n\n📋 Chi tiết lỗi:\n{ex.InnerException.Message}";
                }
                MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategories();
            ClearForm();
        }

        private async void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.webp|All Files|*.*",
                Title = "Chọn hình ảnh",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Validate file size
                var fileInfo = new FileInfo(ofd.FileName);
                if (fileInfo.Length > MAX_FILE_SIZE)
                {
                    MessageBox.Show(
                        $"⚠️ File quá lớn!\n\nKích thước: {fileInfo.Length / 1024 / 1024:.0}MB\nTối đa: {MAX_FILE_SIZE / 1024 / 1024}MB",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Validate file extension
                string extension = Path.GetExtension(ofd.FileName).ToLower();
                if (!ALLOWED_EXTENSIONS.Contains(extension))
                {
                    MessageBox.Show(
                        $"⚠️ Định dạng file không hợp lệ!\n\nChỉ chấp nhận: {string.Join(", ", ALLOWED_EXTENSIONS)}",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                string fileName = Path.GetFileName(ofd.FileName);
                string imagesFolder = Path.Combine(GetProjectImagesPath(), "Categories");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{extension}";
                string destPath = Path.Combine(imagesFolder, newFileName);

                File.Copy(ofd.FileName, destPath, true);

                string relativePath = Path.Combine("Images", "Categories", newFileName);
                txtImageUrl.Text = relativePath;
                LoadImagePreview(destPath);

                if (_selectedCategory != null)
                {
                    await UpdateCategoryImageOnly(relativePath, destPath);
                }
            }
        }

        #endregion

        #region Database Operations

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

            var existingCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

            if (existingCategory != null)
            {
                MessageBox.Show($"⚠️ Danh mục '{categoryName}' đã tồn tại!\nVui lòng nhập tên khác.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                txtName.SelectAll();
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
                    CreatedAt = DateTime.UtcNow,
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
                ShowDbError("lưu", dbEx);
            }
            catch (Exception ex)
            {
                ShowError("lưu", ex);
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

            var existingCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower() && c.Id != _selectedCategory!.Id);

            if (existingCategory != null)
            {
                MessageBox.Show($"⚠️ Danh mục '{categoryName}' đã tồn tại!\nVui lòng nhập tên khác.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                txtName.SelectAll();
                return;
            }

            try
            {
                var category = await _context.Categories.FindAsync(_selectedCategory!.Id);
                if (category != null)
                {
                    string oldImageUrl = category.ImageUrl;
                    if (!string.IsNullOrEmpty(oldImageUrl) && oldImageUrl != txtImageUrl.Text)
                    {
                        DeleteOldImage(oldImageUrl);
                    }

                    category.Name = categoryName;
                    category.Description = txtDescription.Text;
                    category.DisplayOrder = (int)numDisplayOrder.Value;
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
                ShowDbError("cập nhật", dbEx);
            }
            catch (Exception ex)
            {
                ShowError("cập nhật", ex);
            }
        }

        #endregion

        #region Helper Methods

        private void ShowLoading(bool isLoading)
        {
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
            pnlMain.Enabled = !isLoading;
            pnlToolbar.Enabled = !isLoading;
            pnlSearch.Enabled = !isLoading;
        }

        private void ShowDbError(string action, DbUpdateException ex)
        {
            string errorMsg = $"❌ Lỗi khi {action} vào database:\n\n{ex.Message}";
            if (ex.InnerException != null)
            {
                errorMsg += $"\n\n📋 Chi tiết lỗi:\n{ex.InnerException.Message}";
            }
            MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowError(string action, Exception ex)
        {
            string errorMsg = $"❌ Lỗi khi {action}:\n{ex.Message}";
            if (ex.InnerException != null)
            {
                errorMsg += $"\n\n📋 Chi tiết lỗi:\n{ex.InnerException.Message}";
            }
            MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion
    }
}
