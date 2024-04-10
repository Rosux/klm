public static class UserMenu{
    private static UserAccess u = new UserAccess();

    public static User Register()
    {
        bool exit = false;
        Console.WriteLine("Enter your first name:");
        string firstName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(firstName))
        {
            Console.WriteLine("First name cannot be empty. Please try again.");
        }
        Console.WriteLine("Enter your last name:");
        string lastName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Last name cannot be empty. Please try again.");
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
        } while (!IsValidEmail(email));
        Console.WriteLine("Enter your password (totally secured btw):");
        string password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty. Please try again.");
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
        bool exit = false;
        Console.WriteLine("Enter user first name:");
        string firstName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(firstName))
        {
            Console.WriteLine("First name cannot be empty. Please try again.");
        }
        Console.WriteLine("Enter user last name:");
        string lastName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Last name cannot be empty. Please try again.");
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
        } while (!IsValidEmail(email));
        Console.WriteLine("Enter user password (totally secured btw):");
        string password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty. Please try again.");
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

    public static void NoUsersToRemove(){
        Console.Clear();
        Console.WriteLine("There are no Users stored to remove.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void UserRemoved(){
        Console.Clear();
        Console.WriteLine("the user has been removed.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void UserAdded(){
        Console.Clear();
        Console.WriteLine("the user has been added.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    
}