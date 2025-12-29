using CustomControls;
using System.Drawing.Drawing2D;

public class RoutesForm : Form
{
    private IMessageFilter menuFilterStart;
    private IMessageFilter menuFilterEnd;

    private Station StartStation;
    private Station EndStation;
    private Route editingRoute = null;

    private FlowLayoutPanel fpList;
    private RoundedFlowLayoutPanel mainPanel;
    private RoundedTextBox tbDist;
    private DropDownRoundedButton btnSave;
    private DropDownRoundedButton btnStart;
    private DropDownRoundedButton btnEnd;

    private class ClickOutsideMenuFilter : IMessageFilter
    {
        private readonly Control menu;
        private readonly Control button;

        public ClickOutsideMenuFilter(Control menu, Control button)
        {
            this.menu = menu;
            this.button = button;
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;

            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN)
            {
                if (!menu.Visible)
                    return false;

                Point p = Control.MousePosition;

                bool clickOnMenu = menu.RectangleToScreen(menu.ClientRectangle).Contains(p);
                bool clickOnButton = button.RectangleToScreen(button.ClientRectangle).Contains(p);

                if (!clickOnMenu && !clickOnButton)
                {
                    menu.Hide();
                }
            }
            return false;
        }
    }

    public RoutesForm()
    {
        InitializeComponent();
        RefreshRouteList();
    }

    private void InitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(1920, 1080);
        BackColor = Color.FromArgb(245, 245, 245);

        var header = new Header(1920, this)
        {
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = Color.White,
            Dock = DockStyle.Top,
            Height = 65,
            Padding = new Padding(0)
        };
        header.LogoClicked += () => GoToMain();
        Controls.Add(header);

        mainPanel = new RoundedFlowLayoutPanel
        {
            Size = new Size(600, 600),
            Location = new Point(660, 200),
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 18,
            Padding = new Padding(40),
            Visible = false
        };
        Controls.Add(mainPanel);

        mainPanel.Controls.Add(new Label { Text = "Start Station", Font = new Font("Segoe UI", 12f), AutoSize = true });
        btnStart = new DropDownRoundedButton
        {
            Width = 520,
            Height = 40,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            ButtonText = "Select Start Station",
            Font = new Font("Segoe UI", 12f),
            Margin = new Padding(0, 0, 0, 20)
        };
        var menuStart = new AutoSizedMenuPanel { Width = 520, Visible = false, BorderRadius = 10, BorderSize = 1, BorderColor = Color.LightGray };
        Controls.Add(menuStart);
        mainPanel.Controls.Add(btnStart);

        btnStart.Click += (s, e) =>
        {
            if (menuStart.Visible) { menuStart.Hide(); return; }
            menuStart.Clear();
            foreach (Station st in DB.stations)
            {
                menuStart.AddItem(st.ToString(), () =>
                {
                    StartStation = st;
                    btnStart.ButtonText = st.ToString();
                    menuStart.Hide();
                });
            }
            menuStart.Location = mainPanel.PointToScreen(new Point(btnStart.Left, btnStart.Bottom));
            menuStart.Visible = true;
            menuStart.BringToFront();
            if (menuFilterStart == null) menuFilterStart = new ClickOutsideMenuFilter(menuStart, btnStart);
            Application.AddMessageFilter(menuFilterStart);
        };

        mainPanel.Controls.Add(new Label { Text = "End Station", Font = new Font("Segoe UI", 12f), AutoSize = true });
        btnEnd = new DropDownRoundedButton
        {
            Width = 520,
            Height = 40,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            ButtonText = "Select End Station",
            Font = new Font("Segoe UI", 12f),
            Margin = new Padding(0, 0, 0, 20)
        };
        var menuEnd = new AutoSizedMenuPanel { Width = 520, Visible = false, BorderRadius = 10, BorderSize = 1, BorderColor = Color.LightGray };
        Controls.Add(menuEnd);
        mainPanel.Controls.Add(btnEnd);

        btnEnd.Click += (s, e) =>
        {
            if (menuEnd.Visible) { menuEnd.Hide(); return; }
            menuEnd.Clear();
            foreach (Station st in DB.stations)
            {
                menuEnd.AddItem(st.ToString(), () =>
                {
                    EndStation = st;
                    btnEnd.ButtonText = st.ToString();
                    menuEnd.Hide();
                });
            }
            menuEnd.Location = mainPanel.PointToScreen(new Point(btnEnd.Left, btnEnd.Bottom));
            menuEnd.Visible = true;
            menuEnd.BringToFront();
            if (menuFilterEnd == null) menuFilterEnd = new ClickOutsideMenuFilter(menuEnd, btnEnd);
            Application.AddMessageFilter(menuFilterEnd);
        };

        mainPanel.Controls.Add(new Label { Text = "Distance (km)", Font = new Font("Segoe UI", 12f), AutoSize = true });
        tbDist = new RoundedTextBox { Width = 520, Margin = new Padding(0, 0, 0, 30) };
        mainPanel.Controls.Add(tbDist);

        btnSave = new DropDownRoundedButton
        {
            Size = new Size(200, 45),
            ButtonText = "Save Route",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            NormalBackColor = Color.FromArgb(220, 38, 38),
            ForeColor = Color.White,
            BorderRadius = 12,
            BorderSize = 0
        };
        mainPanel.Controls.Add(btnSave);

        fpList = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(1600, 860),
            Location = new Point(160, 220),
            AutoScroll = true
        };
        Controls.Add(fpList);

        var btnAdd = new DropDownRoundedButton
        {
            Size = new Size(300, 45),
            ButtonText = "+ Add New Route",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            Margin = new Padding(0, 0, 0, 20)
        };
        fpList.Controls.Add(btnAdd);

        btnAdd.Click += (s, e) =>
        {
            editingRoute = null;
            StartStation = null;
            EndStation = null;
            btnStart.ButtonText = "Select Start Station";
            btnEnd.ButtonText = "Select End Station";
            tbDist.TbText = "";
            btnSave.ButtonText = "Save Route";
            fpList.Visible = false;
            mainPanel.Visible = true;
        };

        btnSave.Click += (s, e) =>
        {
            if (StartStation != null && EndStation != null && !string.IsNullOrWhiteSpace(tbDist.TbText))
            {
                if (editingRoute == null)
                {
                    DB.createRoute(StartStation, EndStation, int.Parse(tbDist.TbText));
                }
                else
                {
                    editingRoute.routeStart = StartStation;
                    editingRoute.routeEnd = EndStation;
                    editingRoute.distance = int.Parse(tbDist.TbText);
                    editingRoute = null;
                    DB.Save();
                }

                mainPanel.Visible = false;
                fpList.Visible = true;
                RefreshRouteList();
            }
            else
            {
                MessageBox.Show("Please fill in all fields");
            }
        };
    }

    private void RefreshRouteList()
    {
        while (fpList.Controls.Count > 1)
        {
            fpList.Controls.RemoveAt(1);
        }

        var routes = DB.routes;

        foreach (var r in routes)
        {
            var rPanel = new RoutePanel(r);

            rPanel.DeleteRequested += (routeToDelete) =>
            {
                var res = MessageBox.Show($"Удалить маршрут {routeToDelete}?", "Подтверждение", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    DB.DeleteRoute(routeToDelete);
                    RefreshRouteList();
                }
            };

            rPanel.EditRequested += (routeToEdit) =>
            {
                editingRoute = routeToEdit;
                StartStation = routeToEdit.routeStart;
                EndStation = routeToEdit.routeEnd;
                btnStart.ButtonText = StartStation.ToString();
                btnEnd.ButtonText = EndStation.ToString();
                tbDist.TbText = routeToEdit.distance.ToString();

                btnSave.ButtonText = "Обновить данные";
                fpList.Visible = false;
                mainPanel.Visible = true;
            };

            fpList.Controls.Add(rPanel);
        }
    }

    private void GoToMain()
    {
        var main = new MainPageForm(1920, 1080);
        main.Show();
        this.Close();
    }
}

public class RoutePanel : UserControl
{
    public event Action<Route> EditRequested;
    public event Action<Route> DeleteRequested;

    public string RouteInfo { get; set; }
    public string Distance { get; set; }

    private int borderRadius = 18;
    private Color borderColor = Color.LightGray;

    public RoutePanel(Route route)
    {
        this.DoubleBuffered = true;
        this.Size = new Size(1550, 140);
        this.BackColor = Color.White;
        this.Padding = new Padding(20);
        this.Margin = new Padding(0, 5, 0, 5);

        RouteInfo = route.ToString();
        Distance = route.distance.ToString() + " km";

        var btnDelete = new Button
        {
            Text = "Удалить",
            Size = new Size(100, 35),
            Location = new Point(1430, 52),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(254, 242, 242),
            ForeColor = Color.FromArgb(220, 38, 38),
            Cursor = Cursors.Hand
        };
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(220, 38, 38);
        btnDelete.Click += (s, e) => DeleteRequested?.Invoke(route);

        var btnEdit = new Button
        {
            Text = "Изменить",
            Size = new Size(100, 35),
            Location = new Point(1320, 52),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(240, 249, 255),
            ForeColor = Color.FromArgb(2, 132, 199),
            Cursor = Cursors.Hand
        };
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(2, 132, 199);
        btnEdit.Click += (s, e) => EditRequested?.Invoke(route);

        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        Rectangle rectFull = new Rectangle(0, 0, this.Width, this.Height);
        using (GraphicsPath clipPath = GetRoundPath(rectFull, borderRadius))
        {
            this.Region = new Region(clipPath);
        }

        using (SolidBrush brush = new SolidBrush(this.BackColor))
        {
            g.FillRectangle(brush, rectFull);
        }

        Rectangle rectBorder = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        using (GraphicsPath borderPath = GetRoundPath(rectBorder, borderRadius))
        {
            using (Pen pen = new Pen(borderColor, 1))
            {
                pen.Alignment = PenAlignment.Inset;
                g.DrawPath(pen, borderPath);
            }
        }

        Font fontBold = new Font("Segoe UI", 14f, FontStyle.Bold);
        Font fontRegular = new Font("Segoe UI", 12f, FontStyle.Regular);

        g.DrawString(RouteInfo, fontBold, Brushes.Black, 25, 40);
        g.DrawString("Distance: " + Distance, fontRegular, Brushes.Gray, 25, 80);
    }

    private GraphicsPath GetRoundPath(Rectangle r, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;
        if (d <= 0) { path.AddRectangle(r); return path; }
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}