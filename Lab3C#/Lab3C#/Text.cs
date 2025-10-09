class Text
{
    List<Sentence> text;
    int quantity;

    public Text()
    {
        text = new List<Sentence>();
        quantity = 0;
    }

    public void AddSentence(Sentence sentence)
    {
        text.Add(sentence);
    }
}