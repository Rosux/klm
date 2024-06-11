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
                MenuHelper.OptionsUtility.SelectOptions("Choose an option", new Dictionary<string, Action>(){
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
                    MenuHelper.OptionsUtility.SelectOptions("Choose an option", new Dictionary<string, Action>(){
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
                    MenuHelper.OptionsUtility.SelectOptions("Choose an option", new Dictionary<string, Action>(){
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

        MediaLogic.Media();
        // bool x = MediaAccess.DeleteMedia(
        //     new Serie(1, "TITLE SERIE", 0, "", 0f, "", new List<Genre>(), DateOnly.MinValue, Certification.NONE, new List<string>(), false, new List<Season>())
        // );
        // Console.WriteLine(x);

        // MediaLogic.Media();
        // List<Media> x = new List<Media>(){
        //     new Film("FILM SERIE", 0, "", 0f, "", new List<Genre>(), DateOnly.MinValue, Certification.NONE, new List<string>(), new List<string>(), new List<string>()),
        //     new Serie("TITLE SERIE", 0, "", 0f, "", new List<Genre>(), DateOnly.MinValue, Certification.NONE, new List<string>(), false, new List<Season>()),
        // };
        // // Console.WriteLine(x[0].Title);
        // // Console.WriteLine(x[1].Title);

        // MenuHelper.Table<Media>(
        //     x,
        //     new Dictionary<string, Func<Media, object>>(){
        //         {"Title but like long af cus it breaks the ui", m=>m.Title},
        //         {"Genre", m=>m.Genres.Count},
        //     },
        //     false,
        //     true,
        //     true,
        //     new Dictionary<string, PropertyEditMapping<Media>>(){
        //         {"Title", new(x=>x.Title, (Media m)=>{return "NEW TITLE:!!!!!!!!!!";})},
        //         {"Genres", new(x=>ShowListInTable(x.Genres), x=>x.Genres, (Media m)=>{return new List<Genre>(){Genre.HORROR,Genre.ACTION,Genre.COMEDY,Genre.DRAMA};})},
        //     },
        //     SaveEditedMedia,
        //     false,
        //     null,
        //     false,
        //     null
        // );

        // MediaLogic.Media();

        // List<Media> m = MediaAccess.GetAllMedia();
        // MenuHelper.Table();

    }
}
