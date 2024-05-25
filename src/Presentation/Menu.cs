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
                            RoomMenu.AdminOverView();
                        }},
                        {"Reservations", ()=>{
                            // takes admin to his reservation menu
                            ReservationOverviewAdminMenu.ReservationAdminOverview();
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
                            ReservationLogic.Reservation();
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


        bool editUsers = MenuHelper.SelectFromList("Select an option", new Dictionary<string, bool>(){
            {"Edit Users", true},
            {"Edit Films", false},
        });

        if(editUsers){
            UserAccess u = new UserAccess();
            List<User> baka = u.GetAllUsers();
            // string is the header text
            // User is the type of the model you want in the table
            // object is what the Func must return
            Dictionary<string, Func<User, object>> headers = new Dictionary<string, Func<User, object>>(){
                //     ^required    ^     ^
                //                  ^     ^the type you want the table to use for the User instance (like User.FirstName)
                //                  ^specifies the type u want to use in the table (in this case its users so we use User)
                {"UserName", u => u.FirstName},
                {"Role van de user?", u => u.Role},
                {"e-maillll", u => u.Email},
                {"e-pass", u => u.Password},
                {"e-idk nog een ding", u => u.LastName},
            };
            Dictionary<string, PropertyEditMapping<User>> editStuff = new Dictionary<string, PropertyEditMapping<User>>(){
                {"Firstname", new(u=>u.FirstName, GetNewFirstName)},
                {"Lastname", new(u=>u.LastName, GetNewLastName)},
                {"Role", new(u=>u.Role, GetNewRole)},
                {"Id", new(u=>u.Id, GetNewId)},
            };
            User? y = MenuHelper.Table(baka, headers, false, true, true, editStuff, SaveEditedUser, true, ()=>{return new User("fname", "lname", "email", "pass");}, true, (User u)=>{return true;});
            if(y == null){
                Console.WriteLine("NO USER SELECTED!");
            }else{
                Console.WriteLine($"USER: {y.FirstName}, {y.Id}, {y.Email}, {y.Role}");
            }
            // the SaveEditedUser must return a boolean indicating if the data is saved or not
        }else{
            FilmAcesser f = new FilmAcesser(); // film model
            List<Film> fs = f.Get_info(); // put all films in a list
            Dictionary<string, Func<Film, object>> fheaders = new Dictionary<string, Func<Film, object>> // create a dictionary with the headername and property of the item
            {
                { "Film Name", f => f.Title}, // print "Film Name" bovenaan en gebruikt Film.Title om dat in de lijst te zetten
                { "hoe lang duurt deze film?", f => f.Runtime } // print "hoe lang duurt deze film?" bovenaan en gebruikt Film.Runtime om dat in de lijst te zetten
            };
            Dictionary<string, PropertyEditMapping<Film>> editSettings = new Dictionary<string, PropertyEditMapping<Film>>(){
                {"Title", new(f=>f.Title, (Film u)=>{
                    return MenuHelper.SelectText("Select the new film title:", "(\\S| )") ?? "";
                })},
                {"Length", new(f=>f.Runtime, (Film u)=>{
                    return MenuHelper.SelectInteger("Select the new film length:", "", false, 0, 0, 500) ?? 0;
                })},
            };
            MenuHelper.Table(fs, fheaders, false, true, false, editSettings, (Film)=>{
                // lets just say the film changes are saved
                return true;
            }, true, ()=>{return fs[0];}, true, (Film f)=>{return true;});
        }

    }

    public static bool SaveEditedUser(User editedUserObject){
        Console.Clear();
        Console.WriteLine(editedUserObject.Id);
        Console.WriteLine(editedUserObject.FirstName);
        Console.WriteLine(editedUserObject.LastName);
        Console.WriteLine(editedUserObject.Email);
        Console.WriteLine(editedUserObject.Password);
        Console.WriteLine(editedUserObject.Role);
        Console.ReadKey(true);
        return true;
    }

    public static object GetNewLastName(User userObject){
        Console.Write($"old data: {userObject.LastName}\nFill in the new Lastname: ");
        return Console.ReadLine() ?? "Default User LastName";
    }

    public static object GetNewFirstName(User userObject){
        Console.Write($"old data: {userObject.FirstName}\nFill in the new Firstname: ");
        return Console.ReadLine() ?? "Default User FirstName";
    }

    public static object GetNewRole(User userObject){
        return UserRole.USER;
    }

    public static object GetNewId(User userObject){
        return -1;
    }
}
