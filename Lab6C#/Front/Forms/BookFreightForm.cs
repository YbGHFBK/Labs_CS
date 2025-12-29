using CustomControls;
using System.Drawing.Drawing2D;

public class BookFreightForm : Form
{
    private int WIDTH = 1920;
    private int HEIGHT = 1080;

    private IMessageFilter menuFilterFrom;
    private IMessageFilter menuFilterTo;
    private IMessageFilter menuFilterCargo;

    private Station firstStation = null!;
    private Station lastStation = null!;
    private DateOnly? pickupDate = DateOnly.FromDateTime(DateTime.Now);
    private string selectedCargoType = "General Cargo";

    private DropDownRoundedButton btnCalculate;
    private RoundedTextBox tbWeight;
    private RoundedTextBox tbDimensions;
    private RoundedTextBox tbQuantity;
    private RoundedTextBox tbDescription;

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

    public BookFreightForm(int width, int height)
    {
        WIDTH = width;
        HEIGHT = height;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(WIDTH, HEIGHT);
        BackColor = Color.FromArgb(245, 245, 245);

        var header = new Header(WIDTH, this)
        {
            BackColor = Color.White,
            Dock = DockStyle.Top,
            Height = 65
        };
        header.LogoClicked += () => GoToMain();
        Controls.Add(header);

        var lblTitle = new Label
        {
            Text = "Freight Transportation",
            Font = new Font("Segoe UI", 24f, FontStyle.Bold),
            Location = new Point(50, 80),
            Size = new Size(600, 50),
            TextAlign = ContentAlignment.MiddleLeft
        };
        Controls.Add(lblTitle);

        var leftPanel = new FlowLayoutPanel
        {
            Location = new Point(50, 150),
            Size = new Size(1200, 800),
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.Transparent
        };
        Controls.Add(leftPanel);

        var routeCard = CreateSectionCard("Route Information", 1150, 220, "Images/Location.png");
        leftPanel.Controls.Add(routeCard);

        var fpRouteRow1 = new FlowLayoutPanel { Size = new Size(1100, 80), FlowDirection = FlowDirection.LeftToRight };
        var btnFrom = CreateDropDown("Origin Station", 520, out var menuFrom);
        var btnTo = CreateDropDown("Destination Station", 520, out var menuTo);
        fpRouteRow1.Controls.AddRange(new Control[] { btnFrom, btnTo });
        routeCard.Controls.Add(fpRouteRow1);

        var dpPickup = new RoundedDatePicker { Size = new Size(1050, 40), Margin = new Padding(10, 5, 0, 0) };
        routeCard.Controls.Add(new Label { Text = "Pickup Date", Margin = new Padding(10, 10, 0, 0), Font = new Font("Segoe UI", 10f) });
        routeCard.Controls.Add(dpPickup);

        var cargoCard = CreateSectionCard("Cargo Details", 1150, 450, "Images/Settings.png");
        leftPanel.Controls.Add(cargoCard);

        var btnCargoType = CreateDropDown("Select cargo type", 1050, out var menuCargo);
        cargoCard.Controls.Add(new Label { Text = "Cargo Type", Margin = new Padding(10, 10, 0, 0), Font = new Font("Segoe UI", 10f) });
        cargoCard.Controls.Add(btnCargoType);

        var fpCargoRow = new FlowLayoutPanel { Size = new Size(1100, 80), FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0, 10, 0, 0) };
        tbWeight = new RoundedTextBox { Size = new Size(340, 40), TbText = "1000" };
        tbDimensions = new RoundedTextBox { Size = new Size(340, 40), TbText = "2x1x1" };
        tbQuantity = new RoundedTextBox { Size = new Size(340, 40), TbText = "1" };
        fpCargoRow.Controls.AddRange(new Control[] { tbWeight, tbDimensions, tbQuantity });
        cargoCard.Controls.Add(fpCargoRow);

        tbDescription = new RoundedTextBox { Size = new Size(1050, 120)};
        tbDescription.tb.Multiline = true;
        cargoCard.Controls.Add(new Label { Text = "Cargo Description", Margin = new Padding(10, 10, 0, 0), Font = new Font("Segoe UI", 10f) });
        cargoCard.Controls.Add(tbDescription);

        var estimatePanel = new RoundedFlowLayoutPanel
        {
            Location = new Point(1270, 150),
            Size = new Size(600, 400),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 18,
            Padding = new Padding(20),
            FlowDirection = FlowDirection.TopDown
        };
        Controls.Add(estimatePanel);

        estimatePanel.Controls.Add(new Label { Text = "Cost Estimate", Font = new Font("Segoe UI", 14f, FontStyle.Bold), AutoSize = true });
        btnCalculate = new DropDownRoundedButton
        {
            Size = new Size(540, 45),
            ButtonText = "Calculate Estimate",
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            Margin = new Padding(0, 20, 0, 20)
        };
        estimatePanel.Controls.Add(btnCalculate);

        string[] features = { "• Door-to-door pickup", "• Real-time tracking", "• Insurance included", "• 24/7 customer support", "• Secure handling" };
        estimatePanel.Controls.Add(new Label { Text = "Service Features:", Font = new Font("Segoe UI", 11f, FontStyle.Bold), Margin = new Padding(0, 10, 0, 5) });
        foreach (var f in features)
            estimatePanel.Controls.Add(new Label { Text = f, Font = new Font("Segoe UI", 10f), ForeColor = Color.DimGray, AutoSize = true });

        SetupMenuLogic(btnFrom, menuFrom, (st) => firstStation = st);
        SetupMenuLogic(btnTo, menuTo, (st) => lastStation = st);
    }

    private RoundedFlowLayoutPanel CreateSectionCard(string title, int w, int h, string iconPath)
    {
        var card = new RoundedFlowLayoutPanel
        {
            Size = new Size(w, h),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 18,
            Padding = new Padding(20),
            FlowDirection = FlowDirection.TopDown,
            Margin = new Padding(0, 0, 0, 20)
        };
        var header = new FlowLayoutPanel { Size = new Size(w - 40, 40), FlowDirection = FlowDirection.LeftToRight };
        header.Controls.Add(new RoundedIconPanel { Icon = Image.FromFile(iconPath), Size = new Size(24, 24), BorderRadius = 0 });
        header.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 12f, FontStyle.Bold), AutoSize = true, Margin = new Padding(10, 2, 0, 0) });
        card.Controls.Add(header);
        return card;
    }

    private DropDownRoundedButton CreateDropDown(string placeholder, int w, out AutoSizedMenuPanel menu)
    {
        var btn = new DropDownRoundedButton
        {
            Size = new Size(w, 40),
            ButtonText = placeholder,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(230, 230, 230),
            BorderRadius = 10,
            Margin = new Padding(10)
        };
        menu = new AutoSizedMenuPanel { Width = w, Visible = false, BorderRadius = 10, BorderSize = 1, BorderColor = Color.LightGray };
        this.Controls.Add(menu);
        return btn;
    }

    private void SetupMenuLogic(DropDownRoundedButton btn, AutoSizedMenuPanel menu, Action<Station> onSelect)
    {
        btn.Click += (s, e) =>
        {
            if (menu.Visible) { menu.Hide(); return; }
            menu.Clear();
            foreach (var st in DB.GetAllStations())
            {
                menu.AddItem(st.ToString(), () =>
                {
                    btn.ButtonText = st.ToString();
                    onSelect(st);
                    menu.Hide();
                });
            }
            menu.Location = btn.PointToScreen(new Point(0, btn.Height));
            menu.Visible = true;
            menu.BringToFront();
            Application.AddMessageFilter(new ClickOutsideMenuFilter(menu, btn));
        };
    }

    private void GoToMain()
    {
        new MainPageForm(WIDTH, HEIGHT).Show();
        this.Close();
    }
}