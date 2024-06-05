using System.Reflection.Metadata;

public static class MediaMenu
{
    /// <summary>
    /// Creates a new Film object.
    /// </summary>
    /// <returns>Returns a Film object or null in case the user exists the process.</returns>
    public static Film? CreateFilm(){
        string? title = MenuHelper.SelectText("\nType the title of the film:", "", true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
        if (title == null){return null;}
        int? runtime = MenuHelper.SelectInteger("Please enter the runtime in minutes:", "", true, 0, 0, 500);
        if (runtime == null){return null;}
        string? description = MenuHelper.SelectText("Please enter the description:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
        if (description == null){return null;}
        float? rating = (float?)MenuHelper.SelectPrice("Please enter a rating from 1-10:", "", true, 0d, 10d);
        if (rating == null){return null;}
        string? language = MenuHelper.SelectText("Please enter a language:", "", true, 0, 30);
        if (language == null){return null;}
        DateOnly? date = MenuHelper.SelectDate("Please enter the date of the movie:", true);
        if (date == null){return null;}
        string? director = MenuHelper.SelectText("Please enter the directors seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if (director == null){return null;}
        string? actors = MenuHelper.SelectText("Please enter the actors seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if (actors == null){return null;}
        string? writers = MenuHelper.SelectText("Please enter the writers seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if (writers == null){return null;}
        List<Genre> genres = new List<Genre>{
            Genre.HORROR
        };
        Certification certifications = Certification.PG13;
        Film film = new Film(title, (int)runtime, description, (float)rating, language, genres, (DateOnly)date, certifications, director.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList(), actors.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList(), writers.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList());
        return film;
    }

    /// <summary>
    /// Creates a new Serie object.
    /// </summary>
    /// <returns>Returns a Serie object or null in case the user exists the process.</returns>
    public static Serie? CreateSerie(){
        List<Season> seasons = new List<Season>();

        string? title = MenuHelper.SelectText("\nType the title of the serie:", "", true, 1, 100, @"([A-Za-z]|\ |[0-9]|\-)");
        if (title == null){return null;}
        int runtime = 0;
        string? description = MenuHelper.SelectText("Please enter the description:", "", true, 10, 500, @"([A-Za-z]|\ |[0-9]|\-)");
        if (description == null){return null;}
        float? rating = (float?)MenuHelper.SelectPrice("Please enter a rating from 1-10:", "", true, 0d, 10d);
        if (rating == null){return null;}
        string? language = MenuHelper.SelectText("Please enter a language:", "", true, 0, 30);
        if (language == null){return null;}
        DateOnly? date = MenuHelper.SelectDate("Please enter the date of the movie:", true);
        if (date == null){return null;}
        string? director = MenuHelper.SelectText("Please enter the directors seperated by comma:", "", true, 0, 500, "([a-zA-Z]|\\ |\\,)");
        if (director == null){return null;}

        List<Genre> genres = new List<Genre>{
            Genre.HORROR
        };
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
        Serie serie = new Serie(title, runtime, description, (float)rating, language, genres, (DateOnly)date, certifications, director.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries).ToList(), bingeable, seasons);
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