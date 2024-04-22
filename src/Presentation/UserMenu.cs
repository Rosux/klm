public static class UserMenu{
    private static UserAccess u = new UserAccess();

    public static User Register()
    {
        string firstName= "";
        string lastName= "";
        string password = "";
        while(true){
            Console.WriteLine("Enter your first name:");
            firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("First name cannot be empty. Please try again.");
                continue;
            }
            break;
        }
        while(true){
            Console.WriteLine("Enter your last name:");
            lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Last name cannot be empty. Please try again.");
                continue;
            }
            break;
        }
        string email;
        do
        {
            Console.WriteLine("Enter your email:");
            email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Email cannot be empty. Please try again.");
                continue;
            }
        } 
        while (!IsValidEmail(email));
        while(true){
            Console.WriteLine("Enter your password (totally secured btw):");
            password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty. Please try again.");
                continue;
            }
            break;
        }
        User user = new User(firstName, lastName, email, password);
        Console.WriteLine("User has been added");
        return user;
    }

    public static User Login()
    {
        Console.WriteLine("Enter your email:");
        string LoginEmail = Console.ReadLine();
        Console.WriteLine("Enter your password:");
        string LoginPassword = Console.ReadLine();
        User Login = new User(null, null, LoginEmail, LoginPassword);
        return Login;
    }

    static bool IsValidEmail(string email)
    {
        // Basic email format validation
        if (email.Contains("@") && (email.EndsWith(".com") || email.EndsWith(".nl")))
        {
            return true;
        }
        else
        {
            Console.WriteLine("Invalid email format. Please enter a valid email address.");
            return false;
        }
    }

    public static User AddNewUser()
    {
        string firstName = "";
        string lastName = "";
        string password = "";
        while (true){
            Console.WriteLine("Enter user first name:");
            firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("First name cannot be empty. Please try again.");
                continue;
            }
            break;
        }

        while (true){
            Console.WriteLine("Enter user last name:");
            lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Last name cannot be empty. Please try again.");
                continue;
            }
            break;
        }
            string email;
            do
            {
                Console.WriteLine("Enter user email:");
                email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Email cannot be empty. Please try again.");
                    continue;
                }
            } 
            while (!IsValidEmail(email));
                Console.WriteLine("Enter user password (totally secured btw):");
                while(true){
                password = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password cannot be empty. Please try again.");
                    continue;
                }
                break;
            }
        User user = new User(firstName, lastName, email, password);
        Console.WriteLine("User has been added");
        return user;
    }

    public static User RemoveUser(){
        List<User> users = u.GetAllUsers();
        if (users.Count == 0) {
            Console.WriteLine("There are no Users available to remove.");
            return null;
        }else {
        Dictionary<string, User> d = new Dictionary<string, User>();
        foreach (User user in u.GetAllUsers()){
            d.Add(user.FirstName, user);
        }
        return MenuHelper.SelectFromList("Select id to delete", d);
        }
    }

    public static User? EditUser(){
        Dictionary<string, User> cons = new Dictionary<string, User>();
        foreach (User user in u.GetAllUsers()){
            cons.Add(user.FirstName, user);
        }
        if (cons.Count == 0)
        {
            return null;
        }
        User editedUser = MenuHelper.SelectFromList("Select user to edit", cons);
        bool changing = true;
        while (changing)
        {
            MenuHelper.SelectOptions("Select User property to edit", new Dictionary<string, Action>(){
                {"Name", ()=>{
                    Console.WriteLine("Enter the new name of the User:");
                    editedUser.FirstName = Console.ReadLine();
                }},
                {"LastName", ()=>{
                    Console.WriteLine("Enter the new LastName of the User:");
                    editedUser.LastName = Console.ReadLine();
                }},
                {"Email", ()=>{
                    string email;
                    do
                    {
                        Console.WriteLine("Enter the new Email of the User:");
                        email = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(email))
                        {
                            Console.WriteLine("Email cannot be empty. Please try again.");
                            continue;
                        }
                    } 
                    while (!IsValidEmail(email));
                    editedUser.Email = email;
                }},
                {"Password", ()=>{
                    Console.WriteLine("Enter the new Password of the User:");
                    editedUser.Password = Console.ReadLine();
                }},
                {"Role", ()=>{
                    bool running = true;
                    while(running)
                    {
                        MenuHelper.SelectOptions("Choose what to change the Role to", new Dictionary<string, Action>(){
                            {"1. User", ()=>{
                                editedUser.Role = UserRole.USER;
                                running = false;
                            }},
                            {"2. Admin", ()=>{
                                editedUser.Role = UserRole.ADMIN;
                                running = false;
                            }},
                        });
                    }
                }},
                {"Save changes", ()=>{
                    changing = false;
                }},
            });
        }
        
        return editedUser;
    }

    public static void NoUsersToRemove(){
        Console.Clear();
        Console.WriteLine("There are no Users stored to remove.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void UserRemoved(){
        Console.Clear();
        Console.WriteLine("The user has been removed.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void UserAdded(){
        Console.Clear();
        Console.WriteLine("The user has been added.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void WrongLogin(){
        Console.Clear();
        Console.WriteLine("Email or password was wrong.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void Error(){
        Console.Clear();
        Console.WriteLine("An error occured. Please try again later.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    
}