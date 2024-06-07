/// <summary>
/// This class handles stuff like adding/removing/editing rooms
/// </summary>
public static class RoomLogic
{
    private static RoomAccess _roomAccess = new RoomAccess();
    public static ReservationAccess _reservationAccess = new ReservationAccess();
    /// <summary>
    /// Creates a menu of 4 options to choose from and a back option. Options are: "Show room", "Add room", "Remove room", "Edit room".
    /// </summary>
    public static void Menus()
    {
        bool running = true;
        while(running)
        {
            // gives the user a UI with all options
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Show all rooms", ()=>{
                    ShowAllRooms();
                }},
                {"2. Add a room", ()=>{
                    AddRoom();
                }},
                {"3. Remove a room", ()=>{
                    RemoveRoom();
                }},
                {"4. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }

    /// <summary>
    /// Prints all rooms an a neat UI and has the option to go back.
    /// </summary>
    public static void ShowAllRooms()
    {
        ///count all the rooms. if there are none show a "No rooms found" text otherwise print the rooms
        if(_roomAccess.GetAllRooms().Count == 0){
            RoomMenu.NoRoomsFoundNotification();
        }else{
            Room? chosenRoom = ChooseRoom("choose a room to see");
            if(chosenRoom != null){
                Console.CursorVisible = false;
                Console.Clear();
                Console.WriteLine(MenuHelper.PrintSeats(chosenRoom));
                Console.ReadKey(true);
            }
        }
    }

    /// <summary>
    /// Ask the user to select a room and to delete it. Has an option to go back at any time.
    /// </summary>
    public static void RemoveRoom()
    {
        // ask the user to select a room for deletion
        Room? selectedRoom = ChooseRoom("Select a room to delete");
        if(selectedRoom == null){
            // user pressed escape so dont delete anything and return to the menu
            return;
        }else{
            bool exists = false;
            bool deletion = false;
            List<Reservation> allReservations = _reservationAccess.GetAllReservations();
            foreach(Reservation reservation in allReservations)
            {
                if(reservation.RoomId == selectedRoom.Id)
                {
                    exists = true;
                }
            }
            // ask the user if they're sure they want to delete the room
            if(exists)
            {
                deletion = MenuHelper.Confirm($"Are you sure you want to delete the selected room:\nId: {selectedRoom.Id}\nCapacity: {selectedRoom.Capacity}\nThis room is planned in for 1 or more users.", true);
            }else
            {
                deletion = MenuHelper.Confirm($"Are you sure you want to delete the selected room:\nId: {selectedRoom.Id}\nCapacity: {selectedRoom.Capacity}");
            }
            if(deletion){
                // if the user answered yes remove the room and show the user if it worked or not
                bool success = _roomAccess.RemoveRoom(selectedRoom.Id);
                RoomMenu.RoomDeletedNotification(success);
            }else{
                // user answered no so dont delete anything and return to the menu
                return;
            }
        }
    }

    /// <summary>
    /// lets the admin add a room by using the following steps:
    /// 1. asks how much rows he wants using menuhelper.selectinterger
    /// 2. asks how much seats he wants per row using menuhelper.selectinterger
    /// 3. gives the admin the option to remove or reinstae seats using RoomLayoutManager
    /// 4. than asks if the admin wants to save the rrom.
    /// </summary>
    /// <param name="given_rows"> a interger that gives the rows for the room (only used for loop)</param>
    /// <param name="seat">a interger that gives the seats for the room (only used for loop)</param>
    public static void AddRoom(int given_rows = 0, int seat = 0)
    {
        string RoomName = MenuHelper.SelectText("Give a room name", "", false, 0, 100, @"([A-z]|\d| )");
        int? givenRows = null;
        if(given_rows == 0)
        {
            givenRows = MenuHelper.SelectInteger("Select the amount of rows you want for the new room: ", "", true, 0, 1, 10);
        }
        else
        {
            givenRows = given_rows;
        }
        if (givenRows != null)
        {
            int rows = (int)givenRows;
            int? givenSeats = MenuHelper.SelectInteger($"Current amount of rows: {rows}\nSelect the amount of seats you want per row: ", "", true, 0, 1, 10);
            /// makes the layout for the room
            if (givenSeats  != null)
            {
                int capacity = rows * (int)givenSeats;
                int i = 0;
                bool[][] seats = new bool[rows][];
                while(i < rows)
                {
                    List<bool> seatsRow = new List<bool>();
                    int j = 0;
                    while(j < givenSeats)
                    {
                        seatsRow.Add(true);
                        j++;
                    }
                    bool[] row = seatsRow.ToArray();
                    seats[i] = row;
                    i++;
                }
                Room room = new(seats, RoomName);
                string prefix = "This is the current room layout:";
                string suffix = "Use your arrow keys to select a seat.\nPress space to remove or re-instate a seat, the room can not be empty.\n\nPress enter to save the room.\nPress escape to go back.";
                /// lets admin remove or reinsate seats
                Room roomFinished = RoomLayoutManager(room, prefix, suffix, RoomName);
                if(roomFinished != null)
                {
                    /// asks for conformation
                    bool conformation = MenuHelper.Confirm($"Current room layout:\n{MenuHelper.PrintSeats(roomFinished)}\nAre you sure you want to add this room? ");
                    if(conformation)
                    {
                        /// if conformation is given it adds the room to the database
                        _roomAccess.AddRoom(roomFinished);
                        Console.WriteLine("\nRoom added sucessfully.");
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.WriteLine("\nAction cancelled.");
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    Console.WriteLine("Going back.");
                    Console.Write($"\n\nPress any key to continue...");
                    AddRoom(rows, 0);
                }
            }
            else
            {
                Console.WriteLine("Going back.");
                Console.Write($"\n\nPress any key to continue...");
                AddRoom(0, 0);
            }
        }
        else
        {
            Console.WriteLine("Action cancelled.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    /// <summary>
    /// gives the user an Interface wher he can see all the rooms and choose one to return
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <returns>The selcted room as a Room object or null to cancel the action</returns> 
    public static Room? ChooseRoom(string prefix = "Choose a room")
    {
        int page = 0;
        int i = 0;
        int j = 0;
        int choice = 0;
        /// holds the pages
        List<List<Room>> allRoomList = new List<List<Room>>();
        /// holds all rooms
        List<Room> roomListRoom = _roomAccess.GetAllRooms();
        /// is a list that gets filled with all rooms per page and then added to allRoomList
        List<Room> roomList = new List<Room>();
        /// holds longest sting per page
        List<int> allLength = new List<int>();
        int longest = 0;
        /// checks for longest string per page and adds it to list allLength
        if (roomListRoom.Count == 0)
        {
            Console.WriteLine("No rooms have been found\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return null;
        }
        foreach(Room room in roomListRoom)
        {
            bool check = true;
            string room_str = $"Id: {room.Id}, Name: {room.RoomName}, capacity: {room.Capacity}";
            if(room_str.Length > longest)
            {
                longest = room_str.Length;
            }
            if(i == 9)
            {
                if(longest < prefix.Length)
                {
                    allLength.Add(prefix.Length);
                    longest = 0;
                    i = 0;
                    check = false;
                    
                }
                else
                {
                    allLength.Add(longest);
                    longest = 0;
                    i = 0;
                    check = false;
                }
            }
            if(check)
            {
                i++;
            }
        }
        if(longest != 0)
        {
            if(longest < prefix.Length)
            {
                allLength.Add(prefix.Length);
            }
            else
            {
                allLength.Add(longest);
            }
        }
        i = 0;
        /// makes and adds all pages to allRoomList
        foreach(Room room in roomListRoom)
        {
            roomList.Add(room);
            i++;
            if (i == 10)
            {
                allRoomList.Add(roomList);
                roomList = new List<Room>();
                i = 0;
            }
        }
        if(roomList.Count != 0)
        {
            allRoomList.Add(roomList);
        }
        /// prints the UI
        ConsoleKey key;
        do
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.WriteLine("Press escape to exit.\n");
            /// checks if the longest string is even or odd so the printing of the UI is corect
            if(allLength[page] % 2 == 0)
            {
                j = 2;
            }
            else
            {
                j = 1;
            }
            ///prints header
            Console.WriteLine($"┌─{prefix}{new String('─', Math.Max(0, allLength[page] - prefix.Length))}─┐");
            /// makes sure that if pages are switched the top room is selected first
            i = 0 +10 * page;
            /// prints all info
            foreach(Room room in allRoomList[page])
            {
                string zin = $"Id: {room.Id}, Name: {room.RoomName}, capacity: {room.Capacity}";
                Console.Write("│ ");
                /// if currently selected make the background darkgray instead of black
                if (i == choice){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                Console.Write(zin);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{new String(' ', Math.Max(0, allLength[page] - zin.Length))} │\n");
                i++;
            }

            Console.Write($"├─{new string('─', Math.Max(0, allLength[page] ))}─┤\n");
            string pageNumber = $"{page+1}/{allRoomList.Count}";
            string pageArrows = $"{pageNumber}";
            // add spaces on each side of the page number
            for (int ii=1;ii<=Math.Max(2, allLength[page]-pageNumber.Length-4);ii++){
                pageArrows = ((ii % 2 == 1) ? " " : "") + pageArrows + ((ii % 2 == 0) ? " " : "");
            }
            // add arrows on the correct sides of the page number
            pageArrows = (page > 0 ? "<-" : "  ") + pageArrows + (page < allRoomList.Count-1 ? "->" : "  ");
            /// prints the arrows under the page
            Console.Write($"│ {pageArrows} │\n");
            Console.Write($"└─{new string('─', Math.Max(0, allLength[page] ))}─┘\n");
            
            key = Console.ReadKey(true).Key;
             /// lets user go up and down page to selcect a room
            if (key == ConsoleKey.UpArrow && choice > 0 + 10 * page) 
            {  
                Console.CursorVisible = false;
                Console.Clear();
                choice --;
            }
            else if (key == ConsoleKey.DownArrow && choice < 10 + 10 * page-1 && choice < roomListRoom.Count -1)
            {  
                Console.CursorVisible = false;
                Console.Clear();
                choice ++;
            }
             /// lets user go through pages
            if (key == ConsoleKey.RightArrow && page != allRoomList.Count-1)
            {  
                Console.CursorVisible = false;
                Console.Clear();
                page ++;
                choice = 10 * page;
            }
            else if (key == ConsoleKey.LeftArrow && page != 0)
            {  
                Console.CursorVisible = false;
                Console.Clear();
                page --;
                choice = 10 * page;
            }
            /// returns chosen room
            else if (key == ConsoleKey.Enter)
            {  
                return roomListRoom[choice];
                break;
            }
        } while (key != ConsoleKey.Escape);   
        return null;
    }

    /// <summary>
    /// this maethode takes a room and lets the admin edit this rooms layout
    /// </summary>
    /// <param name="room"> a room object it uses to manage its layout</param>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <returns></returns>
    public static Room? RoomLayoutManager(Room room, string prefix ="", string suffix = "", string RoomName = "")
    {
        ConsoleKey key;
        int choiceSeat = 0;
        string header = "Screen";
        int whiteSpace = 1;
        int longest = 0;
        int seatPerRow = room.Seats[0].Length;
        bool[][] seats = room.Seats;

        foreach(bool[] row in seats)
        {
            longest = 2;
            foreach(bool seat in row)
            { 
                longest = longest + 3;
            }
        }
        if(header.Length > longest)
        {
            longest = header.Length;
            whiteSpace = 2;
        }
        for(int i=0;i<longest - "Screen".Length;i++)
        {
            header = ((i % 2 == 1) ? "─" : "") + header + ((i % 2 == 0) ? "─" : "");
            // header
        }

        do
        {
            List<List<string>> allRowTop = new List<List<string>>();
            List<List<string>> allRowBottom = new List<List<string>>();
            foreach(bool[] row in seats)
            {
                List<string> rowTop = new List<string>();
                List<string> rowBottom = new List<string>();
                foreach(bool seat in row)
                {         
                    if (seat)
                    {
                        rowTop.Add("╔═╗");
                        rowBottom.Add("╚═╝");
                    }
                    else
                    {
                        rowTop.Add("   ");
                        rowBottom.Add("   ");
                    }
                }
                allRowTop.Add(rowTop);
                allRowBottom.Add(rowBottom);
            }
            bool notEmptyCheck = false;
            foreach(bool[] row in seats)
            {
                foreach(bool seat in row)
                {
                    if (seat)
                    {
                        notEmptyCheck = true;
                    }
                }
            }
            var zip = allRowTop.Zip(allRowBottom, (i,j) => (i,j));
            Console.CursorVisible = false;
            Console.Clear();
            Console.WriteLine(prefix + "\n");
            Console.Write($"┌{header}┐\n");
            int i = 0;
            int ii = 0;
            foreach(var (rowTop, rowBottom) in zip)
            {
                Console.Write("│ ");
                foreach(string seatTop in rowTop)
                {
                    
                    if (i == choiceSeat){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write(seatTop);
                    Console.BackgroundColor = ConsoleColor.Black;
                    i++;
                }
                Console.Write($"{new string(' ', whiteSpace)}│");
                Console.WriteLine();
                Console.Write("│ ");
                foreach(string seatBottom in rowBottom)
                {
                    if (ii == choiceSeat){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write(seatBottom);
                    Console.BackgroundColor = ConsoleColor.Black;
                    ii++;
                }
                Console.Write($"{new string(' ', whiteSpace)}│");
                Console.WriteLine();         
            }
            Console.Write($"└{new string('─', longest)}┘\n\n");
            Console.Write(suffix);
            key = Console.ReadKey(true).Key;
            if(key == ConsoleKey.Escape)
            {
                break;
            }
            else if(key == ConsoleKey.RightArrow && choiceSeat < i - 1)
            {
                choiceSeat++;
            }
            else if(key == ConsoleKey.LeftArrow && choiceSeat != 0)
            {
                choiceSeat--;
            }
            else if(key == ConsoleKey.UpArrow && choiceSeat >= seatPerRow)
            {
                choiceSeat = choiceSeat - seatPerRow;
            }
            else if(key == ConsoleKey.DownArrow&& choiceSeat < i - seatPerRow)
            {
                choiceSeat = choiceSeat + seatPerRow;
            }
            else if(key == ConsoleKey.Spacebar)
            {
                int selectedRow = choiceSeat/seatPerRow;
                int selectedSeat = choiceSeat%seatPerRow;
                if(seats[selectedRow][selectedSeat])
                {
                    seats[selectedRow][selectedSeat] = false;
                }
                else
                {
                    seats[selectedRow][selectedSeat] = true;
                }
            }
            else if(key == ConsoleKey.Enter && notEmptyCheck)
            {
                Room roomFinisehed = new(room.Id, seats, RoomName);
                return roomFinisehed;
            }
        }while(key != ConsoleKey.Escape);
        return null;
    }
}