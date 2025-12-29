using CustomControls;
using System.Drawing.Drawing2D;

public class CarriegesForm : Form
{
    private Train _currentTrain;
    private IMessageFilter menuFilterSubType;
    private object SelectedCarSubType;

    private RoundedFlowLayoutPanel mainPanel;
    private FlowLayoutPanel fpList;

    public CarriegesForm(Train train)
    {
        _currentTrain = train;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(1920, 1080);
        BackColor = Color.FromArgb(245, 245, 245);

        var header = new Header(1920, this) { Dock = DockStyle.Top, Height = 65, BackColor = Color.White };
        Controls.Add(header);

        var lblTitle = new Label
        {
            Text = $"Carrieges of {_currentTrain.model}",
            Font = new Font("Segoe UI", 24f, FontStyle.Bold),
            Location = new Point(160, 100),
            AutoSize = true
        };
        Controls.Add(lblTitle);

        mainPanel = CreateCarriegeEditor();
        Controls.Add(mainPanel);

        fpList = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(1600, 750),
            Location = new Point(160, 220),
            AutoScroll = true
        };
        Controls.Add(fpList);

        RefreshList();
    }

    private RoundedFlowLayoutPanel CreateCarriegeEditor()
    {
        var panel = new RoundedFlowLayoutPanel
        {
            Size = new Size(1600, 600),
            Location = new Point(160, 200),
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.White,
            BorderRadius = 18,
            Padding = new Padding(25),
            Visible = false
        };

        panel.Controls.Add(new Label { Text = "Capacity", Font = new Font("Segoe UI", 12f), AutoSize = true });
        var tbCap = new RoundedTextBox { Width = 300, Margin = new Padding(0, 0, 0, 15) };
        panel.Controls.Add(tbCap);

        panel.Controls.Add(new Label { Text = "Type", Font = new Font("Segoe UI", 12f), AutoSize = true });
        var btnSub = new DropDownRoundedButton { Size = new Size(300, 40), Text = "Select Type", BorderRadius = 10 };
        var menuSub = new AutoSizedMenuPanel { Width = 300, Visible = false };
        Controls.Add(menuSub);
        panel.Controls.Add(btnSub);

        btnSub.Click += (s, e) => {
            menuSub.Clear();
            if (_currentTrain.type == TrainType.Passenger)
                foreach (PassengerCarriegeType t in Enum.GetValues(typeof(PassengerCarriegeType)))
                    menuSub.AddItem(t.ToString(), () => { SelectedCarSubType = t; btnSub.Text = t.ToString(); menuSub.Hide(); });
            else
                foreach (CargoCarriegeType t in Enum.GetValues(typeof(CargoCarriegeType)))
                    menuSub.AddItem(t.ToString(), () => { SelectedCarSubType = t; btnSub.Text = t.ToString(); menuSub.Hide(); });

            menuSub.Location = this.PointToClient(panel.PointToScreen(new Point(btnSub.Left, btnSub.Bottom)));
            menuSub.Visible = true; menuSub.BringToFront();
        };

        var btnSave = new DropDownRoundedButton
        {
            Size = new Size(200, 45),
            ButtonText = "Save Carriege",
            NormalBackColor = Color.Crimson,
            ForeColor = Color.White,
            BorderRadius = 14
        };
        btnSave.Click += (s, e) => {
            int cap = int.Parse(tbCap.TbText);
            if (_currentTrain.type == TrainType.Passenger)
                _currentTrain.Add(new PassengerCarriege(cap, (PassengerCarriegeType)SelectedCarSubType));
            else
                _currentTrain.Add(new CargoCarriege(cap) { type = (CargoCarriegeType)SelectedCarSubType });

            DB.Save();

            panel.Visible = false; fpList.Visible = true; RefreshList();
        };
        panel.Controls.Add(btnSave);

        return panel;
    }

    private void RefreshList()
    {
        fpList.Controls.Clear();

        var btnAdd = new DropDownRoundedButton
        {
            Size = new Size(300, 45),
            ButtonText = "+ Add New Carriege",
            BorderRadius = 10,
            Margin = new Padding(0, 0, 0, 20)
        };
        btnAdd.Click += (s, e) => { fpList.Visible = false; mainPanel.Visible = true; };
        fpList.Controls.Add(btnAdd);

        var btnBack = new DropDownRoundedButton
        {
            Size = new Size(150, 45),
            ButtonText = "← Back",
            NormalBackColor = Color.Gray,
            ForeColor = Color.White,
            BorderRadius = 10
        };
        btnBack.Click += (s, e) => {
            new TrainsForm().Show();
            this.Close();
        };
        fpList.Controls.Add(btnBack);

        foreach (var car in _currentTrain.carrieges)
        {
            fpList.Controls.Add(new CarriegeItemPanel(car));
        }
    }
}

public class CarriegeItemPanel : UserControl
{
    private Carriege _car;

    public CarriegeItemPanel(Carriege car)
    {
        _car = car;
        this.Size = new Size(1550, 100);
        this.BackColor = Color.White;
        this.Margin = new Padding(0, 0, 0, 10);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

        using (GraphicsPath path = GetRoundPath(rect, 15))
        {
            this.Region = new Region(path);
            g.DrawPath(Pens.LightGray, path);
        }

        g.DrawString(_car.GetClass(), new Font("Segoe UI", 12f, FontStyle.Bold), Brushes.Black, 20, 20);
        g.DrawString($"Capacity: {_car.carryingCapacity} | Type: {_car.GetCarSpecType()}",
            new Font("Segoe UI", 10f), Brushes.Gray, 20, 50);
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