using System.Drawing.Drawing2D;
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
        //System.Windows.Forms.Button btnLogo = new System.Windows.Forms.Button
        //{
        //    Text = string.Empty,
        //    FlatStyle = FlatStyle.Flat,
        //    BackColor = Color.Transparent,
        //    ForeColor = Color.Transparent,

        //    Size = new Size(130, 50),
        //    Margin = new Padding(50, 8, 0, 0),

        //    BackgroundImage = Image.FromFile("Images/Logo.png"),
        //    BackgroundImageLayout = ImageLayout.Stretch,

        //    FlatAppearance =
        //    {
        //        BorderSize = 0,
        //        MouseOverBackColor = Color.Transparent,
        //        MouseDownBackColor = Color.Transparent
        //    }
        //};
        var il1 = new ImageList
        {
            ImageSize = new Size(60, 60),
        };
        il1.Images.Add(Image.FromFile("Images/Train.png"));

        var btnLogo = new RoundedButton
        {
            Margin = new Padding(50, 0, 0, 0),
            Padding = new Padding(0),
            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderRadius = 0,
            BorderSize = 0,
            Size = new Size(200, 65),
            ImageList = il1,
            ImageIndex = 0,
            Text = "MoveCore",
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ImageAlign = ContentAlignment.MiddleLeft,
            TextAlign = ContentAlignment.MiddleLeft,
            TextImageRelation = TextImageRelation.ImageBeforeText,
        };
        Controls.Add(btnLogo);

        var il2 = new ImageList
        {
            ImageSize = new Size(25, 25),
        };
        il2.Images.Add(Image.FromFile("Images/Settings.png"));
        il2.Images.Add(Image.FromFile("Images/Profile.png"));

        var btnSettings = new DropDownRoundedButton
        {
            Margin = new Padding(WIDTH - 600, 13, 0, 0),
            Padding = new Padding(5, 0, 0, 0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.Red,
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(125, 40),
            Icon = Image.FromFile("Images/Settings.png"),
            ButtonText = "Settings \u25BE",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold)
        };
        Controls.Add(btnSettings);

        var btnProfile = new RoundedButton()
        {
            Margin = new Padding(20, 13, 0, 0),
            Padding = new Padding(5, 0, 0, 0),
            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.Red,
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(120, 40),
            ImageList = il2,
            ImageIndex = 1,
            Text = "Account \u25BE",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ImageAlign = ContentAlignment.MiddleLeft,
            TextAlign = ContentAlignment.MiddleLeft,
            TextImageRelation = TextImageRelation.ImageBeforeText,

            FlatAppearance =
            {
                MouseOverBackColor = Color.FromArgb(254,242,242),
                MouseDownBackColor = Color.FromArgb(254,242,242)
            }
        };
        Controls.Add(btnProfile);

        var closeButton = new RoundedButton
        {
            Margin = new Padding(20, 13, 0, 0),
            Padding = new Padding(3, 0, 0, 0),
            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.Red,
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(40, 40),
            Text = "×",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),

            FlatAppearance =
            {
                MouseOverBackColor = Color.FromArgb(254,242,242),
                MouseDownBackColor = Color.FromArgb(254,242,242)
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