using System.Diagnostics;

public class MediaLogic
{
    private static List<Season> _tempSeasons = new List<Season>();
    private static List<Episode> _tempEpisodes = new List<Episode>();

    /// <summary>
    /// Main method for the media table. handles Films and Series together in one neat table. Whoever made this possible deserves a great reward like at least 200 dollars cash. Send it to my btc address: 1F1tAaz5x1HUXrCNLbtMDqcw6o5GNn4xqX. (totally me btw)
    /// </summary>
    public static void Media(){
        MenuHelper.TableUtility.Table<Media, Film, Serie>(
            MediaAccess.GetAllMedia(),
            new Dictionary<string, Func<Media, object>>(){
                {"Type", m=>(m is Film) ? "Film" : "Serie"},
                {"Title", m=>m.Title},
                {"Runtime (Min)", m=>m.Runtime},
                {"Description", m=>GetDescription(m.Description)},
                {"Rating", m=>m.Rating},
                {"Language", m=>m.Language},
                {"Release Date", m=>m.ReleaseDate},
                {"Certification", m=>m.Certification},
                {"Directors", m=>m.Directors.Count},
            },
            false, // canSelect
            true,
            true,
            true, // canSearch
            (
                new Dictionary<string, PropertyEditMapping<Media>>(){
                    {"Title", new(x=>x.Title, GetValidTitle)},
                    {"Runtime (Min)", new(x=>x.Runtime, GetValidInteger)},
                    {"Description", new(x=>GetDescription(x.Description), x=>x.Description, GetValidDescription)},
                    {"Rating", new(x=>x.Rating, GetValidRating)},
                    {"Language", new(x=>x.Language, GetValidLanguage)},
                    {"Genres", new(x=>ListToString(((Film)x).Genres), x=>((Film)x).Genres, GetValidGenres)},
                    {"Release Date", new(x=>x.ReleaseDate, GetValidReleaseDate)},
                    {"Certification", new(x=>x.Certification, GetValidCertification)},
                    {"Directors", new(x=>ListToString(((Film)x).Directors), x=>((Film)x).Directors , GetValidDirectors)},
                    {"Actors", new(x=>ListToString(((Film)x).Actors), x=>((Film)x).Actors, GetValidActors)},
                    {"Writers", new(x=>ListToString(((Film)x).Writers), x=>((Film)x).Writers, GetValidWriters)},
                },
                new Dictionary<string, PropertyEditMapping<Media>>(){
                    {"Title", new(x=>x.Title, GetValidTitle)},
                    {"Runtime (Min)", new(x=>x.Runtime, GetValidInteger)},
                    {"Description", new(x=>GetDescription(x.Description), x=>x.Description, GetValidDescription)},
                    {"Language", new(x=>x.Language, GetValidLanguage)},
                    {"Genres", new(x=>ListToString(((Serie)x).Genres), x=>((Serie)x).Genres, GetValidGenres)},
                    {"Release Date", new(x=>x.ReleaseDate, GetValidReleaseDate)},
                    {"Certification", new(x=>x.Certification, GetValidCertification)},
                    {"Directors", new(x=>ListToString(((Serie)x).Directors), x=>((Serie)x).Directors , GetValidDirectors)},
                    {"Seasons", new(m=>ListToString(((Serie)m).Seasons), m=>((Serie)m).Seasons, (Media m)=>EditSeasons((Serie)m))},
                }
            ),
            SaveEditedMedia,
            true,
            AddMedia,
            true,
            DeleteMedia
        );
    }

    private static string GetDescription(string description)
    {
        if(description == ""){
            return "No Description.";
        }
        return description.Substring(0, Math.Max(0, 10));
    }

    /// <summary>
    /// Converts a list to a string. Formatted like this: "[item1, item2, +32]".
    /// </summary>
    /// <param name="items">The list to convert.</param>
    /// <typeparam name="T">The type of the list.</typeparam>
    /// <returns>A string containing the list in string format for a table.</returns>
    private static string ListToString<T>(List<T> items)
    {
        return $"{items.Count}";
    }

    /// <summary>
    /// Asks the user to select either Film or Series and create a new Film/Serie.
    /// </summary>
    /// <returns>A Media object containing a Film/Serie or null.</returns>
    private static Media? AddMedia()
    {
        // ask user if they want to add a film or serie
        string? userSelection = MenuHelper.ListUtility.SelectFromList(
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
            Serie? serie = MediaLogic.CreateSerie();
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
    private static bool SaveEditedMedia<T>(T media)
    {
        if(media is Film film){
            bool confirmation = MenuHelper.ConfirmationUtility.Confirm($"Are you sure you want to save the following edited data:\n\nId: {film.Id}\nTitle: {film.Title}\nRuntime: {film.Runtime}\nDescription: {GetDescription(film.Description)}\nRating: {film.Rating}\nLanguage: {film.Language}\nGenres: {ListToString(film.Genres)}\nReleaseDate: {film.ReleaseDate}\nCertification: {film.Certification}\nDirectors: {ListToString(film.Directors)}\nActors: {ListToString(film.Actors)}\nWriters: {ListToString(film.Writers)}");
            if(confirmation){
                bool success = MediaAccess.EditMedia(film);
                MediaMenu.MediaAdded(success);
                return success;
            }
        }else if(media is Serie serie){
            bool confirmation = MenuHelper.ConfirmationUtility.Confirm($"Are you sure you want to save the following edited data:\n\nId: {serie.Id}\nTitle: {serie.Title}\nRuntime: {serie.Runtime}\nDescription: {GetDescription(serie.Description)}\nRating: {serie.Rating}\nLanguage: {serie.Language}\nGenres: {ListToString(serie.Genres)}\nReleaseDate: {serie.ReleaseDate}\nCertification: {serie.Certification}\nDirectors: {ListToString(serie.Directors)}\nSeasons: {serie.Seasons.Count}");
            if(confirmation){
                bool success = MediaAccess.EditMedia(serie);
                MediaMenu.MediaAdded(success);
                return success;
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
        bool confirmation = false;
        if(media is Film film){
            confirmation = MenuHelper.ConfirmationUtility.Confirm($"Are you sure you want to delete the following film:\nId: {film.Id}\nTitle: {film.Title}\nRuntime: {film.Runtime}\nDescription: {GetDescription(film.Description)}\nRating: {film.Rating}\nLanguage: {film.Language}\nGenres: {ListToString(film.Genres)}\nReleaseDate: {film.ReleaseDate}\nCertification: {film.Certification}\nDirectors: {ListToString(film.Directors)}\nActors: {ListToString(film.Actors)}\nWriters: {ListToString(film.Writers)}\n");
            if(confirmation){
                bool success = MediaAccess.DeleteMedia(film);
                MediaMenu.MediaDeleted(success);
                return success;
            }else{
                return false;
            }
        }else if(media is Serie serie){
            confirmation = MenuHelper.ConfirmationUtility.Confirm($"Are you sure you want to delete the following serie:\nId: {serie.Id}\nTitle: {serie.Title}\nRuntime: {serie.Runtime}\nDescription: {GetDescription(serie.Description)}\nRating: {serie.Rating}\nLanguage: {serie.Language}\nGenres: {ListToString(serie.Genres)}\nReleaseDate: {serie.ReleaseDate}\nCertification: {serie.Certification}\nDirectors: {ListToString(serie.Directors)}\nSeasons: {serie.Seasons.Count}\n");
            if(confirmation){
                bool success = MediaAccess.DeleteMedia(serie);
                MediaMenu.MediaDeleted(success);
                return success;
            }else{
                return false;
            }
        }
        return confirmation;
    }

    /// <summary>
    /// Gets a valid title for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film, Serie, Season, or Episode).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>A valid title for the media.</returns>
    private static string GetValidTitle<T>(T media){
        if(media is Film film){
            string prompt = $"Current Title: {film.Title}\n\n";
            string? newTitle = MenuHelper.StringUtility.SelectText(prompt+"Enter the new title of the movie:", "",true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
            return newTitle ?? film.Title;
        }else if(media is Serie serie){
                string prompt = $"Current Title: {serie.Title}\n\n";
                string? newTitle = MenuHelper.StringUtility.SelectText(prompt+"Enter the new title of the serie:", "",true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
                return newTitle ?? serie.Title;
        }else if(media is Season season){
            string prompt = $"Current Title: {season.Title}\n\n";
            string? newTitle = MenuHelper.StringUtility.SelectText(prompt+"Enter the new title of the season:", "",true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
            return newTitle ?? season.Title;
        }else if(media is Episode episode){
            string prompt = $"Current Title: {episode.Title}\n\n";
            string? newTitle = MenuHelper.StringUtility.SelectText(prompt+"Enter the new title of the episode:", "",true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
            return newTitle ?? episode.Title;
        }else{
            return "An error occurred. The provided title is not valid for editing.\n\nPress any key to continue";
        }
    }

    /// <summary>
    /// Gets a valid integer value for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film, Serie, Season, or Episode).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>A valid integer value for the media.</returns>
    private static object GetValidInteger<T>(T media){
        if(media is Film film){
            string prompt = $"Current Runtime: {film.Runtime}\n\n";
            int? newRuntime = MenuHelper.IntegerUtility.SelectInteger(prompt + "Enter the new runtime of the movie:", "", true, 0, 0, 500);
            return newRuntime ?? film.Runtime;
        }else if(media is Season season){
            string prompt = $"Current Season Number: {season.SeasonNumber}\n\n";
            int? newSeasonNumber = MenuHelper.IntegerUtility.SelectInteger(prompt+"Please enter the new season number:", "", true, 0, 0, 60);
            return newSeasonNumber ?? season.SeasonNumber;
        }else if(media is Episode episode){
            string prompt = $"Current Episode Number: {episode.EpisodeNumber}\n\n";
            int? newEpisodeNumber = MenuHelper.IntegerUtility.SelectInteger(prompt+"Please enter the new episode number:", "", true, 0, 0, 60);
            return newEpisodeNumber ?? episode.EpisodeNumber;
        }else{
            return 0;
        }
    }

    /// <summary>
    /// Gets a valid runtime for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Episode).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>A valid runtime for the media.</returns>
    private static object GetValidRuntime<T>(T media){
        if(media is Episode episode){
            string prompt = $"Current Runtime: {episode.Runtime}\n\n";
            int? newRuntime = MenuHelper.IntegerUtility.SelectInteger(prompt+"Please enter the runtime in minutes:", "", true, 0, 0, 500);
            return newRuntime ?? episode.Runtime;
        }else{
            return 0;
        }
    }

    /// <summary>
    /// Gets a valid description for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film or Serie).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>A valid description for the media.</returns>
    private static string GetValidDescription<T>(T media){
        if(media is Film film){
            string prompt = $"Current Description: {film.Description}\n\n";
            string? newDescription = MenuHelper.StringUtility.SelectText(prompt+"Please enter the new description of the movie:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
            return newDescription ?? film.Description;
        }else if(media is Serie serie){
                string prompt = $"Current Description: {serie.Description}\n\n";
                string? newDescription = MenuHelper.StringUtility.SelectText(prompt+"Please enter the new description of the serie:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
                return newDescription ?? serie.Description;
        }else{
            return "An error occurred. The provided description is not valid for editing.\n\nPress any key to continue";
        }
    }

    /// <summary>
    /// Gets a valid rating for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film or Episode).</typeparam>
    /// <param name="rating">The media object containing the rating.</param>
    /// <returns>A valid rating for the media.</returns>
    private static object GetValidRating<T>(T rating){
        if(rating is Film film){
            string prompt = $"Current Rating: {film.Rating}\n\n";
            float? newRating = (float?)MenuHelper.PriceUtility.SelectPrice(prompt+"Please enter the new rating of the movie from 0.0-10.0:", "", true, 0d, 10d);
            return newRating ?? film.Rating;
        }else if(rating is Episode episode){
            string prompt = $"Current Rating: {episode.Rating}\n\n";
            float? newRating = (float?)MenuHelper.PriceUtility.SelectPrice(prompt+"Please enter the new rating of the episode from 0.0-10.0:", "", true, 0d, 10d);
            return newRating ?? episode.Rating;
        }else {
            return 0;
        }
    }

    /// <summary>
    /// Gets a valid language for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film or Serie).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>A valid language for the media.</returns>
    private static string GetValidLanguage<T>(T media){
        if(media is Film film){
            string prompt = $"Current Language: {film.Language}\n\n";
            string? newLanguage = MenuHelper.StringUtility.SelectText(prompt+"Please enter the new language of the movie:", "", true, 0, 30);
            return newLanguage ?? film.Language;
        }else if (media is Serie serie){
                string prompt = $"Current Language: {serie.Language}\n\n";
                string? newLanguage = MenuHelper.StringUtility.SelectText(prompt+"Please enter the new language of the serie:", "", true, 0, 30);
                return newLanguage ?? serie.Language;
        }else{
            return "An error occurred. The provided language is not valid for editing.\n\nPress any key to continue";
        }
    }

    /// <summary>
    /// Gets valid genres for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film or Serie).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>Valid genres for the media.</returns>
    private static object GetValidGenres<T>(T media){
        List<Genre> possibleGenres = new List<Genre> {
            Genre.HORROR, Genre.ACTION, Genre.COMEDY, Genre.FAMILY, Genre.DRAMA,
            Genre.ADVENTURE, Genre.FANTASY, Genre.THRILLER, Genre.MYSTERY, Genre.CRIME
        };

        List<Genre>? newGenres = MenuHelper.EnumUtility.SelectFromEnum<Genre>(
            possibleGenres, "Genres", "Select one or multiple genres", "", true
        );

        if (media is Film film) {
            return newGenres != null ? newGenres : film.Genres;
        } else if (media is Serie serie) {
            return newGenres != null ? newGenres : serie.Genres;
        } else {
            return null;
        }
    }

    /// <summary>
    /// Gets a valid release date for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film or Serie).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>A valid release date for the media.</returns>
    private static object GetValidReleaseDate<T>(T media){
        if(media is Film film){
            string prompt = $"Current Release Date: {film.ReleaseDate}\n\n";
            DateOnly? newDate = MenuHelper.DateUtility.SelectDate(prompt+"Please enter the new release date of the movie:", true);
            return newDate ?? film.ReleaseDate;
        }else if(media is Serie serie){
            string prompt = $"Current Release Date: {serie.ReleaseDate}\n\n";
            DateOnly? newDate = MenuHelper.DateUtility.SelectDate(prompt+"Please enter the new release date of the serie:", true);
            return newDate ?? serie.ReleaseDate;
        }else{
            return new DateOnly();
        }
    }

    /// <summary>
    /// Gets a valid certification for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film or Serie).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>A valid certification for the media.</returns>
    private static object GetValidCertification<T>(T media){
        Certification? newCertification = MenuHelper.ListUtility.SelectFromList(
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
        if(media is Film film){
            return newCertification != null ? newCertification : film.Certification;
        }else if(media is Serie serie){
            return newCertification != null ? newCertification : serie.Certification;
        }else{
            return Certification.NONE;
        }
    }

    /// <summary>
    /// Gets valid directors for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film or Serie).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>Valid directors for the media.</returns>
    private static List<string> GetValidDirectors(Media media){
        if(media is Film film){
            string prompt = $"Current Directors: {ListToString(film.Directors)}\n\n";
            string? newDirectors = MenuHelper.StringUtility.SelectText(prompt+"Please enter the new directors of the movie seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
            return newDirectors != null ? newDirectors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList() : film.Directors;
        }else if(media is Serie serie){
            string prompt = $"Current Directors: {ListToString(serie.Directors)}\n\n";
            string? newDirectors = MenuHelper.StringUtility.SelectText(prompt+"Please enter the new directors of the serie seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
            return newDirectors != null ? newDirectors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList() : serie.Directors;
        }else{
            return media.Directors;
        }
    }

    /// <summary>
    /// Gets valid actors for the media.
    /// </summary>
    /// <typeparam name="T">The type of media (Film or Episode).</typeparam>
    /// <param name="media">The media object.</param>
    /// <returns>Valid actors for the media.</returns>
    private static List<string> GetValidActors<T>(T media){
        if(media is Film film){
            string prompt = $"Current Actors: {ListToString(film.Actors)}\n\n";
            string? newActors = MenuHelper.StringUtility.SelectText(prompt+"Please enter the new actors of the movie seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
            return newActors != null ? newActors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList() : film.Actors;
        }else if(media is Episode episode){
            string prompt = $"Current Actors: {ListToString(episode.Actors)}\n\n";
            string? newActors = MenuHelper.StringUtility.SelectText(prompt+"Please enter the actors of the episode seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
            return newActors != null ? newActors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList() : episode.Actors;
        }else{
            return null;
        }
    }

    /// <summary>
    /// Asks the user to create a new list of writers.
    /// </summary>
    /// <param name="media">The default object (Film).</param>
    /// <typeparam name="T">Can only be Film.</typeparam>
    /// <returns>A new list of directors or the previous version.</returns>
    private static object GetValidWriters<T>(T media){
        if(media is Film film){
            string prompt = $"Current Writers: {ListToString(film.Writers)}\n\n";
            string? newWriters = MenuHelper.StringUtility.SelectText(prompt+"Please enter the new actors of the movie seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
            return newWriters != null ? newWriters.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList() : film.Writers;
        }else{
            return null;
        }
    }

    /// <summary>
    /// Shows the user a table to edit seasons of a serie.
    /// </summary>
    /// <param name="serie">The serie to show the user.</param>
    /// <returns>A list of edited seasons.</returns>
    private static List<Season> EditSeasons(Serie serie)
    {
        MenuHelper.TableUtility.Table<Season>(
            serie.Seasons,
            new Dictionary<string, Func<Season, object>>(){
                {"Title", m=>m.Title},
                {"Season Number", m=>m.SeasonNumber},
                {"Runtime", m=>m.Runtime},
                {"Rating", m=>m.Rating},
                {"Episodes", m=>ListToString(m.Episodes)},
            },
            false, // canSelect
            true,
            true,
            true, // canSearch
            new Dictionary<string, PropertyEditMapping<Season>>(){
                {"Title", new(x => x.Title, GetValidTitle)}, // TODO make Title
                {"Season Number", new(x => x.SeasonNumber, GetValidInteger)}, // TODO make SeasonNumber
                {"Episode", new(x=>ListToString(x.Episodes), x=>x.Episodes, s=>EditEpisodes(s.Episodes))},
            },
            (Season season)=>true,
            true,
            CreateSeason,
            true,
            DeleteTempSeason
        );
        return serie.Seasons;
    }

    /// <summary>
    /// Shows the user a table to edit episodes of a season.
    /// </summary>
    /// <param name="episodes">The episodes to edit.</param>
    /// <returns>A list of edited episodes.</returns>
    private static List<Episode> EditEpisodes(List<Episode> episodes)
    {
        MenuHelper.TableUtility.Table<Episode>(
            episodes,
            new Dictionary<string, Func<Episode, object>>(){
                {"Title", m=>m.Title},
                {"Runtime", m=>m.Runtime},
                {"Episode Number", m=>m.EpisodeNumber},
                {"Rating", m=>((Episode)m).Rating},
                {"Actors", m=>ListToString(m.Actors)},
            },
            false,
            true,
            true,
            true,
            new Dictionary<string, PropertyEditMapping<Episode>>(){
                {"Title", new(x => x.Title, GetValidTitle)}, // TODO make Title
                {"Runtime", new(x => x.Runtime, GetValidRuntime)}, // TODO make Runtime
                {"Episode Number", new(x => x.EpisodeNumber, GetValidInteger)},
                {"Rating", new(m=>((Episode)m).Rating, GetValidRating)},
                {"Actors", new(x=>ListToString(((Episode)x).Actors), x=>((Episode)x).Actors, GetValidActors)}, // TODO make Actors
            },
            (Episode episode)=>true,
            true,
            CreateEpisode,
            true,
            DeleteTempEpisode
        );
        return episodes;
    }

    /// <summary>
    /// Asks the user to create a new Serie and lets them create/remove/edit new seasons and episodes.
    /// </summary>
    /// <returns>The created Serie or NULL in case the user exited.</returns>
    private static Serie? CreateSerie()
    {
        Serie? currentSerie = MediaMenu.CreateSerie();
        if(currentSerie == null){
            return null;
        }

        MenuHelper.TableUtility.Table<Season>(
            _tempSeasons,
            new Dictionary<string, Func<Season, object>>(){
                {"Title", m=>m.Title},
                {"Season Number", m=>m.SeasonNumber},
                {"Runtime", m=>m.Runtime},
                {"Rating", m=>m.Rating},
                {"Episodes", m=>ListToString(m.Episodes)},
            },
            false, // canSelect
            true,
            true,
            true, // canSearch
            new Dictionary<string, PropertyEditMapping<Season>>(){
                {"Title", new(x=>x.Title, GetValidTitle)},
                {"Season Number", new(x => x.SeasonNumber, GetValidInteger)},
                {"Episode", new(x=>ListToString(x.Episodes), x=>x.Episodes, CreateEpisode)},
            },
            (Season season)=>true,
            true,
            CreateSeason,
            true,
            DeleteTempSeason
        );

        List<Season> seasonsClone = new List<Season>();
        foreach(Season season in _tempSeasons){
            seasonsClone.Add(season);
        }
        _tempSeasons = new List<Season>();
        currentSerie.Seasons = seasonsClone;
        return currentSerie;
    }

    /// <summary>
    /// Shows the user an empty table they can fill up and returns the list of episodes.
    /// </summary>
    /// <param name="season">A season (can be empty).</param>
    /// <returns>A list of episodes made by the user.</returns>
    private static List<Episode> CreateEpisode(Season season)
    {
        MenuHelper.TableUtility.Table<Episode>(
            _tempEpisodes,
            new Dictionary<string, Func<Episode, object>>(){
                {"Title", m=>m.Title},
                {"Runtime", m=>m.Runtime},
                {"Episode Number", m=>m.EpisodeNumber},
                {"Rating", m=>((Episode)m).Rating},
                {"Actors", m=>ListToString(m.Actors)},
            },
            false,
            true,
            true,
            true,
            new Dictionary<string, PropertyEditMapping<Episode>>(){
                {"Title", new(x => x.Title, GetValidTitle)}, // TODO make Title
                {"Runtime", new(x => x.Runtime, GetValidRuntime)}, // TODO make Runtime
                {"Episode Number", new(x => x.EpisodeNumber, GetValidInteger)}, // TODO make EpisodeNumber\
                {"Rating", new(m=>((Episode)m).Rating, GetValidRating)},
                {"Actors", new(x=>ListToString(((Episode)x).Actors), x=>((Episode)x).Actors, GetValidActors)}, // TODO make Actors
            },
            (Episode episode)=>true,
            true,
            CreateEpisode,
            true,
            DeleteTempEpisode
        );
        List<Episode> episodesClone = new List<Episode>();
        foreach(Episode episode in _tempEpisodes){
            episodesClone.Add(episode);
        }
        _tempEpisodes = new List<Episode>();
        return episodesClone;
    }

    /// <summary>
    /// Creates a new episode.
    /// </summary>
    /// <returns>The created episode or NULL if the user stops.</returns>
    private static Episode? CreateEpisode()
    {
        return MediaMenu.CreateEpisode();
    }

    /// <summary>
    /// Remove an episode from the temporary Season episode lists.
    /// </summary>
    /// <param name="episode">The Episode to delete.</param>
    /// <returns>Always returns true.</returns>
    private static bool DeleteTempEpisode(Episode episode)
    {
        _tempEpisodes.Remove(episode);
        return true;
    }

    /// <summary>
    /// Creates a new Season.
    /// </summary>
    /// <returns>The new Season data. Or NULL if the user stopped.</returns>
    private static Season? CreateSeason()
    {
        return MediaMenu.CreateSeason();
    }

    /// <summary>
    /// Removes the given Season from the temporary seasons.
    /// </summary>
    /// <param name="season">Season to remove</param>
    /// <returns>Always returns true.</returns>
    private static bool DeleteTempSeason(Season season)
    {
        _tempSeasons.Remove(season);
        return true;
    }
}