using CustomControls;
using System.Drawing.Drawing2D;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class BookTicketForm : Form
{
    private int WIDTH = 1920;
    private int HEIGHT = 1080;

    private IMessageFilter menuFilterFrom;
    private IMessageFilter menuFilterTo;
    private IMessageFilter menuFilterClass;

    private Station firstStation = null!;
    private Station lastStation = null!;
    private DateOnly? date = DateOnly.FromDateTime(DateTime.Now);
    private PassengerCarriegeType type = PassengerCarriegeType.AllClasses;

    private DropDownRoundedButton btnSearch;

    private FlowLayoutPanel fpTickets;


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

    public BookTicketForm(int width, int height)
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
        Text = string.Empty;
        ControlBox = false;

        var header = new Header(WIDTH, this)
        {
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = Color.White,
            Dock = DockStyle.Top,
            Height = 65,
            Padding = new Padding(0)
        };
        header.LogoClicked += () => GoToMain();
        Controls.Add(header);

        var lblBook = new Label
        {
            Size = new Size(500, 50),
            Text = "Book Train Tickets",
            Font = new Font("Segoe UI", 24f, FontStyle.Bold),
            Location = new Point(160, 100),
            TextAlign = ContentAlignment.MiddleLeft,
        };
        Controls.Add(lblBook);

        var mainPanel = new RoundedFlowLayoutPanel
        {
            Size = new Size(1600, 180),
            Location = new Point(160, 200),
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 18,
            
        };
        Controls.Add(mainPanel);

        var fpSearch = new FlowLayoutPanel
        {
            BackColor = Color.Transparent,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(25),
            Size = new Size(500, 30),
        };
        mainPanel.Controls.Add(fpSearch);

        var iconSearch = new RoundedIconPanel
        {
            Icon = Image.FromFile("Images/Search.png"),
            IconSize = new Size(20, 20),
            BackColor = Color.White,
            ForeColor = Color.Black,
            BorderRadius = 0,
            Size = new Size(20, 20),
            Padding = new Padding(0),
        };
        fpSearch.Controls.Add(iconSearch);

        var lblSearch = new Label
        {
            Margin = new Padding(5, 1, 0, 0),
            Size = new Size(200, 20),
            TextAlign = ContentAlignment.MiddleLeft,
            Text = "Search Trains",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.Transparent,
            ForeColor = Pallette.Text,
        };
        fpSearch.Controls.Add(lblSearch);

        var fpFilters = new FlowLayoutPanel
        {
            BackColor = Color.White,
            Size = new Size(1550, 80),
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(25, 0, 25, 15),
        };
        mainPanel.Controls.Add(fpFilters);

        var fpFrom = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(300, 70),
            BackColor = Color.Transparent,
            Margin = new Padding(5, 0, 5, 0)
        };
        fpFilters.Controls.Add(fpFrom);

        var lblFrom = new Label
        {
            Text = "From",
            Font = new Font("Segoe UI", 10f),
            BackColor = Color.Transparent,
            ForeColor = Pallette.Text,
        };
        fpFrom.Controls.Add(lblFrom);

        var btnFrom = new DropDownRoundedButton
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
            Text = "Select Station",
            ShowSelected = true,
            AllowDrop = true,
            Font = new Font("Segoe UI", 12f),
            ButtonTextFormat = TextFormatFlags.Left,

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,
        };
        var menuFrom = new AutoSizedMenuPanel();
        btnFrom.DropDownMenu = menuFrom;
        menuFrom.Width = 300;
        menuFrom.Visible = false;
        menuFrom.BorderRadius = 10;
        menuFrom.BorderSize = 1;
        menuFrom.BorderColor = Color.LightGray;
        Controls.Add(menuFrom);
        fpFrom.Controls.Add(btnFrom);

        var fpTo = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(300, 70),
            BackColor = Color.Transparent,
            Margin = new Padding(5, 0, 5, 0)

        };
        fpFilters.Controls.Add(fpTo);

        var lblTo = new Label
        {
            Text = "To",
            Font = new Font("Segoe UI", 10f),
            BackColor = Color.Transparent,
            ForeColor = Pallette.Text,
        };
        fpTo.Controls.Add(lblTo);

        var btnTo = new DropDownRoundedButton
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
            Text = "Select Station",
            ShowSelected = true,
            AllowDrop = true,
            Font = new Font("Segoe UI", 12f),
            ButtonTextFormat = TextFormatFlags.Left,

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,
        };
        var menuTo = new AutoSizedMenuPanel();
        btnTo.DropDownMenu = menuTo;
        menuTo.Width = 300;
        menuTo.Visible = false;
        menuTo.BorderRadius = 10;
        menuTo.BorderSize = 1;
        menuTo.BorderColor = Color.LightGray;
        Controls.Add(menuTo);
        btnTo.Click += (s, e) =>
        {
            if (menuTo.Visible == true)
            {
                menuTo.Visible = false;
                return;
            }

            menuTo.Clear();

            var stations = DB.GetAllStations();

            foreach (Station station in stations)
            {
                if (menuFrom.SelectedItem == null)
                    menuTo.AddItem(station.ToString(), () =>
                    {
                        menuTo.SelectedItem = new AutoSizedMenuPanel.MenuItemData(station.ToString());
                        menuTo.Hide();
                        lastStation = station;
                        ValidateSearchButton();
                        btnTo.Invalidate();
                    });
                else if (menuFrom.SelectedItem.Text != station.ToString())
                    menuTo.AddItem(station.ToString(), () =>
                    {
                        menuTo.SelectedItem = new AutoSizedMenuPanel.MenuItemData(station.ToString());
                        menuTo.Hide();
                        lastStation = station;
                        ValidateSearchButton();
                        btnTo.Invalidate();
                    });
            }

            menuTo.Location = new Point(btnTo.Left + 160 + 25 + 315, btnTo.Bottom + 200 + 85);
            menuTo.Visible = true;
            menuTo.BringToFront();

            if (menuFilterTo == null)
                menuFilterTo = new ClickOutsideMenuFilter(menuTo, btnTo);

            Application.AddMessageFilter(menuFilterTo);
        };
        btnFrom.Click += (s, e) =>
        {
            if (menuFrom.Visible == true)
            {
                menuFrom.Visible = false;
                return;
            }

            menuFrom.Clear();

            var stations = DB.GetAllStations();

            foreach (Station station in stations)
            {
                if (menuTo.SelectedItem == null)
                    menuFrom.AddItem(station.ToString(), () =>
                    {
                        menuFrom.SelectedItem = new AutoSizedMenuPanel.MenuItemData(station.ToString());
                        menuFrom.Hide();
                        firstStation = station;
                        ValidateSearchButton();
                        btnFrom.Invalidate();
                    });
                else if (menuTo.SelectedItem.Text != station.ToString())
                    menuFrom.AddItem(station.ToString(), () =>
                    {
                        menuFrom.SelectedItem = new AutoSizedMenuPanel.MenuItemData(station.ToString());
                        menuFrom.Hide();
                        firstStation = station;
                        ValidateSearchButton();
                        btnFrom.Invalidate();
                    });
            }

            menuFrom.Location = new Point(btnFrom.Left + 160 + 30, btnFrom.Bottom + 200 + 85);
            menuFrom.Visible = true;
            menuFrom.BringToFront();

            if (menuFilterFrom == null)
                menuFilterFrom = new ClickOutsideMenuFilter(menuFrom, btnFrom);

            Application.AddMessageFilter(menuFilterFrom);
        };
        fpTo.Controls.Add(btnTo);

        var fpDate = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(300, 70),
            BackColor = Color.Transparent,
            Margin = new Padding(5, 0, 5, 0)
        };
        fpFilters.Controls.Add(fpDate);

        var lblDate = new Label
        {
            Text = "Date",
            Font = new Font("Segoe UI", 10f),
            BackColor = Color.Transparent,
            ForeColor = Pallette.Text,
        };
        fpDate.Controls.Add(lblDate);

        var dpDate = new RoundedDatePicker
        {
            Size = new Size(300, 100),
            Padding = new Padding(0),
            Margin = new Padding(0),
        };
        fpDate.Controls.Add(dpDate);
        dpDate.ValueChanged += (s, e) =>
        {
            date = DateOnly.FromDateTime(dpDate.Value);
        };

        var fpClass = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(300, 70),
            BackColor = Color.Transparent,
            Margin = new Padding(5, 0, 5, 0)
        };
        fpFilters.Controls.Add(fpClass);

        var lblClass = new Label
        {
            Text = "Class",
            Font = new Font("Segoe UI", 10f),
            BackColor = Color.Transparent,
            ForeColor = Pallette.Text,
        };
        fpClass.Controls.Add(lblClass);

        var btnClass = new DropDownRoundedButton
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
            Text = "Select Class",
            ShowSelected = true,
            AllowDrop = true,
            Font = new Font("Segoe UI", 12f),
            ButtonTextFormat = TextFormatFlags.Left,

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,
        };
        var menuClass = new AutoSizedMenuPanel();
        btnClass.DropDownMenu = menuClass;
        menuClass.Width = 300;
        menuClass.Visible = false;
        menuClass.BorderRadius = 10;
        menuClass.BorderSize = 1;
        menuClass.BorderColor = Color.LightGray;
        Controls.Add(menuClass);
        btnClass.Click += (s, e) =>
        {
            if (menuClass.Visible == true)
            {
                menuClass.Visible = false;
                return;
            }

            menuClass.Clear();

            menuClass.AddItem("All Classes", () =>
            {
                menuClass.SelectedItem = new AutoSizedMenuPanel.MenuItemData("All Classes");
                menuClass.Hide();
                type = PassengerCarriegeType.AllClasses;
                btnClass.Invalidate();
            });
            menuClass.AddItem("Compartment", () =>
            {
                menuClass.SelectedItem = new AutoSizedMenuPanel.MenuItemData("Compartment");
                menuClass.Hide();
                type = PassengerCarriegeType.Compartment;
                btnClass.Invalidate();
            });
            menuClass.AddItem("Seat", () =>
            {
                menuClass.SelectedItem = new AutoSizedMenuPanel.MenuItemData("Seat");
                menuClass.Hide();
                type = PassengerCarriegeType.Seat;
                btnClass.Invalidate();
            });
            menuClass.AddItem("ReservedSeat", () =>
            {
                menuClass.SelectedItem = new AutoSizedMenuPanel.MenuItemData("ReservedSeat");
                menuClass.Hide();
                type = PassengerCarriegeType.ReservedSeat;
                btnClass.Invalidate();
            });

            menuClass.Location = new Point(btnClass.Left + 160 + 25 + 315*3 - 10, btnClass.Bottom + 200 + 85);
            menuClass.Visible = true;
            menuClass.BringToFront();

            if (menuFilterClass == null)
                menuFilterClass = new ClickOutsideMenuFilter(menuClass, btnClass);

            Application.AddMessageFilter(menuFilterClass);
        };
        fpClass.Controls.Add(btnClass);

        btnSearch = new DropDownRoundedButton
        {
            Margin = new Padding(5, 25, 5, 0),
            Padding = new Padding(100, 0, 0, 0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            DisabledForeColor = Color.White,
            HoverForeColor = Color.Black,
            PressedForeColor = Color.Black,
            BorderRadius = 10,
            BorderSize = 1,
            IconHeight = 16,
            Size = new Size(300, 40),
            Icon = Image.FromFile("Images/Search.png"),
            ButtonText = "Search",
            Font = new Font("Segoe UI", 12f),

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,

            Enabled = false,
            DisabledBorderColor = Color.Transparent,
        };
        fpFilters.Controls.Add(btnSearch);
        btnSearch.Click += BtnSearch_Click;

        fpTickets = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.Transparent,
            Size = new Size(1600, 400),
            Location = new Point(160, 400),
        };
        Controls.Add(fpTickets);

        var lblTickets = new Label
        {
            Text = "Available Tickets",
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            BackColor = Color.Transparent,
            ForeColor = Pallette.Text,
            Size = new Size(1600, 30),
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 20, 0, 20)
        };
        fpTickets.Controls.Add(lblTickets);
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        while (fpTickets.Controls.Count > 1) fpTickets.Controls.RemoveAt(1);

        var stis = DB.SearchSchedules(firstStation, lastStation, type);

        foreach (var sti in stis)
        {
            Train t = DB.GetById<Train>(sti.Schedule.TrainId);
            Route r = DB.GetById<Route>(sti.Schedule.RouteId);
            PassengerCarriege car = (PassengerCarriege)t.carrieges[sti.CarNum];

            var tPanel = new TicketInfoPanel();

            tPanel.TrainName = t.model;
            tPanel.TrainId = t.Id.ToString();
            tPanel.ClassType = car.type.ToString();
            tPanel.FromStation = r.routeStart.ToString();
            tPanel.ToStation = r.routeEnd.ToString();
            tPanel.DepartureTime = sti.Schedule.DepartureDateString;
            tPanel.ArrivalTime = sti.Schedule.ArrivalDateString;
            tPanel.Duration = sti.Schedule.DurationString;
            sti.CalcPrice();
            tPanel.Price = "$" + sti.Price.ToString();
            tPanel.SeatsLeft = car.freeSeats.ToString() + " seats left";

            tPanel.BookClicked += (panel) =>
            {
                var data = panel;

                var confirmPopup = new BookingConfirmationPanel(data);

                confirmPopup.Location = new Point(
                    (this.Width - confirmPopup.Width) / 2,
                    (this.Height - confirmPopup.Height) / 2
                );

                confirmPopup.CancelClicked += () => {
                    this.Controls.Remove(confirmPopup);
                };

                confirmPopup.ConfirmClicked += (email, passengers) => {
                    Ticket ticket = new Ticket();
                    ticket.UserId = AuthService.GetCurUserId();
                    ticket.CarNum = sti.CarNum;
                    ticket.Price = sti.Price;
                    PassengerCarriege car = (PassengerCarriege)DB.GetById<Train>(sti.Schedule.TrainId).carrieges[sti.CarNum];
                    ticket.Seat = car.GetFreeSeatNumber();
                    ticket.PurchaseDate = DateTime.Now;
                    ticket.ScheduleId = sti.Schedule.Id;

                    DB.SaveTicket(ticket);

                    car.freeSeats = car.freeSeats - 1;

                    this.Controls.Remove(confirmPopup);
                };

                this.Controls.Add(confirmPopup);
                confirmPopup.BringToFront();
            };

            fpTickets.Controls.Add(tPanel);
        }
    }

    private void GoToMain()
    {
        var main = new MainPageForm(WIDTH, HEIGHT);
        main.Show();
        Close();
    }

    private void ValidateSearchButton()
    {
        if (firstStation != null && lastStation != null && date != null)
            btnSearch.Enabled = true;
        else btnSearch.Enabled = false;
    }
}