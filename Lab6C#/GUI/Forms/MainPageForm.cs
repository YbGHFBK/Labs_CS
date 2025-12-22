public class MainPageForm : Form
{
    private int WIDTH = 1920;
    private int HEIGHT = 1080;

    public MainPageForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(WIDTH, HEIGHT);
        Text = string.Empty;
        ControlBox = false;

        var header = new Header(WIDTH)
        {
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = Color.White,
            Dock = DockStyle.Top,
            Height = 65,
            Padding = new Padding(0)
        };
        Controls.Add(header);

        var dd = new DropDownRoundedButton
        {
            Location = new Point(200, 200),
            Size = new Size(140, 34),
            ButtonText = "Settings",
            Icon = Image.FromFile("Images/Settings.png"),
            BorderRadius = 10,
            ForeColor = Color.Black,
            BorderSize = 1
        };

        var cms = new ContextMenuStrip();
        cms.Items.Add("Option 1", null, (s, e) => MessageBox.Show("Option 1"));
        cms.Items.Add("Option 2", null, (s, e) => MessageBox.Show("Option 2"));
        dd.DropDownMenu = cms;

        dd.Click += (s, e) => cms.Show();

        this.Controls.Add(dd);
    }
}