namespace MenuHelper
{
    public static class SeatUtility
    {
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
            for(int i=0;i<(widestSeats*4)+1 - "Screen".Length;i++)
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
    }
}