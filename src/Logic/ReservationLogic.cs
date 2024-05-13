using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class ReservationLogic
{
    private static ReservationAccess ReservationAccess = new ReservationAccess();
    public static void Reservation(){
        bool running = true;
        ReservationOverviewLogic accesser = new ReservationOverviewLogic();
        while(running)
        {
            MenuHelper.SelectOptions("Reservations", new Dictionary<string, Action>(){
                {"1. Book a Reservation", BookReservation},
                {"2. View Reservation", ()=> ViewReservations()},
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
    private static void ViewReservations()
    {
        // get specific reservation
        Dictionary<string, Reservation> reservationDict = new Dictionary<string, Reservation>();
        int id = 1;
        foreach(Reservation r in ReservationAccess.ReadReservationsUserId(Program.CurrentUser.Id)){
            reservationDict.Add($"Reservation: {id} ({r.StartDate})", r);
        }
        Reservation? reservation = MenuHelper.SelectFromList("Select a reservation to view", reservationDict);
        if(reservation == null){
            return;
        }
        MenuHelper.PrintTimeLine(reservation.TimeLine.t);
    }
}