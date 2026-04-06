using System;
using System.Drawing;
using System.Windows.Forms;
using MilkTeaPOS.Models;
using FormsTimer = System.Windows.Forms.Timer;

namespace MilkTeaPOS
{
    public partial class frmMain : Form
    {
        private readonly User _currentUser;
        private FormsTimer? _clockTimer;
        private Button? _activeButton;
        private Form? _currentChildForm;

        // Professional color scheme
        private static readonly Color ColorSidebarBg = Color.FromArgb(20, 27, 36);
        private static readonly Color ColorButtonDefault = Color.FromArgb(30, 40, 50);
        private static readonly Color ColorButtonHover = Color.FromArgb(40, 55, 75);
        private static readonly Color ColorButtonActive = Color.FromArgb(52, 152, 219);
        private static readonly Color ColorAccentBar = Color.FromArgb(52, 152, 219);
        private static readonly Color ColorTextDefault = Color.FromArgb(165, 175, 185);
        private static readonly Color ColorTextActive = Color.White;

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
            setupButtonHoverEffects();
            setupChildFormEvents();
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

        #region Button Hover & Active State Management

        private void setupButtonHoverEffects()
        {
            // Attach hover events to all menu buttons
            var menuButtons = new[] { btnDashboard, btnCategories, btnProducts, btnToppings,
                btnTables, btnPOS, btnOrderHistory, btnCustomers, btnMemberships,
                btnVouchers, btnReports, btnUsers, btnAuditLog };

            foreach (var btn in menuButtons)
            {
                if (btn != null)
                {
                    btn.MouseEnter += Button_MouseEnter;
                    btn.MouseLeave += Button_MouseLeave;
                    // Use MouseDown for instant active (fires BEFORE Click)
                    btn.MouseDown += Button_MouseDown;
                }
            }

            // Auto-select Dashboard on startup
            setActiveButton(btnDashboard);
        }

        private void Button_MouseDown(object? sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                // Reset previous active button
                if (_activeButton != null && _activeButton != btn && !_activeButton.IsDisposed)
                {
                    _activeButton.BackColor = ColorButtonDefault;
                    _activeButton.ForeColor = ColorTextDefault;
                    _activeButton.FlatAppearance.MouseOverBackColor = ColorButtonHover;
                    var oldFont = _activeButton.Font;
                    _activeButton.Font = new Font("Segoe UI", 11.5F, FontStyle.Regular);
                    oldFont?.Dispose();
                }

                // Set new active button
                _activeButton = btn;
                btn.BackColor = ColorButtonActive;
                btn.ForeColor = ColorTextActive;
                // QUAN TRỌNG: Set MouseOverBackColor = ActiveColor để Windows Forms ko override
                btn.FlatAppearance.MouseOverBackColor = ColorButtonActive;
                var newFont = btn.Font;
                btn.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
                newFont?.Dispose();

                btn.Refresh();
            }
        }

        private void Button_MouseEnter(object? sender, EventArgs e)
        {
            // Active button KHÔNG BAO GIỜ bị hover override
            if (sender is Button btn && btn != _activeButton)
            {
                btn.BackColor = ColorButtonHover;
                btn.ForeColor = ColorTextActive;
            }
        }

        private void Button_MouseLeave(object? sender, EventArgs e)
        {
            // Active button KHÔNG BAO GIỜ bị hover override
            if (sender is Button btn && btn != _activeButton)
            {
                btn.BackColor = ColorButtonDefault;
                btn.ForeColor = ColorTextDefault;
            }
        }

        private void setActiveButton(Button? activeBtn)
        {
            if (activeBtn == null) return;

            // Reset previous active button instantly
            if (_activeButton != null && !_activeButton.IsDisposed)
            {
                _activeButton.BackColor = ColorButtonDefault;
                _activeButton.ForeColor = ColorTextDefault;
                var oldFont = _activeButton.Font;
                _activeButton.Font = new Font("Segoe UI", 11.5F, FontStyle.Regular);
                _activeButton.Refresh();
                oldFont?.Dispose();
            }

            // Set new active button instantly
            _activeButton = activeBtn;
            _activeButton.BackColor = ColorButtonActive;
            _activeButton.ForeColor = ColorTextActive;
            var newFont = _activeButton.Font;
            _activeButton.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            _activeButton.Refresh(); // Force immediate repaint
            newFont?.Dispose();
        }

        #endregion

        #region Child Form Management

        private void setupChildFormEvents()
        {
            // Track when child forms are closed
            pnlContent.ControlAdded += (s, e) =>
            {
                if (e.Control is Form childForm)
                {
                    _currentChildForm = childForm;
                    childForm.FormClosed += ChildForm_FormClosed;
                }
            };
        }

        private void ChildForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _currentChildForm = null;
            pnlWelcome.Visible = true;
        }

        #endregion

        private void btnMenu_Click(object? sender, EventArgs e)
        {
            // Button đã được active trong MouseDown rồi, chỉ cần mở form
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
                // Close existing form if any
                closeCurrentChildForm();

                Form frm = formName switch
                {
                    "frmDashboard" => new frmDashboard(),
                    "frmCategories" => new frmCategories(),
                    "frmUsers" => new frmUsers(),
                    "frmCustomers" => new frmCustomers(),
                    "frmSalesReport" => new frmSalesReport(),
                    "frmAuditLog" => new frmAuditLog(),
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

        private void closeCurrentChildForm()
        {
            if (_currentChildForm != null && !_currentChildForm.IsDisposed)
            {
                _currentChildForm.FormClosed -= ChildForm_FormClosed;
                _currentChildForm.Close();
                _currentChildForm.Dispose();
                _currentChildForm = null;
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

            // Dispose active button font
            if (_activeButton?.Font != null && _activeButton.Font != Font)
            {
                _activeButton.Font.Dispose();
            }

            closeCurrentChildForm();
            base.OnFormClosing(e);
        }

    }
}
