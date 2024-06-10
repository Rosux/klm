using System.Data.SQLite;

[TestFixture]
public class ConsumptionTest
{
    protected SQLiteConnection _Conn = new SQLiteConnection();
    ConsumptionAccess? c = null;

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
        c = new ConsumptionAccess("./DataSource/TEST.db");
    }

    [Test]
    public void InsertionTest()
    {
        var x = new Consumption(
            Id: 1,
            Name: "Testname",
            Price: 2.50,
            StartTime: new TimeOnly(11,20),
            EndTime: new TimeOnly(12,20)
        );
        bool insertResult = c.CreateConsumption(x);
        Assert.IsTrue(insertResult, "Failed to insert Consumption.");

        List<Consumption> readResult = new List<Consumption>();

        _Conn.Open();
        string query = $"SELECT * FROM Consumptions WHERE Id = {x.Id}";
        using (SQLiteCommand command = new SQLiteCommand(query, _Conn))
        {
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int fetchedId = reader.GetInt32(0);
                string name = reader.GetString(1);
                double price = reader.GetDouble(2);
                TimeOnly startTime = TimeOnly.Parse(reader.GetString(3));
                TimeOnly endTime = TimeOnly.Parse(reader.GetString(4));
                readResult.Add(new Consumption(fetchedId, name, price, startTime, endTime));
            }
        }
        _Conn.Close();
            
        foreach (var consumption in readResult)
        {
            if (consumption.Id == x.Id && consumption.Name == x.Name && consumption.Price == x.Price && consumption.StartTime == x.StartTime && consumption.EndTime == x.EndTime)
            {
                Assert.Pass("Inserted Consumption found in the list.");
            }
        }

        Assert.Fail("Inserted Consumption not found in the list.");
    }
    
    [Test]
    public void RemoveTest(){
        var x = new Consumption(
            Id: 1,
            Name: "Testname",
            Price: 2.50,
            StartTime: new TimeOnly(11,20),
            EndTime: new TimeOnly(12,20)
        );
        _Conn.Open();
        string NewQuery = @"INSERT INTO Consumptions(Name, Price, StartTime, EndTime)
        VALUES (@Name, @Price, @StartTime, @EndTime)";
        int rowsAffected;
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@Name", x.Name);
            Launch.Parameters.AddWithValue("@Price", x.Price);
            Launch.Parameters.AddWithValue("@StartTime", x.StartTime.Hour.ToString("00") + ":" + x.StartTime.Minute.ToString("00"));
            Launch.Parameters.AddWithValue("@EndTime", x.EndTime.Hour.ToString("00") + ":" + x.EndTime.Minute.ToString("00"));
            rowsAffected = Launch.ExecuteNonQuery();
        }
        _Conn.Close();
        bool removeResult = c.DeleteConsumption(x, true);

            bool isDatabaseEmpty;
            using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM Consumptions", _Conn))
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
        var x = new Consumption(
            Id: 1,
            Name: "Testname",
            Price: 2.50,
            StartTime: new TimeOnly(11,20),
            EndTime: new TimeOnly(12,20)
        );
        _Conn.Open();
        string NewQuery = @"INSERT INTO Consumptions(Name, Price, StartTime, EndTime)
        VALUES (@Name, @Price, @StartTime, @EndTime)";
        int rowsAffected;
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@Name", x.Name);
            Launch.Parameters.AddWithValue("@Price", x.Price);
            Launch.Parameters.AddWithValue("@StartTime", x.StartTime.Hour.ToString("00") + ":" + x.StartTime.Minute.ToString("00"));
            Launch.Parameters.AddWithValue("@EndTime", x.EndTime.Hour.ToString("00") + ":" + x.EndTime.Minute.ToString("00"));
            rowsAffected = Launch.ExecuteNonQuery();
        }
        _Conn.Close();
        List<Consumption> readResult = c.ReadConsumption();
        foreach (var consumption in readResult)
        {
            if (consumption.Id == x.Id && consumption.Name == x.Name && consumption.Price == x.Price && consumption.StartTime == x.StartTime && consumption.EndTime == x.EndTime)
            {
                Assert.Pass("Inserted Consumption found in the list.");
            }
        }

        Assert.Fail("Inserted Consumption not found in the list.");
    }

    [Test]
    public void ExistsTest(){
        var x = new Consumption(
            Id: 1,
            Name: "BIG LONG TEST NAME",
            Price: 2.50,
            StartTime: new TimeOnly(11,20),
            EndTime: new TimeOnly(12,20)
        );
        _Conn.Open();
        string NewQuery = @"INSERT INTO Consumptions(Name, Price, StartTime, EndTime)
        VALUES (@Name, @Price, @StartTime, @EndTime)";
        int rowsAffected;
        using (SQLiteCommand Launch = new SQLiteCommand(NewQuery, _Conn))
        {
            Launch.Parameters.AddWithValue("@Name", x.Name);
            Launch.Parameters.AddWithValue("@Price", x.Price);
            Launch.Parameters.AddWithValue("@StartTime", x.StartTime.Hour.ToString("00") + ":" + x.StartTime.Minute.ToString("00"));
            Launch.Parameters.AddWithValue("@EndTime", x.EndTime.Hour.ToString("00") + ":" + x.EndTime.Minute.ToString("00"));
            rowsAffected = Launch.ExecuteNonQuery();
        }
        _Conn.Close();

        bool shouldBeTrue = c.ConsumptionExists("BIG LONG TEST NAME");
        bool shouldBefalse = c.ConsumptionExists("this item doesn't even exist");
        Assert.IsTrue(shouldBeTrue);
        Assert.IsFalse(shouldBefalse);
    }
}