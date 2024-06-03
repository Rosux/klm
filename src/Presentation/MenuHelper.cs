using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using TimeLine;

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

            Console.CursorVisible = false;
            Console.Clear();
            Console.Write($"{prefix}\n\n{num}\n{error}\n\n{keybinds}\n{suffix}");
            RawKey = Console.ReadKey(true);
            key = RawKey.Key;

            // add number to string
            if (char.IsDigit(RawKey.KeyChar) && int.TryParse(inputNum+RawKey.KeyChar, out int x))
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
    #endregion

    #region Text
    /// <summary>
    /// Asks the user to type in a string and returns that value.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
    /// <param name="minimumLength">The minimum amount of characters required.</param>
    /// <param name="maximumLength">The maximum amount of characters required.</param>
    /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([A-z]| )" making it accept all letters and spaces.</param>
    /// <returns>A string chosen by the user or null if the user canceled the process.</returns>
    public static string? SelectText(string prefix="", string suffix="", bool canCancel=false, int minimumLength=0, int maximumLength=int.MaxValue, string allowedRegexPattern="([A-z]| )")
    {
        string input = "";
        string errorMessage = "";
        string keybinds = "Press Enter to confirm";
        if (canCancel){keybinds += "\nPress Escape to cancel";}
        ConsoleKey key;
        char keyChar;
        do
        {
            errorMessage = "";
            if (input.Length < minimumLength || input.Length > maximumLength)
            {
                errorMessage += $"Text must be between {minimumLength} and {maximumLength} characters\n";
            }
            Console.CursorVisible = false;
            Console.Clear();
            Console.Write($"{prefix}\n\n{input}\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{errorMessage}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\n\n{keybinds}\n{suffix}");
            ConsoleKeyInfo uwu = Console.ReadKey(true);
            key = uwu.Key;
            keyChar = uwu.KeyChar;

            if(Regex.IsMatch(keyChar.ToString(), allowedRegexPattern)){
                input += keyChar;
            }
            if (key == ConsoleKey.Backspace && input.Length > 0){
                input = input.Remove(input.Length-1);
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
    /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([A-z]| )" making it accept all letters and spaces.</param>
    /// <returns>A string chosen by the user or null if the user canceled the process.</returns>
    public static string? SelectText(string prefix="", bool canCancel=false, string allowedRegexPattern="([A-z]| )")
    {
        return SelectText(prefix, "", canCancel, 0, int.MaxValue, allowedRegexPattern);
    }

    /// <summary>
    /// Asks the user to type in a string and returns that value.
    /// </summary>
    /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
    /// <param name="minimumLength">The minimum amount of characters required.</param>
    /// <param name="maximumLength">The maximum amount of characters required.</param>
    /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([A-z]| )" making it accept all letters and spaces.</param>
    /// <returns>A string chosen by the user or null if the user canceled the process.</returns>
    public static string? SelectText(bool canCancel=false, int minimumLength=0, int maximumLength=int.MaxValue, string allowedRegexPattern="([A-z]| )")
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
    /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([A-z]| )" making it accept all letters and spaces.</param>
    /// <returns>A string chosen by the user.</returns>
    public static string SelectText(string prefix="", string allowedRegexPattern="([A-z]| )")
    {
        return SelectText(prefix, "", false, 0, int.MaxValue, allowedRegexPattern);
    }

    /// <summary>
    /// Asks the user to type in a string and returns that value.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="minimumLength">The minimum amount of characters required.</param>
    /// <param name="maximumLength">The maximum amount of characters required.</param>
    /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([A-z]| )" making it accept all letters and spaces.</param>
    /// <returns>A string chosen by the user.</returns>
    public static string SelectText(string prefix="", int minimumLength=0, int maximumLength=int.MaxValue, string allowedRegexPattern="([A-z]| )")
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
    /// <param name="allowedRegexPattern">A string containing a regex pattern of allowed characters the user is allowed to use. The default is "([A-z]| )" making it accept all letters and spaces.</param>
    /// <returns>A string chosen by the user.</returns>
    public static string SelectText(int minimumLength=0, int maximumLength=int.MaxValue, string allowedRegexPattern="([A-z]| )")
    {
        return SelectText("", "", false, minimumLength, maximumLength, allowedRegexPattern);
    }
    #endregion

    #region Date
    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <param name="canCancel">A boolean indicating if the user can cancel the selection.</param>
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
            Console.CursorVisible = false;
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
        return SelectDate("", "", canCancel, defaultTime, minDate, maxDate) ?? null;
    }

    /// <summary>
    /// Ask the user to select a date between specified values and returns the chosen date.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="canCancel">A boolean indicating if the user can cancel the selection.</param>
    /// <returns>An DateOnly object with the date chosen by the user.</returns>
    public static DateOnly? SelectDate(string prefix="", bool canCancel=false){
        return SelectDate(prefix, "", canCancel, null, null, null) ?? null;
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
    /// <param name="CanCancel">A boolean indicating if the user can stop the select process.</param>
    /// <param name="defaultTime">A TimeOnly object as starting point for the user.</param>
    /// <param name="minTime">The minimum allowed selected time.</param>
    /// <param name="maxTime">The maximum allowed selected time.</param>
    /// <returns>A TimeOnly object containing the user selected time or null if the user cances the process.</returns>
    public static TimeOnly? SelectTime(string prefix = "", string suffix = "", bool CanCancel=false, TimeOnly defaultTime = new TimeOnly(), TimeOnly? minTime = null, TimeOnly? maxTime = null){
        TimeOnly MinTime = minTime ?? TimeOnly.MinValue;
        TimeOnly MaxTime = maxTime ?? TimeOnly.MaxValue;
        if(defaultTime <= MinTime){
            defaultTime = MinTime;
        }
        TimeOnly time = defaultTime;
        bool hour = true;
        ConsoleKey key;
        do{
            Console.CursorVisible = false;
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
            if (key == ConsoleKey.Escape && CanCancel)
            {
                Console.CursorVisible = false;
                Console.Clear();
                return null;
            }
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
        Console.CursorVisible = false;
        Console.Clear();
        return time;
    }

    /// <summary>
    /// Shows a select time to user as: 00:00 and lets the user select a specific time in HH:MM format.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <param name="defaultTime">A TimeOnly object as starting point for the user.</param>
    /// <param name="minTime">The minimum allowed selected time.</param>
    /// <param name="maxTime">The maximum allowed selected time.</param>
    /// <returns>A TimeOnly object containing the user selected time.</returns>
    public static TimeOnly SelectTime(string prefix = "", string suffix = "", TimeOnly defaultTime = new TimeOnly(), TimeOnly? minTime = null, TimeOnly? maxTime = null){
        return SelectTime(prefix, suffix, false, defaultTime, minTime, maxTime) ?? TimeOnly.MinValue;
    }

    /// <summary>
    /// Shows a select time to user as: 00:00 and lets the user select a specific time in HH:MM format.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <returns>A TimeOnly object containing the user selected time.</returns>
    public static TimeOnly SelectTime(string prefix = ""){
        return SelectTime(prefix, "", false, TimeOnly.MinValue, null, null) ?? TimeOnly.MinValue;
    }

    /// <summary>
    /// Shows a select time to user as: 00:00 and lets the user select a specific time in HH:MM format.
    /// </summary>
    /// <param name="defaultTime">A TimeOnly object as starting point for the user.</param>
    /// <returns>A TimeOnly object containing the user selected time.</returns>
    public static TimeOnly SelectTime(TimeOnly defaultTime = new TimeOnly()){
        return SelectTime("", "", false, defaultTime, null, null) ?? TimeOnly.MinValue;
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
    #endregion

    #region Options from a Dict
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
            Console.CursorVisible = false;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"Search by Titles, Genres or Directors seperated by commas.\nPress escape to cancel.\n\n┌─Start typing to search{new string('─', Math.Max(0, longestWord-22))}─┐\n");
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
                        Console.CursorVisible = false;
                        Console.Clear();
                        return uwu;
                    }
                } else if (selectedMedia is Film) {
                    break;
                }
            }
            if(key == ConsoleKey.Escape){
                Console.CursorVisible = false;
                Console.Clear();
                return null;
            }
            selectedResult = Math.Clamp(selectedResult, 0, Math.Max(0, Math.Min(results.Count(), 5)-1));
            cursorPosition = Math.Clamp(cursorPosition, 0, Math.Max(0, searchString.Length));
        }while(true);
        Console.CursorVisible = false;
        Console.Clear();
        return selectedMedia;
    }

    /// <summary>
    /// Ask the user to select only a movie.
    /// </summary>
    /// <returns>null if the user cancels the search. If a movie is selected it will return a Movie object.</returns>
    public static Film? SelectMovie(){
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
            List<Media> results = searchAccess.Search(searchString, true);
            longestWord = "Start typing to search".Length + 2;
            foreach(Media m in results){
                if (m is Film && ((Film)m).Title.Length+3 > longestWord){
                    longestWord = ((Film)m).Title.Length + 3;
                }
            }
            if (searchString.Length + 2 > longestWord){
                longestWord = searchString.Length + 2;
            }

            // print the search box thing
            Console.CursorVisible = false;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"Search by Titles, Genres or Directors seperated by commas.\nPress escape to cancel.\n\n┌─Start typing to search{new string('─', Math.Max(0, longestWord-22))}─┐\n");
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
                break;
            }
            if(key == ConsoleKey.Escape){
                Console.CursorVisible = false;
                Console.Clear();
                return null;
            }
            selectedResult = Math.Clamp(selectedResult, 0, Math.Max(0, Math.Min(results.Count(), 5)-1));
            cursorPosition = Math.Clamp(cursorPosition, 0, Math.Max(0, searchString.Length));
        }while(true);
        Console.CursorVisible = false;
        Console.Clear();
        return (Film)selectedMedia;
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
            Console.CursorVisible = false;
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

    #region Price
    /// <summary>
    /// Ask the user to select a price and return the chosen price.
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
    /// <returns>A double containing the user chosen price or null if the user chose to cancel the process.</returns>
    public static double? SelectPrice(string prefix="", string suffix="", bool canCancel=false)
    {
        string input = "";
        double price;
        string keybinds = "Press Enter to confirm";
        if(canCancel){keybinds += "\nPress Escape to cancel";}

        ConsoleKey key;
        ConsoleKeyInfo RawKey;
        do
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.Write($"{prefix}\n\n{input}\n\n{keybinds}\n{suffix}");
            RawKey = Console.ReadKey(true);
            key = RawKey.Key;

            if(key == ConsoleKey.Backspace && input.Length > 0){
                input = input.Remove(input.Length-1);
            }
            if(key == ConsoleKey.Escape && canCancel){
                Console.CursorVisible = false;
                Console.Clear();
                return null;
            }
            if(key == ConsoleKey.Enter && double.TryParse(input, out price)){
                break;
            }
            if((RawKey.KeyChar == ',' || RawKey.KeyChar == '.') && !input.Contains(',')){
                if(input.Length == 0){
                    input += "0,";
                }else{
                    input += ",";
                }
            }
            if(char.IsDigit(RawKey.KeyChar)){
                if(input.Contains(',') && input.Length >= 3 && input[input.Length-3] == ','){
                    continue;
                }
                input += RawKey.KeyChar.ToString();
            }

        }while(true);
        Console.CursorVisible = false;
        Console.Clear();
        return price;
    }

    public static double? SelectPrice(string prefix="", string suffix="", bool canCancel=false, double minimumPrice=double.MinValue, double maximumPrice=double.MaxValue)
    {
        string input = "";
        string error = "";
        double price = minimumPrice;
        string keybinds = "Press Enter to confirm";
        if(canCancel){keybinds += "\nPress Escape to cancel";}

        ConsoleKey key;
        ConsoleKeyInfo RawKey;
        do
        {
            if (price < minimumPrice || price > maximumPrice)
            {
                error = $"Please select a value between {minimumPrice} and {maximumPrice}";
            }
            else
            {
                error = "";
            }
            Console.Clear();
            Console.Write($"{prefix}\n\n{input}\n{error}\n\n{keybinds}\n{suffix}");
            RawKey = Console.ReadKey(true);
            key = RawKey.Key;

            if(key == ConsoleKey.Backspace && input.Length > 0){
                input = input.Remove(input.Length-1);
            }
            if(key == ConsoleKey.Escape && canCancel){
                Console.Clear();
                return null;
            }
            if(key == ConsoleKey.Enter && double.TryParse(input, out price) && price >= minimumPrice && price <= maximumPrice){
                break;
            }
            if((RawKey.KeyChar == ',' || RawKey.KeyChar == '.') && !input.Contains(',')){
                if(input.Length == 0){
                    input += "0,";
                }else{
                    input += ",";
                }
            }
            if(char.IsDigit(RawKey.KeyChar)){
                if(input.Contains(',') && input.Length >= 3 && input[input.Length-3] == ','){
                    continue;
                }
                input += RawKey.KeyChar.ToString();
            }

        }while(true);
        Console.Clear();
        return price;
    }
    #endregion

    #region Confirmation
    /// <summary>
    /// Gives the user 2 options (true and false) and makes them choose one of these options.
    /// </summary>
    /// <param name="prompt">A string containing the question to confirm.</param>
    /// <returns>A boolean indicating if the user confirms or not.</returns>
    public static bool Confirm(string prompt=""){
        bool selection = false;
        ConsoleKey key;
        do
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.Write($"{prompt}\n\n");
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
    #endregion

    #region TimeLine
    /// <summary>
    /// Prints the timeline movies/episodes given.
    /// </summary>
    /// <param name="prefix">A string of text printed before the timeline.</param>
    /// <param name="suffix">A string of text printed after the timeline.</param>
    /// <param name="t">A List of timeline items to display.</param>
    public static void PrintTimeLine(string prefix, string suffix, List<TimeLine.Item> t)
    {
        List<string> Dates = new List<string>();
        List<DateTime> Times = new List<DateTime>();
        List<string> Action = new List<string>();
        t = t.OrderBy(o=>o.StartTime).ToList();
        List<TimeLine.Item> watchables = new List<TimeLine.Item>();
            foreach(var x in t){
                if(x.Action is Episode || x.Action is Film){
                    watchables.Add(x);
                }
            }
            // Foreach loop to check what the action is in the timeline and prints that out.
            for(int i = 0; i < watchables.Count; i++)
            {
                TimeLine.Item item = watchables[i];
                if(item.Action is Episode || item.Action is Film){
                    DateTime StartTimeString = item.StartTime;
                    DateTime EndTimeString = item.EndTime;
                    if (i > 0)
                    {
                        if(watchables[i-1].EndTime != watchables[i].StartTime)
                        {
                            Times.Add(StartTimeString);
                        }
                    }else
                    {
                        Times.Add(StartTimeString);
                    }
                    Times.Add(EndTimeString);
                    if (i > 0)
                    {
                        if (watchables[i - 1].EndTime < item.StartTime)
                        {
                            Action.Add("");
                            Dates.Add("");
                        }
                    }
                    if(item.Action is Film)
                    {
                        Action.Add(((Film)item.Action).Title);
                        Dates.Add(item.StartTime.Date.ToString("yyyy-MM-dd"));
                    }
                    else if(item.Action is Episode)
                    {
                        Action.Add(((Episode)item.Action).Title);
                        Dates.Add(item.StartTime.Date.ToString("yyyy-MM-dd"));
                    }
                }
            }
            if(watchables.Count == 0)
            {
                Console.CursorVisible = false;
                Console.Clear();
                Console.WriteLine("\nNo Movies/Series were added\n\nPress any key to return");
                Console.ReadKey(true);
            }else{
                string Line1 = "";
                string Line2 = "";
                string Line3 = "";
                string Line4 = "";
                Console.CursorVisible = false;
                Console.Clear();
                Line1 += ($"  ");
                for(int i=0;i<Dates.Count;i++){
                    if(i == 0){
                        Line1 += ($"{Dates[i]}{new string(' ', Math.Max(0, 3+Math.Max(10, Action[i].Length)-Dates[i].Length))}");
                    }else if(Dates[i] == "" || Action[i] == ""){
                        Line1 += ($"{new string(' ', 6)}");
                    }else if(Dates[i-1] == Dates[i]){
                        Line1 += ($"{new string(' ', Math.Max(10, Action[i].Length)+3)}");
                    }else{
                        Line1 += ($"{Dates[i]}{new string(' ', Math.Max(0, 3+Math.Max(10, Action[i].Length)-Dates[i].Length))}");
                    }
                }
                Line2 += ("  ├");
                for(int i=0;i<Action.Count;i++)
                {
                    if(Action[i] == "")
                    {
                        Line2 += ($"{new string('─', 5)}┼");
                    }
                    else
                    {
                        Line2 += ($"{new string('─', Math.Max(12, 2 + Action[i].Length))}");
                        Line2 += (i == Action.Count - 1 ? "┤" : "┼");
                    }
                }
                int actionId = 0;
                for(int i=0;i<Times.Count;i++)
                {
                    if(Action[actionId] == ""){
                        Line3 += ($"{Times[i].ToString("HH:mm")}{new string(' ', 1)}");
                    }else{
                        Line3 += ($"{Times[i].ToString("HH:mm")}{new string(' ', Math.Max(0, Math.Max(10, Action[actionId].Length))-2)}");
                    }
                    actionId++;
                    actionId = Math.Clamp(actionId, 0, Action.Count-1);
                }
                Line4 += ("  |");
                for(int i=0;i<Action.Count;i++)
                {
                    if(Action[i] == "")
                    {
                        Line4 += ($" {new string(' ', 3)} |");
                    }
                    else
                    {
                        Line4 += ($" {Action[i]}{new string(' ', Math.Max(0, 10-Action[i].Length))} |");
                    }
                }


                int scrollAmount = 0;
                string[] Lines = {Line1, Line2, Line3, Line4};
                ConsoleKey key;
                do{
                    Console.CursorVisible = false;
                    Console.Clear();
                    Console.Write($"{prefix}\n\n");
                    foreach(string Line in Lines){
                        string L = Line;
                        L = L.Substring(scrollAmount, Math.Min(Console.WindowWidth/2, L.Length - scrollAmount));
                        Console.WriteLine(L);
                    }
                    Console.Write($"{suffix}\n\n");
                    key = Console.ReadKey(true).Key;
                    if(key == ConsoleKey.LeftArrow){
                        scrollAmount -= 5;
                    }
                    if(key == ConsoleKey.RightArrow){
                        scrollAmount += 5;
                    }
                    scrollAmount = Math.Clamp(scrollAmount, 0, Lines.Min(line => line.Length)-5);
                }while(key != ConsoleKey.Escape);
            }
        }
    #endregion

    #region Table
    /// <summary>
    /// Calculate the width of each column based on the longest value or header of that column.
    /// </summary>
    /// <typeparam name="T">The type of item to get the properties from.</typeparam>
    /// <param name="items">A list of items where the longest word gets generated from</param>
    /// <param name="headers">A dictionary representing the headers of the table. Each key represents a column name, and the corresponding value is the property/field for that the columns data.</param>
    /// <returns>A list of integers representing the maximum width for each column in characters.</returns>
    private static List<int> GetMaxColumnWidths<T>(List<T> items, Dictionary<string, Func<T, object>> headers){
        List<int> columnWidths = new List<int>();
        // loop over all items
        int columnIndex = 0;
        foreach(KeyValuePair<string, Func<T, object>> header in headers){
            string headerName = header.Key;
            columnWidths.Add(headerName.Length);
            for(int i=0;i<items.Count;i++){
                string propertyValue = header.Value(items[i]).ToString() ?? "";
                if(propertyValue.Length >= columnWidths[columnIndex]){
                    columnWidths[columnIndex] = propertyValue.Length;
                }
            }
            columnIndex++;
        }
        return columnWidths;
    }

    /// <summary>
    /// Creates a table that holds a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of object that the table will handle.</typeparam>
    /// <param name="items">A list of type T that holds all the objects</param>
    /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> being a method that gets a type T object and returns an object of any type.</param>
    /// <param name="canSelect">A boolean indicating if the user is able to select a row. The item on this row will be returned.</param>
    /// <param name="canCancel">A boolean indicating if the user can press escape to cancel everything and return back.</param>
    /// <param name="canEdit">A boolean indicating if the user is able to edit properties of the items.</param>
    /// <param name="propertyEditMapping">A Dictionary where the key is the editing text and the value being a PropertyEditMapping instance of type T that holds a Func<T, object> that returns a member type of T and a Func<T, object> that is a method that will take the user object and returns an object being the new member of type T.</param>
    /// <param name="saveEditedUserMethod">A Func<T, bool> that takes in the newly edited T object and returns a boolean indicating if it saved or not.</param>
    /// <param name="canAdd">A boolean indicating if the user can add a new object of type T. If the user chooses to make a new object of type T it will call the addMethod.</param>
    /// <param name="addMethod">A Func<T?> that creates a new instance of object T or NULL and returns it. If the result is NULL the new instance wont get saved. If the result is the new object it gets added to the table.</param>
    /// <param name="canDelete">A boolean indicating if the user can delete the item in the list. Uses the deleteMethod to delete the instance.</param>
    /// <param name="deleteMethod">A Func<T, bool> that takes in the selected object T and returns a boolean indicating if the object should be removed from the table.</param>
    /// <returns>NULL if the canSelect is false. If canSelect is true it can either return NULL in case the user presses escape OR it returns an object of type T which the user selected.</returns>
    public static T? Table<T>(List<T> items, Dictionary<string, Func<T, object>> headers, bool canSelect, bool canCancel, bool canEdit, Dictionary<string, PropertyEditMapping<T>>? propertyEditMapping, Func<T, bool>? saveEditedUserMethod, bool canAdd, Func<T?>? addMethod, bool canDelete, Func<T, bool>? deleteMethod)
    {
        if(propertyEditMapping == null || saveEditedUserMethod == null){
            canEdit = false;
        }
        if(addMethod == null){
            canAdd = false;
        }
        if(deleteMethod == null){
            canDelete = false;
        }
        if(canSelect){
            canDelete = false;
            canEdit = false;
            canAdd = false;
        }

        bool showSelection = true;
        if(!canSelect && !canEdit && !canAdd && !canDelete){
            showSelection = false;
        }

        // create a list of editable options (like UserName, Email, role etc)
        List<string> editOptions = new List<string>();
        if(canEdit){
            foreach(KeyValuePair<string, PropertyEditMapping<T>> editMapping in propertyEditMapping){
                editOptions.Add(editMapping.Key);
            }
        }

        // if you can edit the data and ur not a user prevent editing (failsafe for incompetent developers in case a user ever uses this method (jk jk not incompetent))
        if((canEdit || canDelete || canAdd) && Program.CurrentUser != null && Program.CurrentUser.Role == UserRole.USER){
            return default(T);
        }

        Dictionary<MemberInfo, (object, object)> propertyUpdate = new Dictionary<MemberInfo, (object, object)>();
        T editedObject = default(T);
        bool showEditTable = (canEdit || canDelete);
        bool editing = false;
        int editSelection = 0;
        int currentPageSelection = 0;
        int currentPage = 0;
        ConsoleKey key;
        do
        {
            #region Generate pages
            List<List<T>> chunks = new List<List<T>>();
            for (int i=0;i<items.Count;i+=10)
            {
                chunks.Add(items.Skip(i).Take(10).ToList());
            }
            int maxPage = chunks.Count-1;
            if(currentPage > maxPage){
                currentPage--;
            }
            #endregion

            #region Create edit table values (": oldData -> newData")
            List<string> editOptionData = new List<string>();
            if(showEditTable && chunks.Count > 0){
                // create the edit table text
                if(canEdit){
                    foreach(KeyValuePair<string, PropertyEditMapping<T>> editMapping in propertyEditMapping){
                        Func<T, object> editMappingValueLambda = editMapping.Value.Accessor.Compile();
                        string dataString = editMappingValueLambda(chunks[currentPage][currentPageSelection]).ToString() ?? "";
                        if(editing){

                            string propertyName = "";
                            if(editMapping.Value.Accessor.Body is MemberExpression bodyMember){
                                propertyName = bodyMember.Member.Name;
                            }else if(editMapping.Value.Accessor.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember){
                                propertyName = unaryMember.Member.Name;
                            }

                            foreach(KeyValuePair<MemberInfo, (object, object)> kvp in propertyUpdate){
                                if(kvp.Key.Name.ToString() == propertyName){
                                    (object oldData, object newData) = kvp.Value;
                                    if(oldData.ToString() != newData.ToString()){
                                        dataString = $"{oldData} -> {newData}";
                                    }
                                }
                            }

                        }
                        editOptionData.Add(": "+dataString);
                    }
                }

            }
            #endregion

            #region Calculate lengths of tables/strings
            // get the width of each column
            List<int> columnWidths = GetMaxColumnWidths(items, headers);

            // get the total table width (for alignment purposes and ease of use)
            // │ Firstname │ LongestName │ LongestEmail │\n
            // ^^=2       ^^^=3         ^^^ = 3        ^^=2
            // ............--------------...............=
            int totalWidth = 1; // 1 since the end of the line has a vertical bar
            foreach(int textLength in columnWidths){
                totalWidth += 2 + textLength + 1; // 2 is the 2 spaces before the text (being "| ") and then the max text length (columnWidths) and then 1 for the space after the text (in "| text |" it would be " |"<- this one)
            }

            // calculate the longest edit table options
            int longestEditOption = 0;
            if(showEditTable && chunks.Count > 0){
                for(int i=0;i<editOptions.Count;i++){
                    if(editOptionData[i].Length+editOptions[i].Length > longestEditOption){longestEditOption = editOptionData[i].Length+editOptions[i].Length;}
                }
                if($"Edit {typeof(T)}".Length > longestEditOption){longestEditOption = $"Edit {typeof(T)}".Length;}
                if("Save changes".Length > longestEditOption){longestEditOption = "Save changes".Length;}
                if("Discard changes".Length > longestEditOption){longestEditOption = "Discard changes".Length;}
            }
            if(canDelete){
                if($"Delete {typeof(T)}".Length > longestEditOption){longestEditOption = $"Delete {typeof(T)}".Length;}
            }
            #endregion

            #region Table String List
            // create the page arrows
            string pageArrows = $"{currentPage+1}/{maxPage+1}";
            // 8 in the next line stands for "| <-" and "-> |" totaling to 8 characters
            for(int i=Math.Max(0, totalWidth-pageArrows.Length - 8);i>0;i--){
                pageArrows = ((i % 2 == 1) ? " " : "") + pageArrows + ((i % 2 == 0) ? " " : "");
            }
            pageArrows = (currentPage > 0 ? "│ <-" : "│   ") + pageArrows + (currentPage < maxPage ? "-> │" : "   │");

            List<string> tableStringLines = new List<string>();

            // prints the top line
            tableStringLines.Add($"┌{Format('─', totalWidth-2)}┐");

            // prints the <- pagenumber ->
            tableStringLines.Add($"{pageArrows}");

            // prints the seperator between page number and headers
            string seperatorLine1 = "";
            for(int i=0;i<headers.Keys.ToList().Count;i++){
                // the +2 is for a space at both sides
                seperatorLine1 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '┬')}";
            }
            tableStringLines.Add(seperatorLine1);

            // prints the headers
            string headerLine = "";
            for(int i=0;i<headers.Keys.ToList().Count;i++){
                string headerText = headers.Keys.ToList()[i];
                headerLine += $"{(i==0 ? '│' : "")} {Format(headerText, columnWidths[i])} │";
            }
            tableStringLines.Add(headerLine);

            // prints the seperator between headers and data
            string seperatorLine2 = "";
            for(int i=0;i<headers.Keys.ToList().Count;i++){
                // the +2 is for a space at both sides
                if(chunks.Count > 0){
                    seperatorLine2 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '┼')}";
                }else{
                    seperatorLine2 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '┴')}";
                }
            }
            tableStringLines.Add(seperatorLine2);

            // prints the data
            if(chunks.Count > 0){
                for(int i=0;i<chunks[currentPage].Count;i++){ // loop over all items (like all Users)
                    string dataLine = "";
                    dataLine += "│";
                    int j = 0;
                    foreach(Func<T, object> header in headers.Values){
                        string itemText = header(chunks[currentPage][i]).ToString();
                        dataLine += $" {Format(itemText, columnWidths[j])} │";
                        j++;
                    }
                    tableStringLines.Add(dataLine);
                }
            }else{
                tableStringLines.Add($"│ {Center("No row found", totalWidth-4)} │");
            }

            if(canAdd){
                // prints the seperator line
                string seperatorLine3 = "";
                for(int i=0;i<headers.Keys.ToList().Count;i++){
                    // the +2 is for a space at both sides
                    if(chunks.Count > 0){
                        seperatorLine3 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '┴')}";
                    }else{
                        seperatorLine3 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '─')}";
                    }
                }
                tableStringLines.Add(seperatorLine3);

                // prints the Add T line
                tableStringLines.Add($"│ {Format($"Add new {typeof(T)}", totalWidth-4)} │");

                // print the bottom line
                // the +2 is for a space at both sides
                tableStringLines.Add($"└{new string('─', totalWidth-2)}┘");
            }else{
                string bottomLine = "";
                for(int i=0;i<headers.Keys.ToList().Count;i++){
                    // the +2 is for a space at both sides
                    if(chunks.Count > 0){
                        bottomLine += $"{(i==0 ? '└' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┘' : '┴')}";
                    }else{
                        bottomLine += $"{(i==0 ? '└' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┘' : '─')}";
                    }
                }
                tableStringLines.Add(bottomLine);
            }

            #endregion

            #region Edit string list
            List<string> editStringLines = new List<string>();
            if(showEditTable && canEdit && chunks.Count > 0){
                editStringLines.Add($"┌─Edit {typeof(T)}{new string('─', Math.Max(0, longestEditOption-$"Edit {typeof(T)}".Length))}─┐");
                for(int i=0;i<Math.Max(editOptionData.Count, editOptions.Count);i++){
                    string editLine = editOptions[i];
                    string editDataLine = editOptionData[i];
                    editStringLines.Add($"│ {editLine}{editDataLine}{new string(' ', Math.Max(0, longestEditOption-(editLine.Length+editDataLine.Length)))} │");
                }
                editStringLines.Add($"├─{new string('─', Math.Max(0, longestEditOption))}─┤");
                editStringLines.Add($"│ Save changes{new string(' ', Math.Max(0, longestEditOption-"Save changes".Length))} │");
                editStringLines.Add($"│ Discard changes{new string(' ', Math.Max(0, longestEditOption-"Discard changes".Length))} │");
                if(canDelete){
                    editStringLines.Add($"├{Format('─', longestEditOption+2)}┤");
                    editStringLines.Add($"│ Delete {typeof(T)}{new string(' ', Math.Max(0, longestEditOption-$"Delete {typeof(T)}".Length))} │");
                }
                editStringLines.Add($"└─{new string('─', Math.Max(0, longestEditOption))}─┘");
            }else if(showEditTable && canDelete){
                editStringLines.Add($"┌─Edit {typeof(T)}{new string('─', Math.Max(0, longestEditOption-$"Edit {typeof(T)}".Length))}─┐");
                editStringLines.Add($"│ Delete {typeof(T)}{new string(' ', Math.Max(0, longestEditOption-$"Delete {typeof(T)}".Length))} │");
                editStringLines.Add($"└─{new string('─', Math.Max(0, longestEditOption))}─┘");

            }
            #endregion

            #region Print table
            // clear the screen and print the table
            Console.CursorVisible = false;
            Console.Clear();
            for(int i=0;i<Math.Max(editStringLines.Count, tableStringLines.Count);i++){
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{(i < tableStringLines.Count ? tableStringLines[i].Substring(0, 2) : new string(' ', 2))}");
                if(chunks.Count > 0 && currentPageSelection == chunks[currentPage].Count){
                    Console.BackgroundColor = (canAdd && currentPageSelection == chunks[currentPage].Count && i==6+currentPageSelection) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                }else{
                    Console.BackgroundColor = (i == 5+currentPageSelection && !editing && chunks.Count > 0) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                }
                if(!showSelection){
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                if(chunks.Count == 0 && i == 7){
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                Console.Write($"{(i < tableStringLines.Count ? tableStringLines[i].Substring(2, tableStringLines[i].Length-4) : new string(' ', totalWidth-4))}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{(i < tableStringLines.Count ? tableStringLines[i].Substring(tableStringLines[i].Length-2, 2) : new string(' ', 2))}");
                Console.BackgroundColor = ConsoleColor.Black;
                if(!showEditTable || chunks.Count == 0){
                    Console.Write("\n");
                    continue;
                }
                Console.Write($" {(i==5+currentPageSelection ? "->" : "  ")} ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{(i < editStringLines.Count ? editStringLines[i].Substring(0, 2) : "")}");

                // if i is on the seperator line in the edit block we decrease i by 1 to offset the color (weird hack but works)
                if(canDelete && canEdit){
                    int selectionOffset = editSelection >= editOptions.Count ? 1 : 0;
                    if(editSelection == 3+editOptions.Count-1){
                        selectionOffset++;
                    }
                    if(i == 1 + selectionOffset + editSelection && editing){
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }else{
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }else if(canEdit){
                    int selectionOffset = editSelection >= editOptions.Count ? 1 : 0;
                    if(i == 1 + selectionOffset + editSelection && editing){
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }else{
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }else if(canDelete){
                    Console.BackgroundColor = i == 1 && editing ? ConsoleColor.DarkGray : ConsoleColor.Black;
                }

                Console.Write($"{(i < editStringLines.Count ? editStringLines[i].Substring(2, editStringLines[i].Length-4) : "")}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{(i < editStringLines.Count ? editStringLines[i].Substring(editStringLines[i].Length-2, 2) : "")}");
                Console.Write("\n");
            }
            Console.Write($"\nUser the Arrow Keys to move around\n{(canEdit ? "Press Enter to select a row to edit\n" : "")}{(canCancel ? "Press Escape to go back\n" : "")}");
            #endregion

            #region Input
            key = Console.ReadKey(true).Key;

            if(!editing && (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)){
                currentPage += (key == ConsoleKey.LeftArrow) ? -1 : 1;
                currentPageSelection = 0;
            }
            if(key == ConsoleKey.Escape){
                if(canCancel && !editing){
                    break;
                }
                if(editing){
                    editing = false;
                    editSelection = 0;
                }
            }
            if(key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow){
                if(!editing){
                    currentPageSelection += key == ConsoleKey.UpArrow ? -1 : 1;
                }else{
                    editSelection += key == ConsoleKey.UpArrow ? -1 : 1;
                }
            }
            if(key == ConsoleKey.Enter && canSelect){
                #region Select
                return chunks[currentPage][currentPageSelection];
                #endregion
            }
            if(key == ConsoleKey.Enter && !editing && (canEdit || canDelete)){
                bool adding = false;
                if(chunks.Count > 0){
                    if(currentPageSelection == chunks[currentPage].Count){
                        adding = true;
                    }else{
                        editing = true;
                        editedObject = chunks[currentPage][currentPageSelection];
                        MemberInfo[] toBeEditedMembers = editedObject.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field).ToArray();
                        propertyUpdate = new Dictionary<MemberInfo, (object, object)>();
                        foreach(MemberInfo member in toBeEditedMembers){
                            object memberData = null;
                            if(member.MemberType == MemberTypes.Property){
                                PropertyInfo memberProperty = (PropertyInfo)member;
                                memberData = (object)memberProperty.GetValue(editedObject);
                            }
                            if(member.MemberType == MemberTypes.Field){
                                FieldInfo memberProperty = (FieldInfo)member;
                                memberData = (object)memberProperty.GetValue(editedObject);
                            }
                            propertyUpdate.Add(member, (memberData, memberData));
                        }
                    }
                }else{
                    adding = true;
                }
                if(adding && canAdd && addMethod != null){
                    #region Add new T
                    // Func<T?>? addMethod
                    T? newT = addMethod.Invoke();
                    if(newT != null){
                        items.Add(newT);
                    }
                    editing = false;
                    editSelection = 0;
                    editedObject = default(T);
                    #endregion
                }
            }else if(key == ConsoleKey.Enter && editing && (canEdit || canDelete)){
                bool deleting = false;
                if(canEdit){
                    if(editSelection == 3+editOptions.Count-1 && canDelete){
                        deleting = true;
                    }else{
                        #region Editing
                        if(editSelection == 2+editOptions.Count-2){ // user selected Save Changes
                            // save the user data here
                            if(editedObject != null){
                                // update all members (fields and properties)
                                MemberInfo[] toBeEditedMembers = editedObject.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field).ToArray();
                                foreach(MemberInfo member in toBeEditedMembers){
                                    foreach(KeyValuePair<MemberInfo, (object, object)> updatedMember in propertyUpdate){
                                        if(member == updatedMember.Key){
                                            PropertyInfo p = editedObject.GetType().GetProperty(updatedMember.Key.Name.ToString());
                                            if(p != null){
                                                p.SetValue(editedObject, updatedMember.Value.Item2);
                                            }
                                            FieldInfo f = editedObject.GetType().GetField(updatedMember.Key.Name.ToString());
                                            if(f != null){
                                                f.SetValue(editedObject, updatedMember.Value.Item2);
                                            }
                                        }
                                    }
                                }
                                // send new data to the save method given by user
                                bool shouldRevert = !saveEditedUserMethod.Invoke(editedObject);
                                // revert data if the data didnt get saved (we are working with references and not copies sadly (stupid c#))
                                if(shouldRevert){
                                    // set data back
                                    foreach(MemberInfo member in toBeEditedMembers){
                                        foreach(KeyValuePair<MemberInfo, (object, object)> updatedMember in propertyUpdate){
                                            if(member == updatedMember.Key){
                                                PropertyInfo p = editedObject.GetType().GetProperty(updatedMember.Key.Name.ToString());
                                                if(p != null){
                                                    p.SetValue(editedObject, updatedMember.Value.Item1);
                                                }
                                                FieldInfo f = editedObject.GetType().GetField(updatedMember.Key.Name.ToString());
                                                if(f != null){
                                                    f.SetValue(editedObject, updatedMember.Value.Item1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            editing = false;
                            editSelection = 0;
                            editedObject = default(T);
                            propertyUpdate = new Dictionary<MemberInfo, (object, object)>();
                        }else if(editSelection == 2+editOptions.Count-1){ // user selected Discard Changes
                            editing = false;
                            editSelection = 0;
                            editedObject = default(T);
                            propertyUpdate = new Dictionary<MemberInfo, (object, object)>();
                        }else if(editSelection <= editOptions.Count-1){ // user selected a PropertEditMapping method
                            if(editedObject != null){
                                Func<T, object> editMappingValueLambda = propertyEditMapping.ElementAt(editSelection).Value.Accessor.Compile();

                                object currentPropertyValue = editMappingValueLambda(editedObject);
                                object newPropertyValue = propertyEditMapping.ElementAt(editSelection).Value.ValueGenerator.Invoke(editedObject);
                                foreach(KeyValuePair<MemberInfo, (object, object)> member in propertyUpdate){
                                    string propertyName = "";
                                    if(propertyEditMapping.ElementAt(editSelection).Value.Accessor.Body is MemberExpression bodyMember){
                                        propertyName = bodyMember.Member.Name;
                                    }else if(propertyEditMapping.ElementAt(editSelection).Value.Accessor.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember){
                                        propertyName = unaryMember.Member.Name;
                                    }
                                    if(member.Key.Name.ToString() == propertyName.ToString()){
                                        propertyUpdate[member.Key] = (currentPropertyValue, newPropertyValue);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }else if(canDelete){
                    deleting = true;
                }
                if(deleting){
                    #region Deleting
                    if(deleteMethod != null){
                        bool remove = deleteMethod.Invoke(editedObject);
                        if(remove){
                            items.Remove(editedObject);
                        }
                        editing = false;
                        editSelection = 0;
                        editedObject = default(T);
                        if(remove){
                            if(chunks[currentPage].Count == 0){
                                // go page back
                                currentPage--;
                                currentPageSelection = 0;
                            }else{
                                // go upo 1 spot
                                currentPageSelection--;
                            }
                        }
                    }
                    #endregion
                }

            }
            #endregion

            #region Clamping Values
            currentPage = Math.Clamp(currentPage, 0, Math.Max(0, chunks.Count-1));
            if(chunks.Count > 0){
                if(canAdd){
                    currentPageSelection = Math.Clamp(currentPageSelection, 0, chunks[currentPage].Count);
                }else{
                    currentPageSelection = Math.Clamp(currentPageSelection, 0, chunks[currentPage].Count-1);
                }
            }else{
                currentPageSelection = 0;
            }
            if(canEdit && canDelete){
                editSelection = Math.Clamp(editSelection, 0, 3+editOptions.Count-1);
            }else if(canEdit){
                editSelection = Math.Clamp(editSelection, 0, 2+editOptions.Count-1);
            }else if(canDelete){
                editSelection = Math.Clamp(editSelection, 0, 0);
            }
            if(chunks.Count > 0){
                if(currentPageSelection == chunks[currentPage].Count){
                    showEditTable = false;
                }else if(canEdit || canDelete){
                    showEditTable = true;
                }
            }else{
                showEditTable = false;
                editing = false;
                editSelection = 0;
                editedObject = default(T);
            }
            #endregion
        }while(true);
        return default(T);
    }

    /// <summary>
    /// Displays a table that holds a list of objects and allows the user to select an item.
    /// </summary>
    /// <typeparam name="T">The type of object that the table will handle.</typeparam>
    /// <param name="items">A list of type T that holds all the objects.</param>
    /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> is a method that gets a type T object and returns an object of any type.</param>
    /// <param name="canCancel">A boolean indicating if the user can press escape to cancel the selection and return back.</param>
    /// <returns>Returns the selected object of type T, or NULL if the selection is cancelled by pressing escape.</returns>
    public static T? SelectFromTable<T>(List<T> items, Dictionary<string, Func<T, object>> headers, bool canCancel){
        return Table(items, headers, true, canCancel, false, null, null, false, null, false, null);
    }

    /// <summary>
    /// Displays a table that holds a list of objects and allows the user to select an item.
    /// </summary>
    /// <typeparam name="T">The type of object that the table will handle.</typeparam>
    /// <param name="items">A list of type T that holds all the objects.</param>
    /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> is a method that gets a type T object and returns an object of any type.</param>
    /// <returns>Returns the selected object of type T.</returns>
    public static T SelectFromTable<T>(List<T> items, Dictionary<string, Func<T, object>> headers){
        return Table(items, headers, true, false, false, null, null, false, null, false, null);
    }

    /// <summary>
    /// Displays a table that holds a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of object that the table will handle.</typeparam>
    /// <param name="items">A list of type T that holds all the objects.</param>
    /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> is a method that gets a type T object and returns an object of any type.</param>
    public static void ShowInTable<T>(List<T> items, Dictionary<string, Func<T, object>> headers){
        Table(items, headers, false, true, false, null, null, false, null, false, null);
    }
    #endregion

    #region String Helpers
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

    /// <summary>
    /// Centers the input data within a string of specified total width, padding with the specified filler character.
    /// </summary>
    /// <param name="data">A string that will be centered.</param>
    /// <param name="totalWidth">An integer indicating the total width of the final result. If totalWidth is less than the length of data, the data is returned as is.</param>
    /// <param name="filler">A character used for padding. Default is a space character.</param>
    /// <returns>A string with the input data centered and padded on both sides with the filler character.</returns>
    private static string Center(string data, int totalWidth, char filler=' ')
    {
        string m = data;
        for(int i=Math.Max(0, totalWidth-data.Length);i>0;i--){
            m = ((i % 2 == 1) ? filler : "") + m + ((i % 2 == 0) ? filler : "");
        }
        return m;
    }
    #region Print Seats
    /// <summary>
    /// PrintSeats gets the selectedroom, a list of entertainments on seats and the coordinates of those seats.
    /// </summary>
    /// <param name="r">The RoomId which will be used to get the seat layout.</param>
    /// <param name="entertainments">Hold a list of entertainments on a seat.</param>
    /// <param name="x">The Row coordinates of a seat.</param>
    /// <param name="y">The Column coordinates of a seat.</param>
    public static void PrintSeats(Room r,List<Entertainment> entertainments, int x, int y)
    {
        Console.CursorVisible = false;
        Console.Clear();
        // calculate the longest row of seats
        int widestSeats = r.Seats.OrderByDescending(arr => arr.Length).First().Length;
        Console.Write("Select a seat:\n(Gold indicates there is special entertainment)\n\n");
        // create the top surounding bar with the word Screen centered
        string header = "Screen";
        for(int i=0;i<(widestSeats*2)+1 - "Screen".Length;i++)
        {
            header = ((i % 2 == 1) ? "─" : "") + header + ((i % 2 == 0) ? "─" : "");
        }
        Console.Write($"┌{header}┐\n");
        for(int i=0;i<r.Seats.Length;i++)
        {
            for(int line=0;line<2;line++)
            {
                Console.Write("│ ");
                for(int j=0;j<Math.Max(widestSeats, r.Seats[i].Length);j++)
                {
                    foreach(Entertainment e in entertainments){
                        // if an entertainment takes place at this seat make it "gold"
                        if(e.SeatRow == i && e.SeatColumn == j){
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                    }
                    // if x and y are the selected seat make the background color light gray
                    Console.BackgroundColor = (i == y && j == x) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    // print box (based on line print the top or bottom)
                    Console.Write(j < r.Seats[i].Length && r.Seats[i][j] ? (line==0 ? "╔═╗" : "╚═╝") : "   ");
                    // reset colors
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
                Console.Write($"│\n");
            }
        }
        // print bottom surounding line
        Console.Write($"└{new string('─', (widestSeats*4)+1)}┘\n\n");
    }

    /// <summary>
    /// Takes a room object and returns a string of the room layout.
    /// </summary>
    /// <param name="r">A room object.</param>
    /// <returns>A string that  is the room layout.</returns>
    public static string PrintSeats(Room r)
    {
        int whiteSpace = 0;
        string layout = "";
        Console.CursorVisible = false;
        Console.Clear();
        // calculate the longest row of seats
        int widestSeats = r.Seats.OrderByDescending(arr => arr.Length).First().Length;
        // create the top surounding bar with the word Screen centered
        string header = "Screen";
        // checks if header is longer than seats
        if(header.Length > (widestSeats*3)+2)
        {
            whiteSpace = 1;
        }
        for(int i=0;i<(widestSeats*3)+2 - "Screen".Length;i++)
        {
            header = ((i % 2 == 1) ? "─" : "") + header + ((i % 2 == 0) ? "─" : "");
            // header
        }
        layout = layout + $"┌{header}┐\n";
        for(int i=0;i<r.Seats.Length;i++)
        {
            for(int line=0;line<2;line++)
            {
                layout = layout + "│ ";
                for(int j=0;j<Math.Max(widestSeats, r.Seats[i].Length);j++)
                {
                    // print box (based on line print the top or bottom)
                    layout = layout + (j < r.Seats[i].Length && r.Seats[i][j] ? (line==0 ? "╔═╗" : "╚═╝") : "   ");
                }
                layout = layout + $"{new string(' ', whiteSpace)} │\n";
            }
        }
        // print bottom surounding line
        layout = layout + $"└{new string('─', (widestSeats*3)+whiteSpace+2)}┘\n\n";
        return layout;
    }

    #endregion
}
#endregion
// ┌─┬┐
// │ ││
// ├─┼┤
// └─┴┘