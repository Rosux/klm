namespace MenuHelper
{
    public static class ConfirmationUtility
    {
        /// <summary>
        /// Gives the user 2 options (true and false) and makes them choose one of these options.
        /// </summary>
        /// <param name="prompt">A string containing the question to confirm.</param>
        /// <returns>A boolean indicating if the user confirms or not.</returns>
        public static bool Confirm(string prompt="", bool warning = false){
            bool selection = false;
            ConsoleKey key;
            do
            {
                Console.CursorVisible = false;
                Console.Clear();
                Console.ForegroundColor = warning ? ConsoleColor.DarkYellow : ConsoleColor.White;
                Console.Write($"{prompt}\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = selection ? ConsoleColor.Black : ConsoleColor.DarkGray;
                Console.Write($">No");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"     ");
                Console.BackgroundColor = !selection ? ConsoleColor.Black : ConsoleColor.DarkGray;
                Console.Write($">Yes");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("\n\nPress Enter to confirm");
                key = Console.ReadKey(true).Key;
                if(key == ConsoleKey.Enter){
                    return selection;
                }
                if(key == ConsoleKey.RightArrow){
                    selection = true;
                }
                if(key == ConsoleKey.LeftArrow){
                    selection = false;
                }
            }while(true);
        }
    }
}