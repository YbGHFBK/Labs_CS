using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    public class RoundedFlowLayoutPanel : FlowLayoutPanel
    {
        // Поля для хранения значений свойств
        private int borderRadius = 20;
        private int borderSize = 2;
        private Color borderColor = Color.RoyalBlue;

        // Свойства, которые будут видны в дизайнере
        [Category("Appearance")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate(); // Перерисовать при изменении
            }
        }

        [Category("Appearance")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Padding = new Padding(borderSize); // Отступ, чтобы контент не наезжал на рамку
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        // Конструктор
        public RoundedFlowLayoutPanel()
        {
            // Включаем двойную буферизацию для предотвращения мерцания
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
        }

        // Основной метод отрисовки
        protected override void OnPaint(PaintEventArgs e)
        {
            // Полностью игнорируем base.OnPaint(e)
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = this.ClientRectangle;

            // Вместо Region используем просто заливку, если границы окна не критичны
            // Если Region обязателен, убедитесь, что он создается по полному размеру
            using (GraphicsPath path = GetFigurePath(new RectangleF(0, 0, rect.Width, rect.Height), borderRadius))
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                // Сначала закрашиваем всё белым (или цветом фона)
                e.Graphics.Clear(this.Parent?.BackColor ?? Color.White);
                e.Graphics.FillPath(brush, path);

                // Рисуем рамку
                if (borderSize > 0)
                {
                    using (Pen pen = new Pen(borderColor, borderSize))
                    {
                        pen.Alignment = PenAlignment.Inset; // Рамка внутрь, чтобы не резалась краем
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }
        }

        // Метод для создания пути (фигуры) с закругленными углами
        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();

            // Если радиус слишком большой, ограничиваем его
            float d = radius * 2;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;

            // Рисуем 4 дуги и соединяющие линии
            path.AddArc(rect.X, rect.Y, d, d, 180, 90); // Верхний левый
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90); // Верхний правый
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90); // Нижний правый
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90); // Нижний левый

            path.CloseFigure();
            return path;
        }

        // Обработка изменения размера (чтобы регион пересчитывался)
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            this.Invalidate();
        }
    }
}