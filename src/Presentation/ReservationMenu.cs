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
                {"Add Movies/Episode", ()=>{
                    object FilmOrEpisode = MenuHelper.SelectMovieOrEpisode();
                    if(FilmOrEpisode is Film){
                        Film film = (Film)FilmOrEpisode;
                        DateOnly d = MenuHelper.SelectDate($"Select the date and time you want {film.Title} to begin.", null, startDate, endDate);
                        TimeOnly t;
                        if (d == startDate && d == endDate){
                            t = MenuHelper.SelectTime($"Select the date and time you want {film.Title} to begin.", "", new TimeOnly(), startTime, endTime);
                        }else if (d == startDate){
                            t = MenuHelper.SelectTime($"Select the date and time you want {film.Title} to begin.", "", new TimeOnly(), startTime, TimeOnly.MaxValue);
                        }else if (d == endDate){
                            t = MenuHelper.SelectTime($"Select the date and time you want {film.Title} to begin.", "", new TimeOnly(), TimeOnly.MinValue, endTime);
                        }else{
                            t = MenuHelper.SelectTime($"Select the date and time you want {film.Title} to begin.", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
                        }
                        timeline.Add(
                        film,
                        new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                        new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0).AddMinutes(film.Duration)
                        );
                    }else if(FilmOrEpisode is Dictionary<Serie, List<Episode>>){
                        List<Episode> ruru = ((Dictionary<Serie, List<Episode>>)FilmOrEpisode).First().Value;
                        foreach(Episode episode in ruru)
                        {
                            DateOnly d = MenuHelper.SelectDate($"Select the date and time you want the episode {episode.Title} to begin.", null, startDate, endDate);
                            TimeOnly t;
                            if (d == startDate && d == endDate){
                                t = MenuHelper.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", new TimeOnly(), startTime, endTime);
                            }else if (d == startDate){
                                t = MenuHelper.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", new TimeOnly(), startTime, TimeOnly.MaxValue);
                            }else if (d == endDate){
                                t = MenuHelper.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", new TimeOnly(), TimeOnly.MinValue, endTime);
                            }else{
                                t = MenuHelper.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
                            }
                            timeline.Add(
                            episode,
                            new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                            new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0).AddMinutes(episode.Length)
                            );
                        }
                    }else if(FilmOrEpisode == null){
                        Console.WriteLine("nothing selected");
                    }
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

    /// <summary>
    /// gives a list of all reservations to pick one
    /// </summary>
    /// <returns>a object of Reservation</returns>
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
            Dictionary<string, Reservation> reservationOptions = new Dictionary<string, Reservation>();
            foreach (Reservation reservation in reservations)
            {
                reservationOptions.Add($"Reservation Number: {reservation.Id}, Room: {reservation.RoomId}", reservation);
            }

            reservationOptions.Add("Return to menu", null);

            Reservation selectedReservation = null;
            do
            {
                selectedReservation = MenuHelper.SelectFromList("My Reservations", reservationOptions);

                if (selectedReservation != null)
                {
                    Console.Clear();
                    return selectedReservation;

                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }
                }
            } while (selectedReservation != null);

            return null;
        }
    }

    /// <summary>
    /// gives a list of all reservations to pick one
    /// </summary>
    /// <returns>a object of Reservation</returns>
    public static Reservation? ShowAllReservation()
    {
        List<Reservation> reservations = ReservationAccess.ReadReservations();
        if (reservations.Count == 0)
        {
            Console.WriteLine("There are no reservations available to show.");
            return null;
        }
        else
        {
            Dictionary<string, Reservation> reservationOptions = new Dictionary<string, Reservation>();
            foreach (Reservation reservation in reservations)
            {
                reservationOptions.Add($"Reservation Number: {reservation.Id}, Room: {reservation.RoomId}", reservation);
            }

            reservationOptions.Add("Return to menu", null);

            Reservation selectedReservation = null;
            do
            {
                selectedReservation = MenuHelper.SelectFromList("My Reservations", reservationOptions);

                if (selectedReservation != null)
                {
                    Console.Clear();
                    return selectedReservation;

                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }
                }
            } while (selectedReservation != null);

            return null;
        }
    }
    /// <summary>
    /// shows a list of all reservation on or durig the given date
    /// </summary>
    /// <param name="date"></param>
    /// <returns>object of Reservations</returns>
    public static Reservation? ShowSpecificReservation(DateTime date)
    {
    List<Reservation> reservations = ReservationAccess.PickReservations(date);
    if (reservations.Count == 0)
    {
        Console.WriteLine("There are no reservations during this time period.");
        return null;
    }
    else
    {
        Dictionary<string, Reservation> reservationOptions = new Dictionary<string, Reservation>();
            foreach (Reservation reservation in reservations)
            {
                reservationOptions.Add($"Reservation Number: {reservation.Id}, Room: {reservation.RoomId}", reservation);
            }

            reservationOptions.Add("Return to menu", null);

            Reservation selectedReservation = null;
            do
            {
                selectedReservation = MenuHelper.SelectFromList("My Reservations", reservationOptions);

                if (selectedReservation != null)
                {
                    Console.Clear();
                    return selectedReservation;

                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }
                }
            } while (selectedReservation != null);

            return null;
        }
    }

    /// <summary>
    /// shows a list of all reservation on or durig the week of given date
    /// </summary>
    /// <param name="date"></param>
    /// <returns>object of Reservations</returns>
    public static Reservation? ShowSpecificReservationWeek(DateTime date)
    {
    List<Reservation> reservations = ReservationAccess.PickReservationsWeek(date);
    if (reservations.Count == 0)
    {
        Console.WriteLine("There are no reservations during this time period.");
        return null;
    }
    else
    {
        Dictionary<string, Reservation> reservationOptions = new Dictionary<string, Reservation>();
            foreach (Reservation reservation in reservations)
            {
                reservationOptions.Add($"Reservation Number: {reservation.Id}, Room: {reservation.RoomId}", reservation);
            }

            reservationOptions.Add("Return to menu", null);

            Reservation selectedReservation = null;
            do
            {
                selectedReservation = MenuHelper.SelectFromList("My Reservations", reservationOptions);

                if (selectedReservation != null)
                {
                    Console.Clear();
                    return selectedReservation;

                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }
                }
            } while (selectedReservation != null);

            return null;
        }
    }

    /// <summary>
    /// gives the user a list of all his reservations
    /// </summary>
    /// <returns>a object of Reservations</returns>
    public static Reservation? ShowSpecificReservationUser()
    {
    List<Reservation> reservations = ReservationAccess.PickReservationsUser();
    if (reservations.Count == 0)
    {
        Console.WriteLine("You have no reservations.");
        return null;
    }
    else
    {
        Dictionary<string, Reservation> reservationOptions = new Dictionary<string, Reservation>();
            foreach (Reservation reservation in reservations)
            {
                reservationOptions.Add($"Reservation Number: {reservation.Id}, Room: {reservation.RoomId}", reservation);
            }

            reservationOptions.Add("Return to menu", null);

            Reservation selectedReservation = null;
            do
            {
                selectedReservation = MenuHelper.SelectFromList("My Reservations", reservationOptions);

                if (selectedReservation != null)
                {
                    Console.Clear();
                    return selectedReservation;

                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                            break;
                    }
                }
            } while (selectedReservation != null);

            return null;
        }
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