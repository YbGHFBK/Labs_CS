using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //string text = File.ReadAllText("commands.txt");
        string testEncode = "AAAAAAAATATTTCGCTTTTCAAAAATTGTCAGATGAGAGAAAAAATAAAA";
        string testDecode = "3TIFD8U";
        Console.WriteLine(testEncode);
        Console.WriteLine(EncodeProtein(testEncode));
        Console.WriteLine("\n" + testDecode);
        Console.WriteLine(DecodeProtein(testDecode));


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