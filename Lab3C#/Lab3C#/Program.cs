using System;
using System.Collections.Generic;
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

        text = TextParser.ParseText(str);

        //PrintParsingResult(text);

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
                    PrintSentences(SortByWordsCount(text.sentences));
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
    }

    static void PrintParsingResult(Text text)
    {
        Console.Write("<text>\n");
        foreach (Sentence sentence in text.sentences)
        {
            Console.Write("\t<sentence>\n");
            foreach (Word word in sentence.words)
            {
                Console.Write("\t\t<word>" + word.letters + "</word>\n");
            }
            Console.Write("\t</sentence>\n");
        }

        Console.WriteLine("</text>");
    }

    static void PrintSentences(Text text)
    {
        foreach(Sentence sentence in text.sentences)
        {
            foreach (Token token in sentence.words)
            {
                Console.Write(token.ToString() + " ");
            }
            Console.WriteLine(" " + sentence.WordsCount());
        }
    }

    static Text SortByWordsCount(List<Sentence> sentences)
    {
        List<Sentence> sortedSentences = new List<Sentence>(sentences);

        for (int i = 0; i < sortedSentences.Count - 1; i++)
        {
            int minInd = i;
            for (int j = i + 1; j < sortedSentences.Count; j++)
            {
                
                if (sortedSentences[j].WordsCount() < sortedSentences[minInd].WordsCount())
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