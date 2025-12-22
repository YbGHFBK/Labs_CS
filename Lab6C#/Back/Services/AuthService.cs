public static class AuthService
{
    private static User currentUser = null!;

    //public static Passenger AuthUser(List<Passenger> users)
    //{
    //    Console.WriteLine("Перед началом работы войдите в аккаунт:");
    //    Console.Write("Введите логин: ");
    //    string login = Console.ReadLine()!;

    //    Passenger pas = new Passenger();

    //    bool isExist = false;
    //    foreach (Passenger user in users)
    //    {
    //        if (user.login == login)
    //        {
    //            pas = user;
    //            isExist = true;
    //            break;
    //        }
    //    }
    //    if (!isExist)
    //    {
    //        return RegisterUser(login, users);
    //    }

    //    Console.Write("Введите пароль: ");
    //    string password = Console.ReadLine()!;

    //    if (pas.password == password)
    //    {
    //        Console.WriteLine("Вы успешно вошли в аккаунт.\n");
    //        return pas;
    //    }
    //    else
    //    {
    //        Console.WriteLine("Неврный пароль.");
    //        return null!;
    //    }
    //}

    //private static Passenger RegisterUser(string login, List<Passenger> users)
    //{
    //    Console.Write("Введите пароль: ");
    //    string password = Console.ReadLine()!;

    //    return new Passenger(login, password, UserRole.User, users);
    //}

    //public static bool IsAdmin(Passenger user)
    //{
    //    return (user.role == UserRole.Admin);
    //}

    //public static int RoleTo(Passenger user)
    //{
    //    if (user.role == UserRole.Admin) return 1;
    //    else if (user.role == UserRole.User) return 0;
    //    else return -1;
    //}

    public static LoginResult LoginUser(string login, string password)
    {
        User user = DB.FindByName(login);

        if (user == null)
            return LoginResult.UserNotFound;

        if (user.password != password)
            return LoginResult.InvalidPassword;

        currentUser = user;
        return LoginResult.Success;
    }

    public static RegResult RegUser(string login, string password, string repPassword)
    {
        User user = DB.FindByName(login);

        if (user != null)
            return RegResult.NameTaken;

        if (password != repPassword)
            return RegResult.PasswordMismatch;

        currentUser = DB.createUser(login, password);
        return RegResult.Success;
    }
}