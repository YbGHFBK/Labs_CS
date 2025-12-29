using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

public static class DB
{
    public static List<Train> trains = new();
    public static List<Route> routes = new();
    public static List<Station> stations = new();
    public static List<User> users = new();
    public static List<Ticket> tickets = new();
    public static List<Schedule> schedules = new();

    static string mainDir = "../../../DB/";
    static string trainsDir = mainDir + "Trains/";
    static string routesDir = mainDir + "Routes/";
    static string stationsDir = mainDir + "Stations/";
    static string usersDir = mainDir + "Users/";
    static string ticketsDir = mainDir + "Tickets/";
    static string schedulesDir = mainDir + "Schedules/";

    public static void Initialize()
    {
        string[] trainsFiles = Directory.GetFiles(trainsDir);
        string[] routesFiles = Directory.GetFiles(routesDir);
        string[] stationsFiles = Directory.GetFiles(stationsDir);
        string[] usersFiles = Directory.GetFiles(usersDir);
        string[] ticketsFiles = Directory.GetFiles(ticketsDir);
        string[] schedulesFiles = Directory.GetFiles(schedulesDir);

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
                FileWorker.DeserializeFromFile<User>(userFile)
                );
        }
        foreach (string ticketFile in ticketsFiles)
        {
            tickets.Add(
                FileWorker.DeserializeFromFile<Ticket>(ticketFile)
                );
        }
        foreach (string scheduleFile in schedulesFiles)
        {
            schedules.Add(
                FileWorker.DeserializeFromFile<Schedule>(scheduleFile)
                );
        }

        Console.WriteLine("Database Initialized");
    }

    public static User FindByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null!;
        foreach (User user in users)
        {
            if (!string.IsNullOrEmpty(user.name) && user.name.Equals(name))
                return user;
        }

        return null!;
    }

    public static User createUser(string name, string password)
    {
        User user = new User(name, password, UserRole.User, users);

        users.Add(user);

        FileWorker.SerializeToFile(user, usersDir + user.name + user.Id + ".xml");

        return user;
    }

    public static List<Station> GetAllStations()
    {
        return stations;
    }

    public static List<Route> GetRoutesByStations(Station st1, Station st2)
    {
        List<Route> nRoutes = new();

        foreach (Route route in routes)
        {
            if (route.routeStart == st1 && route.routeEnd == st2)
                nRoutes.Add(route);
        }

        return nRoutes;
    }

    public static int GetNextId(Type type)
    {
        if (type == typeof(Train))
            return IdGenerator.GetNextId(trains);
        if (type == typeof(Station))
            return IdGenerator.GetNextId(stations);
        if (type == typeof(Route))
            return IdGenerator.GetNextId(routes);
        if (type == typeof(Ticket))
            return IdGenerator.GetNextId(tickets);
        if (type == typeof(User))
            return IdGenerator.GetNextId(users);
        if (type == typeof(Schedule))
            return IdGenerator.GetNextId(schedules);

        return -1;
    }

    public static Schedule createSchedule(Route r, Train t, TimeOnly dep, TimeOnly ar)
    {
        Schedule sc = new Schedule();
        sc.ArrivalDate = ar;
        sc.DepartureDate = dep;
        sc.RouteId = r.Id;
        sc.TrainId = t.Id;
        
        schedules.Add(sc);

        FileWorker.SerializeToFile(sc, schedulesDir + sc.RouteId + sc.Id + ".xml");

        return sc;
    }

    public static Station createStation(string country, string city)
    {
        Station st = new Station();
        st.country = country;
        st.city = city;

        stations.Add(st);

        FileWorker.SerializeToFile(st, stationsDir + st.ToString() + st.Id + ".xml");

        return st;
    }

    public static Route createRoute(Station st1, Station st2, int dist)
    {
        Route r = new Route();
        r.routeStartId = st1.Id;
        r.routeEndId = st2.Id;
        r.routeStart = st1;
        r.routeEnd = st2;
        r.distance = dist;

        routes.Add(r);

        FileWorker.SerializeToFile(r, routesDir + r.ToString() + r.Id + ".xml");

        return r;
    }

    public static Train createTrain(string model, TrainType tt, TrainCondition tc, int mileage)
    {
        Train t = new Train();
        
        t.model = model;
        t.type = tt;
        t.condition = tc;
        t.mileage = mileage;
    
        trains.Add(t);

        FileWorker.SerializeToFile(t, trainsDir + t.ToString() + t.Id + ".xml");

        return t;
    }

    public static T GetById<T>(int id) where T : IHasId
    {
        if (typeof(T) == typeof(Train))
            foreach (Train train in trains)
            {
                if (id == train.Id)
                    return (T)(object)train;
            }
        if (typeof(T) == typeof(Station))
            foreach (Station station in stations)
            {
                if (id == station.Id)
                    return (T)(object)station;
            }
        if (typeof(T) == typeof(Route))
            foreach (Route route in routes)
            {
                if (id == route.Id)
                    return (T)(object)route;
            }
        if (typeof(T) == typeof(Ticket))
            foreach (Ticket ticket in tickets)
            {
                if (id == ticket.Id)
                    return (T)(object)ticket;
            }
        if (typeof(T) == typeof(User))
            foreach (User user in users)
            {
                if (id == user.Id)
                    return (T)(object)user;
            }


        foreach (Schedule schedule in schedules)
        {
            if (id == schedule.Id)
                return (T)(object)schedule;
        }

        return default!;
    }
    
    public static List<ScheduleTicketInfo> SearchSchedules(Station st1, Station st2, PassengerCarriegeType type)
    {
        List<ScheduleTicketInfo> list = new();

        foreach (Schedule schedule in schedules)
        {
            Route route = GetById<Route>(schedule.RouteId);
            if (route.routeStart.CompareTo(st1) == 0 && route.routeEnd.CompareTo(st2) == 0)
            {
                Train train = GetById<Train>(schedule.TrainId);
                if (type == PassengerCarriegeType.AllClasses)
                {
                    for (int i = 0; i < train.carrieges.Count; i++)
                    {
                        list.Add(new ScheduleTicketInfo(schedule, i, 0.00));
                    }
                }
                else
                {
                    for (int i = 0; i < train.carrieges.Count; i++)
                    {
                        if (train.carrieges[i] is PassengerCarriege car)
                        if (car.type == type)
                        list.Add(new ScheduleTicketInfo(schedule, i, 0.00));
                    }
                }
            }
        }

        return list;
    }

    public static void Save()
    {
        foreach (Train t in trains)
        {
            FileWorker.SerializeToFile(t, trainsDir + t.ToString() + t.Id + ".xml");
        }
        foreach (Station st in stations)
        {
            FileWorker.SerializeToFile(st, stationsDir + st.ToString() + st.Id + ".xml");
        }
        foreach (Route r in routes)
        {
            FileWorker.SerializeToFile(r, routesDir + r.ToString() + r.Id + ".xml");
        }
    }

    public static void DeleteStation(Station station)
    {
        string filePath = stationsDir + station.ToString() + station.Id + ".xml";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            stations.Remove(station);
        }
        else
        {
            MessageBox.Show("Файл не найден по пути: " + filePath);
        }
    }

    public static void DeleteRoute(Route route)
    {
        string filePath = routesDir + route.ToString() + route.Id + ".xml";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            routes.Remove(route);
        }
        else
        {
            MessageBox.Show("Файл не найден по пути: " + filePath);
        }
    }

    public static void DeleteTrain(Train t)
    {
        string filePath = trainsDir + t.ToString() + t.Id + ".xml";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            trains.Remove(t);
        }
        else
        {
            MessageBox.Show("Файл не найден по пути: " + filePath);
        }
    }
}