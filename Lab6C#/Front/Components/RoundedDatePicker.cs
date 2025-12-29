using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedDatePicker : DateTimePicker
{
    // --- Поля ---
    private Color skinColor = Color.White;
    private Color textColor = Color.Black;
    private Color borderColor = Pallette.LightAccent;
    private int borderSize = 1;
    private int borderRadius = 10; // Радиус скругления

    private bool droppedDown = false;

    // Иконки (лучше загружать их безопаснее, но оставляю логику как была)
    private Image calendarIcon = Image.FromFile("Images/CalendarDark.png"); // Убедитесь, что пути верны
    private RectangleF iconButtonArea;
    private const int calendarIconWidth = 34;

    // --- Свойства ---
    [Category("Appearance")]
    public int BorderRadius
    {
        get { return borderRadius; }
        set
        {
            borderRadius = Math.Max(0, value);
            this.Invalidate();
        }
    }

    [Category("Appearance")]
    public Color SkinColor
    {
        get { return skinColor; }
        set
        {
            skinColor = value;
            // Простая логика смены иконки в зависимости от фона (как в оригинале)
            if (skinColor.GetBrightness() >= 0.6F)
                calendarIcon = Image.FromFile("Images/CalendarDark.png"); // Темная иконка для светлого фона
            else
                calendarIcon = Image.FromFile("Images/CalendarWhite.png"); // Светлая иконка для темного фона
            this.Invalidate();
        }
    }

    [Category("Appearance")]
    public Color TextColor
    {
        get { return textColor; }
        set
        {
            textColor = value;
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

    [Category("Appearance")]
    public int BorderSize
    {
        get { return borderSize; }
        set
        {
            borderSize = value;
            this.Invalidate();
        }
    }

    // --- Конструктор ---
    public RoundedDatePicker()
    {
        this.SetStyle(ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.AllPaintingInWmPaint, true);
        this.MinimumSize = new Size(0, 40);
        this.Font = new Font(this.Font.Name, 9.5F);
        this.Cursor = Cursors.Hand;
    }

    // --- Переопределенные методы ---

    protected override void OnDropDown(EventArgs eventargs)
    {
        base.OnDropDown(eventargs);
        droppedDown = true;
        this.Invalidate(); // Перерисовать, чтобы подсветить иконку
    }

    protected override void OnCloseUp(EventArgs eventargs)
    {
        base.OnCloseUp(eventargs);
        droppedDown = false;
        this.Invalidate();
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);
        e.Handled = true; // Запрещаем ручной ввод текста, только выбор
    }

    // Главный метод отрисовки
    // Главный метод отрисовки
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        // 1. Заливаем углы цветом родителя (эффект прозрачности)
        using (var backBrush = new SolidBrush(Parent?.BackColor ?? SystemColors.Control))
        {
            e.Graphics.FillRectangle(backBrush, ClientRectangle);
        }

        // --- ИСПРАВЛЕНИЕ ЗДЕСЬ ---
        // Вместо rect.Inflate(-1, -1), используем более точный расчет.
        // Мы берем прямоугольник от (0,0) с шириной и высотой, уменьшенными на 1.
        // Это стандартный способ рисования границ в WinForms, чтобы они не обрезались.
        Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        // -------------------------

        // 2. Создаем путь скругления
        using (var path = GetRoundPath(rect, borderRadius))
        using (var skinBrush = new SolidBrush(skinColor))
        using (var textBrush = new SolidBrush(textColor))
        using (var borderPen = new Pen(borderColor, borderSize))
        {
            // 3. Рисуем фон
            e.Graphics.FillPath(skinBrush, path);

            // 4. Рисуем текст (дату)
            // Форматируем текст по вертикали по центру, отступ слева
            Rectangle textRect = new Rectangle(rect.X + 10, rect.Y, rect.Width - calendarIconWidth - 10, rect.Height);
            StringFormat textFormat = new StringFormat();
            textFormat.LineAlignment = StringAlignment.Center; // Вертикально
            textFormat.Alignment = StringAlignment.Near;       // Горизонтально (слева)

            e.Graphics.DrawString("   " + this.Text, this.Font, textBrush, textRect, textFormat);

            // 5. Рисуем иконку календаря
            // Если меню открыто (droppedDown), можно нарисовать фон под иконкой
            if (droppedDown)
            {
                // Слегка затемняем область иконки (круг или прямоугольник)
                // Но так как у нас скругленный край справа, лучше просто оставить как есть или нарисовать path
            }

            // Рисуем само изображение
            if (calendarIcon != null)
            {
                int iconY = (this.Height - calendarIcon.Height) / 2;
                int iconX = this.Width - calendarIcon.Width - 10; // Отступ справа
                e.Graphics.DrawImage(calendarIcon, iconX, iconY);
            }

            // 6. Рисуем рамку сверху
            if (borderSize > 0)
            {
                // Важно: PenAlignment.Inset рисует границу ВНУТРИ пути.
                // Поскольку мы уже уменьшили прямоугольник на 1 пиксель,
                // граница толщиной в 1 пиксель будет нарисована идеально.
                borderPen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(borderPen, path);
            }
        }
    }

    // Метод создания пути скругления (взят из DropDownRoundedButton)
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

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        // Зона клика для иконки (хотя DateTimePicker открывается кликом в любом месте)
        iconButtonArea = new RectangleF(this.Width - calendarIconWidth, 0, calendarIconWidth, this.Height);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        // Курсор всегда рука, так как это кнопка-календарь
        this.Cursor = Cursors.Hand;
    }
}