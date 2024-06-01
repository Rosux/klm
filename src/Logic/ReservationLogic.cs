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
        List<Reservation> allReservations = ReservationAccess.GetReservationsByUserId(Program.CurrentUser.Id);
        if(allReservations.Count == 0){
            ReservationMenu.NoReservations();
            return;
        }
        ShowReservations(allReservations);
    }

    private static void ViewAllReservationsAdmin(){
        List<Reservation> allReservations = ReservationAccess.GetAllReservations();
        if(allReservations.Count == 0){
            ReservationMenu.NoReservations();
            return;
        }
        ShowReservations(allReservations);
    }

    private static void ViewReservationMonthAdmin(){
        DateOnly? selectedMonth = MenuHelper.SelectDate("Select at what date you want to search:", true);
        if(selectedMonth == null){
            return;
        }
        (DateOnly startDate, DateOnly endDate) = GetMonthFromDate(selectedMonth ?? DateOnly.MinValue);
        List<Reservation> reservationsOfTheMonth = ReservationAccess.GetAllReservationsBetweenDates(startDate, endDate);
        if(reservationsOfTheMonth.Count == 0){
            ReservationMenu.NoReservations();
            return;
        }
        ShowReservations(reservationsOfTheMonth);
    }

    private static void ViewReservationWeekAdmin(){
        DateOnly? selectedWeek = MenuHelper.SelectDate("Select at what date you want to search:", true);
        if(selectedWeek == null){
            return;
        }
        (DateOnly startDate, DateOnly endDate) = GetWeekFromDate(selectedWeek ?? DateOnly.MinValue);
        List<Reservation> reservationsOfTheWeek = ReservationAccess.GetAllReservationsBetweenDates(startDate, endDate);
        if(reservationsOfTheWeek.Count == 0){
            ReservationMenu.NoReservations();
            return;
        }
        ShowReservations(reservationsOfTheWeek);
    }

    private static void ViewReservationDayAdmin(){
        DateOnly? startDate = MenuHelper.SelectDate("Select at what date you want to search:", true);
        if(startDate == null){
            return;
        }
        List<Reservation> ReservationsOfTheDay = ReservationAccess.GetAllReservationsBetweenDates(startDate ?? DateOnly.MinValue);
        if(ReservationsOfTheDay.Count == 0){
            ReservationMenu.NoReservations();
            return;
        }
        ShowReservations(ReservationsOfTheDay);
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
        ShowReservations(reservationsWithinDates);
    }

    private static void ShowReservations(List<Reservation> reservations)
    {
        Reservation? reservationresult;
        while(true)
        {
            reservationresult = MenuHelper.SelectFromTable(
                reservations,
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
            if(reservationresult == null)
            {
                return;
            }
            else
            {
                ReservationMenu.ShowReservationDetails(reservationresult);
            }
        }
    }

    /// <summary>
    /// Get the starting date and ending date of the given date week.
    /// </summary>
    /// <param name="date">A DateOnly holding the date to convert to week start/end dates.</param>
    /// <returns>A tuple containing the start and end of the week in DateOnly format.</returns>
    private static (DateOnly StartOfWeek, DateOnly EndOfWeek) GetWeekFromDate(DateOnly date)
    {
        int dayOfWeek = (int)date.DayOfWeek;
        int daysToSubtract = (dayOfWeek == 0) ? 5 : dayOfWeek - 1;
        DateOnly startOfWeek = date.AddDays(-daysToSubtract);
        DateOnly endOfWeek = startOfWeek.AddDays(6);
        return (startOfWeek, endOfWeek);
    }

    /// <summary>
    /// Get the starting date and ending date of the given date month.
    /// </summary>
    /// <param name="date">A DateOnly holding the date to convert to month start/end dates.</param>
    /// <returns>A tuple containing the start and end of the month in DateOnly format.</returns>
    private static (DateOnly StartOfMonth, DateOnly EndOfMonth) GetMonthFromDate(DateOnly date)
    {
        DateOnly startOfMonth = new DateOnly(date.Year, date.Month, 1);
        DateOnly endOfMonth = new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        return (startOfMonth, endOfMonth);
    }
}