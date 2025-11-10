public static class Program
{
    static string inFile = "TextFiles/carrieges.txt";
    static string outFile = "TextFiles/output.xml";
    public static void Main(string[] args)
    {

        Train train = new Train();

        FillReadLines(train);

        Console.WriteLine(train.ToString());
    }

    private static string[] ReadFile(string path)
    {
        try
        {
            string[] lines = File.ReadAllLines(path);
            return lines;
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка чтения файла");
            throw;
        }
    }

    private static Carriege CheckLine(string[] strings, out bool IsCarriege)
    {
        switch (strings[0])
        {
            case "Locomotive":
                IsCarriege = true;
                return new Locomotive(GetParsed(strings[1]));

            case "Passenger":
                IsCarriege = true;
                return new PassengerCarriege(GetParsed(strings[1]), GetParsed(strings[2]));

            case "Cargo":
                IsCarriege = true;
                return new CargoCarriege(GetParsed(strings[1]));

            default:
                throw new FormatException("Неизвестное название вагона");
        }
    }

    private static int GetParsed(string str)
    {
        if(int.TryParse(str, out int result))
        {
            return result;
        }
        Console.WriteLine("вторая инструкция должна быть целым числом");
        throw new FormatException();
    }

    private static void FillReadLines(Train train)
    {
        bool isCarriege = false;
        Carriege car = null;
        foreach (var line in ReadFile(inFile))
        {
            if (!isCarriege)
                train.Add(
                    CheckLine(
                        line.Split('\t'),
                        out isCarriege
                        )
                    );
            else
                train.carrieges[train.carrieges.Count - 1];

        }
    }
}