public class MediaLogic
{
    public static void Media(){
        MenuHelper.Table<Media>(
            MediaAccess.GetAllMedia(),
            new Dictionary<string, Func<Media, object>>(){
                {"Type", m=>(m is Film) ? "Film" : "Serie"},
                {"Id", m=>m.Id},
                {"Title", m=>m.Title},
                {"Language", m=>m.Language},
                {"Genres", m=>string.Join(", ", m.Genres)},
                {"Release Date", m=>m.ReleaseDate},
                {"Certification", m=>m.Certification},
            },
            false, // canSelect
            true,
            true,
            true, // canSearch
            new Dictionary<string, PropertyEditMapping<Media>>(){
                {"Title", new(x=>x.Title, (Media m)=>{return m.Title;})},
            },
            SaveMedia,
            true,
            AddMedia,
            true,
            DeleteMedia
        );
    }

    /// <summary>
    /// Asks the user to select either Film or Series and create a new Film/Serie.
    /// </summary>
    /// <returns>A Media object containing a Film/Serie or null.</returns>
    private static Media? AddMedia()
    {
        // ask user if they want to add a film or serie
        string? userSelection = MenuHelper.SelectFromList(
            "Do you want to add a Film or Serie?",
            true,
            new Dictionary<string, string>(){
                {"Film", "FILM"},
                {"Serie", "SERIE"},
            }
        );
        if(userSelection == null){return null;}
        if(userSelection == "FILM")
        {
            return CreateFilm();
        }
        else if(userSelection == "SERIE")
        {
            return CreateSerie();
        }
        return null;
    }

    /// <summary>
    /// Creates a new Film object.
    /// </summary>
    /// <returns>Returns a Film object or null in case the user exists the process.</returns>
    private static Film? CreateFilm()
    {
        return null;
    }


    /// <summary>
    /// Creates a new Serie object.
    /// </summary>
    /// <returns>Returns a Serie object or null in case the user exists the process.</returns>
    private static Serie? CreateSerie()
    {
        return null;
    }

    /// <summary>
    /// Saves the given media and returns a boolean indicating if the media got saved.
    /// </summary>
    /// <param name="media">The Media object to save.</param>
    /// <returns>Returns a boolean indicating if the Media got saved.</returns>
    private static bool SaveMedia(Media media)
    {
        return false;
    }

    /// <summary>
    /// Deletes the given media and returns a boolean indicating if the media got deleted.
    /// </summary>
    /// <param name="media">The Media object to delete.</param>
    /// <returns>Returns a boolean indicating if the Media got deleted.</returns>
    private static bool DeleteMedia(Media media)
    {
        return false;
    }
}