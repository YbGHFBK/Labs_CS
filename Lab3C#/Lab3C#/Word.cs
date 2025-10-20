using System.Xml.Serialization;

[XmlRoot("Word")]
public class Word : Token
{
    [XmlText]
    public string Letters { get; set; }
    public Word(string letters)
    {
        this.Letters = letters;
    }

    public Word() { }

    public override string ToString()
    {
        return Letters;
    }

    public bool isStartsWithConsonant()
    {
        string consonants = "бвгджзклмнпрстфхцчшщbcdfghjklmnpqrstvwxz";
        if (consonants.Contains(Letters.ToLower()[0])) return true;
        return false;
    }
}