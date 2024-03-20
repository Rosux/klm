using System;

// VIEW (handle user input/output here as much as you can such as asking a user to select a time and recieving that input and sending it to the controller)
public static class Menu{
    /// <summary>
    /// Shows a select time to user as: 00:00 and lets the user select a specific time in HH:MM format.
    /// </summary>
    /// <returns>A TimeOnly object containing the user selected time.</returns>
    
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
                    User user = new User(firstName, lastName, email, password, "User");
                    // return user;
                    Console.WriteLine("Your information has been saved. Proceed to login.");
                }

    public static User Login()
    {
        Console.WriteLine("Enter your email:");
        string LoginEmail = Console.ReadLine();
        Console.WriteLine("Enter your password:");
        string LoginPassword = Console.ReadLine();
        User Login = new User(null, null, LoginEmail, LoginPassword, null);
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

    public static void Options(string Header, Dictionary<string, Action> Options){
        // some basic error checking
        if (Options.Count == 0) { return; }

        // get longest option
        int longestWord = Options.Keys.OrderByDescending(w=>w.Length).First().Length;
        if (Header.Length > longestWord) {longestWord = Header.Length;}

        // selection variables
        int currentSelection = 0;
        
        // draw loop
        ConsoleKey key;
        do{
            // print menu with options
            Console.Clear();
            // write header
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"┌─{Header}{new String('─', Math.Max(0, longestWord-Header.Length))}─┐\n");
            
            // loop over options and print them
            for (int i = 0; i < Options.Keys.Count; i++){
                string word = Options.Keys.ElementAt(i);
                Console.BackgroundColor = ConsoleColor.Black;
                // if currently selected make the background darkgray instead of black (3 prints so the whitespace doesnt get a background color)
                Console.Write("│ ");
                if (currentSelection == i) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                Console.Write($"{word}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{new String(' ', Math.Max(0, longestWord-word.Length))} │\n");
            }

            // write closing border
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"└─{new String('─', Math.Max(0, longestWord))}─┘");

            // get user input and call the callback if an option is selected
            key = Console.ReadKey(true).Key;

            // if the user presses uo/down we increase/decrease the current choice
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow){
                currentSelection += (key == ConsoleKey.DownArrow) ? 1 : -1;
            }

            // limit the current choice so it doesnt cause out of range errors
            currentSelection = Math.Clamp(currentSelection, 0, Options.Count-1);

        } while (key != ConsoleKey.Enter);
        Console.Clear();
        // call callback method based on the users choice
        Options.Values.ElementAt(currentSelection).Invoke();
    }
}