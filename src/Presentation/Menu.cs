using System;

// CONTROLLER (handle logic here such as updating user data) (make for each controller its own private static method)
class Menu
{
    static User? CurrentUser = null;
    public static void Start()
    {
        Console.Title = "24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;
        Console.WriteLine("Welcome to 24-7 binge watch cinema!");

        // asks the user to choose either of these options
        MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
            {"Register", ()=>{
                // run Register method
                // UserLogic.Register();
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
}
