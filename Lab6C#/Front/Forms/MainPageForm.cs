public class MainPageForm : Form
{
    private int WIDTH = 1920;
    private int HEIGHT = 1080;

    public MainPageForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(WIDTH, HEIGHT);
        Text = string.Empty;
        ControlBox = false;

        var header = new Header(WIDTH)
        {
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = Color.White,
            Dock = DockStyle.Top,
            Height = 65,
            Padding = new Padding(0)
        };
        Controls.Add(header);
    }
}