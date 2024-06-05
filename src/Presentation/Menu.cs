using System;
using System.Reflection;
using TimeLine;

class Menu
{
    /// <summary>
    /// Entry point for the application. Creates a login/register menu.
    /// </summary>
    public static void Start()
    {
        Console.Title = Environment.GetEnvironmentVariable("CONSOLE_TITLE") ?? "";
        Console.CursorVisible = false;
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
                        {"Manage Media", ()=>{
                            // takes admin to movie editor
                            MediaLogic.Media();
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

    /// <summary>
    /// Used for testing purposes.
    /// </summary>
    public static void TestStart(){
        Console.WriteLine("TEST START!!!");
        Program.CurrentUser = new User(8, "Ad", "Min", "hihihi", "uwu-onichan-senpai", UserRole.ADMIN);
        Console.Title = "TEST 24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;

        List<Genre>? xxx = MenuHelper.SelectFromEnum<Genre>(new List<Genre>(){ Genre.HORROR, Genre.FANTASY, Genre.ACTION, Genre.FAMILY }, "Genres", "Prefix\nGOES HERE!!!!", "suffix here maybe?", true);
        Console.WriteLine((xxx != null) ? xxx.Count : "NULL");

    }
}
