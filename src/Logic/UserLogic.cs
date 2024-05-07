public static class UserLogic
{
    public static void Login()
    {
        UserAccess u = new UserAccess();
        User Credentials = UserMenu.Login();
        List<User> allUsers = u.GetAllUsers(); 
        foreach (User k in allUsers)
        {
            bool found = BCrypt.Net.BCrypt.EnhancedVerify(Credentials.Password, k.Password);
            if (found)
            {
                Program.CurrentUser = k;
                return;
            }
        }
        Program.CurrentUser = null;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Wrong email or password. Press any key to continue");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadLine();
        return;
    }
    public static void Register()
    {
        UserAccess u = new UserAccess();
        User Credentials = UserMenu.Register();
        if (Credentials == null)
        {
            return;
        }
        u.AddUser(Credentials);
    }
}