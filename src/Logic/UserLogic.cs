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
                {"2. Delete user", ()=>{
                    UserAccess u = new UserAccess();
                    List<User> users = u.GetAllUsers();
                    Dictionary<string, User> userDict = users.ToDictionary(user => user.Email);
                    User selectedUser = MenuHelper.SelectFromList("select user to delete", true, userDict);
                    if (selectedUser != null)
                    {
                        bool delete = false;
                        Console.Clear();
                        MenuHelper.SelectOptions("Are u sure?", new Dictionary<string, Action>(){
                            {"1. yes", ()=>{
                                delete = true;
                            }},
                            {"2. no", ()=>{
                                delete = false;
                            }},
                        });
                        if (delete)
                        {
                            u.DeleteUser(selectedUser);
                            UserMenu.UserRemoved();
                        }else
                        {
                            UserMenu.NotSure();
                        }
                    }
                }},
                {"3. Edit user", ()=>{
                    // when choosing to edit a user ask which user to edit etc
                    UserTable.EditUsers();
                }},
                {"4. Exit to main menu", ()=>{
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