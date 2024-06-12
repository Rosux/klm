using Newtonsoft.Json;

public struct Show
{
    public int? id;
    public string? name;
    public string? language;
    public string? summary;
    public string? premiered;
    public List<string>? genres;
    [JsonProperty("_embedded")]
    public ShowEmbed embeds;
}

public struct ShowEmbed{
    public List<ShowEpisode> episodes;
    public List<ShowSeason> seasons;
    public List<ShowCrew> crew;
    public List<ShowCast> cast;
}

public struct ShowSeason
{
    public int number;
    public string name;
}

public struct ShowEpisode
{
    public string? name; // title
    public int? season; // season number
    public int number; // EpisodeNumber
    public int? runtime; // Runtime (minutes)
    public int uwu {
        get{
            if(runtime.HasValue){
                return (int)runtime;
            }else{
                return 0;
            }
        }
        set{}
    }
    public ShowRating rating; // Rating
}

public struct ShowRating
{
    public float? average { get; set; }
}

public struct ShowCrew
{
    public string? type;
    public ShowPerson person;
}

public struct ShowCast
{
    public ShowPerson person;
}

public struct ShowPerson
{
    public string? name;
}