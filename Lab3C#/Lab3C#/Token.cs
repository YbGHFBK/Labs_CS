using System.Xml.Serialization;

[XmlInclude(typeof(Word))]
[XmlInclude(typeof(Punctuation))]
[XmlInclude(typeof(Sentence))]
public class Token
{
    public override string ToString() => base.ToString();
}