public static class UserLogic
{

    /// <summary>
    /// Asks the user to select an option among Edit user and Exit to main menu.
    /// </summary>
    public static void User(){
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Add user", ()=>{
                    UserAccess u = new UserAccess();
                    User? cred = UserMenu.AddNewUser();
                    if (cred == null)
                    {
                        return;
                    }
                    bool added = u.AddUser(cred);
                    UserMenu.NotifyAddUser(added);
                }},
                {"2. Edit user", ()=>{
                    // when choosing to edit a user ask which user to edit etc
                    UserTable.EditUsers();
                }},
                {"3. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }

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