using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedForm : Form
{
    private int borderRadius = 20;
    private int borderSize = 2;
    private Color borderColor = Color.FromArgb(232, 131, 136);
    private Color shadowColor = Color.FromArgb(60, 0, 0, 0);
    private int shadowOffset = 5;

    public RoundedForm()
    {
        SetStyle(ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);
        UpdateStyles();

        FormBorderStyle = FormBorderStyle.None;
        Padding = new Padding(borderSize);
        BackColor = Color.White;
        ClientSize = new Size(600, 840);
    }

    public int BorderRadius { get => borderRadius; set { borderRadius = value; InvalidateRegion(); } }
    public int BorderSize { get => borderSize; set { borderSize = value; InvalidateRegion(); } }
    public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }
    public Color ShadowColor { get => shadowColor; set { shadowColor = value; Invalidate(); } }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var rectForm = ClientRectangle;
        var rectShadow = new Rectangle(rectForm.X + shadowOffset, rectForm.Y + shadowOffset,
                                       rectForm.Width - shadowOffset, rectForm.Height - shadowOffset);

        using (var path = GetRoundedPath(rectForm, borderRadius))
        using (var shadowPath = GetRoundedPath(rectShadow, borderRadius))
        using (var shadowBrush = new SolidBrush(shadowColor))
        using (var bodyBrush = new SolidBrush(this.BackColor))
        using (var borderPen = new Pen(borderColor, borderSize))
        {
            g.FillPath(shadowBrush, shadowPath);

            g.FillPath(bodyBrush, path);

            if (borderSize > 0)
                g.DrawPath(borderPen, path);
        }
    }
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        InvalidateRegion();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        InvalidateRegion();
    }

    private void InvalidateRegion()
    {
        try
        {
            var r = ClientRectangle;
            if (r.Width > 0 && r.Height > 0)
            {
                this.Region?.Dispose();
                this.Region = new Region(GetRoundedPath(r, borderRadius));
            }
        }
        catch { }
        Invalidate();
    }

    private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        int diameter = Math.Max(0, radius * 2);

        int w = rect.Width;
        int h = rect.Height;
        if (diameter > w) diameter = w;
        if (diameter > h) diameter = h;

        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}
