public class RoomAccess : DatabaseHandler {
    public RoomAccess(string DatabasePath="./DataSource/CINEMA.db") : base(DatabasePath){

    }

    public int AddToRoomTable(Room room)
    {
        int newRoomId = -1;
        _Conn.Open();
        string query = "INSERT INTO Rooms (Capacity) VALUES (@capacity); SELECT last_insert_rowid();";
        var command = new SQLiteCommand(query, _Conn);
        command.Parameters.AddWithValue("@capacity", room.Capacity);
        newRoomId = Convert.ToInt32(command.ExecuteScalar());
        _Conn.Close();
        return newRoomId;
    }
    public void EditFromRoomTable(int id, int capacity)
    {
        _Conn.Open();
        string query = "UPDATE Rooms SET Capacity=@capacity WHERE ID=@id";
        var command = new SQLiteCommand(query, _Conn);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@capacity", capacity);
        command.ExecuteNonQuery();
        _Conn.Close();
    }
    public void RemoveFromRoomTable(int roomid)
    {
        _Conn.Open();
        string query = "DELETE FROM Rooms WHERE ID=@id";
        var command = new SQLiteCommand(query, _Conn);
        command.Parameters.AddWithValue("@id", roomid);
        command.ExecuteNonQuery();
        _Conn.Close();
    }
    public string GetRoom(int id)
    {
        string RoomInfo = "";
        _Conn.Open();
        string query = "SELECT * from Rooms WHERE ID=@id";
        var command = new SQLiteCommand(query, _Conn);
        command.Parameters.AddWithValue("@id", id);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read()) 
                {
                    RoomInfo = $"Room-ID = {reader.GetValue(0).ToString()} Capacity = {reader.GetValue(1).ToString()}";
                }
        _Conn.Close();
        return RoomInfo;
    }
    public List<string> _getAllRooms()
    {
        _Conn.Open();
        List<string> roomlist = new List<string>();
        string query = "SELECT * FROM Rooms";
        var command = new SQLiteCommand(query, _Conn);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())  
                {  
                    roomlist.Add($"Room-ID = {reader.GetValue(0).ToString()} Capacity = {reader.GetValue(1).ToString()}");   
                } 
        _Conn.Close();
        return roomlist;
    }

    public List<Room> GetAllRooms(int filterMinSize=0){
        List<Room> roomlist = new List<Room>();
        _Conn.Open();
        string query = "SELECT * FROM Rooms WHERE Capacity >= @MinRoomSize";
        using (SQLiteCommand Command = new SQLiteCommand(query, _Conn)){
            Command.Parameters.AddWithValue("@MinRoomSize", filterMinSize);
            SQLiteDataReader reader = Command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int nocap = reader.GetInt32(1);
                roomlist.Add(new Room(id, nocap));
            }
        }
        _Conn.Close();
        return roomlist;
    }

    public int GetMaxRoomCapacity()
    {
        int maxCapacity = 0;
        _Conn.Open();
        string query = "SELECT MAX(Capacity) FROM Rooms";
        using (SQLiteCommand Command = new SQLiteCommand(query, _Conn))
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
