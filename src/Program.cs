﻿using System;

// CONTROLLER (handle logic here such as updating user data) (make for each controller its own private static method)
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
        Menu.SelectOptions("Choose an option", new Dictionary<string, Action>(){
            {"Register", ()=>{
                // run Register method
                // Register();
            }},
            {"Login", ()=>{
                // run Login method
                // Login();
            }},
            {"Exit", ()=>{
                // close application
                Environment.Exit(1);
            }},
        });
    }
}
