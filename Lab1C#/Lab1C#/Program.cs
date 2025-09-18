using System;
using System.IO;
using System.Text;

class Program
{
    public static string map = "ACDEFGHIKLMNPQRSTVWY";
    public static int comCounter = 1;

    static void Main(string[] args)
    {

        using (StreamWriter sw = new StreamWriter("TextFiles/genedata.txt", false))
        {
            sw.WriteLine("Maxim\nGenetic Searching\n" + new string('-', 40) + '\n');
        }

        string[] commands = ReadFile("TextFiles/commands.txt");
        string[] sequences = ReadFile("TextFiles/sequences.txt");


        GiveCommands(commands, sequences);
    }

    static string[] ReadFile(string path)
    {
        string[] text = File.ReadAllLines(path);
        return text;
    }

    static void AppendToFile(string path, string command, string result, bool doAppend = true)
    {
        StringBuilder sb = new StringBuilder();
        Console.WriteLine(comCounter.ToString("000") + " " + command);
        sb.Append(comCounter.ToString("000")  + " " + command + '\n');

        comCounter++;

        switch (command.Split('\t')[0])
        {
            case "search":
                {
                    Console.WriteLine("organism".PadRight(25) + "protein".PadRight(25));
                    sb.Append("organism".PadRight(25) + "protein".PadRight(40) + '\n');

                    Console.WriteLine(result + '\n' + new string('-', 40));
                    sb.Append(result + '\n' + new string('-', 40) + '\n');

                    break;
                }
            case "diff":
                {
                    Console.WriteLine("amino-acids difference:\n" + result + '\n' + new string('-', 40));
                    sb.Append("amino-acids difference:\n" + result + '\n' + new string('-', 40) + '\n');

                    break;
                }
            case "mode":
                {
                    Console.WriteLine("amino-acids occurs:\n" + result + '\n' + new string('-', 40));
                    sb.Append("amino-acids occurs:\n" + result + '\n' + new string('-', 40) + '\n');

                    break;
                }
        }
        
        using (StreamWriter sw = new StreamWriter(path, doAppend))
        {
            sw.WriteLine(sb.ToString());
        }
    }

    static void GiveCommands(string[] commands, string[] sequnces)
    {
        foreach (string command in commands)
        {
            string[] comParts = command.Split('\t');

            switch (comParts[0])
            {
                case "search":
                    {
                        bool result = false;
                        string organism = null, protein = null;
                        foreach (string sequence in sequnces)
                        {
                            string[] seqParts = sequence.Split("\t");

                            DecodeProtein(seqParts[2]);
                            DecodeProtein(comParts[1]);

                            result = Search(seqParts[2], comParts[1]);
                            if (result == true)
                            {
                                organism = seqParts[1];
                                protein = seqParts[0];
                                break;
                            }
                        }

                        AppendToFile("TextFiles/genedata.txt", command, (result ? (organism.PadRight(25) + protein.PadRight(25)) : "NOT FOUND")) ;

                        break;
                    }

                case "diff":
                    {
                        string protein1 = null;
                        string protein2 = null;

                        foreach (string sequence in sequnces)
                        {
                            string[] seqParts = sequence.Split('\t');

                            DecodeProtein(seqParts[2]);
                            DecodeProtein(comParts[1]);
                            DecodeProtein(comParts[2]);

                            if (seqParts[0] == comParts[1]) protein1 = seqParts[2];
                            if (seqParts[0] == comParts[2]) protein2 = seqParts[2];
                        }

                        string result;

                        if (protein1 != null && protein2 != null)
                        {
                            int diff = Diff(protein1, protein2);
                            result = diff.ToString();
                        }
                        else
                        {
                            result = "MISSING";
                        }

                        AppendToFile("TextFiles/genedata.txt", command, result);

                        break;
                    }

                case "mode":
                    {
                        string protein = null;

                        foreach(string sequence in sequnces)
                        {
                            string[] seqParts = sequence.Split('\t');

                            DecodeProtein(seqParts[2]);
                            DecodeProtein(comParts[1]);

                            if (seqParts[0] == comParts[1])
                            {
                                protein = seqParts[2];
                                break;
                            }
                        }

                        string result;

                        if(protein != null)
                        {
                            var mode = Mode(protein);
                            result = mode.prot + ":" + mode.count.ToString();
                        }
                        else
                        {
                            result = "MISSING";
                        }

                        AppendToFile("TextFiles/genedata.txt", command, result);

                        break;
                    }

            }
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

    static bool Search(string input, string desiredSequence)
    {
        return input.Contains(desiredSequence);
    }

    static int Diff(string firstProtein, string secondProtein)
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

    static (char prot, int count) Mode(string input)
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