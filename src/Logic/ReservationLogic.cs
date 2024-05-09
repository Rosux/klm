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
                {"2. View Reservation", ()=> Val()},
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
    private static void Val()
    {
        // get specific reservation
        Reservation reservation = ReservationAccess.GetReservationById(1);
        MenuHelper.PrintTimeLine(reservation.TimeLine.t);

        // Console.WriteLine(((Film)(reservation.TimeLine.t[0].Action)).Title);

        Console.ReadKey(true);
        // MenuHelper.PrintTimeLine(timeLine);
    }
}