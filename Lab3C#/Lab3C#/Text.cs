class Text
{
    public List<Sentence> sentences;
    int quantity;

    public Text()
    {
        sentences = new List<Sentence>();
        quantity = 0;
    }

    public void AddSentence(Sentence sentence)
    {
        sentences.Add(sentence);
    }
}