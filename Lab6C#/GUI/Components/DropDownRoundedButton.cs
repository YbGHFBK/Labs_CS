using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class DropDownRoundedButton : UserControl
{
    // визуальные свойства
    private int borderRadius = 12;
    private int borderSize = 1;
    private Color borderColor = Color.LightGray;
    private Color normalBack = Color.White;
    private Color hoverBack = Color.FromArgb(245, 245, 245);
    private Color pressedBack = Color.FromArgb(230, 230, 230);
    private Color disabledBack = Color.FromArgb(220, 220, 220);
    private Image icon;
    private string label = "Button";
    private Padding contentPadding = new Padding(8);

    private bool isHovered = false;
    private bool isPressed = false;

    [Browsable(true), Category("Appearance")]
    public int BorderRadius { get => borderRadius; set { borderRadius = Math.Max(0, value); Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public int BorderSize { get => borderSize; set { borderSize = Math.Max(0, value); Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Color NormalBackColor { get => normalBack; set { normalBack = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Color HoverBackColor { get => hoverBack; set { hoverBack = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Color PressedBackColor { get => pressedBack; set { pressedBack = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Image Icon { get => icon; set { icon = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public string ButtonText { get => label; set { label = value ?? ""; Invalidate(); } }

    [Browsable(true), Category("Behavior")]
    public ContextMenuStrip DropDownMenu { get; set; } = null;

    public DropDownRoundedButton()
    {
        DoubleBuffered = true;
        Size = new Size(140, 34);
        Cursor = Cursors.Hand;
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        // фон (на случай прозрачности родителя)
        using (var backBrush = new SolidBrush(Parent?.BackColor ?? SystemColors.Control))
        {
            e.Graphics.FillRectangle(backBrush, ClientRectangle);
        }

        Rectangle rect = ClientRectangle;
        rect.Inflate(-1, -1);

        // выбирать цвет по состоянию
        Color fill = Enabled ? (isPressed ? pressedBack : (isHovered ? hoverBack : normalBack)) : disabledBack;

        int radius = Math.Min(BorderRadius, Height / 2);
        using (var path = GetRoundPath(rect, radius))
        using (var brush = new SolidBrush(fill))
        using (var pen = new Pen(borderColor, BorderSize))
        {
            e.Graphics.FillPath(brush, path);
            if (BorderSize > 0)
            {
                pen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(pen, path);
            }

            // рисуем иконку и текст вручную, чтобы ничего не смещалось
            DrawContent(e.Graphics, rect);
        }
    }

    private void DrawContent(Graphics g, Rectangle rect)
    {
        int x = rect.Left + contentPadding.Left;
        int y = rect.Top + contentPadding.Top;
        int h = rect.Height - contentPadding.Vertical;

        // если есть иконка — размещаем слева
        int iconSize = Math.Min(24, h);
        if (icon != null)
        {
            var iconRect = new Rectangle(x, rect.Top + (rect.Height - iconSize) / 2, iconSize, iconSize);
            g.DrawImage(icon, iconRect);
            x += iconSize + 6; // отступ после иконки
        }

        // текст (оставляем правый отступ для стрелочки выпадающего меню)
        int rightReserve = DropDownMenu != null ? 16 : 0;
        Rectangle textRect = new Rectangle(x, rect.Top, rect.Width - (x - rect.Left) - contentPadding.Right - rightReserve, rect.Height);

        TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;
        Color textColor = Enabled ? ForeColor : Color.Gray;
        TextRenderer.DrawText(g, label, Font, textRect, textColor, flags);

        // стрелочка для выпадающего меню
        if (DropDownMenu != null)
        {
            Point p = new Point(rect.Right - contentPadding.Right - 10, rect.Top + rect.Height / 2 - 1);
            DrawDownArrow(g, p);
        }
    }

    private void DrawDownArrow(Graphics g, Point p)
    {
        // простой треугольник "▾"
        var pts = new Point[]
        {
            new Point(p.X - 5, p.Y - 2),
            new Point(p.X + 5, p.Y - 2),
            new Point(p.X, p.Y + 4)
        };
        using (var br = new SolidBrush(Enabled ? ForeColor : Color.Gray))
            g.FillPolygon(br, pts);
    }

    private GraphicsPath GetRoundPath(Rectangle r, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;
        if (radius <= 1)
        {
            path.AddRectangle(r);
            return path;
        }

        path.StartFigure();
        path.AddArc(r.Left, r.Top, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    // Состояния мыши
    protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); isHovered = true; Invalidate(); }
    protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); isHovered = false; isPressed = false; Invalidate(); }
    protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); if (e.Button == MouseButtons.Left) { isPressed = true; Invalidate(); } }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (isPressed && e.Button == MouseButtons.Left)
        {
            // клик — показываем меню (если есть) или вызываем обычный Click
            if (DropDownMenu != null && DropDownMenu.Items.Count > 0)
            {
                // позиция чуть ниже кнопки
                DropDownMenu.Show(this, new Point(0, Height));
            }
            else
            {
                OnClick(EventArgs.Empty);
            }
        }
        isPressed = false;
        Invalidate();
    }

    // чтобы при нажатии клавишой Enter/Space работало
    protected override bool IsInputKey(Keys keyData) => keyData == Keys.Space || keyData == Keys.Enter ? true : base.IsInputKey(keyData);
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) { isPressed = true; Invalidate(); }
        base.OnKeyDown(e);
    }
    protected override void OnKeyUp(KeyEventArgs e)
    {
        if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) && isPressed)
        {
            if (DropDownMenu != null && DropDownMenu.Items.Count > 0) DropDownMenu.Show(this, new Point(0, Height));
            else OnClick(EventArgs.Empty);
        }
        isPressed = false; Invalidate(); base.OnKeyUp(e);
    }
}
