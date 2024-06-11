namespace MenuHelper
{
    public static class EnumUtility
    {
        public static List<T>? SelectFromEnum<T>(List<T> options, string selectionHeader, string prefix, string suffix, bool canCancel)
        {
            string keybinds = "Press Enter to confirm\nUse the Up/Down arrows to select an item\nUse the Left/Right arrow to switch selection\n";
            if(canCancel){keybinds+="Press Escape to cancel";}
            List<T> selectedItems = new List<T>();
            bool inSelection = false;
            int selectedIndex = 0;
            int longestSelection = 0;
            int longestOption = 0;
            ConsoleKey key;
            do
            {
                #region Calculate max length
                if("Your selection".Length > longestSelection){
                    longestSelection = "Your selection".Length;
                }
                if("Save Selection".Length > longestSelection){
                    longestSelection = "Save Selection".Length;
                }
                if(selectionHeader.Length > longestOption){
                    longestOption = selectionHeader.Length;
                }
                foreach(T v in options)
                {
                    if(v.ToString().Length > longestOption){
                        longestOption = v.ToString().Length;
                    }
                }
                foreach(T v in selectedItems)
                {
                    if(v.ToString().Length > longestSelection){
                        longestSelection = v.ToString().Length;
                    }
                }
                #endregion

                #region Print
                Console.CursorVisible = false;
                Console.Clear();
                Console.Write($"{prefix}\n\n");
                Console.Write($"┌─{Format("Your selection", longestSelection, '─')}─┐ {(inSelection ? "->" : "<-")} ┌─{Format(selectionHeader, longestOption, '─')}─┐\n");
                for(int i=0;i<Math.Max(selectedItems.Count+3, options.Count+1);i++)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    if(i < selectedItems.Count){
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"│ ");
                        if(selectedIndex == i && inSelection){
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        Console.Write($"{Format(selectedItems[i].ToString(), longestSelection, ' ')}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($" │");
                    }else if(i == selectedItems.Count){
                        Console.Write($"├{Format('─', longestSelection+2)}┤");
                    }else if(i == selectedItems.Count + 1){
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"│ ");
                        if(selectedIndex == i-1 && inSelection){
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        Console.Write($"{Format("Save Selection", longestSelection, '─')}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($" │");
                    }else if(i == selectedItems.Count + 2){
                        Console.Write($"└─{Format('─', longestSelection)}─┘");
                    }else{
                        Console.Write($"{Format(' ', longestSelection+4)}");
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("    ");
                    if(i < options.Count){
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"│ ");
                        if(selectedIndex == i && !inSelection){
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        Console.Write($"{Format(options[i].ToString(), longestOption, ' ')}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($" │");
                    }else if(i == options.Count){
                        Console.Write($"└─{Format('─', longestOption)}─┘");
                    }
                    Console.Write($"\n");
                }
                Console.Write($"\n{keybinds}\n\n{suffix}\n");
                #endregion

                #region Input
                key = Console.ReadKey(true).Key;

                if(key == ConsoleKey.Enter && !inSelection && options.Count > 0)
                {
                    selectedItems.Add(options.ElementAt(selectedIndex));
                    options.RemoveAt(selectedIndex);
                    selectedIndex--;
                }
                if(key == ConsoleKey.Enter && inSelection && selectedItems.Count > 0 && selectedIndex < selectedItems.Count)
                {
                    options.Add(selectedItems.ElementAt(selectedIndex));
                    selectedItems.RemoveAt(selectedIndex);
                    selectedIndex--;
                }
                if(key == ConsoleKey.LeftArrow)
                {
                    inSelection = true;
                    selectedIndex = 0;
                }
                if(key == ConsoleKey.RightArrow)
                {
                    inSelection = false;
                    selectedIndex = 0;
                }
                if(key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
                {
                    selectedIndex += (key == ConsoleKey.UpArrow) ? -1 : 1;
                }
                // confirm/escape
                if(key == ConsoleKey.Enter && inSelection && selectedIndex == selectedItems.Count)
                {
                    return selectedItems;
                }
                if(key == ConsoleKey.Escape && canCancel)
                {
                    return null;
                }
                #endregion

                #region Clamping
                if(inSelection){
                    selectedIndex = Math.Clamp(selectedIndex, 0, Math.Max(0, selectedItems.Count));
                }else{
                    selectedIndex = Math.Clamp(selectedIndex, 0, Math.Max(0, options.Count-1));
                }
                #endregion

            }while(true);

            return null;
        }
        
        /// <summary>
        /// Creates a string of length totalWidth by putting the input data on the left side and padding it on the right with the specified char.
        /// </summary>
        /// <param name="data">A string that gets padded.</param>
        /// <param name="totalWidth">An integer indicating the maximum length of the final result. (overflows if data is longer than totalWidth)</param>
        /// <param name="d">A character used for the padding. Default is a space character.</param>
        /// <returns>A string of format "{data}d*totalWidth-data.Length"</returns>
        private static string Format(string data, int totalWidth, char d=' ')
        {
            return $"{data}{new string(d, Math.Max(0, totalWidth-data.Length))}";
        }

        /// <summary>
        /// Creates a string by repeating the specified character for the given number of times.
        /// </summary>
        /// <param name="d">A character to be repeated.</param>
        /// <param name="repeatTimes">An integer indicating the number of times to repeat the character. If repeatTimes is less than or equal to zero, an empty string is returned.</param>
        /// <returns>A string consisting of the character repeated repeatTimes times.</returns>
        private static string Format(char d, int repeatTimes)
        {
            return $"{new string(d, Math.Max(0, repeatTimes))}";
        }
    }
}