/// <summary>
/// Model class for a season.
/// </summary>
public class Season
{
    public int Id = -1;
    public int SeasonNumber = 0;
    public string SeasonTitle = "";
    public List<Episode> Episodes = new List<Episode>();
    public int Runtime = 0;


    /// <summary>
    /// Creates a new season for the serie.
    /// </summary>
    /// <param name="id">The id of the season.</param>
    /// <param name="seasonNumber">The seasonnumber of the season.</param>
    /// <param name="seasonTitle">The seasontitle of the season.</param>
    /// <param name="episodes">The episodes in the season.</param>
    /// <param name="runtime">The runtime of the season.</param>
    public Season(int id, int seasonNumber, string seasonTitle, List<Episode> episodes, int runtime){
        this.Id = id;
        this.SeasonNumber = seasonNumber;
        this.SeasonTitle = seasonTitle;
        this.Episodes = episodes;
        this.Runtime = runtime;
    }

    /// <summary>
    /// Creates a new season for the serie.
    /// </summary>
    /// <param name="seasonNumber">The seasonnumber of the season.</param>
    /// <param name="seasonTitle">The seasontitle of the season.</param>
    /// <param name="episodes">The episodes in the season.</param>
    /// <param name="runtime">The runtime of the season.</param>
    public Season(int seasonNumber, string seasonTitle, List<Episode> episodes, int runtime){
        this.SeasonNumber = seasonNumber;
        this.SeasonTitle = seasonTitle;
        this.Episodes = episodes;
        this.Runtime = runtime;
    }
}