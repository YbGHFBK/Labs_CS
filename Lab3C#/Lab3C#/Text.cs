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

    public Text GetLengthWordsInQuestions(int len)
    {
        List<Sentence> newSentences = new List<Sentence>();

        for(int i = 0; i < sentences.Count; i++)
        {
            if (sentences[i].words[sentences[i].words.Count - 1] is Punctuation punc && punc.getChar() == '?')
            {
                newSentences.Add(GetLengthWords(sentences[i], len));
            }
        }

        return new Text(newSentences);
    }

    private Sentence GetLengthWords(Sentence sentence, int len)
    {
        Sentence newSentence = new Sentence();

        foreach (Token token in sentence.words)
        {
            if (token is Word word && word.letters.Length == len)
            {
                newSentence.AddWordOrPunctuation(word);
            }
        }

        return newSentence;
    }

    public Text RemoveLengthСonsonantWords(int len) 
    {
        List<Sentence> newSentences = new List<Sentence>();

        for(int i = 0; i < sentences.Count; i++)
        {
            Sentence sentence = new Sentence();
            for(int j = 0; j < sentences[i].words.Count; j++)
            {
                if (!(sentences[i].words[j] is Word word && word.letters.Length == len && word.isStartsWithConsonant()))
                {
                    sentence.AddWordOrPunctuation(sentences[i].words[j]);
                }
            }
            newSentences.Add(sentence);
        }

        return new Text(newSentences);
    }
}