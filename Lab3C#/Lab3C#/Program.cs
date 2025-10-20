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
        string outputFile = "TextFiles/output.xml";

        Text text = new Text();

        string str = ReadFile(inputFile);

        text = TextParser.ParseText(str);

        //PrintParsingResult(text);

        bool exit = false;

        while (!exit)
        {
            Console.Write("""
                
                Выберите пункт меню:
                1. Вывести все предложения заданного текста в порядке возрастания количества слов в предложениях
                2. Вывести все предложения заданного текста в порядке возрастания длины предложения
                3. Во всех вопросительных предложениях текста найти слова заданной длины
                4. Удалить из текста все слова заданной длины, начинающиеся с согласной буквы
                5. В некотором предложении текста заменить слова заданной длины на указанную подстроку, длина которой может не совпадать с длиной слова
                6. Удалить стоп-слова
                7. Экспортировать текстовый объект в XML-документ
                0. Выход

                Ваш выбор: 
                """);

            byte choice = 0;
            while (true)
            {
                try
                {
                    choice = byte.Parse(Console.ReadLine());

                    switch (choice)
                        {
                            case 1:
                                PrintSentences(text.GetSortedByWordsCount());
                                break;

                            case 2:
                                PrintSentences(text.GetSortedBySentenceLength());
                                break;

                            case 3:
                                Console.Write("Выберите длину слов, которые надо найти: ");
                                PrintSentences(text.GetLengthWordsInQuestions(int.Parse(Console.ReadLine())));
                                break;

                            case 4:
                                Console.Write("Выберите длину: ");
                                PrintSentences(text.RemoveLengthСonsonantWords(int.Parse(Console.ReadLine())));
                                break;

                            case 5:
                                Console.Write("Введите номер предложения: ");
                                int sentNum = int.Parse(Console.ReadLine());
                                Console.Write("Введите длину: ");
                                int len = int.Parse(Console.ReadLine());
                                Console.Write("Введите подстроку: ");
                                String line = Console.ReadLine();

                                PrintSentences(
                                    new Text
                                    (new List<Sentence>
                                        { text.ReplaceSubstring(sentNum, len, line)
                                    }));
                                break;

                            case 6:
                                PrintSentences(text.RemoveStopWords(ReadFileLines(stopWordsRu)));
                                break;

                            case 7:
                                XmlSerializerHelper.SerializeToFile(text, outputFile);
                                break;

                            case 8:
                                Console.WriteLine(XmlSerializerHelper.DeserializeFromFile1(outputFile));
                                break;

                            case 0:
                                exit = true;
                                break;
                        }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                break;
            }
        }
    }

    static string ReadFile(string path)
    {
        try
        {
            string text  = File.ReadAllText(path);
            return text;
        }
        catch (IOException ex)
        {
            Console.WriteLine("Файл не найден\n");
            throw;
        }
    }

    static string[] ReadFileLines(string path)
    {
        try
        {
            string[] text = File.ReadAllLines(path);
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
        foreach (Sentence sentence in text.Sentences)
        {
            Console.Write("\t<sentence>\n");
            foreach (Word word in sentence.Words)
            {
                Console.Write("\t\t<word>" + word.Letters + "</word>\n");
            }
            Console.Write("\t</sentence>\n");
        }

        Console.WriteLine("</text>");
    }

    static void PrintSentences(Text text)
    {
        foreach(Sentence sentence in text.Sentences)
        {
            bool isLastWord = false;
            foreach (Token token in sentence.Words)
            {
                if (token is Word)
                {
                    if (isLastWord)
                        Console.Write(" " + token.ToString());
                    else
                        Console.Write(token.ToString());
                    isLastWord = true;
                }
                else if (token is Punctuation)
                {
                    isLastWord = false;
                    Console.Write(token.ToString() + " ");
                }
            }
            Console.WriteLine();
        }
    }

}