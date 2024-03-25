public static class RoomLogic
{
    private static RoomAccess r = new RoomAccess();

    public static void CreateRoom(int GivenCapacity)
    {
        Room newRoom = new Room(GivenCapacity);
        int newRoomId = r.AddToRoomTable(newRoom);
        Console.Clear();
        Console.WriteLine($"New room created. \nID:{newRoomId} \nCapacity: {GivenCapacity}\n\nPress any key to continue...");
        Console.ReadKey();
    }
    public static void RemoveRoom(int roomid)
    {
        r.RemoveFromRoomTable(roomid);
        Console.Write($"Room with id {roomid} removed.\n\nPress any key to continue...");
        Console.ReadKey();
    }
    public static void EditRoom(int capacity, int roomid)
    {
        Console.Write($"Old room:  {r.GetRoom(roomid)}.\n");
        r.EditFromRoomTable(roomid, capacity);
        Console.Write($"New room: {r.GetRoom(roomid)}\n\nPress any key to continue...");
        Console.ReadKey();
    }
    public static void FetchRoom(int roomid)
    {
        Console.WriteLine("Room: ");
        Console.WriteLine(r.GetRoom(roomid));
        Console.Write($"\n\nPress any key to continue...");
        Console.ReadKey();
    }
    public static List<string> GetAllRooms()
    {
        return r._getAllRooms();
    }

}