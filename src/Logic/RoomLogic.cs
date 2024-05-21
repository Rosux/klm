/// <summary>
/// This class handles stuff like adding/removing/editing rooms
/// </summary>
public static class RoomLogic
{
    private static RoomAccess r = new RoomAccess();

    /// <summary>
    /// Creates a menu of 4 options to choose from and a back option. Options are: "Show room", "Add room", "Remove room", "Edit room".
    /// </summary>
    public static void Menu()
    {
        bool running = true;
        while(running)
        {
            // gives the user a UI with all options
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Show all rooms", ()=>{
                    ShowAllRooms();
                }},
                {"2. Add a room", ()=>{
                    AddRoom();
                }},
                {"3. Remove a room", ()=>{
                    RemoveRoom();
                }},
                {"4. Edit a room", ()=>{
                    EditRoom();
                }},
                {"5. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }

    /// <summary>
    /// Prints all rooms an a neat UI and has the option to go back.
    /// </summary>
    public static void ShowAllRooms()
    {
        // count all the rooms. if there are none show a "No rooms found" text otherwise print the rooms
        if(r.GetAllRooms().Count == 0){
            RoomMenu.NoRoomsFoundNotification();
        }else{
            RoomMenu.PrintAllRooms();
        }
    }

    /// <summary>
    /// Ask the user to select a room and to delete it. Has an option to go back at any time.
    /// </summary>
    public static void RemoveRoom()
    {
        // convert all rooms to a dictionary
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        foreach(Room room in RoomLogic.r.GetAllRooms()){
            rooms.Add($"{room.Id}: {room.Capacity}", room);
        }
        // if there are no rooms show the user a "No rooms found" text
        if(rooms.Count == 0){
            RoomMenu.NoRoomsFoundNotification();
            return;
        }
        // ask the user to select a room for deletion
        Room? selectedRoom = MenuHelper.SelectFromList("Select a room to delete", true, rooms);
        if(selectedRoom == null){
            // user pressed escape so dont delete anything and return to the menu
            return;
        }else{
            // ask the user if theyre sure they want to delete the room
            bool deletion = MenuHelper.Confirm($"Are you sure you want to delete the selected room:\nId: {selectedRoom.Id}\nCapacity: {selectedRoom.Capacity}");
            if(deletion){
                // if the user answered yes remove the room and show the user if it worked or not
                bool success = r.RemoveRoom(selectedRoom.Id);
                RoomMenu.RoomDeletedNotification(success);
            }else{
                // user answered no so dont delete anything and return to the menu
                return;
            }
        }
    }

    /// <summary>
    /// Ask the user to make a new room. saves it to the database. Has the option to go back at any time.
    /// </summary>
    public static void AddRoom()
    {
        // TODO add AddRoom logic here (this file handles the data as in saves it to the database using r.whatevermethod and telling the RoomMenu.cs to print/ask stuff)
    }

    /// <summary>
    /// Ask the user to select a room and to make any edits for that room if they wish. Has the option to go back at any time.
    /// </summary>
    public static void EditRoom()
    {
        // TODO implement editing a room. EditRoom logic here (this file handles the data as in saves/updates it in the database and tells RoomMenu.cs what to print/ask)
    }
}