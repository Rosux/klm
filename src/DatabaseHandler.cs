using System;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

// MODEL (handle sqlite related things here such as insert's and update's)
public class DatabaseHandler
{
    private static readonly string _CreateUserString = @"
        CREATE TABLE IF NOT EXISTS Users (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            Email TEXT NOT NULL,
            Password TEXT NOT NULL,
            Role TEXT DEFAULT USER NOT NULL
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
    
    private SQLiteConnection _Conn = new SQLiteConnection();
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
        string createTablesString = @"
        CREATE TABLE IF NOT EXISTS Users (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            FirstName TEXT NOT NULL,
            LastName TEXT NOT NULL,
            Email TEXT NOT NULL,
            Password TEXT NOT NULL,
            Role TEXT NOT NULL
        )
        ";
        SQLiteCommand createTables = new SQLiteCommand(createTablesString, _Conn);
        createTables.ExecuteNonQuery();
        _Conn.Close();
    }

    public void SaveUser(User user)
    {
        _Conn.Open();
        string insertSql = @"
            INSERT INTO Users (FirstName, LastName, Email, Password, Role)
            VALUES (@FirstName, @LastName, @Email, @Password, @User)
        ";

        using (SQLiteCommand command = new SQLiteCommand(insertSql, _Conn))
        {
            command.Parameters.AddWithValue("@FirstName", user.firstName);
            command.Parameters.AddWithValue("@LastName", user.lastName);
            command.Parameters.AddWithValue("@Email", user.email);
            command.Parameters.AddWithValue("@Password", user.password);
            command.Parameters.AddWithValue("@User", user.Role);

            command.ExecuteNonQuery();
        }

        _Conn.Close();
    }
    public User CheckUser(User login)
    {
        _Conn.Open();
        string selectSql = @"
            SELECT *
            FROM Users
            WHERE Email = @Email AND Password = @Password
        ";

        User currentUser = null;

        using (SQLiteCommand command = new SQLiteCommand(selectSql, _Conn))
        {
            command.Parameters.AddWithValue("@Email", login.email);
            command.Parameters.AddWithValue("@Password", login.password);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    currentUser = new User(
                        reader["firstName"].ToString(),
                        reader["lastName"].ToString(),
                        reader["email"].ToString(),
                        reader["password"].ToString(),
                        reader["Role"].ToString()
                    );
                }
            }
        }

        _Conn.Close();

        return currentUser;
    }
}
