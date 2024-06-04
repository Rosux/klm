using System.Net.Mail;
using System.Text.RegularExpressions;
using BCrypt.Net;

public static class UserMenu{
    private static UserAccess _userAccess = new UserAccess();

    /// <summary>
    /// Asks the user to fill in fields and return a new User object with the field data.
    /// </summary>
    /// <returns>A User object or NULL if the user cancels.</returns>
    public static User? GetNewUser()
    {
        string? firstName = GetValidInput("Enter first name:", 3, 20);
        if (firstName == null)
        {
            return null;
        }
        string? lastName = GetValidInput("Enter last name:", 3, 20);
        if (lastName == null)
        {
            return null;
        }
        string? email = GetValideEmail("Enter email:", 3, 20);
        if (email == null)
        {
            return null;
        }
        string? password = GetValidPassword("Enter password:", 6, 20);
        if (password == null)
        {
            return null;
        }
        string? password2 = GetValidPassword("Enter password again:", 6, 20, true, password);
        if (password2 == null)
        {
            return null;
        }
        string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        User user = new User(firstName, lastName, email, passwordHash);
        return user;
    }

    /// <summary>
    /// Asks the user to fill in login credentials and returns then as a user.
    /// </summary>
    /// <returns>A empty user instance except for an Email and raw Password</returns>
    public static User? GetLoginCredentials()
    {
        string Email = "";
        do{
            Console.CursorVisible = false;
            Console.Clear();
            Console.Write($"Email: {Email}\nPassword: \n\nPress Escape to cancel");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if(keyInfo.Key == ConsoleKey.Enter){
                break;
            }
            if(keyInfo.Key == ConsoleKey.Backspace && Email.Length > 0){
                Email = Email.Remove(Email.Length-1);
            }
            if(!char.IsControl(keyInfo.KeyChar)){
                Email += keyInfo.KeyChar;
            }
            if(keyInfo.Key == ConsoleKey.Escape){
                return null;
            }
        }while(true);
        string Password = "";
        do{
            Console.CursorVisible = false;
            Console.Clear();
            Console.Write($"Email: {Email}\nPassword: {new string('*', Password.Length)}\n\nPress Escape to cancel");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if(keyInfo.Key == ConsoleKey.Enter){
                break;
            }
            if(keyInfo.Key == ConsoleKey.Backspace && Password.Length > 0){
                Password = Password.Remove(Password.Length-1);
            }
            if(!char.IsControl(keyInfo.KeyChar)){
                Password += keyInfo.KeyChar;
            }
            if(keyInfo.Key == ConsoleKey.Escape){
                return null;
            }
        }while(true);
        return new User(null, null, Email, Password);
    }


    #region Input validation
    /// <summary>
    /// Asks the user to fill in a password and checks if its strong enough.
    /// </summary>
    /// <param name="prompt">A string containing text for the user to read.</param>
    /// <param name="minLength">Minimum length required for the password.</param>
    /// <param name="maxLength">Maximum length required for the password.</param>
    /// <param name="secondPass">A boolean indicating if the password should match the firstPassword parameter.</param>
    /// <param name="firstPassword">A string of the first password that gets compared to check if the passwords both match.</param>
    /// <returns>A string containing the password if it matches or NULL if the user cancels.</returns>
    private static string? GetValidPassword(string prompt, int minLength, int maxLength, bool secondPass = false, string firstPassword = "")
    {
        string input = "";
        string error= "";
        do
        {
            Console.CursorVisible = false;
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
        Console.CursorVisible = false;
        Console.Clear();
        return input;
    }

    /// <summary>
    /// Asks the user to fill in a string of only A-Z characters with a minimum and maximum length.
    /// </summary>
    /// <param name="prompt">A string containing text for the user to read.</param>
    /// <param name="minLength">Minimum length required for the value.</param>
    /// <param name="maxLength">Minimum length required for the value.</param>
    /// <returns>A string contaning the user input or NULL if the user cancels.</returns>
    public static string? GetValidInput(string prompt, int minLength, int maxLength)
    {
        string input = "";
        string error= "";
        do
        {
            Console.CursorVisible = false;
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
        Console.CursorVisible = false;
        Console.Clear();
        return input;
    }

    /// <summary>
    /// Asks the user to type in a valid email.
    /// </summary>
    /// <param name="prompt">A string containing text for the user to read.</param>
    /// <param name="minLength">Minimum length required for the email.</param>
    /// <param name="maxLength">Maximum length required for the email.</param>
    /// <returns>A string containing the email or NULL in case the user cancels.</returns>
    public static string? GetValideEmail(string prompt, int minLength, int maxLength)
    {
        string input = "";
        string error= "";
        do
        {
            error = "";
            User? user = _userAccess.VerifyUser(input);
            if (user != null)
            {
                error += $"Email already exists.\n";
            }
            if (!MailAddress.TryCreate(input, out MailAddress? _))
            {
                error += $"email is not valid";
            }
            Console.CursorVisible = false;
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
        Console.CursorVisible = false;
        Console.Clear();
        return input;
    }

    /// <summary>
    /// Checks if a given password is strong enough.
    /// </summary>
    /// <param name="password">A string containing the raw password.</param>
    /// <returns>A boolean indicating if it is strong enough or not.</returns>
    private static bool IsStrongPassword(string password)
    {
        var hasLowercase = new Regex(@"[a-z]").IsMatch(password);
        var hasUppercase = new Regex(@"[A-Z]").IsMatch(password);
        var hasSpecialChar = new Regex(@"\W").IsMatch(password);
        var hasNumber = new Regex(@"\d").IsMatch(password);

        return hasLowercase && hasUppercase && hasSpecialChar && hasNumber;
    }
    #endregion

    #region Text output for validation
    /// <summary>
    /// Notifies the user about their password/email not being valid or not matching.
    /// </summary>
    public static void WrongLogin(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Email or password.\n\nPress any key to continue");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey(true);
    }

    /// <summary>
    /// Notifies the user about if the user got removed successfully or not.
    /// </summary>
    /// <param name="success">A boolean indicating if the removal was successfull.</param>
    public static void UserRemoved(bool success){
        Console.CursorVisible = false;
        Console.Clear();
        if(success){
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The user has successfully been removed.\n\nPress any key to continue");
        }else{
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("An error occurred. Please try again later.\n\nPress any key to continue");
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey(true);
    }

    /// <summary>
    /// Notifies the user about if the user got added successfully or not.
    /// </summary>
    /// <param name="success">A boolean indicating if the adition was successfull.</param>
    public static void UserAdded(bool success){
        Console.CursorVisible = false;
        Console.Clear();
        if(success){
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The user has successfully been added.\n\nPress any key to continue");
        }else{
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("An error occurred. Please try again later.\n\nPress any key to continue");
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey(true);
    }

    /// <summary>
    /// Notifies the user about if the user got updated successfully or not.
    /// </summary>
    /// <param name="success">A boolean indicating if the updating was successfull.</param>
    public static void UserUpdated(bool success){
        Console.CursorVisible = false;
        Console.Clear();
        if(success){
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The user has successfully been updated.\n\nPress any key to continue");
        }else{
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("An error occurred. Please try again later.\n\nPress any key to continue");
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey(true);
    }
    #endregion
}
