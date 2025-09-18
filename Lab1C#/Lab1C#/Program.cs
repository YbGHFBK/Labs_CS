using System;
using System.Text;

class Program
{
    public static string map = "ACDEFGHIKLMNPQRSTVWY";

    static void Main(string[] args)
    {
        string[] commands = ReadFile("TextFiles/commands.txt");
        string[] sequences = ReadFile("TextFiles/sequences.txt");


        GiveCommands(commands, sequences);

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
                        string organism, protein;
                        foreach (string sequence in sequnces)
                        {
                            string[] seqParts = sequence.Split("\t");
                            result = Search(comParts[1], seqParts[2]);
                            if (result == true)
                            {
                                organism = seqParts[1];
                                protein = seqParts[0];
                                break;
                            }
                        }
                        if (result == false)
                        {
                            //
                        }

                        break;
                    }

                case "diff":
                    {
                        string protein1 = null;
                        string protein2 = null;

                        foreach (string sequence in sequnces)
                        {
                            if (sequence == comParts[1]) protein1 = comParts[1];
                            if (sequence == comParts[2]) protein1 = comParts[2];
                        }

                        int diff = 0;

                        if (protein1 != null && protein2 != null)
                        {
                            diff = Diff(protein1, protein2);
                            //
                        }
                        else
                        {
                            //
                        }

                        break;
                    }

                case "mode":
                    {
                        string protein = null;

                        foreach(string sequence in sequnces)
                        {
                            if(sequence == comParts[1])
                            {
                                protein = comParts[1];
                                break;
                            }
                        }

                        if(protein != null)
                        {
                            Mode(protein);
                            //
                        }
                        else
                        {
                            //
                        }


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

    static void CheckMethod()
    {
        string text1 = "MLQSIIKNIWIPMKPYYTKVYQEIWIGMGLMGFIVYKIRAADKRSKALKASAPAPGHH";
        string text2 = "MDTTGKVIKCKAAVAWEAGKPLTIEEVEVAPPKAHEVRVKIHATGVCHTDAYTLSGSDPEGLFPVILGHEGAGTVESVGEGVTK";

        CheckProtein(text1);
        CheckProtein(text2);

        DecodeProtein(text1);
        DecodeProtein(text2);
        Console.WriteLine(Search(text1, "SIIK") ? "FOUND" : "NOT FOUND");
        Console.WriteLine("diff: " + Diff(text1, text2));
        (char prot, int count) result = Mode(text2);
        Console.WriteLine(result.prot + ":" + result.count);
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