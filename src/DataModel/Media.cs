/// <summary>
/// Base class for Film and Serie.
/// </summary>
public abstract class Media
{
    public int Id = -1;
    public string Title = "";
    public int Runtime = 0;
    public string Description = "";
    public float Rating = 0.0f; // ranges from 0.0f to 10.0f
    public string Language = "";
    public List<Genre> Genres = new List<Genre>();
    public DateOnly ReleaseDate = DateOnly.MinValue;
    public Certification Certification = Certification.NONE;
    public List<string> Directors = new List<string>();

    /// <summary>
    /// Sets the common media attributes.
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
    public Media(int id, string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors)
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
    }

    /// <summary>
    /// Sets the common media attributes.
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
    public Media(string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors)
    {
        this.Title = title;
        this.Runtime = runtime;
        this.Description = description;
        this.Rating = rating;
        this.Language = language;
        this.Genres = genres;
        this.ReleaseDate = releaseDate;
        this.Certification = certification;
        this.Directors = directors;
    }
}