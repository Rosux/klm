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
    private static void Action2()
    {
        Console.Clear();
        int? GivenCapacity_p = MenuHelper.SelectInteger("Select capacity: ", "", true, 0, 1, 2147483647);
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
    private static void Action3()
    {
        List<Room> rooms = r.GetAllRooms();
        if (rooms.Count == 0)
        {
            Console.WriteLine("There are no rooms.");
        }
        else
        {
            Dictionary<string, Room> roomOptions = new Dictionary<string, Room>();
            foreach (Room room in rooms)
            {
                roomOptions.Add($"Room ID: {room.Id}, Room capacity: {room.Capacity}", room);
            }

            roomOptions.Add("Return to menu", null);

            Room selectedRoom = null;
            do
            {
                selectedRoom = MenuHelper.SelectFromList("Choose a room to remove", roomOptions);

                if (selectedRoom != null)
                {
                    Console.Clear();
                    RoomLogic.RemoveRoom(selectedRoom.Id);
                    break;
                    
                    Console.Clear();
                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }
                }
            } while (selectedRoom != null);
        }
    }
    private static void Action4()
    {
        List<Room> rooms = r.GetAllRooms();
        if (rooms.Count == 0)
        {
            Console.WriteLine("There are no rooms.");
        }
        else
        {
            Dictionary<string, Room> roomOptions = new Dictionary<string, Room>();
            foreach (Room room in rooms)
            {
                roomOptions.Add($"Room ID: {room.Id}, Room capacity: {room.Capacity}", room);
            }

            roomOptions.Add("Return to menu", null);

            Room selectedRoom = null;
            do
            {
                selectedRoom = MenuHelper.SelectFromList("Choose a room to edit", roomOptions);

                if (selectedRoom != null)
                {
                    Console.Clear();
                    int? new_capacity_p = MenuHelper.SelectInteger("Select new capacity: ", "", true, 0, 1, 2147483647);
                    if(new_capacity_p == null)
                    {
                        Console.WriteLine("Action cancelled.");
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey(true);
                        break;
                    }
                    else
                    {
                        int  new_capacity = (int) new_capacity_p;
                        RoomLogic.EditRoom(new_capacity, selectedRoom.Id);
                        break;
                    }
                    
                    Console.Clear();
                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }
                }
            } while (selectedRoom != null);
        }
    }
    private static void Action5()
    {
        Console.Clear();
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
    private static void PrintAllRooms()
    {
        string allrooms = "";
        int page = 0;
        int i = 0;
        int j = 0;
        List<string> allroomlist = new List<string>();
        List<int> alllength = new List<int>();
        int longest = 0;
        List<string> roomlist = RoomLogic.GetAllRooms();
        bool isEmpty = !roomlist.Any();

        foreach(string str in roomlist)
        {
            if(str.Length > longest)
            {
                longest = str.Length;
            }
            if(i == 10)
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
            j = 0;
            allrooms = allrooms + ($"┌─All Rooms{new String('─', Math.Max(0, alllength[j] - "all rooms".Length))}─┐\n");
            foreach(string rooms in roomlist)
            {
                allrooms = allrooms + ($"│ {rooms}{new String(' ', Math.Max(0, alllength[j] - rooms.Length))} │\n");
                i++;
                if (i == 10)
                {
                    allrooms = allrooms + ($"├─{new string('─', Math.Max(0, alllength[j] ))}─┤\n");
                    allroomlist.Add(allrooms);
                    i=0;
                    allrooms = "";
                    j++;
                    allrooms = allrooms + ($"┌─All Rooms{new String('─', Math.Max(0, alllength[j] - "all rooms".Length))}─┐\n");
                }
            }
            if(allrooms != $"┌─All Rooms{new String('─', Math.Max(0, alllength[j] - "all rooms".Length))}─┐\n")
            {
                allrooms = allrooms + ($"├─{new string('─', Math.Max(0, alllength[j] ))}─┤\n");
                allroomlist.Add(allrooms);
            }
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Press escape to exit.\n");
                allrooms = allroomlist[page];
                if(allroomlist.Count == 1 )
                {
                    allrooms = allrooms + ($"│ {new String(' ', Math.Max(0, alllength[page]/2 ))} {page+1}/{allroomlist.Count}");
                    allrooms = allrooms + ($"{ new String(' ', Math.Max(0, alllength[page]/2-3 ))} │\n");
                    allrooms = allrooms + ($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                    Console.WriteLine(allrooms);
                }
                else
                {
                    if(page == 0)
                    {
                        allrooms = allrooms + ($"│ {new String(' ', Math.Max(0, alllength[page]/2-2 ))} {page+1}/{allroomlist.Count}");
                        allrooms = allrooms + ($"{ new String(' ', Math.Max(0, alllength[page]/2-4 ))} -> │\n");
                        allrooms = allrooms + ($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                        Console.WriteLine(allrooms);
                    }
                    else if(page == allroomlist.Count-1)
                    {
                        allrooms = allrooms + ($"│ <-{new String(' ', Math.Max(0, alllength[page]/2-4 ))} {page+1}/{allroomlist.Count}");
                        allrooms = allrooms + ($"{ new String(' ', Math.Max(0, alllength[page]/2-1 ))} │\n");
                        allrooms = allrooms + ($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                        Console.WriteLine(allrooms);
                    }
                    else
                    {
                        allrooms = allrooms + ($"│ <-{new String(' ', Math.Max(0, alllength[page]/2-4 ))} {page+1}/{allroomlist.Count}");
                        allrooms = allrooms + ($"{ new String(' ', Math.Max(0, alllength[page]/2-4 ))} -> │\n");
                        allrooms = allrooms + ($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                        Console.WriteLine(allrooms);
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
            Console.WriteLine("There are currently no rooms.");
        } 
    }
}