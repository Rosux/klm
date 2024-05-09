public static class ReservationLogic
{
    private static ReservationAccess ReservationAccess = new ReservationAccess();
    public static void Reservation(int loggedInUserId){
        bool running = true;
        ReservationOverviewLogic accesser = new ReservationOverviewLogic();
        while(running)
        {
            MenuHelper.SelectOptions("Reservations", new Dictionary<string, Action>(){
                {"1. Book a Reservation", BookReservation},
                {"2. View Reservation", (loggedInUserId)=>{
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
                {"3. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }

    public static void BookReservation(){
        Reservation? r = ReservationMenu.BookReservation();
        if (r == null)
        {
            return;
        }
        else
        {
            // save reservation
            bool success = ReservationAccess.CreateReservation(r);
            if(success){
                ReservationMenu.Saved();
            }else{
                ReservationMenu.Error();
            }
        }
    }

    public static void ViewReservation(int loggedInUserId){
        Reservation? r = ReservationMenu.ShowReservation(loggedInUserId);
        if (r == null)
        {
            return;
        }
    }
}