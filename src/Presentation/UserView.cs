public static class UserView{
    public static void Register()
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
        // return user;
        Console.WriteLine("Your information has been saved. Proceed to login.");
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
}