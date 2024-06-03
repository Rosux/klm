using Newtonsoft.Json;
public class Film : Media, IComparable
{
    public int Id { get; set; }
    private List<string> _genres;

    [JsonProperty("Genres")]
    [JsonConverter(typeof(MovieConverter<string>))]
     // ^^MovieConverter.cs --> Newtonsoft.JsonConverter overload to 
     //accept both string and list of string because 
     //newtonsoft writes a list with a single entry as a string, not a list
    public List<string> Genres // list of genres
    {
        get => _genres ?? new List<string>();
        set => _genres = value;
    }

    [JsonProperty("Original_language")]
    public string OriginalLanguage { get; set; } // original language of movies

    public string Overview { get; set; } // string with plot of movie

    [JsonProperty("Release_date")]
    public string ReleaseDate { get; set; }


    public int Runtime { get; set; } // length of movie in minutes

    public string Title { get; set; } // Movie Title

    [JsonProperty("Vote_average")]
    public double VoteAverage { get; set; } // Average rating out of ten

    public string Certification { get; set; } // age certification like PG-13, R rated ETC.

    public List<string> Directors { get; set; } // list of dicts containing ID and name of directors

    public Film(int id, List<string> genres, string original_language, string overview, string release_date, int runtime, string title, double voteaverage, string certification, List<string> directors)
    {
        Id = id;
        Genres = genres;
        OriginalLanguage = original_language;
        Overview = overview;
        ReleaseDate = release_date;
        Runtime = runtime;
        Title = title;
        VoteAverage = voteaverage;
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
        double objRating = (obj is Film) ? ((Film)obj).VoteAverage : ((Serie)obj).Rating;
        return Rating.CompareTo(objRating);
    }


}
