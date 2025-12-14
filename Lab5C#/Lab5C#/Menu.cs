using System.Diagnostics;

public class Menu
{
    static List<Train> trains = new();
    static List<Route> routes = new();
    static List<Station> stations = new();

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
                //train = AddTrain();
                break;

            default:
                train = trains[choice - 1];
                break;
        }



        string nextText = "";

        while (true)
        {
            Console.Clear();

            if (!string.IsNullOrEmpty(nextText))
                Console.WriteLine($"{nextText}\n");

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
                    AddCarriege(train, nextText);
                    nextText = "Вагон успешно добавлен";
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
                    train.Id = 2;
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

    //private static Train AddTrain()
    //{
    //    Train train = new Train();
    //    Console.Write("Модель поезда:");
    //    train.model = Console.ReadLine();
    //    Console.Write("ID поезда:");
    //    train.Id = int.Parse(Console.ReadLine());

    //    train.Id = 1;

    //    return train;
    //}

    private static Route AddRoute()
    {
        Station routeStart = Search(stations, s => $"{s.country}, {s.city}");
        if (routeStart == null)
        {
            Console.Write("Ввведите отправную станцию маршрута:\nВведите страну: ");
            string country1 = Console.ReadLine();
            Console.Write("Введите город: ");
            string city1 = Console.ReadLine();

            routeStart = new Station(country1, city1, stations);

            FileWorker.SerializeToFile<Station>(routeStart, mainDir + stationsDir + routeStart.city + routeStart.Id + ".xml");
        }

        Station routeEnd = Search(stations, s => $"{s.country}, {s.city}");
        if (routeEnd == null)
        {
            Console.Write("Ввведите конечную станцию маршрута:\nВведите страну: ");
            string country2 = Console.ReadLine();
            Console.Write("Введите город: ");
            string city2 = Console.ReadLine();

            routeEnd = new Station(country2, city2, stations);

            FileWorker.SerializeToFile<Station>(routeEnd, mainDir + stationsDir + routeEnd.city + routeEnd.Id + ".xml");
        }

        Route route = new Route(routeStart, routeEnd, routes);

        FileWorker.SerializeToFile<Route>(
            route,
            mainDir + routesDir + route.routeStart.city + "-" + route.routeEnd.city + " " + route.Id + ".xml"
            );

        return route;
    }

    private static void SaveTrain(Train train)
    {
        FileWorker.SerializeToFile<Train>(train, mainDir + trainsDir + train.model + train.Id + ".xml");
    }

    private static List<(T, string, int)> SearchBy<T>(List<T> items, Func<T, string> getSearchText, string input, int linePosition)
    {
        var matches = items
       .Select(item => {
           var text = getSearchText(item) ?? string.Empty;
           int idx = text.IndexOf(input, StringComparison.OrdinalIgnoreCase);
           return (item, text, idx);
       })
       .Where(x => x.idx >= 0)
       .Take(5)
       .ToList();

        var prevBg = Console.BackgroundColor;
        var prevFg = Console.ForegroundColor;

        Console.WriteLine();
        for (int i = 0; i < matches.Count; i++)
        {
            var (item, text, start) = matches[i];
            int length = input.Length;

            if (start < 0) start = 0;
            if (start > text.Length) start = text.Length;
            if (start + length > text.Length) length = Math.Max(0, text.Length - start);

            try
            {
                if (i == linePosition - 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                //Console.Write($"{i + 1}. ");
                Console.Write(text.Substring(0, start));

                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;

                if (length > 0)
                    Console.Write(text.Substring(start, length));

                if (i == linePosition - 1)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = prevBg;
                    Console.ForegroundColor = prevFg;
                }
                    Console.WriteLine(text.Substring(start + length));
                //Console.WriteLine($"{text} [{start}, {start + length}]");
            }
            finally
            {
                Console.BackgroundColor = prevBg;
                Console.ForegroundColor = prevFg;
            }
        }
        return matches;
    }

    private static T? Search<T>(List<T> list, Func<T, string> getSearchText) where T : class
    {
        int cursorPosition = 0;
        int linePosition = 0;

        Console.Clear();
        Console.WriteLine("Введите запрос (ESC для выхода):\n");
        string currentInput = "";

        while (true)
        {
            var matches = SearchBy<T>(list, getSearchText, currentInput, linePosition);

            Console.SetCursorPosition(cursorPosition, 1);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Escape) return null;
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (currentInput.Length > 0)
                {
                    currentInput = currentInput.Substring(0, cursorPosition-1) + currentInput.Substring(cursorPosition);
                    cursorPosition--;
                    linePosition = 0;
                }
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow || keyInfo.Key == ConsoleKey.RightArrow)
            {
                if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (cursorPosition > 0)
                        cursorPosition--;
                }
                else if (cursorPosition < currentInput.Length)
                {
                    cursorPosition++;
                }
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.UpArrow)
            {
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if(linePosition < matches.Count)
                    {
                        linePosition++;
                    }
                }
                else if (linePosition > 0)
                {
                    linePosition--;
                }
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (linePosition != 0)
                {
                    return matches[linePosition - 1].Item1;
                }
            }
            else
            {
                currentInput = currentInput.Substring(0, cursorPosition) + keyInfo.KeyChar + currentInput.Substring(cursorPosition);
                cursorPosition++;
                linePosition = 0;
            }

            Console.Clear();
            Console.WriteLine("Введите запрос (ESC для выхода):" + cursorPosition);
            Console.WriteLine(currentInput);
        }
    }

    private static void AddCarriege(Train train, string nextText)
    {
        nextText = "ВАгролгн усшепоно доабавелен!:)";

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
    }
}