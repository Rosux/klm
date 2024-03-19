using System;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

class Program
{
    static void Main(string[] args)
    {
        DatabaseHandler dbHandler = new DatabaseHandler();
        Menu(dbHandler);
    }

    static void Menu(DatabaseHandler dbHandler)
    {
        bool exit = false;
        bool loggedin = false;
        while (!exit)
        {
            Console.WriteLine("Type the number of the action you want to perform:");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");

            if (!int.TryParse(Console.ReadLine(), out int menuAnswer))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (menuAnswer)
            {
                case 1:
                    Console.WriteLine("Enter your first name:");
                    string firstName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(firstName))
                    {
                        Console.WriteLine("First name cannot be empty. Please try again.");
                        break;
                    }
                    Console.WriteLine("Enter your last name:");
                    string lastName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(lastName))
                    {
                        Console.WriteLine("Last name cannot be empty. Please try again.");
                        break;
                    }
                    string email;
                    do
                    {
                        Console.WriteLine("Enter your email:");
                        email = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(email))
                        {
                            Console.WriteLine("Email cannot be empty. Please try again.");
                            continue;
                        }
                    } while (!IsValidEmail(email));
                    Console.WriteLine("Enter your password (totally secured btw):");
                    string password = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        Console.WriteLine("Password cannot be empty. Please try again.");
                        break;
                    }
                    User user = new User(firstName, lastName, email, password);
                    dbHandler.SaveUser(user);
                    Console.WriteLine("Your information has been saved. Proceed to login.");
                    break;
                case 2:
                    Console.WriteLine("Enter your email:");
                    string LoginEmail = Console.ReadLine();
                    Console.WriteLine("Enter your password:");
                    string LoginPassword = Console.ReadLine();
                    break;
                case 3:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a valid action.");
                    break;
            }
        }
    }
    static bool IsValidEmail(string email)
    {
        // Basic email format validation
        if (email.Contains("@") && (email.EndsWith(".com") || email.EndsWith(".nl")))
        {
            return true;
        }
        else
        {
            Console.WriteLine("Invalid email format. Please enter a valid email address.");
            return false;
        }
    }
}

