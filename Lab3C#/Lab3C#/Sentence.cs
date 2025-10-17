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

    public int GetWordsCount()
    {
        int count = 0;
        foreach (Token token in words)
        {
            if (token is Word) count++;
        }

        return count;
    }

    public int GetSentenceLength()
    {
        int count = 0;
        foreach (Token token in words)
        {
            if (token is Punctuation) count++;
            else if (token is Word word) count += word.letters.Length;
        }

        count += GetWordsCount() - 1;

        return count;
    }
}