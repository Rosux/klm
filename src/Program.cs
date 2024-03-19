using System;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to 24-7 binge watch cinema!");
        DatabaseHandler DB = new DatabaseHandler();
        Console.ReadLine();
    }
}
