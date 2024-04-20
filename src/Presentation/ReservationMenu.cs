public static class ReservationMenu
{
    private static RoomAccess RoomsAccess = new RoomAccess();
    private static ReservationAccess ReservationAccess = new ReservationAccess();       
    private static ConsumptionAccess Consumptions = new ConsumptionAccess();

    public static Reservation? BookReservation(){
        double totalPrice = 0.0;
        int roomMaxSize = RoomsAccess.GetMaxRoomCapacity();
        int GroupSize = MenuHelper.SelectInteger("Select your group size:", "", 1, 1, roomMaxSize);
        totalPrice += (double)GroupSize * 12.0;
        // create dict of room and room names
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        int RoomCounter = 1;
        foreach(Room room in RoomsAccess.GetAllRooms(GroupSize)){
            rooms.Add($"Room {RoomCounter} (Maximum size: {room.Capacity})", room);
            RoomCounter++;
        }
        Room SelectedRoom = MenuHelper.SelectFromList("Choose the room you want to reserve:", rooms);
        DateOnly startDate = MenuHelper.SelectDate("Select at what date you want to start your reservation:", null, DateOnly.FromDateTime(DateTime.Now), null);
        TimeOnly startTime = MenuHelper.SelectTime("Select at what time you want to start your reservation:", "", new TimeOnly(), null, null);
        DateOnly endDate = MenuHelper.SelectDate("Select at what date you want to end your reservation:", null, startDate, null);
        TimeOnly endTime = new TimeOnly();
        if (startDate == endDate){
            endTime = MenuHelper.SelectTime("Select at what time you want to end your reservation:", "", startTime.AddMinutes(1), startTime.AddMinutes(1), null);
        }else{
            endTime = MenuHelper.SelectTime("Select at what time you want to end your reservation:");
        }

        TimeLine.Holder timeline = new TimeLine.Holder();

        bool addingToTimeline = true;
        bool save = false;
        while(addingToTimeline){
            MenuHelper.SelectOptions($"Select an option", new Dictionary<string, Action>(){
                {"Add Movies/Series", ()=>{
                    // TODO implement movies/series picking (better fucking ui pls)
                }},
                {"Add Consumptions", ()=>{
                    Dictionary<string, Consumption> consumptions = new Dictionary<string, Consumption>();
                    foreach(Consumption food in Consumptions.ReadConsumption()){
                        consumptions.Add($"{food.Name} (Price: {food.Price.ToString("0.00")})", food);
                    }
                    Consumption c = MenuHelper.SelectFromList("Select the food", consumptions);
                    DateOnly d = MenuHelper.SelectDate("Select the date and time of your food", null, startDate, endDate);
                    TimeOnly t;
                    if (d == startDate && d == endDate){
                        t = MenuHelper.SelectTime("Select the date and time of your food", "", new TimeOnly(), startTime, endTime);
                    }else if (d == startDate){
                        t = MenuHelper.SelectTime("Select the date and time of your food", "", new TimeOnly(), startTime, TimeOnly.MaxValue);
                    }else if (d == endDate){
                        t = MenuHelper.SelectTime("Select the date and time of your food", "", new TimeOnly(), TimeOnly.MinValue, endTime);
                    }else{
                        t = MenuHelper.SelectTime("Select the date and time of your food", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
                    }
                    timeline.Add(
                        c,
                        new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                        new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0)
                    );
                    totalPrice += c.Price;
                }},
                {"Add Breaks", ()=>{
                    int breakTime = MenuHelper.SelectInteger("Select the length for your break in minutes", 1, 1, int.MaxValue);
                    DateOnly d = MenuHelper.SelectDate("Select break date and time", null, startDate, endDate);
                    TimeOnly t;
                    if (d == startDate && d == endDate){
                        t = MenuHelper.SelectTime("Select the date and time for your break", "", new TimeOnly(), startTime, endTime);
                    }else if (d == startDate){
                        t = MenuHelper.SelectTime("Select the date and time for your break", "", new TimeOnly(), startTime, TimeOnly.MaxValue);
                    }else if (d == endDate){
                        t = MenuHelper.SelectTime("Select the date and time for your break", "", new TimeOnly(), TimeOnly.MinValue, endTime);
                    }else{
                        t = MenuHelper.SelectTime("Select the date and time for your break", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
                    }
                    timeline.Add(
                        new Break(breakTime),
                        new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                        new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0).AddMinutes(breakTime)
                    );
                }},
                {"Discard", ()=>{
                    save = false;
                    addingToTimeline = false;
                }},
                {"Save", ()=>{
                    save = true;
                    addingToTimeline = false;
                }},
            });
        }
        
        if (Program.CurrentUser != null && save){
            Reservation r = new Reservation(
                SelectedRoom.Id,
                Program.CurrentUser.Id,
                GroupSize,
                new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, 0),
                new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, 0),
                (double)(GroupSize * 12.0) + totalPrice,
                timeline
            );
            return r;
        }else if(!save){
            return null;
        }
        return null;
    }
    public static Reservation? ShowReservation(int loggedInUserId)
    {
    List<Reservation> reservations = ReservationAccess.ReadReservations(loggedInUserId);
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
    public static void Error(){
        Console.Clear();
        Console.WriteLine("An error occured. Please try again later.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    
    public static void Saved(){
        Console.Clear();
        Console.WriteLine("Your reservation has been saved succesfully.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
}