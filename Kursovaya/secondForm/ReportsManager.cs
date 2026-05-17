using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;

namespace Smirnov_kursovaya.secondForm
{
    public partial class ReportsForm : Form
    {
        private readonly DatabaseHelper dbHelper;
        private readonly CultureInfo Ru = CultureInfo.GetCultureInfo("ru-RU");

        // Данные графиков
        private List<DailyPoint> revenueCurrent = new List<DailyPoint>();
        private List<DailyPoint> revenuePrior = new List<DailyPoint>();
        private List<BarItem> topProducts = new List<BarItem>();
        private List<PieSlice> statusSlices = new List<PieSlice>();

        private static readonly Color[] PiePalette = new[]
        {
            Color.FromArgb(255, 127, 80),
            Color.FromArgb(72, 167, 215),
            Color.FromArgb(120, 200, 130),
            Color.FromArgb(245, 196, 80),
            Color.FromArgb(160, 130, 220),
            Color.FromArgb(220, 110, 150),
        };

        public ReportsForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            reportFromDatePicker.Value = DateTime.Today.AddDays(-29);
            reportToDatePicker.Value = DateTime.Today;

            revenueChartPanel.Paint += RevenueChartPanel_Paint;
            topProductsChartPanel.Paint += TopProductsChartPanel_Paint;
            statusPiePanel.Paint += StatusPiePanel_Paint;

            EnableDoubleBuffer(revenueChartPanel);
            EnableDoubleBuffer(topProductsChartPanel);
            EnableDoubleBuffer(statusPiePanel);

            ApplyButtonHover(refreshButton, Color.Coral, Color.FromArgb(255, 147, 100));
            ApplyButtonHover(pdfButton, Color.FromArgb(235, 107, 60), Color.FromArgb(255, 127, 80));
            foreach (var b in new[] { presetWeekButton, presetMonthButton, presetQuarterButton, presetYearButton })
            {
                b.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
                b.FlatAppearance.BorderSize = 1;
                b.MouseEnter += (s, e) => ((Button)s).BackColor = Color.FromArgb(252, 238, 228);
                b.MouseLeave += (s, e) => ((Button)s).BackColor = Color.White;
            }

            StyleTopClientsGrid();
            foreach (var card in new[] { kpiCard1, kpiCard2, kpiCard3, kpiCard4 }) StyleKpiCard(card);
            foreach (var p in new[] { revenueChartPanel, topProductsChartPanel, statusPiePanel, topClientsPanel }) StyleChartPanel(p);
        }

        private void EnableDoubleBuffer(Panel panel)
        {
            // ResizeRedraw защищён в Control, поэтому ставим оба свойства через рефлексию.
            var t = typeof(Panel);
            t.GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(panel, true, null);
            t.GetProperty("ResizeRedraw",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(panel, true, null);
        }

        private void StyleKpiCard(Panel card)
        {
            card.Paint += (s, e) =>
            {
                var bounds = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                using (var pen = new Pen(Color.FromArgb(225, 225, 225), 1)) e.Graphics.DrawRectangle(pen, bounds);
                using (var brush = new SolidBrush(Color.FromArgb(235, 107, 60))) e.Graphics.FillRectangle(brush, 0, 0, 4, card.Height);
            };
        }

        private void StyleChartPanel(Panel panel)
        {
            panel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(225, 225, 225), 1))
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, panel.Width - 1, panel.Height - 1));
            };
        }

        private void StyleTopClientsGrid()
        {
            topClientsGridView.RowHeadersVisible = false;
            topClientsGridView.GridColor = Color.FromArgb(235, 235, 235);
            topClientsGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            topClientsGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(252, 238, 228);
            topClientsGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
            topClientsGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            topClientsGridView.EnableHeadersVisualStyles = false;
            topClientsGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 248, 246);
            topClientsGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(252, 238, 228);
            topClientsGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void ApplyButtonHover(Button btn, Color normal, Color hover)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = hover;
            btn.MouseLeave += (s, e) => btn.BackColor = normal;
        }

        private void ReportsForm_Load(object sender, EventArgs e) => ReloadData();

        private void ReloadData()
        {
            try
            {
                DateTime from = reportFromDatePicker.Value.Date;
                DateTime to = reportToDatePicker.Value.Date;
                if (to < from) { var tmp = from; from = to; to = tmp; }
                int days = (int)(to - from).TotalDays + 1;
                DateTime priorTo = from.AddDays(-1);
                DateTime priorFrom = priorTo.AddDays(-(days - 1));

                LoadKpis(from, to, priorFrom, priorTo);
                LoadRevenueSeries(from, to, priorFrom, priorTo);
                LoadTopProducts(from, to);
                LoadStatusDistribution(from, to);
                LoadTopClients(from, to);

                revenueChartPanel.Invalidate();
                topProductsChartPanel.Invalidate();
                statusPiePanel.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки аналитики: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadKpis(DateTime from, DateTime to, DateTime priorFrom, DateTime priorTo)
        {
            decimal revenue = 0, priorRevenue = 0;
            int orders = 0, priorOrders = 0, activeClients = 0, priorActive = 0;

            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                ReadKpiRow(conn, from, to, out revenue, out orders, out activeClients);
                ReadKpiRow(conn, priorFrom, priorTo, out priorRevenue, out priorOrders, out priorActive);
            }
            decimal avg = orders > 0 ? revenue / orders : 0;
            decimal priorAvg = priorOrders > 0 ? priorRevenue / priorOrders : 0;

            kpi1Value.Text = revenue.ToString("N2", Ru) + " ₽";
            SetDelta(kpi1Delta, revenue, priorRevenue);
            kpi2Value.Text = orders.ToString("N0", Ru);
            SetDelta(kpi2Delta, orders, priorOrders);
            kpi3Value.Text = avg.ToString("N2", Ru) + " ₽";
            SetDelta(kpi3Delta, avg, priorAvg);
            kpi4Value.Text = activeClients.ToString("N0", Ru);
            SetDelta(kpi4Delta, activeClients, priorActive);
        }

        private void ReadKpiRow(MySqlConnection conn, DateTime from, DateTime to,
            out decimal revenue, out int orders, out int clients)
        {
            revenue = 0; orders = 0; clients = 0;
            using (var cmd = new MySqlCommand(
                @"SELECT COALESCE(SUM(final_amount),0), COUNT(*), COUNT(DISTINCT client_id)
                  FROM orders WHERE date_of_creation BETWEEN @from AND @to", conn))
            {
                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to.AddDays(1).AddSeconds(-1));
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        revenue = rd.IsDBNull(0) ? 0 : Convert.ToDecimal(rd.GetValue(0));
                        orders = rd.IsDBNull(1) ? 0 : Convert.ToInt32(rd.GetValue(1));
                        clients = rd.IsDBNull(2) ? 0 : Convert.ToInt32(rd.GetValue(2));
                    }
                }
            }
        }

        private void SetDelta(Label lbl, decimal current, decimal prior)
        {
            if (prior == 0)
            {
                lbl.Text = current > 0 ? "▲ новый период" : "—";
                lbl.ForeColor = current > 0 ? Color.FromArgb(35, 145, 75) : Color.Gray;
                return;
            }
            decimal pct = (current - prior) / prior * 100m;
            string arrow = pct >= 0 ? "▲" : "▼";
            lbl.ForeColor = pct >= 0 ? Color.FromArgb(35, 145, 75) : Color.FromArgb(200, 60, 60);
            lbl.Text = $"{arrow} {Math.Abs(pct):0.0}% к прошлому периоду";
        }

        private void LoadRevenueSeries(DateTime from, DateTime to, DateTime priorFrom, DateTime priorTo)
        {
            revenueCurrent = new List<DailyPoint>();
            revenuePrior = new List<DailyPoint>();
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                LoadDailyRevenueInto(conn, from, to, revenueCurrent);
                LoadDailyRevenueInto(conn, priorFrom, priorTo, revenuePrior);
            }
        }

        private void LoadDailyRevenueInto(MySqlConnection conn, DateTime from, DateTime to, List<DailyPoint> target)
        {
            var dict = new Dictionary<DateTime, decimal>();
            using (var cmd = new MySqlCommand(
                @"SELECT DATE(date_of_creation), COALESCE(SUM(final_amount),0)
                  FROM orders WHERE date_of_creation BETWEEN @from AND @to
                  GROUP BY DATE(date_of_creation)", conn))
            {
                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to.AddDays(1).AddSeconds(-1));
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        dict[rd.GetDateTime(0).Date] = rd.IsDBNull(1) ? 0 : Convert.ToDecimal(rd.GetValue(1));
                }
            }
            for (DateTime d = from; d <= to; d = d.AddDays(1))
                target.Add(new DailyPoint { Date = d, Value = dict.TryGetValue(d, out var v) ? v : 0 });
        }

        private void LoadTopProducts(DateTime from, DateTime to)
        {
            topProducts = new List<BarItem>();
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    @"SELECT p.name, COALESCE(SUM(oi.total),0)
                      FROM order_items oi
                      INNER JOIN products p ON oi.product_id = p.id
                      INNER JOIN orders o ON oi.order_id = o.id
                      WHERE o.date_of_creation BETWEEN @from AND @to
                      GROUP BY p.name ORDER BY 2 DESC LIMIT 5", conn))
                {
                    cmd.Parameters.AddWithValue("@from", from);
                    cmd.Parameters.AddWithValue("@to", to.AddDays(1).AddSeconds(-1));
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read())
                            topProducts.Add(new BarItem
                            {
                                Label = rd.GetString(0),
                                Value = rd.IsDBNull(1) ? 0 : Convert.ToDecimal(rd.GetValue(1))
                            });
                }
            }
        }

        private void LoadStatusDistribution(DateTime from, DateTime to)
        {
            statusSlices = new List<PieSlice>();
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    @"SELECT s.name, COUNT(o.id)
                      FROM orders o
                      INNER JOIN statuses s ON o.status_id = s.id
                      WHERE o.date_of_creation BETWEEN @from AND @to
                      GROUP BY s.name ORDER BY 2 DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@from", from);
                    cmd.Parameters.AddWithValue("@to", to.AddDays(1).AddSeconds(-1));
                    int idx = 0;
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read())
                        {
                            statusSlices.Add(new PieSlice
                            {
                                Label = rd.GetString(0),
                                Value = Convert.ToDecimal(rd.GetValue(1)),
                                Color = PiePalette[idx % PiePalette.Length]
                            });
                            idx++;
                        }
                }
            }
        }

        private void LoadTopClients(DateTime from, DateTime to)
        {
            var dt = new DataTable();
            dt.Columns.Add("Клиент", typeof(string));
            dt.Columns.Add("Заказы", typeof(int));
            dt.Columns.Add("Сумма", typeof(string));
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    @"SELECT c.fio, COUNT(o.id), COALESCE(SUM(o.final_amount),0)
                      FROM clients c INNER JOIN orders o ON o.client_id = c.id
                      WHERE o.date_of_creation BETWEEN @from AND @to
                      GROUP BY c.fio ORDER BY 3 DESC LIMIT 5", conn))
                {
                    cmd.Parameters.AddWithValue("@from", from);
                    cmd.Parameters.AddWithValue("@to", to.AddDays(1).AddSeconds(-1));
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read())
                            dt.Rows.Add(rd.GetString(0), Convert.ToInt32(rd.GetValue(1)),
                                (rd.IsDBNull(2) ? 0m : Convert.ToDecimal(rd.GetValue(2))).ToString("N2", Ru) + " ₽");
                }
            }
            topClientsGridView.DataSource = dt;
            if (topClientsGridView.Columns.Contains("Сумма"))
                topClientsGridView.Columns["Сумма"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (topClientsGridView.Columns.Contains("Заказы"))
                topClientsGridView.Columns["Заказы"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        // ====== Рисование графиков на формe ======
        private void RevenueChartPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawChartTitle(g, "Выручка по дням (текущий vs прошлый период)");
            var area = new Rectangle(60, 50, revenueChartPanel.Width - 80, revenueChartPanel.Height - 80);
            DrawLineChart(g, area, revenueCurrent, revenuePrior);
            DrawLineLegend(g, revenueChartPanel.Width - 220, 32);
        }

        private void TopProductsChartPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawChartTitle(g, "Топ-5 товаров по выручке");
            var area = new Rectangle(180, 50, topProductsChartPanel.Width - 200, topProductsChartPanel.Height - 70);
            DrawHorizontalBars(g, area, topProducts, 12);
        }

        private void StatusPiePanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawChartTitle(g, "Распределение по статусам");
            DrawPie(g, new Rectangle(14, 44, statusPiePanel.Width - 28, statusPiePanel.Height - 60), statusSlices);
        }

        private void DrawChartTitle(Graphics g, string title)
        {
            using (var f = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (var b = new SolidBrush(Color.FromArgb(40, 40, 40)))
                g.DrawString(title, f, b, new PointF(14, 12));
        }

        private void DrawLineChart(Graphics g, Rectangle area, List<DailyPoint> current, List<DailyPoint> prior)
        {
            using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
            {
                g.DrawLine(pen, area.Left, area.Top, area.Left, area.Bottom);
                g.DrawLine(pen, area.Left, area.Bottom, area.Right, area.Bottom);
            }
            decimal max = 0;
            foreach (var p in current) if (p.Value > max) max = p.Value;
            foreach (var p in prior) if (p.Value > max) max = p.Value;
            if (max <= 0) max = 1;
            decimal step = NiceStep(max / 4m);
            decimal yMax = step * (decimal)Math.Ceiling((double)(max / step));
            if (yMax <= 0) yMax = step;

            using (var gridPen = new Pen(Color.FromArgb(238, 238, 238)))
            using (var lf = new Font("Segoe UI", 8))
            using (var lb = new SolidBrush(Color.Gray))
            {
                for (int i = 0; i <= 4; i++)
                {
                    decimal v = step * i;
                    int y = area.Bottom - (int)((double)(v / yMax) * area.Height);
                    g.DrawLine(gridPen, area.Left, y, area.Right, y);
                    string text = ShortMoney(v);
                    var size = g.MeasureString(text, lf);
                    g.DrawString(text, lf, lb, area.Left - size.Width - 4, y - size.Height / 2);
                }
            }

            DrawSeries(g, area, prior, yMax, Color.FromArgb(180, 180, 180), true);
            DrawSeries(g, area, current, yMax, Color.FromArgb(235, 107, 60), false);

            if (current.Count > 0)
            {
                using (var lf = new Font("Segoe UI", 8))
                using (var lb = new SolidBrush(Color.Gray))
                {
                    int n = current.Count;
                    int[] idxs = n >= 3 ? new[] { 0, n / 2, n - 1 } : new[] { 0, n - 1 };
                    foreach (int i in idxs)
                    {
                        if (i < 0 || i >= n) continue;
                        float x = area.Left + (n == 1 ? area.Width / 2f : (i / (float)(n - 1)) * area.Width);
                        string text = current[i].Date.ToString("dd.MM");
                        var size = g.MeasureString(text, lf);
                        g.DrawString(text, lf, lb, x - size.Width / 2, area.Bottom + 4);
                    }
                }
            }
        }

        private void DrawSeries(Graphics g, Rectangle area, List<DailyPoint> data, decimal yMax, Color color, bool dashed)
        {
            if (data.Count < 2) return;
            var pts = new PointF[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                float x = area.Left + (i / (float)(data.Count - 1)) * area.Width;
                float y = area.Bottom - (float)((double)(data[i].Value / yMax) * area.Height);
                pts[i] = new PointF(x, y);
            }
            using (var pen = new Pen(color, 2.4f))
            {
                if (dashed) pen.DashStyle = DashStyle.Dash;
                g.DrawLines(pen, pts);
            }
            if (!dashed)
                using (var b = new SolidBrush(color))
                    foreach (var p in pts) g.FillEllipse(b, p.X - 2.5f, p.Y - 2.5f, 5f, 5f);
        }

        private void DrawLineLegend(Graphics g, int x, int y)
        {
            using (var f = new Font("Segoe UI", 8.5F))
            using (var tb = new SolidBrush(Color.FromArgb(60, 60, 60)))
            {
                using (var pen = new Pen(Color.FromArgb(235, 107, 60), 2.4f))
                using (var br = new SolidBrush(Color.FromArgb(235, 107, 60)))
                {
                    g.DrawLine(pen, x, y + 6, x + 18, y + 6);
                    g.FillEllipse(br, x + 7, y + 4, 5, 5);
                }
                g.DrawString("текущий период", f, tb, x + 22, y);
                using (var pen = new Pen(Color.FromArgb(180, 180, 180), 2.4f) { DashStyle = DashStyle.Dash })
                    g.DrawLine(pen, x, y + 22, x + 18, y + 22);
                g.DrawString("прошлый период", f, tb, x + 22, y + 16);
            }
        }

        private void DrawHorizontalBars(Graphics g, Rectangle area, List<BarItem> items, int leftLabelOffset)
        {
            if (items == null || items.Count == 0) { DrawEmpty(g, area, "Нет данных"); return; }
            decimal max = 0;
            foreach (var it in items) if (it.Value > max) max = it.Value;
            if (max <= 0) max = 1;
            int n = items.Count;
            int rowH = area.Height / Math.Max(n, 1);
            int barH = (int)(rowH * 0.55);

            using (var lf = new Font("Segoe UI", 9F))
            using (var lb = new SolidBrush(Color.FromArgb(60, 60, 60)))
            using (var vf = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var vb = new SolidBrush(Color.FromArgb(40, 40, 40)))
            using (var bg = new SolidBrush(Color.FromArgb(245, 245, 245)))
            using (var fg = new SolidBrush(Color.FromArgb(235, 107, 60)))
            {
                for (int i = 0; i < n; i++)
                {
                    int y = area.Top + i * rowH + (rowH - barH) / 2;
                    int barW = (int)((double)(items[i].Value / max) * area.Width * 0.8);
                    if (barW < 1) barW = 1;
                    string label = TrimText(g, items[i].Label, lf, area.Left - leftLabelOffset - 8);
                    var size = g.MeasureString(label, lf);
                    g.DrawString(label, lf, lb, leftLabelOffset, y + (barH - size.Height) / 2);
                    g.FillRectangle(bg, area.Left, y, area.Width, barH);
                    g.FillRectangle(fg, area.Left, y, barW, barH);
                    string vt = ShortMoney(items[i].Value);
                    var vs = g.MeasureString(vt, vf);
                    g.DrawString(vt, vf, vb, area.Left + barW + 6, y + (barH - vs.Height) / 2);
                }
            }
        }

        private void DrawPie(Graphics g, Rectangle outer, List<PieSlice> slices)
        {
            if (slices == null || slices.Count == 0) { DrawEmpty(g, outer, "Нет данных"); return; }
            decimal total = 0; foreach (var s in slices) total += s.Value;
            if (total <= 0) { DrawEmpty(g, outer, "Нет данных"); return; }

            int legendW = 170;
            int diameter = Math.Min(outer.Height, outer.Width - legendW - 24);
            if (diameter < 60) diameter = 60;
            var pieRect = new Rectangle(outer.X, outer.Y + (outer.Height - diameter) / 2, diameter, diameter);

            float startAngle = -90f;
            foreach (var slice in slices)
            {
                float sweep = (float)((double)(slice.Value / total) * 360);
                using (var brush = new SolidBrush(slice.Color)) g.FillPie(brush, pieRect, startAngle, sweep);
                startAngle += sweep;
            }
            using (var brush = new SolidBrush(Color.White))
            {
                int inset = diameter / 4;
                g.FillEllipse(brush, pieRect.X + inset, pieRect.Y + inset, diameter - 2 * inset, diameter - 2 * inset);
            }
            using (var tf = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (var tb = new SolidBrush(Color.FromArgb(40, 40, 40)))
            using (var sf = new Font("Segoe UI", 8F))
            using (var sb = new SolidBrush(Color.Gray))
            {
                string totalText = ((int)total).ToString("N0", Ru);
                var s1 = g.MeasureString(totalText, tf);
                g.DrawString(totalText, tf, tb,
                    pieRect.X + (pieRect.Width - s1.Width) / 2,
                    pieRect.Y + pieRect.Height / 2 - s1.Height);
                string sub = "заказов";
                var s2 = g.MeasureString(sub, sf);
                g.DrawString(sub, sf, sb,
                    pieRect.X + (pieRect.Width - s2.Width) / 2,
                    pieRect.Y + pieRect.Height / 2 + 2);
            }

            int lx = pieRect.Right + 16;
            int ly = pieRect.Top + 4;
            using (var nf = new Font("Segoe UI", 9F))
            using (var nb = new SolidBrush(Color.FromArgb(60, 60, 60)))
            using (var pf = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var pb = new SolidBrush(Color.FromArgb(40, 40, 40)))
            {
                foreach (var slice in slices)
                {
                    using (var brush = new SolidBrush(slice.Color)) g.FillRectangle(brush, lx, ly + 4, 12, 12);
                    string name = TrimText(g, slice.Label, nf, outer.Right - lx - 60);
                    g.DrawString(name, nf, nb, lx + 18, ly);
                    decimal pct = total > 0 ? slice.Value / total * 100m : 0;
                    string pctText = $"{pct:0.0}%";
                    var ps = g.MeasureString(pctText, pf);
                    g.DrawString(pctText, pf, pb, outer.Right - ps.Width - 8, ly);
                    ly += 22;
                }
            }
        }

        private void DrawEmpty(Graphics g, Rectangle area, string text)
        {
            using (var f = new Font("Segoe UI", 10F))
            using (var b = new SolidBrush(Color.Gray))
            {
                var s = g.MeasureString(text, f);
                g.DrawString(text, f, b, area.X + (area.Width - s.Width) / 2, area.Y + (area.Height - s.Height) / 2);
            }
        }

        private string TrimText(Graphics g, string text, Font font, float maxWidth)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (g.MeasureString(text, font).Width <= maxWidth) return text;
            string t = text;
            while (t.Length > 1 && g.MeasureString(t + "…", font).Width > maxWidth) t = t.Substring(0, t.Length - 1);
            return t + "…";
        }

        private decimal NiceStep(decimal raw)
        {
            if (raw <= 0) return 1;
            double exp = Math.Floor(Math.Log10((double)raw));
            double pow = Math.Pow(10, exp);
            double m = (double)raw / pow;
            double nice = m < 1.5 ? 1 : m < 3 ? 2 : m < 7 ? 5 : 10;
            return (decimal)(nice * pow);
        }

        private string ShortMoney(decimal v)
        {
            if (Math.Abs(v) >= 1_000_000m) return (v / 1_000_000m).ToString("0.#", Ru) + " млн ₽";
            if (Math.Abs(v) >= 1000m) return (v / 1000m).ToString("0.#", Ru) + " тыс. ₽";
            return v.ToString("0", Ru) + " ₽";
        }

        // ====== Кнопки ======
        private void presetWeekButton_Click(object sender, EventArgs e) => SetPeriod(7);
        private void presetMonthButton_Click(object sender, EventArgs e) => SetPeriod(30);
        private void presetQuarterButton_Click(object sender, EventArgs e) => SetPeriod(90);
        private void presetYearButton_Click(object sender, EventArgs e) => SetPeriod(365);
        private void SetPeriod(int days)
        {
            reportFromDatePicker.Value = DateTime.Today.AddDays(-(days - 1));
            reportToDatePicker.Value = DateTime.Today;
            ReloadData();
        }
        private void refreshButton_Click(object sender, EventArgs e) => ReloadData();
        private void reportFromDatePicker_ValueChanged(object sender, EventArgs e) { }
        private void reportToDatePicker_ValueChanged(object sender, EventArgs e) { }
        private void menuButton_Click(object sender, EventArgs e) => this.Close();

        // ====== PDF ======
        private void pdfButton_Click(object sender, EventArgs e)
        {
            try { ExportToPdf(); }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в PDF: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToPdf()
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF файл (*.pdf)|*.pdf";
                sfd.FileName = $"Аналитика_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                sfd.CheckPathExists = true;
                sfd.OverwritePrompt = true;
                if (sfd.ShowDialog() != DialogResult.OK) return;

                string path = SanitizeFileName(sfd.FileName);
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if (File.Exists(path)) File.Delete(path);

                BuildPdfFile(path);

                MessageBox.Show($"PDF сохранён:\n{path}", "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string SanitizeFileName(string path)
        {
            string dir = Path.GetDirectoryName(path) ?? "";
            string name = Path.GetFileName(path);
            foreach (char c in Path.GetInvalidFileNameChars()) name = name.Replace(c, '_');
            name = name.Replace(' ', '_').Replace('№', 'N');
            return string.IsNullOrEmpty(dir) ? name : Path.Combine(dir, name);
        }

        // Собираем PDF напрямую (без принтера и каких-либо диалогов).
        // Рендерим дашборд в высокоразрешающий bitmap → встраиваем JPEG в одностраничный PDF.
        private void BuildPdfFile(string outputPath)
        {
            const int pageWPt = 842; // A4 landscape: 297×210 мм = 842×595 pt
            const int pageHPt = 595;
            const int dpi = 200;
            int pxW = pageWPt * dpi / 72;
            int pxH = pageHPt * dpi / 72;

            using (var bmp = new Bitmap(pxW, pxH))
            {
                bmp.SetResolution(dpi, dpi);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    g.PageUnit = GraphicsUnit.Point;
                    g.PageScale = 1f;
                    g.FillRectangle(Brushes.White, 0, 0, pageWPt, pageHPt);

                    Rectangle bounds = new Rectangle(40, 40, pageWPt - 80, pageHPt - 80);
                    RenderDashboardToGraphics(g, bounds);
                }
                WritePdfFromBitmap(bmp, outputPath, pageWPt, pageHPt);
            }
        }

        private void RenderDashboardToGraphics(Graphics g, Rectangle bounds)
        {
            int x = bounds.Left, y = bounds.Top;
            using (var tf = new Font("Segoe UI", 18F, FontStyle.Bold))
            using (var sf = new Font("Segoe UI", 10F))
            using (var tb = new SolidBrush(Color.FromArgb(40, 40, 40)))
            using (var sb = new SolidBrush(Color.Gray))
            {
                g.DrawString("Аналитика и отчёты", tf, tb, x, y); y += 32;
                g.DrawString($"Период: {reportFromDatePicker.Value:dd.MM.yyyy} — {reportToDatePicker.Value:dd.MM.yyyy}", sf, sb, x, y); y += 18;
                g.DrawString($"Сформирован: {DateTime.Now:dd.MM.yyyy HH:mm}", sf, sb, x, y); y += 22;
            }

            string[] titles = { kpi1Title.Text, kpi2Title.Text, kpi3Title.Text, kpi4Title.Text };
            string[] values = { kpi1Value.Text, kpi2Value.Text, kpi3Value.Text, kpi4Value.Text };
            string[] deltas = { kpi1Delta.Text, kpi2Delta.Text, kpi3Delta.Text, kpi4Delta.Text };
            Color[] dCol = { kpi1Delta.ForeColor, kpi2Delta.ForeColor, kpi3Delta.ForeColor, kpi4Delta.ForeColor };
            int cardW = (bounds.Width - 24) / 4, cardH = 80;
            for (int i = 0; i < 4; i++)
            {
                Rectangle r = new Rectangle(x + i * (cardW + 8), y, cardW, cardH);
                using (var border = new Pen(Color.FromArgb(220, 220, 220))) g.DrawRectangle(border, r);
                using (var accent = new SolidBrush(Color.FromArgb(235, 107, 60))) g.FillRectangle(accent, r.X, r.Y, 3, r.Height);
                using (var titleFont = new Font("Segoe UI", 9F))
                using (var titleBrush = new SolidBrush(Color.Gray))
                using (var vf = new Font("Segoe UI", 16F, FontStyle.Bold))
                using (var vb = new SolidBrush(Color.FromArgb(40, 40, 40)))
                using (var df = new Font("Segoe UI", 8.5F))
                using (var db = new SolidBrush(dCol[i]))
                {
                    g.DrawString(titles[i], titleFont, titleBrush, r.X + 12, r.Y + 8);
                    g.DrawString(values[i], vf, vb, r.X + 10, r.Y + 26);
                    g.DrawString(deltas[i], df, db, r.X + 12, r.Y + 56);
                }
            }
            y += cardH + 16;
            int remH = bounds.Bottom - y;
            int leftW = (int)(bounds.Width * 0.62);
            int rightW = bounds.Width - leftW - 12;
            int topH = remH * 55 / 100;
            int botH = remH - topH - 12;

            DrawPdfFrame(g, new Rectangle(x, y, leftW, topH), "Выручка по дням", out var revArea);
            DrawLineChart(g, revArea, revenueCurrent, revenuePrior);

            DrawPdfFrame(g, new Rectangle(x + leftW + 12, y, rightW, topH), "Распределение по статусам", out var pieOuter);
            DrawPie(g, pieOuter, statusSlices);

            int y2 = y + topH + 12;
            DrawPdfFrame(g, new Rectangle(x, y2, leftW, botH), "Топ-5 товаров по выручке", out var barsArea);
            DrawHorizontalBars(g, new Rectangle(barsArea.X + 170, barsArea.Y, barsArea.Width - 180, barsArea.Height), topProducts, barsArea.X);

            DrawPdfFrame(g, new Rectangle(x + leftW + 12, y2, rightW, botH), "Топ-5 клиентов по выручке", out var clArea);
            DrawTopClientsTable(g, clArea);
        }

        private static void WritePdfFromBitmap(Bitmap bmp, string path, int pageWPt, int pageHPt)
        {
            byte[] jpegBytes;
            using (var ms = new MemoryStream())
            {
                var encoder = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/jpeg");
                if (encoder != null)
                {
                    using (var encParams = new EncoderParameters(1))
                    {
                        encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 88L);
                        bmp.Save(ms, encoder, encParams);
                    }
                }
                else
                {
                    bmp.Save(ms, ImageFormat.Jpeg);
                }
                jpegBytes = ms.ToArray();
            }
            int wPx = bmp.Width;
            int hPx = bmp.Height;

            string contentStream = $"q\n{pageWPt} 0 0 {pageHPt} 0 0 cm\n/Im0 Do\nQ\n";
            byte[] contentBytes = Encoding.ASCII.GetBytes(contentStream);

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                long[] offsets = new long[6];

                WriteAscii(fs, "%PDF-1.4\n");
                fs.Write(new byte[] { 0x25, 0xE2, 0xE3, 0xCF, 0xD3, 0x0A }, 0, 6);

                offsets[1] = fs.Position;
                WriteAscii(fs, "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");

                offsets[2] = fs.Position;
                WriteAscii(fs, "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n");

                offsets[3] = fs.Position;
                WriteAscii(fs,
                    "3 0 obj\n<< /Type /Page /Parent 2 0 R " +
                    $"/MediaBox [0 0 {pageWPt} {pageHPt}] " +
                    "/Resources << /XObject << /Im0 4 0 R >> /ProcSet [/PDF /ImageC] >> " +
                    "/Contents 5 0 R >>\nendobj\n");

                offsets[4] = fs.Position;
                WriteAscii(fs,
                    $"4 0 obj\n<< /Type /XObject /Subtype /Image /Width {wPx} /Height {hPx} " +
                    $"/ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length {jpegBytes.Length} >>\nstream\n");
                fs.Write(jpegBytes, 0, jpegBytes.Length);
                WriteAscii(fs, "\nendstream\nendobj\n");

                offsets[5] = fs.Position;
                WriteAscii(fs, $"5 0 obj\n<< /Length {contentBytes.Length} >>\nstream\n");
                fs.Write(contentBytes, 0, contentBytes.Length);
                WriteAscii(fs, "endstream\nendobj\n");

                long xrefPos = fs.Position;
                WriteAscii(fs, "xref\n0 6\n");
                WriteAscii(fs, "0000000000 65535 f \n");
                for (int i = 1; i <= 5; i++)
                    WriteAscii(fs, offsets[i].ToString("D10") + " 00000 n \n");

                WriteAscii(fs, $"trailer\n<< /Size 6 /Root 1 0 R >>\nstartxref\n{xrefPos}\n%%EOF\n");
            }
        }

        private static void WriteAscii(Stream s, string text)
        {
            byte[] b = Encoding.ASCII.GetBytes(text);
            s.Write(b, 0, b.Length);
        }

        private void DrawPdfFrame(Graphics g, Rectangle outer, string title, out Rectangle inner)
        {
            using (var border = new Pen(Color.FromArgb(220, 220, 220))) g.DrawRectangle(border, outer);
            using (var tf = new Font("Segoe UI", 10F, FontStyle.Bold))
            using (var tb = new SolidBrush(Color.FromArgb(40, 40, 40)))
                g.DrawString(title, tf, tb, outer.X + 10, outer.Y + 8);
            inner = new Rectangle(outer.X + 50, outer.Y + 38, outer.Width - 60, outer.Height - 50);
        }

        private void DrawTopClientsTable(Graphics g, Rectangle area)
        {
            var dt = topClientsGridView.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) { DrawEmpty(g, area, "Нет данных"); return; }
            int colW1 = area.Width * 55 / 100;
            int colW2 = area.Width * 18 / 100;
            int rowH = (int)(area.Height / (dt.Rows.Count + 1.5));
            if (rowH < 18) rowH = 18; if (rowH > 32) rowH = 32;

            int y = area.Top;
            using (var hf = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var hb = new SolidBrush(Color.FromArgb(40, 40, 40)))
            using (var bg = new SolidBrush(Color.FromArgb(252, 238, 228)))
            using (var rf = new Font("Segoe UI", 9F))
            using (var rb = new SolidBrush(Color.FromArgb(60, 60, 60)))
            using (var br = new Pen(Color.FromArgb(230, 230, 230)))
            {
                g.FillRectangle(bg, area.X, y, area.Width, rowH);
                g.DrawString("Клиент", hf, hb, area.X + 8, y + 4);
                g.DrawString("Заказы", hf, hb, area.X + colW1 + 8, y + 4);
                g.DrawString("Сумма", hf, hb, area.X + colW1 + colW2 + 8, y + 4);
                y += rowH;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (y + rowH > area.Bottom) break;
                    g.DrawLine(br, area.X, y + rowH, area.Right, y + rowH);
                    string fio = TrimText(g, dt.Rows[i][0]?.ToString(), rf, colW1 - 12);
                    g.DrawString(fio, rf, rb, area.X + 8, y + 4);
                    g.DrawString(dt.Rows[i][1]?.ToString(), rf, rb, area.X + colW1 + 8, y + 4);
                    g.DrawString(dt.Rows[i][2]?.ToString(), rf, rb, area.X + colW1 + colW2 + 8, y + 4);
                    y += rowH;
                }
            }
        }

        // ====== Модели ======
        private class DailyPoint { public DateTime Date; public decimal Value; }
        private class BarItem { public string Label; public decimal Value; }
        private class PieSlice { public string Label; public decimal Value; public Color Color; }
    }
}
