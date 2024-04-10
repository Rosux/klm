public static class ReservationMenu
{
    private static RoomAccess RoomsAccess = new RoomAccess();
    private static ReservationAccess ReservationAccess = new ReservationAccess();       
    public static Reservation? BookReservation(){
        int roomMaxSize = RoomsAccess.GetMaxRoomCapacity();
        int GroupSize = MenuHelper.SelectInteger("Select your group size:", "", 1, 1, roomMaxSize);
        // create dict of room and room names
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        int RoomCounter = 1;
        foreach(Room room in RoomsAccess.GetAllRooms(GroupSize)){
            rooms.Add($"Room {RoomCounter} (Maximum size: {room.Capacity})", room);
            RoomCounter++;
        }
        Room SelectedRoom = MenuHelper.SelectFromList("Choose the room you want to reserve:", rooms);
        // Console.WriteLine(SelectedRoom.id);
        // Console.ReadLine();
        DateOnly startDate = MenuHelper.SelectDate("Select at what date you want to start your reservation:", null, DateOnly.FromDateTime(DateTime.Now), null);
        TimeOnly startTime = MenuHelper.SelectTime("Select at what time you want to start your reservation:", "", new TimeOnly(), null, null);
        DateOnly endDate = MenuHelper.SelectDate("Select at what date you want to end your reservation:", null, startDate, null);
        TimeOnly endTime = new TimeOnly();
        if (startDate == endDate){
            endTime = MenuHelper.SelectTime("Select at what time you want to end your reservation:", "", startTime.AddMinutes(1), startTime.AddMinutes(1), null);
        }else{
            endTime = MenuHelper.SelectTime("Select at what time you want to end your reservation:");
        }

        TimeLine.Holder t = new TimeLine.Holder();
        t.Add(
            new Break(15),
            new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, 0),
            new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, 0).AddMinutes(new Break(15).Time)
        );
        // t.AddBreak();

        // TODO remove (for debug purposes only)
        Program.CurrentUser = new User(69, "Ad", "Min", "hihihi", "uwu-onichan-senpai", UserRole.ADMIN);
        
        if (Program.CurrentUser != null){
            Reservation r = new Reservation(
                SelectedRoom.Id,
                Program.CurrentUser.Id,
                GroupSize,
                new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, 0),
                new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, 0),
                (double)(GroupSize * 12.0),
                t
            );
            Console.WriteLine(r);
            Console.ReadKey();
        }
        return null;
    }
    public static Reservation? ShowReservation()
    {
    List<Reservation> reservations = ReservationAccess.ReadReservations();
    if (reservations.Count == 0)
    {
        Console.WriteLine("There are no reservations available to show.");
        return null;
    }
    else
    {
        Console.WriteLine("Available Reservations:");
        foreach (Reservation reservation in reservations)
        {
            Console.WriteLine($"ID: {reservation.Id}, Room ID: {reservation.RoomId}, User ID: {reservation.UserId}, Group Size: {reservation.GroupSize}, Start Date: {reservation.StartDate}, End Date: {reservation.EndDate}, Price: {reservation.Price}");
        }

        Console.Write("Enter the ID of the reservation to select: ");
        int selectedId;
        if (int.TryParse(Console.ReadLine(), out selectedId))
        {
            foreach (Reservation reservation in reservations)
            {
                if (reservation.Id == selectedId)
                {
                    return reservation;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid ID.");
        }
    }
    return null;
    }
}