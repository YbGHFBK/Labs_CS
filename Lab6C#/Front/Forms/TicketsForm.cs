using CustomControls;
using System.Drawing.Drawing2D;

public class TicketsForm : Form
{
    private FlowLayoutPanel fpList;

    public TicketsForm()
    {
        InitializeComponent();
        RefreshTicketList();
    }

    private void InitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(1920, 1080);
        BackColor = Color.FromArgb(245, 245, 245);

        var header = new Header(1920, this)
        {
            BackColor = Color.White,
            Dock = DockStyle.Top,
            Height = 65
        };
        header.LogoClicked += () => GoToMain();
        Controls.Add(header);

        fpList = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(1600, 900),
            Location = new Point(160, 120),
            AutoScroll = true
        };
        Controls.Add(fpList);
    }

    private void RefreshTicketList()
    {
        fpList.Controls.Clear();
        foreach (var t in DB.tickets)
        {
            var tPanel = new TicketPanel(t);
            fpList.Controls.Add(tPanel);
        }
    }

    private void GoToMain()
    {
        var main = new MainPageForm(1920, 1080);
        main.Show();
        this.Close();
    }
}

public class TicketPanel : UserControl
{
    public string TicketId { get; set; }
    public string RouteInfo { get; set; }
    public string TimeInfo { get; set; }
    public string Duration { get; set; }
    public string Price { get; set; }
    public string SeatInfo { get; set; }
    public string PassengerInfo { get; set; }

    private int borderRadius = 18;
    private Color borderColor = Color.LightGray;

    public TicketPanel(Ticket ticket)
    {
        this.DoubleBuffered = true;
        this.Size = new Size(1550, 160);
        this.BackColor = Color.White;
        this.Padding = new Padding(25);
        this.Margin = new Padding(0, 5, 0, 5);

        var user = DB.GetById<User>(ticket.UserId);

        var schedule = DB.GetById<Schedule>(ticket.ScheduleId);

        TicketId = $"TICKET #{ticket.Id}";
        PassengerInfo = $"Passenger: {user?.name ?? "Unknown"}";
        Price = $"${ticket.Price:F2}";
        SeatInfo = $"Carriage: {ticket.CarNum} | Seat: {ticket.Seat}";

        if (schedule != null)
        {
            var route = DB.GetById<Route>(schedule.RouteId);
            var train = DB.GetById<Train>(schedule.TrainId);

            RouteInfo = route != null ? $"{route.routeStart.city} → {route.routeEnd.city}" : "Route N/A";
            TimeInfo = $"{schedule.DepartureDateString} — {schedule.ArrivalDateString}";

            TimeSpan durationSpan = schedule.ArrivalDate - schedule.DepartureDate;
            if (durationSpan < TimeSpan.Zero) durationSpan = durationSpan.Add(TimeSpan.FromDays(1));
            Duration = $"Duration: {durationSpan.Hours}h {durationSpan.Minutes}m";
        }
        else
        {
            RouteInfo = "Schedule information not found";
            TimeInfo = "--:--";
            Duration = "";
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        Rectangle rectFull = new Rectangle(0, 0, this.Width, this.Height);
        using (GraphicsPath path = GetRoundPath(rectFull, borderRadius))
        {
            this.Region = new Region(path);
            using (SolidBrush brush = new SolidBrush(this.BackColor)) g.FillPath(brush, path);
            using (Pen pen = new Pen(borderColor, 1)) g.DrawPath(pen, path);
        }
        Font fontId = new Font("Segoe UI", 10f, FontStyle.Bold);
        Font fontRoute = new Font("Segoe UI", 16f, FontStyle.Bold);
        Font fontMain = new Font("Segoe UI", 12f, FontStyle.Regular);
        Font fontPrice = new Font("Segoe UI", 18f, FontStyle.Bold);
        Font fontLabel = new Font("Segoe UI", 9f, FontStyle.Regular);

        g.DrawString(TicketId, fontId, Brushes.Gray, 25, 20);
        g.DrawString(RouteInfo, fontRoute, Brushes.Black, 25, 45);
        g.DrawString(SeatInfo, fontMain, Brushes.Black, 25, 85);
        g.DrawString(PassengerInfo, fontLabel, Brushes.DimGray, 25, 115);

        g.DrawString("DEPARTURE - ARRIVAL", fontLabel, Brushes.Gray, 600, 30);
        g.DrawString(TimeInfo, fontMain, Brushes.Black, 600, 50);
        g.DrawString(Duration, fontMain, Brushes.DimGray, 600, 80);

        g.DrawString("TOTAL PRICE", fontLabel, Brushes.Gray, 1350, 30);
        g.DrawString(Price, fontPrice, Brushes.Crimson, 1350, 50);

        using (Pen linePen = new Pen(Color.FromArgb(230, 230, 230), 1))
        {
            g.DrawLine(linePen, 1300, 30, 1300, 130);
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