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
        Directory.Delete("./DataSource");
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
            string JSONDATA = @"[{""Title"":""Godzilla vs. Kong"",""Rating"":0,""Id"":1,""Genres"":[""Action"",""Science Fiction""],""Original_language"":""English"",""Overview"":""In a time when monsters walk the Earth, humanity’s fight for its future sets Godzilla and Kong on a collision course that will see the two most powerful forces of nature on the planet collide in a spectacular battle for the ages."",""Release_date"":""2020-05-08"",""Runtime"":113,""Vote_average"":8.4,""Certification"":""PG-13"",""Directors"":[{""id"":""98631"",""name"":""Adam Wingard""}]},{""Title"":""Zack Snyder's Justice League"",""Rating"":0,""Id"":2,""Genres"":[""Action"",""Adventure"",""Fantasy"",""Science Fiction""],""Original_language"":""English"",""Overview"":""Determined to ensure Superman's ultimate sacrifice was not in vain, Bruce Wayne aligns forces with Diana Prince with plans to recruit a team of metahumans to protect the world from an approaching threat of catastrophic proportions."",""Release_date"":""2021-03-18"",""Runtime"":242,""Vote_average"":8.6,""Certification"":""R"",""Directors"":[{""id"":""15217"",""name"":""Zack Snyder""}]},{""Title"":""Chaos Walking"",""Rating"":0,""Id"":3,""Genres"":[""Science Fiction"",""Action"",""Adventure"",""Thriller""],""Original_language"":""English"",""Overview"":""Two unlikely companions embark on a perilous adventure through the badlands of an unexplored planet as they try to escape a dangerous and disorienting reality, where all inner thoughts are seen and heard by everyone."",""Release_date"":""2021-02-24"",""Runtime"":109,""Vote_average"":7.5,""Certification"":""PG-13"",""Directors"":[{""id"":""11694"",""name"":""Doug Liman""}]},{""Title"":""Raya and the Last Dragon"",""Rating"":0,""Id"":4,""Genres"":[""Animation"",""Adventure"",""Fantasy"",""Family"",""Action""],""Original_language"":""English"",""Overview"":""Long ago, in the fantasy world of Kumandra, humans and dragons lived together in harmony. But when an evil force threatened the land, the dragons sacrificed themselves to save humanity. Now, 500 years later, that same evil has returned and it’s up to a lone warrior, Raya, to track down the legendary last dragon to restore the fractured land and its divided people."",""Release_date"":""2021-03-03"",""Runtime"":107,""Vote_average"":8.3,""Certification"":""PG"",""Directors"":[{""id"":""227439"",""name"":""Don Hall""},{""id"":""1932178"",""name"":""Carlos López Estrada""}]},{""Title"":""Tom & Jerry"",""Rating"":0,""Id"":5,""Genres"":[""Comedy"",""Family"",""Animation""],""Original_language"":""English"",""Overview"":""Tom the cat and Jerry the mouse get kicked out of their home and relocate to a fancy New York hotel, where a scrappy employee named Kayla will lose her job if she can’t evict Jerry before a high-class wedding at the hotel. Her solution? Hiring Tom to get rid of the pesky mouse."",""Release_date"":""2021-02-11"",""Runtime"":101,""Vote_average"":7.3,""Certification"":""PG"",""Directors"":[{""id"":""20400"",""name"":""Tim Story""}]},{""Title"":""Cherry"",""Rating"":0,""Id"":6,""Genres"":[""Crime"",""Drama""],""Original_language"":""English"",""Overview"":""Cherry drifts from college dropout to army medic in Iraq - anchored only by his true love, Emily. But after returning from the war with PTSD, his life spirals into drugs and crime as he struggles to find his place in the world."",""Release_date"":""2021-02-26"",""Runtime"":140,""Vote_average"":7.6,""Certification"":""R"",""Directors"":[{""id"":""19271"",""name"":""Anthony Russo""},{""id"":""19272"",""name"":""Joe Russo""}]},{""Title"":""Mortal Kombat Legends: Scorpion's Revenge"",""Rating"":0,""Id"":7,""Genres"":[""Animation"",""Action"",""Fantasy""],""Original_language"":""English"",""Overview"":""After the vicious slaughter of his family by stone-cold mercenary Sub-Zero, Hanzo Hasashi is exiled to the torturous Netherrealm. There, in exchange for his servitude to the sinister Quan Chi, he’s given a chance to avenge his family – and is resurrected as Scorpion, a lost soul bent on revenge. Back on Earthrealm, Lord Raiden gathers a team of elite warriors – Shaolin monk Liu Kang, Special Forces officer Sonya Blade and action star Johnny Cage – an unlikely band of heroes with one chance to save humanity. To do this, they must defeat Shang Tsung’s horde of Outworld gladiators and reign over the Mortal Kombat tournament."",""Release_date"":""2020-04-12"",""Runtime"":80,""Vote_average"":8.4,""Certification"":""R"",""Directors"":[{""id"":""204553"",""name"":""Ethan Spaulding""}]},{""Title"":""The Croods: A New Age"",""Rating"":0,""Id"":8,""Genres"":[""Family"",""Fantasy"",""Animation"",""Comedy""],""Original_language"":""English"",""Overview"":""Searching for a safer habitat, the prehistoric Crood family discovers an idyllic, walled-in paradise that meets all of its needs. Unfortunately, they must also learn to live with the Bettermans -- a family that's a couple of steps above the Croods on the evolutionary ladder. As tensions between the new neighbors start to rise, a new threat soon propels both clans on an epic adventure that forces them to embrace their differences, draw strength from one another, and survive together."",""Release_date"":""2020-11-25"",""Runtime"":95,""Vote_average"":7.5,""Certification"":""PG"",""Directors"":[{""id"":""1450348"",""name"":""Joel Crawford""}]},{""Title"":""Miraculous World: New York, United HeroeZ"",""Rating"":0,""Id"":9,""Genres"":[""Animation"",""Family""],""Original_language"":""French"",""Overview"":""During a school field trip, Ladybug and Cat Noir meet the American superheroes, whom they have to save from an akumatised super-villain. They discover that Miraculous exist in the United States too."",""Release_date"":""2020-09-26"",""Runtime"":55,""Vote_average"":8.3,""Certification"":""NR"",""Directors"":[{""id"":""1565301"",""name"":""Thomas Astruc""}]},{""Title"":""Soul"",""Rating"":0,""Id"":10,""Genres"":[""Family"",""Animation"",""Comedy"",""Drama"",""Music"",""Fantasy""],""Original_language"":""English"",""Overview"":""Joe Gardner is a middle school teacher with a love for jazz music. After a successful gig at the Half Note Club, he suddenly gets into an accident that separates his soul from his body and is transported to the You Seminar, a center in which souls develop and gain passions before being transported to a newborn child. Joe must enlist help from the other souls-in-training, like 22, a soul who has spent eons in the You Seminar, in order to get back to Earth."",""Release_date"":""2020-12-25"",""Runtime"":101,""Vote_average"":8.3,""Certification"":""PG"",""Directors"":[{""id"":""12890"",""name"":""Pete Docter""},{""id"":""2451598"",""name"":""Kemp Powers""}]}]";
            ff.Write(JSONDATA);
            // ff.Close();
        }
        sa = new SearchAccess();
    }

    [Test]
    public void SearchTitleTest()
    {
        List<Media> results = sa.Search("Chaos Walking");
        List<Media> results2 = sa.Search("Chaos WaLK");
        List<Media> results3 = sa.Search("Chaos, Last draGON");
        List<Media> results4 = sa.Search("Chaos , Last draGON");
        List<Media> results44 = sa.Search("Chaos ,Last draGON");
        List<Media> results444 = sa.Search("Chaos,Last draGON");
        List<Media> results5 = sa.Search("Action");
        List<Media> results6 = sa.Search("Thriller");
        List<Media> results7 = sa.Search("Zack");

        Assert.AreEqual(results.Count, 1, "Returns only 1 in case of perfect match");
        Assert.AreEqual(results2.Count, 1, "Returns only 1 in case of weird case perfect match");
        Assert.AreEqual(results3.Count, 2, "'haos, Last draGON' Returns 2 cases");
        Assert.AreEqual(results4.Count, 2, "'haos , Last draGON' Returns 2 cases");
        Assert.AreEqual(results44.Count, 2, "'haos ,Last draGON' Returns 2 cases");
        Assert.AreEqual(results444.Count, 2, "'ChaosLast draGON' Returns 2 cases");
        Assert.AreEqual(results5.Count, 5, "Can search by genre");
        Assert.AreEqual(results6.Count, 1, "Can search by genre");
        Assert.AreEqual(results7.Count, 1, "Can search by Director");
    }
}