public class Text
{
    public List<Sentence> sentences;
    int quantity;

    public Text()
    {
        sentences = new List<Sentence>();
        quantity = 0;
    }
    public Text(List<Sentence> sentences)
    {
        this.sentences = sentences;
        quantity = 0;
    }

    public void AddSentence(Sentence sentence)
    {
        sentences.Add(sentence);
    }
}