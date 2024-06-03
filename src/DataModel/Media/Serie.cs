/// <summary>
///  Model class for a serie.
/// </summary>
public class Serie : Media
{
    public bool Bingeable = false;
    public List<Season> Seasons = new List<Season>();

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
    public Serie(int id, string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors,bool bingeable, List<Season> seasons) : base(id, title, runtime, description, rating, language, genres, releaseDate, certification, directors){
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
    public Serie(string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors,bool bingeable, List<Season> seasons) : base(title, runtime, description, rating, language, genres, releaseDate, certification, directors){
        this.Bingeable = bingeable;
        this.Seasons = seasons;
    }
}