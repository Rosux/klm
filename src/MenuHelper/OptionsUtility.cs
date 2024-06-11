namespace MenuHelper
{
    public static class OptionsUtility
    {
        /// <summary>
        /// Show a menu to the user of the given options and use the callback when the user selects a specific option.
        /// </summary>
        /// <param name="Header">A string of what comes at the top. (Like "Select an option")</param>
        /// <param name="Options">A Dictionary of options and callbacks.</param>
        /// <example>
        /// For example, this:
        /// <code>
        /// Options("Select an action", new Dictionary<string, Action>(){
        ///     {"Login", ()=>{SomeLoginMethod();}},
        ///     {"Register", ()=>{SomeRegisterMethod();}},
        ///     {"Exit", ()=>{SomeExitMethod();}},
        /// });
        /// </code>
        /// Would result in this being shown to the user:
        /// ┌─Select an action─┐
        /// │ Login            │
        /// │ Register         │
        /// │ Exit             │
        /// └──────────────────┘
        /// </example>
        public static void SelectOptions(string Header, Dictionary<string, Action> Options){
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
                Console.CursorVisible = false;
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
            Console.CursorVisible = false;
            Console.Clear();
            // call callback method based on the users choice
            Options.Values.ElementAt(currentSelection).Invoke();
        }
    }
}