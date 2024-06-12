using Newtonsoft.Json;

/// <summary>
/// Model class for a serie.
/// </summary>
public class Serie : Media
{
    public int Id { get; set; } = -1;
    public string Title { get; set; } = "";
    public int Runtime
    {
        get
        {
            return Seasons?.Sum(s => s.Runtime) ?? 0;
        }
        set{}
    }

    public string Description { get; set; } = "";
    public float Rating{
        get{
            if (Seasons == null || Seasons.Count == 0)
            {
                return 0f;
            }
            return (float)Math.Round(Seasons.Average(e => e.Rating), 2);
        }
        set{}
    } // ranges from 0.0f to 10.0f
    public string Language { get; set; } = "";
    public List<Genre> Genres { get; set; } = new List<Genre>();
    public DateOnly ReleaseDate { get; set; } = DateOnly.MinValue;
    public Certification Certification { get; set; } = Certification.NONE;
    public List<string> Directors { get; set; } = new List<string>();
    public bool Bingeable {
        get{
            if (Rating >= 7.5 && Runtime >= 400){
                return true;
            }
            return false;
        }
        set{}
    }
    public List<Season> Seasons { get; set; } = new List<Season>();

    /// <summary>
    /// Creates a serie with specified data.
    /// </summary>
    /// <param name="id">The id of the media.</param>
    /// <param name="title">The title of the media.</param>
    /// <param name="runtime">How long the media runs on screen in minutes.</param>
    /// <param name="description">The description of the media.</param>
    /// <param name="rating">The rating of the media ranging from 0.0f to 10.0f</param>
    /// <param name="language">The language of the media.</param>
    /// <param name="genres">A list of genres.</param>
    /// <param name="releaseDate">The release date of the media.</param>
    /// <param name="certification">The certification of the media.</param>
    /// <param name="directors">A list of directors.</param>
    /// <param name="bingeable"></param>
    /// <param name="seasons"></param>
    /// <returns>The created serie.</returns>
    [JsonConstructor]
    public Serie(int id, string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors, bool bingeable, List<Season> seasons)
    {
        this.Id = id;
        this.Title = title;
        this.Runtime = runtime;
        this.Description = description;
        this.Rating = rating;
        this.Language = language;
        this.Genres = genres;
        this.ReleaseDate = releaseDate;
        this.Certification = certification;
        this.Directors = directors;
        this.Bingeable = bingeable;
        this.Seasons = seasons;
    }

    /// <summary>
    /// Creates a serie with specified data.
    /// </summary>
    /// <param name="title">The title of the media.</param>
    /// <param name="runtime">How long the media runs on screen in minutes.</param>
    /// <param name="description">The description of the media.</param>
    /// <param name="rating">The rating of the media ranging from 0.0f to 10.0f</param>
    /// <param name="language">The language of the media.</param>
    /// <param name="genres">A list of genres.</param>
    /// <param name="releaseDate">The release date of the media.</param>
    /// <param name="certification">The certification of the media.</param>
    /// <param name="directors">A list of directors.</param>
    /// <param name="bingeable"></param>
    /// <param name="seasons"></param>
    /// <returns>The created serie.</returns>
    public Serie(string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors, bool bingeable, List<Season> seasons)
    {
        this.Id = -1;
        this.Title = title;
        this.Runtime = runtime;
        this.Description = description;
        this.Rating = rating;
        this.Language = language;
        this.Genres = genres;
        this.ReleaseDate = releaseDate;
        this.Certification = certification;
        this.Directors = directors;
        this.Bingeable = bingeable;
        this.Seasons = seasons;
    }
}