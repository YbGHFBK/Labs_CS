public class RegisterForm : AuthStyleForm
{
    private readonly int WIDTH = 600;
    private readonly int HEIGHT = 840;
    private readonly int SIDEPAD = 50;

    private FlowLayoutPanel flowPanel;

    private Label lblWelcome;
    private Label lblReg;

    private Label lblName;
    private RoundedTextBox txtName;
    private Label lblPassword;
    private RoundedTextBox txtPassword;
    private Label lblRepPassword;
    private RoundedTextBox txtRepPassword;

    private DropDownRoundedButton btnReg;
    private Button btnToLog;
    private Label lblError;

    public RegisterForm(int Width, int Height) : base(Width, Height)
    {
        WIDTH = Width;
        HEIGHT = Height;
        btnToLog.Click += BtnToLog_Click;
    }

    protected override void InitializeComponent()
    {
        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        flowPanel = new FlowLayoutPanel()
        {
            Dock = DockStyle.Top,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowOnly,
            Anchor = AnchorStyles.None,
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.Transparent,
            Padding = new Padding(50, 0, 50, 0),
            Margin = new Padding(0),
        };
        table.Controls.Add(flowPanel, 0, 0);
        this.Controls.Add(table);

        var userIcon = new RoundedIconPanel
        {
            Margin = new Padding(0),
            Anchor = AnchorStyles.None,
            Size = new Size(100, 100),
            BorderRadius = 25,
            BackColor = Color.White,
            ForeColor = Pallette.MainAccent,
            BackgroundColor = Pallette.MainAccent,
            Icon = Image.FromFile("Images/User.png"),
            IconSize = new Size(40, 40),
            Text = string.Empty,

        };
        flowPanel.Controls.Add(userIcon);

        lblWelcome = new Label
        {
            AutoSize = true,
            Text = "Create Account",
            Font = new Font("Segoe UI", 28f),
            ForeColor = Pallette.Text,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 20, 0, 0),
        };
        flowPanel.Controls.Add(lblWelcome);

        lblReg = new Label
        {
            Text = "Join  and start your journey",
            Font = new Font("Segoe UI", 16f),
            ForeColor = Pallette.Text,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 0, 0, 20),
            AutoSize = true,
        };
        flowPanel.Controls.Add(lblReg);

        lblName = new Label
        {
            Margin = new Padding(0, 50, 0, 0),

            Text = "Name",
            Font = new Font("Segoe UI", 14f),
            ForeColor = Pallette.Text,

            AutoSize = true,
            Width = WIDTH - SIDEPAD * 2,
        };
        flowPanel.Controls.Add(lblName);

        txtName = new RoundedTextBox
        {
            PlaceholderText = "username",
            Width = 500,
            ForeColor = Pallette.Text,
            PlaceholderColor = Pallette.SecText,
            Padding = new Padding(20, 20, 20, 20),
            Margin = new Padding(0, 0, 0, 20),
        };
        flowPanel.Controls.Add(txtName);

        lblPassword = new Label
        {
            Text = "Password",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Pallette.Text,

            AutoSize = true,
            Width = 500,
        };
        flowPanel.Controls.Add(lblPassword);

        txtPassword = new RoundedTextBox
        {
            IsPassword = true,
            PasswordChar = '•',
            PlaceholderText = "12345678",
            Width = WIDTH - SIDEPAD * 2,
            ForeColor = Pallette.Text,
            Padding = new Padding(20, 20, 20, 20),
            Margin = new Padding(0, 0, 0, 10),
        };
        flowPanel.Controls.Add(txtPassword);

        lblRepPassword = new Label
        {
            Text = "Confirm Password",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Pallette.Text,

            AutoSize = true,
            Width = 500,
        };
        flowPanel.Controls.Add(lblRepPassword);

        txtRepPassword = new RoundedTextBox
        {
            IsPassword = true,
            PasswordChar = '•',
            PlaceholderText = "12345678",
            Width = WIDTH - SIDEPAD * 2,
            ForeColor = Pallette.Text,
            Padding = new Padding(20, 20, 20, 20),
            Margin = new Padding(0, 0, 0, 10),
        };
        flowPanel.Controls.Add(txtRepPassword);

        lblError = new Label
        {
            Text = string.Empty,
            Font = new Font("Segoe UI", 12f),
            ForeColor = Color.Red,
            Margin = new Padding(0, 10, 0, 10),

            BackColor = Color.White,
            AutoSize = true,
            Width = 500,
        };
        lblError.Hide();
        flowPanel.Controls.Add(lblError);

        btnReg = new DropDownRoundedButton
        {
            //Margin = new Padding(20, 13, 0, 0),
            Padding = new Padding(100, 0, 0, 0),

            ForeColor = Color.White,
            NormalBackColor = Pallette.MainAccent,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 0,
            Size = new Size(500, 60),
            Icon = null,
            ButtonText = "Sign In",
            ButtonTextFormat = TextFormatFlags.HorizontalCenter,
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 10, 0, 0),


            HoverBackColor = Color.FromArgb(193, 0, 7),
            PressedBackColor = Color.FromArgb(193, 0, 7)

        };
        flowPanel.Controls.Add(btnReg);
        btnReg.Click += BtnReg_Click;

        var fpToReg = new FlowLayoutPanel
        {
            WrapContents = false,
            AutoSize = true,
            Anchor = AnchorStyles.None,
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = Color.Transparent,
            Padding = new Padding(10),
            Margin = new Padding(0),
            Width = 500,
            Height = 40,
        };
        flowPanel.Controls.Add(fpToReg);

        var lblToReg = new Label
        {
            Size = new Size(165, 40),
            Margin = new Padding(0),
            TextAlign = ContentAlignment.MiddleRight,
            Dock = DockStyle.Fill,
            Text = "Already have an account?",
            Font = new Font("Segoe UI", 10f),
            BackColor = Color.Transparent,
            ForeColor = Pallette.SecText,
        };
        fpToReg.Controls.Add(lblToReg);

        btnToLog = new Button
        {
            Size = new Size(50, 40),

            Padding = new Padding(0),
            Margin = new Padding(0, 1, 0, 0),
            Image = null,

            Text = "Sign in",
            Font = new Font("SegoeUI", 10f),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent,
            ForeColor = Pallette.MainAccent,
            AutoSize = true,

            FlatStyle = FlatStyle.Flat,
            FlatAppearance =
            {
                BorderSize = 0,
                MouseOverBackColor = Color.Transparent,
                MouseDownBackColor = Color.Transparent
            }
        };
        btnToLog.MouseEnter += BtnToLog_MouseEnter;
        btnToLog.MouseLeave += BtnToLog_MouseLeave;
        fpToReg.Controls.Add(btnToLog);

        var rtb = new RoundedTextBox
        {
            PlaceholderText = "email"
        };
        rtb._TextChanged += Rtb_TextChanged;
    }

    private void BtnToLog_MouseEnter(object? sender, EventArgs e)
    {
        Font buf = btnToLog.Font;
        btnToLog.Font = new Font(buf.FontFamily, buf.Size, FontStyle.Underline);
        btnToLog.ForeColor = Pallette.DarkAccent;
    }

    private void BtnToLog_MouseLeave(object? sender, EventArgs e)
    {
        Font buf = btnToLog.Font;
        btnToLog.Font = new Font(buf.FontFamily, buf.Size, FontStyle.Regular);
        btnToLog.ForeColor = Pallette.MainAccent;
    }

    private void Rtb_TextChanged(object? sender, EventArgs e)
    {
    }

    private void BtnReg_Click(object? sender, EventArgs e)
    {
        lblError.Hide();
        switch (
            AuthService.RegUser(txtName.TbText, txtPassword.TbText, txtRepPassword.TbText)
        )
        {
            case RegResult.Success:
                Hide();
                var mainForm = new MainPageForm(1920, 1080);
                mainForm.FormClosed += (_, __) => Close();
                mainForm.Show();

                break;

            case RegResult.PasswordMismatch:
                lblError.Text = "Passwords don't match";
                lblError.Show();
                break;

            case RegResult.InvalidPasswordFormat:
                lblError.Text = "Invalid Password";
                lblError.Show();
                break;

            case RegResult.InvalidNameFormat:
                lblError.Text = "Invalid Name Format";
                lblError.Show();
                break;

            case RegResult.NameTaken:
                lblError.Text = "User with that name already exists";
                lblError.Show();
                break;
        }
    }

    private void BtnToLog_Click(object? sender, EventArgs e)
    {
        Close();
    }


}