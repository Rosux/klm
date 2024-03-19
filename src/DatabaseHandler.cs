using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

public class DatabaseHandler
{
    private static readonly string _CreateUserString = @"
        CREATE TABLE IF NOT EXISTS Users (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            Email TEXT NOT NULL,
            Password TEXT NOT NULL
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
            Capacity INTEGER NOT NULL
        )
    ";
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
            FOREIGN KEY (RoomId) REFERENCES Rooms(ID),
            FOREIGN KEY (UserId) REFERENCES Users(ID)
        )
    ";
    
    private SQLiteConnection? _Conn = null;
    private readonly string DatabasePath = "./database/CINEMA.db";
    public DatabaseHandler(){
        if (!File.Exists(DatabasePath)){
            SQLiteConnection.CreateFile(DatabasePath);
        }
        _Conn = new SQLiteConnection($"DATA Source={DatabasePath};Version=3");
        CreateDatabaseIfNotExist();
    }

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

    // public void SaveUser(User u){
    //     // u.Name;
    //     // u.Password;
    //     // database open
    //     // database insert into
    //     // database close
    // }
}
