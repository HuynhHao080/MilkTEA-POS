using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using MilkTeaPOS.Models;

namespace MilkTeaPOS
{
    public partial class frmChangePassword : Form
    {
        private readonly User _loggedInUser;

        public frmChangePassword(User loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            InitializeForm();
        }

        private void InitializeForm()
        {
            lblUsername.Text = _loggedInUser.Username;
        }

        #region Event Handlers

        private async void btnChangePassword_Click(object sender, EventArgs e)
        {
            await ChangePassword();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCurrentPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                txtNewPassword.Focus();
            }
        }

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                txtConfirmPassword.Focus();
            }
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnChangePassword_Click(sender, e);
            }
        }

        #endregion

        #region Password Change Logic

        private async Task ChangePassword()
        {
            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validate input
            if (string.IsNullOrEmpty(currentPassword))
            {
                MessageBox.Show(
                    "⚠️ Vui lòng nhập mật khẩu hiện tại!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtCurrentPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show(
                    "⚠️ Vui lòng nhập mật khẩu mới!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show(
                    "⚠️ Mật khẩu mới phải có ít nhất 6 ký tự!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                txtNewPassword.SelectAll();
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show(
                    "⚠️ Mật khẩu xác nhận không khớp!\n\nVui lòng nhập lại.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtConfirmPassword.Focus();
                txtConfirmPassword.SelectAll();
                return;
            }

            try
            {
                // Verify current password
                using (var context = new PostgresContext())
                {
                    var user = await context.Users.FindAsync(_loggedInUser.Id);
                    if (user == null)
                    {
                        MessageBox.Show(
                            "❌ Không tìm thấy người dùng!",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    // Verify current password with BCrypt
                    if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                    {
                        MessageBox.Show(
                            "❌ Mật khẩu hiện tại không đúng!\n\nVui lòng nhập lại.",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtCurrentPassword.Focus();
                        txtCurrentPassword.SelectAll();
                        return;
                    }

                    // Hash new password with BCrypt (work factor = 12)
                    string newHash = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12);

                    // Update password hash
                    user.PasswordHash = newHash;
                    user.Password = newPassword; // Temporary: satisfy NOT NULL constraint
                    user.UpdatedAt = DateTime.UtcNow;

                    await context.SaveChangesAsync();
                }

                MessageBox.Show(
                    "✅ Đổi mật khẩu thành công!\n\nMật khẩu mới đã được cập nhật và mã hóa.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Clear form
                txtCurrentPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
                txtCurrentPassword.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Lỗi khi đổi mật khẩu:\n\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region UI Styling

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            StylePasswordTextBox();
        }

        private void StylePasswordTextBox()
        {
            // Style textboxes
            var textboxes = new[] { txtCurrentPassword, txtNewPassword, txtConfirmPassword };
            foreach (var txt in textboxes)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.Font = new Font("Segoe UI", 11F);
                txt.PasswordChar = '●';
                txt.UseSystemPasswordChar = false;
            }
        }

        #endregion
    }
}
