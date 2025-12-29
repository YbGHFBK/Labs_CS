using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;

public class TicketInfoPanel : UserControl
{
    public string TrainName { get; set; } = "Express Liner";
    public string TrainId { get; set; } = "EX-101";
    public string ClassType { get; set; } = "Coupe";
    public string FromStation { get; set; } = "Paris, France";
    public string ToStation { get; set; } = "New York, USA";
    public string DepartureTime { get; set; } = "08:00";
    public string ArrivalTime { get; set; } = "14:30";
    public string Duration { get; set; } = "6h 30m";
    public string Price { get; set; } = "$89";
    public string SeatsLeft { get; set; } = "45 seats left";

    private int borderRadius = 18;
    private Color borderColor = Color.LightGray;

    private DropDownRoundedButton btnBook;

    public TicketInfoPanel()
    {
        this.DoubleBuffered = true;
        this.Size = new Size(1600, 140);
        this.BackColor = Color.White;
        this.Padding = new Padding(20);
        this.Margin = new Padding(0);

        InitializeButton();
    }

    private void InitializeButton()
    {
        btnBook = new DropDownRoundedButton
        {
            Margin = new Padding(0),
            Padding = new Padding(0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(100, 40),
            Icon = null,
            ButtonText = "Book Now",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,

        };

        btnBook.Click += BtnBook_Click;

        this.Controls.Add(btnBook);

        UpdateButtonLocation();
    }

    private void BtnBook_Click(object? sender, EventArgs e)
    {
        
    }

    private void UpdateButtonLocation()
    {
        if (btnBook != null)
        {
            btnBook.Location = new Point(this.Width - btnBook.Width - 55, this.Height - btnBook.Height - 20);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        UpdateButtonLocation();
        this.Invalidate();
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

        DrawTicketData(g);
    }

    private void DrawTicketData(Graphics g)
    {
        Font fontBold = new Font("Segoe UI", 12f, FontStyle.Bold);
        Font fontRegular = new Font("Segoe UI", 10f, FontStyle.Regular);
        Font fontSmall = new Font("Segoe UI", 9f, FontStyle.Regular);
        Font fontMedium = new Font("Segoe UI", 12f, FontStyle.Regular);
        Font fontTime = new Font("Segoe UI", 16f, FontStyle.Bold);
        Font fontPrice = new Font("Segoe UI", 18f, FontStyle.Bold);

        g.DrawString(TrainName, fontBold, Brushes.Black, 25, 35);
        DrawTag(g, TrainId, 145, 38, fontSmall);
        DrawTag(g, ClassType, 210, 38, fontSmall);

        g.DrawString(FromStation, fontRegular, Brushes.Gray, 25, 75);
        g.DrawString("→", fontRegular, Brushes.Gray, 165, 75);
        g.DrawString(ToStation, fontRegular, Brushes.Gray, 195, 75);

        int center = 1150;
        int centerH = 45;
        g.DrawString(DepartureTime, fontTime, Brushes.Black, center, centerH);
        g.DrawString("Departure", fontSmall, Brushes.Gray, center + 5, centerH + 30);

        g.DrawString(Duration, fontMedium, Brushes.Gray, center + 95, centerH + 8);

        g.DrawString(ArrivalTime, fontTime, Brushes.Black, center + 180, centerH);
        g.DrawString("Arrival", fontSmall, Brushes.Gray, center + 185, centerH + 30);

        int rightAlign = this.Width - 145;
        g.DrawString(Price, fontPrice, Brushes.Black, rightAlign, 25);
        g.DrawString(SeatsLeft, fontSmall, Brushes.Gray, rightAlign - 5, 60);
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
        if (d <= 0) { path.AddRectangle(r); return path; }
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}