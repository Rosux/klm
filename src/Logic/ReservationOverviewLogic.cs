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
                    ReservationMenu.ShowSpecificReservation(DateTime.Now.AddDays(7));
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
