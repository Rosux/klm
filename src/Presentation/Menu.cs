using System;
class Menu
{
    public static void Start()
    {
        Console.Title = "24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;
        Console.WriteLine("Welcome to 24-7 binge watch cinema!");
        // asks the user to choose either of these options
        while(true){
            if(Program.CurrentUser == null)
            {
                MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                    {"Register", ()=>{
                        // run Register method
                        UserLogic.Register();
                    }},
                    {"Login", ()=>{
                        // run Login method
                        UserLogic.Login();
                    }},
                    {"Exit", ()=>{
                        // close application
                        Environment.Exit(1);
                    }},
                });
            }
            else if(Program.CurrentUser.Role == UserRole.ADMIN)
            {
                bool uwu = true;
                while(uwu)
                {
                    MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                        {"Manage movies/series", ()=>{
                            // takes admin to movie editor
                            FilmSerieMenu.UI();
                        }},
                        {"Consumptions", ()=>{
                            // consumption editor
                            ConsumptionLogic.Consumption();
                        }},
                        {"Rooms", ()=>{
                            // room editor
                            RoomLogic.Menu();
                        }},
                        {"Reservations", ()=>{
                            // takes admin to his reservation menu
                            ReservationOverviewAdminMenu.ReservationAdminOverview();
                        }},
                        {"Users", ()=>{
                            // starts the userlogic main loop
                            UserLogic.User();
                        }},
                        {"Logout", ()=>{
                            // goto login screen
                            uwu = false;
                            Program.CurrentUser = null;
                        }},
                    });
                }
            }
            else if(Program.CurrentUser.Role == UserRole.USER)
            {
                bool uwu = true;
                while(uwu)
                {
                    MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                        {"Reservations", ()=>{
                            ReservationLogic.Reservation();
                        }},
                        {"Logout", ()=>{
                            // goto login screen
                            uwu = false;
                            Program.CurrentUser = null;
                        }},
                    });
                }
            }
        }
    }

    // used for testing purpose
    public static void TestStart(){
        Program.CurrentUser = new User(8, "Ad", "Min", "hihihi", "uwu-onichan-senpai", UserRole.ADMIN);
        Console.Title = "TEST 24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;

        // RoomLogic.Menu();

        // seats is basically a layout of the seats in the room, in this case this room has this layout:
        // a 4x4 area with 8 seats and 1 empty space
        // [ seat ] [ seat ] [ seat ] [ seat ]
        // [ seat ] [ seat ] [ seat ] [ seat ]
        // [ seat ] [ seat ] [ empty] [ empty]
        // [ seat ] [ seat ] [ seat ] [ seat ]

        bool[][] seats = new bool[][]{
            new bool[] {true, true, true, true},
            new bool[] {true, true, true, true},
            new bool[] {true, true, false, false},
            new bool[] {true, true, true, true},
        };

        Room r = new Room(seats); // r.Capacity is 14 (it automatically counts all the chairs and assigns it to the Capacity)
        List<Entertainment> entertainments = new List<Entertainment>() {
            new Entertainment(new DateTime(2024, 5, 20, 4, 20, 0), "Lap dance", 2, 1),
            new Entertainment(new DateTime(2024, 5, 20, 4, 59, 0), "Corner Seat Experience", 0, 3)
            //  ^ at 04:20 AM we ordered a lap dance in seat 2, 1
            // [ x0 y0 ], [ x1 y0 ], [ x2 y0 ], [ x3 y0 ]
            // [ x0 y1 ], [ x1 y1 ], [ x2 y1 ], [ x3 y1 ]
            // [ x0 y2 ], [ THIS  ], [ x2 y2 ], [ x3 y2 ]
            // [ x0 y3 ], [ x1 y3 ], [ x2 y3 ], [ x3 y3 ]
        };

        int x = 0;
        int y = 0;
        ConsoleKey key;
        do{
            Console.Clear();
            PrintSeats(r, entertainments, x, y);
            key = Console.ReadKey(true).Key;

            // handle selecting seat column and row (X=column and Y=row)
            if(key == ConsoleKey.UpArrow){
                y--;
            }else if(key == ConsoleKey.DownArrow){
                y++;
            }else if(key == ConsoleKey.LeftArrow){
                x--;
            }else if(key == ConsoleKey.RightArrow){
                x++;
            }

            // Selecting a seat will show the entertainment linked to that specific seat
            if(key == ConsoleKey.Enter){
                bool specialSeat = false;
                foreach(Entertainment e in entertainments){
                    if(e.SeatRow == y && e.SeatColumn == x){
                        specialSeat = true;
                        Console.Write($"{e.Text} on seat x: {x} y: {y} at: {e.Time}");
                        Console.ReadKey(true);
                    }
                }
                // note: r.Seats[y][x] x and y are reversed since the data gets saved as a jagged where the first key is the row and the second key the column
                if(!specialSeat && r.Seats[y][x]){
                    Console.Write($"Nothing is happening on seat x: {x} y: {y}");
                    Console.ReadKey(true);
                }
            }

            if(key == ConsoleKey.Escape){
                break;
            }
            y = Math.Clamp(y, 0, r.Seats.Length-1);
            x = Math.Clamp(x, 0, r.Seats[y].Length-1);
        }while(true);


    }

    private static void PrintSeats(Room r, List<Entertainment> entertainments, int x, int y)
    {
        // calculate the longest row of seats
        int widestSeats = r.Seats.OrderByDescending(arr => arr.Length).First().Length;
        Console.Write("Select a seat:\n(Gold indicates there is special entertainment)\n\n");
        // create the top surounding bar with the word Screen centered
        string header = "Screen";
        for(int i=0;i<(widestSeats*4)+1 - "Screen".Length;i++)
        {
            header = ((i % 2 == 1) ? "─" : "") + header + ((i % 2 == 0) ? "─" : "");
            // header
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
}
