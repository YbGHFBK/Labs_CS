public class Punctuation : Token
{
    char ch;

    public Punctuation(char ch)
    {
        this.ch = ch;
    }

    public override string ToString()
    {
        return ch.ToString();
    }

    public char getChar()
    {
        return ch;
    }
}