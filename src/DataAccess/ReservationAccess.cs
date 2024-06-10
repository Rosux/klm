using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ReservationAccess : DatabaseHandler
{
    public ReservationAccess(string? DatabasePath=null) : base(DatabasePath){}

    /// <summary>
    /// Get a list of all reservations.
    /// </summary>
    /// <returns>A list of all the reservations ever made.</returns>
    public List<Reservation> GetAllReservations(){
        List<Reservation> reservations = new List<Reservation>();
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Reservations";
        using (SQLiteCommand Show = new SQLiteCommand(NewQuery, _Conn))
        {
            SQLiteDataReader reader = Show.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int roomid = reader.GetInt32(1);
                int userid = reader.GetInt32(2);
                int groupsize = reader.GetInt32(3);
                DateTime startdate = DateTime.Parse(reader.GetString(4));
                DateTime enddate = DateTime.Parse(reader.GetString(5));
                double price = reader.GetDouble(6);
                string timeline_str = reader.GetString(7);
                List<Entertainment> entertainments = JsonConvert.DeserializeObject<List<Entertainment>>(reader.GetString(8)) ?? new List<Entertainment>();
                reservations.Add(new Reservation(id, roomid, userid, groupsize, startdate, enddate, price, ReservationAccess.StringToTimeLine(timeline_str), entertainments));
            }
        }
        _Conn.Close();
        return reservations;
    }

    /// <summary>
    /// Get a list of all the reservations within the specified dates.
    /// </summary>
    /// <param name="startDate">A DateOnly holding the start of the search.</param>
    /// <param name="endDate">A DateOnly holding the end of the search.</param>
    /// <returns>A list of all reservations during the given dates.</returns>
    public List<Reservation> GetAllReservationsBetweenDates(DateOnly startDate, DateOnly? endDate=null){
        DateOnly startSearch = startDate;
        DateOnly endSearch = endDate==null ? startSearch : endDate ?? DateOnly.MaxValue;
        List<Reservation> reservations = new List<Reservation>();
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Reservations WHERE date(StartDate) <= date(@endDate) AND date(EndDate) >= date(@startDate)";
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@startDate", new DateTime(startSearch.Year, startSearch.Month, startSearch.Day, 0, 0, 0));
            Launch.Parameters.AddWithValue("@endDate", new DateTime(endSearch.Year, endSearch.Month, endSearch.Day, 0, 0, 0));
            SQLiteDataReader reader = Launch.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int roomid = reader.GetInt32(1);
                int userid = reader.GetInt32(2);
                int groupsize = reader.GetInt32(3);
                DateTime startdate = DateTime.Parse(reader.GetString(4));
                DateTime enddate = DateTime.Parse(reader.GetString(5));
                double price = reader.GetDouble(6);
                string timeline_str = reader.GetString(7);
                List<Entertainment> entertainments = JsonConvert.DeserializeObject<List<Entertainment>>(reader.GetString(8)) ?? new List<Entertainment>();
                reservations.Add(new Reservation(id, roomid, userid, groupsize, startdate, enddate, price, ReservationAccess.StringToTimeLine(timeline_str), entertainments));
            }
        }
        _Conn.Close();
        return reservations;
    }

    /// <summary>
    /// Get a list of all reservations for a specified user.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>A list of all reservations for the specified user.</returns>
    public List<Reservation> GetReservationsByUserId(int id){
        List<Reservation> reservations = new List<Reservation>();
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Reservations WHERE UserId = @UserId";
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@UserId", id);
            SQLiteDataReader reader = Launch.ExecuteReader();
            while (reader.Read())
            {
                int reservationid = reader.GetInt32(0);
                int roomid = reader.GetInt32(1);
                int userid = reader.GetInt32(2);
                int groupsize = reader.GetInt32(3);
                DateTime startdate = DateTime.Parse(reader.GetString(4));
                DateTime enddate = DateTime.Parse(reader.GetString(5));
                double price = reader.GetDouble(6);
                string timeline_str = reader.GetString(7);
                List<Entertainment> entertainments = JsonConvert.DeserializeObject<List<Entertainment>>(reader.GetString(8)) ?? new List<Entertainment>();
                reservations.Add(new Reservation(reservationid, roomid, userid, groupsize, startdate, enddate, price, ReservationAccess.StringToTimeLine(timeline_str), entertainments));
            }
        }
        _Conn.Close();
        return reservations;
    }

    /// <summary>
    /// Create a new reservation.
    /// </summary>
    /// <param name="reservation">A reservation object that gets saved.</param>
    /// <returns>A boolean indicating if the reservation was saved.</returns>
    public bool CreateReservation(Reservation reservation){
        _Conn.Open();
        string NewQuery = @"INSERT INTO Reservations(RoomId, UserId, GroupSize, StartDate, EndDate, Price, TimeLine, Entertainments)
        VALUES (@RoomId, @UserId, @GroupSize, @StartDate, @EndDate, @Price, @TimeLine, @Entertainments)";
        int rowsAffected;
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@RoomId", reservation.RoomId);
            Launch.Parameters.AddWithValue("@UserId", reservation.UserId);
            Launch.Parameters.AddWithValue("@GroupSize", reservation.GroupSize);
            Launch.Parameters.AddWithValue("@StartDate", reservation.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@EndDate", reservation.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@Price", reservation.Price);
            Launch.Parameters.AddWithValue("@TimeLine", reservation.TimeLine.ToString());
            Launch.Parameters.AddWithValue("@Entertainments", JsonConvert.SerializeObject(reservation.Entertainments));
            rowsAffected = Launch.ExecuteNonQuery();
        }
        _Conn.Close();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Delete a reservation from database.
    /// </summary>
    /// <param name="Reservation">A reservation object to delete.</param>
    /// <returns>A boolean indicating if the reservation was deleted.</returns>
    public bool DeleteReservation(Reservation reservation){
        _Conn.Open();
        string NewQuery = @"DELETE FROM Reservations WHERE ID = @Id";
        int rowsAffected;
        using(SQLiteCommand Remove = new SQLiteCommand(NewQuery, _Conn))
        {
            Remove.Parameters.AddWithValue("@Id", reservation.Id);
            rowsAffected = Remove.ExecuteNonQuery();
        }
        _Conn.Close();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Convert a json timeline string to a TimeLine.Holder object.
    /// </summary>
    /// <param name="timeLine">The json string containing the timeline data.</param>
    /// <returns>A TimeLine.Holder object with actual values.</returns>
    public static TimeLine.Holder StringToTimeLine(string timeLine){
        List<TimeLine.Item> timeline = JsonConvert.DeserializeObject<List<TimeLine.Item>>(timeLine)!;
        TimeLine.Holder TimelineHolder = new TimeLine.Holder();
        TimelineHolder.Items = timeline;

        TimeLine.Holder newTimeLine = new TimeLine.Holder();
        foreach(TimeLine.Item i in TimelineHolder.Items){
            // i.Action;
            JObject x = JObject.Parse(JsonConvert.SerializeObject(i.Action)); // <- is either a film/serie/break/consumption
            object? obj = null;

            if(x.Property("Id") != null && x.Property("Title") != null && x.Property("Runtime") != null && x.Property("Description") != null && x.Property("Rating") != null && x.Property("Language") != null && x.Property("Genres") != null && x.Property("ReleaseDate") != null && x.Property("Certification") != null && x.Property("Directors") != null && x.Property("Actors") != null && x.Property("Writers") != null)
            {
                obj = (object)new Film(
                    (int)(x.Property("Id").Value),
                    (string)(x.Property("Title").Value),
                    (int)(x.Property("Runtime").Value),
                    (string)(x.Property("Description").Value),
                    (float)(x.Property("Rating").Value),
                    (string)(x.Property("Language").Value),
                    (List<Genre>)JsonConvert.DeserializeObject<List<Genre>>(x.Property("Genres").Value.ToString()),
                    DateOnly.Parse((x.Property("ReleaseDate").Value.ToString())),
                    (Certification)JsonConvert.DeserializeObject<Certification>(x.Property("Certification").Value.ToString()),
                    (List<string>)JsonConvert.DeserializeObject<List<string>>(x.Property("Directors").Value.ToString()),
                    (List<string>)JsonConvert.DeserializeObject<List<string>>(x.Property("Actors").Value.ToString()),
                    (List<string>)JsonConvert.DeserializeObject<List<string>>(x.Property("Writers").Value.ToString())
                );
            }
            else if(x.Property("Id") != null && x.Property("Title") != null && x.Property("Runtime") != null && x.Property("EpisodeNumber") != null && x.Property("Rating") != null && x.Property("Actors") != null)
            {
                obj = (object)new Episode(
                    (int)(x.Property("Id").Value),
                    (string)(x.Property("Title").Value),
                    (int)(x.Property("Runtime").Value),
                    (int)(x.Property("EpisodeNumber").Value),
                    (float)(x.Property("Rating").Value),
                    (List<string>)JsonConvert.DeserializeObject<List<string>>(x.Property("Actors").Value.ToString())
                );
            }
            else if(x.Property("Id") != null && x.Property("Name") != null && x.Property("Price") != null && x.Property("StartTime") != null && x.Property("EndTime") != null)
            {
                obj = (object)new Consumption(
                    (int)(x.Property("Id").Value),
                    (string)(x.Property("Name").Value),
                    (double)(x.Property("Price").Value),
                    TimeOnly.Parse((string)(x.Property("StartTime").Value)),
                    TimeOnly.Parse((string)(x.Property("EndTime").Value))
                );
            }

            if(obj != null){
                newTimeLine.Add(new TimeLine.Item(
                    obj,
                    i.StartTime,
                    i.EndTime
                ));
            }
        }
        return newTimeLine;
    }
    public bool EditReservation(Reservation reservation)
    {
        _Conn.Open();
        int rowsAffected = -1;
        string UpdateReservation = $@"UPDATE Reservations SET RoomId = @RoomId, UserId= @UserId, GroupSize = @GroupSize, StartDate = @StartDate, EndDate = @EndDate, Price = @Price, TimeLine = @TimeLine, Entertainments = @Entertainments WHERE ID = @Id";
        using (SQLiteCommand Launch = new SQLiteCommand(UpdateReservation, _Conn)){
            Launch.Parameters.AddWithValue("@RoomId", reservation.RoomId);
            Launch.Parameters.AddWithValue("@UserId", reservation.UserId);
            Launch.Parameters.AddWithValue("@GroupSize", reservation.GroupSize);
            Launch.Parameters.AddWithValue("@StartDate", reservation.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@EndDate", reservation.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@Price", reservation.Price);
            Launch.Parameters.AddWithValue("@TimeLine", reservation.TimeLine.ToString());
            Launch.Parameters.AddWithValue("@Entertainments", JsonConvert.SerializeObject(reservation.Entertainments));
            Launch.Parameters.AddWithValue("@Id", reservation.Id);
            rowsAffected = Launch.ExecuteNonQuery();
        }
        _Conn.Close();
        return rowsAffected > 0;
    }
}
