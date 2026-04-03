using System;
using System.Windows.Forms;
using MilkTeaPOS.Models;
using FormsTimer = System.Windows.Forms.Timer;

namespace MilkTeaPOS
{
    public partial class frmMain : Form
    {
        private readonly User _currentUser;
        private FormsTimer? _clockTimer;

        public frmMain(User user)
        {
            if (user == null)
                throw new Exception("User truyền vào frmMain bị null!");

            InitializeComponent();
            _currentUser = user;
            initializeForm();
        }

        private void initializeForm()
        {
            setupClock();
            updateUserInfo();
        }

        private void setupClock()
        {
            _clockTimer = new FormsTimer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += clockTimer_Tick;
            _clockTimer.Start();
        }

        private void clockTimer_Tick(object? sender, EventArgs e)
        {
            if (!this.IsDisposed)
            {
                lblClock.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy HH:mm:ss");
            }
        }

        private void updateUserInfo()
        {
            if (_currentUser == null)
            {
                lblUserInfo.Text = "❌ No user";
                return;
            }

            lblUserInfo.Text =
                $"👤 {_currentUser.Username ?? "Unknown"} | {_currentUser.Role?.Name ?? "User"}";
        }

        private void btnMenu_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is string formName)
            {
                if (formName == "Logout")
                {
                    logout();
                }
                else
                {
                    openForm(formName);
                }
            }
        }

        private void openForm(string formName)
        {
            try
            {
                Form frm = formName switch
                {
                    "frmDashboard" => new frmDashboard(),
                    "frmCategories" => new frmCategories(),
                    "frmUsers" => new frmUsers(),
                    _ => createPlaceholderForm(formName)
                };

                // Embed form into pnlContent
                frm.TopLevel = false;
                frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frm.Dock = System.Windows.Forms.DockStyle.Fill;
                
                // Hide welcome panel and bring form to front
                pnlWelcome.Visible = false;
                
                // Add form and bring to front (index 0 = topmost)
                pnlContent.Controls.Add(frm);
                pnlContent.Controls.SetChildIndex(frm, 0);
                
                // Show welcome panel when form is closed
                frm.FormClosed += (s, e) =>
                {
                    pnlWelcome.Visible = true;
                };
                
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Không thể mở form {formName}:\n\n{ex.Message}",
                    "❌ Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private Form createPlaceholderForm(string formName)
        {
            var frm = new Form
            {
                Text = formName,
                Size = new Size(800, 600),
                BackColor = Color.White
            };

            var lbl = new Label
            {
                Text = $"🚧 Form {formName} đang được phát triển...\n\n" +
                       "💡 Tính năng này sẽ sớm ra mắt!\n\n" +
                       "📞 Liên hệ developer để biết thêm chi tiết.",
                Font = new Font("Segoe UI", 16F, FontStyle.Regular),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            frm.Controls.Add(lbl);
            return frm;
        }

        private void logout()
        {
            var result = MessageBox.Show(
                "👋 Bạn có chắc muốn đăng xuất?\n\n" +
                "⏹️ Tất cả các phiên làm việc chưa lưu sẽ bị mất.\n\n" +
                "💡 Nhấn Yes để đăng xuất hoặc No để tiếp tục.",
                "❓ Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _clockTimer?.Stop();
            _clockTimer?.Dispose();
            base.OnFormClosing(e);
        }

    }
}
