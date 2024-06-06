public class MediaLogic
{
    public static void Media(){
        // MenuHelper.Table<Media>(
        //     MediaAccess.GetAllMedia(),
        //     new Dictionary<string, Func<Media, object>>(){
        //         {"Type", m=>(m is Film) ? "Film" : "Serie"},
        //         {"Title", m=>m.Title},
        //         {"Runtime (Min)", m=>m.Runtime},
        //         {"Description", m=>m.Description},
        //         {"Rating", m=>m.Rating},
        //         {"Language", m=>m.Language},
        //         {"Release Date", m=>m.ReleaseDate},
        //         {"Certification", m=>m.Certification},
        //     },
        //     false, // canSelect
        //     true,
        //     true,
        //     true, // canSearch
        //     new Dictionary<string, PropertyEditMapping<Media>>(){
        //         {"Title", new(x=>x.Title, (Media m)=>{return m.Title;})},
        //     },
        //     SaveEditedMedia,
        //     true,
        //     AddMedia,
        //     true,
        //     DeleteMedia
        // );
    }

    // /// <summary>
    // /// Asks the user to select either Film or Series and create a new Film/Serie.
    // /// </summary>
    // /// <returns>A Media object containing a Film/Serie or null.</returns>
    // private static Media? AddMedia()
    // {
    //     // ask user if they want to add a film or serie
    //     string? userSelection = MenuHelper.SelectFromList(
    //         "Do you want to add a Film or Serie?",
    //         true,
    //         new Dictionary<string, string>(){
    //             {"Film", "FILM"},
    //             {"Serie", "SERIE"},
    //         }
    //     );
    //     if(userSelection == null){return null;}
    //     if(userSelection == "FILM")
    //     {
    //         Film? film = MediaMenu.CreateFilm();
    //         if(film == null){
    //             return null;
    //         }
    //         bool saved = MediaAccess.AddMedia(film);
    //         if(!saved){
    //             return null;
    //         }
    //         return film;
    //     }
    //     else if(userSelection == "SERIE")
    //     {
    //         Serie? serie = MediaMenu.CreateSerie();
    //         if(serie == null){
    //             return null;
    //         }
    //         bool saved = MediaAccess.AddMedia(serie);
    //         if(!saved){
    //             return null;
    //         }
    //         return serie;
    //     }
    //     return null;
    // }

    public static void SeasonsTable(){
        var allMedia = MediaAccess.GetAllMedia();
        var series = allMedia.OfType<Serie>().ToList();
        var seasons = new List<Season>();
        MenuHelper.Table<Season>(
            seasons,
            new Dictionary<string, Func<Season, object>>(){
                {"Title", m=>m.Title},
                {"Runtime", m=>m.Runtime},
                {"SeasonNumber", m=>m.SeasonNumber},
                {"Episodes", m=>m.Episodes},
            },
            false, // canSelect
            true,
            true,
            true, // canSearch
            new Dictionary<string, PropertyEditMapping<Season>>(){
                {"Title", new(x => x.Title, (Season m) => m.Title)},
                {"Runtime", new(x => x.Runtime, (Season m) => m.Runtime)},
                {"SeasonNumber", new(x => x.SeasonNumber, (Season m) => m.SeasonNumber)},
                {"Episode", new(x=>x.Episodes, (Season m) => { EpisodesTable(m); return m.Episodes; })},
            },
            SaveEditedMedia,
            true,
            MediaMenu.AddSeason,
            true,
            DeleteMedia
        );
    }

    public static void  EpisodesTable(Season season){
        var episodes = season.Episodes.ToList();
        MenuHelper.Table<Episode>(
            episodes,
            new Dictionary<string, Func<Episode, object>>(){
                {"Title", m=>m.Title},
                {"Runtime", m=>m.Runtime},
                {"EpisodeNumber", m=>m.EpisodeNumber},
                {"Actors", m=>m.Actors},
            },
            false, // canSelect
            true,
            true,
            true, // canSearch
            new Dictionary<string, PropertyEditMapping<Episode>>(){
                {"Title", new(x => x.Title, (Episode m) => m.Title)},
                {"Runtime", new(x => x.Runtime, (Episode m) => m.Runtime)},
                {"EpisodeNumber", new(x => x.EpisodeNumber, (Episode m) => m.EpisodeNumber)},
                {"Actors", new(x => x.Actors, (Episode m) => m.Actors)},
            },
            SaveEditedMedia,
            true,
            () => MediaMenu.CreateEpisode(season),
            true,
            DeleteMedia
        );
    }

    /// <summary>
    /// Saves the given media and returns a boolean indicating if the media got saved.
    /// </summary>
    /// <param name="media">The Media object to save.</param>
    /// <returns>Returns a boolean indicating if the Media got saved.</returns>
    private static bool SaveEditedMedia<T>(T media)
    {
        if(media is Season season){
            bool confirmation = MenuHelper.Confirm($"you sure you want to save the following edited data:\n\nTitle{season.Title}\nRuntime: {season.Runtime}\nSeasonNumber: {season.SeasonNumber}\nEpisodes: {season.Episodes.Count}");
            if(confirmation){
                bool success = MediaAccess.EditMedia(media);
                MediaMenu.MediaAdded(success);
                return success;
            }
        }else if(media is Episode episode){
            bool confirmation = MenuHelper.Confirm($"you sure you want to save the following edited data:\n\nTitle{episode.Title}\nRuntime: {episode.Runtime}\nSeasonNumber: {episode.EpisodeNumber}\nEpisodes: {episode.Actors}");
            if(confirmation){
                bool succes;
            }
        }
        return false;
    }

    /// <summary>
    /// Deletes the given media and returns a boolean indicating if the media got deleted.
    /// </summary>
    /// <param name="media">The Media object to delete.</param>
    /// <returns>Returns a boolean indicating if the Media got deleted.</returns>
    private static bool DeleteMedia<T>(T media)
    {
        return false;
    }
}