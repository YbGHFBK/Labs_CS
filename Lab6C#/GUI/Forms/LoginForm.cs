public class LoginForm : AuthStyleForm
{
    private FlowLayoutPanel flowPanel;

    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblPassword;
    private TextBox txtPassword;

    private Button btnLogin;
    private Button btnToReg;
    private Label lblError;

    public LoginForm() : base() { }

    protected override void InitializeComponent()
    {
        flowPanel = new FlowLayoutPanel()
        {
            Dock = DockStyle.Fill,
            Anchor = AnchorStyles.None,
            FlowDirection = FlowDirection.TopDown,
            AutoSize = true,
            BackColor = Color.Transparent,
            Padding = new Padding(100, 40, 150, 0)
        };
        Controls.Add(flowPanel);

        lblEmail = new Label
        {
            Text = "Войдите, используя имя аккаунта",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Color.FromArgb(213, 220, 230),

            AutoSize = true,
            Location = new Point(20, 70),
            Width = 400,
        };
        flowPanel.Controls.Add(lblEmail);

        txtEmail = new TextBox
        {
            Font = new Font("Segoe UI", 12f),
            BorderStyle = BorderStyle.None,

            BackColor = Color.FromArgb(44, 56, 74),
            ForeColor = Color.White,

            Location = new Point(25, 100),
            Width = 400,
            Height = 40
        };
        flowPanel.Controls.Add(txtEmail);

        lblPassword = new Label
        {
            Text = "Пароль",
            Font = new Font("Segoe UI", 12f),
            ForeColor = Color.FromArgb(213, 220, 230),

            AutoSize = true,
            Location = new Point(20, 130),
            Width = 400,
        };
        flowPanel.Controls.Add(lblPassword);

        txtPassword = new TextBox
        {
            Font = new Font("Segoe UI", 12f),
            BorderStyle = BorderStyle.None,

            BackColor = Color.FromArgb(44, 56, 74),
            ForeColor = Color.White,

            Location = new Point(25, 160),
            Width = 400,
            Height = 40
        };
        flowPanel.Controls.Add(txtPassword);

        lblError = new Label
        {
            Text = string.Empty,
            Font = new Font("Segoe UI", 11f),
            ForeColor = Color.Red,

            BackColor = Color.FromArgb(27, 35, 46),
            AutoSize = true,
            Location = new Point(20, 190),
            Width = 400,
        };
        lblError.Hide();
        flowPanel.Controls.Add(lblError);

        btnLogin = new Button
        {
            Text = "Войти",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Green,
            ForeColor = Color.White,

            Size = new Size(120, 40),
            Margin = new Padding(120, 10, 120, 0),

            FlatStyle = FlatStyle.Flat,
            FlatAppearance =
            {
                BorderSize = 0,
                MouseOverBackColor = Color.DarkGreen,
                MouseDownBackColor = Color.DarkGreen
            }
        };
        flowPanel.Controls.Add(btnLogin);

        btnToReg = new Button
        {
            Text = "Нет аккаунта? Перейти к регистрации",
            Font = new Font("SegoeUI", 9f, FontStyle.Underline),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent,
            ForeColor = Color.FromArgb(213, 220, 230),
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
}