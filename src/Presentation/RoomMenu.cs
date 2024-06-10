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
            rooms.Add($"Name: {r.RoomName}, Id: {r.Id}, Capacity: {r.Capacity}", r);
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
    /// Checks if there is a conflict with the given time range.
    /// </summary>
    /// <param name="startTime">Start time of the new item.</param>
    /// <param name="endTime">End time of the new item.</param>
    /// <param name="MovieOrSerie"> true = Movie, false = Serie
    /// <returns>bool Conflict, string conflicting title (for testing purposes), string conflicting time ("From ... to ...)</returns>
    public (bool Conflict, string ConflictingTitle, string ConflictingString) HasConflict(DateTime startTime, DateTime endTime)
    {
        foreach (var item in Items)
        {
            if (item.Action is Film film )
            {
                // Check if the new time range is also within any already added film 
                if (startTime < item.EndTime && endTime > item.StartTime)
                {
                    string ConflictingString = $"Movie {film.Title} is already scheduled from {item.StartTime.ToString("HH:mm")} to {item.EndTime.ToString("HH:mm")}";
                    return (true, film.Title, ConflictingString);
                }
            }
            if (item.Action is Episode episode)
            {
                if (startTime < item.EndTime && endTime > item.StartTime)
                {
                    string ConflictingString = $"Episode {episode.Title} is already scheduled from {item.StartTime.ToString("HH:mm")} to {item.EndTime.ToString("HH:mm")}";
                    return (true, episode.Title, ConflictingString);
                }
            }
        }
        return (false, null, null);
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
