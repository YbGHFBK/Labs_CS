public static class AuthService
{
    private static User currentUser = null!;

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

        if (string.IsNullOrEmpty(login))
            return RegResult.InvalidNameFormat;

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repPassword))
            return RegResult.InvalidPasswordFormat;

        if (user != null)
            return RegResult.NameTaken;

        if (password != repPassword)
            return RegResult.PasswordMismatch;

        currentUser = DB.createUser(login, password);
        return RegResult.Success;
    }

    public static bool IsAdmin()
    {
        if (currentUser != null)
            if (currentUser.role == UserRole.User) 
                return false;
        return true;
    }
}