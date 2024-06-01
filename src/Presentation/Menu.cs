using System;
using System.Reflection;
using TimeLine;
class Menu
{
    public static void Start()
    {
        Console.Title = "24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;
        Console.WriteLine("Welcome to 24-7 binge watch cinema!");
        // asks the user to choose either of these options
        while(true){
            if(Program.CurrentUser == null)
            {
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
            else if(Program.CurrentUser.Role == UserRole.ADMIN)
            {
                bool uwu = true;
                while(uwu)
                {
                    MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                        {"Manage movies/series", ()=>{
                            // takes admin to movie editor
                            FilmSerieMenu.UI();
                        }},
                        {"Consumptions", ()=>{
                            // consumption editor
                            ConsumptionLogic.Consumption();
                        }},
                        {"Rooms", ()=>{
                            // room editor
                            RoomLogic.Menus();
                        }},
                        {"Reservations", ()=>{
                            // takes admin to his reservation menu
                            ReservationLogic.ReservationAdmin();
                        }},
                        {"Users", ()=>{
                            // starts the userlogic main loop
                            UserLogic.User();
                        }},
                        {"Logout", ()=>{
                            // goto login screen
                            uwu = false;
                            Program.CurrentUser = null;
                        }},
                    });
                }
            }
            else if(Program.CurrentUser.Role == UserRole.USER)
            {
                bool uwu = true;
                while(uwu)
                {
                    MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                        {"Reservations", ()=>{
                            ReservationLogic.ReservationUser();
                        }},
                        {"Logout", ()=>{
                            // goto login screen
                            uwu = false;
                            Program.CurrentUser = null;
                        }},
                    });
                }
            }
        }
    }

    // used for testing purpose
    public static void TestStart(){
        Program.CurrentUser = new User(8, "Ad", "Min", "hihihi", "uwu-onichan-senpai", UserRole.ADMIN);
        Console.Title = "TEST 24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;
        if(Program.CurrentUser.Role == UserRole.ADMIN)
            {
                bool uwu = true;
                while(uwu)
                {
                    MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                        {"Manage movies/series", ()=>{
                            // takes admin to movie editor
                            FilmSerieMenu.UI();
                        }},
                        {"Consumptions", ()=>{
                            // consumption editor
                            ConsumptionLogic.Consumption();
                        }},
                        {"Rooms", ()=>{
                            // room editor
                            RoomLogic.Menus();
                        }},
                        {"Reservations", ()=>{
                            // takes admin to his reservation menu
                            ReservationLogic.ReservationAdmin();
                        }},
                        {"Users", ()=>{
                            // starts the userlogic main loop
                            UserLogic.User();
                        }},
                        {"Logout", ()=>{
                            // goto login screen
                            uwu = false;
                            Program.CurrentUser = null;
                        }},
                    });
                }
            }
            else if(Program.CurrentUser.Role == UserRole.USER)
            {
                bool uwu = true;
                while(uwu)
                {
                    MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                        {"Reservations", ()=>{
                            ReservationLogic.ReservationUser();
                        }},
                        {"Logout", ()=>{
                            // goto login screen
                            uwu = false;
                            Program.CurrentUser = null;
                        }},
                    });
                }
            }
    }
}
