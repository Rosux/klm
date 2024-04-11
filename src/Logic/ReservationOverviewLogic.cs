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
                }},
                {"2. Choose date",()=> {
                    DateOnly date = MenuHelper.SelectDate("Select at what date you want to start your reservation:", null, DateOnly.FromDateTime(DateTime.Now), null);
                    DateTime date_2 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    ReservationMenu.ShowSpecificReservation(date_2);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.Write(DateTime.Now);
                    Console.ReadKey();
                }},
                {"3. Exit to main menu", ()=>{
                    running = false;
                }},

            });
        }
    }
}
