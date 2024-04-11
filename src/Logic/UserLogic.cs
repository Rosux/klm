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
            UserMenu.WrongLogin();
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
    public static void Addusers()
    {
        UserAccess u = new UserAccess();
        User Credentials = UserMenu.AddNewUser();
        u.AddUser(Credentials);
    }

    private static UserAccess c = new UserAccess();
    public static void User(){
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Remove User", ()=>{
                    //Check if there is a item to remove and if there is it send a message to get it removed.
                    User user =  UserMenu.RemoveUser();
                    if (user == null)
                    {
                        UserMenu.NoUsersToRemove();
                        return;
                    }
                    bool removed = c.DeleteUser(user);
                    UserMenu.UserRemoved();
                }},
                {"2. Add User", ()=>{
                    //Check if there is a item to remove and if there is it send a message to get it removed.
                    Addusers();
                    UserMenu.UserAdded();
                }},
                {"2. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }

}