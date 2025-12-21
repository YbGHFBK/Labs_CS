public class RegisterForm : AuthStyleForm
{
    private FlowLayoutPanel flowPanel;

    private Label lblReg;

    private Label lblName;
    private HintTextBox txtName;
    private Label lblPassword;
    private HintTextBox txtPassword;
    private Label lblRepeatPass;
    private HintTextBox txtRepeatPass;

    private Button btnReg;
    private Label lblError;
    private Button btnToLogIn;

    public RegisterForm() : base()
    {
        btnToLogIn.Click += BtnToLogIn_Click;
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
            Padding = new Padding(80, 40, 80, 0)
        };
        Controls.Add(flowPanel);

        lblReg = new Label
        {
            Text = "Создать аккаунт",
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            ForeColor = Color.Transparent,
            Anchor = AnchorStyles.None,
            AutoSize = true,
            Margin = new Padding(20),
        };
        flowPanel.Controls.Add(lblReg);

        txtName = new HintTextBox
        {
            Font = new Font("Segoe UI", 12f),
            BorderStyle = BorderStyle.None,
            CueText = "Логин",

            BackColor = Color.FromArgb(37, 38, 42),
            ForeColor = Color.White,

            Location = new Point(25, 100),
            Width = 400,
            Height = 40
        };
        flowPanel.Controls.Add(txtName);

        txtPassword = new HintTextBox
        {
            Font = new Font("Segoe UI", 12f),
            BorderStyle = BorderStyle.None,
            CueText = "Введите пароль",
            UseSystemPasswordChar = true,

            BackColor = Color.FromArgb(37, 38, 42),
            ForeColor = Color.White,

            Width = 400,
            Height = 40
        };
        flowPanel.Controls.Add(txtPassword);

        txtRepeatPass = new HintTextBox
        {
            Font = new Font("Segoe UI", 12f),
            BorderStyle = BorderStyle.None,
            CueText = "Повторите пароль",
            UseSystemPasswordChar = true,

            BackColor = Color.FromArgb(37, 38, 42),
            ForeColor = Color.White,

            Width = 400,
            Height = 40
        };
        flowPanel.Controls.Add(txtRepeatPass);

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

        btnReg = new Button
        {
            Text = "Создать аккаунт",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(133, 77, 216),
            ForeColor = Color.White,

            Size = new Size(200, 30),
            Margin = new Padding(120, 10, 120, 0),

            FlatStyle = FlatStyle.Flat,
            FlatAppearance =
            {
                BorderSize = 0,
                MouseOverBackColor = Color.FromArgb(117,60,175),
                MouseDownBackColor = Color.FromArgb(117,60,175)
            }
        };
        flowPanel.Controls.Add(btnReg);

        btnToLogIn = new Button
        {
            Text = "Уже есть аккаунт? Войти",
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
        flowPanel.Controls.Add(btnToLogIn);
    }

    private void BtnToLogIn_Click(object? sender, EventArgs e)
    {
        this.Close();
    }
}