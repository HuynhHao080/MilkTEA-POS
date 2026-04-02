namespace MilkTeaPOS
{
    partial class frmDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblComingSoon;
        private System.Windows.Forms.Panel pnlCard1;
        private System.Windows.Forms.Panel pnlCard2;
        private System.Windows.Forms.Panel pnlCard3;
        private System.Windows.Forms.Panel pnlCard4;
        private System.Windows.Forms.Label lblCard1Emoji;
        private System.Windows.Forms.Label lblCard1Label;
        private System.Windows.Forms.Label lblCard1Value;
        private System.Windows.Forms.Label lblCard2Emoji;
        private System.Windows.Forms.Label lblCard2Label;
        private System.Windows.Forms.Label lblCard2Value;
        private System.Windows.Forms.Label lblCard3Emoji;
        private System.Windows.Forms.Label lblCard3Label;
        private System.Windows.Forms.Label lblCard3Value;
        private System.Windows.Forms.Label lblCard4Emoji;
        private System.Windows.Forms.Label lblCard4Label;
        private System.Windows.Forms.Label lblCard4Value;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblComingSoon = new System.Windows.Forms.Label();
            this.pnlCard1 = new System.Windows.Forms.Panel();
            this.lblCard1Emoji = new System.Windows.Forms.Label();
            this.lblCard1Label = new System.Windows.Forms.Label();
            this.lblCard1Value = new System.Windows.Forms.Label();
            this.pnlCard2 = new System.Windows.Forms.Panel();
            this.lblCard2Emoji = new System.Windows.Forms.Label();
            this.lblCard2Label = new System.Windows.Forms.Label();
            this.lblCard2Value = new System.Windows.Forms.Label();
            this.pnlCard3 = new System.Windows.Forms.Panel();
            this.lblCard3Emoji = new System.Windows.Forms.Label();
            this.lblCard3Label = new System.Windows.Forms.Label();
            this.lblCard3Value = new System.Windows.Forms.Label();
            this.pnlCard4 = new System.Windows.Forms.Panel();
            this.lblCard4Emoji = new System.Windows.Forms.Label();
            this.lblCard4Label = new System.Windows.Forms.Label();
            this.lblCard4Value = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // ============================================
            // FORM SETTINGS
            // ============================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.pnlHeader);
            this.Name = "frmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "📊 Dashboard - MilkTea POS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            // ============================================
            // HEADER
            // ============================================
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 80;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20);

            this.lblTitle.Text = "📊 Dashboard - Tổng quan";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(45, 55, 72);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.AutoSize = true;
            this.pnlHeader.Controls.Add(this.lblTitle);

            // ============================================
            // STATS CARDS (FlowLayoutPanel)
            // ============================================
            this.pnlCards.BackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Height = 180;
            this.pnlCards.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlCards.WrapContents = true;
            this.pnlCards.Padding = new System.Windows.Forms.Padding(20);
            this.pnlCards.Controls.Add(this.pnlCard1);
            this.pnlCards.Controls.Add(this.pnlCard2);
            this.pnlCards.Controls.Add(this.pnlCard3);
            this.pnlCards.Controls.Add(this.pnlCard4);

            // Card 1 - Revenue (Coral Red)
            this.pnlCard1.BackColor = System.Drawing.Color.FromArgb(255, 107, 107);
            this.pnlCard1.Size = new System.Drawing.Size(260, 140);
            this.pnlCard1.Margin = new System.Windows.Forms.Padding(10);
            this.pnlCard1.Cursor = System.Windows.Forms.Cursors.Hand;

            this.lblCard1Emoji.Text = "💰";
            this.lblCard1Emoji.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.lblCard1Emoji.ForeColor = System.Drawing.Color.White;
            this.lblCard1Emoji.Location = new System.Drawing.Point(20, 15);
            this.lblCard1Emoji.AutoSize = true;

            this.lblCard1Label.Text = "Doanh thu hôm nay";
            this.lblCard1Label.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCard1Label.ForeColor = System.Drawing.Color.FromArgb(200, 240, 240);
            this.lblCard1Label.Location = new System.Drawing.Point(20, 65);
            this.lblCard1Label.AutoSize = true;

            this.lblCard1Value.Text = "0đ";
            this.lblCard1Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard1Value.ForeColor = System.Drawing.Color.White;
            this.lblCard1Value.Location = new System.Drawing.Point(20, 95);
            this.lblCard1Value.AutoSize = true;

            this.pnlCard1.Controls.Add(this.lblCard1Emoji);
            this.pnlCard1.Controls.Add(this.lblCard1Label);
            this.pnlCard1.Controls.Add(this.lblCard1Value);

            // Card 2 - Orders (Turquoise)
            this.pnlCard2.BackColor = System.Drawing.Color.FromArgb(78, 205, 196);
            this.pnlCard2.Size = new System.Drawing.Size(260, 140);
            this.pnlCard2.Margin = new System.Windows.Forms.Padding(10);
            this.pnlCard2.Cursor = System.Windows.Forms.Cursors.Hand;

            this.lblCard2Emoji.Text = "📦";
            this.lblCard2Emoji.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.lblCard2Emoji.ForeColor = System.Drawing.Color.White;
            this.lblCard2Emoji.Location = new System.Drawing.Point(20, 15);
            this.lblCard2Emoji.AutoSize = true;

            this.lblCard2Label.Text = "Đơn hàng";
            this.lblCard2Label.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCard2Label.ForeColor = System.Drawing.Color.FromArgb(200, 240, 240);
            this.lblCard2Label.Location = new System.Drawing.Point(20, 65);
            this.lblCard2Label.AutoSize = true;

            this.lblCard2Value.Text = "0";
            this.lblCard2Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard2Value.ForeColor = System.Drawing.Color.White;
            this.lblCard2Value.Location = new System.Drawing.Point(20, 95);
            this.lblCard2Value.AutoSize = true;

            this.pnlCard2.Controls.Add(this.lblCard2Emoji);
            this.pnlCard2.Controls.Add(this.lblCard2Label);
            this.pnlCard2.Controls.Add(this.lblCard2Value);

            // Card 3 - Tables (Yellow)
            this.pnlCard3.BackColor = System.Drawing.Color.FromArgb(255, 206, 86);
            this.pnlCard3.Size = new System.Drawing.Size(260, 140);
            this.pnlCard3.Margin = new System.Windows.Forms.Padding(10);
            this.pnlCard3.Cursor = System.Windows.Forms.Cursors.Hand;

            this.lblCard3Emoji.Text = "🪑";
            this.lblCard3Emoji.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.lblCard3Emoji.ForeColor = System.Drawing.Color.White;
            this.lblCard3Emoji.Location = new System.Drawing.Point(20, 15);
            this.lblCard3Emoji.AutoSize = true;

            this.lblCard3Label.Text = "Bàn đang dùng";
            this.lblCard3Label.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCard3Label.ForeColor = System.Drawing.Color.FromArgb(200, 240, 240);
            this.lblCard3Label.Location = new System.Drawing.Point(20, 65);
            this.lblCard3Label.AutoSize = true;

            this.lblCard3Value.Text = "0/20";
            this.lblCard3Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard3Value.ForeColor = System.Drawing.Color.White;
            this.lblCard3Value.Location = new System.Drawing.Point(20, 95);
            this.lblCard3Value.AutoSize = true;

            this.pnlCard3.Controls.Add(this.lblCard3Emoji);
            this.pnlCard3.Controls.Add(this.lblCard3Label);
            this.pnlCard3.Controls.Add(this.lblCard3Value);

            // Card 4 - Customers (Gray)
            this.pnlCard4.BackColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.pnlCard4.Size = new System.Drawing.Size(260, 140);
            this.pnlCard4.Margin = new System.Windows.Forms.Padding(10);
            this.pnlCard4.Cursor = System.Windows.Forms.Cursors.Hand;

            this.lblCard4Emoji.Text = "👥";
            this.lblCard4Emoji.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.lblCard4Emoji.ForeColor = System.Drawing.Color.White;
            this.lblCard4Emoji.Location = new System.Drawing.Point(20, 15);
            this.lblCard4Emoji.AutoSize = true;

            this.lblCard4Label.Text = "Khách hàng";
            this.lblCard4Label.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCard4Label.ForeColor = System.Drawing.Color.FromArgb(200, 240, 240);
            this.lblCard4Label.Location = new System.Drawing.Point(20, 65);
            this.lblCard4Label.AutoSize = true;

            this.lblCard4Value.Text = "10";
            this.lblCard4Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard4Value.ForeColor = System.Drawing.Color.White;
            this.lblCard4Value.Location = new System.Drawing.Point(20, 95);
            this.lblCard4Value.AutoSize = true;

            this.pnlCard4.Controls.Add(this.lblCard4Emoji);
            this.pnlCard4.Controls.Add(this.lblCard4Label);
            this.pnlCard4.Controls.Add(this.lblCard4Value);

            // ============================================
            // CONTENT AREA
            // ============================================
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Padding = new System.Windows.Forms.Padding(20);

            this.lblComingSoon.Text = "🚧 Dashboard chi tiết đang được phát triển...\n\n" +
                                      "💡 Tính năng biểu đồ, thống kê sẽ sớm ra mắt!\n\n" +
                                      "📞 Liên hệ developer để biết thêm chi tiết.";
            this.lblComingSoon.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.lblComingSoon.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblComingSoon.AutoSize = true;
            this.lblComingSoon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComingSoon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pnlContent.Controls.Add(this.lblComingSoon);

            this.ResumeLayout(false);
        }
    }
}
