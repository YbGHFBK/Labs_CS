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

    public Text GetSortedByWordsCount()
    {
        List<Sentence> sortedSentences = new List<Sentence>(sentences);

        for (int i = 0; i < sortedSentences.Count - 1; i++)
        {
            int minInd = i;
            for (int j = i + 1; j < sortedSentences.Count; j++)
            {

                if (sortedSentences[j].GetWordsCount() < sortedSentences[minInd].GetWordsCount())
                {
                    minInd = j;
                }
            }

            Sentence temp = sortedSentences[minInd];
            sortedSentences[minInd] = sortedSentences[i];
            sortedSentences[i] = temp;
        }

        return new Text(sortedSentences);
    }

    public Text GetSortedBySentenceLength()
    {
        List<Sentence> sortedSentences = new List<Sentence>(sentences);

        for (int i = 0; i < sortedSentences.Count - 1; i++)
        {
            int minInd = i;
            for (int j = i + 1; j < sortedSentences.Count; j++)
            {

                if (sortedSentences[j].GetSentenceLength() < sortedSentences[minInd].GetSentenceLength())
                {
                    minInd = j;
                }
            }

            Sentence temp = sortedSentences[minInd];
            sortedSentences[minInd] = sortedSentences[i];
            sortedSentences[i] = temp;
        }

        return new Text(sortedSentences);
    }
}