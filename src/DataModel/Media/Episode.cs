using Newtonsoft.Json;

/// <summary>
/// Model class for Episode.
/// </summary>
public class Episode
{
    public int Id = -1;
    public string Title = "";
    public int Runtime = 0;
    public float Rating = 0.0f;
    public int EpisodeNumber = 0;
    public List<string> Actors = new List<string>();

    /// <summary>
    /// Creates a new episode for the season.
    /// </summary>
    /// <param name="id">The id of the episode.</param>
    /// <param name="title">The episodetitle of the episode.</param>
    /// <param name="runtime">The runtime of the episode.</param>
    /// <param name="episodeNumber">The episodenumber of the episode.</param>
    /// /// <param name="rating">The rating of the episode.</param>
    /// <param name="actors">The actors in the episode.</param>
    [JsonConstructor]
    public Episode(int id, string title, int runtime, int episodeNumber, float rating, List<string> actors){
        this.Id = id;
        this.Title = title;
        this.Runtime = runtime;
        this.EpisodeNumber = episodeNumber;
        this.Rating = rating;
        this.Actors = actors;
    }

    /// <summary>
    /// Creates a new episode for the season.
    /// </summary>
    /// <param name="title">The episodetitle of the episode.</param>
    /// <param name="runtime">The runtime of the episode.</param>
    /// <param name="episodeNumber">The episodenumber of the episode.</param>
    /// <param name="rating">The rating of the episode.</param>
    /// <param name="actors">The actors in the episode.</param>
    public Episode(string title, int runtime, int episodeNumber, float rating, List<string> actors){
        this.Title = title;
        this.Runtime = runtime;
        this.EpisodeNumber = episodeNumber;
        this.Rating = rating;
        this.Actors = actors;
    }
}