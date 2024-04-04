public static class ReservationMenu
{
    private static RoomAccess RoomsAccess = new RoomAccess();

    public static Reservation? BookReservation(){
        int roomMaxSize = RoomsAccess.GetMaxRoomCapacity();
        int GroupSize = MenuHelper.SelectInteger("Select your group size:", "", 1, 1, roomMaxSize);
        // create dict of room and room names
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        int RoomCounter = 1;
        foreach(Room room in RoomsAccess.GetAllRooms(GroupSize)){
            rooms.Add($"Room {RoomCounter} (Maximum size: {room.Capacity})", room);
        }
        Room SelectedRoom = MenuHelper.SelectFromList("Choose the room you want to reserve:", rooms);
        // Console.WriteLine(SelectedRoom.id);
        // Console.ReadLine();
        DateOnly startDate = MenuHelper.SelectDate("Select at what date you want to start your reservation:", null, DateOnly.FromDateTime(DateTime.Now), null);
        TimeOnly startTime = MenuHelper.SelectTime("Select at what time you want to start your reservation");
        DateOnly endDate = MenuHelper.SelectDate("Select at what date you want to end your reservation:", null, startDate, null);
        // TODO make a check to see if the endTime is after startTime and IF SO: ask the user again. OR implement a parameter in SelectTime where there can be a minimum and maximum set
        TimeOnly endTime = MenuHelper.SelectTime("Select at what time you want to end your reservation:");
        
        return null;
    }
}