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

}