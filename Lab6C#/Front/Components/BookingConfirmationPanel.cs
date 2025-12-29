using CustomControls;
using System.Drawing.Drawing2D;

public class BookingConfirmationPanel : RoundedFlowLayoutPanel
{
    public event Action<string, int> ConfirmClicked;
    public event Action CancelClicked;

    private RoundedTextBox tbEmail;
    private RoundedTextBox tbPassengers;

    public BookingConfirmationPanel(TicketInfoPanel data)
    {
        this.Size = new Size(500, 600);
        this.FlowDirection = FlowDirection.TopDown;
        this.WrapContents = false;
        this.Padding = new Padding(30);
        this.BackColor = Color.White;
        this.BorderRadius = 10;
        this.BorderSize = 1;
        this.BorderColor = Color.LightGray;

        InitializeComponents(data);
    }

    private void InitializeComponents(TicketInfoPanel data)
    {
        var lblTitle = new Label
        {
            Text = "Complete Your Booking",
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(20, 0, 0, 15)
        };

        var lblSubtitle = new Label
        {
            Text = "Trip Details",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(20, 0, 0, 10)
        };

        var detailsCard = new RoundedFlowLayoutPanel
        {
            Size = new Size(440, 200),
            BackColor = Color.FromArgb(240, 242, 245),
            BorderRadius = 15,
            BorderSize = 0,
            Padding = new Padding(20),
            FlowDirection = FlowDirection.TopDown,
            Margin = new Padding(20, 0, 0, 20)
        };

        AddDetailRow(detailsCard, "Train:", $"{data.TrainId} ({data.TrainName})");
        AddDetailRow(detailsCard, "Route:", $"{data.FromStation} → {data.ToStation}");
        AddDetailRow(detailsCard, "Date:", DateTime.Now.ToString("yyyy-MM-dd"));
        AddDetailRow(detailsCard, "Class:", data.ClassType);

        var line = new Label { BorderStyle = BorderStyle.Fixed3D, Height = 2, Width = 400, Margin = new Padding(0, 10, 0, 10) };
        detailsCard.Controls.Add(line);

        AddDetailRow(detailsCard, "Total:", $"{data.Price}", true);

        var lblPass = new Label { Text = "Number of Passengers", Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true, Margin = new Padding(20, 5, 0, 25) };
        tbPassengers = new RoundedTextBox { TbText = "1", Width = 440, Margin = new Padding(20, 5, 0, 15) };

        var lblEmail = new Label { Text = "Email Address", Font = new Font("Segoe UI", 10f, FontStyle.Bold), AutoSize = true, Margin = new Padding(20, 5, 0, 25) };
        tbEmail = new RoundedTextBox { PlaceholderText = "your@email.com", Width = 440, Margin = new Padding(20, 5, 0, 25) };


        var buttonPanel = new FlowLayoutPanel { Size = new Size(440, 50), FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0) };

        var btnCancel = new DropDownRoundedButton
        {
            ButtonText = "Cancel",
            Size = new Size(200, 45),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 10,
            Margin = new Padding(15, 0, 10, 0)
        };
        btnCancel.Click += (s, e) => CancelClicked?.Invoke();

        var btnConfirm = new DropDownRoundedButton
        {
            ButtonText = "Confirm Booking",
            Size = new Size(200, 45),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 10,
            Margin = new Padding(0)
        };
        btnConfirm.Click += (s, e) => {
            if (int.TryParse(tbPassengers.TbText, out int count))
                ConfirmClicked?.Invoke(tbEmail.TbText, count);
        };

        buttonPanel.Controls.AddRange(new Control[] { btnCancel, btnConfirm });

        this.Controls.AddRange(new Control[] { lblTitle, lblSubtitle, detailsCard, lblPass, tbPassengers, lblEmail, tbEmail, buttonPanel });
    }

    private void AddDetailRow(Control parent, string title, string value, bool isTotal = false)
    {
        var row = new Panel { Width = 400, Height = 25, Margin = new Padding(20, 0, 0, 0), Location = new Point(20, 60)};
        var lblT = new Label { Text = title, ForeColor = Color.Gray, AutoSize = true, Location = new Point(0, 0) };
        var lblV = new Label
        {
            Text = value,
            TextAlign = ContentAlignment.TopRight,
            Width = 300,
            Location = new Point(100, 0),
            Font = new Font("Segoe UI", isTotal ? 12f : 10f, isTotal ? FontStyle.Bold : FontStyle.Regular)
        };
        row.Controls.Add(lblT);
        row.Controls.Add(lblV);
        parent.Controls.Add(row);
    }
}