using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class ReservationLogic
{
    private static ReservationAccess ReservationAccess = new ReservationAccess();

    /// <summary>
    /// Shows the user menu to book or view a reservation.
    /// </summary>
    public static void ReservationUser(){
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Reservations", new Dictionary<string, Action>(){
                {"1. Book a Reservation", BookReservation},
                {"2. View Reservation", ()=> ViewReservationsUser()},
                {"3. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }

    /// <summary>
    /// Shows the admin a menu to overview all the reservations.
    /// </summary>
    public static void ReservationAdmin(){
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. View all reservations",()=> {
                    ViewAllReservationsAdmin();
                }},
                {"2. View all reservations at a specific month",()=> {
                    ViewReservationMonthAdmin();
                }},
                {"3. View all reservations at a specific week",()=> {
                    ViewReservationWeekAdmin();
                }},
                {"4. View all reservations at a specific day",()=> {
                    ViewReservationDayAdmin();
                }},
                {"5. View all reservations between specific dates",()=> {
                    ViewReservationCustomAdmin();
                }},
                {"6. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        };
    }


    private static void BookReservation(){
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

    private static void ViewReservationsUser()
    {
        Reservation? r = ReservationMenu.ShowReservation(Program.CurrentUser.Id);
        if (r == null)
        {
            return;
        }
    }

    private static void ViewAllReservationsAdmin(){
        List<Reservation> allReservations = ReservationAccess.ReadReservations();
        Reservation? reservationresult;
        while(true){
            reservationresult = MenuHelper.SelectFromTable<Reservation>(allReservations,
                new Dictionary<string, Func<Reservation, object>>
                {
                    {"Room Number", item => item.RoomId},
                    {"Group Size", item => item.GroupSize},
                    {"Start Date", item => item.StartDate},
                    {"End Date", item => item.EndDate},
                    {"Price", item => item.Price}
                },
                true
            );
            if(reservationresult == null){
                return;
            }else{
                ReservationMenu.ShowReservationDetails(reservationresult);
            }
        }
    }

    private static void ViewReservationMonthAdmin(){
        // DateOnly startDate = MenuHelper.SelectDate("Select at what date you want to start your reservation:", null, DateOnly.FromDateTime(DateTime.Now), null);
        // List<Reservation> weekOfReservations = ReservationAccess.ReadReservationsMonth(month);
        // Reservation? reservationresult;
        // while(true){
        //     reservationresult = MenuHelper.SelectFromTable<Reservation>(weekOfReservations,
        //         new Dictionary<string, Func<Reservation, object>>
        //         {
        //             {"Room Number", item => item.RoomId},
        //             {"Group Size", item => item.GroupSize},
        //             {"Start Date", item => item.StartDate},
        //             {"End Date", item => item.EndDate},
        //             {"Price", item => item.Price}
        //         },
        //         true
        //     );
        //     if(reservationresult == null){
        //         return;
        //     }else{
        //         ReservationMenu.ShowReservationDetails(reservationresult);
        //     }
        // }
    }

    private static void ViewReservationWeekAdmin(){

    }

    private static void ViewReservationDayAdmin(){
        DateOnly? startDate = MenuHelper.SelectDate("Select at what date you want to search:", true);
        if(startDate == null){
            return;
        }
        List<Reservation> reservationsWithinDates = ReservationAccess.GetAllReservationsBetweenDates(startDate ?? DateOnly.MinValue);
        if(reservationsWithinDates.Count == 0){
            ReservationMenu.NoReservations();
            return;
        }
        Reservation? reservationresult;
        while(true){
            reservationresult = MenuHelper.SelectFromTable<Reservation>(reservationsWithinDates,
                new Dictionary<string, Func<Reservation, object>>
                {
                    {"Room Number", item => item.RoomId},
                    {"Group Size", item => item.GroupSize},
                    {"Start Date", item => item.StartDate},
                    {"End Date", item => item.EndDate},
                    {"Price", item => item.Price}
                },
                true
            );
            if(reservationresult == null){
                return;
            }else{
                ReservationMenu.ShowReservationDetails(reservationresult);
            }
        }
    }

    private static void ViewReservationCustomAdmin(){
        DateOnly? startDate = MenuHelper.SelectDate("Select at what date you want to start your search:", true);
        if(startDate == null){
            return;
        }
        DateOnly? endDate = MenuHelper.SelectDate("Select at what date you want to end your search", "", true, startDate, startDate, null);
        if(endDate == null){
            return;
        }
        List<Reservation> reservationsWithinDates = ReservationAccess.GetAllReservationsBetweenDates(startDate ?? DateOnly.MinValue, endDate ?? DateOnly.MaxValue);
        if(reservationsWithinDates.Count == 0){
            ReservationMenu.NoReservations();
            return;
        }
        Reservation? reservationresult;
        while(true){
            reservationresult = MenuHelper.SelectFromTable<Reservation>(reservationsWithinDates,
                new Dictionary<string, Func<Reservation, object>>
                {
                    {"Room Number", item => item.RoomId},
                    {"Group Size", item => item.GroupSize},
                    {"Start Date", item => item.StartDate},
                    {"End Date", item => item.EndDate},
                    {"Price", item => item.Price}
                },
                true
            );
            if(reservationresult == null){
                return;
            }else{
                ReservationMenu.ShowReservationDetails(reservationresult);
            }
        }
    }
}