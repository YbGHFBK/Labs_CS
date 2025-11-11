public static class Program
{
    static string inFile = "TextFiles/in.xml";
    static string outFile = "TextFiles/output.xml";
    static string file = "TextFiles/file.xml";
    public static void Main(string[] args)
    {
        Train train = new Train();

        train = Serializer.DeserializeFromFile<Train>(file);

        MainLoop(train);
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

    static private void MainLoop(Train train)
    {
        while (true)
        {
            Console.Write("""
                1. Добавить вагон
                2. Удалить вагон
                3. Добавить объект в вагон
                5. Вывести поезд
                0. Выход
                Ваш выбор: 
                """);

            

            switch(MakeChoice(out int choice))
            {
                case 1:
                    Console.Write("""
                        1. Грузовой вагон
                        2. Пассаижрский вагон
                        3. Локомотив
                        Ваш выбор:
                        """);
                    switch(MakeChoice())
                    {
                        case 1:
                            if(train.Locomotives != 1)
                            {
                                Console.WriteLine("Вагон должен находится между начальным и конечным локомотивом.");
                                break;
                            }
                            train.Add(new CargoCarriege());
                            break;

                        case 2:
                            if (train.Locomotives != 1)
                            {
                                Console.WriteLine("Вагон должен находится между начальным и конечным локомотивом.");
                                break;
                            }
                            train.Add(new PassengerCarriege());
                            break;

                        case 3:
                            if(train.Locomotives > 2)
                            {
                                Console.WriteLine("У поезда не может быть больше двух локомотивов.");
                                break;
                            }
                            train.Add(new Locomotive());
                            break;

                        default:
                            Console.WriteLine("Введите целое число 1-3.");
                            break;
                    }
                    continue;

                case 2:
                    Console.Write("Введите номер вагона для удаления: ");
                    try
                    {
                        MakeChoice(out choice);
                        if(choice == 1 && train.carrieges.Count != 1)
                        {
                            Console.WriteLine("Нельзя удалить первый локомотив, когда за ним ещё есть вагоны");
                            throw new FormatException();
                        }
                        train.Delete(MakeChoice()-1);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Введите существующий номер вагона.");
                    }
                    continue;

                case 3:
                    Console.Write("Введите номер вагона для добавления объекта: ");
                    try
                    {
                        MakeChoice(out choice);
                        Item item = null;

                        switch(train.carrieges[choice - 1].GetClass())
                        {
                            case "PassengerCarriege":
                                Console.WriteLine("""
                                    Добавить:
                                    1. Пассажира
                                    2. Багаж
                                    """);
                                switch(MakeChoice())
                                {
                                    case 1:
                                        item = (Passenger)new Passenger();
                                        break;

                                    case 2:
                                        item = (Baggage)new Baggage();
                                        break;

                                    default:
                                        Console.WriteLine("Введите целое число 1-2");
                                        break;
                                }
                                break;

                            case "CargoCarriege":
                                Console.WriteLine("""
                                    Добавить:
                                    1. Груз
                                    """);
                                switch (MakeChoice())
                                {
                                    case 1:
                                        item = (Cargo)new Cargo();
                                        break;

                                    default:
                                        Console.WriteLine("Введите целое число 1-1");
                                        break;
                                }
                                break;

                            case "Locomotive":
                                break;
                        }
                        switch(MakeChoice()) { }
                        if(item != null)
                            train.carrieges[choice - 1].Add(item);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Введите существующий номер вагона.");
                    }
                    continue;

                case 5:
                    Console.WriteLine(train.ToString());
                    continue;

                case 0:
                    Serializer.SerializeToFile(train, file);
                    break; ;

                default:
                    Console.WriteLine("Введите целое число 0-5");
                    break;
            }

            if (choice == 0) break;
        }
    }

    private static int MakeChoice(out int choice)
    {
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Введите целое число");
                continue;
            }
            return choice;
        }
    }
    private static int MakeChoice()
    {
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Введите целое число");
                continue;
            }
            return choice;
        }
    }
}