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
        Console.WriteLine("\n\nPress any key to continue");
        Console.ReadKey();
    }
    private static void Action2()
    {
        Console.Clear();
        int GivenCapacity = MenuHelper.SelectInteger("Capacity: ",0,1,2147483647);
        if (GivenCapacity > 0)
        {
            RoomLogic.CreateRoom(GivenCapacity);
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
                selectedRoom = MenuHelper.SelectFromList("Choose a room to remove", roomOptions);

                if (selectedRoom != null)
                {
                    Console.Clear();
                    int new_capacity = MenuHelper.SelectInteger("Select new capacity: ",0,1,2147483647);
                    RoomLogic.EditRoom(new_capacity, selectedRoom.Id);
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
    private static void Action5()
    {
        Console.Clear();
        int roomid = MenuHelper.SelectInteger("Select room id: ",0,1,2147483647);
        RoomLogic.FetchRoom(roomid);
    

    }
    private static void PrintAllRooms()
    {
        int longest = 0;
        List<string> roomlist = RoomLogic.GetAllRooms();
        bool isEmpty = !roomlist.Any();

        foreach(string str in roomlist)
        {
            if(str.Length > longest)
            {
                longest = str.Length;
            }
        }

        Console.WriteLine($"┌─All Rooms{new String('─', Math.Max(0, longest - "all rooms".Length))}─┐");
        if(!isEmpty)
        {
            foreach(string rooms in roomlist)
            {
                Console.WriteLine($"│ {rooms}{new String(' ', Math.Max(0, longest - rooms.Length))} │");
            }
            Console.WriteLine($"└─{new string('─', Math.Max(0, longest ))}─┘\n");
        }
        else
        {
            Console.WriteLine("There are currently no rooms.");
        } 
    }
}