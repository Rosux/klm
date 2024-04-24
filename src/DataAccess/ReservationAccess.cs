using Newtonsoft.Json;
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

    public List<Reservation> PickReservations(DateTime date){
        List<Reservation> reservations = new List<Reservation>();
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Reservations WHERE StartDate <= @Date AND EndDate >= @Date";
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd HH:mm:ss"));
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

    public List<Reservation> PickReservationsUser(){
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