[TestFixture]
public class SearchTest
{
    SearchAccess? sa = null;

    [TearDown]
    public void Cleanup()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if(File.Exists("./DataSource/Series.json")){
            File.SetAttributes("./DataSource/Series.json", FileAttributes.Normal);
            File.Delete("./DataSource/Series.json");
        }
        if(File.Exists("./DataSource/Films.json")){
            File.SetAttributes("./DataSource/Films.json", FileAttributes.Normal);
            File.Delete("./DataSource/Films.json");
        }
        if (Directory.Exists("./DataSource"))
        {
            Directory.Delete("./DataSource", true); // Recursive 
        }
    }

    [SetUp]
    public void Setup(){
        System.IO.Directory.CreateDirectory("./DataSource/");
        using (File.Create("./DataSource/Series.json")) {}
        using (File.Create("./DataSource/Films.json")) {}
        using (StreamWriter sf = new StreamWriter("./DataSource/Series.json"))
        {
            sf.Write(@"[{""Title"":""The boys"",""Genre"":""abcs"",""Seasons"":[{""Title"":""Season 1"",""Episodes"":[{""Title"":""The name of the game"",""Length"":50,""Id"":1},{""Title"":""Cherry"",""Length"":50,""Id"":2},{""Title"":""Get some"",""Length"":50,""Id"":3}],""Id"":1},{""Title"":""Season 2"",""Episodes"":[{""Title"":""The big ride"",""Length"":50,""Id"":1},{""Title"":""Proper preperation and planning"",""Length"":50,""Id"":2},{""Title"":""We gotta go now"",""Length"":50,""Id"":3}],""Id"":2}],""Id"":0}]");
            // sf.Close();
        }
        using (StreamWriter ff = new StreamWriter("./DataSource/Films.json"))
        {
            var films = new List<Film>
            {
                new Film(
                    id: 0,
                    genres: new List<string> { "Action" },
                    original_language: "English",
                    overview: "A beekeeper embarks on an adventure",
                    release_date: "2023-05-29",
                    runtime: 105,
                    title: "Beekeeper",
                    voteaverage: 7.5,
                    certification: "PG-13",
                    directors: new List<string> { "John Doe" }
                ),
                new Film(
                    id: 1,
                    genres: new List<string> { "Adventure" },
                    original_language: "English",
                    overview: "A journey through mud",
                    release_date: "2023-05-29",
                    runtime: 180,
                    title: "Mudrunner",
                    voteaverage: 8.2,
                    certification: "R",
                    directors: new List<string> { "Jane Smith" }
                )
            };

            ff.Write(JsonConvert.SerializeObject(films));
        }

        // Set the FILM_PATH environment variable
        Environment.SetEnvironmentVariable("FILM_PATH", "./DataSource/Series.json");
        Environment.SetEnvironmentVariable("SERIE_PATH", "./DataSource/Films.json");
        sa = new SearchAccess();
    }

    [Test]
    public void SearchTitleTest()
    {
        List<Media> results = sa.Search("mudrunner");
        List<Media> results2 = sa.Search("mudRUnNeR");
        List<Media> results3 = sa.Search("mud, bee");
        List<Media> results4 = sa.Search("mud , bee");
        List<Media> results44 = sa.Search("mud ,bee");
        List<Media> results444 = sa.Search("mud,bee");
        List<Media> results5 = sa.Search("Action");
        List<Media> results6 = sa.Search("abcs");
        Assert.AreEqual(results.Count, 1, "Returns only 1 in case of perfect match");
        Assert.AreEqual(results2.Count, 1, "Returns only 1 in case of weird case perfect match");
        Assert.AreEqual(results3.Count, 2, "'mud, bee' Returns 2 cases");
        Assert.AreEqual(results4.Count, 2, "'mud , bee' Returns 2 cases");
        Assert.AreEqual(results44.Count, 2, "'mud ,bee' Returns 2 cases");
        Assert.AreEqual(results444.Count, 2, "'mud,bee' Returns 2 cases");
        Assert.AreEqual(results5.Count, 1, "Can search by genre");
        Assert.AreEqual(results6.Count, 1, "Can search by genre");
    }
}