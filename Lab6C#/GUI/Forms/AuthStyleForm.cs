using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

public class AuthStyleForm : Form
{
    private Panel titleBar;
    private Button closeButton;

    private bool _drag;
    private Point _dragStart;

    public AuthStyleForm()
    {
        MainInitializeComponent();
        titleBar.MouseDown += TitleBar_MouseDown;
        titleBar.MouseMove += TitleBar_MouseMove;
        titleBar.MouseUp += TitleBar_MouseUp;
        closeButton.Click += CloseButton_Click;
    }

    private void MainInitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        ControlBox = false;
        Text = string.Empty;
        MaximizeBox = false;
        BackColor = Color.FromArgb(29, 30, 34);
        ClientSize = new Size(600, 350);

        titleBar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 35,
            BackColor = Color.FromArgb(29, 30, 34)
        };
        Controls.Add(titleBar);

        closeButton = new Button
        {
            Text = "×",
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent,
            ForeColor = Color.FromArgb(213, 220, 230),
            Size = new Size(35, 35),
            Dock = DockStyle.Right,

            FlatStyle = FlatStyle.Flat,
            FlatAppearance =
            {
                BorderSize = 0,
                MouseOverBackColor = Color.DarkRed,
                MouseDownBackColor = Color.DarkRed
            }
        };
        titleBar.Controls.Add(closeButton);

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
}