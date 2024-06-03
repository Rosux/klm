/// <summary>
/// Model class for Episode.
/// </summary>
public class Episode
{
    public int Id = -1;
    public int EpisodeNumber = 0;
    public string EpisodeTitle = "";
    public int Runtime = 0;
    public List<string> Actors = new List<string>();

    /// <summary>
    /// Creates a new episode for the season.
    /// </summary>
    /// <param name="id">The id of the episode.</param>
    /// <param name="episodeNumber">The episodenumber of the episode.</param>
    /// <param name="episodeTitle">The episodetitle of the episode.</param>
    /// <param name="runtime">The runtime of the episode.</param>
    /// <param name="actors">The actors in the episode.</param>
    public Episode(int id, int episodeNumber, string episodeTitle, int runtime, List<string> actors){
        this.Id = id;
        this.EpisodeNumber = episodeNumber;
        this.EpisodeTitle = episodeTitle;
        this.Runtime = runtime;
        this.Actors = actors;
    }

    /// <summary>
    /// Creates a new episode for the season.
    /// </summary>
    /// <param name="episodeNumber">The episodenumber of the episode.</param>
    /// <param name="episodeTitle">The episodetitle of the episode.</param>
    /// <param name="runtime">The runtime of the episode.</param>
    /// <param name="actors">The actors in the episode.</param>
    public Episode(int episodeNumber, string episodeTitle, int runtime, List<string> actors){
        this.EpisodeNumber = episodeNumber;
        this.EpisodeTitle = episodeTitle;
        this.Runtime = runtime;
        this.Actors = actors;
    }
}