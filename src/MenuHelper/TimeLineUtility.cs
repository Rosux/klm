namespace MenuHelper
{
    public static class TimeLineUtility
    {
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
    }
}