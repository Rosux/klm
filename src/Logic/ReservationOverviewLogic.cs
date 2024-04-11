public static class ReservationOverviewLogic
{
    public static void ReservationOverview()
    {
                bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. View all reservations",()=> {
                    ReservationLogic.ViewReservation();
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"2. View all reservations for this week",()=> {
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
                    ReservationMenu.ShowSpecificReservation(date_cor);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"3. Choose date",()=> {
                    DateOnly date = MenuHelper.SelectDate("Select at what date you want to start your reservation:", null, DateOnly.FromDateTime(DateTime.Now), null);
                    DateTime date_2 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    ReservationMenu.ShowSpecificReservation(date_2);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"4. Exit to main menu", ()=>{
                    running = false;
                }},

            });
        }
    }
}
