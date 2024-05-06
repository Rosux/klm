using System.Text.RegularExpressions;
public static class UserMenu{
    private static UserAccess u = new UserAccess();

    public static User Register()
    {
        string firstName = GetValidInput("Enter your first name:", 3, 20);
        string lastName = GetValidInput("Enter your last name:", 3, 20);

        string email;
            Console.WriteLine("Enter your email:");
            email = GetValidEmail("Enter your email:", 3, 32);
        string password;
        do
        {
            password = GetValidInput("Enter your password (totally secured btw):", 6, 20);
            if (!IsStrongPassword(password))
            {
                Console.WriteLine("Password must contain at least 1 lowercase letter, 1 uppercase letter, 1 special character, and 1 number. Please try again.");
                continue;
            }
        } while (!IsStrongPassword(password));

        User user = new User(firstName, lastName, email, password);
        Console.WriteLine("User has been added");
        return user;
    }

    private static string GetValidInput(string prompt, int minLength, int maxLength)
    {
        string input = "";
        while (true)
        {
            Console.WriteLine(prompt);
            string currentInput = "";
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (currentInput.Length > 0)
                    {
                        currentInput = currentInput.Substring(0, currentInput.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    currentInput += keyInfo.KeyChar;
                    Console.Write(keyInfo.KeyChar);
                }
            }

            if (string.IsNullOrWhiteSpace(currentInput))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
                continue;
            }
            if (currentInput.Length < minLength || currentInput.Length > maxLength)
            {
                Console.WriteLine($"Input must be between {minLength} and {maxLength} characters. Please try again.");
                continue;
            }
            input = currentInput;
            break;
        }
        return input;
    }

    private static string GetValidEmail(string prompt, int minLength, int maxLength)
    {
        string input = "";
        while (true)
        {
            Console.Clear();
            Console.Write(prompt);
            string currentInput = "";
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter && !string.IsNullOrWhiteSpace(currentInput))
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (currentInput.Length > 0)
                    {
                        currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    }
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    currentInput += keyInfo.KeyChar;
                    Console.Write(keyInfo.KeyChar);
                }
            }

            if (string.IsNullOrWhiteSpace(currentInput))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
                continue;
            }
            if (currentInput.Length < minLength || currentInput.Length > maxLength)
            {
                Console.WriteLine($"Input must be between {minLength} and {maxLength} characters. Please try again.");
                continue;
            }
            input = currentInput;
            break;
        }
        return input;
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

    private static bool IsStrongPassword(string password)
    {
        var hasLowercase = new Regex(@"[a-z]").IsMatch(password);
        var hasUppercase = new Regex(@"[A-Z]").IsMatch(password);
        var hasSpecialChar = new Regex(@"\W").IsMatch(password);
        var hasNumber = new Regex(@"\d").IsMatch(password);

        return hasLowercase && hasUppercase && hasSpecialChar && hasNumber;
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

    public static User? EditUser(User USR){
        User editedUser = USR;
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
                    Console.WriteLine("Enter the new Email of the User:");
                    editedUser.Email = Console.ReadLine();
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
    
}