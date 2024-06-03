/// <summary>
/// This class handles stuff like adding/removing/editing rooms
/// </summary>
public static class RoomLogic
{
    private static RoomAccess _RoomAccess = new RoomAccess();

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
                {"4. Edit a room", ()=>{
                    EditRoom();
                }},
                {"5. Exit to main menu", ()=>{
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
        if(_RoomAccess.GetAllRooms().Count == 0){
            RoomMenu.NoRoomsFoundNotification();
        }else{
            Room? ChosenRoom = ChooseRoom("choose a room to see");
            if(ChosenRoom != null){
                Console.CursorVisible = false;
                Console.Clear();
                Console.WriteLine(RoomLayoutPrinter(ChosenRoom));
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
            // ask the user if theyre sure they want to delete the room
            bool deletion = MenuHelper.Confirm($"Are you sure you want to delete the selected room:\nId: {selectedRoom.Id}\nCapacity: {selectedRoom.Capacity}");
            if(deletion){
                // if the user answered yes remove the room and show the user if it worked or not
                bool success = _RoomAccess.RemoveRoom(selectedRoom.Id);
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
        int? GivenRows = null;
        if(given_rows == 0)
        {
            GivenRows = MenuHelper.SelectInteger("Select the amount of rows you want for the new room: ", "", true, 0, 1, 10);
        }
        else
        {
            GivenRows = given_rows;
        }
        if (GivenRows != null)
        {
            int rows = (int)GivenRows;
            int? GivenSeats = MenuHelper.SelectInteger($"Current amount of rows: {rows}\nSelect the amount of seats you want per row: ", "", true, 0, 1, 10);
            /// makes the layout for the room
            if (GivenSeats  != null)
            {
                int capacity = rows * (int)GivenSeats;
                int i = 0;
                bool[][] seats = new bool[rows][];
                while(i < rows)
                {
                    List<bool> seats_row = new List<bool>();
                    int j = 0;
                    while(j < GivenSeats)
                    {
                        seats_row.Add(true);
                        j++;
                    }
                    bool[] row = seats_row.ToArray();
                    seats[i] = row;
                    i++;
                }
                Room room = new(seats);
                string prefix = "This is the current room layout:";
                string suffix = "Use your arrow keys to select a seat.\nPress space to remove or re-instate a seat, the room can not be empty.\n\nPress enter to save the room.\nPress escape to go back.";
                /// lets admin remove or reinsate seats
                Room RoomFinished = RoomLayoutManager(room, prefix, suffix);
                if(RoomFinished != null)
                {
                    /// asks for conformation
                    bool conformation = MenuHelper.Confirm($"Current room layout:\n{RoomLayoutPrinter(RoomFinished)}\nAre you sure you want to add this room? ");
                    if(conformation)
                    {
                        /// if conformation is given it adds the room to the database
                        _RoomAccess.AddRoom(RoomFinished);
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
    /// lets the admin edit a room by taking these steps:
    /// 1. lets the admin select a room using ChooseRoom
    /// 2. lets admin edit the selcted room by using RoomLayouManager
    /// 3. asks if the admin wants to save changes
    /// 4. either cancels action or submits changes
    /// </summary>
    /// <param name="ChosenRoom"> takes a room object you want to edit (is only used for loop)</param>
    public static void EditRoom(Room ChosenRoom = null)
    {
        if(ChosenRoom == null)
        {
            /// kets user choose a room
            ChosenRoom = ChooseRoom("choose a room to edit");
        }
        if (ChosenRoom != null)
        {
            string prefix = "This is the current room layout:";
            string suffix = "Use your arrow keys to select a seat.\nPress space to remove or re-instate a seat, the room can not be empty.\n\nPress enter to save the room.\nPress escape to go back.";
            /// lets user edit the room
            Room? EditedRoom = RoomLayoutManager(ChosenRoom, prefix, suffix);
            if (EditedRoom != null)
            {
                /// asks if he wants to save changes
                bool conformation = MenuHelper.Confirm($"new room layout:\n{RoomLayoutPrinter(EditedRoom)}\nAre you sure you want to make these changes ");
                if(conformation)
                {
                    /// edits room
                    _RoomAccess.EditRoom(EditedRoom);
                    Console.WriteLine("\nRoom changed sucessfully.");
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
                EditRoom();
            }
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
        List<List<Room>> allroomlist = new List<List<Room>>();
        /// holds all rooms
        List<Room> roomlist_room = _RoomAccess.GetAllRooms();
        /// is a list that gets filled with all rooms per page and then added to allroomlist
        List<Room> roomlist_p = new List<Room>();
        /// holds longest sting per page
        List<int> alllength = new List<int>();
        int longest = 0;
        /// checks for longest string per page and adds it to list alllength
        if (roomlist_room.Count == 0)
        {
            Console.WriteLine("No rooms have been found\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return null;
        }
        foreach(Room room in roomlist_room)
        {
            bool check = true;
            string room_str = $"Id: {room.Id}, capacity: {room.Capacity}";
            if(room_str.Length > longest)
            {
                longest = room_str.Length;
            }
            if(i == 9)
            {
                if(longest < prefix.Length)
                {
                    alllength.Add(prefix.Length);
                    longest = 0;
                    i = 0;
                    check = false;
                    
                }
                else
                {
                    alllength.Add(longest);
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
                alllength.Add(prefix.Length);
            }
            else
            {
                alllength.Add(longest);
            }
        }
        i = 0;
        /// makes and adds all pages to allroomlist
        foreach(Room room in roomlist_room)
        {
            roomlist_p.Add(room);
            i++;
            if (i == 10)
            {
                allroomlist.Add(roomlist_p);
                roomlist_p = new List<Room>();
                i = 0;
            }
        }
        if(roomlist_p.Count != 0)
        {
            allroomlist.Add(roomlist_p);
        }
        /// prints the UI
        ConsoleKey key;
        do
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.WriteLine("Press escape to exit.\n");
            /// checks if the longest string is even or odd so the printing of the UI is corect
            if(alllength[page] % 2 == 0)
            {
                j = 2;
            }
            else
            {
                j = 1;
            }
            ///prints header
            Console.WriteLine($"┌─{prefix}{new String('─', Math.Max(0, alllength[page] - prefix.Length))}─┐");
            /// makes sure that if pages are switched the top room is selected first
            i = 0 +10 * page;
            /// prints all info
            foreach(Room room in allroomlist[page])
            {
                string zin = $"Id: {room.Id}, capacity: {room.Capacity}";
                Console.Write("│ ");
                /// if currently selected make the background darkgray instead of black
                if (i == choice){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                Console.Write(zin);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{new String(' ', Math.Max(0, alllength[page] - zin.Length))} │\n");
                i++;
            }

            Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
            string pageNumber = $"{page+1}/{allroomlist.Count}";
            string pageArrows = $"{pageNumber}";
            // add spaces on each side of the page number
            for (int ii=1;ii<=Math.Max(2, alllength[page]-pageNumber.Length-4);ii++){
                pageArrows = ((ii % 2 == 1) ? " " : "") + pageArrows + ((ii % 2 == 0) ? " " : "");
            }
            // add arrows on the correct sides of the page number
            pageArrows = (page > 0 ? "<-" : "  ") + pageArrows + (page < allroomlist.Count-1 ? "->" : "  ");
            /// prints the arrows under the page
            Console.Write($"│ {pageArrows} │\n");
            Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
            
            key = Console.ReadKey(true).Key;
             /// lets user go up and down page to selcect a room
            if (key == ConsoleKey.UpArrow && choice > 0 + 10 * page) 
            {  
                Console.CursorVisible = false;
                Console.Clear();
                choice --;
            }
            else if (key == ConsoleKey.DownArrow && choice < 10 + 10 * page-1 && choice < roomlist_room.Count -1)
            {  
                Console.CursorVisible = false;
                Console.Clear();
                choice ++;
            }
             /// lets user go through pages
            if (key == ConsoleKey.RightArrow && page != allroomlist.Count-1)
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
                return roomlist_room[choice];
                break;
            }
        } while (key != ConsoleKey.Escape);   
        return null;
    }

    /// <summary>
    /// this method takes a room object and returns a string of its layout
    /// </summary>
    /// <param name="room">takes a room object to make the layout of</param>
    /// <returns>a string that is the room layout</returns>
    public static string RoomLayoutPrinter(Room room)
    {
        string layout = "";
        int ChoiceSeat = 0;
        int ii = 0;
        int iii = 0;
        string header = "Screen";
        int h = 1;
        int longest = 2;
        int SeatPerRow = room.Seats[0].Length;
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
            h = 2;
        }
        for(int k=0;k<longest - "Screen".Length;k++)
        {
            header = ((k % 2 == 1) ? "─" : "") + header + ((k % 2 == 0) ? "─" : "");
            // header
        }
            List<List<string>> AllRowTop = new List<List<string>>();
            List<List<string>> AllRowBottom = new List<List<string>>();
            foreach(bool[] row in seats)
            {
                List<string> RowTop = new List<string>();
                List<string> RowBottom = new List<string>();
                foreach(bool seat in row)
                {         
                    if (seat)
                    {
                        RowTop.Add("╔═╗");
                        RowBottom.Add("╚═╝");
                    }
                    else
                    {
                        RowTop.Add("   ");
                        RowBottom.Add("   ");
                    }
                }
                AllRowTop.Add(RowTop);
                AllRowBottom.Add(RowBottom);
            }
            var zip = AllRowTop.Zip(AllRowBottom, (i,j) => (i,j));
            Console.CursorVisible = false;
            Console.Clear();
            //Console.WriteLine(prefix + "\n");
            layout = layout + $"┌{header}┐\n";
            ii = 0;
            iii = 0;
            foreach(var (RowTop, RowBottom) in zip)
            {
                layout = layout + "│ ";
                foreach(string SeatTop in RowTop)
                {
                    
                    if (ii == ChoiceSeat){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    layout = layout + SeatTop;
                    Console.BackgroundColor = ConsoleColor.Black;
                    ii++;
                }
                layout = layout + $"{new string(' ', h)}│";
                layout = layout + "\n";
                layout = layout + "│ ";
                foreach(string SeatBottom in RowBottom)
                {
                    if (iii == ChoiceSeat){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    layout = layout + SeatBottom;
                    Console.BackgroundColor = ConsoleColor.Black;
                    iii++;
                }
                layout = layout + $"{new string(' ', h)}│";
                layout = layout + "\n";         
            }
                layout = layout + $"└{new string('─', longest)}┘\n\n";
            //Console.Write(suffix);
            return layout;
    }
    /// <summary>
    /// this maethode takes a room and lets the admin edit this rooms layout
    /// </summary>
    /// <param name="room"> a room object it uses to manage its layout</param>
   /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="suffix">A string of text printed after the selected value.</param>
    /// <returns></returns>
    public static Room? RoomLayoutManager(Room room, string prefix ="", string suffix = "")
    {
        ConsoleKey key;
        int ChoiceSeat = 0;
        string header = "Screen";
        int WhiteSpace = 1;
        int longest = 0;
        int SeatPerRow = room.Seats[0].Length;
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
            WhiteSpace = 2;
        }
        for(int i=0;i<longest - "Screen".Length;i++)
        {
            header = ((i % 2 == 1) ? "─" : "") + header + ((i % 2 == 0) ? "─" : "");
            // header
        }

        do
        {
            List<List<string>> AllRowTop = new List<List<string>>();
            List<List<string>> AllRowBottom = new List<List<string>>();
            foreach(bool[] row in seats)
            {
                List<string> RowTop = new List<string>();
                List<string> RowBottom = new List<string>();
                foreach(bool seat in row)
                {         
                    if (seat)
                    {
                        RowTop.Add("╔═╗");
                        RowBottom.Add("╚═╝");
                    }
                    else
                    {
                        RowTop.Add("   ");
                        RowBottom.Add("   ");
                    }
                }
                AllRowTop.Add(RowTop);
                AllRowBottom.Add(RowBottom);
            }
            bool NotEmptyCheck = false;
            foreach(bool[] row in seats)
            {
                foreach(bool seat in row)
                {
                    if (seat)
                    {
                        NotEmptyCheck = true;
                    }
                }
            }
            var zip = AllRowTop.Zip(AllRowBottom, (i,j) => (i,j));
            Console.CursorVisible = false;
            Console.Clear();
            Console.WriteLine(prefix + "\n");
            Console.Write($"┌{header}┐\n");
            int i = 0;
            int ii = 0;
            foreach(var (RowTop, RowBottom) in zip)
            {
                Console.Write("│ ");
                foreach(string SeatTop in RowTop)
                {
                    
                    if (i == ChoiceSeat){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write(SeatTop);
                    Console.BackgroundColor = ConsoleColor.Black;
                    i++;
                }
                Console.Write($"{new string(' ', WhiteSpace)}│");
                Console.WriteLine();
                Console.Write("│ ");
                foreach(string SeatBottom in RowBottom)
                {
                    if (ii == ChoiceSeat){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write(SeatBottom);
                    Console.BackgroundColor = ConsoleColor.Black;
                    ii++;
                }
                Console.Write($"{new string(' ', WhiteSpace)}│");
                Console.WriteLine();         
            }
            Console.Write($"└{new string('─', longest)}┘\n\n");
            Console.Write(suffix);
            key = Console.ReadKey(true).Key;
            if(key == ConsoleKey.Escape)
            {
                break;
            }
            else if(key == ConsoleKey.RightArrow && ChoiceSeat < i - 1)
            {
                ChoiceSeat++;
            }
            else if(key == ConsoleKey.LeftArrow && ChoiceSeat != 0)
            {
                ChoiceSeat--;
            }
            else if(key == ConsoleKey.UpArrow && ChoiceSeat >= SeatPerRow)
            {
                ChoiceSeat = ChoiceSeat - SeatPerRow;
            }
            else if(key == ConsoleKey.DownArrow&& ChoiceSeat < i - SeatPerRow)
            {
                ChoiceSeat = ChoiceSeat + SeatPerRow;
            }
            else if(key == ConsoleKey.Spacebar)
            {
                int SelctedRow = ChoiceSeat/SeatPerRow;
                int SelctedSeat = ChoiceSeat%SeatPerRow;
                if(seats[SelctedRow][SelctedSeat])
                {
                    seats[SelctedRow][SelctedSeat] = false;
                }
                else
                {
                    seats[SelctedRow][SelctedSeat] = true;
                }
            }
            else if(key == ConsoleKey.Enter && NotEmptyCheck)
            {
                Room room_finisehed = new(room.Id, seats);
                return room_finisehed;
            }
        }while(key != ConsoleKey.Escape);
        return null;
    }
}