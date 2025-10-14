using System;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {

        string inputFile = "TextFiles/Text.txt";
        string stopWordsEn = "TextFiles/StopWordsEn.txt";
        string stopWordsRu = "TextFiles/StopWordsRu.txt";

        Text text = new Text();

        string str = ReadFile(inputFile);

        ParseText(str, text);

        PrintParsingResult(text);

        bool exit = false;

        while (!exit)
        {
            Console.Write("""
                1. Вывести все предложения заданного текста в порядке возрастания количества слов в предложениях
                2. Вывести все предложения заданного текста в порядке возрастания длины предложения
                3. Во всех вопросительных предложениях текста найти слова заданной длины
                4. Удалить из текста все слова заданной длины, начинающиеся с согласной буквы
                5. В некотором предложении текста заменить слова заданной длины на указанную подстроку, длина которой может не совпадать с длиной слова
                6. Удалить стоп-слова
                7. Экспортировать текстовый объект в XML-документ
                0. Выход

                Выберите: 
                """);

            byte choice = byte.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;

                case 6:
                    break;

                case 7:
                    break;

                case 0:
                    exit = true;
                    break;
            }
        }
    }

    static string ReadFile(string path)
    {
        try
        {
            string text = File.ReadAllText(path);

            return text;
        }
        catch (IOException ex)
        {
            Console.WriteLine("Файл не найден\n");
            throw;
        }
        catch (Exception ex)  //finaly
        {
            throw;
        }
    }

    static void ParseText(string str, Text text)
    {
        StringBuilder sb = new StringBuilder();
        bool isRu = false;
        Sentence sentence = new Sentence();

        foreach(char c in str)
        {
            if (IsEnglishOrRussianLetter(c, sb).Item1)
            {
                //isRu = IsEnglishOrRussianLetter(c, sb).Item2;
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
                    break;

                case 2:
                    MakeSentence(text, sentence);
                    sentence = new Sentence();
                    break;

                case 0:
                    break;
            }
        }
    }

    static (bool, bool) IsEnglishOrRussianLetter(char c, StringBuilder sb)
    {
        if (Regex.IsMatch(c.ToString(), "[а-яА-ЯёЁ]")) /////////////////////////////////////////////////////
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
        if(sb.Length != 0)
        {
            Word word = new Word(sb.ToString());
            sentence.AddWord(word);
            sb.Clear();
        }
    }

    static void MakeSentence(Text text, Sentence sentence)
    {
        text.AddSentence(sentence);
    }

    static void PrintParsingResult(Text text)
    {
        foreach (Sentence sentence in text.sentences)
            foreach (Word word in sentence.words)
            {
                Console.Write(word.letters + " ");
            }

        //Console.WriteLine(text.sentences[0].words[0].letters);
    }
}