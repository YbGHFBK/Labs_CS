public static class Logger
{
    public static Passenger AuthUser(List<Passenger> users)
    {
        Console.WriteLine("Перед началом работы войдите в аккаунт:");
        Console.Write("Введите логин: ");
        string login = Console.ReadLine()!;

        Passenger pas = new Passenger();

        bool isExist = false;
        foreach (Passenger user in users)
        {
            if (user.login == login)
            {
                pas = user;
                isExist = true;
                break;
            }
        }
        if (!isExist)
        {
            return RegisterUser(login, users);
        }

        Console.Write("Введите пароль: ");
        string password = Console.ReadLine()!;

        if (pas.password == password)
        {
            Console.WriteLine("Вы успешно вошли в аккаунт.\n");
            return pas;
        }
        else
        {
            Console.WriteLine("Неврный пароль.");
            return null;
        }
    }

    private static Passenger RegisterUser(string login, List<Passenger> users)
    {
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine()!;

        return new Passenger(login, password, users);
    }

    public static bool IsAdmin(Passenger user)
    {
        return (user.role == UserRole.Admin);
    }
}