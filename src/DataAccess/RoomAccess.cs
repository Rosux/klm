using Newtonsoft.Json;

public class RoomAccess : DatabaseHandler {
    public RoomAccess(string? DatabasePath=null) : base(DatabasePath){}

    /// <summary>
    /// Create a new room.
    /// </summary>
    /// <param name="room">An instance of the room to create.</param>
    /// <returns>A room if creation is succesful otherwise null.</returns>
    public Room? AddRoom(Room room)
    {
        Room? newRoom = null;
        _Conn.Open();
        string query = "INSERT INTO Rooms (Capacity, Seats, RoomName) VALUES (@capacity, @seats, @roomname); SELECT * FROM Rooms WHERE ID = last_insert_rowid();";
        using(SQLiteCommand Command = new SQLiteCommand(query, _Conn)){
            Command.Parameters.AddWithValue("@capacity", room.Capacity);
            Command.Parameters.AddWithValue("@seats", JsonConvert.SerializeObject(room.Seats));
            Command.Parameters.AddWithValue("@roomname", room.RoomName);
            SQLiteDataReader reader = Command.ExecuteReader();
            while(reader.Read()){
                room.Id = reader.GetInt32(0);
                newRoom = new Room(reader.GetInt32(0), reader.GetString(2), reader.GetString(3));
            }
        }
        _Conn.Close();
        return newRoom;
    }

    /// <summary>
    /// Updates a room item in the database.
    /// </summary>
    /// <param name="room">A room instance to update. Must hold the new data and have a valid id.</param>
    /// <returns>A boolean indicating if the update was successful.</returns>
    public bool EditRoom(Room room)
    {
        int rowsAffected;
        _Conn.Open();
        string query = "UPDATE Rooms SET Capacity=@capacity, Seats=@seats, RoomName=@roomname WHERE ID=@id";
        using(SQLiteCommand Command = new SQLiteCommand(query, _Conn)){
            Command.Parameters.AddWithValue("@id", room.Id);
            Command.Parameters.AddWithValue("@capacity", room.Capacity);
            Command.Parameters.AddWithValue("@seats", JsonConvert.SerializeObject(room.Seats));
            Command.Parameters.AddWithValue("@roomname", room.RoomName);
            rowsAffected = Command.ExecuteNonQuery();
        }
        _Conn.Close();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Remove a room with specific ID.
    /// </summary>
    /// <param name="id">The room with ID to remove.</param>
    /// <returns>A boolean indicating if the room was removed.</returns>
    public bool RemoveRoom(int id)
    {
        int rowsAffected;
        _Conn.Open();
        string query = "DELETE FROM Rooms WHERE ID=@id;DELETE FROM Reservations WHERE RoomId = @id";
        using(SQLiteCommand Command = new SQLiteCommand(query, _Conn)){
            Command.Parameters.AddWithValue("@id", id);
            rowsAffected = Command.ExecuteNonQuery();
        }
        _Conn.Close();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Returns a room with specific ID, null if not found.
    /// </summary>
    /// <param name="id">The ID to search for.</param>
    /// <returns>A Room object if found otherwise null.</returns>
    public Room? GetRoomById(int id)
    {
        Room? r = null;
        _Conn.Open();
        string query = "SELECT * from Rooms WHERE ID=@id";
        using(SQLiteCommand Command = new SQLiteCommand(query, _Conn)){
            Command.Parameters.AddWithValue("@id", id);
            SQLiteDataReader reader = Command.ExecuteReader();
            while(reader.Read())
            {
                r = new Room(reader.GetInt32(0), reader.GetString(2), reader.GetString(3));
            }
        }
        _Conn.Close();
        return r;
    }

    /// <summary>
    /// Returns a list of all rooms.
    /// </summary>
    /// <param name="filterMinSize">Set the minimum size filter. will only return rooms that are bigger or equal in size.</param>
    /// <returns>A list of rooms.</returns>
    public List<Room> GetAllRooms(int filterMinSize=0){
        List<Room> roomlist = new List<Room>();
        _Conn.Open();
        string query = "SELECT * FROM Rooms WHERE Capacity >= @MinRoomSize";
        using(SQLiteCommand Command = new SQLiteCommand(query, _Conn)){
            Command.Parameters.AddWithValue("@MinRoomSize", filterMinSize);
            SQLiteDataReader reader = Command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string layoutJson = reader.GetString(2);
                string roomname = reader.GetString(3);
                roomlist.Add(new Room(id, layoutJson, roomname));
            }
        }
        _Conn.Close();
        return roomlist;
    }

    /// <summary>
    /// Get the biggest room capacity currently available.
    /// </summary>
    /// <returns>An integer defining the biggest room capacity.</returns>
    public int GetMaxRoomCapacity()
    {
        int maxCapacity = 0;
        _Conn.Open();
        string query = "SELECT MAX(Capacity) FROM Rooms";
        using(SQLiteCommand Command = new SQLiteCommand(query, _Conn))
        {
            object result = Command.ExecuteScalar();
            if (result != DBNull.Value)
            {
                maxCapacity = Convert.ToInt32(result);
            }
        }
        _Conn.Close();
        return maxCapacity;
    }
}
