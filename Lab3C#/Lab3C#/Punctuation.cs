using System.Xml.Serialization;

[XmlRoot("Punctuation")]
public class Punctuation : Token
{
    [XmlText]
    public string Symbol { get; set; }

    public Punctuation() { }
    
    public Punctuation(string ch)
    {
        Symbol = ch;
    }

    public override string ToString()
    {
        return Symbol.ToString();
    }

    public string getPunc()
    {
        return Symbol;
    }
}