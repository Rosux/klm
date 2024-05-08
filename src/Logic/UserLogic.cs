public static class UserLogic
{
    public static void Login()
    {
        UserAccess u = new UserAccess();
        User Credentials = UserMenu.Login();
        User? user = u.VerifyUser(Credentials.Email);
        if(user != null && BCrypt.Net.BCrypt.EnhancedVerify(Credentials.Password, user.Password)){
            Program.CurrentUser = user;
            return;
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
        bool added = u.AddUser(Credentials);
        UserMenu.NotifyAddUser(added);
    }
}