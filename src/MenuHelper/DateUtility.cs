namespace MenuHelper
{
    public static class DateUtility
    {
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
    }
}