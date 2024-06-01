public static class ReservationMenu
{
    private static RoomAccess RoomsAccess = new RoomAccess();
    private static ConsumptionAccess Consumptions = new ConsumptionAccess();
    private static ReservationAccess ReservationAccess = new ReservationAccess();

    /// <summary>
    /// The following region contains all the code needed to create a reservation.
    /// </summary>
    #region BookReservation
    /// <summary>
    /// Asks the user to plan in a reservation. Provides null if cancelled or saves it into the database.
    /// </summary>
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
        Room SelectedRoom  = MenuHelper.SelectFromList("Choose the room you want to reserve:", rooms);
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
        List<Entertainment> entertainments = new List<Entertainment>();

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
                {"Add Entertainment", ()=>{
                    int x = 0;
                    int y = 0;
                    ConsoleKey key;
                    do{
                        MenuHelper.PrintSeats(SelectedRoom, entertainments, x, y);
                        Console.WriteLine("Press escape to return and save\n");

                        // Check if the current coordinates correspond to a valid seat
                        if (SelectedRoom.Seats[y][x]) // Assuming SelectedRoom.Seats is a 2D bool array
                        {
                            if (entertainments.Any(e => e.SeatRow == y && e.SeatColumn == x)){
                                Console.WriteLine("This seat already has an or multiple entertainment:");
                            }else{
                                Console.WriteLine("This seat has no entertainments yet.");
                            }
                            foreach (var entertainment in entertainments)
                            {
                                if (entertainment.SeatRow == y && entertainment.SeatColumn == x)
                                {
                                    Console.WriteLine($"Entertainment: {entertainment.Text} at {entertainment.Time}");
                                }
                            }

                            key = Console.ReadKey(true).Key;
                            // handle selecting seat column and row (X=column and Y=row)
                            if(key == ConsoleKey.UpArrow){
                                y--;
                            }else if(key == ConsoleKey.DownArrow){
                                y++;
                            }else if(key == ConsoleKey.LeftArrow){
                                x--;
                            }else if(key == ConsoleKey.RightArrow){
                                x++;
                            }
                            y = Math.Clamp(y, 0, SelectedRoom.Seats.Length-1);
                            x = Math.Clamp(x, 0, SelectedRoom.Seats[y].Length-1);

                            // Selecting a seat will show the entertainment linked to that specific seat
                            if(key == ConsoleKey.Enter){
                                //Ask the user to provide the date they want a entertainment to take place
                                DateOnly EntertainmentDate = MenuHelper.SelectDate("Select Entertainment date and time", startDate, startDate, endDate);
                                //Ask the user to provide the time they want a entertainment to take place
                                TimeOnly EntertainmentStartTime;
                                //Check if the time stays within the timetable of the reservation
                                if (EntertainmentDate == startDate && EntertainmentDate == endDate){
                                    EntertainmentStartTime = MenuHelper.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), startTime, endTime);
                                }else if (EntertainmentDate == startDate){
                                    EntertainmentStartTime = MenuHelper.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), startTime, TimeOnly.MaxValue);
                                }else if (EntertainmentDate == endDate){
                                    EntertainmentStartTime = MenuHelper.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), TimeOnly.MinValue, endTime);
                                }else{
                                    EntertainmentStartTime = MenuHelper.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
                                }
                                //Convert the provided date and time to a DateTime format
                                DateTime Time = new DateTime(EntertainmentDate.Year, EntertainmentDate.Month, EntertainmentDate.Day, EntertainmentStartTime.Hour, EntertainmentStartTime.Minute, 0);
                                Console.Write("Enter the entertainment description: ");
                                string? text = MenuHelper.SelectText("Enter the entertainment description:", "", true, 0 , 20, "([A-z]| )");
                                if (text == null){
                                    return;
                                }
                                //Check for confirmation for user
                                string prompt = $"Are you sure you want to save the following entertainment to the chair:\nDescription: {text}\nDate: {EntertainmentDate}\nTime: {EntertainmentStartTime}";
                                if(!MenuHelper.Confirm(prompt)){
                                    return;
                                }
                                //Add entertainment to the list and provide a update message
                                entertainments.Add(new Entertainment(SelectedRoom.Id, Time, text, y, x));
                                Console.WriteLine("Entertainment added successfully.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("This is not a valid seat.");
                            key = Console.ReadKey(true).Key;
                            // handle selecting seat column and row (X=column and Y=row)
                            if(key == ConsoleKey.UpArrow){
                                y--;
                            }else if(key == ConsoleKey.DownArrow){
                                y++;
                            }else if(key == ConsoleKey.LeftArrow){
                                x--;
                            }else if(key == ConsoleKey.RightArrow){
                                x++;
                            }
                            y = Math.Clamp(y, 0, SelectedRoom.Seats.Length-1);
                            x = Math.Clamp(x, 0, SelectedRoom.Seats[y].Length-1);
                        }

                        if(key == ConsoleKey.Escape){
                            break;
                        }
                    }while(true);
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
                timeline,
                entertainments
            );
            return r;
        }else if(!save){
            return null;
        }
        return null;
    }
    #endregion

    /// <summary>
    /// The following region contains all the methods used to show reservation/reservation details.
    /// </summary>
    #region ShowReservationDetails
    /// <summary>
    /// Displays a table with all the consumptions that are reserved when booking the reservation.
    /// </summary>
    /// <param name="t">A TimeLine which holds the consumptions for a reservation</param>
    public static void ShowConsumptions(List<TimeLine.Item> t)
    {
        List<TimeLine.Item> consumptions = new List<TimeLine.Item>();
        Console.CursorVisible = false;
        Console.Clear();
        foreach (TimeLine.Item item in t)
        {
            if(item.Action is Consumption)
            {
                consumptions.Add(item);
            }
        }
        MenuHelper.ShowInTable<TimeLine.Item>(consumptions,
            new Dictionary<string, Func<TimeLine.Item, object>>
            {
                {"Name", item => ((Consumption)item.Action).Name},
                {"Price", item => ((Consumption)item.Action).Price},
                {"Order Time", item => item.StartTime},
            }
        );
    }

    /// <summary>
    /// Displays a table which holds all the entertainments reserved when booking a reservation.
    /// </summary>
    /// <param name="entertainments">A list of entertainments which are linked to the reservation</param>
    public static void ShowEntertainments(List<Entertainment> entertainments){
            MenuHelper.ShowInTable<Entertainment>(entertainments, 
            new Dictionary<string, Func<Entertainment, object>>
            {
                {"Starting Date/Time", item => item.Time},
                {"Description", item => item.Text},
                {"Seat Row", item => item.SeatRow + 1},
                {"Seat Number", item => item.SeatColumn + 1}
            }
        );
    }

    /// <summary>
    /// Displays all the reservation details and has the options to view TimeLine, Consumptions, Entertainments or return to the menu.
    /// </summary>
    /// <param name="selectedReservation">A reservation type which holds all the data of the given reservation</param>
    /// Provides a list of consumptions for the ShowConsumptions method.
    /// Provides a list of entertainments for the ShowEntertainments method.
    public static void ShowReservationDetails(Reservation selectedReservation){
        List<string> Options = new List<string>(
            new string[] { "Timeline", "Consumptions", "Entertainments" }
        );
        string header = "Reservation Details";
        int currentSelection = 0;
        ConsoleKey key;
        do
        {
            Console.CursorVisible = false;
            Console.Clear();
            string[] details = new string[]
            {
                $"Room Number: {selectedReservation.RoomId}",
                $"Group Size: {selectedReservation.GroupSize}",
                $"Start Date: {selectedReservation.StartDate}",
                $"End Date: {selectedReservation.EndDate}",
                $"Price: {selectedReservation.Price}"
            };

            Console.BackgroundColor = ConsoleColor.Black;
            int longestLineLength = Math.Max(header.Length, details.Max(detail => detail.Length));
            Console.Write($"┌─{header}{new string('─', Math.Max(0 ,longestLineLength - header.Length))}─┐\n");
            foreach (string detail in details)
            {
                Console.Write($"│ {detail}{new string(' ', Math.Max(0 ,longestLineLength - detail.Length))} │\n");
            }

            Console.Write($"├─{new string('─', Math.Max(0 ,longestLineLength))}─┤\n");
            for(int i = 0; i < Options.Count; i ++)
            {
                Console.Write("│ ");
                if (currentSelection == i) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                Console.Write($"{Options[i]}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{new String(' ', Math.Max(0, longestLineLength - Options[i].Length))} │\n");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"└─{new String('─', Math.Max(0, longestLineLength))}─┘\n");
            Console.WriteLine("Press Escape to return");
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Enter){
                if(currentSelection == 0)
                {
                    MenuHelper.PrintTimeLine("Press Escape to return", "",  selectedReservation.TimeLine.t);
                }
                if(currentSelection == 1){
                    ShowConsumptions(selectedReservation.TimeLine.t);
                }
                if(currentSelection == 2){
                    ShowEntertainments(selectedReservation.Entertainments);
                }
            }
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow){
                currentSelection += (key == ConsoleKey.DownArrow) ? 1 : -1;
            }
            currentSelection = Math.Clamp(currentSelection, 0, Options.Count-1);
        } while (key != ConsoleKey.Escape);
    }
    #endregion

    public static void Error(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("An error occured. Please try again later.\n\nPress any key to continue");
        Console.ReadKey(true);
    }

    public static void Saved(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("Your reservation has been saved succesfully.\n\nPress any key to continue");
        Console.ReadKey(true);
    }

    public static void NoReservations(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.Write("No reservations found.\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}