using System;

// VIEW (handle user input/output here as much as you can such as asking a user to select a time and recieving that input and sending it to the controller)
public static class MenuHelper{
    /// <summary>
    /// Shows a select time to user as: 00:00 and lets the user select a specific time in HH:MM format.
    /// </summary>
    /// <returns>A TimeOnly object containing the user selected time.</returns>
    public static TimeOnly SelectTime(){
        TimeOnly time = new TimeOnly();
        bool hour = true;
        ConsoleKey key;
        do{
            Console.Clear();
            Console.BackgroundColor = hour ? ConsoleColor.DarkGray : ConsoleColor.Black;
            Console.Write($"{time.Hour.ToString("00")}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($":");
            Console.BackgroundColor = !hour ? ConsoleColor.DarkGray : ConsoleColor.Black;
            Console.Write($"{time.Minute.ToString("00")}");
            Console.BackgroundColor = ConsoleColor.Black;
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.LeftArrow)
            {
                hour = true;
            }
            if (key == ConsoleKey.RightArrow)
            {
                hour = false;
            }
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
            {
                double TimeAmount = key == ConsoleKey.DownArrow ? -1 : 1;
                if (hour){
                    time = time.AddHours(TimeAmount);
                } else {
                    time = time.AddMinutes(TimeAmount);
                }
            }
        } while (key != ConsoleKey.Enter);
        Console.Clear();
        return time;
    }

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

    /// <summary>
    /// Shows a list of options to the user and return the value of the chosen option.
    /// </summary>
    /// <typeparam name="T">The type you want returned.</typeparam>
    /// <param name="Header">A string of what comes at the top. (Like "Select an option")</param>
    /// <param name="Options">A Dictionary of options and any typed values that will be returned if that item is selected.</param>
    /// <returns>The value of the chosen option by the user.</returns>
    public static T SelectFromList<T>(string Header, Dictionary<string, T> Options){
        // split Options into a list of chunks
        List<Dictionary<string, T>> chunks = new List<Dictionary<string, T>>();
        for (int i=0;i<Options.Count;i+=10)
        {
            chunks.Add(Options.Skip(i).Take(10).ToDictionary(kv => kv.Key, kv => kv.Value));
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

            // limit the current choice so it doesnt cause out of range errors
            currentPage = Math.Clamp(currentPage, 0, chunks.Count-1);
            currentSelection = Math.Clamp(currentSelection, 0, chunks[currentPage].Count-1);

        } while (key != ConsoleKey.Enter);
        return chunks[currentPage].Values.ElementAt(currentSelection);
    }
}
