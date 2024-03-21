public class UserLogic
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
}