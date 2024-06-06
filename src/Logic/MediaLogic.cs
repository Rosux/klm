public class MediaLogic
{
    public static void Media(){
        MenuHelper.Table<Media>(
            MediaAccess.GetAllMedia(),
            new Dictionary<string, Func<Media, object>>(){
                {"Type", m=>(m is Film) ? "Film" : "Serie"},
                {"Title", m=>m.Title},
                {"Runtime (Min)", m=>m.Runtime},
                {"Description", m=>m.Description},
                {"Rating", m=>m.Rating},
                {"Language", m=>m.Language},
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
            Film? film = MediaMenu.CreateFilm();
            if(film == null){
                return null;
            }
            bool saved = MediaAccess.AddMedia(film);
            if(!saved){
                return null;
            }
            return film;
        }
        else if(userSelection == "SERIE")
        {
            Serie? serie = MediaMenu.CreateSerie();
            if(serie == null){
                return null;
            }
            bool saved = MediaAccess.AddMedia(serie);
            if(!saved){
                return null;
            }
            return serie;
        }
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