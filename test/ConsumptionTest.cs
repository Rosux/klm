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
        bool insertresult = c.CreateConsumption(x);
        Assert.IsTrue(insertresult);
    }
}