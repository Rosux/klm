[TestFixture]
public class SearchTest
{
    // TODO fix
    // these retarded tests dont work at all fuck nunit worthless nunit devs

    // SearchAccess? sa = null;

    // [TearDown]
    // public void Cleanup()
    // {
    //     GC.Collect();
    //     GC.WaitForPendingFinalizers();
    //     if(File.Exists("./DataSource/Series.json")){
    //         File.SetAttributes("./DataSource/Series.json", FileAttributes.Normal);
    //         File.Delete("./DataSource/Series.json");
    //     }
    //     if(File.Exists("./DataSource/Films.json")){
    //         File.SetAttributes("./DataSource/Films.json", FileAttributes.Normal);
    //         File.Delete("./DataSource/Films.json");
    //     }
    //     Directory.Delete("./DataSource");
    // }

    // [SetUp]
    // public void Setup(){
    //     System.IO.Directory.CreateDirectory("./DataSource/");
    //     File.Create("./DataSource/Series.json");
    //     File.Create("./DataSource/Films.json");
    //     using (StreamWriter sf = new StreamWriter("./DataSource/Series.json"))
    //     {
    //         sf.Write(@"[{""Title"":""The boys"",""Genre"":""abcs"",""Seasons"":[{""Title"":""Season 1"",""Episodes"":[{""Title"":""The name of the game"",""Length"":50,""Id"":1},{""Title"":""Cherry"",""Length"":50,""Id"":2},{""Title"":""Get some"",""Length"":50,""Id"":3}],""Id"":1},{""Title"":""Season 2"",""Episodes"":[{""Title"":""The big ride"",""Length"":50,""Id"":1},{""Title"":""Proper preperation and planning"",""Length"":50,""Id"":2},{""Title"":""We gotta go now"",""Length"":50,""Id"":3}],""Id"":2}],""Id"":0}]""");
    //         sf.Close();
    //     }
    //     using (StreamWriter ff = new StreamWriter("./DataSource/Films.json"))
    //     {
    //         ff.Write(@"[{""Title"":""Beekeeper"",""Genre"":""Action"",""Id"":0,""Duration"":105},{""Title"":""Mudrunner"",""Genre"":""Adventure"",""Id"":1,""Duration"":180}]""");
    //         ff.Close();
    //     }
    //     sa = new SearchAccess();
    // }

    // [Test]
    // public void SearchTitleTest()
    // {
    //     List<Media> results = sa.Search("mudrunner");
    //     List<Media> results2 = sa.Search("mudRUnNeR");
    //     List<Media> results3 = sa.Search("mud, bee");
    //     List<Media> results4 = sa.Search("mud , bee");
    //     List<Media> results5 = sa.Search("Action");
    //     List<Media> results6 = sa.Search("abcs");
    //     Assert.AreEqual(results.Count, 1, "Returns only 1 in case of perfect match");
    //     Assert.AreEqual(results2.Count, 1, "Returns only 1 in case of weird case perfect match");
    //     Assert.AreEqual(results3.Count, 2, "'mud, bee' Returns 2 cases");
    //     Assert.AreEqual(results4.Count, 2, "'mud , bee' Returns 2 cases");
    //     Assert.AreEqual(results5.Count, 1, "Can search by genre");
    //     Assert.AreEqual(results6.Count, 1, "Can search by genre");
    // }
}