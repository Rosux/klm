using System;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

public class DatabaseHandler
{
    private SQLiteConnection? conn = null;
    private readonly string DatabasePath = "./database/CINEMA.db";
    public DatabaseHandler(){
        if (!File.Exists(DatabasePath)){
            SQLiteConnection.CreateFile(DatabasePath);
        }
        conn = new SQLiteConnection($"DATA Source={DatabasePath};Version=3");
        CreateDatabaseIfNotExist();
    }

    private void CreateDatabaseIfNotExist(){
        conn.Open();
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
        SQLiteCommand createTables = new SQLiteCommand(createTablesString, conn);
        createTables.ExecuteNonQuery();
        conn.Close();
    }

    public void SaveUser(User user)
    {
        conn.Open();
        string insertSql = @"
            INSERT INTO Users (FirstName, LastName, Email, Password, Role)
            VALUES (@FirstName, @LastName, @Email, @Password, @User)
        ";

        using (SQLiteCommand command = new SQLiteCommand(insertSql, conn))
        {
            command.Parameters.AddWithValue("@FirstName", user.firstName);
            command.Parameters.AddWithValue("@LastName", user.lastName);
            command.Parameters.AddWithValue("@Email", user.email);
            command.Parameters.AddWithValue("@Password", user.password);
            command.Parameters.AddWithValue("@User", user.Role);

            command.ExecuteNonQuery();
        }

        conn.Close();
    }
    public User CheckUser(User login)
    {
        conn.Open();
        string selectSql = @"
            SELECT *
            FROM Users
            WHERE Email = @Email AND Password = @Password
        ";

        User currentUser = null;

        using (SQLiteCommand command = new SQLiteCommand(selectSql, conn))
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

        conn.Close();

        return currentUser;
    }
}
