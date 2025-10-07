class Program
{

    static void Main(string[] args)
    {
        Game.inputFile = "TextFiles/ChaseData.txt";
        Game.outputFile = "TextFiles/PursuitLog.txt";

        string[] text = ReadFile(Game.inputFile);
        CheckFile(text);

        Game.size = int.Parse(text[0]);

        Game game = new Game();
        game.Run(text);

    }

    static string[] ReadFile(string path)
    {
        try
        {
            string[] text = File.ReadAllLines(path)
                                .Where(line => !string.IsNullOrEmpty(line))
                                .ToArray();
            return text;
        }
        catch (IOException ex)
        {
            Console.WriteLine("Файл не найден\n");
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    static void CheckFile(string[] text)
    {
        if (text.Length == 0 || !int.TryParse(text[0], out _)) throw new FormatException("First line must contain board size");

        for (int i = 1; i < text.Length; i++)
        {
            string[] parts = text[i].Split('\t');

            switch (parts[0])
            {
                default:
                    break;

                case "M":
                    if (parts.Length != 2) throw new FormatException("Invalid M command format in line: " + (i + 1));
                    break;

                case "C":
                    if (parts.Length != 2) throw new FormatException("Invalid C command format in line: " + (i + 1));
                    break;

                case "P":
                    if (parts.Length != 1) throw new FormatException("Invalid P command format in line: " + (i + 1));
                    break;
            }
        }
    }
}