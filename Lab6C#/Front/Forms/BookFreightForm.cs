using CustomControls;
using System.Drawing.Drawing2D;
using System.Globalization;

public class BookFreightForm : Form
{
    private int WIDTH = 1920;
    private int HEIGHT = 1080;

    private Station firstStation = null!;
    private Station lastStation = null!;
    private DateOnly? pickupDate = DateOnly.FromDateTime(DateTime.Now);
    private string selectedCargoType = "General Cargo";

    private DropDownRoundedButton btnCalculate;
    private RoundedTextBox tbWeight;
    private RoundedTextBox tbDimensions;
    private RoundedTextBox tbQuantity;
    private RoundedTextBox tbDescription;

    private Label lblBaseRateValue;
    private Label lblWeightChargeValue;
    private Label lblTotalValue;
    private Label lblDimensionsChargeValue;
    private Label lblQuantityChargeValue;


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
        ValidateForm();
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
            Size = new Size(1200, 1000),
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.FromArgb(245, 245, 245)
        };
        Controls.Add(leftPanel);

        var routeCard = CreateSectionCard("Route Information", 1150, 300, "Images/Location.png");
        leftPanel.Controls.Add(routeCard);

        var fpRouteRow1 = new FlowLayoutPanel { Size = new Size(1100, 80), FlowDirection = FlowDirection.LeftToRight };
        var btnFrom = CreateDropDown("Origin Station", 520, out var menuFrom);
        var btnTo = CreateDropDown("Destination Station", 520, out var menuTo);
        fpRouteRow1.Controls.AddRange(new Control[] { btnFrom, btnTo });
        routeCard.Controls.Add(fpRouteRow1);

        var dpPickup = new RoundedDatePicker { Size = new Size(1050, 40), Margin = new Padding(10, 5, 0, 0), BorderColor = Color.FromArgb(230, 230, 230) };
        routeCard.Controls.Add(new Label { Text = "Pickup Date", Margin = new Padding(10, 10, 0, 0), Font = new Font("Segoe UI", 10f) });
        routeCard.Controls.Add(dpPickup);

        var cargoCard = CreateSectionCard("Cargo Details", 1150, 480, "Images/Settings.png");
        leftPanel.Controls.Add(cargoCard);

        cargoCard.Controls.Add(new Label { Text = "Cargo Type", Margin = new Padding(10, 10, 0, 0), Font = new Font("Segoe UI", 10f), AutoSize = true });
        var btnCargoType = CreateDropDown("Select cargo type", 1050, out var menuCargo);
        cargoCard.Controls.Add(btnCargoType);

        btnCargoType.Click += (s, e) =>
        {
            if (menuCargo.Visible) { menuCargo.Hide(); return; }
            menuCargo.Clear();
            foreach (CargoCarriegeType cType in Enum.GetValues(typeof(CargoCarriegeType)))
            {
                menuCargo.AddItem(cType.ToString(), () =>
                {
                    btnCargoType.ButtonText = cType.ToString();
                    selectedCargoType = cType.ToString();
                    menuCargo.Hide();
                    ValidateForm();
                });
            }
            menuCargo.Location = btnCargoType.PointToScreen(new Point(0, btnCargoType.Height));
            menuCargo.Visible = true;
            menuCargo.BringToFront();
            Application.AddMessageFilter(new ClickOutsideMenuFilter(menuCargo, btnCargoType));
        };

        var fpCargoRow = new FlowLayoutPanel
        {
            Size = new Size(1100, 85),
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0, 10, 0, 0),
            BackColor = Color.White
        };

        var fpWeight = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, Size = new Size(345, 75), Margin = new Padding(0) };
        fpWeight.Controls.Add(new Label { Text = "Weight (kg)", Font = new Font("Segoe UI", 9f), Margin = new Padding(10, 0, 0, 0), AutoSize = true });
        tbWeight = new RoundedTextBox { Size = new Size(330, 40), PlaceholderText = "1000", BorderColor = Color.FromArgb(230, 230, 230) };
        tbWeight.tb.TextChanged += (s, e) => ValidateForm();
        fpWeight.Controls.Add(tbWeight);

        var fpDimensions = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, Size = new Size(345, 75), Margin = new Padding(0) };
        fpDimensions.Controls.Add(new Label { Text = "Dimensions (LxWxH m)", Font = new Font("Segoe UI", 9f), Margin = new Padding(10, 0, 0, 0), AutoSize = true });
        tbDimensions = new RoundedTextBox { Size = new Size(330, 40), PlaceholderText = "2x2x1", BorderColor = Color.FromArgb(230, 230, 230) };
        tbDimensions.tb.TextChanged += (s, e) => ValidateForm();
        fpDimensions.Controls.Add(tbDimensions);

        var fpQuantity = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, Size = new Size(345, 75), Margin = new Padding(0) };
        fpQuantity.Controls.Add(new Label { Text = "Quantity", Font = new Font("Segoe UI", 9f), Margin = new Padding(10, 0, 0, 0), AutoSize = true });
        tbQuantity = new RoundedTextBox { Size = new Size(330, 40), PlaceholderText = "1", BorderColor = Color.FromArgb(230, 230, 230) };
        tbQuantity.tb.TextChanged += (s, e) => ValidateForm();
        fpQuantity.Controls.Add(tbQuantity);

        fpCargoRow.Controls.AddRange(new Control[] { fpWeight, fpDimensions, fpQuantity });
        cargoCard.Controls.Add(fpCargoRow);

        cargoCard.Controls.Add(new Label { Text = "Cargo Description", Margin = new Padding(10, 10, 0, 0), Font = new Font("Segoe UI", 10f), AutoSize = true });
        tbDescription = new RoundedTextBox { Size = new Size(1050, 120), BorderColor = Color.FromArgb(230, 230, 230) };
        tbDescription.tb.Multiline = true;
        tbDescription.tb.TextChanged += (s, e) => ValidateForm();
        cargoCard.Controls.Add(tbDescription);

        var estimatePanel = new RoundedFlowLayoutPanel
        {
            Location = new Point(1270, 150),
            Size = new Size(600, 540),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 18,
            Padding = new Padding(25),
            FlowDirection = FlowDirection.TopDown
        };
        Controls.Add(estimatePanel);

        estimatePanel.Controls.Add(new Label { Text = "Cost Estimate", Font = new Font("Segoe UI", 14f, FontStyle.Bold), AutoSize = true });
        btnCalculate = new DropDownRoundedButton
        {
            Size = new Size(550, 45),
            ButtonText = "Calculate Estimate",
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            Margin = new Padding(0, 20, 0, 20),
            Enabled = false,
        };
        btnCalculate.Click += (s, e) => PerformCalculation();
        estimatePanel.Controls.Add(btnCalculate);

        lblBaseRateValue = CreatePriceRow(estimatePanel, "Base Rate:", "$0.00");
        lblWeightChargeValue = CreatePriceRow(estimatePanel, "Weight Charge:", "$0.00");
        lblDimensionsChargeValue = CreatePriceRow(estimatePanel, "Dimensions Charge:", "$0.00");
        lblQuantityChargeValue = CreatePriceRow(estimatePanel, "Quantity:", "x1");

        var line = new Panel { Size = new Size(550, 1), BackColor = Color.LightGray, Margin = new Padding(0, 15, 0, 15) };
        estimatePanel.Controls.Add(line);


        var fpTotal = new FlowLayoutPanel { Size = new Size(550, 50), FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0) };
        var lblTotalTitle = new Label { Text = "Estimated Total:", Font = new Font("Segoe UI", 12f, FontStyle.Bold), AutoSize = false, Size = new Size(230, 50), TextAlign = ContentAlignment.MiddleLeft };
        lblTotalValue = new Label { Text = "$0.00", Font = new Font("Segoe UI", 24f, FontStyle.Bold), AutoSize = false, Size = new Size(300, 50), TextAlign = ContentAlignment.MiddleRight };
        fpTotal.Controls.Add(lblTotalTitle);
        fpTotal.Controls.Add(lblTotalValue);
        estimatePanel.Controls.Add(fpTotal);

        string[] features = { "• Door-to-door pickup", "• Real-time tracking", "• Insurance included", "• 24/7 customer support", "• Secure handling" };
        estimatePanel.Controls.Add(new Label { Text = "Service Features:", Font = new Font("Segoe UI", 11f, FontStyle.Bold), Margin = new Padding(0, 10, 0, 5) });
        foreach (var f in features)
            estimatePanel.Controls.Add(new Label { Text = f, Font = new Font("Segoe UI", 10f), ForeColor = Color.DimGray, AutoSize = true });

        SetupMenuLogic(btnFrom, menuFrom, (st) => firstStation = st);
        SetupMenuLogic(btnTo, menuTo, (st) => lastStation = st);
    }

    private Label CreatePriceRow(Panel parent, string title, string initialValue)
    {
        var row = new FlowLayoutPanel { Size = new Size(550, 30), Margin = new Padding(0, 5, 0, 5) };
        var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 10f), ForeColor = Color.DimGray, Size = new Size(220, 25), TextAlign = ContentAlignment.MiddleLeft };
        var lblValue = new Label { Text = initialValue, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Size = new Size(300, 25), TextAlign = ContentAlignment.MiddleRight };
        row.Controls.Add(lblTitle);
        row.Controls.Add(lblValue);
        parent.Controls.Add(row);
        return lblValue;
    }

    private void ValidateForm()
    {
        bool isValid = firstStation != null &&
                       lastStation != null &&
                       !string.IsNullOrEmpty(selectedCargoType) &&
                       !string.IsNullOrWhiteSpace(tbWeight.TbText) &&
                       ValidateDimensions(tbDimensions.tb.Text) &&
                       !string.IsNullOrWhiteSpace(tbQuantity.TbText);
        btnCalculate.Enabled = isValid;
        btnCalculate.BackColor = isValid ? Color.White : Color.FromArgb(240, 240, 240);
    }

    private bool ValidateDimensions(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        var parts = input.ToLower().Split('x');
        if (parts.Length != 3) return false;
        foreach (var part in parts)
        {
            if (!double.TryParse(part.Trim(), out double val) || val <= 0) return false;
        }
        return true;
    }

    private void PerformCalculation()
    {
        string weightText = tbWeight.tb.Text.Trim().Replace('.', ',');
        string dimsText = tbDimensions.tb.Text.Trim().ToLower();
        string qtyText = tbQuantity.tb.Text.Trim();

        if (!double.TryParse(weightText, NumberStyles.Any, CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"), out double weight))
        {
            MessageBox.Show("Please enter a valid numeric weight.");
            return;
        }

        var parts = dimsText.Split('x', '×');
        if (parts.Length != 3 ||
            !double.TryParse(parts[0].Trim().Replace('.', ','), NumberStyles.Any, CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"), out double L) ||
            !double.TryParse(parts[1].Trim().Replace('.', ','), NumberStyles.Any, CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"), out double W) ||
            !double.TryParse(parts[2].Trim().Replace('.', ','), NumberStyles.Any, CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"), out double H))
        {
            MessageBox.Show("Please enter dimensions in format LxWxH (e.g. 2x2x1).");
            return;
        }

        if (!int.TryParse(qtyText, out int quantity) || quantity < 1)
            quantity = 1;

        double baseRate = 50.00;
        double weightCharge = weight * 0.5;
        double volume = L * W * H;
        double dimensionsCharge = volume * 100.0;

        double chargesSingleUnit = weightCharge + dimensionsCharge;
        double total = baseRate + chargesSingleUnit * quantity;

        var inv = CultureInfo.InvariantCulture;
        lblBaseRateValue.Text = baseRate.ToString("F2", inv) + " $";
        lblWeightChargeValue.Text = weightCharge.ToString("F2", inv) + " $";
        lblDimensionsChargeValue.Text = dimensionsCharge.ToString("F2", inv) + " $";
        lblQuantityChargeValue.Text = "x" + quantity.ToString();
        lblTotalValue.Text = "$" + total.ToString("N2", inv);

        lblBaseRateValue.Refresh();
        lblWeightChargeValue.Refresh();
        lblDimensionsChargeValue.Refresh();
        lblQuantityChargeValue.Refresh();
        lblTotalValue.Refresh();
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
        header.Controls.Add(new RoundedIconPanel { Icon = Image.FromFile(iconPath), Size = new Size(32, 32), BorderRadius = 0 });
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
                    ValidateForm();
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