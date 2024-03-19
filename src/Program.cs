using System;

// CONTROLLER (handle logic here such as updating user data) (make for each controller its own private static method)
class Program
{
    // User? User = null;
    private static DatabaseHandler DB = new DatabaseHandler();
    static void Main(string[] args)
    {
        Console.Title = "24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;
        Console.WriteLine("Welcome to 24-7 binge watch cinema!");

        // asks the user to choose either of these options
        // Menu.Options("Choose an option", new Dictionary<string, Action>(){
        //     {"Register", ()=>{
        //         // Register code here or call a Register method
        //     }},
        //     {"Login", ()=>{
        //         // Login code here or call a Login method
        //     }},
        //     {"Exit", ()=>{
        //         // Exit code here or call a Exit method
        //     }},
        // });

        // asks the user for a time in HH:MM
        // Menu.Time();
    }
}
