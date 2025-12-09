public class Menu
{
    static string file = "";
    static string mainDir = "TextFiles/";
    static string trainsDir = "Trains/";
    static string routesDir = "Routes/";
    static string stationsDir = "Stations/";

    static public void MainLoop()
    {

        Train train = null;

        string[] trainsFiles = Directory.GetFiles(mainDir + trainsDir);
        string[] routesFiles = Directory.GetFiles(mainDir + routesDir);
        string[] stationsFiles = Directory.GetFiles(mainDir + stationsDir);

        List<Train> trains = new();
        List<Route> routes = new();
        List<Station> stations = new();

        foreach(string trainFile in trainsFiles)
        {
            trains.Add(
                FileWorker.DeserializeFromFile<Train>(trainFile)
                );
        }
        foreach (string routeFile in routesFiles)
        {
            routes.Add(
                FileWorker.DeserializeFromFile<Route>(routeFile)
                );
        }
        foreach (string stationFile in stationsFiles)
        {
            stations.Add(
                FileWorker.DeserializeFromFile<Station>(stationFile)
                );
        }

        Console.WriteLine($"Выберите один из сохранённых поездов {1}-{trainsFiles.Length} или 0 для создания нового:");
        PrintList(trains);

        switch (MakeChoice(0, trainsFiles.Length, out int choice))
        {
            case 0:
                train = AddTrain();
                break;

            default:
                train = trains[choice - 1];
                break;
        }





        while (true)
        {
            Console.Write("""
                1. Добавить вагон
                2. Удалить вагон
                3. Добавить объект в вагон
                4. Изменить id поезда
                5. Вывести поезд
                6. Выполнить поиск по маршрутам
                7. Сохранить текущий поезд
                8. Добавить маршрут
                0. Выход
                Ваш выбор: 
                """);



            switch (MakeChoice(out choice))
            {
                case 1:
                    Console.Write("""
                        1. Грузовой вагон
                        2. Пассаижрский вагон
                        3. Локомотив
                        Ваш выбор:
                        """);
                    switch (MakeChoice())
                    {
                        case 1:
                            if (train.Locomotives != 1)
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
                            if (train.Locomotives > 2)
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
                        if (choice == 1 && train.carrieges.Count != 1)
                        {
                            Console.WriteLine("Нельзя удалить первый локомотив, когда за ним ещё есть вагоны");
                            throw new FormatException();
                        }
                        train.Delete(MakeChoice() - 1);
                    }
                    catch (Exception ex)
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

                        switch (train.carrieges[choice - 1].GetClass())
                        {
                            case "PassengerCarriege":
                                Console.WriteLine("""
                                    Добавить:
                                    1. Пассажира
                                    2. Багаж
                                    """);
                                switch (MakeChoice())
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
                        switch (MakeChoice()) { }
                        if (item != null)
                            train.carrieges[choice - 1].Add(item);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Введите существующий номер вагона.");
                    }
                    continue;

                case 4:
                    train.id = 2;
                    break;

                case 5:
                    Console.WriteLine(train.ToString());
                    continue;

                case 6:
                    Search(routes, r => $"{r.routeStart.city}-{r.routeEnd.city}");
                    break;

                case 7:
                    SaveTrain(train);
                    break;

                case 8:
                    AddRoute();
                    break;

                case 0:
                    break;

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

    private static int MakeChoice(int leftBound, int rightBound, out int choice)
    {
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Введите целое число");
                continue;
            }
            if (choice < leftBound ||  choice > rightBound)
            {
                Console.WriteLine("Введите целое число " + leftBound + "-" + rightBound);
                continue;
            }
            return choice;
        }
    }

    private static void PrintList<T>(List<T> list)
    {
        int count = 1;
        foreach(T item in list)
        {
            Console.WriteLine((count) + ". " + item.ToString());
            count++;
        }
    }

    private static Train AddTrain()
    {
        Train train = new Train();
        Console.Write("Модель поезда:");
        train.model = Console.ReadLine();
        Console.Write("ID поезда:");
        train.id = int.Parse(Console.ReadLine());

        train.id = 1;

        return train;
    }

    private static Route AddRoute()
    {
        Console.Write("Ввведите отправную станцию маршрута:\nВведите страну: ");
        string country1 = Console.ReadLine();
        Console.Write("Введите город: ");
        string city1 = Console.ReadLine();
        Console.Write("Введите ID: ");
        int id1 = int.Parse(Console.ReadLine());

        Station routeStart = new Station(country1, city1, id1);

        FileWorker.SerializeToFile<Station>(routeStart, mainDir + stationsDir + routeStart.city + routeStart.id + ".xml");



        Console.Write("Ввведите конечную станцию маршрута:\nВведите страну: ");
        string country2 = Console.ReadLine();
        Console.Write("Введите город: ");
        string city2 = Console.ReadLine();
        Console.Write("Введите ID: ");
        int id2 = int.Parse(Console.ReadLine());

        Station routeEnd = new Station(country2, city2, id2);

        FileWorker.SerializeToFile<Station>(routeEnd, mainDir + stationsDir + routeEnd.city + routeEnd.id + ".xml");


        Console.Write("Введите id маршрута: ");
        int id3 = int.Parse(Console.ReadLine());

        Route route = new Route(routeStart, routeEnd, id3);

        FileWorker.SerializeToFile<Route>(
            route,
            mainDir + routesDir + route.routeStart.city + "-" + route.routeEnd.city + " " + route.id + ".xml"
            );

        return route;
    }

    private static void SaveTrain(Train train)
    {
        FileWorker.SerializeToFile<Train>(train, mainDir + trainsDir + train.model + train.id + ".xml");
    }

    private static void SearchBy<T>(List<T> items, Func<T, string> getSearchText, string input)
    {
        var matches = items
            .Where(item => getSearchText(item).Contains(input,
            StringComparison.OrdinalIgnoreCase))
            .Take(5)
            .ToList();

        Console.WriteLine();
        for (int i = 0; i < matches.Count; i++)
        {
            try
            {
                Console.WriteLine($"{i + 1}. {getSearchText(matches[i])}");
            }
            catch (Exception e) { }
        }
    }

    private static void Search<T>(List<T> list, Func<T, string> getSearchText)
    {
        Console.Clear();
        Console.WriteLine("Введите запрос (ESC для выхода):");
        string currentInput = "";

        while (true)
        {
            SearchBy<T>(list, getSearchText, currentInput);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Escape) break;
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if(currentInput.Length > 0)
                    currentInput = currentInput[..^1];
            }
            else
            {
                currentInput += keyInfo.KeyChar;
            }

            Console.Clear();
            Console.WriteLine("Введите запрос (ESC для выхода):");
            Console.WriteLine(currentInput);
        }
    }

    //static IEnumerable<string> GetSuggestions<T>(string input, List<T> list)
    //{
    //    if (string.IsNullOrEmpty(input)) return list;
    //    return list.Where(s => s.Contains(input, StringComparison.OrdinalIgnoreCase));
    //}
}