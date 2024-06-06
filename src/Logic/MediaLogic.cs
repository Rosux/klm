public class MediaLogic
{
    public static List<Season> TempSeasonList = new List<Season>();
    public static List<Episode> TempEpisodeList = new List<Episode>();
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
                {"Title", new(x=>x.Title, GetValidTitle)},
                // {"Runtime (Min)", new(x=>x.Runtime, GetValidRuntime)},
                {"Description", new(x=>x.Description, GetValidDescription)},

            },
            SaveEditedMedia,
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

    public static void SeasonsTable(Serie serie){
        var allMedia = MediaAccess.GetAllMedia();
        var series = allMedia.OfType<Serie>().ToList();
        var seasons = new List<Season>();
        MenuHelper.Table<Season>(
            TempSeasonList,
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
            TempSaveTest,
            true,
            () => MediaMenu.AddSeason(serie),
            true,
            DeleteMedia
        );
    }

    public static void EpisodesTable(Season season){
        var episodes = season.Episodes.ToList();
        MenuHelper.Table<Episode>(
            TempEpisodeList,
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
        if(media is Film film){
            bool confirmation = MenuHelper.Confirm($"Are you sure you want to save the following edited data:\n\nTitle: {film.Title}\nRuntime: {film.Runtime}\nDescription: {film.Description.Substring(0, 10) + (film.Description.Length > 10 ? "..." : "")}\nRating: {film.Rating}\nLanguage: {film.Language}\nGenres: {film.Genres.Count}\nReleaseDate: {film.ReleaseDate}\nCertification: {film.Certification}\nDirectors: {string.Join(", ", film.Directors)}\nActors: {string.Join(", ", film.Actors)}\nWriters: {string.Join(", ", film.Writers)}");
            if(confirmation){
                bool success = MediaAccess.EditMedia(film);
                MediaMenu.MediaAdded(success);
                return success;
            }
        }else if(media is Serie serie){
            bool confirmation = MenuHelper.Confirm($"Are you sure you want to save the following edited data:\n\nTitle: {serie.Title}\nRuntime: {serie.Runtime}\nDescription: {serie.Description.Substring(0, 10) + (serie.Description.Length > 10 ? "..." : "")}\nRating: {serie.Rating}\nLanguage: {serie.Language}\nGenres: {serie.Genres.Count}\nReleaseDate: {serie.ReleaseDate}\nCertification: {serie.Certification}\nDirectors: {string.Join(", ", serie.Directors)}");
            if(confirmation){
                bool success = MediaAccess.EditMedia(serie);
                MediaMenu.MediaAdded(success);
                return success;
            }
        }
        return false;
    }


    public static bool TempSaveTest<T>(T media){
        if (media is Season editedSeason)
        {
            bool confirmation = MenuHelper.Confirm($"Are you sure you want to save the following edited data:\n\nTitle: {editedSeason.Title}\nRuntime: {editedSeason.Runtime}\nSeason Number: {editedSeason.SeasonNumber}\nEpisodes: {editedSeason.Episodes.Count}");
            if (confirmation)
            {
                var existingSeason = TempSeasonList.FirstOrDefault(s => s.SeasonNumber == editedSeason.SeasonNumber);
                if (existingSeason != null)
                {
                    TempSeasonList.Remove(existingSeason);
                }
                TempSeasonList.Add(editedSeason);
                return true;
            }
        }
        else if (media is Episode editedEpisode)
        {
            bool confirmation = MenuHelper.Confirm($"Are you sure you want to save the following edited data:\n\nTitle: {editedEpisode.Title}\nRuntime: {editedEpisode.Runtime}\nEpisode Number: {editedEpisode.EpisodeNumber}\nActors: {string.Join(", ", editedEpisode.Actors)}");
            if (confirmation)
            {
                var season = TempSeasonList.FirstOrDefault(s => s.Episodes.Any(e => e.Title == editedEpisode.Title));
                if (season != null)
                {
                    var existingEpisode = season.Episodes.FirstOrDefault(e => e.Title == editedEpisode.Title);
                    if (existingEpisode != null)
                    {
                        season.Episodes.Remove(existingEpisode);
                    }
                    season.Episodes.Add(editedEpisode);
                    return true;
                }
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

    private static string GetValidTitle<T>(T title){
        if(title is Film film){
            string prompt = $"Current Title: {film.Title}\n\n";
            string? newTitle = MenuHelper.SelectText(prompt+"Enter the new Title of the Movie:", "",true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
            return newTitle ?? film.Title;
        }else if (title is Serie serie){
                string prompt = $"Current Title: {serie.Title}\n\n";
                string? newTitle = MenuHelper.SelectText(prompt+"Enter the new Title of the Serie:", "",true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
                return newTitle ?? serie.Title;
        }else{
            return "An error occurred. The provided title is not valid for editing.\n\nPress any key to continue";
        }
    }

    // private static int GetValidRuntime<T>(T runtime){
    //     if(runtime is Film film){
    //         string prompt = $"Current Runtime: {film.Runtime}\n\n";
    //         int? newRuntime = MenuHelper.SelectInteger(prompt + "Enter the new Runtime of the Movie:", "", true, 0, 0, 500);
    //         return newRuntime ?? film.Runtime;
    //     } else {
    //         throw new ArgumentException("Runtime can only be edited for movies.");
    //     }
    // }

    private static string GetValidDescription<T>(T description){
        if(description is Film film){
            string prompt = $"Current Description: {film.Description}\n\n";
            string? newDescription = MenuHelper.SelectText(prompt+"Please enter the description:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
            return newDescription ?? film.Description;
        }else if (description is Serie serie){
                string prompt = $"Current Description: {serie.Description}\n\n";
                string? newDescription = MenuHelper.SelectText(prompt+"Please enter the description:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
                return newDescription ?? serie.Description;
        }else{
            return "An error occurred. The provided description is not valid for editing.\n\nPress any key to continue";
        }
    }
}