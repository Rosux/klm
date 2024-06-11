namespace MenuHelper
{
    public static class TimeUtility
    {
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
    }
}