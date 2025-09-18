using System;
using System.Text;

class Program
{
    public static string map = "ACDEFGHIKLMNPQRSTVWY";

    static void Main(string[] args)
    {
        string[] commands = ReadFile("TextFiles/commands.txt");
        string[] sequences = ReadFile("TextFiles/sequences.txt");

        CheckMethod();
    }

    static string[] ReadFile(string path)
    {
        string[] text = File.ReadAllLines(path);
        return text;
    }

    static void AppendToFile(string path, string text, bool doAppend)
    {
        using (StreamWriter sw = new StreamWriter(path, doAppend))
        {
            sw.WriteLine(text);
        }
    }

    static bool CheckProtein(string input)
    {
        bool isWrong = false;
        foreach (char ch  in input)
        {
            if(!map.Contains(ch))
            {
                isWrong = true;
            }
        }

        if(isWrong)
        {
            return false;
        }
        return true;
    }

    static void CheckMethod()
    {
        string text1 = "MLQSIIKNIWIPMKPYYTKVYQEIWIGMGLMGFIVYKIRAADKRSKALKASAPAPGHH";
        string text2 = "MDTTGKVIKCKAAVAWEAGKPLTIEEVEVAPPKAHEVRVKIHATGVCHTDAYTLSGSDPEGLFPVILGHEGAGTVESVGEGVTK";

        CheckProtein(text1);
        CheckProtein(text2);

        DecodeProtein(text1);
        DecodeProtein(text2);
        Console.WriteLine(search(text1, "SIIK") ? "FOUND" : "NOT FOUND");
        Console.WriteLine("diff: " + diff(text1, text2));
        (char prot, int count) result = mode(text2);
        Console.WriteLine(result.prot + ":" + result.count);
    }

    static bool search(string input, string desiredSequence)
    {
        return input.Contains(desiredSequence);
    }

    static int diff(string firstProtein, string secondProtein)
    {
        int diff = 0;
        for (int i = 0; i < Math.Min(firstProtein.Length, secondProtein.Length); i++)
        {
            if (firstProtein[i] != secondProtein[i])
            {
                diff++;
            }
        }
        diff += Math.Abs(firstProtein.Length - secondProtein.Length);
        return diff;
    }

    static (char prot, int count) mode(string input)
    {
        int[] map = new int[26];
        foreach (char ch in input)
        {
            if (ch >= 'A' && ch <= 'Z')
            {
                map[ch - 'A']++;
            }
        }
        int maxValue = 0;
        char prot = ' ';

        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] > maxValue)
            {
                maxValue = map[i];
                prot = (char)('A' + i);
            }
        }

        return (prot, maxValue);
    }

    static string DecodeProtein(string protein)
    {

        if (string.IsNullOrEmpty(protein)) return protein;

        StringBuilder sb = new StringBuilder(capacity: 20);
        int indexNumber = 0;

        foreach (char nextChar in protein)
        {
            if (char.IsDigit(nextChar))
            {
                indexNumber = indexNumber * 10 + (int)char.GetNumericValue(nextChar);
            }
            else
            {
                if (indexNumber != 0)
                {
                    sb.Append(nextChar, indexNumber);
                    indexNumber = 0;
                }
                else
                {
                    sb.Append(nextChar);
                }
            }
        }

        return sb.ToString();

    }

    static string EncodeProtein(string protein)
    {

        if (string.IsNullOrEmpty(protein)) return protein;

        StringBuilder sb = new StringBuilder(capacity: 20);
        char prev = protein[0];
        int count = 1;

        for (int i = 1; i < protein.Length; i++)
        {
            char c = protein[i];
            if (c == prev)
            {
                count++;
            }
            else
            {
                if (count >= 3)
                {
                    sb.Append(count).Append(prev);
                }
                else
                {
                    sb.Append(prev, count);
                }

                prev = c;
                count = 1;
            }
        }

        if (count >= 3)
            sb.Append(count).Append(prev);
        else
            sb.Append(prev, count);

        return sb.ToString();
    }
}