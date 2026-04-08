using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MilkTeaPOS.Models;

namespace MilkTeaPOS
{
    public class frmTable : Form
    {
        private readonly RadioButton _rdoTakeAway;
        private readonly RadioButton _rdoDelivery;
        private readonly RadioButton _rdoDineIn;
        private readonly ComboBox _cboTables;
        private readonly Button _btnConfirm;
        private readonly Button _btnCancel;

        public string SelectedOrderType { get; private set; } = "Mang về";
        public string? SelectedTableName { get; private set; }

        public frmTable()
        {
            Text = "Chọn trạng thái / bàn";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(430, 250);

            var lblTitle = new Label
            {
                Left = 16,
                Top = 12,
                Width = 390,
                Height = 28,
                Text = "Vui lòng chọn trước khi vào Order",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            _rdoTakeAway = new RadioButton
            {
                Left = 20,
                Top = 52,
                Width = 160,
                Height = 24,
                Text = "Mang về",
                Checked = true
            };

            _rdoDelivery = new RadioButton
            {
                Left = 20,
                Top = 82,
                Width = 160,
                Height = 24,
                Text = "Giao hàng"
            };

            _rdoDineIn = new RadioButton
            {
                Left = 20,
                Top = 112,
                Width = 160,
                Height = 24,
                Text = "Tại quán (chọn bàn)"
            };

            var lblTable = new Label
            {
                Left = 20,
                Top = 148,
                Width = 70,
                Height = 24,
                Text = "Bàn:"
            };

            _cboTables = new ComboBox
            {
                Left = 90,
                Top = 146,
                Width = 316,
                Height = 28,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };

            _btnCancel = new Button
            {
                Left = 238,
                Top = 198,
                Width = 80,
                Height = 32,
                Text = "Hủy",
                DialogResult = DialogResult.Cancel
            };

            _btnConfirm = new Button
            {
                Left = 326,
                Top = 198,
                Width = 80,
                Height = 32,
                Text = "Tiếp",
            };

            _rdoTakeAway.CheckedChanged += (_, _) => UpdateTableSelectorState();
            _rdoDelivery.CheckedChanged += (_, _) => UpdateTableSelectorState();
            _rdoDineIn.CheckedChanged += (_, _) => UpdateTableSelectorState();
            _btnConfirm.Click += btnConfirm_Click;
            Load += frmTables_Load;

            Controls.Add(lblTitle);
            Controls.Add(_rdoTakeAway);
            Controls.Add(_rdoDelivery);
            Controls.Add(_rdoDineIn);
            Controls.Add(lblTable);
            Controls.Add(_cboTables);
            Controls.Add(_btnCancel);
            Controls.Add(_btnConfirm);

            AcceptButton = _btnConfirm;
            CancelButton = _btnCancel;
        }

        private async void frmTables_Load(object? sender, EventArgs e)
        {
            try
            {
                using var context = new PostgresContext();
                var dbConn = context.Database.GetDbConnection();
                var tables = await context.Tables
                    .AsNoTracking()
                    .OrderBy(t => t.Name)
                    .Select(t => t.Name)
                    .ToListAsync();

                _cboTables.Items.Clear();
                foreach (var table in tables.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    _cboTables.Items.Add(table);
                }

                if (_cboTables.Items.Count > 0)
                {
                    _cboTables.SelectedIndex = 0;
                }
                else
                {
                    _rdoDineIn.Enabled = false;
                    _rdoTakeAway.Checked = true;
                    MessageBox.Show(
                        $"Không có dữ liệu bàn trong database hiện tại.\nHost: {dbConn.DataSource}\nDatabase: {dbConn.Database}\n\nVui lòng kiểm tra lại seed bảng 'tables'.",
                        "Thiếu dữ liệu bàn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được danh sách bàn.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            UpdateTableSelectorState();
        }

        private void UpdateTableSelectorState()
        {
            _cboTables.Enabled = _rdoDineIn.Checked;
        }

        private void btnConfirm_Click(object? sender, EventArgs e)
        {
            if (_rdoTakeAway.Checked)
            {
                SelectedOrderType = "Mang về";
                SelectedTableName = null;
            }
            else if (_rdoDelivery.Checked)
            {
                SelectedOrderType = "Giao hàng";
                SelectedTableName = null;
            }
            else
            {
                if (_cboTables.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SelectedTableName = _cboTables.SelectedItem.ToString();
                SelectedOrderType = SelectedTableName ?? "Mang về";
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
