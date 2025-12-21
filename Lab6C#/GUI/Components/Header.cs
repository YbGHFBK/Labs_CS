using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

public class Header : FlowLayoutPanel
{

    private int WIDTH;
    private int HEIGHT = 65;

    public Header(int w) : base()
    {
        WIDTH = w;

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        System.Windows.Forms.Button btnLogo = new System.Windows.Forms.Button
        {
            Text = string.Empty,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Transparent,
            ForeColor = Color.Transparent,

            Size = new Size(130, 50),

            BackgroundImage = Image.FromFile("Images/Logo.png"),
            BackgroundImageLayout = ImageLayout.Stretch,

            FlatAppearance =
            {
                BorderSize = 0,
                MouseOverBackColor = Color.Transparent,
                MouseDownBackColor = Color.Transparent
            }
        };
        Controls.Add(btnLogo);

        var il = new ImageList
        {
            ImageSize = new Size(20, 20),
        };
        il.Images.Add(Image.FromFile("Images/Settings.png"));
        il.Images.Add(Image.FromFile("Images/Profile.png"));

        var btnSettings = new RoundedButton()
        {
            Margin = new Padding(WIDTH - 560, 18, 0, 0),
            Padding = new Padding(5, 0, 0, 0),
            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(120, 30),
            ImageList = il,
            ImageIndex = 0,
            Text = "Settings \u25BE",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ImageAlign = ContentAlignment.MiddleLeft,
            TextAlign = ContentAlignment.MiddleLeft,
            TextImageRelation = TextImageRelation.ImageBeforeText,
        };
        Controls.Add(btnSettings);

        var btnProfile = new RoundedButton()
        {
            Margin = new Padding(20, 18, 0, 0),
            Padding = new Padding(5, 0, 0, 0),
            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(120, 30),
            ImageList = il,
            ImageIndex = 1,
            Text = "Account \u25BE",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ImageAlign = ContentAlignment.MiddleLeft,
            TextAlign = ContentAlignment.MiddleLeft,
            TextImageRelation = TextImageRelation.ImageBeforeText,
        };
        Controls.Add(btnProfile);

        var closeButton = new RoundedButton
        {
            Margin = new Padding(20, 18, 0, 0),
            Padding = new Padding(3, 0, 0, 0),
            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(30, 30),
            Text = "×",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),

            FlatAppearance =
            {
                MouseOverBackColor = Color.DarkRed,
                MouseDownBackColor = Color.IndianRed
            }
        };
        Controls.Add(closeButton);
        closeButton.Click += CloseButton_Click;

        var menu = new ContextMenuStrip();
        menu.Items.Add("Profile", null, (_, __) => MessageBox.Show("Profile"));
        menu.Items.Add("Security", null, (_, __) => MessageBox.Show("Security"));
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Logout", null, (_, __) => MessageBox.Show("Logout"));

        btnSettings.Click += (s, e) =>
        {
            menu.Show(btnSettings, new Point(0, btnSettings.Height));
        };

    }

    private void CloseButton_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }
}