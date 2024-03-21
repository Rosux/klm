public static class UserLogic
{
    public static void Login()
    {
        UserAccess u = new UserAccess();
        User Credentials = UserMenu.Login();
        User LoggedUser = u.CheckUser(Credentials);
        if (LoggedUser == null)
        {
            Console.WriteLine("not logged in bozo");
        }else{
            Program.CurrentUser = LoggedUser;
            Console.WriteLine(Program.CurrentUser.FirstName);
        }
    }
    public static void Register()
    {
        UserAccess u = new UserAccess();
        User Credentials = UserMenu.Register();
        u.AddUser(Credentials);
    }
}