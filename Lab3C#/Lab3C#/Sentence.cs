class Sentence
{
    List<Word> words;

    public Sentence()
    {
        words = new List<Word>();
    }

    public void AddWord(Word word)
    {
        words.Add(word);
    }
}