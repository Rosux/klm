public static class UserLogic
{
    public static void Login()
    {
        UserAccess u = new UserAccess();
        User? Credentials = UserMenu.Login();
        if(Credentials == null){
            Program.CurrentUser = null;
            return;
        }
        User? user = u.VerifyUser(Credentials.Email);
        if(user != null && BCrypt.Net.BCrypt.EnhancedVerify(Credentials.Password, user.Password)){
            Program.CurrentUser = user;
        }else{
            Program.CurrentUser = null;
            UserMenu.WrongLogin();
        }
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