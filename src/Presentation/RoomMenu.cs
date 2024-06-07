/// <summary>
/// This class holds methods for printing information and asking information from the user about rooms.
/// </summary>
public class RoomMenu
{
    private static RoomAccess r = new RoomAccess();

    /// <summary>
    /// Print all rooms. Has an option to go back.
    /// </summary>
    public static void PrintAllRooms()
    {
        Console.CursorVisible = false;
        Console.Clear();
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        foreach(Room r in r.GetAllRooms()){
            rooms.Add($"Id: {r.Id}, Capacity: {r.Capacity}", r);
        }
        MenuHelper.SelectFromList("All Rooms", true, rooms);
    }

    /// <summary>
    /// Tell the user if the room has been deleted succesfully or not.
    /// </summary>
    /// <param name="success">A boolean indicating if the deletion was succesful</param>
    public static void RoomDeletedNotification(bool success)
    {
        Console.CursorVisible = false;
        Console.Clear();
        Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write(success ? "Room succesfully deleted" : "Room deletion failed, try again later");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Tell the user that there are no rooms.
    /// </summary>
    public static void NoRoomsFoundNotification()
    {
        Console.CursorVisible = false;
        Console.Clear();
        Console.Write("No rooms have been found\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}
