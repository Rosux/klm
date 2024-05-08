using System.Net.Mail;
using System.Text.RegularExpressions;
using BCrypt.Net;
public static class UserMenu{
    private static UserAccess u = new UserAccess();

    public static User? Register()
    {
        string? firstName = GetValidInput("Enter your first name:", 3, 20);
        if (firstName == null)
        {
            return null;
        }
        string? lastName = GetValidInput("Enter your last name:", 3, 20);
        if (lastName == null)
        {
            return null;
        }
        string? email = GetValideEmail("Enter your email:", 3, 20);
        if (email == null)
        {
            return null;
        }
        string passwordHash = "";
        string? password = GetValidPassword("Enter your password:", 6, 20);
        if (password == null)
        {
            return null;
        }
        string? password2 = GetValidPassword("Enter your password again:", 6, 20, true, password);
        if (password2 == null)
        {
            return null;
        }
        passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        User user = new User(firstName, lastName, email, passwordHash);
        return user;
    }

    public static void NotifyAddUser(bool x)
    {
        if(x)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User has been added. Press any key to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey(true);
        }else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("User has not been added. Press any key to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey(true);
        }
    }
    /// <summary>
    /// This checks allot of statements that need to be true before you can press enter and go to the next valid input.
    /// </summary>
    /// <param name="prompt">Here you can give the promt that should be printed at the top before you get the readkey.</param>
    /// <param name="minLength">Here you give how long the minimum lenght must be for the input.</param>
    /// <param name="maxLength">Here you give how long the maximum lenght must be for the input.</param>
    /// <returns>Return the user given string.</returns>
    private static string? GetValidInput(string prompt, int minLength, int maxLength)
    {
        string input = "";
        string error= "";
        do
        {
            Console.Clear();
            Console.Write($"{prompt}\n{input}\n\nPress enter to confirm. Press escape to cancel.\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
            error = "";
            ConsoleKeyInfo keyInfo;
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Enter && Regex.IsMatch(input, "[A-z]") && input.Length <= maxLength && input.Length >= minLength)
            {
                break;
            }
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                }
            }
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return null;
            }
            if (char.IsLetter(keyInfo.KeyChar))
            {
                input += keyInfo.KeyChar;
            }
            if (input.Length < minLength || input.Length > maxLength)
            {
                error += $"Input must be between {minLength} and {maxLength} characters. Please try again.\n";
            }
            if (!Regex.IsMatch(input, "[A-z]"))
            {
                error += $"Input must only contain letters.\n";
            }
        }while(true);
        Console.Clear();
        return input;
    }

    private static string GetValideEmail(string prompt, int minLength, int maxLength)
    {
        string input = "";
        string error= "";
        do
        {
            error = "";
            User? user = u.VerifyUser(input);
            if (user != null)
            {
                error += $"Email already exists.\n";
            }
            Console.Clear();
            Console.Write($"{prompt}\n{input}\n\nPress enter to confirm. Press escape to cancel.\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo keyInfo;
            MailAddress b;
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return null;
            }
            if (!char.IsControl(keyInfo.KeyChar))
            {
                input += keyInfo.KeyChar;
            }
            if (!MailAddress.TryCreate(input, out b))
            {
                error += $"email is not valid";
            }
            if (keyInfo.Key == ConsoleKey.Enter && input.Length <= maxLength && input.Length >= minLength && MailAddress.TryCreate(input, out b))
            {
                if (user == null)
                {
                    break;
                }
            }
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                }
            }
            if (input.Length < minLength || input.Length > maxLength)
            {
                error += $"Input must be between {minLength} and {maxLength} characters. Please try again.\n";
            }
            if (!Regex.IsMatch(input, "[A-z]"))
            {
                error += $"Input must only contain letters.\n";
            }
        }while(true);
        Console.Clear();
        return input;
    }
    /// <summary>
    /// This is the same as the above but a little different because you need to use this twice to check if both passwords are the same.
    /// </summary>
    /// <param name="prompt">Here you type the promt that should be written above the readkey.</param>
    /// <param name="minLength">Here you give how long the minimum lenght must be for the input.</param>
    /// <param name="maxLength">Here you give how long the maximum lenght must be for the input.</param>
    /// <param name="secondPass">Her you give a true if its the second password your providing or false if its the first password.</param>
    /// <param name="firstPassword">Here you give the first password if secondPass is true.</param>
    /// <returns>Return the user given string.</returns>
    private static string? GetValidPassword(string prompt, int minLength, int maxLength, bool secondPass = false, string firstPassword = "")
    {
        string input = "";
        string error= "";
        do
        {
            Console.Clear();
            Console.Write($"{prompt}\n{input}\n\nPress enter to confirm. Press escape to cancel.\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
            error = "";
            ConsoleKeyInfo keyInfo;
            keyInfo = Console.ReadKey(true);
            if (!char.IsControl(keyInfo.KeyChar))
            {
                input += keyInfo.KeyChar;
            }
            if (keyInfo.Key == ConsoleKey.Enter && Regex.IsMatch(input, "[A-z]") && input.Length <= maxLength && input.Length >= minLength && IsStrongPassword(input))
            {
                if (secondPass && firstPassword == input)
                {
                    break;
                }else if(!secondPass)
                {
                    break;
                }
            }
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                }
            }
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return null;
            }
            if (input != firstPassword && secondPass)
            {
                error += "passwords do not match\n";
            }
            if (!IsStrongPassword(input))
            {
                error += $"Password must contain at least 1 lowercase letter, 1 uppercase letter, 1 special character, and 1 number. Please try again.\n";
            }
            if (input.Length < minLength || input.Length > maxLength)
            {
                error += $"Input must be between {minLength} and {maxLength} characters. Please try again.\n";
            }
            if (!Regex.IsMatch(input, "[A-z]"))
            {
                error += $"Input must only contain letters.\n";
            }
        }while(true);
        Console.Clear();
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
