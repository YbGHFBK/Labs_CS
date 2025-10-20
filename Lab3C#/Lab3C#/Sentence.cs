using System.Xml.Serialization;

[XmlRoot("Sentence")]
public class Sentence : Token
{
    [XmlElement("Word", typeof(Word))]
    [XmlElement("Punctuation", typeof(Punctuation))]
    public List<Token> Words { get; set; } = new List<Token>();

    public Sentence()
    {
        Words = new List<Token>();
    }

    public void AddWordOrPunctuation(Token token)
    {
        Words.Add(token);
    }

    public int GetWordsCount()
    {
        int count = 0;
        foreach (Token token in Words)
        {
            if (token is Word) count++;
        }

        return count;
    }

    public int GetSentenceLength()
    {
        int count = 0;
        foreach (Token token in Words)
        {
            if (token is Punctuation) count++;
            else if (token is Word word) count += word.Letters.Length;
        }

        count += GetWordsCount() - 1;

        return count;
    }
}