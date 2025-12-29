using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class AutoSizedMenuPanel : UserControl
{
    private int borderRadius = 15;
    private int borderSize = 2;
    private Color borderColor = Color.FromArgb(250, 204, 206);
    private Color itemHover = Color.FromArgb(245, 245, 245);
    private Color itemPressed = Color.FromArgb(230, 230, 230);
    private Color itemFore = Color.Black;
    private Color backColorNormal = Color.White;
    private int itemHeight = 32;

    private MenuItemData selectedItem = null!;

    private List<MenuItemData> items = new();

    public class MenuItemData
    {
        public string Text { get; set; }
        public Image Icon { get; set; }
        public Action OnClick { get; set; }

        public MenuItemData(string text, Action onClick = null, Image icon = null)
        {
            Text = text;
            Icon = icon;
            OnClick = onClick;
        }
    }

    [Browsable(true), Category("Appearance")]
    public int BorderRadius { get => borderRadius; set { borderRadius = Math.Max(0, value); Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public int BorderSize { get => borderSize; set { borderSize = Math.Max(0, value); Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Color ItemHoverColor { get => itemHover; set { itemHover = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Color ItemPressedColor { get => itemPressed; set { itemPressed = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public Color ItemForeColor { get => itemFore; set { itemFore = value; Invalidate(); } }

    [Browsable(true), Category("Appearance")]
    public int ItemHeight { get => itemHeight; set { itemHeight = Math.Max(20, value); RecalculateSize(); Invalidate(); } }

    public MenuItemData SelectedItem { get => selectedItem; set { selectedItem = value; Invalidate(); } }

    public AutoSizedMenuPanel()
    {
        DoubleBuffered = true;
        AutoSize = false;
        BackColor = backColorNormal;
        Padding = new Padding(1);
    }

    public void AddItem(string text, Action onClick, Image icon = null)
    {
        items.Add(new MenuItemData(text, onClick, icon));
        RecalculateSize();
        Invalidate();
    }

    public void Clear()
    {
        items.Clear();
        RecalculateSize();
        Invalidate();
    }

    private void RecalculateSize()
    {
        Height = items.Count * itemHeight + Padding.Vertical;
    }

    private GraphicsPath GetFigurePath(RectangleF rect, float radius)
    {
        GraphicsPath path = new GraphicsPath();

        float d = radius * 2;

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Width - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Width - d, rect.Height - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Height - d, d, d, 90, 90);
        path.CloseFigure();

        return path;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var rect = ClientRectangle;
        RectangleF rectSurface = new RectangleF(0, 0, this.Width, items.Count * itemHeight);
        RectangleF rectBorder = new RectangleF(1, 1, this.Width - 0.8F, items.Count * itemHeight - 1);

        if(borderSize > 0)
        {
            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - 1F))
            using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                penBorder.Alignment = PenAlignment.Inset;
                this.Region = new Region(pathSurface);

                pevent.Graphics.DrawPath(penSurface, pathSurface);

                if (borderSize >= 1)
                    pevent.Graphics.DrawPath(penBorder, pathBorder);


            }
        }
        else
        {
            this.Region = new Region(rectSurface);
            if (borderSize >= 1)
            {
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }
        for (int i = 0; i < items.Count; i++)
        {
            DrawItem(pevent.Graphics, i, rect);
        }
    }

    private int hoveredIndex = -1;
    private int pressedIndex = -1;

    private void DrawItem(Graphics g, int index, Rectangle bounds)
    {
        Rectangle itemRect = new Rectangle(bounds.Left + Padding.Left, bounds.Top + Padding.Top + index * itemHeight, bounds.Width - Padding.Horizontal, itemHeight);
        var data = items[index];

        // фон по состоянию
        Color fill = hoveredIndex == index ? (pressedIndex == index ? itemPressed : itemHover) : BackColor;
        using (var b = new SolidBrush(fill))
        {
            g.FillRectangle(b, itemRect);
        }

        int x = itemRect.Left + 8;
        if (data.Icon != null)
        {
            Rectangle iconRect = new Rectangle(x, itemRect.Top + (itemRect.Height - 20) / 2, 20, 20);
            g.DrawImage(data.Icon, iconRect);
            x += 26;
        }

        Rectangle textRect = new Rectangle(x, itemRect.Top, itemRect.Width - x - 8, itemRect.Height);
        TextRenderer.DrawText(g, data.Text, Font, textRect, itemFore, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
    }

    private GraphicsPath GetRoundPath(Rectangle r, int radius)
    {
        var path = new GraphicsPath();
        if (radius <= 0)
        {
            path.AddRectangle(r);
            return path;
        }

        int d = radius * 2;
        path.StartFigure();
        path.AddArc(r.Left, r.Top, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int newIndex = e.Y / itemHeight;
        if (newIndex != hoveredIndex && newIndex >= 0 && newIndex < items.Count)
        {
            hoveredIndex = newIndex;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        hoveredIndex = -1;
        pressedIndex = -1;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        pressedIndex = e.Y / itemHeight;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        int index = e.Y / itemHeight;
        if (index >= 0 && index < items.Count)
        {
            items[index].OnClick?.Invoke();
        }
        pressedIndex = -1;
        Invalidate();
    }
}
