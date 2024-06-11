using System.Text.RegularExpressions;


namespace MenuHelper
{
    public static class StringUtility
    {
        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="suffix">A string of text printed after the selected value.</param>
        /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
        /// <param name="minimumLength">The minimum amount of characters required.</param>
        /// <param name="maximumLength">The maximum amount of characters required.</param>
        /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([a-zA-Z]| )" making it accept all letters and spaces.</param>
        /// <returns>A string chosen by the user or null if the user canceled the process.</returns>
        public static string? SelectText(string prefix="", string suffix="", bool canCancel=false, int minimumLength=0, int maximumLength=int.MaxValue, string allowedRegexPattern="([a-zA-Z]| )")
        {
            string input = "";
            string errorMessage = "";
            string keybinds = "Press Enter to confirm";
            if (canCancel){keybinds += "\nPress Escape to cancel";}
            int placeInput = 0;
            ConsoleKey key;
            char keyChar;
            do
            {
                int i = 0;
                errorMessage = "";
                if (input.Length < minimumLength || input.Length > maximumLength)
                {
                    errorMessage += $"Text must be between {minimumLength} and {maximumLength} characters\n";
                }
                Console.CursorVisible = false;
                Console.Clear();
                Console.Write($"{prefix}\n\n");
                foreach (char c in input)
                {
                    if(i == placeInput){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write(c);
                    Console.BackgroundColor = ConsoleColor.Black;
                    i++;
                }
                if(placeInput == input.Length){Console.BackgroundColor = ConsoleColor.DarkGray;}
                Console.Write($" \n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{errorMessage}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"\n\n{keybinds}\n{suffix}");
                ConsoleKeyInfo uwu = Console.ReadKey(true);
                key = uwu.Key;
                keyChar = uwu.KeyChar;

                if(Regex.IsMatch(keyChar.ToString(), allowedRegexPattern)){
                    input = input.Insert(placeInput, $"{keyChar}");
                    placeInput += 1;
                }
                if (key == ConsoleKey.Backspace && input.Length > 0 && placeInput > 0){
                    input = input.Remove(placeInput-1, 1);
                    placeInput -= 1;
                }
                if(key == ConsoleKey.LeftArrow && placeInput > 0){
                    placeInput -= 1;
                }
                if(key == ConsoleKey.RightArrow && placeInput < input.Length){
                    placeInput += 1;
                }
                if (key == ConsoleKey.Enter)
                {
                    if (input.Length >= minimumLength && input.Length <= maximumLength)
                    {
                        break;
                    }
                }
                if (canCancel && key == ConsoleKey.Escape)
                {
                    Console.CursorVisible = false;
                    Console.Clear();
                    return null;
                }
            }while(true);
            Console.CursorVisible = false;
            Console.Clear();
            return input;
        }

        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
        /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([a-zA-Z]| )" making it accept all letters and spaces.</param>
        /// <returns>A string chosen by the user or null if the user canceled the process.</returns>
        public static string? SelectText(string prefix="", bool canCancel=false, string allowedRegexPattern="([a-zA-Z]| )")
        {
            return SelectText(prefix, "", canCancel, 0, int.MaxValue, allowedRegexPattern);
        }

        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
        /// <param name="minimumLength">The minimum amount of characters required.</param>
        /// <param name="maximumLength">The maximum amount of characters required.</param>
        /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([a-zA-Z]| )" making it accept all letters and spaces.</param>
        /// <returns>A string chosen by the user or null if the user canceled the process.</returns>
        public static string? SelectText(bool canCancel=false, int minimumLength=0, int maximumLength=int.MaxValue, string allowedRegexPattern="([a-zA-Z]| )")
        {
            return SelectText("", "", canCancel, minimumLength, maximumLength, allowedRegexPattern);
        }

        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="suffix">A string of text printed after the selected value.</param>
        /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
        /// <returns>A string chosen by the user or null if the user canceled the process.</returns>
        public static string? SelectText(string prefix="", string suffix="", bool canCancel=false)
        {
            return SelectText(prefix, suffix, canCancel);
        }

        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
        /// <returns>A string chosen by the user or null if the user canceled the process.</returns>
        public static string? SelectText(string prefix="", bool canCancel=false)
        {
            return SelectText(prefix, "", canCancel);
        }

        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([a-zA-Z]| )" making it accept all letters and spaces.</param>
        /// <returns>A string chosen by the user.</returns>
        public static string SelectText(string prefix="", string allowedRegexPattern="([a-zA-Z]| )")
        {
            return SelectText(prefix, "", false, 0, int.MaxValue, allowedRegexPattern);
        }

        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="minimumLength">The minimum amount of characters required.</param>
        /// <param name="maximumLength">The maximum amount of characters required.</param>
        /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([a-zA-Z]| )" making it accept all letters and spaces.</param>
        /// <returns>A string chosen by the user.</returns>
        public static string SelectText(string prefix="", int minimumLength=0, int maximumLength=int.MaxValue, string allowedRegexPattern="([a-zA-Z]| )")
        {
            return SelectText(prefix, "", false, minimumLength, maximumLength, allowedRegexPattern);
        }

        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="minimumLength">The minimum amount of characters required.</param>
        /// <param name="maximumLength">The maximum amount of characters required.</param>
        /// <returns>A string chosen by the user.</returns>
        public static string SelectText(int minimumLength=0, int maximumLength=int.MaxValue)
        {
            return SelectText("", "", false, minimumLength, maximumLength);
        }

        /// <summary>
        /// Asks the user to type in a string and returns that value.
        /// </summary>
        /// <param name="minimumLength">The minimum amount of characters required.</param>
        /// <param name="maximumLength">The maximum amount of characters required.</param>
        /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([a-zA-Z]| )" making it accept all letters and spaces.</param>
        /// <returns>A string chosen by the user.</returns>
        public static string SelectText(int minimumLength=0, int maximumLength=int.MaxValue, string allowedRegexPattern="([a-zA-Z]| )")
        {
            return SelectText("", "", false, minimumLength, maximumLength, allowedRegexPattern);
        }
    }
}