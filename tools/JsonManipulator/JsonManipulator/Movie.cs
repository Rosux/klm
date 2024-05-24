using Newtonsoft.Json;
public class Movie
{
    public int Id { get; set; }
    private List<string> _genres;

    [JsonProperty("genres")]
    [JsonConverter(typeof(MovieConverter<string>))]
    public List<string> Genres
    {
        get => _genres ?? new List<string>();
        set => _genres = value;
    }
    public string Original_language { get; set; } // original language of movies

    public string Overview { get; set; } // string with plot of movie

    public string Release_date { get; set; }

    public int Runtime { get; set; } // length of movie in minutes

    public string Title { get; set; } // Movie Title

    public double Vote_average { get; set; } // Average rating out of ten

    public string Certification { get; set; } // age certification like PG-13, R rated ETC.

    public List<string> Directors { get; set; } // list of dicts containing ID and name of directors

    public Movie(int id, List<string> genres, string original_language, string overview, string release_date, int runtime, string title, double voteaverage, string certification, List<Dictionary<string, string>> directors)
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
        var x = new List<string>();
        foreach(Dictionary<string, string> d in directors){
            foreach(KeyValuePair<string, string> kvp in d){
                if(kvp.Key == "name"){
                    string directorName = kvp.Value;
                    x.Add(directorName);
                }
            }
        }
        Directors = x;
    }

    // public Film(int id, string title, string genre, int duration)
    // {
    //     Id = id;
    //     Genre = genre;
    //     Title = title;
    //     Duration = duration;
    // }



}
