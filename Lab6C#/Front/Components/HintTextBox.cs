using System.ComponentModel;
using System.Runtime.InteropServices;

public class HintTextBox : TextBox
{
    private string _cueText = "";
    private Padding _textPadding = new Padding(10);

    [Category("Appearance")]
    public string CueText
    {
        get => _cueText;
        set { _cueText = value; UpdateCueText(); }
    }

    [Category("Appearance")]
    public Padding TextPadding
    {
        get => _textPadding;
        set
        {
            _textPadding = value;
            SetTextMargins();
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        UpdateCueText();
        SetTextMargins();
    }

    private void UpdateCueText()
    {
        if (IsHandleCreated && !string.IsNullOrEmpty(_cueText))
            SendMessage(Handle, 0x1501, 1, _cueText);  // EM_SETCUEBANNER
    }

    private void SetTextMargins()
    {
        if (IsHandleCreated)
        {
            // EM_SETMARGINS = 0x00D3
            int left = TextPadding.Left;
            int right = TextPadding.Right;
            SendMessage(Handle, 0x00D3, 1, (right << 16) | left);
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);
}
