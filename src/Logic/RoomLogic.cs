public static class RoomLogic
{
    private static RoomAccess r = new RoomAccess();

    public static void CreateRoom(int GivenCapacity)
    {
        Room newRoom = new Room(GivenCapacity);
        Console.Clear();
        MenuHelper.SelectOptions($"Save room with capcity: {GivenCapacity}?", new Dictionary<string, Action>(){
        {"1. Save.", ()=>{
            Console.Clear();
            int newRoomId = r.AddToRoomTable(newRoom);
            Console.WriteLine($"New room created: \nID: {newRoomId}. \nCapacity: {GivenCapacity}.\n\nPress any key to continue...");
            Console.ReadKey();
        }},
        {"2. Discard.", ()=>{
            Console.Clear();
            Console.WriteLine($"Room discarded.\n\nPress any key to continue...");
            Console.ReadKey();
        }},
        });
    }
    public static void RemoveRoom(int roomid)
    {
        MenuHelper.SelectOptions($"remove room {roomid}?", new Dictionary<string, Action>(){
        {"1. Remove.", ()=>{
            Console.Clear();
            r.RemoveFromRoomTable(roomid);
            Console.WriteLine($"Room with id {roomid} removed.\n\nPress any key to continue...");
            Console.ReadKey();
        }},
        {"2. Cancel.", ()=>{
            Console.Clear();
            Console.WriteLine($"Room not removed.\n\nPress any key to continue...");
            Console.ReadKey();
        }},
        });
    }
    public static void EditRoom(int capacity, int roomid)
    {
        Console.Write($"Old room: {r.GetRoom(roomid)}.\n");
        Console.Write($"New room: Room-ID = {roomid} Capacity = {capacity}.\n\nPress any key to continue...");
        Console.ReadKey();
        MenuHelper.SelectOptions($"Save these changes?", new Dictionary<string, Action>(){
        {"1. Save.", ()=>{
            Console.Clear();
            r.EditFromRoomTable(roomid, capacity);
            Console.WriteLine($"Changes saved succesfully.\n\nPress any key to continue...");
            Console.ReadKey();
        }},
        {"2. Cancel.", ()=>{
            Console.Clear();
            Console.WriteLine($"Changes not saved.\n\nPress any key to continue...");
            Console.ReadKey();
        }},
        });
    }
    public static void FetchRoom(int roomid)
    {
        if (r.GetRoom(roomid) != "")
        {
            Console.WriteLine("Room: ");
            Console.WriteLine(r.GetRoom(roomid));
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine($"there is no room with ID {roomid}.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey();
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
}