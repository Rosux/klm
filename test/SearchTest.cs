[TestFixture]
public class SearchTest
{
    SearchAccess? sa = null;

    [TearDown]
    public void Cleanup()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if(File.Exists("./DataSource/Media.json")){
            File.SetAttributes("./DataSource/Media.json", FileAttributes.Normal);
            File.Delete("./DataSource/Media.json");
        }
        Directory.Delete("./DataSource");
    }

    [SetUp]
    public void Setup(){
        System.IO.Directory.CreateDirectory("./DataSource/");
        using (File.Create("./DataSource/Media.json")) {}
        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[{""Id"":1,""Title"":""New Film"",""Runtime"":150,""Description"":""A new film description"",""Rating"":8.5,""Language"":""English"",""Genres"":[1],""ReleaseDate"":""2024-12-25"",""Certification"":1,""Directors"":[""Director1""],""Actors"":[""Actor1""],""Writers"":[""Writer1""]}],""Series"":[{""Id"":1,""Title"":""New Series"",""Runtime"":240,""Description"":""A new series description"",""Rating"":7.8,""Language"":""English"",""Genres"":[2, 3],""ReleaseDate"":""2024-06-15"",""Certification"":2,""Directors"":[""Director2""]}]}");
        }
        sa = new SearchAccess();
    }

    [Test]
    public void SearchTitleTest()
    {
        List<Media> results = sa.Search("New Film");
        List<Media> results2 = sa.Search("NEW FiLm");
        List<Media> results3 = sa.Search("Film, Serie");
        List<Media> results4 = sa.Search("Film , Serie");
        List<Media> results44 = sa.Search("Film ,Serie");
        List<Media> results444 = sa.Search("Film,Serie");
        List<Media> results5 = sa.Search("Action");
        List<Media> results6 = sa.Search("Horror, Action");
        Assert.AreEqual(results.Count, 1, "Returns only 1 in case of perfect match");
        Assert.AreEqual(results2.Count, 1, "Returns only 1 in case of weird case perfect match");
        Assert.AreEqual(results3.Count, 2, "'Film, Serie' Returns 2 cases");
        Assert.AreEqual(results4.Count, 2, "'Film , Serie' Returns 2 cases");
        Assert.AreEqual(results44.Count, 2, "'Film ,Serie' Returns 2 cases");
        Assert.AreEqual(results444.Count, 2, "'Film,Serie' Returns 2 cases");
        Assert.AreEqual(results5.Count, 1, "Can search by genre");
        Assert.AreEqual(results6.Count, 2, "Can search by genre");
    }
}