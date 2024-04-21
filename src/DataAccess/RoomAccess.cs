public class RoomAccess : DatabaseHandler {
    public RoomAccess(string DatabasePath="./DataSource/CINEMA.db") : base(DatabasePath)
    {

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

}
