global using System.Data.Entity.Infrastructure.Design;
global using System.Data.SQLite;
global using System.Data.SQLite.Generic;
using System;
using System.Data;

/// <summary>
/// Creates a database handle and connection. Creates the tables if they do not exist.
/// </summary>
public class DatabaseHandler
{
    private static readonly string _CreateUserString = @"
        CREATE TABLE IF NOT EXISTS Users (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            Email TEXT NOT NULL,
            Password TEXT NOT NULL,
            Role TEXT DEFAULT 'USER' NOT NULL
        )
    ";

    private static readonly string _CreateConsumtionString = @"
        CREATE TABLE IF NOT EXISTS Consumptions(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Price REAL NOT NULL,
            StartTime TEXT NOT NULL,
            EndTime TEXT NOT NULL
        )
    ";

    private static readonly string _CreateRoomsString = @"
        CREATE TABLE IF NOT EXISTS Rooms(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            Capacity INTEGER NOT NULL,
            Seats TEXT NOT NULL
        )
    "; // Seats is a bool[][] array indicating the seat layout

    private static readonly string _CreateReservations =  @"
        CREATE TABLE IF NOT EXISTS Reservations(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            RoomId INTEGER NOT NULL,
            UserId INTEGER NOT NULL,
            GroupSize INTEGER NOT NULL,
            StartDate TEXT NOT NULL,
            EndDate TEXT NOT NULL,
            Price REAL NOT NULL,
            TimeLine TEXT NOT NULL,
            Entertainments TEXT NOT NULL,
            FOREIGN KEY (RoomId) REFERENCES Rooms(ID),
            FOREIGN KEY (UserId) REFERENCES Users(ID)
        )
    "; // Entertainments is a List<Entertainment> containg all the special entertainments of the reservation

    protected SQLiteConnection _Conn = new SQLiteConnection();

    /// <summary>
    /// Creates a new connection to the database and sets up the database if needed.
    /// </summary>
    /// <param name="DatabasePath">A string containing the database filepath.</param>
    public DatabaseHandler(string? DatabasePath=null){
        if(DatabasePath == null){
            if(Environment.GetEnvironmentVariable("DATABASE_PATH") == null){
                throw new Exception($"Environment DATABASE_PATH not set.");
            }
            DatabasePath = Environment.GetEnvironmentVariable("DATABASE_PATH") ?? "";
        }
        if (!File.Exists(DatabasePath)){
            SQLiteConnection.CreateFile(DatabasePath);
        }
        _Conn = new SQLiteConnection($"DATA Source={DatabasePath};Version=3");
        CreateDatabaseIfNotExist();
    }

    /// <summary>
    /// Creates the database tables if they do not exist.
    /// </summary>
    private void CreateDatabaseIfNotExist(){
        _Conn.Open();
        List<SQLiteCommand> Tables = new List<SQLiteCommand>(){
            new SQLiteCommand(_CreateUserString, _Conn),
            new SQLiteCommand(_CreateConsumtionString, _Conn),
            new SQLiteCommand(_CreateRoomsString, _Conn),
            new SQLiteCommand(_CreateReservations, _Conn),
        };
        foreach (SQLiteCommand comm in Tables){
            comm.ExecuteNonQuery();
        }
        _Conn.Close();
    }
}
