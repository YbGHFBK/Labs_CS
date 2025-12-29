public class MainPageForm : Form
{
    private int WIDTH = 1920;
    private int HEIGHT = 1080;

    public MainPageForm(int width, int height)
    {
        Width = width;
        Height = height;
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
        Controls.Add(header);

        if (!AuthService.IsAdmin())
        {
            var btnBook = new DropDownRoundedButton
            {
                Location = new Point(300, 300),
                Margin = new Padding(300),
                Size = new Size(200, 40),
                ButtonText = "Book your journey",
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
            btnBook.Click += (s, e) =>
            {
                Hide();
                var bookF = new BookTicketForm(WIDTH, HEIGHT);
                bookF.Show();
            };
            Controls.Add(btnBook);
        }

        if (!AuthService.IsAdmin())
        {
            var btnViewTickets = new DropDownRoundedButton
            {
                Location = new Point(300, 350),
                Margin = new Padding(300),
                Size = new Size(200, 40),
                ButtonText = "View your tickets",
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
            btnViewTickets.Click += (s, e) =>
            {
                Hide();
                var ticketsf = new TicketsForm();
                ticketsf.Show();
            };
            Controls.Add(btnViewTickets);
        }

            if (AuthService.IsAdmin())
        {
            var btnSchedules = new DropDownRoundedButton
            {
                Location = new Point(300, 350),
                Margin = new Padding(300),
                Size = new Size(200, 40),
                ButtonText = "Schedules",
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
            btnSchedules.Click += (s, e) =>
            {
                Hide();
                var schedulesF = new SchedulesForm();
                schedulesF.Show();
            };
            Controls.Add(btnSchedules);
        }

        if (AuthService.IsAdmin())
        {
            var btnStations = new DropDownRoundedButton
            {
                Location = new Point(300, 400),
                Margin = new Padding(300),
                Size = new Size(200, 40),
                ButtonText = "Stations",
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
            btnStations.Click += (s, e) =>
            {
                Hide();
                var stationsF = new StationsForm();
                stationsF.Show();
            };
            Controls.Add(btnStations);
        }

        if (AuthService.IsAdmin())
        {
            var btnRoutes = new DropDownRoundedButton
            {
                Location = new Point(300, 450),
                Margin = new Padding(300),
                Size = new Size(200, 40),
                ButtonText = "Routes",
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
            btnRoutes.Click += (s, e) =>
            {
                Hide();
                var routesF = new RoutesForm();
                routesF.Show();
            };
            Controls.Add(btnRoutes);
        }

        if (AuthService.IsAdmin())
        {
            var btnTrains = new DropDownRoundedButton
            {
                Location = new Point(300, 500),
                Margin = new Padding(300),
                Size = new Size(200, 40),
                ButtonText = "Trains",
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
            btnTrains.Click += (s, e) =>
            {
                Hide();
                var trainsF = new TrainsForm();
                trainsF.Show();
            };
            Controls.Add(btnTrains);
        }

    }
}