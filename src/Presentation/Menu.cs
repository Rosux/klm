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
                            RoomMenu.AdminOverView();
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

        // seats is basically a layout of the seats in the room, in this case this room has this layout:
        // a 2x2 area with 3 seats and 1 empty space
        // [seat] [empty space]
        // [seat] [seat]

        bool[][] seats = new bool[][]{
            new bool[] {true, false},
            new bool[] {true, true},
        };

        Room r = new Room(2, seats);
        List<Entertainment> entertainments = new List<Entertainment>() {
            new Entertainment(new DateTime(2024, 5, 20, 4, 20, 0), "Lap dance", 1, 1)
        };
        // at 04:20 AM we ordered a lap dance in seat 1, 1
        // 1,1 in this case is the bottom right chair as in
        // [], []
        // [], [THIS ONE]

        int x = 0;
        int y = 0;
        ConsoleKey key;
        do{
            Console.Clear();
            Console.Write("Select a seat:\n(Gold indicates there is special entertainment)\n\n");
            for(int i=0;i<r.Seats.Length;i++)
            {
                for(int j=0;j<r.Seats[i].Length;j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if(r.Seats[i][j]){
                        Console.BackgroundColor = y == i && x == j ? ConsoleColor.DarkGray : ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        foreach(Entertainment e in entertainments){
                            if(e.SeatRow == i && e.SeatColumn == j){
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            }
                        }
                        Console.Write("[x]");
                    }else{
                        Console.BackgroundColor = y == i && x == j ? ConsoleColor.DarkGray : ConsoleColor.Black;
                        Console.Write("   ");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
                Console.Write("\n");
            }
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
}
