public class ConsumptionAcces : DatabaseHandler
{
    public bool CreateConsumption(Consumption consumption){
        _Conn.Open();
        string NewQuery = @"INSERT INTO Consumptions(Name, Price, StartTime, EndTime)
        VALUES (@Name, @Price, @StartTime, @EndTime)";
        int rowsAffected;
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@Name", consumption.Name);
            Launch.Parameters.AddWithValue("@Price", consumption.Price);
            Launch.Parameters.AddWithValue("@StartTime", consumption.StartTime.Hour.ToString("00") + ":" + consumption.StartTime.Minute.ToString("00"));
            Launch.Parameters.AddWithValue("@EndTime", consumption.EndTime.Hour.ToString("00") + ":" + consumption.EndTime.Minute.ToString("00"));
            rowsAffected = Launch.ExecuteNonQuery();
            _Conn.Close();
        }
        return rowsAffected > 0;
    }
    public List<Consumption> ReadConsumption(){
        List<Consumption> consumptions = new List<Consumption>();
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Consumptions";
        using (SQLiteCommand Show = new SQLiteCommand(NewQuery, _Conn))
        {
            SQLiteDataReader reader = Show.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                double price = reader.GetDouble(2);
                TimeOnly startTime = TimeOnly.Parse(reader.GetString(3));
                TimeOnly endTime = TimeOnly.Parse(reader.GetString(4));
                consumptions.Add(new Consumption(id, name, price, startTime, endTime));
            }
        }
        _Conn.Close();
        return consumptions;
    }
    
    public bool DeleteConsumption(Consumption consumption){
        _Conn.Open();
        string NewQuery = @"DELETE FROM Consumptions WHERE ID = @Id ";
        using(SQLiteCommand Remove = new SQLiteCommand(NewQuery, _Conn))
        {
            Remove.Parameters.AddWithValue("@Id", consumption.Id);
            int rowsAffected = Remove.ExecuteNonQuery();
            _Conn.Close();
            return rowsAffected > 0;
        }
    }

    public bool UpdateConsumption(Consumption consumption){
        _Conn.Open();
        int rowsAffected = -1;
        string updateConsumption = @"UPDATE Consumptions SET Name = @Name, Price = @Price, StartTime = @StartTime, EndTime = @EndTime WHERE ID = @Id";
        using (SQLiteCommand comm = new SQLiteCommand(updateConsumption, _Conn)){
            comm.Parameters.AddWithValue("@Name", consumption.Name);
            comm.Parameters.AddWithValue("@Price", consumption.Price);
            comm.Parameters.AddWithValue("@StartTime", consumption.StartTime.Hour.ToString("00") + ":" + consumption.StartTime.Minute.ToString("00"));
            comm.Parameters.AddWithValue("@EndTime", consumption.EndTime.Hour.ToString("00") + ":" + consumption.EndTime.Minute.ToString("00"));
            comm.Parameters.AddWithValue("@Id", consumption.Id);
            rowsAffected = comm.ExecuteNonQuery();
        }
        _Conn.Close();
        return rowsAffected > 0;
    }
}