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
        )
        ";
        SQLiteCommand createTables = new SQLiteCommand(createTablesString, conn);
        createTables.ExecuteNonQuery();
        conn.Close();
    }

    // public void SaveUser(User u){
    //     // u.Name;
    //     // u.Password;
    //     // database open
    //     // database insert into
    //     // database close
    // }
}
