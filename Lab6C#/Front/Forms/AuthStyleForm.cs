using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

public class AuthStyleForm : RoundedForm1
{
    private Panel titleBar;
    private Button closeButton;

    private bool _drag;
    private Point _dragStart;

    public AuthStyleForm(int Width, int Height) : base(Width, Height)
    {
        ClientSize = new Size(600, 840);

        MainInitializeComponent();
        titleBar.MouseDown += TitleBar_MouseDown;
        titleBar.MouseMove += TitleBar_MouseMove;
        titleBar.MouseUp += TitleBar_MouseUp;
    }

    private void MainInitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        ControlBox = false;
        Text = string.Empty;
        MaximizeBox = false;
        BackColor = Color.White;

        titleBar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.White,
            Padding = new Padding(0)
        };
        Controls.Add(titleBar);

        var closeButton = new DropDownRoundedButton
        {
            Margin = new Padding(0, 10, 10, 10),
            Padding = new Padding(0, 0, 0, 0),

            ForeColor = Color.Black,
            BackColor = Color.White,
            BorderColor = Color.White,
            BorderRadius = 7,
            BorderSize = 1,
            Size = new Size(30, 30),
            Icon = Image.FromFile("Images/Cross.png"),
            ButtonText = String.Empty,
            Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
            ButtonTextFormat = TextFormatFlags.HorizontalCenter,

            HoverBackColor = Color.FromArgb(254, 242, 242),
            PressedBackColor = Color.FromArgb(254, 242, 242),
            HoverBorderColor = Color.Red,
            PressedBorderColor = Color.Red,
        };

        closeButton.Dock = DockStyle.None;
        closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        closeButton.Location = new Point(titleBar.ClientSize.Width - closeButton.Width - 5, 5);

        titleBar.Controls.Add(closeButton);
        closeButton.Click += (sender, args) => Application.Exit();

        InitializeComponent();
    }


    protected virtual void InitializeComponent()
    {
    }

    private void TitleBar_MouseDown(object? sender, MouseEventArgs e)
    {
        _drag = true;
        _dragStart = e.Location;
    }

    private void TitleBar_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_drag)
            Location = new Point(
                Location.X + e.X - _dragStart.X,
                Location.Y + e.Y - _dragStart.Y);
    }

    private void TitleBar_MouseUp(object? sender, MouseEventArgs e)
    {
        _drag = false;
    }

    private void CloseButton_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    private void AuthStyleForm_Load(object sender, EventArgs e)
    {

    }
}