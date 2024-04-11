[TestFixture]
public class ConsumptionTest
{   
    private static ConsumptionAccess? c = new ConsumptionAccess("./TEST1.db");
    // [SetUp] 
    // public void Setup()
    // {   
    //     c.CloseAllConnections();
    //     File.Delete("./TEST1.db");
    // }

    // [TearDown] 
    // public void Cleanup()
    // {
    //     c.CloseAllConnections();    
    //     File.Delete("./TEST1.db");  
    // }

    [Test]
    public void InsertionTest()
    {
        var x = new Consumption(
            Id :1,
            Name: "Testname",
            Price: 2.50,
            StartTime: new TimeOnly(11,20),
            EndTime: new TimeOnly(12,20) 
        );
        bool insertresult = c.CreateConsumption(x);
        c.CloseAllConnections();
        // Assert.IsTrue(insertresult);
        // List<Consumption> inserteddata = c.ReadConsumption();
        // Assert.IsTrue(inserteddata.Any(testdata => testdata.Id == x.Id));
    }
        
    [Test]
    public void RemoveTest()
    {

    }
}