using System.Text;
using System.Text.RegularExpressions;

public static class TextParser
{
    public static Text ParseText(string str)
    {
        List<Sentence> sentences = new List<Sentence>();

        StringBuilder sb = new StringBuilder();
        bool isRu = false;
        Sentence sentence = new Sentence();

        foreach (char c in str)
        {
            var (isLetter, isRussian) = IsEnglishOrRussianLetter(c, sb);
            if (isLetter)
            {
                isRu = isRussian;
                continue;
            }

            if (char.IsWhiteSpace(c))
            {
                MakeWord(sb, sentence);
                continue;
            }

            switch (IsPunctuation(c))
            {
                case 1:
                    MakeWord(sb, sentence);
                    continue;

                case 2:
                    MakeSentence(sentences, sentence);
                    sentence = new Sentence();
                    continue;

                case 0:
                    break;
            }
        }
        return new Text(sentences);
    }

    static (bool, bool) IsEnglishOrRussianLetter(char c, StringBuilder sb)
    {
        if (Regex.IsMatch(c.ToString(), "[а-яА-ЯёЁ]"))
        {
            sb.Append(c);
            return (true, true);
        }

        if (Regex.IsMatch(c.ToString(), "[a-zA-Z]"))
        {
            sb.Append(c);
            return (true, false);
        }

        return (false, false);
    }

    static byte IsPunctuation(char c)
    {
        string WordPunctuation = ",;:\"'-()[]{}";
        string SentencePunctuation = ".!?";

        if (WordPunctuation.Contains(c))
            return 1;

        if (SentencePunctuation.Contains(c))
            return 2;
       
        return 0;
    }

    static void MakeWord(StringBuilder sb, Sentence sentence)
    {
        if (sb.Length != 0)
        {
            Word word = new Word(sb.ToString());
            sentence.AddWord(word);
            sb.Clear();
        }
    }

    static void MakeSentence(List<Sentence> sentences, Sentence sentence)
    {
        sentences.Add(sentence);
    }
}