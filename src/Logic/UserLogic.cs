public static class UserLogic
{
    public static void Login()
    {
        UserAccess u = new UserAccess();
        User Credentials = UserMenu.Login();
        User LoggedUser = u.CheckUser(Credentials);
        if (LoggedUser == null)
        {
            Program.CurrentUser = null;
        }else{
            Program.CurrentUser = LoggedUser;
        }
    }
    public static void Register()
    {
        UserAccess u = new UserAccess();
        User Credentials = UserMenu.Register();
        u.AddUser(Credentials);
    }
}