using System.Xml.Serialization;

[XmlRoot("Text")]
public class Text
{
    [XmlElement("Sentence")]
    public List<Sentence> Sentences { get; set; } = new List<Sentence>();

    public Text()
    {
        Sentences = new List<Sentence>();
    }
    public Text(List<Sentence> sentences)
    {
        this.Sentences = sentences;
    }

    public void AddSentence(Sentence sentence)
    {
        Sentences.Add(sentence);
    }

    public Text GetSortedByWordsCount()
    {
        List<Sentence> sortedSentences = new List<Sentence>(Sentences);

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
        List<Sentence> sortedSentences = new List<Sentence>(Sentences);

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

        for(int i = 0; i < Sentences.Count; i++)
        {
            if (Sentences[i].Words[Sentences[i].Words.Count - 1] is Punctuation punc && punc.getPunc() == "?")
            {
                newSentences.Add(GetLengthWords(Sentences[i], len));
            }
        }

        return new Text(newSentences);
    }

    private Sentence GetLengthWords(Sentence sentence, int len)
    {
        Sentence newSentence = new Sentence();

        foreach (Token token in sentence.Words)
        {
            if (token is Word word && word.Letters.Length == len)
            {
                newSentence.AddWordOrPunctuation(word);
            }
        }

        return newSentence;
    }

    public Text RemoveLengthСonsonantWords(int len) 
    {
        List<Sentence> newSentences = new List<Sentence>();

        for(int i = 0; i < Sentences.Count; i++)
        {
            Sentence sentence = new Sentence();
            for(int j = 0; j < Sentences[i].Words.Count; j++)
            {
                if (!(Sentences[i].Words[j] is Word word && word.Letters.Length == len && word.isStartsWithConsonant()))
                {
                    sentence.AddWordOrPunctuation(Sentences[i].Words[j]);
                }
            }
            newSentences.Add(sentence);
        }

        return new Text(newSentences);
    }

    public Sentence ReplaceSubstring(int sent, int len, string substring)
    {
        Sentence sentence = new Sentence();

        for(int i = 0; i < Sentences[sent-1].Words.Count; i++)
        {
            if (Sentences[sent-1].Words[i] is Word word && word.Letters.Length == len)
            {
                sentence.AddWordOrPunctuation(new Word(substring));
            }
            else sentence.AddWordOrPunctuation(Sentences[sent-1].Words[i]);
        }

        return sentence;
    }

    public Text RemoveStopWords(string[] stopWords)
    {
        List<Sentence> newSentences = new List<Sentence>();

        for(int i = 0; i < Sentences.Count; i++)
        {
            Sentence newSentence = new Sentence();
            for (int j = 0; j < Sentences[i].Words.Count; j++) {
                bool isStopWord = false;
                foreach (string stopWord in stopWords)
                {
                    if (Sentences[i].Words[j] is Word word && word.Letters == stopWord)
                    {
                        isStopWord = true;
                    }

                    if (isStopWord) continue;
                }
                if(!isStopWord) newSentence.AddWordOrPunctuation(Sentences[i].Words[j]);
            }

            newSentences.Add(newSentence);
        }

        return new Text(newSentences);
    }
}