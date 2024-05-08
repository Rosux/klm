public static class RoomLogic
{
    private static RoomAccess r = new RoomAccess();

    public static void CreateRoom(int GivenCapacity)
    {
        Room newRoom = new Room(GivenCapacity);
        Console.Clear();
        bool? choice = ConformationRoom("Save this room?", $"New room created with capacity: {GivenCapacity}.");
        if(choice!= null)
        {
            if((bool)choice)
            {
                int newRoomId = r.AddToRoomTable(newRoom);
                Console.WriteLine($"New room created: \nID: {newRoomId}. \nCapacity: {GivenCapacity}.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine($"Room discarded.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
        else
        {
            RoomMenu.Action2();
        }
    }
    public static void RemoveRoom(Room room)
    {
        List<string> options = new List<string>(){"Yes", "No"};
        bool? choice = ConformationRoom($"remove this room?", $"Selected room:\nId: {room.Id}.\nCapacity: {room.Capacity}.",options);
        if(choice!= null)
        {
            if((bool)choice)
            {
                r.RemoveFromRoomTable(room.Id);
                Console.WriteLine($"Room succesfully removed.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine($"Room not removed.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
        else
        {
            RoomMenu.Action3();
        }
    }
    public static void EditRoom(Room room, int capacity)
    {
        List<string> options = new List<string>(){"Yes", "No"};
        string text = $"Old room: {r.GetRoom(room.Id)}.\n" + $"New room: Room-ID = {room.Id} Capacity = {capacity}.";
        bool? choice = ConformationRoom($"Save these changes?", text, options);
        if(choice!= null)
        {
            if((bool)choice)
            {
                r.EditFromRoomTable(room.Id, capacity);
                Console.WriteLine($"Changes saved succesfully.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine($"Changes not saved.\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
        else
        {
            RoomMenu.Action4(room);
        }
    }
    public static void FetchRoom(int roomid)
    {
        if (r.GetRoom(roomid) != "")
        {
            Console.WriteLine("Room: ");
            Console.WriteLine(r.GetRoom(roomid));
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
        else
        {
            Console.WriteLine($"there is no room with ID {roomid}.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
    public static List<string> GetAllRooms()
    {
        return r._getAllRooms();
    }

    public static Room? ChooseRoom(string prefix = "Choose a room") 
    {
        int page = 0;
        int i = 0;
        int choice = 0;
        List<List<Room>> allroomlist = new List<List<Room>>();
        List<Room> roomlist_room = r.GetAllRooms();
        List<Room> roomlist_p = new List<Room>();
        List<int> alllength = new List<int>();
        int longest = 0;

        foreach(Room room in roomlist_room)
        {
            string room_str = $"Id: {room.Id}, capacity: {room.Capacity}";
            if(room_str.Length > longest)
            {
                longest = room_str.Length;
            }
            if(i == 10)
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
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("Press escape to exit.\n");
            Console.WriteLine($"┌─{prefix}{new String('─', Math.Max(0, alllength[page] - prefix.Length))}─┐");
            i = 0 +10 * page;
            foreach(Room room in allroomlist[page])
            {
                string zin = $"Id: {room.Id}, capacity: {room.Capacity}";
                Console.Write("│ ");
                if (i == choice){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                Console.Write(zin);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{new String(' ', Math.Max(0, alllength[page] - zin.Length))} │\n");
                i++;
            }
            if(allroomlist.Count == 1 )
            {
                Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                Console.Write($"│ {new String(' ', Math.Max(0, alllength[page]/2-2 ))} {page+1}/{allroomlist.Count}");
                Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2-1 ))} │\n");
                Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
            }
            else
            {
                if(page == 0)
                {
                    Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                    Console.Write($"│ {new String(' ', Math.Max(0, alllength[page]/2-2 ))} {page+1}/{allroomlist.Count}");
                    Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2-4 ))} -> │\n");
                    Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                }
                else if(page == allroomlist.Count-1)
                {
                    Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                    Console.Write($"│ <- {new String(' ', Math.Max(0, alllength[page]/2-4 ))} {page+1}/{allroomlist.Count}");
                    Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2-2 ))} │\n");
                    Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                }
                else
                {
                    Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                    Console.Write($"│ <-{new String(' ', Math.Max(0, alllength[page]/2-4 ))} {page+1}/{allroomlist.Count}");
                    Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2-4 ))} -> │\n");
                    Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                }
            }
            key = Console.ReadKey(true).Key;
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
            else if (key == ConsoleKey.Enter)
            {  
                return roomlist_room[choice];
                break;
            }
        } while (key != ConsoleKey.Escape);   
        return null;
    }
    
    public static bool? ConformationRoom(string prefix = "", string text = "", List<string> options = null)
    {
        int choice = 0;
        int longest = 0;
        int i = 0;
        if (options == null)
        {
            options ??= new List<string>();
            options.Add("Save");
            options.Add("Discard");
        }
        i = 1;
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
        Console.WriteLine(options[0]);
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("Press escape to go back.\n");
            if(text != "")
            {
                Console.WriteLine(text +"\n");
            }
            Console.WriteLine($"┌─{prefix}{new String('─', Math.Max(0, longest - prefix.Length))}─┐");
            i = 0;
            foreach(string option in options)
            {
                Console.Write("│ ");
                if (i == choice){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                Console.Write($"{i+1}. {options[i]}.");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{new String(' ', Math.Max(0, longest - $"{i+1}. {options[i]}.".Length))} │\n");
                i++;
            }
            Console.Write($"└─{new string('─', Math.Max(0, longest ))}─┘\n");
            
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