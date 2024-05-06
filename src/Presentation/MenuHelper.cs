using System;

public static class MenuHelper{
private static SearchAccess searchAccess = new SearchAccess();

    #region Integer
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
        if (canCancel)
        {
            keybinds += "\nPress Escape to cancel";
        }
        ConsoleKey key;
        ConsoleKeyInfo RawKey;
        do{
            // set error message
            if (num < min || num > max)
            {
                error = $"Please select a value between {min} and {max}";
            }
            else
            {
                error = "";
            }

            Console.Clear();
            Console.Write($"{prefix}\n\n{num}\n{error}\n\n{keybinds}\n{suffix}");
            RawKey = Console.ReadKey(true);
            key = RawKey.Key;

            // add number to string
            if (char.IsDigit(RawKey.KeyChar))
            {
                inputNum += RawKey.KeyChar;
            }
            // remove last from string
            if (key == ConsoleKey.Backspace){
                if (inputNum.Length > 0)
                {
                    inputNum = inputNum.Remove(inputNum.Length-1);
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
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
            {
                num += key == ConsoleKey.DownArrow ? -1 : 1;
                inputNum = num.ToString();
            }
            // enter tries to see if the number is between the min/max and if not sets an suggestive error message
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
    #endregion

    #region Date
    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <param name="defaultTime">A DateOnly object holding the default date shown to the user.</param>
    /// <param name="minDate">The minimum allowed value of the DateOnly.</param>
    /// <param name="maxDate">The maximum allowed value of the DateOnly.</param>
    /// <returns>An DateOnly object with the date chosen by the user.</returns>
    public static DateOnly? SelectDate(string prefix="", string suffix="", bool canCancel = false, DateOnly? defaultTime = null, DateOnly? minDate = null, DateOnly? maxDate = null){
        DateOnly MinDate = minDate ?? DateOnly.MinValue;
        DateOnly MaxDate = maxDate ?? DateOnly.MaxValue;
        DateOnly SelectedDate = defaultTime ?? new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        bool SelectMonth = true;
        string keybinds = "Press Enter to confirm\nUse the arrows to select a date";
        if (canCancel)
        {
            keybinds += "\nPress Escape to cancel";
        }

        ConsoleKey key;
        do{
            // some needed calulations
            int startDay = ((int)new DateOnly(SelectedDate.Year, SelectedDate.Month, 1).DayOfWeek - 1 + 7) % 7;
            // print callender
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"{prefix}");
            if(prefix != ""){
                Console.Write("\n");
            }
            Console.BackgroundColor = SelectMonth ? ConsoleColor.DarkGray : ConsoleColor.Black;
            Console.Write($"{SelectedDate.ToString("dd/MM/yyyy")}\n");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"Mo Tu We Th Fr Sa Su\n");
            int dayNum = 1;
            int OverFlowNum = 1;
            for (int day = 0; day < (int)Math.Ceiling((double)(DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month) + startDay) / 7) * 7; day++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (day < startDay)
                {
                    int d = DateTime.DaysInMonth(SelectedDate.AddMonths(-1).Year, SelectedDate.AddMonths(-1).Month);
                    if ((SelectedDate.Year == MinDate.Year && SelectedDate.Month == MinDate.Month && MinDate.Day > dayNum) || (SelectedDate.Year == MaxDate.Year && SelectedDate.Month == MaxDate.Month && MaxDate.Day < dayNum)){
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }else{
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    Console.Write($"{d-(startDay-day)+1,2} ");
                    continue;
                }
                Console.BackgroundColor = (!SelectMonth && dayNum == SelectedDate.Day) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                if ((SelectedDate.Year == MinDate.Year && SelectedDate.Month == MinDate.Month && MinDate.Day > dayNum) || (SelectedDate.Year == MaxDate.Year && SelectedDate.Month == MaxDate.Month && MaxDate.Day < dayNum))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                if(dayNum <= DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month)){
                    Console.Write($"{dayNum,2}");
                    dayNum++;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(((day+8) % 7 == 0) ? " \n" : " ");
                }else{
                    if ((SelectedDate.Year == MinDate.Year && SelectedDate.Month == MinDate.Month && MinDate.Day > dayNum) || (SelectedDate.AddMonths(1).Year == MaxDate.Year && SelectedDate.AddMonths(1).Month == MaxDate.Month && MaxDate.Day < OverFlowNum)){
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }else{
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{OverFlowNum,2}");
                    OverFlowNum++;
                    Console.Write(((day+8) % 7 == 0) ? " \n" : " ");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"\n\n{keybinds}\n{suffix}");
            // read input
            key = Console.ReadKey(true).Key;
            // handle input
            if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
            {
                if (SelectMonth)
                {
                    if (
                        SelectedDate.AddMonths(key == ConsoleKey.LeftArrow ? -1 : 1).Year  >= MinDate.Year  &&
                        SelectedDate.AddMonths(key == ConsoleKey.LeftArrow ? -1 : 1).Month >= MinDate.Month &&
                        SelectedDate.AddMonths(key == ConsoleKey.LeftArrow ? -1 : 1).Year  <= MaxDate.Year  &&
                        SelectedDate.AddMonths(key == ConsoleKey.LeftArrow ? -1 : 1).Month <= MaxDate.Month
                    ) {
                        SelectedDate = SelectedDate.AddMonths(key == ConsoleKey.LeftArrow ? -1 : 1);
                        SelectedDate = new DateOnly(SelectedDate.Year, SelectedDate.Month, 1);
                    }
                }
                else
                {
                    if (SelectedDate.Day + ((key == ConsoleKey.LeftArrow) ? -1 : 1) <= 0 && SelectedDate.AddMonths(-1).Year >= MinDate.Year && SelectedDate.AddMonths(-1).Month >= MinDate.Month)
                    {
                        // previous month
                        SelectedDate = SelectedDate.AddMonths(-1);
                        SelectedDate = new DateOnly(SelectedDate.Year, SelectedDate.Month, DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month));
                    }
                    else if (SelectedDate.Day + ((key == ConsoleKey.LeftArrow) ? -1 : 1) > DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month) && SelectedDate.AddMonths(1).Year <= MaxDate.Year && SelectedDate.AddMonths(1).Month <= MaxDate.Month)
                    {
                        // next month
                        SelectedDate = SelectedDate.AddMonths(1);
                        SelectedDate = new DateOnly(SelectedDate.Year, SelectedDate.Month, 1);
                    }
                    else if (SelectedDate.Day + ((key == ConsoleKey.LeftArrow) ? -1 : 1) >= 1 && SelectedDate.Day + ((key == ConsoleKey.LeftArrow) ? -1 : 1) <= DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month))
                    {
                        SelectedDate = SelectedDate.AddDays((key == ConsoleKey.LeftArrow) ? -1 : 1);
                    }
                }
            }
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
            {
                if(!SelectMonth && key == ConsoleKey.UpArrow && SelectedDate.Day + startDay <= 7){
                    SelectMonth = true;
                }
                if (!SelectMonth){
                    int addAmount = (key == ConsoleKey.UpArrow ? -7 : 7);
                    if (SelectedDate.Day + addAmount >= 1 && SelectedDate.Day + addAmount <= DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month))
                    {
                        SelectedDate = SelectedDate.AddDays(addAmount);
                    }
                    else if (addAmount > 0)
                    {
                        SelectedDate = new DateOnly(SelectedDate.Year, SelectedDate.Month, DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month));
                    }
                    else if (addAmount < 0)
                    {
                        SelectedDate = new DateOnly(SelectedDate.Year, SelectedDate.Month, 1);
                    }
                }
                if(SelectMonth && key == ConsoleKey.DownArrow){
                    SelectMonth = false;
                }
            }
            if (key == ConsoleKey.Escape && canCancel)
            {
                return null;
            }
        }while(key != ConsoleKey.Enter || SelectMonth || SelectedDate < MinDate || SelectedDate > MaxDate);
        return SelectedDate;
    }

    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="defaultTime">A DateOnly object holding the default date shown to the user.</param>
    /// <param name="minDate">The minimum allowed value of the DateOnly.</param>
    /// <param name="maxDate">The maximum allowed value of the DateOnly.</param>
    /// <returns>An DateOnly object with the date chosen by the user.</returns>
    public static DateOnly SelectDate(DateOnly? defaultTime = null, DateOnly? minDate = null, DateOnly? maxDate = null){
        return SelectDate("", "", false, defaultTime, minDate, maxDate) ?? DateOnly.MinValue;
    }

    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <param name="defaultTime">A DateOnly object holding the default date shown to the user.</param>
    /// <param name="minDate">The minimum allowed value of the DateOnly.</param>
    /// <param name="maxDate">The maximum allowed value of the DateOnly.</param>
    /// <returns>An DateOnly object with the date chosen by the user.</returns>
    public static DateOnly SelectDate(string prefix="", string suffix="", DateOnly? defaultTime = null, DateOnly? minDate = null, DateOnly? maxDate = null){
        return SelectDate(prefix, suffix, false, defaultTime, minDate, maxDate) ?? DateOnly.MinValue;
    }

    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="defaultTime">A DateOnly object holding the default date shown to the user.</param>
    /// <param name="minDate">The minimum allowed value of the DateOnly.</param>
    /// <param name="maxDate">The maximum allowed value of the DateOnly.</param>
    /// <returns>An DateOnly object with the date chosen by the user.</returns>
    public static DateOnly SelectDate(string prefix="", DateOnly? defaultTime = null, DateOnly? minDate = null, DateOnly? maxDate = null){
        return SelectDate(prefix, "", false, defaultTime, minDate, maxDate) ?? DateOnly.MinValue;
    }

    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <returns>An DateOnly object with the date chosen by the user.</returns>
    public static DateOnly SelectDate(string prefix=""){
        return SelectDate(prefix, "", false, null, null, null) ?? DateOnly.MinValue;
    }

    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="canCancel">A boolean value indicating whether the user can cancel the operation.</param>
    /// <param name="defaultTime">A DateOnly object holding the default date shown to the user.</param>
    /// <param name="minDate">The minimum allowed value of the DateOnly.</param>
    /// <param name="maxDate">The maximum allowed value of the DateOnly.</param>
    /// <returns>An DateOnly object with the date chosen by the user.</returns>
    public static DateOnly? SelectDate(bool canCancel=false, DateOnly? defaultTime = null, DateOnly? minDate = null, DateOnly? maxDate = null){
        return SelectDate("", "", canCancel, defaultTime, minDate, maxDate) ?? DateOnly.MinValue;
    }

    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="minDate">The minimum allowed value of the DateOnly.</param>
    /// <param name="maxDate">The maximum allowed value of the DateOnly.</param>
    /// <returns>An DateOnly object with the date chosen by the user.</returns>
    public static DateOnly SelectDate(DateOnly? minDate = null, DateOnly? maxDate = null){
        return SelectDate("", "", false, null, minDate, maxDate) ?? DateOnly.MinValue;
    }
    #endregion

    #region Time
    /// <summary>
    /// Shows a select time to user as: 00:00 and lets the user select a specific time in HH:MM format.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <param name="defaultTime">A TimeOnly object as starting point for the user.</param>
    /// <returns>A TimeOnly object containing the user selected time.</returns>
    public static TimeOnly SelectTime(string prefix = "", string suffix = "", TimeOnly defaultTime = new TimeOnly(), TimeOnly? minTime = null, TimeOnly? maxTime = null){
        TimeOnly MinTime = minTime ?? TimeOnly.MinValue;
        TimeOnly MaxTime = maxTime ?? TimeOnly.MaxValue;
        if(defaultTime <= MinTime){
            defaultTime = MinTime;
        }
        TimeOnly time = defaultTime;
        bool hour = true;
        ConsoleKey key;
        do{
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"{prefix}\n\n");
            Console.BackgroundColor = hour ? ConsoleColor.DarkGray : ConsoleColor.Black;
            Console.Write($"{time.Hour.ToString("00")}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($":");
            Console.BackgroundColor = !hour ? ConsoleColor.DarkGray : ConsoleColor.Black;
            Console.Write($"{time.Minute.ToString("00")}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"\n\n{suffix}");
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

                if (time < MinTime){
                    time = MinTime;
                }
                if (time > MaxTime){
                    time = MaxTime;
                }
            }
        } while (key != ConsoleKey.Enter || time > MaxTime || time < MinTime);
        Console.Clear();
        return time;
    }

    /// <summary>
    /// Shows a select time to user as: 00:00 and lets the user select a specific time in HH:MM format.
    /// </summary>
    /// <param name="defaultTime">A TimeOnly object as starting point for the user.</param>
    /// <returns>A TimeOnly object containing the user selected time.</returns>
    public static TimeOnly SelectTime(TimeOnly defaultTime = new TimeOnly()){
        return SelectTime("", "", defaultTime, null, null);
    }
    #endregion

    #region Callback options from a Dict
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
    #endregion

    #region Options from a Dict
    /// <summary>
    /// Shows a list of options to the user and return the value of the chosen option.
    /// </summary>
    /// <typeparam name="T">The type you want returned.</typeparam>
    /// <param name="Header">A string of what comes at the top. (Like "Select an option")</param>
    /// <param name="Options">A Dictionary of options and any typed values that will be returned if that item is selected.</param>
    /// <returns>The value of the chosen option by the user.</returns>
    public static T SelectFromList<T>(string Header, Dictionary<string, T> Options){
        if (Options.Count == 0){
            return default(T);
        }
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
    #endregion


    #region Movie or Series/Episodes select
    /// <summary>
    /// Ask the user to select a movie or series episodes.
    /// </summary>
    /// <returns>null if the user cancels the search. If a movie is selected it will return a Movie object. if a series is selected it will return a Dictionary<Serie, List<Episode>></returns>
    public static object? SelectMovieOrEpisode(){
        Media selectedMedia;
        int longestWord;
        string searchString = "";
        int cursorPosition = searchString.Length;
        bool typing = true;
        int selectedResult = 0;
        ConsoleKey key;
        ConsoleKeyInfo keyInfo;
        do{
            // calculate longest word
            List<Media> results = searchAccess.Search(searchString);
            longestWord = "Start typing to search".Length + 2;
            foreach(Media m in results){
                if (m is Film && ((Film)m).Title.Length+3 > longestWord){
                    longestWord = ((Film)m).Title.Length + 3;
                }else if(m is Serie && ((Serie)m).Title.Length+3 > longestWord){
                    longestWord = ((Serie)m).Title.Length + 3;
                }
            } 
            if (searchString.Length + 2 > longestWord){
                longestWord = searchString.Length + 2;
            }

            // print the search box thing
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"Search by Titles or Genres seperated by commas.\nPress escape to cancel.\n\n┌─Start typing to search{new string('─', Math.Max(0, longestWord-22))}─┐\n");
            Console.Write($"│ > ");
            string printedText = searchString.Substring(Math.Max(0, searchString.Length-longestWord));
            for(int i = 0; i < printedText.Length; i++){
                Console.BackgroundColor = (i == cursorPosition && typing) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                Console.Write($"{printedText[i]}");
            }
            Console.BackgroundColor = (cursorPosition == printedText.Length && typing) ? ConsoleColor.DarkGray : ConsoleColor.Black;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"{new string(' ', Math.Max(0, longestWord-printedText.Length-2))}");
            Console.Write($"│\n");

            if(searchString == "" || results.Count() == 0){
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"└─{new string('─', Math.Max(0, longestWord))}─┘");
            }else{
                for(int i=0;i<Math.Min(results.Count(), 5);i++)
                {
                    if (i == 0) {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("└── ");
                        string title = results[i] is Film ? ((Film)results[i]).Title : ((Serie)results[i]).Title;
                        Console.BackgroundColor = (!typing && selectedResult == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                        Console.Write($"{title}{new string(' ', Math.Max(0, longestWord-title.Length-3))}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($" ─┘\n");
                    } else {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("    ");
                        Console.BackgroundColor = (!typing && selectedResult == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                        string title = results[i] is Film ? ((Film)results[i]).Title : ((Serie)results[i]).Title;
                        Console.Write($"{title}{new string(' ', Math.Max(0, longestWord-title.Length-3))}\n");
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }

            keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if(typing && (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)) {
                if (key == ConsoleKey.LeftArrow && cursorPosition > 0) {
                    cursorPosition--; // Move cursor left if not at the beginning
                } else if (key == ConsoleKey.RightArrow && cursorPosition < searchString.Length) {
                    cursorPosition++; // Move cursor right if not at the end
                }
            }
            if (typing && !char.IsControl(keyInfo.KeyChar)) {
                searchString = searchString.Insert(cursorPosition, keyInfo.KeyChar.ToString());
                cursorPosition++;
            }
            if (typing && cursorPosition > 0 && searchString.Length > 0 && key == ConsoleKey.Backspace) {
                searchString = searchString.Remove(cursorPosition - 1, 1);
                cursorPosition--;
            }
            if (!typing && (key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow)) {
                // move selection up/down
                if (key == ConsoleKey.UpArrow && selectedResult == 0) {
                    typing = true;
                    continue;
                }
                selectedResult += (key == ConsoleKey.DownArrow) ? 1 : -1;
            }
            if (typing && key == ConsoleKey.DownArrow && results.Count() > 0) {
                typing = false;
                continue;
            }
            if (!typing && key == ConsoleKey.Enter) {
                selectedMedia = results[selectedResult];
                if (selectedMedia is Serie) {
                    List<string>? allSelectedEpisodes = SelectEpisodes((Serie)selectedMedia);
                    if (allSelectedEpisodes == null) {
                        continue;
                    }else{
                        Dictionary<Serie, List<Episode>> uwu = new Dictionary<Serie, List<Episode>>();
                        uwu.Add((Serie)selectedMedia, new List<Episode>());
                        for(int i=0;i<allSelectedEpisodes.Count;i++){
                            int season = Convert.ToInt32(allSelectedEpisodes[i].Split('.')[0]);
                            int episode = Convert.ToInt32(allSelectedEpisodes[i].Split('.')[1]);
                            uwu[(Serie)selectedMedia].Add(((Serie)selectedMedia).Seasons[season].Episodes[episode]);
                        }
                        Console.Clear();
                        return uwu;
                    }
                } else if (selectedMedia is Film) {
                    break;
                }
            }
            if(key == ConsoleKey.Escape){
                Console.Clear();
                return null;
            }
            selectedResult = Math.Clamp(selectedResult, 0, Math.Max(0, Math.Min(results.Count(), 5)-1));
            cursorPosition = Math.Clamp(cursorPosition, 0, Math.Max(0, searchString.Length));
        }while(true);
        Console.Clear();
        return selectedMedia;
    }

    /// <summary>
    /// Asks the user to select episodes and returns a list of the episodes the user selected.
    /// </summary>
    /// <param name="serie">A Serie object to select episodes from</param>
    /// <returns>null if the user doesnt select anything. A list of strings containing the user selected episodes formatted as: "1.1" being season 1 episode 1.</returns>
    public static List<string>? SelectEpisodes(Serie serie){
        bool selectSeason = true;
        bool saving = false;
        int selectedSeason = 0;
        int selectedEpisode = 0;
        List<Season> allSeasons = serie.Seasons;
        List<Episode> selectedEpisodes = new List<Episode>();
        ConsoleKey key;
        do{
            List<Episode> allEpisodes = allSeasons[selectedSeason].Episodes;

            int longestSeasonName = "Select season".Length;
            int longestEpisodeName = "Select Episode".Length;

            // calculate season/episode name length
            foreach(Season s in allSeasons){
                if (s.Title.Length+3 > longestSeasonName){
                    longestSeasonName = s.Title.Length+3;
                }
            }
            foreach(Episode e in allEpisodes){
                if (e.Title.Length+3 > longestEpisodeName){
                    longestEpisodeName = e.Title.Length+3;
                }
            }

            List<string> seasons = new List<string>();
            List<string> episodes = new List<string>();
            for(int i=0;i<allSeasons.Count;i++){
                Season s = allSeasons[i];
                for(int j=0;j<s.Episodes.Count;j++){
                    Episode e = s.Episodes[j];
                    bool add = false;
                    foreach(Episode se in selectedEpisodes){
                        if (se == e){ add = true; }
                    }
                    if(add){episodes.Add($"{i+1}.{j+1}");}
                    if(add && !seasons.Contains($"{i+1}")){seasons.Add($"{i+1}");}
                }
            }

            Console.Clear();
            Console.Write($"Series: {serie.Title}\nSeasons: ");
            for(int i=0;i<seasons.Count;i++){
                if(i == seasons.Count-1){
                    Console.Write($"{seasons[i]}\n");
                }else{
                    Console.Write($"{seasons[i]}, ");
                }
            }
            Console.Write($"Episodes: ");
            for(int i=0;i<episodes.Count;i++){
                if(i == episodes.Count-1){
                    Console.Write($"{episodes[i]}\n");
                }else{
                    Console.Write($"{episodes[i]}, ");
                }
            }
            Console.Write($"\n\n┌─Select season{new string('─', Math.Max(0, longestSeasonName-13))}─┐ -> ┌─Select Episode{new string('─', Math.Max(0, longestEpisodeName-14))}─┐\n");
            for(int i=0;i<Math.Max(allSeasons.Count+2, allEpisodes.Count+1);i++){
                if(i == allSeasons.Count+1){
                    Console.Write($"└─{new string('─', Math.Max(0, longestSeasonName))}─┘    ");
                }else if(i == allSeasons.Count){
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"│ ");
                    Console.BackgroundColor = (saving) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($"{i+1}. Select");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{new string(' ', Math.Max(0, longestSeasonName-(i+1).ToString().Length-8))} │    ");
                }else if(i < allSeasons.Count){
                    Season s = allSeasons[i];
                    int seasonTextLength = (i+1).ToString().Length + 2 + s.Title.Length;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"│ ");
                    Console.BackgroundColor = (!saving && s == allSeasons[selectedSeason]) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    bool hasAllSeasons = allSeasons[i].Episodes.All(e => selectedEpisodes.Contains(e));
                    if(hasAllSeasons){
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.Write($"{i+1}. {s.Title}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{new string(' ', Math.Max(0, longestSeasonName-seasonTextLength))} │    ");
                }else{
                    Console.Write($"{new string(' ', longestSeasonName+8)}");
                }
                if(i < allEpisodes.Count){
                    Episode e = allEpisodes[i];
                    int episodeTextLength = (i+1).ToString().Length + 2 + e.Title.Length;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"│ ");
                    Console.BackgroundColor = (!selectSeason && e == allSeasons[selectedSeason].Episodes[selectedEpisode]) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    if(selectedEpisodes.Contains(e)){
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.Write($"{i+1}. {e.Title}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{new string(' ', Math.Max(0, longestEpisodeName-episodeTextLength))} │\n");
                }else if(i == allEpisodes.Count){
                    Console.Write($"└─{new string('─', Math.Max(0, longestEpisodeName))}─┘\n");
                }else{
                    Console.Write("\n");
                }
            }
            Console.Write("\nPress escape to cancel.\n");
            key = Console.ReadKey(true).Key;
            if(key == ConsoleKey.Escape){
                return null;
            }
            if(key == ConsoleKey.LeftArrow){
                selectSeason = true;
            }else if(key == ConsoleKey.RightArrow && !saving){
                selectSeason = false;
            }
            if(key == ConsoleKey.UpArrow){
                if(selectSeason && saving){
                    saving = false;
                    continue;
                }
                if (selectSeason) {
                    selectedEpisode = 0;
                    selectedSeason--;
                }else{
                    selectedEpisode--;
                }
            }else if(key == ConsoleKey.DownArrow){
                if(!saving && selectedSeason == allSeasons.Count-1 && selectSeason){
                    saving = true;
                    continue;
                }
                if (selectSeason) {
                    selectedEpisode = 0;
                    selectedSeason++;
                }else{
                    selectedEpisode++;
                }
            }
            if(key == ConsoleKey.Enter){
                if(saving){
                    break;
                }else if(!selectSeason){
                    // add specific episode
                    if (selectedEpisodes.Contains(allSeasons[selectedSeason].Episodes[selectedEpisode])){
                        selectedEpisodes.Remove(allSeasons[selectedSeason].Episodes[selectedEpisode]);
                    }else{
                        selectedEpisodes.Add(allSeasons[selectedSeason].Episodes[selectedEpisode]);
                    }
                }else{
                    bool hasAllSeasons = allSeasons[selectedSeason].Episodes.All(e => selectedEpisodes.Contains(e));
                    // add/remove whole season
                    if(!hasAllSeasons){
                        foreach (Episode e in allSeasons[selectedSeason].Episodes){
                            selectedEpisodes.Remove(e);
                        }
                        foreach (Episode e in allSeasons[selectedSeason].Episodes){
                            selectedEpisodes.Add(e);
                        }
                    }else{
                        foreach (Episode e in allSeasons[selectedSeason].Episodes){
                            selectedEpisodes.Remove(e);
                        }
                    }
                }
            }
            selectedSeason = Math.Clamp(selectedSeason, 0, Math.Max(0, allSeasons.Count-1));
            selectedEpisode = Math.Clamp(selectedEpisode, 0, Math.Max(0, allSeasons[selectedSeason].Episodes.Count-1));
        } while(true);

        if(selectedEpisodes.Count == 0){
            return null;
        }else{
            List<string> episodes = new List<string>();
            for(int i=0;i<allSeasons.Count;i++){
                Season s = allSeasons[i];
                for(int j=0;j<s.Episodes.Count;j++){
                    Episode e = s.Episodes[j];
                    bool add = false;
                    foreach(Episode se in selectedEpisodes){
                        if (se == e){ add = true; }
                    }
                    if(add){episodes.Add($"{i}.{j}");}
                }
            }
            return episodes;
        }
    }
    #endregion
}
