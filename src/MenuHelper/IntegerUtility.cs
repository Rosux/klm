namespace MenuHelper
{
    public static class IntegerUtility
    {
        /// <summary>
        /// Ask the user to select an integer between specified values and returns the chosen integer.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="suffix">A string of text printed after the selected value.</param>
        /// <param name="canCancel">A boolean indicating if the user can cancel the selection; Returns null if canceled.</param>
        /// <param name="defaultInt">The default integer shown to the user.</param>
        /// <param name="min">The minimum allowed value of the integer.</param>
        /// <param name="max">The maximum allowed value of the integer.</param>
        /// <returns>An integer chosen by the user or null if the user canceled the selection.</returns>
        public static int? SelectInteger(string prefix="", string suffix="", bool canCancel = false, int defaultInt = 0, int min=int.MinValue, int max=int.MaxValue){
            int num = defaultInt;
            string error = "";
            string inputNum = defaultInt.ToString();
            string keybinds = "Press Enter to confirm";
            int placeInputNum = 0;
            if (canCancel)
            {
                keybinds += "\nPress Escape to cancel";
            }
            ConsoleKey key;
            ConsoleKeyInfo RawKey;
            do{
                int i = 0;
                // set error message
                if (num < min || num > max)
                {
                    error = $"Please select a value between {min} and {max}";
                }
                else
                {
                    error = "";
                }

                Console.CursorVisible = false;
                Console.Clear();
                Console.Write($"{prefix}\n\n");
                foreach (char c in inputNum)
                {
                    if(i == placeInputNum){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write(c);
                    Console.BackgroundColor = ConsoleColor.Black;
                    i++;
                }
                if(placeInputNum == inputNum.Length){Console.BackgroundColor = ConsoleColor.DarkGray;}
                Console.Write($" \n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{error}\n\n{keybinds}\n{suffix}");
                RawKey = Console.ReadKey(true);
                key = RawKey.Key;

                // add number to string
                if (char.IsDigit(RawKey.KeyChar) && int.TryParse(inputNum+RawKey.KeyChar, out int x))
                {
                    inputNum = inputNum.Insert(placeInputNum, $"{RawKey.KeyChar}");
                    placeInputNum += 1;
                }
                // remove interger from string
                if (key == ConsoleKey.Backspace){
                    if (inputNum.Length > 0 && placeInputNum > 0)
                    {
                        inputNum = inputNum.Remove(placeInputNum-1, 1);
                        placeInputNum -= 1;
                    }
                    if (inputNum.Length == 0)
                    {
                        inputNum = "0";
                    }
                }

                // set num to inputNum
                if(!int.TryParse(inputNum, out num)){
                    continue;
                }

                // up/down arrow increases/decreases number
                if (key == ConsoleKey.UpArrow && placeInputNum !=  inputNum.Length || key == ConsoleKey.DownArrow && placeInputNum !=  inputNum.Length )
                {
                    num += key == ConsoleKey.DownArrow ? -(int)Math.Pow(10, inputNum.Length - placeInputNum -1) : (int)Math.Pow(10, inputNum.Length - placeInputNum -1);
                    inputNum = num.ToString();
                }
                // enter tries to see if the number is between the min/max and if not sets an suggestive error message
                if(key == ConsoleKey.LeftArrow && placeInputNum > 0){
                    placeInputNum -= 1;
                }
                if(key == ConsoleKey.RightArrow && placeInputNum < inputNum.Length){
                    placeInputNum += 1;
                }
                if (key == ConsoleKey.Enter)
                {
                    if (num >= min && num <= max)
                    {
                        break;
                    }
                }
                if (canCancel && key == ConsoleKey.Escape)
                {
                    return null;
                }
            } while(true);
            Console.CursorVisible = false;
            Console.Clear();
            return num;
        }

        /// <summary>
        /// Shortcut method to select an integer with default prefix and suffix and without the option to cancel.
        /// </summary>
        /// <param name="defaultInt">The default integer shown to the user.</param>
        /// <param name="min">The minimum allowed value of the integer.</param>
        /// <param name="max">The maximum allowed value of the integer.</param>
        /// <returns>An integer chosen by the user.</returns>
        public static int SelectInteger(int defaultInt = 0, int min=int.MinValue, int max=int.MaxValue){
            return SelectInteger("", "", false, defaultInt, min, max) ?? defaultInt;
        }

        /// <summary>
        /// Shortcut method to select an integer with custom prefix, default suffix, and without the option to cancel.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="defaultInt">The default integer shown to the user.</param>
        /// <param name="min">The minimum allowed value of the integer.</param>
        /// <param name="max">The maximum allowed value of the integer.</param>
        /// <returns>An integer chosen by the user.</returns>
        public static int SelectInteger(string prefix="", int defaultInt = 0, int min=int.MinValue, int max=int.MaxValue){
            return SelectInteger(prefix, "", false, defaultInt, min, max) ?? defaultInt;
        }

        /// <summary>
        /// Shortcut method to select an integer with default prefix and suffix and the option to cancel.
        /// </summary>
        /// <param name="canCancel">A boolean indicating if the user can cancel the selection; Returns null if canceled.</param>
        /// <param name="defaultInt">The default integer shown to the user.</param>
        /// <param name="min">The minimum allowed value of the integer.</param>
        /// <param name="max">The maximum allowed value of the integer.</param>
        /// <returns>An integer chosen by the user or null if the user canceled the selection.</returns>
        public static int? SelectInteger(bool canCancel = false, int defaultInt = 0, int min=int.MinValue, int max=int.MaxValue){
            return SelectInteger("", "", canCancel, defaultInt, min, max);
        }

        /// <summary>
        /// Shortcut method to select an integer with default prefix and suffix, without the option to cancel, and with a specified range.
        /// </summary>
        /// <param name="min">The minimum allowed value of the integer.</param>
        /// <param name="max">The maximum allowed value of the integer.</param>
        /// <returns>An integer chosen by the user.</returns>
        public static int SelectInteger(int min=int.MinValue, int max=int.MaxValue){
            return SelectInteger("", "", false, min, min, max) ?? -1;
        }

        /// <summary>
        /// Shortcut method to select an integer with default prefix and suffix and without the option to cancel.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="suffix">A string of text printed after the selected value.</param>
        /// <param name="defaultInt">The default integer shown to the user.</param>
        /// <param name="min">The minimum allowed value of the integer.</param>
        /// <param name="max">The maximum allowed value of the integer.</param>
        /// <returns>
        /// An integer chosen by the user.
        /// </returns>
        public static int SelectInteger(string prefix="", string suffix="", int defaultInt = 0, int min=int.MinValue, int max=int.MaxValue){
            return SelectInteger(prefix, suffix, false, defaultInt, min, max) ?? defaultInt;
        }
    }
}