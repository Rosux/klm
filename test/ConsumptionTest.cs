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
        if(File.Exists("./DataSource/TEST.db")){
            File.Delete("./DataSource/TEST.db");
        }
        Directory.Delete("./DataSource");
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
            if (AreEqual(consumption, x))
            {
                Assert.Pass("Inserted Consumption found in the list.");
            }
        }

        Assert.Fail("Inserted Consumption not found in the list.");
    }
    
    private bool AreEqual(Consumption consumption1, Consumption consumption2)
    {
        return consumption1.Id == consumption2.Id &&
               consumption1.Name == consumption2.Name &&
               consumption1.Price == consumption2.Price &&
               consumption1.StartTime == consumption2.StartTime &&
               consumption1.EndTime == consumption2.EndTime;
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

    //     bool removeResult = c.DeleteConsumption(x);
    //     if (removeResult)
    //     {
    //         Assert.Pass("Consumption successfully removed.");
    //     }
    //     else
    //     {
    //         Assert.Fail("Failed to remove Consumption.");
    //     }
    // }
}