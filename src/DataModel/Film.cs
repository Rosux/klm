/// <summary>
/// Model class for a film.
/// </summary>
public class Film : Media
{
    public List<string> Actors = new List<string>();
    public List<string> Writers = new List<string>();

    /// <summary>
    /// Creates a new film with the specified data.
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
    /// <param name="actors">A list of actors.</param>
    /// <param name="writers">A list of writers.</param>
    /// <returns>The created film.</returns>
    public Film(int id, string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors, List<string> actors, List<string> writers) : base(id, title, runtime, description, rating, language, genres, releaseDate, certification, directors)
    {
        this.Actors = actors;
        this.Writers = writers;
    }

    /// <summary>
    /// Creates a new film with the specified data.
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
    /// <param name="actors">A list of actors.</param>
    /// <param name="writers">A list of writers.</param>
    /// <returns>The created film.</returns>
    public Film(string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors, List<string> actors, List<string> writers) : base(-1, title, runtime, description, rating, language, genres, releaseDate, certification, directors)
    {
        this.Actors = actors;
        this.Writers = writers;
    }
}
