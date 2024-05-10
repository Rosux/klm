public class RoomMenu
{   
    private static RoomAccess r = new RoomAccess();
    public static void AdminOverView()
    {
        bool running = true;
        while(running)
        {
            Console.WriteLine("Welcome to Admin overview for Rooms");
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Show all rooms", ()=>{
                    Action1();
                }},
                {"2. Add a room", ()=>{
                    Action2();
                }},
                {"3. Remove a room", ()=>{
                    Action3();
                }},
                {"4. Edit a room", ()=>{
                    Action4();
                }},
                {"5. Show specified room", ()=>{
                    Action5();
                }},
                {"6. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }
    private static void Action1()
    {
        Console.Clear();
        PrintAllRooms();
    }
    public static void Action2()
    {
        Console.Clear();
        int? GivenCapacity_p = MenuHelper.SelectInteger("Select capacity for new room: ", "", true, 0, 1, 2147483647);
        if (GivenCapacity_p  != null)
        {
            int GivenCapacity = (int)GivenCapacity_p;
            RoomLogic.CreateRoom(GivenCapacity);
        }
        else
        {
            Console.WriteLine("Action cancelled.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
    public static void Action3()
    {
        List<Room> listroom = r.GetAllRooms();
        if (listroom.Count != 0)
        {
            Room? selectRoom_p = RoomLogic.ChooseRoom("Pick room to remove");
            if (selectRoom_p == null)
            {
                Console.WriteLine("Action cancelled.");
                Console.Write($"\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Room selectedRoom = (Room)selectRoom_p;
                RoomLogic.RemoveRoom(selectedRoom);
            }
        }
        else
        {
            Console.WriteLine("There are no rooms to remove.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
    public static void Action4(Room selectedroom = null)
    {
        List<Room> listroom = r.GetAllRooms();
        if (listroom.Count != 0)
        {
            Room? selectRoom_p = null;
            if(selectedroom != null)
            {
                selectRoom_p = selectedroom;
            }
            else
            {
                selectRoom_p = RoomLogic.ChooseRoom("Pick room to edit");
            }
            if (selectRoom_p == null)
            {
                Console.WriteLine("Action cancelled.");
                Console.Write($"\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Room selectedRoom = (Room)selectRoom_p;
                int? new_capacity_p = MenuHelper.SelectInteger("Select new capacity: ", $"Current capacity: {selectedRoom.Capacity}.", true, 0, 1, 2147483647);
                if(new_capacity_p == null)
                {
                    Action4();
                }
                else
                {
                    int  new_capacity = (int) new_capacity_p;
                    RoomLogic.EditRoom(selectedRoom, new_capacity);
                }
            }
        }
        else
        {
            Console.WriteLine("There are no rooms to edit.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
    private static void Action5()
    {
        Console.Clear();
        List<Room> listroom = r.GetAllRooms();
        if (listroom.Count != 0)
        {
            int? roomid_p = MenuHelper.SelectInteger("Select room id: ", "", true, 0, 1, 2147483647);
            if (roomid_p != null)
            {
                int roomid = (int) roomid_p;
                RoomLogic.FetchRoom(roomid);
            }
            else
            {
                Console.WriteLine("Action cancelled.");
                Console.Write($"\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
        else
        {
            Console.WriteLine("There are no rooms currently.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    private static void PrintAllRooms()
    {
        int page = 0;
        int i = 0;
        int j = 0;
        List<List<Room>> allroomlist = new List<List<Room>>();
        List<Room> roomlist_room = r.GetAllRooms();
        List<Room> roomlist_p = new List<Room>();
        List<int> alllength = new List<int>();
        int longest = 0;
        bool isEmpty = !roomlist_room.Any();
        foreach(Room room in roomlist_room)
        {
            string room_str = $"Id: {room.Id}, capacity: {room.Capacity}";
            if(room_str.Length > longest)
            {
                longest = room_str.Length;
            }
            if(i == 9)
            {
                alllength.Add(longest);
                longest = 0;
                i = 0;
            }
            i++;
        }
        if(longest != 0)
        {
            alllength.Add(longest);
        }
        if(!isEmpty)
        {
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
                if(alllength[page] % 2 == 0)
                {
                    j = 2;
                }
                else
                {
                    j = 1;
                }
                Console.WriteLine($"┌─All rooms{new String('─', Math.Max(0, alllength[page] - "All rooms".Length))}─┐");
                foreach(Room room in allroomlist[page])
                {
                    string zin = $"Id: {room.Id}, capacity: {room.Capacity}";
                    Console.Write("│ ");
                    Console.Write(zin);
                    Console.Write($"{new String(' ', Math.Max(0, alllength[page] - zin.Length))} │\n");
                }
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
                if (key == ConsoleKey.RightArrow && page != allroomlist.Count-1)
                {  
                    Console.Clear();
                    page ++;
                }
                else if (key == ConsoleKey.LeftArrow && page != 0)
                {  
                    Console.Clear();
                    page --;

                }
            } while (key != ConsoleKey.Escape);   
        }
        else
        {
            Console.WriteLine("There are no rooms currently.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}