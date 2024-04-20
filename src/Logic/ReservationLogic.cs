public static class ReservationLogic
{
    private static ReservationAccess ReservationAccess = new ReservationAccess();
    public static void Reservation(int loggedInUserId){
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Reservations", new Dictionary<string, Action>()
            {
                {"1. Book a Reservation", () => BookReservation()},
                {"2. View Reservations", () => ViewReservation(loggedInUserId)},
                {"3. Exit to main menu", () => { running = false; }},
            });
        }
    }

    public static void BookReservation(){
        Reservation? r = ReservationMenu.BookReservation();
        if (r == null)
        {
            // exit out
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