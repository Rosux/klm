namespace MenuHelper
{
    public static class ListUtility
    {
        /// <summary>
        /// Shows a list of options to the user and return the value of the chosen option unless the user cancels the selection.
        /// </summary>
        /// <typeparam name="T">The type you want returned.</typeparam>
        /// <param name="Header">A string of what comes at the top. (Like "Select an option")</param>
        /// <param name="canCancel">A boolean indicating if the user can cancel the selection.</param>
        /// <param name="Options">A Dictionary of options and any typed values that will be returned if that item is selected.</param>
        /// <returns>NULL if the user cancels the selection otherwise the value of the chosen option by the user.</returns>
        public static T? SelectFromList<T>(string Header, bool canCancel, Dictionary<string, T> Options){
            if (Options.Count == 0){
                return default(T);
            }
            // split Options into a list of chunks
            List<Dictionary<string, T>> chunks = new List<Dictionary<string, T>>();
            for (int i=0;i<Options.Count;i+=10)
            {
                chunks.Add(Options.Skip(i).Take(10).ToDictionary(kv => kv.Key, kv => kv.Value));
            }

            string keybinds = "Press Enter to select\nUse the Up/Down arrows to select a row\nUse the Left/Right arrow to browse pages";
            if(canCancel){
                keybinds += "\nPress Escape to cancel";
            }

            // get longest option
            int longestWord = Options.Keys.OrderByDescending(w=>w.Length).First().Length;
            if (Header.Length > longestWord) {longestWord = Header.Length;}
            int longestArrowString = ((chunks.Count.ToString().Length*2)+1) + Math.Max(2, longestWord-((chunks.Count.ToString().Length*2)+1)-4) + 4;
            if (longestArrowString > longestWord){ longestWord = longestArrowString; }

            // selection variables
            int currentSelection = 0;
            int currentPage = 0;

            // draw loop
            ConsoleKey key;
            do{
                // create the page number
                string pageNumber = $"{currentPage+1}/{chunks.Count}";
                string pageArrows = $"{pageNumber}";
                // add spaces on each side of the page number
                for (int i=1;i<=Math.Max(2, longestWord-pageNumber.Length-4);i++){
                    pageArrows = ((i % 2 == 1) ? " " : "") + pageArrows + ((i % 2 == 0) ? " " : "");
                }
                // add arrows on the correct sides of the page number
                pageArrows = (currentPage > 0 ? "<-" : "  ") + pageArrows + (currentPage < chunks.Count-1 ? "->" : "  ");

                // print menu with options
                Console.CursorVisible = false;
                Console.Clear();
                // write header
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"┌─{Header}{new String('─', Math.Max(0, longestWord-Header.Length))}─┐\n");

                // loop over options and print them
                for (int i = 0; i < chunks[currentPage].Keys.Count; i++){
                    string word = chunks[currentPage].Keys.ElementAt(i);
                    Console.BackgroundColor = ConsoleColor.Black;
                    // if currently selected make the background darkgray instead of black (3 prints so the whitespace doesnt get a background color)
                    Console.Write("│ ");
                    if (currentSelection == i) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write($"{word}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{new String(' ', Math.Max(0, longestWord-word.Length))} │\n");
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"├─{new String('─', Math.Max(0, longestWord))}─┤\n");

                // write the current page and arrows
                Console.Write($"│ {pageArrows} │\n");

                // write closing border
                Console.Write($"└─{new String('─', Math.Max(0, longestWord))}─┘");
                Console.Write($"\n\n{keybinds}");

                // get user input and call the callback if an option is selected
                key = Console.ReadKey(true).Key;

                // if the user presses uo/down we increase/decrease the current choice
                if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow){
                currentSelection += (key == ConsoleKey.DownArrow) ? 1 : -1;
                }
                if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow){
                currentPage += (key == ConsoleKey.RightArrow) ? 1 : -1;
                currentSelection = 0;
                }
                if(key == ConsoleKey.Escape && canCancel){
                    return default;
                }

                // limit the current choice so it doesnt cause out of range errors
                currentPage = Math.Clamp(currentPage, 0, chunks.Count-1);
                currentSelection = Math.Clamp(currentSelection, 0, chunks[currentPage].Count-1);

            } while (key != ConsoleKey.Enter);
            return chunks[currentPage].Values.ElementAt(currentSelection);
        }

        /// <summary>
        /// Shows a list of options to the user and return the value of the chosen option.
        /// </summary>
        /// <typeparam name="T">The type you want returned.</typeparam>
        /// <param name="Header">A string of what comes at the top. (Like "Select an option")</param>
        /// <param name="Options">A Dictionary of options and any typed values that will be returned if that item is selected.</param>
        /// <returns>The value of the chosen option by the user.</returns>
        public static T SelectFromList<T>(string Header, Dictionary<string, T> Options){
            return SelectFromList(Header, false, Options) ?? default;
        }
    }
}