using System.Drawing.Drawing2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

public class Header : FlowLayoutPanel
{

    private int WIDTH;
    private int HEIGHT = 65;

    private IMessageFilter menuFilter;

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

    public Header(int w, Control? parent) : base()
    {
        WIDTH = w;
        Parent = parent;

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

        var btnLogo = new System.Windows.Forms.Button
        {
            Margin = new Padding(50, 0, 0, 0),
            Padding = new Padding(0),
            ForeColor = Color.Black,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            FlatAppearance =
            {
                BorderSize = 0,
            },
            Height = 65,
            Size = new Size(235, 65),
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
            Margin = new Padding(WIDTH - 670, 13, 0, 0),
            Padding = new Padding(5, 0, 0, 0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(130, 40),
            Icon = Image.FromFile("Images/Settings.png"),
            ButtonText = "Settings \u25BE",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,

        };
        Controls.Add(btnSettings);

        var btnProfile = new DropDownRoundedButton
        {
            Margin = new Padding(20, 13, 0, 0),
            Padding = new Padding(5, 0, 0, 0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(130, 40),
            Icon = Image.FromFile("Images/Profile.png"),
            ButtonText = "Account \u25BE",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,

        };
        Controls.Add(btnProfile);

        var closeButton = new DropDownRoundedButton
        {
            Margin = new Padding(20, 13, 0, 0),
            //Padding = new Padding(7, 0, 0, 0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Size = new Size(40, 40),
            Icon = null,
            ButtonText = "×",
            Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
            ButtonTextFormat = TextFormatFlags.HorizontalCenter,

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,

        };
        Controls.Add(closeButton);
        closeButton.Click += CloseButton_Click;

        var menu = new AutoSizedMenuPanel();
        menu.Width = 130;
        menu.AddItem("Profile", () => MessageBox.Show("Profile!!"), Image.FromFile("Images/Train.png"));
        menu.AddItem("Logout", () => SettingsButton_Logout());
        menu.Visible = false;
        Parent.Controls.Add(menu);

        btnSettings.Click += (s, e) =>
        {
            if (menu.Visible)
            {
                menu.Hide();
                return;
            }

            menu.Location = new Point(btnSettings.Left, btnSettings.Bottom);
            menu.Visible = true;
            menu.BringToFront();

            if (menuFilter == null)
                menuFilter = new ClickOutsideMenuFilter(menu, btnSettings);

            Application.AddMessageFilter(menuFilter);
        };

    }

    private void CloseButton_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    private void SettingsButton_Logout()
    {
        Parent.Hide();

        var log = new LoginForm(600, 840);
        log.FormClosed += (s, args) =>
        {
            Show();
        };
        log.Show();
    }
}