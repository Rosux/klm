/// <summary>
/// This class handles stuff like adding/removing/editing rooms
/// </summary>
public static class RoomLogic
{
    private static RoomAccess r = new RoomAccess();

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
        if(r.GetAllRooms().Count == 0){
            RoomMenu.NoRoomsFoundNotification();
        }else{
            Room? chosenroom = ChooseRoom("choose a room to see");
            if(chosenroom != null){
                Console.Clear();
                List<Entertainment> entertainments = new List<Entertainment>(){};
                Menu.PrintSeats(chosenroom, entertainments, -1, -1);
                Console.ReadKey(true);
            }
        }
    }

    /// <summary>
    /// Ask the user to select a room and to delete it. Has an option to go back at any time.
    /// </summary>
    public static void RemoveRoom()
    {
        // convert all rooms to a dictionary
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        foreach(Room room in RoomLogic.r.GetAllRooms()){
            rooms.Add($"{room.Id}: {room.Capacity}", room);
        }
        // if there are no rooms show the user a "No rooms found" text
        if(rooms.Count == 0){
            RoomMenu.NoRoomsFoundNotification();
            return;
        }
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
                bool success = r.RemoveRoom(selectedRoom.Id);
                RoomMenu.RoomDeletedNotification(success);
            }else{
                // user answered no so dont delete anything and return to the menu
                return;
            }
        }
    }

    /// <summary>
    /// Ask the user to make a new room. saves it to the database. Has the option to go back at any time.
    /// </summary>
    public static void AddRoom()
    {
        int? GivenRows_p = MenuHelper.SelectInteger("Select the amount of rows you want for the new room: ", "", true, 0, 1, 2147483647);
        if (GivenRows_p  != null)
        {
            int rows = (int)GivenRows_p;
            int? GivenSeats_p = MenuHelper.SelectInteger($"Current amount of rows: {rows}\nSelect the amount of seats you want per row: ", "", true, 0, 1, 2147483647);
            if (GivenSeats_p  != null)
            {
                int seats_per_rows = 0;
                int extra_seats = 0;
                int capacity = rows * (int) GivenSeats_p;
                if(capacity%rows == 0)
                {
                    seats_per_rows = capacity/rows;
                }
                else
                {
                    extra_seats = capacity%rows;
                    seats_per_rows = capacity/rows;
                }
                int i = 0;
                bool[][] seats = new bool[rows][];

                while(i < rows)
                {
                    List<bool> seats_row = new List<bool>();
                    int j = 0;
                    if(extra_seats == 0)
                    {
                        while(j < seats_per_rows)
                        {
                            seats_row.Add(true);
                            j++;
                        }
                    }
                    else
                    {
                        while(j < seats_per_rows)
                        {
                            seats_row.Add(true);
                            if(i < extra_seats && j == 0)
                            {
                                seats_row.Add(true);
                            }
                            j++;
                        }
                    }
                    bool[] row = seats_row.ToArray();
                    seats[i] = row;
                    i++;
                }
                Room room = new(seats);
                r.AddRoom(room);

                // List<Entertainment> entertainments = new List<Entertainment>() {
                // new Entertainment(new DateTime(2024, 5, 20, 4, 20, 0), "Lap dance", 2, 1),
                // new Entertainment(new DateTime(2024, 5, 20, 4, 59, 0), "Corner Seat Experience", 0, 3)
                // //  ^ at 04:20 AM we ordered a lap dance in seat 2, 1
                // // [ x0 y0 ], [ x1 y0 ], [ x2 y0 ], [ x3 y0 ]
                // // [ x0 y1 ], [ x1 y1 ], [ x2 y1 ], [ x3 y1 ]
                // // [ x0 y2 ], [ THIS  ], [ x2 y2 ], [ x3 y2 ]
                // // [ x0 y3 ], [ x1 y3 ], [ x2 y3 ], [ x3 y3 ]
                // };
                // Menu.PrintSeats(room, entertainments, 0, 0);
                // Console.ReadKey(true);
                
                ConsoleKey key;
                int choice_seat = 0;
                int choice_row = 0;
                int ii = 0;
                int iii = 0;
                do
                {
                    List<List<string>> all_row_top = new List<List<string>>();
                    List<List<string>> all_row_bottom = new List<List<string>>();
                    foreach(bool[] row in seats)
                    {
                        List<string> row_top = new List<string>();
                        List<string> row_bottom = new List<string>();
                        foreach(bool seatss in row)
                        {         
                            if (seatss)
                            {
                                row_top.Add("╔═╗");
                                row_bottom.Add("╚═╝");
                            }
                            else
                            {
                                row_top.Add("   ");
                                row_bottom.Add("   ");
                            }
                            
                        }
                        all_row_top.Add(row_top);
                        all_row_bottom.Add(row_bottom);
                    }

                    var zip = all_row_top.Zip(all_row_bottom, (i,j) => (i,j));
                    Console.Clear();
                    Console.Write("-----------------------------------\n");
                    ii = 0;
                    iii = 0;
                    foreach(var (row_top, row_bottom) in zip)
                    {
                        foreach(string seat_top in row_top)
                        {
                            if (ii == choice_seat){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                            Console.Write(seat_top);
                            Console.BackgroundColor = ConsoleColor.Black;
                            ii++;
                        }
                        Console.WriteLine();
                        foreach(string seat_bottom in row_bottom)
                        {
                            if (iii == choice_seat){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                            Console.Write(seat_bottom);
                            Console.BackgroundColor = ConsoleColor.Black;
                            iii++;
                        }
                        Console.WriteLine();

                        
                    }

                    key = Console.ReadKey(true).Key;
                    if(key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else if(key == ConsoleKey.RightArrow && choice_seat < ii - 1)
                    {
                        choice_seat++;
                    }
                    else if(key == ConsoleKey.LeftArrow && choice_seat != 0)
                    {
                        choice_seat--;
                    }

                }while(key != ConsoleKey.Escape);

            }
            else
            {
                Console.WriteLine("Going back.");
                Console.Write($"\n\nPress any key to continue...");
                AddRoom();
            }
        }
        else
        {
            Console.WriteLine("Action cancelled.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
    
        // TODO add AddRoom logic here (this file handles the data as in saves it to the database using r.whatevermethod and telling the RoomMenu.cs to print/ask stuff)
    

    /// <summary>
    /// Ask the user to select a room and to make any edits for that room if they wish. Has the option to go back at any time.
    /// </summary>
    public static void EditRoom()
    {
        // TODO implement editing a room. EditRoom logic here (this file handles the data as in saves/updates it in the database and tells RoomMenu.cs what to print/ask)
    }

    public static Room? ChooseRoom(string prefix = "Choose a room") 
    {
        int page = 0;
        int i = 0;
        int j = 0;
        int choice = 0;
        /// holds the pages
        List<List<Room>> allroomlist = new List<List<Room>>();
        /// holds all rooms
        List<Room> roomlist_room = r.GetAllRooms();
        /// is a list that gets filled with all rooms per page and then added to allroomlist
        List<Room> roomlist_p = new List<Room>();
        /// holds longest sting per page
        List<int> alllength = new List<int>();
        int longest = 0;
        /// checks for longest string per page and adds it to list alllength
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
                Console.Clear();
                choice --;
            }
            else if (key == ConsoleKey.DownArrow && choice < 10 + 10 * page-1 && choice < roomlist_room.Count -1)
            {  
                Console.Clear();
                choice ++;
            }
             /// lets user go through pages
            if (key == ConsoleKey.RightArrow && page != allroomlist.Count-1)
            {  
                Console.Clear();
                page ++;
                choice = 10 * page;
            }
            else if (key == ConsoleKey.LeftArrow && page != 0)
            {  
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
}