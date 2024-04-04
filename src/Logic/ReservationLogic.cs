public static class ReservationLogic
{
    public static void Reservation(){
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Reservations", new Dictionary<string, Action>(){
                {"1. Book a Reservation", BookReservation},
                {"2. View Reservations", ViewReservation},
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
            // exit out
            return;
        }
        else
        {
            // save reservation
        }
    }

    public static void ViewReservation(){

    }
}