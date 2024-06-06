using System.Reflection.Metadata;

public static class MediaMenu
{
    /// <summary>
    /// Creates a new Film object.
    /// </summary>
    /// <returns>Returns a Film object or null in case the user exists the process.</returns>
    public static Film? CreateFilm(){
        string? title = MenuHelper.SelectText($"Title: \nRuntime: \nDescription: \nRating: \nLanguage: \nRelease Date: \nGenres: \nCertification: \nDirectors: \nActors: \nWriters: \n\nType the title of the film:", "", true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
        if (title == null){return null;}

        int? runtime = MenuHelper.SelectInteger($"Title: {title}\nRuntime: \nDescription: \nRating: \nLanguage: \nRelease Date: \nGenres: \nCertification: \nDirectors: \nActors: \nWriters: \n\nPlease enter the runtime in minutes:", "", true, 0, 0, 500);
        if (runtime == null){return null;}

        string? description = MenuHelper.SelectText($"Title: {title}\nRuntime: {runtime}\nDescription: \nRating: \nLanguage: \nRelease Date: \nGenres: \nCertification: \nDirectors: \nActors: \nWriters: \n\nPlease enter the description:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
        if (description == null){return null;}

        float? rating = (float?)MenuHelper.SelectPrice($"Title: {title}\nRuntime: {runtime}\nDescription: {description.Substring(0, 10) + (description.Length > 10 ? "..." : "")}\nRating: \nLanguage: \nRelease Date: \nGenres: \nCertification: \nDirectors: \nActors: \nWriters: \n\nPlease enter a rating from 1-10:", "", true, 0d, 10d);
        if (rating == null){return null;}

        string? language = MenuHelper.SelectText($"Title: {title}\nRuntime: {runtime}\nDescription: {description.Substring(0, 10) + (description.Length > 10 ? "..." : "")}\nRating: {rating}\nLanguage: \nRelease Date: \nGenres: \nCertification: \nDirectors: \nActors: \nWriters: \n\nPlease enter a language:", "", true, 0, 30);
        if (language == null){return null;}

        DateOnly? releaseDate = MenuHelper.SelectDate($"Title: {title}\nRuntime: {runtime}\nDescription: {description.Substring(0, 10) + (description.Length > 10 ? "..." : "")}\nRating: {rating}\nLanguage: {language}\nRelease Date: \nGenres: \nCertification: \nDirectors: \nActors: \nWriters: \n\nPlease enter the date of the movie:", true);
        if (releaseDate == null){return null;}

        List<Genre> genres = new List<Genre>{ Genre.HORROR, Genre.ACTION, Genre.COMEDY, Genre.FAMILY, Genre.DRAMA, Genre.ADVENTURE, Genre.FANTASY, Genre.THRILLER, Genre.MYSTERY, Genre.CRIME };
        List<Genre>? selectedGenres = MenuHelper.SelectFromEnum<Genre>(genres, "Genres", $"Title: {title}\nRuntime: {runtime}\nDescription: {description.Substring(0, 10) + (description.Length > 10 ? "..." : "")}\nRating: {rating}\nLanguage: {language}\nRelease Date: {releaseDate}\nGenres: \nCertification: \nDirectors: \nActors: \nWriters: \n\nSelect one or multiple Genres", "", true);
        if(selectedGenres == null){return null;}

        Certification? certification = MenuHelper.SelectFromList(
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

        string? directors = MenuHelper.SelectText($"Title: {title}\nRuntime: {runtime}\nDescription: {description.Substring(0, 10) + (description.Length > 10 ? "..." : "")}\nRating: {rating}\nLanguage: {language}\nRelease Date: {releaseDate}\nGenres: {genres.Count}\nCertification: {certification}\nDirectors: \nActors: \nWriters: \n\nPlease enter the directors seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if (directors == null){return null;}

        string? actors = MenuHelper.SelectText($"Title: {title}\nRuntime: {runtime}\nDescription: {description.Substring(0, 10) + (description.Length > 10 ? "..." : "")}\nRating: {rating}\nLanguage: {language}\nRelease Date: {releaseDate}\nGenres: {genres.Count}\nCertification: {certification}\nDirectors: {directors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList().Count}\nActors: \nWriters: \n\nPlease enter the actors seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if (actors == null){return null;}

        string? writers = MenuHelper.SelectText($"Title: {title}\nRuntime: {runtime}\nDescription: {description.Substring(0, 10) + (description.Length > 10 ? "..." : "")}\nRating: {rating}\nLanguage: {language}\nRelease Date: {releaseDate}\nGenres: {genres.Count}\nCertification: {certification}\nDirectors: {directors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList().Count}\nActors: {actors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList().Count}\nWriters: \n\nPlease enter the writers seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if (writers == null){return null;}

        return new Film(title, (int)runtime, description, (float)rating, language, selectedGenres, (DateOnly)releaseDate, (Certification)certification, directors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList(), actors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList(), writers.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList());
    }

    /// <summary>
    /// Creates a new Serie object.
    /// </summary>
    /// <returns>Returns a Serie object or null in case the user exists the process.</returns>
    public static Serie? CreateSerie(){
        List<Season> seasons = new List<Season>();

        string? title = MenuHelper.SelectText("\nType the title of the serie:", "", true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
        if(title == null){return null;}

        int runtime = 0;

        string? description = MenuHelper.SelectText("Please enter the description:", "", true, 1, 500, @"([A-Za-z]|\ |\.|\,|[0-9]|\-)");
        if(description == null){return null;}

        float rating = 0f;

        string? language = MenuHelper.SelectText("Please enter a language:", "", true, 0, 30);
        if(language == null){return null;}

        DateOnly? date = MenuHelper.SelectDate("Please enter the date of the movie:", true);
        if(date == null){return null;}

        string? director = MenuHelper.SelectText("Please enter the directors seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if(director == null){return null;}


        List<Genre> possibleGenres = new List<Genre>{ Genre.HORROR, Genre.ACTION, Genre.COMEDY, Genre.FAMILY, Genre.DRAMA, Genre.ADVENTURE, Genre.FANTASY, Genre.THRILLER, Genre.MYSTERY, Genre.CRIME };
        List<Genre>? genres = MenuHelper.SelectFromEnum<Genre>(possibleGenres, "Genres", "Select one or multiple genres", "", true);
        if(genres == null){return null;}


        Certification certifications = Certification.PG13;


        bool running = true;
        while(running){
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"Add Season", ()=>{
                    Season? season = CreateSeason();
                    if (season != null)
                    {
                        seasons.Add(season);
                        runtime += season.Runtime;
                    }
                }},
                {"Save", ()=>{
                    running = false;
                }},
                {"Return to menu", ()=>{
                    running = false;
                }},
            });
        }
        bool bingeable = false;
        if (runtime > 400 && rating > 8){
            bingeable = true;
        }
        Serie serie = new Serie(title, runtime, description, rating, language, genres, (DateOnly)date, certifications, director.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList(), bingeable, seasons);
        return serie;
    }

    public static Season? CreateSeason(){
        List<Episode> episodes = new List<Episode>();

        string? title = MenuHelper.SelectText("\nType the title of the season:", "", true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
        if (title == null){return null;}
        int runtime = episodes.Sum(episode => episode.Runtime);
        int? seasonNumber = MenuHelper.SelectInteger("Please enter the seasonnumber:", "", true, 0, 0, 60);
        if (seasonNumber == null){return null;}


        bool running = true;
        while(running){
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"Add Episode", ()=>{
                    Episode? episode = CreateEpisode();
                    if(episode == null){
                        return;
                    }else{
                        episodes.Add(episode);
                        runtime += episode.Runtime;
                    }
                }},
                {"Return to menu", ()=>{
                    running = false;
                }},
            });
        }
        Season season = new Season(title, runtime, (int)seasonNumber, episodes);
        return season;
    }

    public static Episode? CreateEpisode(){
        string? title = MenuHelper.SelectText("\nType the title of the episode:", "", true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
        if (title == null){return null;}
        int? runtime = MenuHelper.SelectInteger("Please enter the runtime in minutes:", "", true, 0, 0, 500);
        if (runtime == null){return null;}
        int? episodeNumber = MenuHelper.SelectInteger("Please enter the episodenumber:", "", true, 0, 0, 60);
        if (episodeNumber == null){return null;}
        string? actors = MenuHelper.SelectText("Please enter the actors seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if (actors == null){return null;}
        Episode episode = new Episode(title, (int)runtime, (int)episodeNumber, actors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList());
        return episode;
    }
}