using CustomControls;
using System.Drawing.Drawing2D;

public class SchedulesForm : Form
{
    private IMessageFilter menuFilterRoute;
    private IMessageFilter menuFilterTrain;

    private Route SelectedRoute;
    private Train SelectedTrain;
    private Schedule editingSchedule = null!;

    private FlowLayoutPanel fpList;
    private RoundedFlowLayoutPanel mainPanel;
    private RoundedTextBox tbDep;
    private RoundedTextBox tbAr;
    private DropDownRoundedButton btnSave;
    private DropDownRoundedButton btnRoute;
    private DropDownRoundedButton btnTrain;

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
                if (!menu.Visible) return false;
                Point p = Control.MousePosition;
                bool clickOnMenu = menu.RectangleToScreen(menu.ClientRectangle).Contains(p);
                bool clickOnButton = button.RectangleToScreen(button.ClientRectangle).Contains(p);
                if (!clickOnMenu && !clickOnButton) menu.Hide();
            }
            return false;
        }
    }

    public SchedulesForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(1920, 1080);
        BackColor = Color.FromArgb(245, 245, 245);

        var header = new Header(1920, this)
        {
            Dock = DockStyle.Top,
            Height = 65,
            BackColor = Color.White
        };
        header.LogoClicked += () => GoToMain();
        Controls.Add(header);

        mainPanel = new RoundedFlowLayoutPanel
        {
            Size = new Size(600, 650),
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

        mainPanel.Controls.Add(new Label { Text = "Route", Font = new Font("Segoe UI", 12f), AutoSize = true });
        btnRoute = new DropDownRoundedButton
        {
            Size = new Size(520, 40),
            ButtonText = "Select Route",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Margin = new Padding(0, 0, 0, 20)
        };
        var menuRoute = new AutoSizedMenuPanel { Width = 520, Visible = false, BorderRadius = 10, BorderSize = 1, BorderColor = Color.LightGray };
        Controls.Add(menuRoute);
        mainPanel.Controls.Add(btnRoute);

        btnRoute.Click += (s, e) =>
        {
            if (menuRoute.Visible) { menuRoute.Hide(); return; }
            menuRoute.Clear();
            foreach (var route in DB.routes)
            {
                menuRoute.AddItem(route.ToString(), () => {
                    SelectedRoute = route;
                    btnRoute.ButtonText = route.ToString();
                    menuRoute.Hide();
                });
            }
            menuRoute.Location = mainPanel.PointToScreen(new Point(btnRoute.Left, btnRoute.Bottom));
            menuRoute.Visible = true;
            menuRoute.BringToFront();
            if (menuFilterRoute == null) menuFilterRoute = new ClickOutsideMenuFilter(menuRoute, btnRoute);
            Application.AddMessageFilter(menuFilterRoute);
        };

        mainPanel.Controls.Add(new Label { Text = "Train", Font = new Font("Segoe UI", 12f), AutoSize = true });
        btnTrain = new DropDownRoundedButton
        {
            Size = new Size(520, 40),
            ButtonText = "Select Train",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Margin = new Padding(0, 0, 0, 20)
        };
        var menuTrain = new AutoSizedMenuPanel { Width = 520, Visible = false, BorderRadius = 10, BorderSize = 1, BorderColor = Color.LightGray };
        Controls.Add(menuTrain);
        mainPanel.Controls.Add(btnTrain);

        btnTrain.Click += (s, e) =>
        {
            if (menuTrain.Visible) { menuTrain.Hide(); return; }
            menuTrain.Clear();
            foreach (var train in DB.trains)
            {
                menuTrain.AddItem(train.ToString(), () => {
                    SelectedTrain = train;
                    btnTrain.ButtonText = train.ToString();
                    menuTrain.Hide();
                });
            }
            menuTrain.Location = mainPanel.PointToScreen(new Point(btnTrain.Left, btnTrain.Bottom));
            menuTrain.Visible = true;
            menuTrain.BringToFront();
            if (menuFilterTrain == null) menuFilterTrain = new ClickOutsideMenuFilter(menuTrain, btnTrain);
            Application.AddMessageFilter(menuFilterTrain);
        };

        mainPanel.Controls.Add(new Label { Text = "Departure Time (HH:mm)", Font = new Font("Segoe UI", 12f), AutoSize = true });
        tbDep = new RoundedTextBox { Width = 520, Margin = new Padding(0, 0, 0, 20) };
        mainPanel.Controls.Add(tbDep);

        mainPanel.Controls.Add(new Label { Text = "Arrival Time (HH:mm)", Font = new Font("Segoe UI", 12f), AutoSize = true });
        tbAr = new RoundedTextBox { Width = 520, Margin = new Padding(0, 0, 0, 30) };
        mainPanel.Controls.Add(tbAr);

        btnSave = new DropDownRoundedButton
        {
            Size = new Size(200, 45),
            ButtonText = "Save Schedule",
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
            ButtonText = "+ Add New Schedule",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            Margin = new Padding(0, 0, 0, 20)
        };
        fpList.Controls.Add(btnAdd);

        btnAdd.Click += (s, e) =>
        {
            editingSchedule = null!;
            SelectedRoute = null!;
            SelectedTrain = null!;
            tbDep.TbText = "";
            tbAr.TbText = "";
            btnRoute.ButtonText = "Select Route";
            btnTrain.ButtonText = "Select Train";
            btnSave.ButtonText = "Save Schedule";
            fpList.Visible = false;
            mainPanel.Visible = true;
        };

        btnSave.Click += (s, e) =>
        {
            if (SelectedRoute != null && SelectedTrain != null &&
                TimeOnly.TryParse(tbDep.TbText, out TimeOnly dT) && TimeOnly.TryParse(tbAr.TbText, out TimeOnly aT))
            {
                if (editingSchedule == null)
                {
                    DB.createSchedule(SelectedRoute, SelectedTrain, dT, aT);
                }
                else
                {
                    editingSchedule.RouteId = SelectedRoute.Id;
                    editingSchedule.TrainId = SelectedTrain.Id;
                    editingSchedule.DepartureDate = dT;
                    editingSchedule.ArrivalDate = aT;
                    editingSchedule = null!;
                }

                DB.Save();

                mainPanel.Visible = false;
                fpList.Visible = true;
                RefreshList();
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly (Time format: HH:mm)");
            }
        };

        RefreshList();
    }

    private void GoToMain()
    {
        var main = new MainPageForm(1920, 1080);
        main.Show();
        Close();
    }

    private void RefreshList()
    {
        while (fpList.Controls.Count > 1) fpList.Controls.RemoveAt(1);

        foreach (var sc in DB.schedules)
        {
            var scPanel = new SchedulePanel(sc);

            scPanel.DeleteRequested += (item) => {
                var res = MessageBox.Show($"Удалить рейс?", "Подтверждение", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    DB.DeleteSchedule(item);
                    RefreshList();
                }
            };

            scPanel.EditRequested += (item) => {
                editingSchedule = item;
                SelectedRoute = DB.GetById<Route>(item.RouteId);
                SelectedTrain = DB.GetById<Train>(item.TrainId);
                btnRoute.ButtonText = SelectedRoute.ToString();
                btnTrain.ButtonText = SelectedTrain.ToString();
                tbDep.TbText = item.DepartureDate.ToString("HH:mm");
                tbAr.TbText = item.ArrivalDate.ToString("HH:mm");

                btnSave.ButtonText = "Обновить данные";
                fpList.Visible = false;
                mainPanel.Visible = true;
            };

            fpList.Controls.Add(scPanel);
        }
    }
}

public class SchedulePanel : UserControl
{
    public event Action<Schedule> EditRequested;
    public event Action<Schedule> DeleteRequested;

    private Schedule _sc;
    private string TrainName;
    private string TrainId;
    private string ClassType;
    private string FromStation;
    private string ToStation;
    private string DepartureTime;
    private string ArrivalTime;

    private int borderRadius = 18;
    private Color borderColor = Color.LightGray;

    public SchedulePanel(Schedule sc)
    {
        _sc = sc;
        this.DoubleBuffered = true;
        this.Size = new Size(1550, 140);
        this.BackColor = Color.White;
        this.Margin = new Padding(0, 0, 0, 15);

        var train = DB.GetById<Train>(sc.TrainId);
        var route = DB.GetById<Route>(sc.RouteId);

        TrainName = train.model;
        TrainId = train.Id.ToString();
        ClassType = train.type.ToString();
        FromStation = route.routeStart.ToString();
        ToStation = route.routeEnd.ToString();
        DepartureTime = sc.DepartureDate.ToString("HH:mm");
        ArrivalTime = sc.ArrivalDate.ToString("HH:mm");

        var btnDelete = new Button
        {
            Text = "Удалить",
            Size = new Size(100, 35),
            Location = new Point(1430, 85),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(254, 242, 242),
            ForeColor = Color.FromArgb(220, 38, 38),
            Cursor = Cursors.Hand
        };
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(220, 38, 38);
        btnDelete.Click += (s, e) => DeleteRequested?.Invoke(_sc);

        var btnEdit = new Button
        {
            Text = "Изменить",
            Size = new Size(100, 35),
            Location = new Point(1320, 85),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(240, 249, 255),
            ForeColor = Color.FromArgb(2, 132, 199),
            Cursor = Cursors.Hand
        };
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(2, 132, 199);
        btnEdit.Click += (s, e) => EditRequested?.Invoke(_sc);

        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rectBorder = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        using (GraphicsPath path = GetRoundPath(rectBorder, borderRadius))
        {
            this.Region = new Region(path);
            using (Pen pen = new Pen(borderColor, 1)) g.DrawPath(pen, path);
        }

        Font fontBold = new Font("Segoe UI", 12f, FontStyle.Bold);
        Font fontRegular = new Font("Segoe UI", 10f);
        Font fontSmall = new Font("Segoe UI", 9f);
        Font fontTime = new Font("Segoe UI", 16f, FontStyle.Bold);

        g.DrawString(TrainName, fontBold, Brushes.Black, 25, 35);
        DrawTag(g, "ID: " + TrainId, 165, 38, fontSmall);
        DrawTag(g, ClassType, 230, 38, fontSmall);

        g.DrawString(FromStation + " → " + ToStation, fontRegular, Brushes.Gray, 25, 75);

        int center = 1000;
        g.DrawString(DepartureTime, fontTime, Brushes.Black, center, 45);
        g.DrawString("Departure", fontSmall, Brushes.Gray, center, 75);

        g.DrawString(ArrivalTime, fontTime, Brushes.Black, center + 150, 45);
        g.DrawString("Arrival", fontSmall, Brushes.Gray, center + 150, 75);
    }

    private void DrawTag(Graphics g, string text, int x, int y, Font font)
    {
        SizeF size = g.MeasureString(text, font);
        RectangleF tagRect = new RectangleF(x, y, size.Width + 8, size.Height + 2);
        using (GraphicsPath path = GetRoundPath(Rectangle.Round(tagRect), 6))
        {
            g.FillPath(new SolidBrush(Color.FromArgb(240, 242, 245)), path);
            g.DrawString(text, font, Brushes.Black, x + 4, y + 2);
        }
    }

    private GraphicsPath GetRoundPath(Rectangle r, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}