using System;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

class Program
{
    // User? LoggedInUser = null;
    static void Main(string[] args)
    {
        Console.Title = "24/7 BINGE WATCH CINEMA!";
        Console.WriteLine("Welcome to 24-7 binge watch cinema!");
        Console.CursorVisible = false;
        DatabaseHandler DB = new DatabaseHandler();
        // Console.ReadLine();
        Console.WriteLine(SelectTime());
    }

    static DateTime SelectTime(){
        TimeOnly time = new TimeOnly();
        bool hour = true;
        ConsoleKey key;
        do{
            Console.Clear();
            Console.BackgroundColor = hour ? ConsoleColor.DarkGray : ConsoleColor.Black;
            Console.Write($"{time.Hour.ToString("00")}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($":");
            Console.BackgroundColor = !hour ? ConsoleColor.DarkGray : ConsoleColor.Black;
            Console.Write($"{time.Minute.ToString("00")}");
            Console.BackgroundColor = ConsoleColor.Black;
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.LeftArrow)
            {
                hour = true;
            }
            if (key == ConsoleKey.RightArrow)
            {
                hour = false;
            }
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
            {
                double TimeAmount = key == ConsoleKey.DownArrow ? -1 : 1;
                if (hour){
                    time = time.AddHours(TimeAmount);
                } else {
                    time = time.AddMinutes(TimeAmount);
                }
            }
        } while (key != ConsoleKey.Enter);
        Console.Clear();
        return new DateTime() + time.ToTimeSpan();
    }
}
