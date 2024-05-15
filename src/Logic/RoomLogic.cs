public static class RoomLogic
{
    private static RoomAccess r = new RoomAccess();

    /// takes a int and uses it to make a new room with the int as capcity
    /// then calls the method ConformationRoom with asks if he is suer he wants to make a new room
    /// then calls the method AddToRoomTable to add a new room to the data base
    /// <param name="GivenCapacity">Interger with capacity for new room.</param>
    public static void CreateRoom(int GivenCapacity)
    {
        /// creates new room with given capacity
        Room newRoom = new Room(GivenCapacity);
        Console.Clear();
        /// asks for conformation to createroom
        bool? choice = ConformationRoom("Save this room?", $"New room created with capacity: {GivenCapacity}.");
        /// if conformation is not given it calls method RoomMenu.Action2() it basiclay goes back to the start
        if(choice!= null)
        {
            if((bool)choice)
            {
                /// ads new room to database
                Console.Clear();
                int newRoomId = r.AddToRoomTable(newRoom);
                Console.WriteLine($"New room created: \nID: {newRoomId}. \nCapacity: {GivenCapacity}.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Room discarded.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
        else
        {
            RoomMenu.Action2();
        }
    }
    /// <summary>
    /// removes the give room from database if conformation is given
    /// </summary>
    /// <param name="room">Takes room object to remove form database</param>
    public static void RemoveRoom(Room room)
    {
        List<string> options = new List<string>(){"Yes", "No"};
        /// asks for conformation to remove room
        bool? choice = ConformationRoom($"remove this room?", $"Selected room:\nId: {room.Id}.\nCapacity: {room.Capacity}.",options);
        /// if conformation is not given it calls method RoomMenu.Action3() it basiclay goes back to the start
        if(choice!= null)
        {
            if((bool)choice)
            {
                /// removes given room from database
                Console.Clear();
                r.RemoveFromRoomTable(room.Id);
                Console.WriteLine($"Room succesfully removed.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Room not removed.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
        else
        {
            RoomMenu.Action3();
        }
    }
    /// <summary>
    /// chages the given room's capactiy by given capacity
    /// </summary>
    /// <param name="room"> Takes room object to edit</param>
    /// <param name="capacity"> Takes new capcity</param>
    public static void EditRoom(Room room, int capacity)
    {
        List<string> options = new List<string>(){"Yes", "No"};
        string text = $"Old room: {r.GetRoom(room.Id)}.\n" + $"New room: Room-ID = {room.Id} Capacity = {capacity}.";
        /// asks for conformation to edit room
        bool? choice = ConformationRoom($"Save these changes?", text, options);
        /// if conformation is not given it calls method RoomMenu.Action4(room) it basiclay goes back to the start 
        if(choice!= null)
        {
            if((bool)choice)
            {
                /// edits the given room
                Console.Clear();
                r.EditFromRoomTable(room.Id, capacity);
                Console.WriteLine($"Changes saved succesfully.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Changes not saved.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
        else
        {
            RoomMenu.Action4(room);
        }
    }
    
    /// <summary>
    /// returns a list of info from all rooms
    /// </summary>
    /// <returns> a list of info from all rooms </returns>
    public static List<string> GetAllRooms()
    {
        return r._getAllRooms();
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
        List<Room> roomlist_room = r.GetAllRooms();
        /// is a list that gets filled with all rooms per page and then added to allroomlist
        List<Room> roomlist_p = new List<Room>();
        /// holds longest sting per page
        List<int> alllength = new List<int>();
        int longest = 0;
        /// checks for longest string per page and adds it to list alllength
        foreach(Room room in roomlist_room)
        {
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
                }
                else
                {
                    alllength.Add(longest);
                    longest = 0;
                    i = 0;
                }
            }
            i++;
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
            /// prints the arrows under the page
            if(allroomlist.Count == 1 )
            {
                Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                Console.Write($"│ {new String(' ', Math.Max(0, alllength[page]/2 - 2 ))} {page+1}/{allroomlist.Count}");
                Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2 - 1 * j ))} │\n");
                Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
            }
            else
            {
                if(page == 0)
                {
                    Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                    Console.Write($"│ {new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length + 1 ))} {page+1}/{allroomlist.Count}");
                    Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length - 1 * j ))} -> │\n");
                    Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                }
                else if(page == allroomlist.Count-1)
                {
                    Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                    Console.Write($"│ <- {new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length - 2 ))} {page+1}/{allroomlist.Count}");
                    Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length + 2 - j + 1 ))} │\n");
                    Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                }
                else
                {
                    Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                    Console.Write($"│ <-{new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length - 1 ))} {page+1}/{allroomlist.Count}");
                    Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length - j ))} -> │\n");
                    Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                }
            }
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

    /// <summary>
    /// gives the user an interface with two choises ont to confirm and on to deny
    /// </summary>
    /// <param name="prefix">A string of text printed before the selected value.</param>
    /// <param name="text">A string containing the question to confirm.</param>
    /// <param name="options"> a list eith strings of options currently only to one to confirm and one to deny</param>
    /// <returns> a boolean true to cofirm and false to deny</returns>
    public static bool? ConformationRoom(string prefix = "", string text = "", List<string> options = null)
    {
        int choice = 0;
        int longest = 0;
        int i = 0;
        /// if no list is given it creates one wit options save and cancel
        if (options == null)
        {
            options ??= new List<string>();
            options.Add("Save");
            options.Add("Discard");
        }
        i = 1;
        /// checks for longest string
        foreach(string option in options)
        {
            string option_str = $"{i}. "+ option + ".";
            if(option_str.Length > longest)
            {
                longest = option_str.Length;
            }
            i++;
        }
        if (prefix.Length > longest)
        {
            longest = prefix.Length;
        }
        ConsoleKey key;
        /// prints UI
        do
        {
            /// prints text if given
            Console.Clear();
            Console.WriteLine("Press escape to go back.\n");
            if(text != "")
            {
                Console.WriteLine(text +"\n");
            }
            /// prints header
            Console.WriteLine($"┌─{prefix}{new String('─', Math.Max(0, longest - prefix.Length))}─┐");
            i = 0;
            /// prints options
            foreach(string option in options)
            {
                Console.Write("│ ");
                /// if currently selected make the background darkgray instead of black
                if (i == choice){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                Console.Write($"{i+1}. {options[i]}.");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{new String(' ', Math.Max(0, longest - $"{i+1}. {options[i]}.".Length))} │\n");
                i++;
            }
            Console.Write($"└─{new string('─', Math.Max(0, longest ))}─┘\n");
            /// lets user go through options
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow && choice > 0) 
            {  
                Console.Clear();
                choice --;
            }
            else if (key == ConsoleKey.DownArrow && choice < options.Count-1)
            {  
                Console.Clear();
                choice ++;
            }
            /// returns true if first option is chosen and fals if second option is chosen
            else if (key == ConsoleKey.Enter)
            { 
                if (choice == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }while(key != ConsoleKey.Escape);
        return null;
    }
}