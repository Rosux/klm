using System.Data.SQLite;
using Newtonsoft.Json;

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
        // Consumption consumption1 = new Consumption();
        // TimeLine.Item timeline_item = new TimeLine.Item(consumption1, DateTime.Parse("2024-04-24 02:00:00"), DateTime.Parse("2024-04-24 02:00:00"));
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
            Console.WriteLine(Reservation.TimeLine);
            Console.WriteLine(x.TimeLine);
            if (Reservation.Id == x.Id && Reservation.Price == x.Price && Reservation.RoomId == x.RoomId && Reservation.UserId == x.UserId && Reservation.StartDate == x.StartDate && Reservation.EndDate == x.EndDate && Reservation.GroupSize == x.GroupSize)
            {
                Assert.Pass("Inserted Consumption found in the list.");
            }
        }

        Assert.Fail("Inserted Consumption not found in the list.");
    }

    // [Test]
    // public void RemoveTest(){
    //     var x = new Consumption(
    //         Id: 1,
    //         Name: "Testname",
    //         Price: 2.50,
    //         StartTime: new TimeOnly(11,20),
    //         EndTime: new TimeOnly(12,20)
    //     );
    //     _Conn.Open();
    //     string NewQuery = @"INSERT INTO Consumptions(Name, Price, StartTime, EndTime)
    //     VALUES (@Name, @Price, @StartTime, @EndTime)";
    //     int rowsAffected;
    //     using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
    //     {
    //         Launch.Parameters.AddWithValue("@Name", x.Name);
    //         Launch.Parameters.AddWithValue("@Price", x.Price);
    //         Launch.Parameters.AddWithValue("@StartTime", x.StartTime.Hour.ToString("00") + ":" + x.StartTime.Minute.ToString("00"));
    //         Launch.Parameters.AddWithValue("@EndTime", x.EndTime.Hour.ToString("00") + ":" + x.EndTime.Minute.ToString("00"));
    //         rowsAffected = Launch.ExecuteNonQuery();
    //     }
    //     _Conn.Close();
    //     bool removeResult = c.DeleteConsumption(x);

    //         bool isDatabaseEmpty;
    //         using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM Consumptions", _Conn))
    //         {
    //             _Conn.Open();
    //             int rowCount = Convert.ToInt32(checkCommand.ExecuteScalar());
    //             isDatabaseEmpty = rowCount == 0;
    //             _Conn.Close();
    //         }

    //         if (removeResult)
    //         {
    //         if (isDatabaseEmpty)
    //         {
    //             Assert.Pass("Consumption successfully removed and database is empty.");
    //         }
    //         else
    //         {
    //             Assert.Pass("Consumption successfully removed.");
    //         }
    //     }
    //     else
    //     {
    //         Assert.Fail("Failed to remove Consumption.");
    //     }
    // }

    // [Test]
    // public void ReadTest(){
    //     var x = new Consumption(
    //         Id: 1,
    //         Name: "Testname",
    //         Price: 2.50,
    //         StartTime: new TimeOnly(11,20),
    //         EndTime: new TimeOnly(12,20)
    //     );
    //     _Conn.Open();
    //     string NewQuery = @"INSERT INTO Consumptions(Name, Price, StartTime, EndTime)
    //     VALUES (@Name, @Price, @StartTime, @EndTime)";
    //     int rowsAffected;
    //     using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
    //     {
    //         Launch.Parameters.AddWithValue("@Name", x.Name);
    //         Launch.Parameters.AddWithValue("@Price", x.Price);
    //         Launch.Parameters.AddWithValue("@StartTime", x.StartTime.Hour.ToString("00") + ":" + x.StartTime.Minute.ToString("00"));
    //         Launch.Parameters.AddWithValue("@EndTime", x.EndTime.Hour.ToString("00") + ":" + x.EndTime.Minute.ToString("00"));
    //         rowsAffected = Launch.ExecuteNonQuery();
    //     }
    //     _Conn.Close();
    //     List<Consumption> readResult = c.ReadConsumption();
    //     foreach (var consumption in readResult)
    //     {
    //         if (consumption.Id == x.Id && consumption.Name == x.Name && consumption.Price == x.Price && consumption.StartTime == x.StartTime && consumption.EndTime == x.EndTime)
    //         {
    //             Assert.Pass("Inserted Consumption found in the list.");
    //         }
    //     }

    //     Assert.Fail("Inserted Consumption not found in the list.");
    // }
}