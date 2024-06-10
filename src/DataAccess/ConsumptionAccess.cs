public class ConsumptionAccess : DatabaseHandler
{
    public static ReservationAccess _reservationAccess = new ReservationAccess();

    public ConsumptionAccess(string? DatabasePath=null) : base(DatabasePath){}

    /// <summary>
    /// Creates a new consumption item.
    /// </summary>
    /// <param name="consumption">A consumption instance to save. The id property will not be used.</param>
    /// <returns>A boolean indicating if the creation was successful.</returns>
    public bool CreateConsumption(Consumption consumption){
        _Conn.Open();
        string NewQuery = @"INSERT INTO Consumptions(Name, Price, StartTime, EndTime)
        VALUES (@Name, @Price, @StartTime, @EndTime); SELECT last_insert_rowid();";
        int lastId = -1;
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@Name", consumption.Name);
            Launch.Parameters.AddWithValue("@Price", consumption.Price);
            Launch.Parameters.AddWithValue("@StartTime", consumption.StartTime.Hour.ToString("00") + ":" + consumption.StartTime.Minute.ToString("00"));
            Launch.Parameters.AddWithValue("@EndTime", consumption.EndTime.Hour.ToString("00") + ":" + consumption.EndTime.Minute.ToString("00"));
            lastId = Convert.ToInt32(Launch.ExecuteScalar());
            _Conn.Close();
        }
        consumption.Id = lastId == -1 ? -1 : lastId;
        return lastId != -1;
    }

    /// <summary>
    /// Gets all consumptions from the database and returns them.
    /// </summary>
    /// <returns>A list of all consumptions.</returns>
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

    /// <summary>
    /// Deletes a consumption item.
    /// </summary>
    /// <param name="consumption">A consumption instance to delete. Must have a valid id.</param>
    /// <param name="noReservations">If it cant have reservations make this true.</param>
    /// <returns>A boolean indicating if the deletion was successful.</returns>
    public bool DeleteConsumption(Consumption consumption, bool noReservations){
        _Conn.Open();
        int rowsAffected = 0;
        string NewQuery = @"DELETE FROM Consumptions WHERE ID = @Id ";
        using(SQLiteCommand Remove = new SQLiteCommand(NewQuery, _Conn))
        {
            Remove.Parameters.AddWithValue("@Id", consumption.Id);
            rowsAffected = Remove.ExecuteNonQuery();
            _Conn.Close();
        }
        bool changed = false;
        if(consumption != null && noReservations == false)
        {
            List<Reservation> Reservations = _reservationAccess.GetAllReservations();
            foreach(Reservation Reservation in Reservations)
            {
                foreach(var TimelineObject in Reservation.TimeLine.Items.ToList())
                {
                    if(TimelineObject.Action is Consumption)
                    {
                        if(((Consumption)TimelineObject.Action).Id == consumption.Id)
                        {
                            Reservation.TimeLine.Items.Remove((TimeLine.Item)TimelineObject);
                        }
                    }
                }
                changed = _reservationAccess.EditReservation(Reservation);
            }
            return changed;
        }
        return rowsAffected > 0;
    }

    /// <summary>
    /// Updates a consumption item.
    /// </summary>
    /// <param name="consumption">A consumption instance to update. Must hold the new data and have a valid id.</param>
    /// <returns>A boolean indicating if the update was successful.</returns>
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

    /// <summary>
    /// Checks if any Consumption item with the same name already exists.
    /// </summary>
    /// <param name="name">The search name to use.</param>
    /// <returns>A boolean indicating if an item with the same name already exists.</returns>
    public bool ConsumptionExists(string name){
        bool exists = false;
        _Conn.Open();
        string existsQuery = @"SELECT * FROM Consumptions WHERE Name = @Name";
        using (SQLiteCommand command = new SQLiteCommand(existsQuery, _Conn))
        {
            command.Parameters.AddWithValue("@Name", name);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                exists = true;
            }
        }
        _Conn.Close();
        return exists;
    }
}
