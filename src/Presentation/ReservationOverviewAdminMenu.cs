public static class ReservationOverviewAdminMenu
{
    public static void ReservationAdminOverview()
    {
        ReservationOverviewLogic accesser = new ReservationOverviewLogic();
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. View all reservations",()=> {
                    // lets admin pick from a list of all reservations to see that reservations info.
                    Reservation selected_res = ReservationMenu.GetAllReservation();
                    if (selected_res != null)
                    {
                        Console.WriteLine(accesser.Overview(selected_res));
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    Console.Clear();
                }},
                {"2. View all reservations for this week",()=> {
                    // lets admin pick from a list of all reservations during current week to see that reservations info.
                    DateTime date = DateTime.Now;
                    DayOfWeek day = DateTime.Now.DayOfWeek;
                    TimeSpan date_s = new TimeSpan(0, 0, 0, 0, 0, 0);
                    DateTime date_cor = new DateTime();
                    string day_str = day.ToString();
                    switch(day_str)
                    {
                        case "Monday":
                            date_cor = date;
                            break;
                        case "Tuesday":
                            date_s = new TimeSpan(0, 0, 1, 0, 0, 0);
                            date_cor = date.Subtract(date_s);
                            break;
                        case "Wednesday":
                            date_s = new TimeSpan(0, 0, 2, 0, 0, 0);
                            date_cor = date.Subtract(date_s);
                            break;
                        case "Thursday":
                            date_s = new TimeSpan(0, 0, 3, 0, 0, 0);
                            date_cor = date.Subtract(date_s);
                            break;
                        case "Friday":
                            date_s = new TimeSpan(0, 0, 4, 0, 0, 0);
                            date_cor = date.Subtract(date_s);
                            break;
                        case "Saturday":
                            date_s = new TimeSpan(0, 0, 5, 0, 0, 0);
                            date_cor = date.Subtract(date_s);
                            break;
                        case "Sunday":
                            date_s = new TimeSpan(0, 0, 6, 0, 0, 0);
                            date_cor = date.Subtract(date_s);
                            break;
                    }
                    Reservation selected_res = ReservationMenu.GetSpecificReservationWeek(date_cor);
                    if (selected_res != null)
                    {
                        Console.WriteLine(accesser.Overview(selected_res));
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    Console.Clear();
                }},
                {"3. Choose date",()=> {
                    // lets admin pick from a list of all reservations during the date that the admin can choose and shows all that reservations info.
                    DateOnly date = MenuHelper.SelectDate("Select at what date you want to start your reservation:", null, DateOnly.FromDateTime(DateTime.Now), null);
                    DateTime date_2 = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    Reservation selected_res = ReservationMenu.GetSpecificReservation(date_2);
                    if (selected_res != null)
                    {
                        Console.WriteLine(accesser.Overview(selected_res));
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    Console.Clear();
                }},
                {"4. Exit to main menu", ()=>{
                    running = false;
                }},

            });
        }
    }
}
