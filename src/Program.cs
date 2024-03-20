using System;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

class Program
{
    static User? CurrentUser = null;
    private static DatabaseHandler DB = new DatabaseHandler();
    static void Main(string[] args)
    {
        Console.Title = "24/7 BINGE WATCH CINEMA!";
        Console.CursorVisible = false;
        Console.WriteLine("Welcome to 24-7 binge watch cinema!");
        // asks the user to choose either of these options
        Menu.Options("Choose an option", new Dictionary<string, Action>(){
            {"Register", ()=>{
                Register();
            }},
            {"Login", ()=>{
                Login();
            }},
            {"Exit", ()=>{
                // Exit code here or call a Exit method
            }},
        });
    }

    static void Register()
    {
        Menu.Register();

    }
    static void Login()
    {
        User Credentials = Menu.Login();
        User LoggedUser = DB.CheckUser(Credentials);
        if (LoggedUser == null)
        {
            Console.WriteLine("not logged in bozo");
        }else{
            CurrentUser = LoggedUser;
            Console.WriteLine(CurrentUser.firstName);
        }
    }

}

