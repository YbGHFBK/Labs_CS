public class RegisterForm : AuthStyleForm
{
    private FlowLayoutPanel flowPanel;

    private Label lblReg;
    private Label lblWelcome;

    private Label lblName;
    private HintTextBox txtName;
    private Label lblPassword;
    private HintTextBox txtPassword;
    private Label lblRepeatPassword;
    private HintTextBox txtRepeatPassword;

    private DropDownRoundedButton btnReg;
    private Button btnToLogin;
    private Label lblError;

    public RegisterForm(int Width, int Height) : base(Width, Height)
    {
        btnToLogin.Click += BtnToLogin_Click;
    }

    protected override void InitializeComponent()
    {
        flowPanel = new FlowLayoutPanel()
        {
            Dock = DockStyle.Fill,
            Anchor = AnchorStyles.None,
            FlowDirection = FlowDirection.TopDown,
            AutoSize = true,
            BackColor = Color.Transparent,
            Padding = new Padding(20, 40, 40, 0)
        };
        Controls.Add(flowPanel);

        lblReg = new Label
        {
            Text = "Create Account",
            Font = new Font("Segoe UI", 15f),
            ForeColor = Color.Black,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 20, 0, 20),
            AutoSize = true,
        };
        flowPanel.Controls.Add(lblReg);

        lblWelcome = new Label
        {
            AutoSize = true,
            Text = "Join MoveCore and start your journey",
            Font = new Font("Segoe UI", 11f),
            ForeColor = Color.Black,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 0, 0, 0),
        };
        flowPanel.Controls.Add(lblWelcome);

        lblName = new Label
        {
            Text = "Name",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Color.Black,

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

            BackColor = Color.FromArgb(230, 230, 230),
            ForeColor = Color.Black,

            Width = 340,
            Height = 40,
        };
        flowPanel.Controls.Add(txtPassword);

        lblRepeatPassword = new Label
        {
            Text = "Confirm Password",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Color.Black,

            AutoSize = true,
            Width = 400,
        };
        flowPanel.Controls.Add(lblRepeatPassword);

        txtRepeatPassword = new HintTextBox
        {
            Margin = new Padding(0, 0, 20, 0),
            UseSystemPasswordChar = true,
            Font = new Font("Segoe UI", 12f),
            BorderStyle = BorderStyle.None,
            CueText = "********",

            BackColor = Color.FromArgb(230, 230, 230),
            ForeColor = Color.Black,

            Width = 340,
            Height = 40,
        };
        flowPanel.Controls.Add(txtRepeatPassword);

        lblError = new Label
        {
            Text = string.Empty,
            Font = new Font("Segoe UI", 11f),
            ForeColor = Color.Red,

            BackColor = Color.White,
            AutoSize = true,
            Width = 360,
        };
        lblError.Hide();
        flowPanel.Controls.Add(lblError);

        btnReg = new DropDownRoundedButton
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
            ButtonText = "Create Account",
            ButtonTextFormat = TextFormatFlags.HorizontalCenter,
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),


            HoverBackColor = Color.FromArgb(193, 0, 7),
            PressedBackColor = Color.FromArgb(193, 0, 7)

        };
        flowPanel.Controls.Add(btnReg);
        btnReg.Click += BtnReg_Click;

        btnToLogin = new Button
        {
            Margin = new Padding(0, 20, 0, 0),

            Text = "Already have an account?Sign in",
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
        flowPanel.Controls.Add(btnToLogin);
    }

    private void BtnReg_Click(object? sender, EventArgs e)
    {
        lblError.Hide();
        switch (
            AuthService.RegUser(txtName.Text, txtPassword.Text, txtRepeatPassword.Text)
        )
        {
            case RegResult.Success:
                Hide();
                var mainForm = new MainPageForm();
                mainForm.FormClosed += (_, __) => Close();
                mainForm.Show();

                break;

            case RegResult.NameTaken:
                lblError.Text = "User with that name already exists";
                lblError.Show();
                break;

            case RegResult.PasswordMismatch:
                lblError.Text = "Passwords don't match";
                lblError.Show();
                break;

            case RegResult.InvalidNameFormat:
                lblError.Text = "Invalid Name Format";
                lblError.Show();
                break;

            case RegResult.InvalidPasswordFormat:
                lblError.Text = "Invalid Password Format";
                lblError.Show();
                break;
        }
    }

    private void BtnToLogin_Click(object? sender, EventArgs e)
    {
        Close();
    }
}