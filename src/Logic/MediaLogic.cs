public class MediaLogic
{
    public static List<Season> TempSeasonList = new List<Season>();
    public static List<Episode> TempEpisodeList = new List<Episode>();
    public static void Media(){
        MenuHelper.Table<Media, Film, Serie>(
            MediaAccess.GetAllMedia(),
            new Dictionary<string, Func<Media, object>>(){
                {"Type", m=>(m is Film) ? "Film" : "Serie"},
                {"Title", m=>m.Title},
                {"Runtime (Min)", m=>m.Runtime},
                {"Description", m=>m.Description.Substring(0, 10) + (m.Description.Length > 10 ? "..." : "")},
                {"Rating", m=>m.Rating},
                {"Language", m=>m.Language},
                {"Release Date", m=>m.ReleaseDate},
                {"Certification", m=>m.Certification},
                {"Directors", m=>ListToString(m.Directors)},
            },
            false, // canSelect
            true,
            true,
            true, // canSearch
            (
                new Dictionary<string, PropertyEditMapping<Media>>(){
                    {"Actors", new(m=>ListToString(((Film)m).Actors), m=>((Film)m).Actors, (Media m)=>new List<String>(){"Actorssss", "Actosssssr2"})},
                },
                new Dictionary<string, PropertyEditMapping<Media>>(){
                    {"Bingeable", new(m=>((Serie)m).Bingeable, (Media m)=>true)},
                }
            ),
            SaveEditedMedia,
            true,
            AddMedia,
            true,
            DeleteMedia
        );
    }

    /// <summary>
    /// Converts a list to a string. Formatted like this: "[item1, item2, +32]".
    /// </summary>
    /// <param name="items">The list to convert.</param>
    /// <typeparam name="T">The type of the list.</typeparam>
    /// <returns>A string containing the list in string format for a table.</returns>
    private static string ListToString<T>(List<T> items)
    {
        if(items.Count == 0){return "None";}
        if(items.Count == 1){return $"[{items[0]}]";}
        if(items.Count == 2){return $"[{items[0]}, {items[1]}]";}
        return $"[{items[0]}, {items[1]}, +{items.Count-2}]";
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
            string? newTitle = MenuHelper.SelectText(prompt+"Enter the new title of the movie:", "",true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
            return newTitle ?? film.Title;
        }else if(title is Serie serie){
                string prompt = $"Current Title: {serie.Title}\n\n";
                string? newTitle = MenuHelper.SelectText(prompt+"Enter the new title of the serie:", "",true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
                return newTitle ?? serie.Title;
        }else{
            return "An error occurred. The provided title is not valid for editing.\n\nPress any key to continue";
        }
    }

    private static object GetValidRuntime<T>(T runtime){
        if(runtime is Film film){
            string prompt = $"Current Runtime: {film.Runtime}\n\n";
            int? newRuntime = MenuHelper.SelectInteger(prompt + "Enter the new runtime of the movie:", "", true, 0, 0, 500);
            return newRuntime ?? film.Runtime;
        } else {
            return 0;
        }
    }

    private static string GetValidDescription<T>(T description){
        if(description is Film film){
            string prompt = $"Current Description: {film.Description}\n\n";
            string? newDescription = MenuHelper.SelectText(prompt+"Please enter the new description of the movie:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
            return newDescription ?? film.Description;
        }else if(description is Serie serie){
                string prompt = $"Current Description: {serie.Description}\n\n";
                string? newDescription = MenuHelper.SelectText(prompt+"Please enter the new description of the serie:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
                return newDescription ?? serie.Description;
        }else{
            return "An error occurred. The provided description is not valid for editing.\n\nPress any key to continue";
        }
    }

    private static object GetValidRating<T>(T rating){
        if(rating is Film film){
            string prompt = $"Current Rating: {film.Rating}\n\n";
            float? newRating = (float?)MenuHelper.SelectPrice(prompt+"Please enter ther new rating of the movie from 1-10::", "", true, 0d, 10d);
            return newRating ?? film.Rating;
        } else {
            return 0;
        }
    }

    private static string GetValidLanguage<T>(T language){
        if(language is Film film){
            string prompt = $"Current Language: {film.Language}\n\n";
            string? newLanguage = MenuHelper.SelectText(prompt+"Please enter the new language of the movie:", "", true, 0, 30);
            return newLanguage ?? film.Language;
        }else if (language is Serie serie){
                string prompt = $"Current Language: {serie.Language}\n\n";
                string? newLanguage = MenuHelper.SelectText(prompt+"Please enter the new language of the serie:", "", true, 0, 30);
                return newLanguage ?? serie.Language;
        }else{
            return "An error occurred. The provided language is not valid for editing.\n\nPress any key to continue";
        }
    }

    private static object GetValidReleaseDate<T>(T releasedate){
        if(releasedate is Film film){
            string prompt = $"Current Release Date: {film.ReleaseDate}\n\n";
            DateOnly? newDate = MenuHelper.SelectDate(prompt+"Please enter the new release date of the movie:", true);
            return newDate ?? film.ReleaseDate;
        }else if(releasedate is Serie serie){
            string prompt = $"Current Release Date: {serie.ReleaseDate}\n\n";
            DateOnly? newDate = MenuHelper.SelectDate(prompt+"Please enter the new release date of the serie:", true);
            return newDate ?? serie.ReleaseDate;
        }else{
            return new DateOnly();
        }
    }

    private static object GetValidCertification<T>(T certification){
        Certification? newCertification = MenuHelper.SelectFromList(
            "Select a certification",
            true,
            new Dictionary<string, Certification>(){
                {"No certification specified.", Certification.NONE},
                {"General audiences. All ages admitted.", Certification.G},
                {"Parental guidance suggested. Some material may not be suitable for children.", Certification.PG},
                {"Parents strongly cautioned. Some material may be inappropriate for children under 13.", Certification.PG13},
                {"Suitable for viewers over 18 years old.", Certification.PG18},
                {"Restricted. Restricted to viewers over 17 years old unless accompanied by an adult.", Certification.R},
                {"No one 17 and under admitted.", Certification.NC17},
                {"Suitable for all children.", Certification.TVY},
                {"Directed to older children, suitable for ages 7 and up.", Certification.TVY7},
                {"Suitable for all ages.", Certification.TVG},
                {"Parental guidance suggested. May contain material unsuitable for young children.", Certification.TVPG},
                {"Parents strongly cautioned. May be unsuitable for children under 14.", Certification.TV14},
                {"Mature audiences only. Content is intended only for adults.", Certification.TVMA},
                {"Adult content. Viewer discretion advised.", Certification.X}
            }
        );
        if(certification is Film film){
            return newCertification != null ? newCertification : film.Certification;
        }else if(certification is Serie serie){
            return newCertification != null ? newCertification : serie.Certification;
        }else{
            return Certification.NONE;
        }
    }
}