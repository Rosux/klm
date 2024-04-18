using System;

class Menu
{
    public static void Start()
    {
        Console.Title = "24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;
        Console.WriteLine("Welcome to 24-7 binge watch cinema!");
        // asks the user to choose either of these options
        while(Program.CurrentUser == null){
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"Register", ()=>{
                    // run Register method
                    UserLogic.Register();
                }},
                {"Login", ()=>{
                    // run Login method
                    UserLogic.Login();
                }},
                {"Exit", ()=>{
                    // close application
                    Environment.Exit(1);
                }},
            });
        }
        if(Program.CurrentUser.Role == UserRole.ADMIN){
            while(true)
            {
                MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                    {"Movies", ()=>{
                        // movie editor
                        FilmSerieMenu.UI();
                    }},
                    {"Consumptions", ()=>{
                        // consumption editor
                        ConsumptionLogic.Consumption();
                    }},
                    {"Rooms", ()=>{
                        // room editor
                        RoomMenu.AdminOverView();
                    }},
                    {"View reservations", ()=>{
                        // close application
                        ReservationOverviewMenu.ReservationOverview();
                    }},
                    {"Exit", ()=>{
                        // close application
                        Environment.Exit(1);
                    }},
                });
            }
        }else if(Program.CurrentUser.Role == UserRole.USER){
            while(true)
            {
                MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                    {"Exit", ()=>{
                        // close application
                        Environment.Exit(1);
                    }},
                    {"Make reservation", ()=>{
                        // close application
                        ReservationLogic.BookReservation();
                    }},
                    {"Show all your Reservations", ()=>{
                        // close application
                        ReservationMenu.ShowSpecificReservationUser();
                        Console.Write($"\n\nPress any key to continue...");
                        Console.ReadKey();
                    }},
                });
            }
        }

        // DECLAN zet hieronder je logic call
        // hier
        // RoomMenu.AdminOverView();
        // FilmSerieMenu.UI();
    }

    // used for testing purpose
    public static void TestStart(){
        Program.CurrentUser = new User(69, "Ad", "Min", "hihihi", "uwu-onichan-senpai", UserRole.ADMIN);
        Console.Title = "TEST 24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;
        while(true)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"Test", ()=>{
                    ReservationLogic.Reservation();
                }},
                {"Exit", ()=>{
                    // close application
                    Environment.Exit(1);
                }},
            });
        }
    }
}
