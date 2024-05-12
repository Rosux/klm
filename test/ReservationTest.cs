using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[TestFixture]
public class ReservationTest
{
    protected SQLiteConnection _Conn = new SQLiteConnection();
    ReservationAccess? r = null;

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
        r = new ReservationAccess("./DataSource/TEST.db");
    }

    [Test]
    public void InsertionTest()
    {
        TimeLine.Holder Timeline = new TimeLine.Holder();
        var x = new Reservation(
            Id: 1,
            RoomId: 1,
            UserId: 1,
            GroupSize: 1,
            StartDate: DateTime.Parse("2024-04-24 02:00:00"),
            EndDate: DateTime.Parse("2024-04-27 02:00:00"),
            Price: 100.50,
            TimeLine: Timeline
        );
        bool insertResult = r.CreateReservation(x);
        Assert.IsTrue(insertResult, "Failed to insert Consumption.");

        List<Reservation> readResult = new List<Reservation>();

        _Conn.Open();
        string query = $"SELECT * FROM Reservations WHERE Id = {x.Id}";
        using (SQLiteCommand command = new SQLiteCommand(query, _Conn))
        {
            SQLiteDataReader reader = command.ExecuteReader();
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
                readResult.Add(new Reservation(id, roomid, userid, groupsize, startdate, enddate, price, TimelineHolder));
            }
        }
        _Conn.Close();


        foreach (var Reservation in readResult)
        {
            List<TimeLine.Item> timeline_x = JsonConvert.DeserializeObject<List<TimeLine.Item>>(x.TimeLine.ToString());
            if (Reservation.Id == x.Id && Reservation.Price == x.Price && Reservation.RoomId == x.RoomId && Reservation.UserId == x.UserId && Reservation.StartDate == x.StartDate && Reservation.EndDate == x.EndDate && Reservation.GroupSize == x.GroupSize && timeline_x.ToString() == Reservation.TimeLine.t.ToString())
            {
                Assert.Pass("Inserted Consumption found in the list.");
            }
        }

        Assert.Fail("Inserted Consumption not found in the list.");
    }

    [Test]
    public void RemoveTest(){
        TimeLine.Holder Timeline = new TimeLine.Holder();
        var x = new Reservation(
            Id: 1,
            RoomId: 1,
            UserId: 1,
            GroupSize: 1,
            StartDate: DateTime.Parse("2024-04-24 02:00:00"),
            EndDate: DateTime.Parse("2024-04-27 02:00:00"),
            Price: 100.50,
            TimeLine: Timeline
        );
        _Conn.Open();
        string NewQuery = @"INSERT INTO Reservations(RoomId, UserId, GroupSize, StartDate, EndDate, Price, TimeLine)
        VALUES (@RoomId, @UserId, @GroupSize, @StartDate, @EndDate, @Price, @TimeLine)";
        int rowsAffected;
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@RoomId", x.RoomId);
            Launch.Parameters.AddWithValue("@UserId", x.UserId);
            Launch.Parameters.AddWithValue("@GroupSize", x.GroupSize);
            Launch.Parameters.AddWithValue("@StartDate", x.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@EndDate", x.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@Price", x.Price);
            Launch.Parameters.AddWithValue("@TimeLine", x.TimeLine.ToString());
            rowsAffected = Launch.ExecuteNonQuery();
        }
        _Conn.Close();
        bool removeResult = r.DeleteReservation(x);

            bool isDatabaseEmpty;
            using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM Reservations", _Conn))
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
                Assert.Pass("Consumption successfully removed and database is empty.");
            }
            else
            {
                Assert.Pass("Consumption successfully removed.");
            }
        }
        else
        {
            Assert.Fail("Failed to remove Consumption.");
        }
    }

    [Test]
    public void ReadTest(){
        TimeLine.Holder Timeline = new TimeLine.Holder();
        var x = new Reservation(
            Id: 1,
            RoomId: 1,
            UserId: 1,
            GroupSize: 1,
            StartDate: DateTime.Parse("2024-04-24 02:00:00"),
            EndDate: DateTime.Parse("2024-04-27 02:00:00"),
            Price: 100.50,
            TimeLine: Timeline
        );
        _Conn.Open();
        string NewQuery = @"INSERT INTO Reservations(RoomId, UserId, GroupSize, StartDate, EndDate, Price, TimeLine)
        VALUES (@RoomId, @UserId, @GroupSize, @StartDate, @EndDate, @Price, @TimeLine)";
        int rowsAffected;
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@RoomId", x.RoomId);
            Launch.Parameters.AddWithValue("@UserId", x.UserId);
            Launch.Parameters.AddWithValue("@GroupSize", x.GroupSize);
            Launch.Parameters.AddWithValue("@StartDate", x.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@EndDate", x.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Launch.Parameters.AddWithValue("@Price", x.Price);
            Launch.Parameters.AddWithValue("@TimeLine", x.TimeLine.ToString());
            rowsAffected = Launch.ExecuteNonQuery();
        }
        _Conn.Close();
        List<Reservation> readResult = r.ReadReservations();
        foreach (var Reservation in readResult)
        {
            List<TimeLine.Item> timeline_x = JsonConvert.DeserializeObject<List<TimeLine.Item>>(x.TimeLine.ToString());
            if (Reservation.Id == x.Id && Reservation.Price == x.Price && Reservation.RoomId == x.RoomId && Reservation.UserId == x.UserId && Reservation.StartDate == x.StartDate && Reservation.EndDate == x.EndDate && Reservation.GroupSize == x.GroupSize && timeline_x.ToString() == Reservation.TimeLine.t.ToString())
            {
                Assert.Pass("Inserted Consumption found in the list.");
            }
        }

        Assert.Fail("Inserted Consumption not found in the list.");
    }

    [Test]
    public void TimeLineTest(){
        // sorry but this test assumes the insert methods in ReservationAccess.cs work
        TimeLine.Holder Timeline = new TimeLine.Holder();
        Timeline.Add(
            (object)(new Break(15)),
            DateTime.Parse("2024-04-24 02:00:00"),
            DateTime.Parse("2024-04-24 02:00:00")
        );
        Timeline.Add(
            (object)(new Film("Test Film #1", "Genre #1", 200)),
            DateTime.Parse("2024-04-24 02:00:00"),
            DateTime.Parse("2024-04-24 02:00:00")
        );
        Timeline.Add(
            (object)(new Episode("Test Episode #1", 45)),
            DateTime.Parse("2024-04-24 02:00:00"),
            DateTime.Parse("2024-04-24 02:00:00")
        );
        Timeline.Add(
            (object)(new Consumption("Test Consumption #1", 40.20, TimeOnly.MinValue, TimeOnly.MinValue)),
            DateTime.Parse("2024-04-24 02:00:00"),
            DateTime.Parse("2024-04-24 02:00:00")
        );
        var x = new Reservation(
            Id: 1,
            RoomId: 1,
            UserId: 2,
            GroupSize: 10,
            StartDate: DateTime.Parse("2024-04-24 02:00:00"),
            EndDate: DateTime.Parse("2024-04-27 02:00:00"),
            Price: 99.99,
            TimeLine: Timeline
        );
        r.CreateReservation(x);

        // tests start here
        List<Reservation> a = r.ReadReservationsUserId(2);
        if(a.Count == 1){
            Assert.Pass("Reservation exists in list");
            Assert.AreEqual(TestTimeLine(a[0].TimeLine), 4);
        }

        List<Reservation> b = r.ReadReservationsWeek(DateTime.Parse("2024-04-23 00:00:00"));
        if(b.Count == 1){
            Assert.Pass("Reservation exists in list");
            Assert.AreEqual(TestTimeLine(b[0].TimeLine), 4);
        }

        List<Reservation> c = r.ReadReservationsDate(DateTime.Parse("2024-04-23 00:00:00"));
        if(c.Count == 1){
            Assert.Pass("Reservation exists in list");
            Assert.AreEqual(TestTimeLine(c[0].TimeLine), 4);
        }

        List<Reservation> d = r.ReadReservations();
        if(d.Count == 1){
            Assert.Pass("Reservation exists in list");
            Assert.AreEqual(TestTimeLine(d[0].TimeLine), 4);
        }
    }

    private int TestTimeLine(TimeLine.Holder t){
        int u = 0;
        List<TimeLine.Item> timelineItems = t.t;
        if(timelineItems.Count == 4){
            Assert.Pass("Reservation timeline holds all the objects in the list");
            if(timelineItems[0].Action is Break && (Break)(timelineItems[0].Action) != null){
                u++;
            }
            if(timelineItems[1].Action is Film && (Film)(timelineItems[1].Action) != null){
                u++;
            }
            if(timelineItems[2].Action is Episode && (Episode)(timelineItems[2].Action) != null){
                u++;
            }
            if(timelineItems[3].Action is Consumption && (Consumption)(timelineItems[3].Action) != null){
                u++;
            }
        }
        return u;
    }
}