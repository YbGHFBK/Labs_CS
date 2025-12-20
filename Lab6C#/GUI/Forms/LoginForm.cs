public class LoginForm : AuthStyleForm
{
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
        lblEmail = new Label
        {
            Text = "Войдите, используя имя аккаунта",
            Font = new Font("Segoe UI", 11f),
            ForeColor = Color.FromArgb(213, 220, 230),

            AutoSize = true,
            Location = new Point(20, 70)
        };
        Controls.Add(lblEmail);

        txtEmail = new TextBox
        {
            Font = new Font("Segoe UI", 11f),
            BorderStyle = BorderStyle.None,

            BackColor = Color.FromArgb(44, 56, 74),

            Location = new Point(20, 100),
            Width = 270
        };
        Controls.Add(txtEmail);
    }
}