public class Sentence : Token
{
    public List<Token> words;

    public Sentence()
    {
        words = new List<Token>();
    }

    public void AddWordOrPunctuation(Token token)
    {
        words.Add(token);
    }

    public int WordsCount()
    {
        int count = 0;
        foreach (Token token in words)
        {
            if (token is Word) count++;
        }

        return count;
    }
}