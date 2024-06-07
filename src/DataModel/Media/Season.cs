using Newtonsoft.Json;

/// <summary>
/// Model class for a season.
/// </summary>
public class Season
{
    public int Id = -1;
    public string Title = "";
    public int Runtime = 0;
    public int SeasonNumber = 0;
    public List<Episode> Episodes { get; set; } = new List<Episode>();


    /// <summary>
    /// Creates a new season for the serie.
    /// </summary>
    /// <param name="id">The id of the season.</param>
    /// <param name="title">The seasontitle of the season.</param>
    /// <param name="runtime">The runtime of the season.</param>
    /// <param name="seasonNumber">The seasonnumber of the season.</param>
    /// <param name="episodes">The episodes in the season.</param>
    [JsonConstructor]
    public Season(int id, string title, int runtime, int seasonNumber, List<Episode> episodes){
        this.Id = id;
        this.SeasonNumber = seasonNumber;
        this.Title = title;
        this.Episodes = episodes;
        this.Runtime = runtime;
    }

    /// <summary>
    /// Creates a new season for the serie.
    /// </summary>
    /// <param name="title">The seasontitle of the season.</param>
    /// <param name="runtime">The runtime of the season.</param>
    /// <param name="seasonNumber">The seasonnumber of the season.</param>
    /// <param name="episodes">The episodes in the season.</param>
    public Season(string title, int runtime, int seasonNumber, List<Episode> episodes){
        this.SeasonNumber = seasonNumber;
        this.Title = title;
        this.Episodes = episodes;
        this.Runtime = runtime;
    }
}