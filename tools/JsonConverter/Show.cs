using Newtonsoft.Json;

public struct Show
{
    public int id;
    public string name;
    public string language;
    public string summary;
    public string premiered;
    public List<string> genres;
    [JsonProperty("_embedded")]
    public ShowEmbed embeds;
}

public struct ShowEmbed{
    public List<ShowEpisode> episodes;
    public List<ShowSeason> seasons;
    public List<ShowCrew> crew;
    public List<ShowPerson> cast;
}

public struct ShowSeason
{
    public int number;
    public string name;
}

public struct ShowEpisode
{
    public string name; // title
    public int season; // season number
    public int number; // EpisodeNumber
    public int runtime; // Runtime (minutes)
    public ShowRating rating; // Rating
}

public struct ShowRating
{
    public float average { get; set; }
}

public struct ShowCrew
{
    public string type;
    public ShowPerson person;
}

public struct ShowPerson
{
    public string name;
}