using CustomControls;
using System.Xml.Linq;

public class SchedulesForm : Form
{
    private IMessageFilter menuFilterRoute;
    private IMessageFilter menuFilterTrain;
    private IMessageFilter menuFilterClass;

    private Route Route;
    private Train Train;
    private TimeOnly DepTime;
    private TimeOnly ArTime;

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

                bool clickOnMenu =
                    menu.RectangleToScreen(menu.ClientRectangle).Contains(p);

                bool clickOnButton =
                    button.RectangleToScreen(button.ClientRectangle).Contains(p);

                if (!clickOnMenu && !clickOnButton)
                {
                    menu.Hide();
                }
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
        Text = string.Empty;
        ControlBox = false;

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

        var mainPanel = new RoundedFlowLayoutPanel
        {
            Size = new Size(1600, 600),
            Location = new Point(160, 200),
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 18,
            Padding = new Padding(25),

        };
        Controls.Add(mainPanel);

        var lblRoute = new Label
        {
            Text = "Route",
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 12f),
        };
        mainPanel.Controls.Add(lblRoute);

        var btnRoute = new DropDownRoundedButton
        {
            Margin = new Padding(0),
            Padding = new Padding(0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(300, 40),
            Icon = null,
            Text = "Select Route",
            ShowSelected = true,
            AllowDrop = true,
            Font = new Font("Segoe UI", 12f),
            ButtonTextFormat = TextFormatFlags.Left,

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,
        };
        var menuRoute = new AutoSizedMenuPanel();
        btnRoute.DropDownMenu = menuRoute;
        menuRoute.Width = 300;
        menuRoute.Visible = false;
        menuRoute.BorderRadius = 10;
        menuRoute.BorderSize = 1;
        menuRoute.BorderColor = Color.LightGray;
        Controls.Add(menuRoute);
        mainPanel.Controls.Add(btnRoute);

        btnRoute.Click += (s, e) =>
        {
            if (menuRoute.Visible == true)
            {
                menuRoute.Visible = false;
                return;
            }

            menuRoute.Clear();

            var routes = DB.routes;

            foreach (Route route in routes)
            {
                menuRoute.AddItem(route.ToString(), () =>
                {
                    menuRoute.SelectedItem = new AutoSizedMenuPanel.MenuItemData(route.ToString());
                    Route = route;
                    menuRoute.Hide();
                });
            }

            menuRoute.Location = new Point(btnRoute.Left + 160, btnRoute.Bottom + 200);
            menuRoute.Visible = true;
            menuRoute.BringToFront();

            if (menuFilterRoute == null)
                menuFilterRoute = new ClickOutsideMenuFilter(menuRoute, btnRoute);

            Application.AddMessageFilter(menuFilterRoute);
        };





        var lblTrain = new Label
        {
            Text = "Train",
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 12f),
        };
        mainPanel.Controls.Add(lblTrain);

        var btnTrain = new DropDownRoundedButton
        {
            Margin = new Padding(0),
            Padding = new Padding(0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(300, 40),
            Icon = null,
            Text = "Select Train",
            ShowSelected = true,
            AllowDrop = true,
            Font = new Font("Segoe UI", 12f),
            ButtonTextFormat = TextFormatFlags.Left,

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,
        };
        var menuTrain = new AutoSizedMenuPanel();
        btnTrain.DropDownMenu = menuTrain;
        menuTrain.Width = 300;
        menuTrain.Visible = false;
        menuTrain.BorderRadius = 10;
        menuTrain.BorderSize = 1;
        menuTrain.BorderColor = Color.LightGray;
        Controls.Add(menuTrain);
        mainPanel.Controls.Add(btnTrain);

        btnTrain.Click += (s, e) =>
        {
            if (menuTrain.Visible == true)
            {
                menuTrain.Visible = false;
                return;
            }

            menuTrain.Clear();

            var trains = DB.trains;

            foreach (Train train in trains)
            {
                menuTrain.AddItem(train.ToString(), () =>
                {
                    menuTrain.SelectedItem = new AutoSizedMenuPanel.MenuItemData(train.ToString());
                    Train = train;
                    menuTrain.Hide();
                });
            }

            menuTrain.Location = new Point(btnRoute.Left + 160, btnTrain.Bottom + 200);
            menuTrain.Visible = true;
            menuTrain.BringToFront();

            if (menuFilterTrain == null)
                menuFilterTrain = new ClickOutsideMenuFilter(menuTrain, btnTrain);

            Application.AddMessageFilter(menuFilterTrain);
        };





        var lblDep = new Label
        {
            Text = "Departure Time",
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 12f),
        };
        mainPanel.Controls.Add(lblDep);

        var tbDep = new RoundedTextBox
        {
            PlaceholderText = "",
            Width = 200,
            ForeColor = Pallette.Text,
            Padding = new Padding(8),
            Margin = new Padding(0, 0, 0, 20),
        };
        mainPanel.Controls.Add(tbDep);

        var lblAr = new Label
        {
            Text = "Arrival Time",
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 12f),
        };
        mainPanel.Controls.Add(lblAr);

        var tbAr = new RoundedTextBox
        {
            PlaceholderText = "",
            Width = 200,
            ForeColor = Pallette.Text,
            Padding = new Padding(8),
            Margin = new Padding(0, 0, 0, 20),
        };
        mainPanel.Controls.Add(tbAr);

        var btnSave = new DropDownRoundedButton
        {
            Margin = new Padding(0),
            Size = new Size(200, 40),
            ButtonText = "Save",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            NormalBackColor = Pallette.MainAccent,
            HoverBackColor = Pallette.DarkAccent,
            PressedBackColor = Pallette.DarkAccent,
            ForeColor = Color.White,
            BorderSize = 0,
            BorderRadius = 14,
            Icon = null,
            ButtonTextFormat = TextFormatFlags.HorizontalCenter,
        };
        mainPanel.Controls.Add(btnSave);

        mainPanel.Hide();



















        var fpList = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(1600, 860),
            Location = new Point(160, 220),
        };
        Controls.Add(fpList);

        var btnAdd = new DropDownRoundedButton
        {
            Margin = new Padding(0),
            Padding = new Padding(0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(300, 40),
            Icon = null,
            ButtonText = "Add New Schedule",
            ShowSelected = true,
            AllowDrop = true,
            Font = new Font("Segoe UI", 12f),
            ButtonTextFormat = TextFormatFlags.Left,

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,
        };
        fpList.Controls.Add(btnAdd);
        btnAdd.Click += (s, e) =>
        {
            fpList.Hide();
            mainPanel.Show();
        };

        Action refreshList = () =>
        {
            while (fpList.Controls.Count > 1) fpList.Controls.RemoveAt(1);

            var schedules = DB.schedules;

            foreach (var sc in schedules)
            {
                var scPanel = new SchedulePanel(sc);
                fpList.Controls.Add(scPanel);
            }
        };

        refreshList();

        btnSave.Click += (s, e) =>
        {
            if (Train != null && Route != null && !string.IsNullOrEmpty(tbDep.TbText) && !string.IsNullOrEmpty(tbAr.TbText))
            {
                DB.createSchedule(Route, Train, TimeOnly.Parse(tbDep.TbText), TimeOnly.Parse(tbAr.TbText));

                refreshList();

                mainPanel.Hide();
                fpList.Show();
            }
        };
    }
    private void GoToMain()
    {
        var main = new MainPageForm(1920, 1080);
        main.Show();
        Close();
    }
}