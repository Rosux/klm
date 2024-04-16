[TestFixture]
public class ConsumptionTest
{
    ConsumptionAccess? c = null;
    UserAccess? u = null;
    [OneTimeSetUp]
    public void Setup(){
        File.Delete("./TEST.db");
        c = new ConsumptionAccess("./TEST.db");
        u = new UserAccess("./TEST.db");
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
    public void RemoveTest(){
        var x = new Consumption(
            Id: 1,
            Name: "Testname",
            Price: 2.50,
            StartTime: new TimeOnly(11,20),
            EndTime: new TimeOnly(12,20)
        );

        bool removeResult = c.DeleteConsumption(x);
        if (removeResult)
        {
            Assert.Pass("Consumption successfully removed.");
        }
        else
        {
            Assert.Fail("Failed to remove Consumption.");
        }
    }
}