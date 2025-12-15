using System.Diagnostics;
using System.Text;

public class Menu
{
    static List<Train> trains = new();
    static List<Route> routes = new();
    static List<Station> stations = new();
    static List<Passenger> users = new();
    static List<Ticket> tickets = new();

    static string mainDir = "TextFiles/";
    static string trainsDir = mainDir + "Trains/";
    static string routesDir = mainDir + "Routes/";
    static string stationsDir = mainDir + "Stations/";
    static string usersDir = mainDir + "Users/";
    static string ticketsDir = mainDir + "Tickets/";

    static public void MainLoop()
    {

        DefineLists();

        Passenger currentUser = new Passenger();
        while (true)
        {
            currentUser = Logger.AuthUser(users);

            if (currentUser != null)
                break;
        }
        users.Add(currentUser);
        FileWorker.SerializeToFile(currentUser, usersDir + currentUser.login + currentUser.Id + ".xml");

        //Train train = PickTrain();

        string nextText = "Вы успешно вошли в аккаунт";

        while (true)
        {
            Console.Clear();

            if (!string.IsNullOrEmpty(nextText))
                Console.WriteLine($"{nextText}\n");

            Console.Write("""
                Выберите пункт меню:
                1. Вывести данные о пользователе
                2. Выполнить поиск по маршрутам
                3. Добавить маршрут
                4. Купить билет
                5. Просмотреть свои билеты
                6. Действия с поездами
                0. Выход
                Ваш выбор: 
                """);



            switch (MakeChoice(out int choice))
            {
                case 1:
                    nextText = currentUser.ToString();
                    continue;

                case 2:
                    Search(routes, r => $"{r.routeStart.city}-{r.routeEnd.city}");
                    continue;
                    
                case 3:
                    nextText = AddRoute();
                    continue;

                case 4:
                    nextText = BuyTicket(currentUser);
                    continue;

                case 5:
                    nextText = GetList<Ticket>(tickets);
                    continue;

                case 6:
                    EditTrain();
                    continue;

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

    private static string GetList<T>(List<T> list)
    {
        StringBuilder sb = new StringBuilder(list.Count * 20);

        int count = 1;
        foreach(T item in list)
        {
            sb.Append((count) + ". " + item.ToString() + '\n');
            count++;
        }

        return sb.ToString();
    }

    private static void PrintList<T>(List<T> list)
    {
        int count = 1;
        foreach (T item in list)
        {
            Console.WriteLine((count) + ". " + item.ToString());
            count++;
        }
    }

    private static Train AddTrain()
    {
        Train train = new Train();

        Console.Write("Модель поезда: ");
        string model = Console.ReadLine()!;

        Console.WriteLine("""
            Выберите тип поезда:
            1. Грузовой
            2. Пассажирский
            """);
        switch(MakeChoice(1, 2, out int choice))
        {
            case 1:
                train = new Train(TrainType.Cargo, TrainCondition.AtTheDepot, model, trains);
                break;

            case 2:
                train = new Train(TrainType.Passenger, TrainCondition.AtTheDepot, model, trains);
                break;

            default:
                return null;
        }

        return train;
    }

    private static string AddRoute()
    {
        Station routeStart = Search(stations, s => $"{s.country}, {s.city}");
        
        if (routeStart == null)
        {
            Console.Clear();
            Console.Write("Ввведите отправную станцию маршрута:\nВведите страну: ");
            string country1 = Console.ReadLine();
            Console.Write("Введите город: ");
            string city1 = Console.ReadLine();

            routeStart = new Station(country1, city1, stations);

            if(
                CheckForIdentical<Station>(stations, routeStart)
                )
                return "Такая станция уже существует. Выберите её в поиске";

            FileWorker.SerializeToFile<Station>(routeStart, stationsDir + routeStart.city + routeStart.Id + ".xml");

            DefineLists();
        }

        Station routeEnd = Search(stations, s => $"{s.country}, {s.city}");
        if (routeEnd == null)
        {
            Console.Clear();
            Console.Write("Ввведите конечную станцию маршрута:\nВведите страну: ");
            string country2 = Console.ReadLine();
            Console.Write("Введите город: ");
            string city2 = Console.ReadLine();

            routeEnd = new Station(country2, city2, stations);

            if (
                CheckForIdentical<Station>(stations, routeEnd)
                )
                return "Такая станция уже существует. Выберите её в поиске";

            FileWorker.SerializeToFile<Station>(routeEnd, stationsDir + routeEnd.city + routeEnd.Id + ".xml");

            DefineLists();
        }

        Route route = new Route(routeStart, routeEnd, routes);

        if (routeStart.CompareTo(routeEnd) == 0)
            return "Начальная станция не может совпадать с конечной.";

        if (
            CheckForIdentical<Route>(routes, route)
            )
            return "Такой маршрут уже существует.";

        FileWorker.SerializeToFile<Route>(
            route,
            routesDir + route.routeStart.city + "-" + route.routeEnd.city + " " + route.Id + ".xml"
            );

        DefineLists();

        return "Маршрут успешно добавлен";
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
            Console.WriteLine("Введите запрос (ESC для выхода):");
            Console.WriteLine(currentInput);
        }
    }

    private static string AddCarriege(Train train)
    {
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
                    return "Вагон должен находится между начальным и конечным локомотивом";
                }
                train.Add(new CargoCarriege());
                break;

            case 2:
                if (train.Locomotives != 1)
                {
                    return "Вагон должен находится между начальным и конечным локомотивом";
                }
                train.Add(new PassengerCarriege());
                break;

            case 3:
                if (train.Locomotives > 2)
                {
                    return "У поезда не может быть больше двух локомотивов";
                }
                train.Add(new Locomotive());
                break;

            default:
                return "Введите целое число 1-3";
        }

        return "Вагон успешно добавлен";
    }

    private static void DeleteCar(Train train)
    {
        Console.Write("Введите номер вагона для удаления: ");
        try
        {
            MakeChoice(out int choice);
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
        return;
    }

    private static void AddItem(Train train)
    {
        Console.Write("Введите номер вагона для добавления объекта: ");
        try
        {
            MakeChoice(out int choice);
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
        return;
    }

    private static void DefineLists()
    {
        Train train = new Train();
        string[] trainsFiles = Directory.GetFiles(trainsDir);
        string[] routesFiles = Directory.GetFiles(routesDir);
        string[] stationsFiles = Directory.GetFiles(stationsDir);
        string[] usersFiles = Directory.GetFiles(usersDir);
        string[] ticketsFiles = Directory.GetFiles(ticketsDir);

        foreach (string trainFile in trainsFiles)
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
        foreach (string userFile in usersFiles)
        {
            users.Add(
                FileWorker.DeserializeFromFile<Passenger>(userFile)
                );
        }
        foreach (string ticketFile in ticketsFiles)
        {
            tickets.Add(
                FileWorker.DeserializeFromFile<Ticket>(ticketFile)
                );
        }
    }

    private static Train PickTrain()
    {
        Train train = new Train();

        Console.WriteLine($"Выберите один из сохранённых поездов {1}-{trains.Count} или 0 для создания нового:");
        PrintList(trains);

        switch (MakeChoice(0, trains.Count, out int choice))
        {
            case 0:
                train = AddTrain();
                break;

            default:
                train = trains[choice - 1];
                break;
        }

        return train;
    }

    private static bool CheckForIdentical<T>(List<T> items, T obj) where T : IComparable<T>
    {
        bool isIdentical = false;

        foreach (T item in items)
        {
            if(obj.CompareTo(item) == 0)
            {
                isIdentical = true;
                break;
            }
        }

        return isIdentical;

    }

    private static string BuyTicket(Passenger user)
    {
        Route route = Search<Route>(routes, r => $"{r.routeStart.city}-{r.routeEnd.city}")!;

        if (route == null)
            return "Билет не был приобретён.";

        Console.WriteLine(route.ToString());

        Train train = Search<Train>(
            SearchForTrainsWithRoute(
                route.routeStart, route.routeEnd),
            t => $"{t.type}-{t.model}"
            )!;

        if (train == null)
            return "Билет не был приобретён.";

        PassengerCarriege car = Search<PassengerCarriege>(
            train.carrieges.GetRange(
                1, train.carrieges.Count - 2).ConvertAll(c => (PassengerCarriege)c), //////////////
            c => $"{c.GetType()}, свободных мест: {c.GetFreeSeats()}, цена: {route.distance * c.GetTypeCostModifier()}"
            )!;

        if (car == null)
            return "Билет не был приобретён.";

        if (car.GetFreeSeats() <= 0)
            return "В выбранном вагоне нет свободных мест";

        Ticket ticket = new Ticket(
                user,
                route,
                train,
                car,
                (double)route.distance * car.GetTypeCostModifier(),
                car.GetFreeSeatNumber(),
                tickets
                );

        user.AddTicket(ticket);

        FileWorker.SerializeToFile(ticket, ticketsDir + route.routeStart.city + "-" + route.routeEnd.city + ticket.Id + ".xml");

        return "Билет успешно приобретён.";
    }

    private static List<Train> SearchForTrainsWithRoute(Station routeStart, Station routeEnd)
    {
        List<Train> sTrains = new();

        foreach (Train train in trains)
        {
            if(train.route.routeStart.CompareTo(routeStart) == 0 
                && train.route.routeEnd.CompareTo(routeEnd) == 0
                && train.type == TrainType.Passenger)
            {
                sTrains.Add(train);
            }
        }

        return sTrains;
    }

    private static void EditTrain()
    {
        string nextText = "";

        Console.Clear();
        PrintList<Train>(trains);
        Train train = trains[
            MakeChoice(1, trains.Count, out int choice) - 1
            ];

        while (true)
        {
            Console.Clear();
            Console.WriteLine(nextText + '\n');
            Console.WriteLine("""
                Выберите действие в с поездом:
                1. Изменить тип
                2. Изменить маршрут
                3. Изменить состояние
                4. Добавить вагон
                5. Удалить вагон
                6. Вывести поезд
                0. Выход
                """);

            switch(MakeChoice(out choice))
            {
                case 1:
                    nextText = SetTrainType(train);
                    continue;

                case 2:
                    SetTrainRoute(train);
                    nextText = "Маршрут поезда успешно установлен.";
                    continue;

                case 3:
                    SetTrainCondition(train);
                    nextText = "Состояние поезда успешно изменёно.";
                    continue;

                case 4:
                    nextText = AddCarriege(train);
                    continue;

                case 5:
                    DeleteCar(train);
                    continue;

                case 6:
                    nextText = train.ToString();
                    continue;

                case 0:
                    break;

                default:
                    nextText = "Введите целое число 0-3";
                    break;
            }

            if (choice == 0) break;
        }
    }

    private static string SetTrainType(Train train)
    {
        Console.WriteLine("""
            Выберите новый тип для поезда:
            1. Грузовой
            2. Пассажирский
            """);

        string res = train.SetType(
            (MakeChoice(1, 2, out int choice) == 1 ? TrainType.Cargo : TrainType.Passenger)
            );

        FileWorker.SerializeToFile(train, trainsDir + train.model + train.Id + ".xml");

        return res;
    }

    private static void SetTrainRoute(Train train)
    {
        Route route = Search<Route>(routes, r => $"{r.routeStart.city}-{r.routeEnd.city}")!;

        if (route == null) return;

        train.SetRoute(route);

        FileWorker.SerializeToFile(train, trainsDir + train.model + train.Id + ".xml");
    }

    private static void SetTrainCondition(Train train)
    {
        Console.WriteLine("""
            Выберите новый тип для поезда:
            1. Работает
            2. На обслуживании
            3. В депо
            """);

        TrainCondition condition = new TrainCondition();

        switch(MakeChoice(1, 3, out int choice))
        {
            case 1:
                condition = TrainCondition.Running;
                break;

            case 2:
                condition = TrainCondition.OnMaintenance;
                break;

            case 3:
                condition = TrainCondition.AtTheDepot;
                break;
        }

        train.SetCondition(condition);

        FileWorker.SerializeToFile(train, trainsDir + train.model + train.Id + ".xml");
    }
}