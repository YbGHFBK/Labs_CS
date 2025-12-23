public class LoginForm : AuthStyleForm
{
    private FlowLayoutPanel flowPanel;

    private Label lblWelcome;
    private Label lblLogin;

    private Label lblName;
    private HintTextBox txtName;
    private Label lblPassword;
    private HintTextBox txtPassword;

    private DropDownRoundedButton btnLogin;
    private Button btnToReg;
    private Label lblError;

    public LoginForm(int Width, int Height) : base(Width, Height)
    {
        btnToReg.Click += BtnToReg_Click;
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
            Padding = new Padding(20, 40, 20, 0),
            Margin = new Padding(0),
        };
        table.Controls.Add(flowPanel, 0, 0);
        this.Controls.Add(table);

        lblWelcome = new Label
        {
            AutoSize = true,
            Text = "Welcome Back",
            Font = new Font("Segoe UI", 15f),
            ForeColor = Color.Black,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 20, 0, 0),
        };
        flowPanel.Controls.Add(lblWelcome);

        lblLogin = new Label
        {
            Text = "Sign in to access your bookings and preferences",
            Font = new Font("Segoe UI", 10f),
            ForeColor = Color.Black,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 20, 0, 20),
            AutoSize = true,
        };
        flowPanel.Controls.Add(lblLogin);

        lblName = new Label
        {
            Text = "Name",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Color.Black,//FromArgb(213, 220, 230),

            AutoSize = true,
            Width = 400,
        };
        flowPanel.Controls.Add(lblName);

        txtName = new HintTextBox
        {
            Margin = new Padding(0, 0, 20, 0),
            Font = new Font("Segoe UI", 12f),
            BorderStyle = BorderStyle.None,
            CueText = "account name",

            BackColor = Color.FromArgb(230, 230, 230),
            ForeColor = Color.Black,

            Width = 340,
            Height = 40
        };
        flowPanel.Controls.Add(txtName);

        lblPassword = new Label
        {
            Text = "Password",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Color.Black,

            AutoSize = true,
            Width = 400,
        };
        flowPanel.Controls.Add(lblPassword);

        txtPassword = new HintTextBox
        {
            Margin = new Padding(0, 0, 20, 0),
            UseSystemPasswordChar = true,
            Font = new Font("Segoe UI", 12f),
            BorderStyle = BorderStyle.None,
            CueText = "********",

            BackColor = Color.FromArgb(230, 230, 230),//(37, 38, 42),
            ForeColor = Color.Black,

            Width = 340,
            Height = 40,
        };
        flowPanel.Controls.Add(txtPassword);

        lblError = new Label
        {
            Text = string.Empty,
            Font = new Font("Segoe UI", 11f),
            ForeColor = Color.Red,

            BackColor = Color.White,
            AutoSize = true,
            Width = 340,
        };
        lblError.Hide();
        flowPanel.Controls.Add(lblError);

        btnLogin = new DropDownRoundedButton
        {
            //Margin = new Padding(20, 13, 0, 0),
            Padding = new Padding(100, 0, 0, 0),

            ForeColor = Color.White,
            NormalBackColor = Color.Red,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 0,
            Size = new Size(340, 40),
            Icon = null,
            ButtonText = "Sign In",
            ButtonTextFormat = TextFormatFlags.HorizontalCenter,
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),


            HoverBackColor = Color.FromArgb(193, 0, 7),
            PressedBackColor = Color.FromArgb(193, 0, 7)

        };
        flowPanel.Controls.Add(btnLogin);
        btnLogin.Click += BtnLogin_Click;

        btnToReg = new Button
        {
            Margin = new Padding(0, 20, 0, 0),

            Text = "Don't have an account? Sign up",
            Font = new Font("SegoeUI", 9f, FontStyle.Underline),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent,
            ForeColor = Color.FromArgb(115, 115, 115),
            AutoSize = true,

            FlatStyle = FlatStyle.Flat,
            FlatAppearance =
            {
                BorderSize = 0,
                MouseOverBackColor = Color.Transparent,
                MouseDownBackColor = Color.Transparent
            }
        };
        flowPanel.Controls.Add(btnToReg);
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        lblError.Hide();
        switch (
            AuthService.LoginUser(txtName.Text, txtPassword.Text)
        )
        {
            case LoginResult.Success:
                Hide();
                var mainForm = new MainPageForm();
                mainForm.FormClosed += (_, __) => Close();
                mainForm.Show();

                break;

            case LoginResult.UserNotFound:
                lblError.Text = "User with that name does not exist";
                lblError.Show();
                break;

            case LoginResult.InvalidPassword:
                lblError.Text = "Invalid Password";
                lblError.Show();
                break;

            case LoginResult.InvalidNameFormat:
                lblError.Text = "Invalid Name Format";
                lblError.Show();
                break;

            case LoginResult.InvalidPasswordFormat:
                lblError.Text = "Invalid Password Format";
                lblError.Show();
                break;
        }
    }

    private void BtnToReg_Click(object? sender, EventArgs e)
    {
        Hide();

        var reg = new RegisterForm(600, 840);
        reg.FormClosed += (s, args) =>
        {
            Show();
        };
        reg.Show();
    }


}