public class ReservationOverviewUserMenu
{
    public static void ReservationUserOverview()
    {
        ReservationOverviewLogic accesser = new ReservationOverviewLogic();
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. View reservation", ()=>{
                    // lets the user choose a reservation from a list of all his reservations to seethe info
                    Reservation selected_res = ReservationMenu.GetSpecificReservationUser();
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
                {"2. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }
}
