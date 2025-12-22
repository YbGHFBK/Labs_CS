using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

public static class DB
{
    private static List<User> users = new();

    static string mainDir = "../../../DB/";
    static string trainsDir = mainDir + "Trains/";
    static string routesDir = mainDir + "Routes/";
    static string stationsDir = mainDir + "Stations/";
    static string usersDir = mainDir + "Users/";
    static string ticketsDir = mainDir + "Tickets/";

    public static void Initialize()
    {
        //string[] trainsFiles = Directory.GetFiles(trainsDir);
        //string[] routesFiles = Directory.GetFiles(routesDir);
        //string[] stationsFiles = Directory.GetFiles(stationsDir);
        string[] usersFiles = Directory.GetFiles(usersDir);
        //string[] ticketsFiles = Directory.GetFiles(ticketsDir);

        //foreach (string trainFile in trainsFiles)
        //{
        //    trains.Add(
        //        FileWorker.DeserializeFromFile<Train>(trainFile)
        //        );
        //}
        //foreach (string routeFile in routesFiles)
        //{
        //    routes.Add(
        //        FileWorker.DeserializeFromFile<Route>(routeFile)
        //        );
        //}
        //foreach (string stationFile in stationsFiles)
        //{
        //    stations.Add(
        //        FileWorker.DeserializeFromFile<Station>(stationFile)
        //        );
        //}
        foreach (string userFile in usersFiles)
        {
            users.Add(
                FileWorker.DeserializeFromFile<User>(userFile)
                );
        }
        //foreach (string ticketFile in ticketsFiles)
        //{
        //    tickets.Add(
        //        FileWorker.DeserializeFromFile<Ticket>(ticketFile)
        //        );
        //}

        Console.WriteLine("Database Initialized");
    }

    public static User FindByName(string name)
    {
        foreach (User user in users)
        {
            if (user.name.Equals(name))
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
}