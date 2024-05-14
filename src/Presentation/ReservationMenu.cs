public static class ReservationMenu
{
    private static RoomAccess RoomsAccess = new RoomAccess();
    private static ConsumptionAccess Consumptions = new ConsumptionAccess();
    private static ReservationAccess ReservationAccess = new ReservationAccess();

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
        DateOnly endDate = MenuHelper.SelectDate("Select at what date you want to end your reservation:", startDate, startDate, null);
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
                    object? FilmOrEpisode = MenuHelper.SelectMovieOrEpisode();
                    if(FilmOrEpisode != null && FilmOrEpisode is Film){
                        Film film = (Film)FilmOrEpisode;
                        DateOnly d = MenuHelper.SelectDate($"Select the date and time you want {film.Title} to begin.", startDate, startDate, endDate);
                        TimeOnly t;
                        if (d == startDate && d == endDate){
                            t = MenuHelper.SelectTime($"Select the date and time you want {film.Title} to begin.", "", startTime, startTime, endTime);
                        }else if (d == startDate){
                            t = MenuHelper.SelectTime($"Select the date and time you want {film.Title} to begin.", "", startTime, startTime, TimeOnly.MaxValue);
                        }else if (d == endDate){
                            t = MenuHelper.SelectTime($"Select the date and time you want {film.Title} to begin.", "", new TimeOnly(), TimeOnly.MinValue, endTime);
                        }else{
                            t = MenuHelper.SelectTime($"Select the date and time you want {film.Title} to begin.", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
                        }
                        timeline.Add(
                            film,
                            new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                            new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0).AddMinutes(film.Runtime)
                        );
                    }else if(FilmOrEpisode != null && FilmOrEpisode is Dictionary<Serie, List<Episode>>){
                        List<Episode> episode_list = ((Dictionary<Serie, List<Episode>>)FilmOrEpisode).First().Value;
                        DateTime serieTime = new DateTime();
                        int lastDay = -1;
                        for(int i = 0; i < episode_list.Count; i++)
                        {
                            Episode episode = episode_list[i];
                            DateOnly d = MenuHelper.SelectDate($"Select the date and time you want the episode {episode.Title} to begin.", startDate, startDate, endDate);
                            TimeOnly t;
                            if(i == 0){
                                serieTime = new DateTime(d.Year, d.Month, d.Day, startTime.Hour, startTime.Minute, 0);
                                lastDay = d.Day;
                            }
                            if(d.Day != lastDay){
                                serieTime = DateTime.MinValue;
                            }
                            if(serieTime < new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, 0)){
                                serieTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, 0);
                            }
                            if(serieTime > new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, 0)){
                                serieTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, 0);
                            }
                            if (d == startDate && d == endDate){
                                t = MenuHelper.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", TimeOnly.FromDateTime(serieTime), startTime, endTime);
                            }else if (d == startDate){
                                t = MenuHelper.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", TimeOnly.FromDateTime(serieTime), startTime, TimeOnly.MaxValue);
                            }else if (d == endDate){
                                t = MenuHelper.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", TimeOnly.FromDateTime(serieTime), TimeOnly.MinValue, endTime);
                            }else{
                                t = MenuHelper.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", TimeOnly.FromDateTime(serieTime), TimeOnly.MinValue, TimeOnly.MaxValue);
                            }
                            lastDay = d.Day;
                            timeline.Add(
                                episode,
                                new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                                new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0).AddMinutes(episode.Length)
                            );
                            if(i == 0){
                                serieTime = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0).AddMinutes(episode.Length);
                            }else{
                                serieTime = serieTime.AddMinutes(episode.Length);
                            }
                        }
                    }
                }},
                {"Add Consumptions", ()=>{
                    DateOnly d = MenuHelper.SelectDate("Select the date and time of your food", startDate, startDate, endDate);
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
                    Dictionary<string, Consumption> consumptions = new Dictionary<string, Consumption>();
                    foreach(Consumption food in Consumptions.ReadConsumption()){
                        if(food.EndTime.Hour == 0 && food.EndTime.Minute == 0){
                            if(t >= food.StartTime){
                                consumptions.Add($"{food.Name} (Price: {food.Price.ToString("0.00")})", food);
                            }
                        }else{
                            if(t >= food.StartTime && t <= food.EndTime){
                                consumptions.Add($"{food.Name} (Price: {food.Price.ToString("0.00")})", food);
                            }
                        }
                    }
                    bool cont = true;
                    if(consumptions.Count == 0){
                        Console.WriteLine("There are no foods available at the selected time.\n\nPress any button to return.");
                        Console.ReadKey(true);
                        cont = false;
                    }
                    if(cont){
                        Consumption c = MenuHelper.SelectFromList("Select the food", consumptions);
                        timeline.Add(
                            c,
                            new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                            new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0)
                        );
                        totalPrice += c.Price;
                    }
                }},
                {"Add Breaks", ()=>{
                    int breakTime = MenuHelper.SelectInteger("Select the length for your break in minutes", 1, 1, int.MaxValue);
                    DateOnly d = MenuHelper.SelectDate("Select break date and time", startDate, startDate, endDate);
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
                {"Save", ()=>{
                    save = true;
                    addingToTimeline = false;
                }},
                {"Return to menu", ()=>{
                    save = false;
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
    /// uses menu helper to gives a list of all reservations to pick one to return
    /// </summary>
    /// <returns>a object of Reservation</returns>
    public static Reservation? ShowReservation(int loggedUserId)
    {
        List<Reservation> reservations = ReservationAccess.ReadReservationsUserId(loggedUserId);
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
                {   //This while loop gets reservations and gives all the details in the prefix towards the timeline. 
                    // while (true)
                    // {
                        Console.Clear();
                        MenuHelper.PrintTimeLine($"Reservation Details:\nRoom ID: {selectedReservation.RoomId}\nGroup Size: {selectedReservation.GroupSize}\nStart Date: {selectedReservation.StartDate}\nEnd Date: {selectedReservation.EndDate}\nPrice: {selectedReservation.Price}", $"\nPress escape to return to the main menu", selectedReservation.TimeLine.t);  
                        // ConsoleKeyInfo key = Console.ReadKey(true);
                        // if (key.Key == ConsoleKey.Escape)
                        //     break;
                    // }
                }
            } while (selectedReservation != null);

            return null;
        }
    }

    /// <summary>
    /// uses menu helper to gives a list of all reservations to pick one to return
    /// </summary>
    /// <returns>a object of Reservation</returns>
    public static Reservation? GetAllReservation()
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
                reservationOptions.Add($"Reservation Number: {reservation.Id}, Room: {reservation.RoomId}, Start: {reservation.StartDate}, End: {reservation.EndDate}", reservation);
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
            Console.WriteLine("\n No reservation selected.");
            return null;
        }
    }
    /// <summary>
    /// uses menu helper to gives a list of all reservations during the given date to pick one to return
    /// </summary>
    /// <param name="date"></param>
    /// <returns>object of Reservations</returns>
    public static Reservation? GetSpecificReservation(DateTime date)
    {
    List<Reservation> reservations = ReservationAccess.ReadReservationsDate(date);
    if (reservations.Count == 0)
    {
        Console.WriteLine("\nThere are no reservations during this time period.");
        return null;
    }
    else
    {
        Dictionary<string, Reservation> reservationOptions = new Dictionary<string, Reservation>();
            foreach (Reservation reservation in reservations)
            {
                reservationOptions.Add($"Reservation Number: {reservation.Id}, Room: {reservation.RoomId}, Start: {reservation.StartDate}, End: {reservation.EndDate}", reservation);
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
            Console.WriteLine("\n No reservation selected.");
            return null;
        }
    }

    /// <summary>
    /// uses menu helper to gives a list of all reservations during the cuurent week to pick one to return
    /// </summary>
    /// <param name="date"></param>
    /// <returns>object of Reservations</returns>
    public static Reservation? GetSpecificReservationWeek(DateTime date)
    {
    List<Reservation> reservations = ReservationAccess.ReadReservationsWeek(date);
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
                reservationOptions.Add($"Reservation Number: {reservation.Id}, Room: {reservation.RoomId}, Start: {reservation.StartDate}, End: {reservation.EndDate}", reservation);
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
            Console.WriteLine("\n No reservation selected.");
            return null;
        }
    }

    /// <summary>
    /// uses menu helper to gives a list of all the useres reservations to pick one to return
    /// </summary>
    /// <returns>a object of Reservations</returns>
    public static Reservation? GetSpecificReservationUser()
    {
        List<Reservation> reservations = ReservationAccess.ReadReservationsUserId(Program.CurrentUser.Id);
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
            Console.WriteLine("\n No reservation selected.");
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