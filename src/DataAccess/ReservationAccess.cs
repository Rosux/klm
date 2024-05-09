using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ReservationAccess : DatabaseHandler
{
    public ReservationAccess(string DatabasePath="./DataSource/CINEMA.db") : base(DatabasePath){}
    public List<Reservation> ReadReservations(){
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
                List<TimeLine.Item> timeline = JsonConvert.DeserializeObject<List<TimeLine.Item>>(timeline_str)!;
                TimeLine.Holder TimelineHolder = new TimeLine.Holder();
                TimelineHolder.t = timeline;
                reservations.Add(new Reservation(id, roomid, userid, groupsize, startdate, enddate, price, TimelineHolder));
            }
        }
        _Conn.Close();
        return reservations;
    }

    public Reservation? GetReservationById(int Id){
        Reservation? reservations = null;
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Reservations WHERE ID = " + Id;
        using (SQLiteCommand Show = new SQLiteCommand(NewQuery, _Conn))
        {
            SQLiteDataReader reader = Show.ExecuteReader();
            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                int roomid = reader.GetInt32(1);
                int userid = reader.GetInt32(2);
                int groupsize = reader.GetInt32(3);
                DateTime startdate = DateTime.Parse(reader.GetString(4));
                DateTime enddate = DateTime.Parse(reader.GetString(5));
                double price = reader.GetDouble(6);
                #region TimeLine
                    string timeline_str = reader.GetString(7);
                    List<TimeLine.Item> timeline = JsonConvert.DeserializeObject<List<TimeLine.Item>>(timeline_str)!;
                    TimeLine.Holder TimelineHolder = new TimeLine.Holder();
                    TimelineHolder.t = timeline;

                    TimeLine.Holder newTimeLine = new TimeLine.Holder();
                    foreach(TimeLine.Item i in TimelineHolder.t){
                        // i.Action;
                        JObject x = JObject.Parse(JsonConvert.SerializeObject(i.Action)); // <- is either a film/serie/break/consumption
                        object? obj = null;

                        if(x.Property("Title") != null && x.Property("Genre") != null && x.Property("Rating") != null && x.Property("Id") != null && x.Property("Duration") != null)
                        {
                            obj = (object)new Film(
                                (int)(x.Property("Id").Value),
                                (string)(x.Property("Title").Value),
                                (string)(x.Property("Genre").Value),
                                (int)(x.Property("Duration").Value)
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
                        else if(x.Property("Title") != null)
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
                #endregion
                reservations = new Reservation(id, roomid, userid, groupsize, startdate, enddate, price, newTimeLine);
            }
        }
        _Conn.Close();
        return reservations;
    }

    /// <summary>
    /// looks through the database for reservations during given date
    /// </summary>
    /// <param name="date"></param>
    /// <returns>a list of all reservations on or during the given date</returns>
    public List<Reservation> ReadReservationsDate(DateTime date){
        List<Reservation> reservations = new List<Reservation>();
        TimeSpan date_s = new TimeSpan(0, 23, 59, 59, 0, 0);
        DateTime date_e_cor = date.Subtract(date_s);
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Reservations WHERE StartDate <= @Date AND EndDate >= @Date OR StartDate >= @SDate AND EndDate <= @Date";
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@SDate", date_e_cor.ToString("yyyy-MM-dd HH:mm:ss"));
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
                List<TimeLine.Item> timeline = JsonConvert.DeserializeObject<List<TimeLine.Item>>(reader.GetString(7));
                TimeLine.Holder TimelineHolder  = new TimeLine.Holder();
                TimelineHolder.t = timeline;
                reservations.Add(new Reservation(id, roomid, userid, groupsize, startdate, enddate, price, TimelineHolder));
            }
        }
        _Conn.Close();
        return reservations;
    }
    /// <summary>
    /// uses the given date to look through the database to pick all reservations for current week
    /// </summary>
    /// <param name="date"></param>
    /// <returns>a list of all reservations during the week of given date</returns>
     public List<Reservation> ReadReservationsWeek(DateTime date){
        List<Reservation> reservations = new List<Reservation>();
        TimeSpan date_s = new TimeSpan(7, 0, 0, 0, 0, 0);
        DateTime date_e = date.Add(date_s);
        DateTime date_e_cor = new DateTime(date_e.Year, date_e.Month, date_e.Day, 23, 59, 59);
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Reservations WHERE StartDate >= @sDate AND StartDate <= @eDate";
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@sDate", date.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@eDate", date_e_cor.ToString("yyyy-MM-dd HH:mm:ss"));
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
                List<TimeLine.Item> timeline = JsonConvert.DeserializeObject<List<TimeLine.Item>>(reader.GetString(7));
                TimeLine.Holder TimelineHolder  = new TimeLine.Holder();
                TimelineHolder.t = timeline;
                reservations.Add(new Reservation(id, roomid, userid, groupsize, startdate, enddate, price, TimelineHolder));
            }
        }
        _Conn.Close();
        return reservations;
    }
    /// <summary>
    /// searches the database for all reservations for a user
    /// </summary>
    /// <returns>a list of all reservations for the user</returns>
    public List<Reservation> ReadReservationsUser(){
        List<Reservation> reservations = new List<Reservation>();
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Reservations WHERE UserId = @UserId";
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@UserId", Program.CurrentUser.Id);
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
                List<TimeLine.Item> timeline = JsonConvert.DeserializeObject<List<TimeLine.Item>>(reader.GetString(7));
                TimeLine.Holder TimelineHolder  = new TimeLine.Holder();
                TimelineHolder.t = timeline;
                reservations.Add(new Reservation(id, roomid, userid, groupsize, startdate, enddate, price, TimelineHolder));
            }
        }
        _Conn.Close();
        return reservations;
    }

    public bool CreateReservation(Reservation reservation){
        _Conn.Open();
        string NewQuery = @"INSERT INTO Reservations(RoomId, UserId, GroupSize, StartDate, EndDate, Price, TimeLine)
        VALUES (@RoomId, @UserId, @GroupSize, @StartDate, @EndDate, @Price, @TimeLine)";
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
            rowsAffected = Launch.ExecuteNonQuery();
        }
        _Conn.Close();
        return rowsAffected > 0;
    }

    /// <summary>
    /// delete a reservation from database
    /// </summary>
    /// <param name="Reservation">a object of Reservations</param>
    /// <returns>a boolean true if deleted</returns>Conn
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
}