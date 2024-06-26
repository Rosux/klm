using System.Data.Entity.Core.Objects;

public static class ReservationMenu
{
    private static RoomAccess RoomsAccess = new RoomAccess();
    private static ConsumptionAccess Consumptions = new ConsumptionAccess();
    private static ReservationAccess ReservationAccess = new ReservationAccess();
    private static UserAccess UserAccess = new UserAccess();


    #region Book Reservation
    /// <summary>
    /// Asks the user to make a reservation.
    /// </summary>
    /// <returns>A reservation object holding the users planned reservation or NULL in case the user cancels the creation.</returns>
    public static Reservation? BookReservation(){
        double totalPrice = 0.0;
        List<Room> AllRooms = RoomsAccess.GetAllRooms();
        if (AllRooms.Count == 0)
        {
            Console.WriteLine("There are currently no rooms available. Press enter to go back...");
            Console.ReadKey(true);
            return null;
        }
        int roomMaxSize = RoomsAccess.GetMaxRoomCapacity();
        int GroupSize = MenuHelper.IntegerUtility.SelectInteger("Select your group size:", "", 1, 1, roomMaxSize);
        totalPrice += (double)GroupSize * 12.0;
        // create dict of room and room names
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        int RoomCounter = 1;
        foreach(Room room in RoomsAccess.GetAllRooms(GroupSize)){
            rooms.Add($"{room.RoomName} (Maximum size: {room.Capacity})", room);
            RoomCounter++;
        }
        DateOnly startDate = MenuHelper.DateUtility.SelectDate("Select at what date you want to start your reservation:", null, DateOnly.FromDateTime(DateTime.Now), null);
        TimeOnly startTime = MenuHelper.TimeUtility.SelectTime("Select at what time you want to start your reservation:", "", new TimeOnly(), null, null);
        DateOnly endDate = MenuHelper.DateUtility.SelectDate("Select at what date you want to end your reservation:", startDate, startDate, null);
        TimeOnly endTime = new TimeOnly();
        if (startDate == endDate){
            endTime = MenuHelper.TimeUtility.SelectTime("Select at what time you want to end your reservation:", "", startTime.AddMinutes(1), startTime.AddMinutes(1), null);
        }else{
            endTime = MenuHelper.TimeUtility.SelectTime("Select at what time you want to end your reservation:");
        }

        // Create a list to hold the keys of rooms that are not available
        List<string> roomsToRemove = new List<string>();
        DateTime StartDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, 0);
        DateTime EndDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, 0);
        // Check availability and add the keys of unavailable rooms to the list
        foreach(var kvp in rooms)
        {
            if (!ReservationAccess.RoomAvailable(kvp.Value.Id, StartDateTime, EndDateTime))
            {
                roomsToRemove.Add(kvp.Key);
            }
        }
        //remove non-available rooms from rooms list and ask user to select a room.
        foreach(string key in roomsToRemove)
        {
            rooms.Remove(key);
        }

        Room SelectedRoom = null;
        if(rooms.Count > 0) {
            SelectedRoom  = MenuHelper.ListUtility.SelectFromList("Choose the room you want to reserve:", rooms);
        }
        else {
            Console.WriteLine("There are no rooms available for the selected date\nReservation has not been added\n\nReturning to main menu, press enter to continue...");
            Console.ReadKey(true);
            return null;
        }

        TimeLine.Holder timeline = new TimeLine.Holder();
        List<Entertainment> entertainments = new List<Entertainment>();

        bool addingToTimeline = true;
        bool save = false;
        while(addingToTimeline){
            MenuHelper.OptionsUtility.SelectOptions($"Select an option", new Dictionary<string, Action>(){
                {"Add Movies/Episode", ()=>{
                    object? FilmOrEpisode = MenuHelper.MediaUtility.SelectMovieOrEpisode();
                    if(FilmOrEpisode != null && FilmOrEpisode is Film){
                        Film film = (Film)FilmOrEpisode;
                        DateOnly d = MenuHelper.DateUtility.SelectDate($"Select the date and time you want {film.Title} to begin.", startDate, startDate, endDate);
                        TimeOnly t;
                        if (d == startDate && d == endDate){
                            t = MenuHelper.TimeUtility.SelectTime($"Select the date and time you want {film.Title} to begin.", "", startTime, startTime, endTime.AddMinutes(-film.Runtime)); //Endtime - runtime ensures that the movie does not last longer than the reservation
                        }else if (d == startDate){
                            t = MenuHelper.TimeUtility.SelectTime($"Select the date and time you want {film.Title} to begin.", "", startTime, startTime, TimeOnly.MaxValue);
                        }else if (d == endDate){
                            t = MenuHelper.TimeUtility.SelectTime($"Select the date and time you want {film.Title} to begin.", "", new TimeOnly(), TimeOnly.MinValue, endTime.AddMinutes(-film.Runtime)); //Endtime - runtime ensures that the movie does not last longer than the reservation 
                        }else{
                            t = MenuHelper.TimeUtility.SelectTime($"Select the date and time you want {film.Title} to begin.", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
                        }
                        DateTime startDateTime = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0);
                        DateTime endDateTime = startDateTime.AddMinutes(film.Runtime);
                        //HasConflict returns tuple with bool Conflict (true = conflict) and string ConflictingFilm (title)
                        var ConflictReturns = timeline.HasConflict(startDateTime, endDateTime);
                        if (!ConflictReturns.Conflict) {
                            timeline.Add(
                                film,
                                startDateTime,
                                endDateTime
                            );
                        } else {
                            Console.WriteLine($"Movie has not been added: {ConflictReturns.ConflictingString}.\n\nPress enter to continue...");
                            Console.ReadKey(true);
                        }
                    }else if(FilmOrEpisode != null && FilmOrEpisode is Dictionary<Serie, List<Episode>>){
                        List<Episode> episode_list = ((Dictionary<Serie, List<Episode>>)FilmOrEpisode).First().Value;
                        DateTime serieTime = new DateTime();
                        int lastDay = -1;
                        for(int i = 0; i < episode_list.Count; i++)
                        {
                            Episode episode = episode_list[i];
                            DateOnly d = MenuHelper.DateUtility.SelectDate($"Select the date and time you want the episode {episode.Title} to begin.", startDate, startDate, endDate);
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
                                t = MenuHelper.TimeUtility.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", TimeOnly.FromDateTime(serieTime), startTime, endTime.AddMinutes(-episode.Runtime));
                            }else if (d == startDate){
                                t = MenuHelper.TimeUtility.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", TimeOnly.FromDateTime(serieTime), startTime, TimeOnly.MaxValue);
                            }else if (d == endDate){
                                t = MenuHelper.TimeUtility.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", TimeOnly.FromDateTime(serieTime), TimeOnly.MinValue, endTime.AddMinutes(-episode.Runtime));
                            }else{
                                t = MenuHelper.TimeUtility.SelectTime($"Select the date and time you want the episode {episode.Title} to begin.", "", TimeOnly.FromDateTime(serieTime), TimeOnly.MinValue, TimeOnly.MaxValue);
                            }
                            DateTime startDateTime = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0);
                            DateTime endDateTime = startDateTime.AddMinutes(episode.Runtime);
                            //HasConflict returns tuple with bool Conflict (true = conflict) and string ConflictingFilm (title)
                            var ConflictReturns = timeline.HasConflict(startDateTime, endDateTime);
                            if (!ConflictReturns.Conflict) {
                                timeline.Add(
                                    episode,
                                    startDateTime,
                                    endDateTime
                                );
                            } else {
                                Console.WriteLine($"Episode has not been added: {ConflictReturns.ConflictingString}.\nPlease try again!\n\nPress enter to continue...");
                                Console.ReadKey(true);
                                i--;
                            }
                        }
                    }
                }},
                {"Add Consumptions", ()=>{
                    DateOnly d = MenuHelper.DateUtility.SelectDate("Select the date and time of your food", startDate, startDate, endDate);
                    TimeOnly t;
                    if (d == startDate && d == endDate){
                        t = MenuHelper.TimeUtility.SelectTime("Select the date and time of your food", "", new TimeOnly(), startTime, endTime);
                    }else if (d == startDate){
                        t = MenuHelper.TimeUtility.SelectTime("Select the date and time of your food", "", new TimeOnly(), startTime, TimeOnly.MaxValue);
                    }else if (d == endDate){
                        t = MenuHelper.TimeUtility.SelectTime("Select the date and time of your food", "", new TimeOnly(), TimeOnly.MinValue, endTime);
                    }else{
                        t = MenuHelper.TimeUtility.SelectTime("Select the date and time of your food", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
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
                        Consumption c = MenuHelper.ListUtility.SelectFromList("Select the food", consumptions);
                        timeline.Add(
                            c,
                            new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                            new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0)
                        );
                        totalPrice += c.Price;
                    }
                }},
                {"Add Entertainment", ()=>{
                    int x = 0;
                    int y = 0;
                    ConsoleKey key;
                    do{
                        MenuHelper.SeatUtility.PrintSeats(SelectedRoom, entertainments, x, y);
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
                                DateOnly EntertainmentDate = MenuHelper.DateUtility.SelectDate("Select Entertainment date and time", startDate, startDate, endDate);
                                //Ask the user to provide the time they want a entertainment to take place
                                TimeOnly EntertainmentStartTime;
                                //Check if the time stays within the timetable of the reservation
                                if (EntertainmentDate == startDate && EntertainmentDate == endDate){
                                    EntertainmentStartTime = MenuHelper.TimeUtility.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), startTime, endTime);
                                }else if (EntertainmentDate == startDate){
                                    EntertainmentStartTime = MenuHelper.TimeUtility.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), startTime, TimeOnly.MaxValue);
                                }else if (EntertainmentDate == endDate && endTime == TimeOnly.MinValue){
                                    EntertainmentStartTime = MenuHelper.TimeUtility.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), TimeOnly.MinValue, new TimeOnly(23, 59));
                                }else if (EntertainmentDate == endDate){
                                    EntertainmentStartTime = MenuHelper.TimeUtility.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), TimeOnly.MinValue, endTime);
                                }else{
                                    EntertainmentStartTime = MenuHelper.TimeUtility.SelectTime("Select the time you want to start the Entertainment", "", new TimeOnly(), TimeOnly.MinValue, TimeOnly.MaxValue);
                                }
                                //Convert the provided date and time to a DateTime format
                                DateTime Time = new DateTime(EntertainmentDate.Year, EntertainmentDate.Month, EntertainmentDate.Day, EntertainmentStartTime.Hour, EntertainmentStartTime.Minute, 0);
                                Console.Write("Enter the entertainment description: ");
                                string? text = MenuHelper.StringUtility.SelectText("Enter the entertainment description:", "", true, 0 , 20, "([A-z]| )");
                                if (text == null){
                                    return;
                                }
                                //Check for confirmation for user
                                string prompt = $"Are you sure you want to save the following entertainment to the chair:\nDescription: {text}\nDate: {EntertainmentDate}\nTime: {EntertainmentStartTime}";
                                if(!MenuHelper.ConfirmationUtility.Confirm(prompt)){
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
                {"Show current reservation", ()=>{
                    Reservation CurrentReservation = new Reservation(
                        SelectedRoom.Id,
                        Program.CurrentUser.Id,
                        GroupSize,
                        new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, 0),
                        new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, 0),
                        (double)(GroupSize * 12.0) + totalPrice,
                        timeline,
                        entertainments
                    );
                    ShowReservationDetails(CurrentReservation);
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

    #region Show Reservation Details
    /// <summary>
    /// Displays a table with all the consumptions that are reserved when booking the reservation.
    /// </summary>
    /// <param name="t">A List of TimeLine Items which holds the consumptions for a reservation.</param>
    public static void ShowConsumptions(List<TimeLine.Item> t)
    {
        List<TimeLine.Item> consumptions = new List<TimeLine.Item>();
        foreach (TimeLine.Item item in t)
        {
            if(item.Action is Consumption)
            {
                consumptions.Add(item);
            }
        }
        MenuHelper.TableUtility.ShowInTable<TimeLine.Item>(consumptions,
            new Dictionary<string, Func<TimeLine.Item, object>>
            {
                {"Name", item => ((Consumption)item.Action).Name},
                {"Price €", item => ((Consumption)item.Action).Price},
                {"Order Time", item => item.StartTime},
            }
        );
    }

    /// <summary>
    /// Displays a table which holds all the entertainments reserved when booking a reservation.
    /// </summary>
    /// <param name="entertainments">A list of entertainments which are linked to the reservation.</param>
    public static void ShowEntertainments(List<Entertainment> entertainments){
            MenuHelper.TableUtility.ShowInTable<Entertainment>(entertainments,
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
    /// Displays all the reservation details and has the options to view TimeLine, Consumptions, Entertainments or to return.
    /// </summary>
    /// <param name="selectedReservation">A reservation object which holds all the data of the given reservation.</param>
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
                $"Price: €{selectedReservation.Price}"
            };
            //For adminOverview there is a extra username field
            if(Program.CurrentUser.Role == UserRole.ADMIN)
            {
                User? reservationUser = UserAccess.GetUser(selectedReservation.UserId);
                details = new string[]
                {
                    $"User Name: {reservationUser.FirstName} {reservationUser.LastName}",
                    $"Room Number: {selectedReservation.RoomId}",
                    $"Group Size: {selectedReservation.GroupSize}",
                    $"Start Date: {selectedReservation.StartDate}",
                    $"End Date: {selectedReservation.EndDate}",
                    $"Price: €{selectedReservation.Price}"
                };
            }
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
                    MenuHelper.TimeLineUtility.PrintTimeLine("Press Escape to return", "",  selectedReservation.TimeLine.Items);
                }
                if(currentSelection == 1){
                    ShowConsumptions(selectedReservation.TimeLine.Items);
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


    /// <summary>
    /// Notifies the user an error occurred.
    /// </summary>
    public static void Error(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("An error occurred. Please try again later.\n\nPress any key to continue");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Notifies the user the reservation got saved.
    /// </summary>
    public static void Saved(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("Your reservation has been saved succesfully.\n\nPress any key to continue");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Notifies the user that there are no reservations found.
    /// </summary>
    public static void NoReservations(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.Write("No reservations found.\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}