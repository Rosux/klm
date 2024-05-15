using Newtonsoft.Json;
public class Film : Media, IComparable
{
    [JsonProperty("Id")]
    public int Id { get; set; }
    private List<string> _genres;

    [JsonProperty("Genres")]
    [JsonConverter(typeof(MovieConverter<string>))]
    public List<string> Genres
    {
        get => _genres ?? new List<string>();
        set => _genres = value;
    }
    [JsonProperty("Original_language")]
    public string Original_language { get; set; } // original language of movies

    [JsonProperty("Overview")]
    public string Overview { get; set; } // string with plot of movie

    [JsonProperty("Release_date")]
    public string Release_date { get; set; }

    [JsonProperty("Runtime")]
    public int Runtime { get; set; } // length of movie in minutes

    [JsonProperty("Title")]
    public string Title { get; set; } // Movie Title

    [JsonProperty("Vote_average")]
    public double Vote_average { get; set; } // Average rating out of ten

    [JsonProperty("Certification")]
    public string Certification { get; set; } // age certification like PG-13, R rated ETC.

    [JsonProperty("Directors")]
    public List<Dictionary<string, string>> Directors { get; set; } // list of dicts containing ID and name of directors

    public Film(int id, List<string> genres, string original_language, string overview, string release_date, int runtime, string title, double voteaverage, string certification, List<Dictionary<string, string>> directors)
    {
        Id = id;
        Genres = genres;
        Original_language = original_language;
        Overview = overview;
        Release_date = release_date;
        Runtime = runtime;
        Title = title;
        Vote_average = voteaverage;
        if(certification != null || certification == "Null")
        {
           Certification = certification; 
        }
        else
        {
            Certification = "No Age rating";
        }
        Directors = directors;
    }

    // public Film(int id, string title, string genre, int duration)
    // {
    //     Id = id;
    //     Genre = genre;
    //     Title = title;
    //     Duration = duration;
    // }

    public int CompareTo(object obj)
    {
        if (!(obj is Film) && !(obj is Serie))
        {
            throw new ArgumentException("Object is not a Film or Serie");
        }
        double objRating = (obj is Film) ? ((Film)obj).Vote_average : ((Serie)obj).Rating;
        return Rating.CompareTo(objRating);
    }


}
