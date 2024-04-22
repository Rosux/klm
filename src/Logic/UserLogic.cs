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
                    Addusers();
                    UserMenu.UserAdded();
                }},
                {"3. Edit User", ()=>{
                    User editedUser = UserMenu.EditUser();
                    if (editedUser == null){
                        UserMenu.NoUsersToRemove();
                        return;
                    }
                    bool updated = c.UpdateUser(editedUser);
                    if (updated){
                        UserMenu.UserRemoved();
                    }else{
                        UserMenu.Error();
                    }
                }},
                {"4. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }

}