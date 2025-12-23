
using System.ComponentModel;
using System.Drawing.Drawing2D;

[DefaultEvent("_TextChanged")]
public class RoundedTextBox : UserControl
{

    private Color borderColor = Color.MediumSlateBlue;
    private int borderSize = 2;
    private bool underlined = false;
    private TextBox tb;
    private Color borderFocusColor = Color.HotPink;
    private bool isFocused = false;
    private int borderRadius = 15;
    private Color placeholderColor = Color.DarkGray;
    private string placeholderText = string.Empty;
    private bool isPlaceholder = false;
    private bool isPassword = false;

    public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }
    public int BorderSize { get => borderSize; set { borderSize = value; Invalidate(); } }
    public bool Underlined { get => underlined; set { underlined = value; Invalidate(); } }
    public bool BorderFocusColor { get => underlined; set { underlined = value; Invalidate(); } }
    public bool UseSystemPasswordChar { get => isPassword; set { isPassword = value; Invalidate(); } }
    public char PasswordChar { get => tb.PasswordChar; set { tb.PasswordChar = value; Invalidate(); } }
    public override Color BackColor { get => base.BackColor; set { base.BackColor = value; tb.BackColor = value; } }
    public override Color ForeColor { get => base.ForeColor; set { base.ForeColor = value; tb.ForeColor = value; } }
    public override Font Font { get => base.Font; set 
        { 
            base.Font = value; tb.Font = value;
            if (DesignMode)
                UpdateControlHeight();
        }}
    public string TbText { get 
        {
            if (isPlaceholder) return string.Empty;
            return tb.Text; 
        } set 
        { 
            tb.Text = value;
            SetPlaceholder();
        }
    }
    public int BorderRadius { get => borderRadius; set
        {
            if (value >= 0)
            {
                borderRadius = value;
                this.Invalidate();
            }
        }
    }
    public Color PlaceholderColor { get => placeholderColor; set
        {
            placeholderColor = value;
            if (isPlaceholder)
                tb.ForeColor = value;
        }
    }
    public string PlaceholderText
    {
        get => placeholderText; set
        {
            placeholderText = value;
            tb.Text = string.Empty;
            SetPlaceholder();
        }
    }


    public RoundedTextBox()
    {
        tb = new TextBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
        };
        Controls.Add(tb);
        tb.TextChanged += Tb_TextChanged;
        tb.Click += Tb_Click;
        tb.MouseEnter += Tb_MouseEnter;
        tb.MouseLeave += Tb_MouseLeave;
        tb.KeyPress += Tb_KeyPress;
        tb.Enter += Tb_Enter;
        tb.Leave += Tb_Leave;

        BackColor = Color.White;
        ForeColor = Color.DimGray;
        Font = new Font("Segoe UI", 10f);
        AutoScaleMode = AutoScaleMode.None;
        Padding = new Padding(15);
        Size = new Size(250, 40);
    }

    public event EventHandler _TextChanged;

    private GraphicsPath GetFigurePath(Rectangle rect, int radius)
    {
        GraphicsPath path = new();
        float curveSize = radius * 2F;

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
        path.CloseFigure();

        return path;
    }   

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;

        if (borderRadius > 1)
        {
            var rectBorderSmooth = ClientRectangle;
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
            int smoothSize = borderSize > 0 ? borderSize : 1;

            using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
            using (Pen penBorderSmooth = new Pen(this.Parent.BackColor, smoothSize))
            using (Pen penBorder = new Pen(BorderColor, borderSize))
            {
                this.Region = new Region(pathBorderSmooth);

                if (borderRadius > 15) SetTextBoxRoundedRegion();

                g.SmoothingMode = SmoothingMode.AntiAlias;

                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                if (isFocused) penBorder.Color = borderFocusColor;

                if (underlined)
                {
                    g.DrawPath(penBorderSmooth, pathBorderSmooth);
                    g.SmoothingMode = SmoothingMode.None;
                    g.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                }
                else
                {
                    g.DrawPath(penBorderSmooth, pathBorderSmooth);

                    g.DrawPath(penBorder, pathBorder);
                }
            }
        }
        else
        {
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                this.Region = new Region(this.ClientRectangle);

                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                if (isFocused) penBorder.Color = borderFocusColor;

                if (underlined)
                    g.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                else
                    g.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
            }
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        if(this.DesignMode)
            UpdateControlHeight();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        UpdateControlHeight();
    }

    private void SetTextBoxRoundedRegion()
    {
        GraphicsPath pathTxt;

        if (tb.Multiline)
        {
            pathTxt = GetFigurePath(tb.ClientRectangle, borderRadius - borderSize);
            tb.Region = new Region(pathTxt);
        }
        else
        {
            pathTxt = GetFigurePath(tb.ClientRectangle, borderSize * 2);
            tb.Region = new Region(pathTxt);
        }
    }

    private void SetPlaceholder()
    {
        if (string.IsNullOrWhiteSpace(tb.Text) && placeholderText != "")
        {
            isPlaceholder = true;
            tb.Text = placeholderText;
            tb.ForeColor = placeholderColor;
            if (isPassword)
                tb.UseSystemPasswordChar = false;
        }    
    }

    private void RemovePlaceholder()
    {
        if (isPlaceholder && placeholderText != "")
        {
            isPlaceholder = false;
            tb.Text = "";
            tb.ForeColor = this.ForeColor;
            if (isPassword)
                tb.UseSystemPasswordChar = true;
        }
    }

    private void UpdateControlHeight()
    {
        if (tb.Multiline == false)
        {
            int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
            tb.Multiline = true;
            tb.MinimumSize = new Size(0, txtHeight);
            tb.Multiline = false;

            this.Height = tb.Height + this.Padding.Top + this.Padding.Bottom;
        }
    }

    private void Tb_TextChanged(object? sender, EventArgs e)
    {
        if (_TextChanged != null)
            _TextChanged.Invoke(sender, e);
    }

    private void Tb_Click(object? sender, EventArgs e)
    {
        this.OnClick(e);
    }

    private void Tb_MouseLeave(object? sender, EventArgs e)
    {
        this.OnMouseEnter(e);
    }

    private void Tb_MouseEnter(object? sender, EventArgs e)
    {
        this.OnMouseLeave(e);
    }

    private void Tb_KeyPress(object? sender, KeyPressEventArgs e)
    {
        this.OnKeyPress(e);
    }

    private void Tb_Enter(object? sender, EventArgs e)
    {
        isFocused = true;
        Invalidate();
        RemovePlaceholder();
    }

    private void Tb_Leave(object? sender, EventArgs e)
    {
        isFocused = false;
        Invalidate();
        SetPlaceholder();
    }
}