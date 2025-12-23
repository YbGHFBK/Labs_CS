using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class IconRoundedPanelWithShadow : Control
{
    // Настройки панели
    public int BorderRadius { get; set; } = 20;
    public Color BackgroundColor { get; set; } = Color.White;
    public Image Icon { get; set; }
    public Size IconSize { get; set; } = new Size(32, 32);

    // Настройки тени
    public bool ShowShadow { get; set; } = true;
    public Color ShadowColor { get; set; } = Pallette.SecAccent;//Color.Black;
    public int ShadowOpacity { get; set; } = 70; // От 0 до 255
    public int ShadowSize { get; set; } = 8;

    public IconRoundedPanelWithShadow()
    {
        this.DoubleBuffered = true;
        this.Size = new Size(150, 100);
        this.BackColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Прямоугольник самой панели (с учетом отступа под тень)
        Rectangle rect = new Rectangle(
            ShadowSize,
            ShadowSize,
            Width - (ShadowSize * 2) - 1,
            Height - (ShadowSize * 2) - 1
        );

        if (ShowShadow)
        {
            DrawShadow(g, rect);
        }

        // Отрисовка основной панели
        using (GraphicsPath path = GetRoundedPath(rect, BorderRadius))
        {
            using (SolidBrush brush = new SolidBrush(BackgroundColor))
            {
                g.FillPath(brush, path);
            }

            if (Icon != null)
            {
                int x = rect.X + (rect.Width - IconSize.Width) / 2;
                int y = rect.Y + (rect.Height - IconSize.Height) / 2;
                g.DrawImage(Icon, new Rectangle(new Point(x, y), IconSize));
            }
        }
    }

    private void DrawShadow(Graphics g, Rectangle rect)
    {
        // Рисуем несколько слоев для эффекта размытия (Blur)
        for (int i = 1; i <= ShadowSize; i++)
        {
            // Чем дальше слой, тем он прозрачнее
            int alpha = ShadowOpacity / ShadowSize;
            using (GraphicsPath shadowPath = GetRoundedPath(rect, BorderRadius))
            {
                // Смещаем и немного расширяем каждый слой тени
                using (Pen shadowPen = new Pen(Color.FromArgb(alpha, ShadowColor), i))
                {
                    shadowPen.Alignment = PenAlignment.Outset;
                    g.DrawPath(shadowPen, shadowPath);
                }
            }
        }
    }

    private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        float curveSize = radius * 2F;
        if (curveSize <= 0) curveSize = 1; // Защита от нулевого радиуса

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        this.Invalidate();
    }
}