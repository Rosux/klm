using System.Data.SQLite;

[TestFixture]
public class RoomTest
{
    protected SQLiteConnection _Conn = new SQLiteConnection();
    RoomAccess? r = null;

    [TearDown]
    public void Cleanup()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if (File.Exists("./DataSource/TEST.db"))
        {
            File.Delete("./DataSource/TEST.db");
        }
        if (Directory.Exists("./DataSource"))
        {
            Directory.Delete("./DataSource");
        }
    }

    [SetUp]
    public void Setup(){
        string connectionString = "Data Source=./DataSource/TEST.db;Version=3;";
        _Conn = new SQLiteConnection(connectionString);
        System.IO.Directory.CreateDirectory("./DataSource/");
        r = new RoomAccess("./DataSource/TEST.db");
    }

    [Test]
    public void InsertionTest()
    {
        var room = new Room(
            layout: new bool[][]
            {
                new bool[] { false, true, true, true, false },
                new bool[] { true, true, true, true, true },
                new bool[] { true, true, true, true, true },
                new bool[] { true, true, true, true, true },
                new bool[] { false, true, true, true, false }
            },
            roomName: "Insert Room"
        );

        Room? insertResult = r.AddRoom(room);
        Assert.IsNotNull(insertResult, "Failed to insert Room");

        List<Room> readResult = new List<Room>();

        _Conn.Open();
        string query = $"SELECT * FROM Rooms WHERE Id = {insertResult.Id}";
        using (SQLiteCommand command = new SQLiteCommand(query, _Conn))
        {
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int fetchedId = reader.GetInt32(0);
                string seats = reader.GetString(2);
                string roomName = reader.GetString(3);
                readResult.Add(new Room(fetchedId, seats, roomName));
            }
        }
        _Conn.Close();

        foreach (var roomcheck in readResult)
        {
            if (roomcheck.Id == insertResult.Id && roomcheck.Capacity == insertResult.Capacity && roomcheck.Seats.Length == insertResult.Seats.Length && roomcheck.RoomName == insertResult.RoomName)
            {
                Assert.Pass("Inserted Room found in the list.");
            }
        }
        Assert.Fail("Inserted Room not found in the list.");
    }

    [Test]
    public void RemoveTest()
    {
        var room = new Room(
            layout: new bool[][]
            {
                new bool[] { false, true, true, true, false },
                new bool[] { true, true, true, true, true },
                new bool[] { true, true, true, true, true },
                new bool[] { true, true, true, true, true },
                new bool[] { false, true, true, true, false }
            },
            roomName: "Remove Room"
        );
        _Conn.Open();
        string query = @"INSERT INTO Rooms (Capacity, Seats, RoomName) VALUES (@capacity, @seats, @roomname); SELECT * FROM Rooms WHERE ID = last_insert_rowid();";
        int roomId = -1;
        using(SQLiteCommand Command = new SQLiteCommand(query, _Conn)){
            Command.Parameters.AddWithValue("@capacity", room.Capacity);
            Command.Parameters.AddWithValue("@seats", JsonConvert.SerializeObject(room.Seats));
            Command.Parameters.AddWithValue("@roomname", room.RoomName);
            roomId = Convert.ToInt32(Command.ExecuteScalar());
        }
        _Conn.Close();

        bool removeResult = r.RemoveRoom(roomId);
        bool isDatabaseEmpty;
        using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM Rooms", _Conn))
        {
            _Conn.Open();
            int rowCount = Convert.ToInt32(checkCommand.ExecuteScalar());
            isDatabaseEmpty = rowCount == 0;
            _Conn.Close();
        }

        if (removeResult)
        {
            if (isDatabaseEmpty)
            {
                Assert.Pass("Room successfully removed and database is empty.");
            }
            else
            {
                Assert.Pass("Room successfully removed.");
            }
        }
        else
        {
            Assert.Fail("Failed to remove Room.");
        }
    }

    [Test]
    public  void ReadTest()
    {
        var room = new Room(
            layout: new bool[][]
            {
                new bool[] { false, true, true, true, false },
                new bool[] { true, true, true, true, true },
                new bool[] { true, true, true, true, true },
                new bool[] { true, true, true, true, true },
                new bool[] { false, true, true, true, false }
            },
            roomName: "Read Room"
        );
        _Conn.Open();
        string query = @"INSERT INTO Rooms (Capacity, Seats, RoomName) VALUES (@capacity, @seats, @roomname); SELECT * FROM Rooms WHERE ID = last_insert_rowid();";
        int roomId = -1;
        using(SQLiteCommand Command = new SQLiteCommand(query, _Conn)){
            Command.Parameters.AddWithValue("@capacity", room.Capacity);
            Command.Parameters.AddWithValue("@seats", JsonConvert.SerializeObject(room.Seats));
            Command.Parameters.AddWithValue("@roomname", room.RoomName);
            roomId = Convert.ToInt32(Command.ExecuteScalar());
        }
        _Conn.Close();

        List<Room> readResult = r.GetAllRooms();

        foreach (var roomcheck in readResult)
        {
            if (roomcheck.Id == roomId && roomcheck.Seats.Length == room.Seats.Length && roomcheck.RoomName == room.RoomName)
            {
                Assert.Pass("Inserted Room found in the list.");
            }
        }

        Assert.Fail("Inserted Room not found in the list.");
    }
}
