using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ReservationAccess : DatabaseHandler
{
    public ReservationAccess(string DatabasePath="./DataSource/CINEMA.db") : base(DatabasePath){}

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
        string NewQuery = @"DELETE FROM Reservations WHERE ID = @Id ";
        using(SQLiteCommand Remove = new SQLiteCommand(NewQuery, _Conn))
        {
            Remove.Parameters.AddWithValue("@Id", reservation.Id);
            int rowsAffected = Remove.ExecuteNonQuery();
            _Conn.Close();
            return rowsAffected > 0;
        }
    }

    /// <summary>
    /// Convert a json timeline string to a TimeLine.Holder object.
    /// </summary>
    /// <param name="timeLine">The json string containing the timeline data.</param>
    /// <returns>A TimeLine.Holder object with actual values.</returns>
    public static TimeLine.Holder StringToTimeLine(string timeLine){
        List<TimeLine.Item> timeline = JsonConvert.DeserializeObject<List<TimeLine.Item>>(timeLine)!;
        TimeLine.Holder TimelineHolder = new TimeLine.Holder();
        TimelineHolder.t = timeline;

        TimeLine.Holder newTimeLine = new TimeLine.Holder();
        foreach(TimeLine.Item i in TimelineHolder.t){
            // i.Action;
            JObject x = JObject.Parse(JsonConvert.SerializeObject(i.Action)); // <- is either a film/serie/break/consumption
            object? obj = null;

            if(x.Property("Id") != null && x.Property("Genres") != null && x.Property("Original_language") != null && x.Property("Overview") != null && x.Property("Release_date") != null && x.Property("Runtime") != null && x.Property("Title") != null && x.Property("Vote_average") != null && x.Property("Certification") != null && x.Property("Directors") != null)
            {
                obj = (object)new Film(
                    (int)(x.Property("Id").Value),
                    (List<string>)JsonConvert.DeserializeObject<List<string>>(x.Property("Genres").Value.ToString()),
                    (string)(x.Property("Original_language").Value),
                    (string)(x.Property("Overview").Value),
                    (string)(x.Property("Release_date").Value),
                    (int)(x.Property("Runtime").Value),
                    (string)(x.Property("Title").Value),
                    (double)(x.Property("Vote_average").Value),
                    (string)(x.Property("Certification").Value),
                    (List<string>)JsonConvert.DeserializeObject<List<string>>(x.Property("Directors").Value.ToString())
                );
            }
            else if(x.Property("Title") != null && x.Property("Length") != null && x.Property("Id") != null)
            {
                obj = (object)new Episode(
                    (int)(x.Property("Id").Value),
                    (string)(x.Property("Title").Value),
                    (int)(x.Property("Length").Value)
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
            else if(x.Property("Time") != null)
            {
                obj = (object)new Break(
                    (int)(x.Property("Time").Value)
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
}
