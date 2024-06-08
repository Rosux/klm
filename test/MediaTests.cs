[TestFixture]
public class MediaTest
{
    [TearDown]
    public void Cleanup()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if (File.Exists("./DataSource/Media.json"))
        {
            File.SetAttributes("./DataSource/Media.json", FileAttributes.Normal);
            File.Delete("./DataSource/Media.json");
        }
        Directory.Delete("./DataSource");
    }

    [SetUp]
    public void Setup()
    {
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./DataSource/Media.json");

        Directory.CreateDirectory("./DataSource/");
        using (File.Create("./DataSource/Media.json")) { }

        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[],""Series"":[]}");
        }
    }

    [Test]
    public void InsertMovie()
    {
        Film newMovie = new Film("New Film", 150, "A new film description", 8.5f, "English", new List<Genre> { Genre.ACTION }, DateOnly.Parse("2024-12-25"), Certification.PG, new List<string> { "Director1" }, new List<string> { "Actor1" }, new List<string> { "Writer1" });

        bool result = MediaAccess.AddMedia(newMovie);

        Assert.IsTrue(result);
        string jsonContent = File.ReadAllText("./DataSource/Media.json");
        MediaJsonStruct mediaStructure = JsonConvert.DeserializeObject<MediaJsonStruct>(jsonContent);

        Film insertedFilm = mediaStructure.Films.FirstOrDefault(f => f.Title == "New Film");

        Assert.IsNotNull(insertedFilm);
        Assert.AreEqual("New Film", insertedFilm.Title);
    }

    [Test]
    public void InsertSerie()
    {
        Serie newSerie = new Serie("New Serie", 150, "A new film description", 8.5f, "English", new List<Genre> { Genre.ACTION }, DateOnly.Parse("2024-12-25"), Certification.PG, new List<string> { "Director1" }, false, new List<Season>());

        bool result = MediaAccess.AddMedia(newSerie);

        Assert.IsTrue(result);
        string jsonContent = File.ReadAllText("./DataSource/Media.json");
        MediaJsonStruct mediaStructure = JsonConvert.DeserializeObject<MediaJsonStruct>(jsonContent);

        Serie insertedSerie = mediaStructure.Series.FirstOrDefault(s => s.Title == "New Serie");

        Assert.IsNotNull(insertedSerie);
        Assert.AreEqual("New Serie", insertedSerie.Title);
    }

    [Test]
    public void GetAllMedia()
    {
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./DataSource/Media.json");
        Directory.CreateDirectory("./DataSource/");
        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[{""Id"":1,""Title"":""New Film"",""Runtime"":150,""Description"":""A new film description"",""Rating"":8.5,""Language"":""English"",""Genres"":[1],""ReleaseDate"":""2024-12-25"",""Certification"":1,""Directors"":[""Director1""],""Actors"":[""Actor1""],""Writers"":[""Writer1""]}],""Series"":[{""Id"":1,""Title"":""New Series"",""Runtime"":240,""Description"":""A new series description"",""Rating"":7.8,""Language"":""English"",""Genres"":[2, 3],""ReleaseDate"":""2024-06-15"",""Certification"":2,""Directors"":[""Director2""]}]}");
        }

        List<Media> allMedia = MediaAccess.GetAllMedia();

        Assert.IsNotNull(allMedia);
        Assert.AreEqual(2, allMedia.Count);

        Film film = allMedia.Find(m => m is Film) as Film;
        Assert.IsNotNull(film);
        Assert.AreEqual("New Film", film.Title);

        Serie serie = allMedia.Find(m => m is Serie) as Serie;
        Assert.IsNotNull(serie);
        Assert.AreEqual("New Series", serie.Title);
    }

    [Test]
    public void GetMovie()
    {
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./DataSource/Media.json");
        Directory.CreateDirectory("./DataSource/");
        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[{""Id"":1,""Title"":""New Film"",""Runtime"":150,""Description"":""A new film description"",""Rating"":8.5,""Language"":""English"",""Genres"":[1],""ReleaseDate"":""2024-12-25"",""Certification"":1,""Directors"":[""Director1""],""Actors"":[""Actor1""],""Writers"":[""Writer1""]}],""Series"":[]}");
        }
        Film result = MediaAccess.GetAllFilms().Find(f => f.Id == 1);
        Assert.IsNotNull(result);
        Assert.AreEqual("New Film", result.Title);
    }

    [Test]
    public void GetSerie()
    {
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./DataSource/Media.json");
        Directory.CreateDirectory("./DataSource/");
        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[],""Series"":[{""Id"":1,""Title"":""New Series"",""Runtime"":240,""Description"":""A new series description"",""Rating"":7.8,""Language"":""English"",""Genres"":[2, 3],""ReleaseDate"":""2024-06-15"",""Certification"":2,""Directors"":[""Director2""]}]}");
        }
        Serie result = MediaAccess.GetAllSeries().Find(s => s.Id == 1);
        Assert.IsNotNull(result);
        Assert.AreEqual("New Series", result.Title);
    }

    [Test]
    public void EditMovie()
    {
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./DataSource/Media.json");

        Directory.CreateDirectory("./DataSource/");

        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[{""Id"":1,""Title"":""New Film"",""Runtime"":150,""Description"":""A new film description"",""Rating"":8.5,""Language"":""English"",""Genres"":[1],""ReleaseDate"":""2024-12-25"",""Certification"":1,""Directors"":[""Director1""],""Actors"":[""Actor1""],""Writers"":[""Writer1""]}],""Series"":[]}");
        }

        Film EditedMovie = new Film("Test", 120, "Edited description", 9.5f, "Dutch", new List<Genre> { Genre.ACTION }, DateOnly.Parse("2024-06-08"), Certification.PG, new List<string> { "Edited Director" }, new List<string> { "Edited Actor" }, new List<string> { "Edited Writer" }) { Id = 1 };

        bool result = MediaAccess.EditMedia(EditedMovie);

        Assert.IsTrue(result);
        string jsonContent = File.ReadAllText("./DataSource/Media.json");
        MediaJsonStruct mediaStructure = JsonConvert.DeserializeObject<MediaJsonStruct>(jsonContent);

        Film editedFilm = mediaStructure.Films.FirstOrDefault(f => f.Id == 1);

        Assert.IsNotNull(editedFilm);
        Assert.AreEqual("Edited description", editedFilm.Description);
    }

    [Test]
    public void EditSerie()
    {
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./DataSource/Media.json");

        Directory.CreateDirectory("./DataSource/");

        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[],""Series"":[{""Id"":1,""Title"":""New Series"",""Runtime"":240,""Description"":""A new series description"",""Rating"":7.8,""Language"":""English"",""Genres"":[2, 3],""ReleaseDate"":""2024-06-15"",""Certification"":2,""Directors"":[""Director2""]}]}");
        }

        Serie EditedSerie = new Serie(1, "Test", 120, "Edited description", 9.5f, "Dutch", new List<Genre> { Genre.ACTION }, DateOnly.Parse("2024-06-08"), Certification.PG, new List<string> { "Edited Director" }, false, new List<Season>());

        bool result = MediaAccess.EditMedia(EditedSerie);

        Assert.IsTrue(result);
        string jsonContent = File.ReadAllText("./DataSource/Media.json");
        MediaJsonStruct mediaStructure = JsonConvert.DeserializeObject<MediaJsonStruct>(jsonContent);

        Serie editedSerie = mediaStructure.Series.FirstOrDefault(s => s.Id == 1);

        Assert.IsNotNull(editedSerie);
        Assert.AreEqual("Edited description", editedSerie.Description);
    }

    [Test]
    public void RemoveMovie()
    {
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./DataSource/Media.json");

        Directory.CreateDirectory("./DataSource/");

        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[{""Id"":1,""Title"":""New Film"",""Runtime"":150,""Description"":""A new film description"",""Rating"":8.5,""Language"":""English"",""Genres"":[1],""ReleaseDate"":""2024-12-25"",""Certification"":1,""Directors"":[""Director1""],""Actors"":[""Actor1""],""Writers"":[""Writer1""]}],""Series"":[]}");
        }

        Film movieToDelete = new Film(1,"New Film", 150, "A new film description", 8.5f, "English", new List<Genre> { Genre.ACTION }, DateOnly.Parse("2024-12-25"), Certification.PG, new List<string> { "Director1" }, new List<string> { "Actor1" }, new List<string> { "Writer1" });

        bool result = MediaAccess.DeleteMedia(movieToDelete);

        Assert.IsTrue(result, "The movie was not removed successfully.");

        string jsonContent = File.ReadAllText("./DataSource/Media.json");
        MediaJsonStruct mediaStructure = JsonConvert.DeserializeObject<MediaJsonStruct>(jsonContent);

        Assert.AreEqual(0, mediaStructure.Films.Count, "The movie was not removed from the JSON file.");
    }

    [Test]
    public void RemoveSerie()
    {
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./DataSource/Media.json");

        Directory.CreateDirectory("./DataSource/");

        using (StreamWriter sf = new StreamWriter("./DataSource/Media.json"))
        {
            sf.Write(@"{""CurrentFilmId"":0,""CurrentSerieId"":0,""Films"":[],""Series"":[{""Id"":1,""Title"":""New Series"",""Runtime"":240,""Description"":""A new series description"",""Rating"":7.8,""Language"":""English"",""Genres"":[2, 3],""ReleaseDate"":""2024-06-15"",""Certification"":2,""Directors"":[""Director2""]}]}");
        }

        Serie serieToDelete = new Serie(1, "Test", 120, "Edited description", 9.5f, "Dutch", new List<Genre> { Genre.ACTION }, DateOnly.Parse("2024-06-08"), Certification.PG, new List<string> { "Edited Director" }, false, new List<Season>());

        bool result = MediaAccess.DeleteMedia(serieToDelete);

        Assert.IsTrue(result, "The serie was not removed successfully.");

        string jsonContent = File.ReadAllText("./DataSource/Media.json");
        MediaJsonStruct mediaStructure = JsonConvert.DeserializeObject<MediaJsonStruct>(jsonContent);

        Assert.AreEqual(0, mediaStructure.Series.Count, "The serie was not removed from the JSON file.");
    }
}
