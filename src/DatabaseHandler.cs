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
            Password TEXT NOT NULL
        )
        ";
        SQLiteCommand createTables = new SQLiteCommand(createTablesString, conn);
        createTables.ExecuteNonQuery();
        conn.Close();
    }

    public void SaveUser(string firstName, string lastName, string email, string password)
    {
        conn.Open();
        string insertSql = @"
            INSERT INTO Users (FirstName, LastName, Email, Password)
            VALUES (@FirstName, @LastName, @Email, @Password)
        ";

        using (SQLiteCommand command = new SQLiteCommand(insertSql, conn))
        {
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);
            command.ExecuteNonQuery();
        }

        conn.Close();
    }
}
